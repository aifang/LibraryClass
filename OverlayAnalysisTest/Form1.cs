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
            //DoOverLay();
            //FrmGridView frmGridView = new FrmGridView();
            //frmGridView.Table = outTable;
            //frmGridView.Show();
            myDLL.SpatialReferenceHelper.ChangeMapSpatialReference(0, axMapControl1.Map);
        }
        //打开MDB数据
        public void OpenMdb(AxMapControl axMapControl)
        {
            string connectStr=@"D:\鲅鱼圈区\鲅鱼圈区.mdb";
            IWorkspace pWorkspace = myDLL.WorkspaceHelper.GetAccessWorkspace(connectStr);
            if (pWorkspace != null)
            {
                List<IFeatureLayer> pLayers = (myDLL.LayerHelper.getFeatureLayersFromWorkspace(pWorkspace));
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
        
    }
}
