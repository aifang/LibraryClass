using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;

namespace OverlayAnalysis
{
    /// <summary>
    /// 重叠分析，返回Datatable并在地图上显示结果
    /// </summary>
    public class OverlayAnalysis
    {
        /// <summary>
        /// 叠加分析
        /// </summary>
        /// <param name="_pLayerName">第一图层名</param>
        /// <param name="_pOverLayerName">叠加图层名</param>
        /// <param name="axMapControl">地图控件需要加载用于分析的图层</param>
        /// <returns></returns>
        public static DataTable OverLayersis(string _pLayerName, string _pOverLayerName, AxMapControl axMapControl)
        {
            
            IFeatureClassName pOutPut = new FeatureClassNameClass();
            IFeatureLayer _pLayer = getFeatureLayer(axMapControl.Map, _pLayerName);
            IFeatureLayer _pOverLayer = getFeatureLayer(axMapControl.Map, _pOverLayerName);

            if (_pLayer.FeatureClass == null || _pOverLayer.FeatureClass == null)
            {
                MessageBox.Show("未找到有效图层");
                return null;
            }            
            pOutPut.ShapeType = _pLayer.FeatureClass.ShapeType;
            pOutPut.ShapeFieldName = _pLayer.FeatureClass.ShapeFieldName;
            pOutPut.FeatureType = esriFeatureType.esriFTSimple;
            IWorkspaceName pNewWSName;
            pNewWSName = new WorkspaceNameClass();
            pNewWSName.WorkspaceFactoryProgID =
            "esriDataSourcesFile.ShapefileWorkspaceFactory";
            pNewWSName.PathName = @"c:\temp";
            IDatasetName pDatasetName;
            pDatasetName = pOutPut as IDatasetName;
            pDatasetName.Name = "Intersect_result";
            pDatasetName.WorkspaceName = pNewWSName;

            IBasicGeoprocessor pBasicGeo = new BasicGeoprocessorClass();
            //pBasicGeo.SpatialReference = _pLayer.SpatialReference;            
            IFeatureClass pFeatureClass = pBasicGeo.Intersect(_pLayer.FeatureClass as ITable, false, _pOverLayer.FeatureClass as ITable, false, 0.1, pOutPut);
            DataTable dataTable = DataTableConvert.CreateDataTable(pFeatureClass as ITable);
            IFeatureLayer pFtLayer = new FeatureLayerClass();
            pFtLayer.FeatureClass = AreaFilter(pFeatureClass);
            pFtLayer.Name = "Intersect_result";
            axMapControl.AddLayer(pFtLayer);
            return dataTable;          
            
        }

        #region 获取图层
        /// <summary>
        /// 得到地图上图层列表
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        private static IEnumLayer getFeatureLayers(IMap map)
        {
            UID uid = new UIDClass();
            //{6CA416B1-E160-11D2-9F4E-00C04F6BC78E} IDataLayer （all）
            //{40A9E885-5533-11d0-98BE-00805F7CED21} IFeatureLayer
            //{E156D7E5-22AF-11D3-9F99-00C04F6BC78E} IGeoFeatureLayer
            //{34B2EF81-F4AC-11D1-A245-080009B6F22B} IGraphicsLayer
            //{5CEAE408-4C0A-437F-9DB3-054D83919850} IFDOGraphicsLayer
            //{0C22A4C7-DAFD-11D2-9F46-00C04F6BC78E} ICoverageAnnotationLayer
            //{EDAD6644-1810-11D1-86AE-0000F8751720} IGroupLayer
            uid.Value = "{40A9E885-5533-11d0-98BE-00805F7CED21}";
            IEnumLayer layers = map.get_Layers(uid, true);
            return layers;
        }
        /// <summary>
        /// 根据名称在地图上查找对应矢量图层
        /// </summary>
        /// <param name="map"></param>
        /// <param name="layerName"></param>
        /// <returns></returns>
        private static IFeatureLayer getFeatureLayer(IMap map, string layerName)
        {
            IEnumLayer layers = getFeatureLayers(map);
            layers.Reset();
            ILayer layer = null;
            IFeatureLayer featureLayer = new FeatureLayer();
            while ((layer = layers.Next()) != null)
            {
                if (layer.Name == layerName)
                    featureLayer = layer as IFeatureLayer;
            }
            return featureLayer;
        }
        #endregion

        /// <summary>
        /// 根据面积过滤一些记录，根据要求适当改变代码可实现其他操作
        /// </summary>
        /// <param name="inputFtClass">待过滤要素集</param>
        /// <returns></returns>
        private static IFeatureClass AreaFilter(IFeatureClass inputFtClass)
        {
            IQueryFilter pQueryFilter = new QueryFilterClass();            
            //string whereClause="\"Shape_Area\">=20";
            //pQueryFilter.SubFields = "Shape, NAME, TERM, Pop1996";
            pQueryFilter.WhereClause = "\"Shape_Area\"<9000";
            IFeatureCursor pFtCursor = inputFtClass.Search(pQueryFilter, false);
            IFeature pFeature =null;
            while ((pFeature = pFtCursor.NextFeature())!= null)
            {
                pFeature.Delete();
            }
            return inputFtClass;
        }
    }

    
}
