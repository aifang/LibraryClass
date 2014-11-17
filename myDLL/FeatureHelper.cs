using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace myDLL
{
    public static class FeatureHelper
    {
        /// <summary>
        /// 原版sde Feature删除方法
        /// </summary>
        /// <param name="inputFtClass">待处理的要素集</param>
        /// <param name="whereStr">要删除feature的查询条件</param>
        /// <returns></returns>
        //调用示例FeatureHelper.deleteFeature("HR.Counties", "objectid_1=59");  HR为用户名的大写
        public static bool deleteFeature(ESRI.ArcGIS.Geodatabase.IFeatureClass inputFtClass, string whereStr)
        {
            bool deleteState = false;
            ESRI.ArcGIS.Geodatabase.IWorkspace pWorkspace = ((IDataset)inputFtClass).Workspace;
            //ESRI.ArcGIS.Geodatabase.IFeatureClass inputFtClass = getFeatureClassFromWorkspace(pWorkspace ,featureClassName);
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
        /// 从Workspace中获取指定FeatureClass
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


        /// <summary>
        ///  省厅版低效用地历史图层（撤销功能专用）
        /// </summary>
        /// <param name="inputFtClass">待处理的要素集</param>
        /// <param name="whereStr">要删除feature的查询条件</param>
        /// <returns></returns>
        //调用示例FeatureHelper.deleteFeature("HR.Counties", "objectid_1=59");  HR为用户名的大写
        public static bool deleteFeature(ESRI.ArcGIS.Geodatabase.IFeatureClass inputFtClass, List<string> xmdkguid)
        {
            bool deleteState = false;
            List<string> whereStrList = new List<string>();
            for (int i = 0; i < xmdkguid.Count; i++)
            {
                whereStrList.Add("XMDKGUID='" + xmdkguid[i] + "'");
            }

            //ESRI.ArcGIS.Geodatabase.IWorkspace pWorkspace = //GetSDEWorkspace("localhost", "5151", "hr", "sys", "", "SDE.DEFAULT");//localhost为ip
            //ESRI.ArcGIS.Geodatabase.IFeatureClass inputFtClass = getFeatureClassFromWorkspace(pWorkspace ,featureClassName);
            if (inputFtClass == null) return deleteState;
            ESRI.ArcGIS.Geodatabase.IWorkspace pWorkspace = ((ESRI.ArcGIS.Geodatabase.IDataset)inputFtClass).Workspace;
            ESRI.ArcGIS.Geodatabase.IQueryFilter pQueryFilter = new ESRI.ArcGIS.Geodatabase.QueryFilterClass();

            //pQueryFilter.WhereClause = "\"Shape_Area\"<9000";       gdb, shapefile, dBase table, coverage, INFO table --> "AREA"
            //pQueryFilter.WhereClause = "[Shape_Area]<9000";         mdb --> [AREA]
            //pQueryFilter.WhereClause = "Shape_Area<9000";           SDE、ArcIMS image service、feature service--> AREA    

            foreach (string item in whereStrList)
            {
                pQueryFilter.WhereClause = item;
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
                    catch (Exception e)
                    {
                        //可以添加代码查看异常信息
                        deleteState = false;
                    }
                }

            }
            return deleteState;
        }



        /// <summary>
        /// 省厅低效用地历史图层（撤销功能）插入要素专用方法
        /// </summary>
        /// <param name="sourceFtClass"></param>
        /// <param name="TargetFtClass"></param>
        /// <param name="xmdkguid"></param>
        /// <returns></returns>
        public static bool insertFeatureDXYD(ESRI.ArcGIS.Geodatabase.IFeatureClass sourceFtClass, ESRI.ArcGIS.Geodatabase.IFeatureClass TargetFtClass, List<string> xmdkguid)
        {
            bool insertState = false;

            List<string> whereStrList = new List<string>();
            for (int i = 0; i < xmdkguid.Count; i++)
            {
                whereStrList.Add("XMDKGUID='" + xmdkguid[i] + "'");
            }
            if (sourceFtClass == null) return insertState;
            ESRI.ArcGIS.Geodatabase.IWorkspace pWorkspace = ((ESRI.ArcGIS.Geodatabase.IDataset)TargetFtClass).Workspace;
            ESRI.ArcGIS.Geodatabase.IQueryFilter pQueryFilter = new ESRI.ArcGIS.Geodatabase.QueryFilterClass();

            //pQueryFilter.WhereClause = "\"Shape_Area\"<9000";       gdb, shapefile, dBase table, coverage, INFO table --> "AREA"
            //pQueryFilter.WhereClause = "[Shape_Area]<9000";         mdb --> [AREA]
            //pQueryFilter.WhereClause = "Shape_Area<9000";           SDE、ArcIMS image service、feature service--> AREA    

            foreach (string item in whereStrList)
            {
                pQueryFilter.WhereClause = item;
                ESRI.ArcGIS.Geodatabase.IFeatureCursor pFtCursor = sourceFtClass.Search(pQueryFilter, false);
                IFeatureCursor insertFeatureCursor = TargetFtClass.Insert(true);
                IFeatureBuffer pFeatureBuffer = TargetFtClass.CreateFeatureBuffer();
                ESRI.ArcGIS.Geodatabase.IFeature inputpFeature = null;
                IGeometry pGeometry = null;
                while ((inputpFeature = pFtCursor.NextFeature()) != null)
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

                        //插入feature
                        for (int i = 0; i < inputpFeature.Fields.FieldCount; i++)
                        {
                            IField field = inputpFeature.Fields.Field[i];
                            if ((field.Type != esriFieldType.esriFieldTypeGeometry) && (field.Type != esriFieldType.esriFieldTypeOID) && field.Editable && field!=TargetFtClass.LengthField  && field!=TargetFtClass.AreaField)
                                pFeatureBuffer.set_Value(i, inputpFeature.get_Value(i));
                        }
                        pGeometry = inputpFeature.Shape;
                        pFeatureBuffer.Shape = pGeometry;
                        insertFeatureCursor.InsertFeature(pFeatureBuffer);
                        //count++;
                        //inputpFeature = pFtCursor.NextFeature();
                        insertFeatureCursor.Flush();
                        wkspcEdit.StopEditOperation();

                        if (weStartedEditing)
                        {
                            wkspcEdit.StopEditing(true);
                        }

                        insertState = true;
                    }
                    catch (Exception e)
                    {

                        insertState = false;
                    }
                }
            }
            return insertState;
        }


        /// <summary>
        /// 原版Feature插入方法
        /// </summary>
        /// <param name="sourceFtClass">源图层</param>
        /// <param name="TargetFtClass">历史图层</param>
        /// <param name="Wherestr">插入要素筛选条件</param>
        /// <returns>是否插入成功</returns>
        public static bool insertFeature(ESRI.ArcGIS.Geodatabase.IFeatureClass sourceFtClass, ESRI.ArcGIS.Geodatabase.IFeatureClass TargetFtClass, List<string> Wherestr)
        {
            bool insertState = false;
            List<string> whereStrList = Wherestr;

            #region  适应多种查询条件
            //List<string> whereStrList = new List<string>();
            //for (int i = 0; i < xmdkguid.Count; i++)
            //{
            //    whereStrList.Add("XMDKGUID='" + xmdkguid[i] + "'");
            //}

            //不同数据库的查询语句的写法
            //pQueryFilter.WhereClause = "\"Shape_Area\"<9000";       gdb, shapefile, dBase table, coverage, INFO table --> "AREA"
            //pQueryFilter.WhereClause = "[Shape_Area]<9000";         mdb --> [AREA]
            //pQueryFilter.WhereClause = "Shape_Area<9000";           SDE、ArcIMS image service、feature service--> AREA    

            #endregion 

            if (sourceFtClass == null||TargetFtClass==null) return insertState;
            ESRI.ArcGIS.Geodatabase.IWorkspace pWorkspace = ((ESRI.ArcGIS.Geodatabase.IDataset)TargetFtClass).Workspace;
            ESRI.ArcGIS.Geodatabase.IQueryFilter pQueryFilter = new ESRI.ArcGIS.Geodatabase.QueryFilterClass();

            foreach (string item in whereStrList)
            {
                pQueryFilter.WhereClause = item;
                ESRI.ArcGIS.Geodatabase.IFeatureCursor pFtCursor = sourceFtClass.Search(pQueryFilter, false);
                IFeatureCursor insertFeatureCursor = TargetFtClass.Insert(true);
                IFeatureBuffer pFeatureBuffer = TargetFtClass.CreateFeatureBuffer();
                ESRI.ArcGIS.Geodatabase.IFeature inputpFeature = null;
                IGeometry pGeometry = null;
                int count = 0;

                ESRI.ArcGIS.Geodatabase.IWorkspaceEdit wkspcEdit = pWorkspace as ESRI.ArcGIS.Geodatabase.IWorkspaceEdit;
                bool weStartedEditing = false;
                if (!wkspcEdit.IsBeingEdited())
                {
                    wkspcEdit.StartEditing(false);
                    weStartedEditing = true;
                }
                wkspcEdit.StartEditOperation();

                try
                {

                    while ((inputpFeature = pFtCursor.NextFeature()) != null)
                    {

                        //插入feature
                        for (int i = 0; i < inputpFeature.Fields.FieldCount; i++)
                        {
                            IField field = inputpFeature.Fields.Field[i];
                            if ((field.Type != esriFieldType.esriFieldTypeGeometry) && (field.Type != esriFieldType.esriFieldTypeOID) && field.Editable && field != TargetFtClass.LengthField && field != TargetFtClass.AreaField)
                                pFeatureBuffer.set_Value(i, inputpFeature.get_Value(i));
                        }
                        pGeometry = inputpFeature.Shape;
                        pFeatureBuffer.Shape = pGeometry;
                        insertFeatureCursor.InsertFeature(pFeatureBuffer);
                        count++;
                        if (count % 200 == 0)  //满足保存条件后插入
                        {
                            insertFeatureCursor.Flush();
                        }
                        insertState = true;
                    }
                    insertFeatureCursor.Flush();  //最后一次保存

                }
                catch (Exception e)
                {
                    //添加语句输出错误
                    insertState = false;
                }

                wkspcEdit.StopEditOperation();
                if (weStartedEditing)
                {
                    wkspcEdit.StopEditing(true);
                }
            }
            return insertState;
        }

    }
}
