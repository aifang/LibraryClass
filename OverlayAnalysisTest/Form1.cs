﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Geometry;

using OverlayAnalysis;
using myDLL;

namespace OverlayAnalysisTest
{
    public partial class Form1 : Form
    {
        DataTable outTable = new DataTable();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            #region 重叠分析测试 and getworkspace测试
            //OpenMdb(axMapControl1);
            //DoOverLay();
            //FrmGridView frmGridView = new FrmGridView();
            //frmGridView.Table = outTable;
            //frmGridView.Show();
            //myDLL.SpatialReferenceHelper.ChangeMapSpatialReference(0, axMapControl1.Map);
            #endregion

            #region 创建workspace测试
            //string filepath = @"D:\test";
            //string filename = "test";

            //myDLL.WorkspaceHelper.CreateAccessWorkspace(filepath, filename);
            //myDLL.WorkspaceHelper.CreateFileGDBWorkspace(filepath, filename);
            //myDLL.WorkspaceHelper.CreateShapefileWorkspace(filepath, filename);
            #endregion

            #region   点构建polygon
            //ESRI.ArcGIS.Geometry.IPolygon p= CoordinateToGeometries.GetPolygonGeometry();
            #endregion

            #region 创建featureClass测试
            OpenMdb(axMapControl1);
            #endregion

            #region 删除要素测试
            //FeatureHelper.deleteFeature("HR.Counties1", "objectid_1=59");
            #endregion

            #region 创建空间参考测试
            //ISpatialReference newSRID= SpatialReferenceHelper.createSpatialReference(4610, false, false);
            #endregion

        }
        //打开MDB数据
        public void OpenMdb(AxMapControl axMapControl)
        {
            string connectStr = @"D:\鲅鱼圈区\鲅鱼圈区.mdb";
            IWorkspace pWorkspace = myDLL.WorkspaceHelper.GetAccessWorkspace(connectStr);
            //IWorkspace pWorkspace = myDLL.WorkspaceHelper.GetSDEWorkspace("localhost", "5151", "sde", "sde", "", "SDE.DEFAULT");
            //string connectStr = @"C:\temp\XZQ.shp";
            //IWorkspace pWorkspace = myDLL.WorkspaceHelper.GetShapefileWorkspace(connectStr);
            IFeatureClass pfClass = FeatureClassHelper.CreateFeatureClass(pWorkspace as IWorkspace2, null, "qqq", null, null, null, "", true,esriGeometryType.esriGeometryPolyline);
            if (pWorkspace != null)
            {
                IList<IFeatureLayer> pLayers = (myDLL.LayerHelper.getFeatureLayersFromWorkspace(pWorkspace));
                if (pLayers.Count != 0)
                {
                    foreach (IFeatureLayer item in pLayers)
                        axMapControl1.AddLayer(item);
                }
            }  
        }

        private void DoOverLay()
        {
            outTable = OverlayAnalysis.OverlayAnalysis.OverLayersis("面状重点建设项目", "行政区", axMapControl1);
        }
        
        private void featureExportToShapefile()
        {
            
            IQueryFilter pQFilter = new QueryFilterClass();
            pQFilter.WhereClause = "\"XZQMC\"='123'";
            IFeatureClass pFClass = myDLL.LayerHelper.getFeatureLayerFromMap(axMapControl1.Map, "XZQ").FeatureClass;
            IFeatureCursor pFCursor= pFClass.Search(pQFilter, false);
            IFeature pfeature;
            IFeatureWorkspace pFWS = (pFClass as IDataset).Workspace as IFeatureWorkspace;
            //IFeatureClass newShapeFile = pFWS.CreateFeatureClass("exportfile", pFClass.Fields, null, null, esriFeatureType.esriFTSimple,"FID", null);
            while((pfeature=pFCursor.NextFeature())!=null)
            {
                string txtfile = "";
                IPolygon pPolygon = pfeature.Shape as IPolygon;
                IGeometryCollection pGeometryColl = new PolygonClass();
                pGeometryColl = pPolygon as IGeometryCollection;
                IPointCollection innerPointCollection = new RingClass();
                List<IPointCollection> pPointCollection=new List<IPointCollection>();
                for(int i=0;i<pGeometryColl.GeometryCount;i++)
                {
                    pPointCollection.Add(pGeometryColl.Geometry[i] as IPointCollection);
                }

                IPoint pPoint = new PointClass();
                //foreach(var item in pPointCollection)
                for(int a=0;a<pPointCollection.Count;a++)
                {
                    var item=pPointCollection[a];
                    txtfile += "环" + a + @"\n";
                    for(int i=0;i<item.PointCount;i++)
                    {
                        pPoint = item.Point[i];
                        txtfile += pPoint.X.ToString() + "," + pPoint.Y.ToString() + @"\n";
                    }
                }
                File.WriteAllText(@"c:\temp\坐标.txt",txtfile);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            featureExportToShapefile();
        }
    }
}
