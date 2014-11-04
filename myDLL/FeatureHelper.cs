using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace myDLL
{
    public static class FeatureHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputFtClass">待处理的要素集</param>
        /// <param name="whereStr">要删除feature的查询条件</param>
        /// <returns></returns>
        //调用示例FeatureHelper.deleteFeature("HR.Counties", "objectid_1=59");  HR为用户名的大写
        public static bool deleteFeature(string featureClassName,string whereStr)
        {
            bool deleteState = false;
            ESRI.ArcGIS.Geodatabase.IWorkspace pWorkspace = GetSDEWorkspace("localhost", "5151", "hr", "sys", "", "SDE.DEFAULT");//localhost为ip
            ESRI.ArcGIS.Geodatabase.IFeatureClass inputFtClass = getFeatureClassFromWorkspace(pWorkspace ,featureClassName);
            if (inputFtClass == null) return deleteState;
            ESRI.ArcGIS.Geodatabase.IQueryFilter pQueryFilter = new ESRI.ArcGIS.Geodatabase.QueryFilterClass();    

            //pQueryFilter.WhereClause = "\"Shape_Area\"<9000";       gdb, shapefile, dBase table, coverage, INFO table --> "AREA"
            //pQueryFilter.WhereClause = "[Shape_Area]<9000";         mdb --> [AREA]
            //pQueryFilter.WhereClause = "Shape_Area<9000";           SDE、ArcIMS image service、feature service--> AREA            
            pQueryFilter.WhereClause = whereStr;
            ESRI.ArcGIS.Geodatabase.IFeatureCursor pFtCursor = inputFtClass.Search(pQueryFilter, false);
            ESRI.ArcGIS.Geodatabase.IFeature pFeature = null;
            while ((pFeature = pFtCursor.NextFeature()) != null)
            {
                try
                {
                    ESRI.ArcGIS.Geodatabase.IWorkspaceEdit wkspcEdit = pWorkspace as ESRI.ArcGIS.Geodatabase.IWorkspaceEdit;
                    bool weStartedEditing = false;
                    if (!wkspcEdit.IsBeingEdited())
                    {
                        wkspcEdit.StartEditing(false);
                        weStartedEditing = true;
                    }

                    wkspcEdit.StartEditOperation();
                    pFeature.Delete();
                    wkspcEdit.StopEditOperation();

                    if (weStartedEditing)
                    {
                        wkspcEdit.StopEditing(true);
                    }

                    deleteState = true;
                }                
                catch(Exception e)
                {
                    //可以添加代码查看异常信息

                    deleteState = false;
                }
            }
            return deleteState;
        }


        /// <summary>
        /// 从Workspace中获取指定FeatureClass 写得不太好明天重写这个方法
        /// </summary>
        /// <param name="pWorkspace"></param>
        /// <returns></returns>
        public static ESRI.ArcGIS.Geodatabase.IFeatureClass getFeatureClassFromWorkspace(ESRI.ArcGIS.Geodatabase.IWorkspace pWorkspace,string featureClassName)
        {
            ESRI.ArcGIS.Geodatabase.IFeatureClass featureClass = null;
            if (pWorkspace != null)
            {
                ESRI.ArcGIS.Geodatabase.IFeatureWorkspace pFWS = pWorkspace as ESRI.ArcGIS.Geodatabase.IFeatureWorkspace;
                try
                {
                    featureClass = pFWS.OpenFeatureClass(featureClassName);
                }
                catch(Exception e)
                {

                }
                
            }
            return featureClass;
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
        public static ESRI.ArcGIS.Geodatabase.IWorkspace GetSDEWorkspace(string sServerName, string sInstancePort, string sUserName, string sPassword, string database, string sVersionName)
        {
            ESRI.ArcGIS.esriSystem.IPropertySet set = new ESRI.ArcGIS.esriSystem.PropertySetClass();
            set.SetProperty("Server", sServerName);
            set.SetProperty("Instance", sInstancePort);
            set.SetProperty("User", sUserName);
            set.SetProperty("password", sPassword);
            set.SetProperty("DATABASE", database);
            set.SetProperty("version", sVersionName);
            ESRI.ArcGIS.DataSourcesGDB.SdeWorkspaceFactoryClass class2 = new ESRI.ArcGIS.DataSourcesGDB.SdeWorkspaceFactoryClass();
            try
            {
                return class2.Open(set, 0);
            }
            catch
            {
                return null;
            }
        }
    }
}
