using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
//using System.Windows.Forms;
//using myDLL;

namespace FeatureClassCopy
{
    /*1.从源数据库copy到目标数据库---目标数据库有源数据库的数据集:删除目标数据库的数据图层.数据更新feature及featureClass级别的更新 
     *2.从源数据库copy到目标数据库---目标数据库没有源数据库的数据集:需要在目标数据库中创建数据集和数据图层.名称为:源数据集合数据图层后面加日期 、dataset的完全拷贝
     *
     * 
     */
    public class FeatureClassCopy
    {
        //图形数据更新
        /// <summary>
        /// 更新要素集
        /// </summary>
        /// <param name="_FeatureDatasetNameSource">数据集名称</param>
        /// <param name="_FeatureClassNameSourceList">要素类集合</param>
        public  static void updateFeatureClass(string _FeatureDatasetNameSource,List<string> _FeatureClassNameSourceList)
        {
            IWorkspace pWSSource = myDLL.WorkspaceHelper.GetSDEWorkspace("172.16.1.108", "5151", "FHORCL", "fhorcl", "", "SDE.DEFAULT");
            IWorkspace2 pWStarget = (IWorkspace2)myDLL.WorkspaceHelper.GetSDEWorkspace("172.16.1.24", "5151", "hr", "sys", "", "SDE.DEFAULT");

            //IWorkspace2 pWStarget = (IWorkspace2)myDLL.WorkspaceHelper.GetSDEWorkspace("172.16.1.108", "5151", "FHORCL", "fhorcl", "", "SDE.DEFAULT");

            if (pWSSource != null && pWStarget != null)
            {
                bool datasetExist = pWStarget.get_NameExists(esriDatasetType.esriDTFeatureDataset, _FeatureDatasetNameSource); //判断featureDataset是否存在Workspace中
                IFeatureWorkspace pFWStarget=(IFeatureWorkspace)pWStarget;
                
                if(datasetExist) //featuredataset存在
                {
                    foreach (string pFeatureClassNameSour in _FeatureClassNameSourceList)
                    {
                        bool featureClassExist = pWStarget.get_NameExists(esriDatasetType.esriDTFeatureClass, pFeatureClassNameSour);  //判断featureClass是否存在Workspace中
                        if (featureClassExist) //featureClass存在
                        {
                            IFeatureClass pFeatureClassTarget = pFWStarget.OpenFeatureClass(pFeatureClassNameSour);
                            string strDatasetNameofFClass = pFeatureClassTarget.FeatureDataset.BrowseName.Split('.')[1];
                            if(_FeatureDatasetNameSource==strDatasetNameofFClass) //featureCLass属于featureDataset
                            {
                                //更新代码  删除插入（事务回滚）
                                if(deleteFeature(pFeatureClassTarget, ""))  //删除成功
                                {
                                    List<string> whereList=new List<string>();
                                    whereList.Add("");
                                    IFeatureClass pFeatureClassSource = ((IFeatureWorkspace)pWSSource).OpenFeatureClass(pFeatureClassNameSour);
                                    insertFeature(pFeatureClassSource, pFeatureClassTarget, whereList);
                                }                         
                                
                            }
                            else //featureClass不属于featureDataset
                            {
                                //删除后更新或者直接更新                                
                            }
                        }
                        else //featureClass不存在
                        {
                            //获取数据源图层
                            IFeatureWorkspace pFWSSource = (IFeatureWorkspace)pWSSource;
                            IFeatureClass pFClassSource = pFWSSource.OpenFeatureClass(pFeatureClassNameSour);
                            //创建目标数据图层
                            IFeatureDataset pFDataset = pFWStarget.OpenFeatureDataset(_FeatureDatasetNameSource);
                            IFeatureClass pFClassTarget = pFDataset.CreateFeatureClass(pFeatureClassNameSour, pFClassSource.Fields, pFClassSource.CLSID, pFClassSource.EXTCLSID, pFClassSource.FeatureType,pFClassSource.ShapeFieldName,"");
                            //源数据写入目标数据图层
                            List<string> whereList = new List<string>();
                            whereList.Add("");
                            insertFeature(pFClassSource, pFClassTarget, whereList);
                        }
                    }
                }
                else //featuredataset不存在
                {
                    IFeatureWorkspace pFWSSource = (IFeatureWorkspace)pWSSource;
                    IFeatureDataset pFDatasetSource = pFWSSource.OpenFeatureDataset(_FeatureDatasetNameSource);
                    //创建featuredataset
                    ISpatialReference fdsSR = createSpatialReference(false, 2365);
                    IFeatureDataset pFDatasetTar = pFWStarget.CreateFeatureDataset(_FeatureDatasetNameSource, fdsSR);

                    //复制FeatureClass
                    foreach (string FClassNameTar in _FeatureClassNameSourceList)
                    {
                        //获取数据源图层                        
                        IFeatureClass pFClassSource = pFWSSource.OpenFeatureClass(FClassNameTar);
                        //创建目标数据图层                                      
                        IFeatureClass pFClassTarget = pFDatasetTar.CreateFeatureClass(FClassNameTar, pFClassSource.Fields, pFClassSource.CLSID, pFClassSource.EXTCLSID, pFClassSource.FeatureType, pFClassSource.ShapeFieldName, "");
                        //源数据写入目标数据图层
                        List<string> whereList = new List<string>();
                        whereList.Add("");
                        insertFeature(pFClassSource, pFClassTarget, whereList);  
                    }
                }               
                 
            }
        }

