using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsService.DAL;
using WindowsService.Model;

namespace WindowsService.BLL
{
    public class TDB_THEMATICManage
    {
        public static List<TDB_THEMATICInfo> FingAll()
        {
            return TDB_THEMATICService.GetRows("");
        }
        public static List<TDB_THEMATICInfo> GetRowByID(string ThematicID)
        {
            string whereStr = string.Format("where ID='{0}'", ThematicID);
            return TDB_THEMATICService.GetRows(whereStr);
        }
    }
}
