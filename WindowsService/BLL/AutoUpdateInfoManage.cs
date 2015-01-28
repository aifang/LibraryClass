using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsService.Model;

namespace WindowsService.BLL
{
    public class AutoUpdateInfoManage
    {
        public static  List<AutoUpdateInfo> AutoUpdateInfoList()
        {
            List<AutoUpdateInfo> pAutoUpdateInfoLst = new List<AutoUpdateInfo>();
            List<TDB_DATAAUTOUPDATEInfo> pAutoUpdataPlan = TDB_DATAAUTOUPDATEManage.FingAll();

            foreach (TDB_DATAAUTOUPDATEInfo item in pAutoUpdataPlan)
            {
                var pAutoUpdateInfo = new AutoUpdateInfo();

                //填充基本信息
                pAutoUpdateInfo.ID = item.ID;
                pAutoUpdateInfo.SourceDataset = item.OLDDATASET;
                pAutoUpdateInfo.SourceDataLayer = item.OLDDATALAYER;
                pAutoUpdateInfo.TargetDataset = item.TARGETDATASET;
                pAutoUpdateInfo.TargetDataLayer = item.TARGETDATALAYER;
                pAutoUpdateInfo.UpdataTime = item.UPDATETIME;
                pAutoUpdateInfo.CreateTime = item.CREATTIME;
                pAutoUpdateInfo.LastUpdataTime = item.LASTUPDATETIME;
                #region 获取Target图形库信息

                //获取TargetBackup SDE 用户名、密码、版本
                List<SDB_SDEDATAInfo> pSDB_SdeData = SDB_SDEDATAManage.GetRowByID(item.TARGETBACKDATABASEID);
                pAutoUpdateInfo.TargetBackupSdeUser = pSDB_SdeData[0].USERNAME;
                pAutoUpdateInfo.TargetBackupSdePassword = pSDB_SdeData[0].PASSWORD;
                pAutoUpdateInfo.TargetBackupSdeVersion = pSDB_SdeData[0].VERSION;
                //获取TargetBackup Sde的 Instance
                List<SDB_SDEInfo> pSDB_SDEInfo = SDB_SDEINFOManage.GetRowByID(pSDB_SdeData[0].SDEID);
                pAutoUpdateInfo.TargetBackupSdeInstance = pSDB_SDEInfo[0].INSTANCE;
                //获取targetSDE 的IP
                List<SDB_SERVERInfo> pSDB_SERVER = SDB_SERVERManage.GetRowByID(pSDB_SDEInfo[0].SERVERID);
                pAutoUpdateInfo.TargetBackupSdeIP = pSDB_SERVER[0].IP;

                //获取Target SDE 用户名、密码、版本
                pSDB_SdeData = SDB_SDEDATAManage.GetRowByID(item.TARGETDATABASEID);
                pAutoUpdateInfo.TargetSdeUser = pSDB_SdeData[0].USERNAME;
                pAutoUpdateInfo.TargetSdePassword = pSDB_SdeData[0].PASSWORD;
                pAutoUpdateInfo.TargetSdeVersion = pSDB_SdeData[0].VERSION;
                //获取Target Sde的 Instance
                pSDB_SDEInfo = SDB_SDEINFOManage.GetRowByID(pSDB_SdeData[0].SDEID);
                pAutoUpdateInfo.TargetSdeInstance = pSDB_SDEInfo[0].INSTANCE;
                //获取targetSDE 的IP
                pSDB_SERVER = SDB_SERVERManage.GetRowByID(pSDB_SDEInfo[0].SERVERID);
                pAutoUpdateInfo.TargetSdeIP = pSDB_SERVER[0].IP;
                #endregion

                #region 获取Source图形库信息
                //获取SourceBackup SDE 用户名、密码、版本
                pSDB_SdeData = SDB_SDEDATAManage.GetRowByID(item.OLDBACKDATABASEID);
                pAutoUpdateInfo.SourceBackupSdeUser = pSDB_SdeData[0].USERNAME;
                pAutoUpdateInfo.SourceBackupSdePassword = pSDB_SdeData[0].PASSWORD;
                pAutoUpdateInfo.SourceBackupSdeVersion = pSDB_SdeData[0].VERSION;
                //获取TargetBackup Sde的 Instance
                pSDB_SDEInfo = SDB_SDEINFOManage.GetRowByID(pSDB_SdeData[0].SDEID);
                pAutoUpdateInfo.SourceBackupSdeInstance = pSDB_SDEInfo[0].INSTANCE;
                //获取targetSDE 的IP
                pSDB_SERVER = SDB_SERVERManage.GetRowByID(pSDB_SDEInfo[0].SERVERID);
                pAutoUpdateInfo.SourceBackupSdeIP = pSDB_SERVER[0].IP;

                //获取Suorce SDE 用户名、密码、版本
                pSDB_SdeData = SDB_SDEDATAManage.GetRowByID(item.OLDDATABASEID);
                pAutoUpdateInfo.SourceSdeUser = pSDB_SdeData[0].USERNAME;
                pAutoUpdateInfo.SourceSdePassword = pSDB_SdeData[0].PASSWORD;
                pAutoUpdateInfo.SourceSdeVersion = pSDB_SdeData[0].VERSION;
                //获取TargetBackup Sde的 Instance
                pSDB_SDEInfo = SDB_SDEINFOManage.GetRowByID(pSDB_SdeData[0].SDEID);
                pAutoUpdateInfo.SourceSdeInstance = pSDB_SDEInfo[0].INSTANCE;
                //获取targetSDE 的IP
                pSDB_SERVER = SDB_SERVERManage.GetRowByID(pSDB_SDEInfo[0].SERVERID);
                pAutoUpdateInfo.SourceSdeIP = pSDB_SERVER[0].IP;

                #endregion

                pAutoUpdateInfoLst.Add(pAutoUpdateInfo);

            }

            return pAutoUpdateInfoLst;
        }
    }
}
