using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;

namespace myDLL
{
    public class FeatureClassHelper
    {
        /// <summary>
        /// 获取数据FeatueClass的workspaceFactory
        /// </summary>
        /// <param name="pFeatureClass"></param>
        /// <returns></returns>
        public static string getWorkspaceFactoryName(IFeatureClass pFeatureClass)
        {
            string str=((IDataset)pFeatureClass).Workspace.WorkspaceFactory.ToString();
            return str.Substring(str.LastIndexOf('.') + 1);
        }
        public static string getWorkspaceFactoryName(IDataset pDataset)
        {
            string str = pDataset.Workspace.WorkspaceFactory.ToString();
            return str.Substring(str.LastIndexOf('.') + 1);
        }
    }
}
