using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsService.DAL;
using WindowsService.Model;

namespace WindowsService.BLL
{
    public class SDB_SDEINFOManage
    {
        public static List<SDB_SDEInfo> FingAll()
        {
            return SDB_SDEService.getRows("");
        }

        public static List<SDB_SDEInfo> GetRowByID(string SDEID)
        {
            string whereStr = string.Format("where ID='{0}'", SDEID);
            return SDB_SDEService.getRows(whereStr);
        }
    }
}
