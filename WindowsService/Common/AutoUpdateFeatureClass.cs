using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsService.BLL;
using WindowsService.Model;

namespace WindowsService.Common
{
    public class AutoUpdateFeatureClass
    {
        public static string errMessage { get; set; }
        public static bool AutoUpdateFClass()
        {
            bool state = false;

            List<AutoUpdateInfo> pUpdateLst = AutoUpdateInfoManage.AutoUpdateInfoList();
            if (pUpdateLst.Count == 0)
            {
                //using (System.IO.StreamWriter sw = new System.IO.StreamWriter("C:\\temp\\log.txt", true))
                //{
                //    sw.WriteLine(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 没有找到任何自动更新计划.");
                //}
                return state;
            }

            foreach(AutoUpdateInfo item in pUpdateLst)
            {
                try
                {
                    state=UpdateOnePlan(item);
                }
                catch(Exception error)
                {
                    errMessage += error.Message + "/n";
                    state = false;
                }
                
            }

            //using (System.IO.StreamWriter sw = new System.IO.StreamWriter("C:\\temp\\log.txt", true))
            //{
            //    sw.WriteLine(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +"/n"+ errMessage);
            //}
            return state;
        }

        private static bool UpdateOnePlan(AutoUpdateInfo pUpdateInfo)
        {
            bool state = false;

            #region 初始化参数

            List<string> pFClassSouList = new List<string>();
            pFClassSouList.Add(pUpdateInfo.SourceDataLayer.Split('.')[1]);

            List<string> pFClassTarList = new List<string>();
            pFClassTarList.Add(pUpdateInfo.TargetDataLayer.Split('.')[1]);

            //("172.16.1.108", "5151", "FHORCL", "fhorcl", "", "SDE.DEFAULT");
            List<string> sourSdeSet = new List<string>
            {
                pUpdateInfo.SourceSdeIP,
                pUpdateInfo.SourceSdeInstance,
                pUpdateInfo.SourceSdeUser,
                pUpdateInfo.SourceSdePassword,
                "",
                pUpdateInfo.SourceSdeVersion
            };

            List<string> sourBackupSdeSet = new List<string>
            {
                pUpdateInfo.SourceBackupSdeIP,
                pUpdateInfo.SourceBackupSdeInstance,
                pUpdateInfo.SourceBackupSdeUser,
                pUpdateInfo.SourceBackupSdePassword,
                "",
                pUpdateInfo.SourceBackupSdeVersion
            };

            List<string> targetBackupSdeSet = new List<string>
            {
                pUpdateInfo.TargetBackupSdeIP,
                pUpdateInfo.TargetBackupSdeInstance,
                pUpdateInfo.TargetBackupSdeUser,
                pUpdateInfo.TargetBackupSdePassword,
                "",
                pUpdateInfo.TargetBackupSdeVersion
            };

            List<string> targetSdeSet = new List<string>
            {
                pUpdateInfo.TargetSdeIP,
                pUpdateInfo.TargetSdeInstance,
                pUpdateInfo.TargetSdeUser,
                pUpdateInfo.TargetSdePassword,
                "",
                pUpdateInfo.TargetSdeVersion
            };
            #endregion

            //1.Source数据库备份
            FeatureClassCopy.backupFeatureDataset(pUpdateInfo.SourceDataset.Split('.')[1], pFClassSouList, "", sourSdeSet, sourBackupSdeSet);

            //2.Target数据库备份
            //FeatureClassCopy.backupFeatureDataset(pUpdateInfo.TargetDataset.Split('.')[1], pFClassTarList, "", targetSdeSet, targetBackupSdeSet);

            //3.Source写入Target数据库
            //FeatureClassCopy.updateFeatureClass(pUpdateInfo.SourceDataset.Split('.')[1], pFClassSouList, pUpdateInfo.TargetDataset.Split('.')[1], pFClassTarList, targetSdeSet, targetBackupSdeSet);
            state = true;
            return state;
        }
    }
}
