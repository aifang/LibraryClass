using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.DataSourcesFile;


namespace myDLL
{
    public class WorkspaceHelper
    {
        //public static string GISConnectionString;
        #region  公有方法

        //获取已有存在的工作空间

        public static IWorkspace GetAccessWorkspace(string sFilePath)
        {
            if (!File.Exists(sFilePath))
            {
                return null;
            }
            try
            {
                IWorkspaceFactory factory = new AccessWorkspaceFactoryClass();
                return factory.OpenFromFile(sFilePath, 0);
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// SDE连接数据库
        /// </summary>
        /// <param name="sServerName">server = "localhost"</param>
        /// <param name="sInstancePort">Instance = "5151"</param>
        /// <param name="sUserName">用户名</param>
        /// <param name="sPassword">密码</param>
        /// <param name="database">Database = "SDE" or "" if Oracle</param>
        /// <param name="sVersionName">Version = "SDE.DEFAULT"</param>
        /// <returns></returns>        
        public static IWorkspace GetSDEWorkspace(string sServerName, string sInstancePort, string sUserName, string sPassword,string database, string sVersionName)
        {
            IPropertySet set = new PropertySetClass();
            set.SetProperty("Server", sServerName);
            set.SetProperty("Instance", sInstancePort);
            set.SetProperty("User", sUserName);
            set.SetProperty("password", sPassword);
            set.SetProperty("DATABASE", database);
            set.SetProperty("version", sVersionName);
            SdeWorkspaceFactoryClass class2 = new SdeWorkspaceFactoryClass();
            try
            {
                return class2.Open(set, 0);
            }
            catch
            {
                return null;
            }
        }
        public static IWorkspace GetFGDBWorkspace(string sFilePath)
        {
            if (!System.IO.Directory.Exists(sFilePath))
            {
                return null;
            }
            try
            {
                IWorkspaceFactory factory = new FileGDBWorkspaceFactoryClass();
                return factory.OpenFromFile(sFilePath, 0);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 输入.shp完整路径返回Workspace
        /// </summary>
        /// <param name="sFilePath">.shp所在完整路径</param>
        public static IWorkspace GetShapefileWorkspace(string sFilePath)
        {
            if (!File.Exists(sFilePath))
            {
                return null;
            }
            try
            {
                IWorkspaceFactory factory = new ShapefileWorkspaceFactoryClass();
                sFilePath = System.IO.Path.GetDirectoryName(sFilePath);
                return factory.OpenFromFile(sFilePath, 0);
            }
            catch
            {
                return null;
            }
        }



        //创建工作空间


        /// <summary>
        /// 创建mdb工作空间
        /// </summary>
        /// <param name="filePath">路径</param>
        /// <param name="fileName">sample.mdb或者sample</param>
        /// <returns></returns>
        public static IWorkspace CreateAccessWorkspace(string filePath, string fileName)
        {
            return PriCreateDBWorkspace(filePath, fileName, "esriDataSourcesGDB.AccessWorkspaceFactory");
        }

        /// <summary>
        /// 创建gdb工作空间
        /// </summary>
        /// <param name="filePath">路径</param>
        /// <param name="fileName">sample.gdb或者sample</param>
        /// <returns></returns>
        public static IWorkspace CreateFileGDBWorkspace(string filePath, string fileName)
        {
            return PriCreateDBWorkspace(filePath, fileName, "esriDataSourcesGDB.FileGDBWorkspaceFactory");
        }


        /// <summary>
        /// 创建shapefile工作空间
        /// </summary>
        /// <param name="filePath">路径</param>
        /// <param name="fileName">sample</param>
        /// <returns></returns>
        public static IWorkspace CreateShapefileWorkspace(string filePath, string fileName)
        {
            return PriCreateDBWorkspace(filePath, fileName, "esriDataSourcesFile.ShapefileWorkspaceFactory");
        }
        
        #endregion


        #region  私有方法

        private static IWorkspace PriCreateDBWorkspace(String path,string fileName,string pType)
        {           
            Type factoryType = Type.GetTypeFromProgID(pType);
            IWorkspaceFactory workspaceFactory = (IWorkspaceFactory)Activator.CreateInstance(factoryType);
            IWorkspaceName workspaceName = workspaceFactory.Create(path, fileName, null, 0);
            IName name = (IName)workspaceName;
            IWorkspace workspace = (IWorkspace)name.Open();
            return workspace;
           
        }

        
        #endregion

        #region    备用方法
        //public static string PGDBDataConnectionString(string sPath)
        //{
        //    return ("Provider=ESRI.GeoDB.OLEDB.1;Data Source=" + sPath + ";Extended Properties=workspacetype=esriDataSourcesGDB.AccessWorkspaceFactory.1;Geometry=WKB");
        //}
        //public static string SDEDataConnectionString(string sServerName, string sDataSource, string sUserName, string sPW)
        //{
        //    return ("Provider=ESRI.GeoDB.OLEDB.1;Location=" + sServerName + ";Data Source=" + sDataSource + "; User Id=" + sUserName + ";Password=" + sPW + "; Extended Properties=WorkspaceType= esriDataSourcesGDB.SDEWorkspaceFactory.1;Geometry=WKB|OBJECT;Instance=5151;Version=SDE.DEFAULT");
        //}
        //public static string ShapefileDataConnectionString(string sPath)
        //{
        //    sPath = System.IO.Path.GetDirectoryName(sPath);
        //    return ("Provider=ESRI.GeoDB.OLEDB.1;Data Source=" + sPath + ";Extended Properties=WorkspaceType=esriDataSourcesFile.ShapefileWorkspaceFactory.1;Geometry=WKB|OBJECT");
        //}
        //public static bool HighPrecision(IWorkspace pWorkspace)
        //{
        //    IGeodatabaseRelease geoVersion = pWorkspace as IGeodatabaseRelease;
        //    if (geoVersion == null) return false;
        //    if (geoVersion.MajorVersion == 2
        //        && geoVersion.MinorVersion == 2)
        //    {
        //        return true;
        //    }
        //    return false;
        //}

    //    public static List<String> QueryFeatureClassName(IWorkspace pWorkspace)
    //    {
    //        return QueryFeatureClassName(pWorkspace, false, false);
    //    }
    //    public static List<String> QueryFeatureClassName(IWorkspace pWorkspace, bool pUpperCase)
    //    {
    //        return QueryFeatureClassName(pWorkspace, pUpperCase, false);
    //    }
        ///// <summary>
        ///// 获取所有图层名字
        ///// </summary>
        ///// <param name="pWorkspace">工作空间</param>
        ///// <param name="pUpperCase">转换大小写</param>
        ///// <param name="pEscapeMetaTable"></param>
        ///// <returns></returns>
        //public static List<String> QueryFeatureClassName(IWorkspace pWorkspace, bool pUpperCase, bool pEscapeMetaTable)
        //{
        //    try
        //    {
        //        String ownerName = "";
        //        if (pWorkspace.Type == esriWorkspaceType.esriRemoteDatabaseWorkspace)
        //        {
        //            ownerName = pWorkspace.ConnectionProperties.GetProperty("user").ToString();
        //            ownerName = ownerName.ToUpper();
        //        }
        //        List<String> sc = new List<String>();
        //        IEnumDatasetName edn = pWorkspace.get_DatasetNames(esriDatasetType.esriDTFeatureDataset);
        //        IDatasetName dn = edn.Next();
        //        while (dn != null)
        //        {
        //            string dsName = dn.Name.ToUpper();
        //            if (ownerName.Equals(LayerHelper.GetClassOwnerName(dsName)))
        //            {
        //                #region 添加数据集下面的FeatureClass
        //                IEnumDatasetName fdn = dn.SubsetNames;

        //                dn = fdn.Next();
        //                while (dn != null)
        //                {
        //                    dsName = dn.Name.ToUpper();
        //                    bool isTopology = dn is ITopologyName;
        //                    if (!isTopology)
        //                    {
        //                        string shortName = LayerHelper.GetClassShortName(dsName);
        //                        if (pUpperCase)
        //                        {
        //                            shortName = shortName.ToUpper();
        //                        }
        //                        if (pEscapeMetaTable)
        //                        {

        //                        }
        //                        else
        //                        {
        //                            sc.Add(shortName);
        //                        }
        //                    }
        //                    dn = fdn.Next();
        //                }
        //                #endregion
        //            }
        //            dn = edn.Next();
        //        }
        //        #region 获取直接的FeatureClass
        //        edn = pWorkspace.get_DatasetNames(esriDatasetType.esriDTFeatureClass);
        //        dn = edn.Next();
        //        while (dn != null)
        //        {
        //            string dsName = dn.Name.ToUpper();
        //            if (ownerName.Equals(LayerHelper.GetClassOwnerName(dsName)))
        //            {
        //                string shortName = LayerHelper.GetClassShortName(dsName);
        //                if (pUpperCase)
        //                {
        //                    shortName = shortName.ToUpper();
        //                }
        //                if (pEscapeMetaTable)
        //                {

        //                }
        //                else
        //                {
        //                    sc.Add(shortName);
        //                }
        //            }
        //            dn = edn.Next();
        //        }
        //        #endregion
        //        return sc;
        //    }
        //    catch (Exception ex) { return null; }
        //}
    //    public static List<IConfigurationKeyword> GetConfigurationKeywordList(IWorkspace pWS)
    //    {
    //        List<IConfigurationKeyword> pList = new List<IConfigurationKeyword>();
    //        IWorkspaceConfiguration pWConfig = pWS as IWorkspaceConfiguration;
    //        IEnumConfigurationKeyword pEnumConfig = pWConfig.ConfigurationKeywords;
    //        IConfigurationKeyword pConfig = pEnumConfig.Next();
    //        while (pConfig != null)
    //        {
    //            pList.Add(pConfig);
    //            pConfig = pEnumConfig.Next();
    //        }
    //        return pList;
    //    }
    //    public static List<IConfigurationParameter> GetConfigurationParameterList(IConfigurationKeyword pConfig)
    //    {
    //        List<IConfigurationParameter> pList = new List<IConfigurationParameter>();
    //        IEnumConfigurationParameter pEnumCP = pConfig.ConfigurationParameters;
    //        IConfigurationParameter pCP = pEnumCP.Next();
    //        while (pCP != null)
    //        {
    //            pList.Add(pCP);
    //            pCP = pEnumCP.Next();
    //        }
    //        return pList;
        //    }

        #endregion
    } 
}
