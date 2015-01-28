using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsService.DAL;
using WindowsService.Model;

namespace WindowsService.BLL
{
    public class TDB_DATAAUTOUPDATEManage
    {
        public static List<TDB_DATAAUTOUPDATEInfo> FingAll()
        {
            return TDB_DATAAUTOUPDATEService.getRows("");
        }

        //public static List<TDB_DATAAUTOUPDATEInfo> GetRowByID(string SDEID)
        //{
        //    string whereStr = string.Format("where ID='{0}'", SDEID);
        //    return TDB_DATAAUTOUPDATEService.getRows(whereStr);
        //}
    }
}
