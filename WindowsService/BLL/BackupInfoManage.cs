using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsService.DAL;
using WindowsService.Model;

namespace WindowsService.BLL
{
    public class BackupInfoManage
    {
        public static List<BackupInfo> BackupInfoList()
        {
            List<BackupInfo> pBackupInfoLst = new List<BackupInfo>();

            List<TDB_BACKUPPLANInfo> pPlan = TDB_BACKUPPLANManage.FingAll();

            foreach(TDB_BACKUPPLANInfo item in pPlan)
            {
                var pBackupInfo = new BackupInfo();

                //填充基本信息
                pBackupInfo.PlanID = item.PLANID;
                pBackupInfo.PlanName = item.PLANNAME;
                pBackupInfo.PlanCreator = item.PLANCREATOR;
                pBackupInfo.PlanCreateTime = item.PLANCREATETIME;
                pBackupInfo.PlanActiveTime = item.PLANACTIVETIME;
                pBackupInfo.PlanActiveMethod = item.PLANACTIVEMETHOD;
                
                #region 获取Target图形库信息

                //获取Target SDE 用户名、密码、版本
                List<SDB_SDEDATAInfo> pSDB_SdeData = SDB_SDEDATAManage.GetRowByID(item.DATABASEID);
                pBackupInfo.TargetSdeUser = pSDB_SdeData[0].USERNAME;
                pBackupInfo.TargetSdePassword = pSDB_SdeData[0].PASSWORD;
                pBackupInfo.TargetSdeVersion = pSDB_SdeData[0].VERSION;

                //获取目标Sde的 Instance
                List<SDB_SDEInfo> pSDB_SDEInfo = SDB_SDEINFOManage.GetRowByID(pSDB_SdeData[0].SDEID);
                pBackupInfo.TargetSdeInstance = pSDB_SDEInfo[0].INSTANCE;

                //获取targetSDE 的IP
                List<SDB_SERVERInfo> pSDB_SERVER = SDB_SERVERManage.GetRowByID(pSDB_SDEInfo[0].SERVERID);
                pBackupInfo.TargetSdeIP = pSDB_SERVER[0].IP;
                #endregion

                #region 获取Source图形库信息

                //获取要备份的FeatureDataset
                List<TDB_BACKUPPLANDATARELInfo> pTDB_BACKUPPLANDATAREL = TDB_BACKUPPLANDATARELManage.GetRowByID(item.PLANID);
                List<TDB_THEMATICInfo> pTDB_THEMATIC = TDB_THEMATICManage.GetRowByID(pTDB_BACKUPPLANDATAREL[0].THEMATICID);
                pBackupInfo.SourceSdeFeatureDataSet = pTDB_THEMATIC[0].STORENAME;




                #endregion


            }




            return pBackupInfoLst;
        }
    }
}
