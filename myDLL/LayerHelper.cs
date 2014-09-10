using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;

namespace myDLL
{
    public class LayerHelper
    {


        #region 公有方法

        /// <summary>
        /// 根据名称在地图上查找对应矢量图层
        /// </summary>
        /// <param name="map"></param>
        /// <param name="layerName"></param>
        /// <returns></returns>
        public static IFeatureLayer getFeatureLayerFromMap(IMap map, string layerName)
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

        public static List<IFeatureLayer> getFeatureLayersFromWorkspace(IWorkspace pWorkspace)
        {
            List<IFeatureLayer> piontLayer=new List<IFeatureLayer>();
            List<IFeatureLayer> lineLayer = new List<IFeatureLayer>();
            List<IFeatureLayer> polygonLayer = new List<IFeatureLayer>();
            List<IFeatureLayer> sortLayer = new List<IFeatureLayer>();

            if (pWorkspace != null)
            {
                IEnumDataset enumDataset = pWorkspace.get_Datasets(esriDatasetType.esriDTFeatureDataset);
                enumDataset.Reset();
                IDataset dataset = enumDataset.Next();
                while (dataset != null)
                {
                    IEnumDataset _subset = dataset.Subsets;
                    _subset.Reset();
                    IDataset pSubset = _subset.Next();
                    while (pSubset != null)
                    {
                        IFeatureClass _fc = (IFeatureClass)pSubset;                        
                        IFeatureLayer _layer = new FeatureLayer();
                        _layer.FeatureClass = _fc;
                        //_layer.Name = dataset.Name;
                        _layer.Name = _fc.AliasName;
                        pSubset = _subset.Next();
                    }
                    dataset = enumDataset.Next();
                }
            }
            return null;
        }

        public static string GetClassOwnerName(string dsName)
        {
            return null;
        }

        public static string GetClassShortName(string dsName)
        {
            return null;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 得到地图上图层列表
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        private static IEnumLayer getFeatureLayers(IMap map)
        {
            UID uid = new UIDClass();
            #region uid获取图层类型
            //{6CA416B1-E160-11D2-9F4E-00C04F6BC78E} IDataLayer （all）
            //{40A9E885-5533-11d0-98BE-00805F7CED21} IFeatureLayer
            //{E156D7E5-22AF-11D3-9F99-00C04F6BC78E} IGeoFeatureLayer
            //{34B2EF81-F4AC-11D1-A245-080009B6F22B} IGraphicsLayer
            //{5CEAE408-4C0A-437F-9DB3-054D83919850} IFDOGraphicsLayer
            //{0C22A4C7-DAFD-11D2-9F46-00C04F6BC78E} ICoverageAnnotationLayer
            //{EDAD6644-1810-11D1-86AE-0000F8751720} IGroupLayer
            #endregion
            uid.Value = "{40A9E885-5533-11d0-98BE-00805F7CED21}";
            IEnumLayer layers = map.get_Layers(uid, true);
            return layers;
        }

        #endregion

    }
}