        //2.从源数据库copy到目标数据库---目标数据库没有源数据库的数据集:需要在目标数据库中创建数据集和数据图层.名称为:源数据集合数据图层后面加日期 、dataset的完全拷贝
        /// <summary>
        /// 数据备份
        /// </summary>
        /// <param name="_FDatasetNameSou">FeatureDataset(数据集名称)</param>
        /// <param name="_FClassNameSouList">Featureclass(要素类名称)集合</param>
        /// <param name="xmlpath">年转换成字母的XML位置</param>
        public static void backupFeatureDataset(string _FDatasetNameSou, List<string> _FClassNameSouList,string xmlpath)
        {
            IWorkspace pWSSource = myDLL.WorkspaceHelper.GetSDEWorkspace("172.16.1.108", "5151", "FHORCL", "fhorcl", "", "SDE.DEFAULT");
            IWorkspace2 pWStarget = (IWorkspace2)myDLL.WorkspaceHelper.GetSDEWorkspace("172.16.1.24", "5151", "hr", "sys", "", "SDE.DEFAULT");
            if(pWSSource!=null&&pWStarget!=null)
            {
                //System.DateTime a = DateTime.Now;
                //string FDatasetToday = _FDatasetNameSou + "_" + a.ToString("yyyyMMdd");
                string FDatasetToday = _FDatasetNameSou + getDatatimeCode(xmlpath);
                bool datasetExist = pWStarget.get_NameExists(esriDatasetType.esriDTFeatureDataset, FDatasetToday); //判断featureDataset是否存在Workspace中
                IFeatureWorkspace pFWStarget = (IFeatureWorkspace)pWStarget;
                if (datasetExist)  //FeatureDatatset存在
                {                    
                    IFeatureWorkspace pFWSSource = (IFeatureWorkspace)pWSSource;

                    //删除FeatureDataset内所有的图层
                    IFeatureDataset pFDatasetTar = pFWStarget.OpenFeatureDataset(FDatasetToday);
                    IEnumDataset _subset = pFDatasetTar.Subsets;
                    _subset.Reset();
                    IDataset pSubset;
                    while ((pSubset = _subset.Next())!=null)
                    {
                        if(pSubset.CanDelete())
                        {
                            pSubset.Delete();
                        }
                    }
                    //复制图层
                    foreach (string item in _FClassNameSouList)
                    {
                        //获取数据源图层                        
                        IFeatureClass pFClassSource = pFWSSource.OpenFeatureClass(item);
                        //创建目标数据图层
                        //string FClassNameTar = item + "_" + a.ToString("yyyyMMdd");
                        string FClassNameTar = item + getDatatimeCode(xmlpath);
                        IFeatureClass pFClassTarget = pFDatasetTar.CreateFeatureClass(FClassNameTar, pFClassSource.Fields, pFClassSource.CLSID, pFClassSource.EXTCLSID, pFClassSource.FeatureType, pFClassSource.ShapeFieldName, "");
                        //源数据写入目标数据图层
                        List<string> whereList = new List<string>();
                        whereList.Add("");
                        insertFeature(pFClassSource, pFClassTarget, whereList);  
                    }
                }
                else  //FeatureDatatset不存在
                {
                    //创建FeatureDataSet
                    IFeatureWorkspace pFWSSource = (IFeatureWorkspace)pWSSource;
                    ISpatialReference fdsSR = createSpatialReference(false, 2365);
                    IFeatureDataset pFDatasetTar = pFWStarget.CreateFeatureDataset(FDatasetToday, fdsSR);
                    //复制FeatureClass
                    foreach(string item in _FClassNameSouList)
                    {
                        //获取数据源图层                        
                        IFeatureClass pFClassSource = pFWSSource.OpenFeatureClass(item);
                        //创建目标数据图层
                        //string FClassNameTar = item+"_"+a.ToString("yyyyMMdd");
                        string FClassNameTar = item + getDatatimeCode(xmlpath);
                        IFeatureClass pFClassTarget = pFDatasetTar.CreateFeatureClass(FClassNameTar, pFClassSource.Fields, pFClassSource.CLSID, pFClassSource.EXTCLSID, pFClassSource.FeatureType, pFClassSource.ShapeFieldName, "");
                        //源数据写入目标数据图层
                        List<string> whereList = new List<string>();
                        whereList.Add("");
                        insertFeature(pFClassSource, pFClassTarget, whereList);  
                    }
                }
            }
        }


