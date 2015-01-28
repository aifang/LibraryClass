using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace OverlayAnalysisTest
{
    class Class1
    {

        ////图形数据更新
        ///// <summary>
        ///// 更新要素集
        ///// </summary>
        ///// <param name="_FeatureDatasetNameSource">数据集名称</param>
        ///// <param name="_FeatureClassNameSourceList">要素类集合</param>
        //public static void updateFeatureClass(string _FeatureDatasetNameSource, List<string> _FeatureClassNameSourceList, string _FeatureDatasetNameTarget, List<string> _FeatureClassNameTargetList, IWorkspace sourceW, IWorkspace2 targetW)
        //{
        //    IWorkspace pWSSource = sourceW;
        //    IWorkspace2 pWStarget = targetW;

        //    //IWorkspace2 pWStarget = (IWorkspace2)myDLL.WorkspaceHelper.GetSDEWorkspace("172.16.1.108", "5151", "FHORCL", "fhorcl", "", "SDE.DEFAULT");
        //    if (pWSSource == null | pWStarget == null)
        //    {
        //        using (System.IO.StreamWriter sw = new System.IO.StreamWriter("C:\\temp\\log.txt", true))
        //        {
        //            sw.WriteLine(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + "-----------未获取到IWorkSpace-------");
        //        }
        //    }

        //    if (pWSSource != null && pWStarget != null)
        //    {


        //        bool datasetExist = pWStarget.get_NameExists(esriDatasetType.esriDTFeatureDataset, _FeatureDatasetNameTarget); //判断featureDataset是否存在Workspace中
        //        IFeatureWorkspace pFWStarget = (IFeatureWorkspace)pWStarget;

        //        if (datasetExist) //featuredataset存在
        //        {
                    
        //            //foreach (string pFeatureClassNameSour in _FeatureClassNameSourceList)
        //            for (int i = 0; i < _FeatureClassNameSourceList.Count;i++ )
        //            {

        //                bool featureClassExist = pWStarget.get_NameExists(esriDatasetType.esriDTFeatureClass, _FeatureClassNameTargetList[i]);  //判断featureClass是否存在Workspace中
        //                if (featureClassExist) //featureClass存在
        //                {
        //                    IFeatureClass pFeatureClassTarget = pFWStarget.OpenFeatureClass(_FeatureClassNameTargetList[i]);
        //                    string strDatasetNameofFClass = pFeatureClassTarget.FeatureDataset.BrowseName.Split('.')[1];
        //                    //string strDatasetNameofFClass = pFeatureClassTarget.FeatureDataset.BrowseName;
        //                    if (_FeatureDatasetNameTarget == strDatasetNameofFClass) //featureCLass属于featureDataset
        //                    {
        //                        //更新代码  删除插入（事务回滚）
        //                        if (deleteFeature(pFeatureClassTarget, ""))  //删除成功
        //                        {
        //                            List<string> whereList = new List<string>();
        //                            whereList.Add("");
        //                            IFeatureClass pFeatureClassSource = ((IFeatureWorkspace)pWSSource).OpenFeatureClass(_FeatureClassNameSourceList[i]);
        //                            insertFeature(pFeatureClassSource, pFeatureClassTarget, whereList);
        //                        }

        //                    }
        //                    else //featureClass不属于featureDataset
        //                    {
        //                        //删除后更新或者直接更新                                
        //                    }
        //                }
        //                else //featureClass不存在
        //                {
        //                    //获取数据源图层
        //                    IFeatureWorkspace pFWSSource = (IFeatureWorkspace)pWSSource;
        //                    IFeatureClass pFClassSource = pFWSSource.OpenFeatureClass(_FeatureClassNameSourceList[i]);
        //                    //创建目标数据图层
        //                    IFeatureDataset pFDataset = pFWStarget.OpenFeatureDataset(_FeatureDatasetNameTarget);
        //                    IFeatureClass pFClassTarget = pFDataset.CreateFeatureClass(_FeatureClassNameTargetList[i], pFClassSource.Fields, pFClassSource.CLSID, pFClassSource.EXTCLSID, pFClassSource.FeatureType, pFClassSource.ShapeFieldName, "");
        //                    //源数据写入目标数据图层
        //                    List<string> whereList = new List<string>();
        //                    whereList.Add("");
        //                    insertFeature(pFClassSource, pFClassTarget, whereList);
        //                }
        //            }
        //        }
        //        else //featuredataset不存在
        //        {
        //            IFeatureWorkspace pFWSSource = (IFeatureWorkspace)pWSSource;
        //            IFeatureDataset pFDatasetSource = pFWSSource.OpenFeatureDataset(_FeatureDatasetNameSource);
        //            //创建featuredataset
        //            IGeoDataset geodataset = (IGeoDataset)pFDatasetSource;
        //            ISpatialReference fdsSR = geodataset.SpatialReference; //createSpatialReference(false, 2365);
        //            IFeatureDataset pFDatasetTar = pFWStarget.CreateFeatureDataset(_FeatureDatasetNameSource, fdsSR);

        //            //复制FeatureClass
        //            foreach (string FClassNameTar in _FeatureClassNameSourceList)
        //            {
        //                //获取数据源图层                        
        //                IFeatureClass pFClassSource = pFWSSource.OpenFeatureClass(FClassNameTar);
        //                //创建目标数据图层                                      
        //                IFeatureClass pFClassTarget = pFDatasetTar.CreateFeatureClass(FClassNameTar, pFClassSource.Fields, pFClassSource.CLSID, pFClassSource.EXTCLSID, pFClassSource.FeatureType, pFClassSource.ShapeFieldName, "");
        //                //源数据写入目标数据图层
        //                List<string> whereList = new List<string>();
        //                whereList.Add("");
        //                insertFeature(pFClassSource, pFClassTarget, whereList);
        //            }
        //        }

        //    }
        //}
    }
}
