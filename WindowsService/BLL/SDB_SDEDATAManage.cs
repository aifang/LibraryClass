using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsService.DAL;
using WindowsService.Model;

namespace WindowsService.BLL
{
    public class SDB_SDEDATAManage
    {
        public static List<SDB_SDEDATAInfo> FingAll()
        {
            return SDB_SDEDATAService.getRows("");
        }

        public static List<SDB_SDEDATAInfo> GetRowByID(string dataBaseID)
        {
            string whereStr=string.Format("where ID='{0}'",dataBaseID);
            return SDB_SDEDATAService.getRows(whereStr);
        }
    }
}