        /// <summary>
        /// 原版sde Feature删除方法
        /// </summary>
        /// <param name="inputFtClass">待处理的要素集</param>
        /// <param name="whereStr">要删除feature的查询条件</param>
        /// <returns></returns>
        //调用示例FeatureHelper.deleteFeature("HR.Counties", "objectid_1=59");  HR为用户名的大写
        private static bool deleteFeature(ESRI.ArcGIS.Geodatabase.IFeatureClass inputFtClass, string whereStr)
        {            
            bool deleteState = false;
            ESRI.ArcGIS.Geodatabase.IWorkspace pWorkspace = ((IDataset)inputFtClass).Workspace;
            //ESRI.ArcGIS.Geodatabase.IFeatureClass inputFtClass = getFeatureClassFromWorkspace(pWorkspace ,featureClassName);
            if (inputFtClass == null) return deleteState;
            ESRI.ArcGIS.Geodatabase.IQueryFilter pQueryFilter = new ESRI.ArcGIS.Geodatabase.QueryFilterClass();
            //检查是否有记录
            pQueryFilter.WhereClause = "";
            if (inputFtClass.FeatureCount(pQueryFilter) == 0) return true;

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
                catch (Exception e)
                {
                    //可以添加代码查看异常信息

                    deleteState = false;
                }
            }
            return deleteState;
        }



        /// <summary>
        /// 原版Feature插入方法
        /// </summary>
        /// <param name="sourceFtClass">源图层</param>
        /// <param name="TargetFtClass">历史图层</param>
        /// <param name="Wherestr">插入要素筛选条件</param>
        /// <returns>是否插入成功</returns>
        private static bool insertFeature(ESRI.ArcGIS.Geodatabase.IFeatureClass sourceFtClass, ESRI.ArcGIS.Geodatabase.IFeatureClass TargetFtClass, List<string> Wherestr)
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

            if (sourceFtClass == null || TargetFtClass == null) return insertState;
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


        /// <summary>
        /// 创建空间参考
        /// </summary>
        /// <param name="coorditateType">地理坐标系为true，投影坐标系为false</param>
        /// <param name="spatialRefEnum">WKID：4326 GCS_WGS_1984为地理坐标系，21478 Beijing1954GK_18N(北京54)和2365 Xian1980_3_Degree_GK_Zone_41(西安80)</param>
        /// <returns></returns>
        private static ESRI.ArcGIS.Geometry.ISpatialReference createSpatialReference(System.Boolean coorditateType, System.Int32 spatialRefEnum)
        {
            //创建SpatialReferenceEnvironmentClass对象
            ISpatialReferenceFactory3 pSpaRefFactory = new SpatialReferenceEnvironmentClass();
            if (coorditateType)
            {
                //创建地理坐标系对象
                IGeographicCoordinateSystem pNewGeoSys = pSpaRefFactory.CreateGeographicCoordinateSystem(spatialRefEnum);
                return pNewGeoSys;
            }
            else
            {
                //创建投影坐标系
                IProjectedCoordinateSystem pNewProjsys = pSpaRefFactory.CreateProjectedCoordinateSystem(spatialRefEnum);
                return pNewProjsys;
            }
        }



        //读XML
        
        /// <summary>
        /// 获取年份对应的字母
        /// </summary>
        /// <param name="xmlpath">XML路径，若设为"",则直接读取默认位置</param>
        /// <returns></returns>
        private static string getDatatimeCode(string xmlpath)
        {
            string code = "";
            string backupNameXML = "";
            System.DateTime a = DateTime.Now;
            //设置XML文件位置
            if(xmlpath=="")
            {
                backupNameXML = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"BackupNameList.xml");
            }
            else
            {
                backupNameXML = xmlpath;
            }
            
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(backupNameXML);
            XmlNode xNode;
            XmlElement xElem;
            xNode = xDoc.SelectSingleNode("/convertName");

            xElem = (XmlElement)xNode.SelectSingleNode("//add[@key='" + (a.Year).ToString() + "']");
            if (xElem != null)
            {
                code = (xElem.Attributes[1]).Value;
            }
            return "_"+code+ a.Month.ToString();

        }        

    }
}
