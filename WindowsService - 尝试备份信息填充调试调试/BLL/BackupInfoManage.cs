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

                //填充图形数据备份信息


            }




            return pBackupInfoLst;
        }
    }
}
