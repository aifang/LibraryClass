using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsService.DAL;
using WindowsService.Model;

namespace WindowsService.BLL
{
    public class SDB_SERVERManage
    {
        public static List<SDB_SERVERInfo> FingAll()
        {
            return SDB_SERVERService.GetRows("");
        }

        public static List<SDB_SERVERInfo>GetRowByID(string ServerID)
        {
            string whereStr = string.Format("where ID='{0}'", ServerID);
            return SDB_SERVERService.GetRows(whereStr);
        }
    }
}
