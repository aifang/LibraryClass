using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;

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
            OpenMdb(axMapControl1);
            DoOverLay();
            FrmGridView frmGridView = new FrmGridView();
            frmGridView.Table = outTable;
            frmGridView.Show();
        }
        //打开MDB数据
        public void OpenMdb(AxMapControl axMapControl)
        {
            IWorkspaceFactory pWorkspaceFactory = new AccessWorkspaceFactoryClass();           
            IWorkspace pWorkspace = pWorkspaceFactory.OpenFromFile(@"D:\鲅鱼圈区\鲅鱼圈区.mdb", 0);
            IFeatureWorkspace pFeatureWorkspace = pWorkspace as IFeatureWorkspace;
            if (pWorkspace != null)
            {
                //递归试试，遍历所有
                axMapControl.ClearLayers();
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
                       // string str = myDLL.FeatureClassHelper.getWorkspaceFactoryName(_fc);
                        IFeatureLayer _layer = new FeatureLayer();
                        _layer.FeatureClass = _fc;
                        //_layer.Name = dataset.Name;
                        _layer.Name = _fc.AliasName;
                        axMapControl.AddLayer(_layer);
                        pSubset = _subset.Next();
                    }
                    dataset = enumDataset.Next();
                }
            }

        }

        private void DoOverLay()
        {
            //IWorkspaceFactory pWorkspaceFactory = new AccessWorkspaceFactoryClass();           
            //IWorkspace pWorkspace = pWorkspaceFactory.OpenFromFile(@"D:\鲅鱼圈区\鲅鱼圈区.mdb", 0);
            //IFeatureWorkspace pFeatureWorkspace = pWorkspace as IFeatureWorkspace;

            //IFeatureLayer pFtLayer = new FeatureLayer();
            //IFeatureLayer inputlayer1=new FeatureLayer();
            //inputlayer1.FeatureClass=pFeatureWorkspace.OpenFeatureClass("MZZDJSXM");
            //IFeatureLayer inputlayer2=new FeatureLayer();
            //inputlayer2.FeatureClass=pFeatureWorkspace.OpenFeatureClass("XZQ");
            //pFtLayer.FeatureClass = OverlayAnalysis.OverlayAnalysis.OverLayersis(inputlayer1, inputlayer2, out outTable);
            //pFtLayer.Name = "Intersect_result";
            //axMapControl1.AddLayer(pFtLayer);
            outTable = OverlayAnalysis.OverlayAnalysis.OverLayersis("面状重点建设项目", "行政区", axMapControl1);
        }
        
    }
}
