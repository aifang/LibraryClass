using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsService.DAL;
using WindowsService.Model;

namespace WindowsService.BLL
{
    public class TDB_BACKUPPLANDATARELManage
    {
        public static List<TDB_BACKUPPLANDATARELInfo> FingAll()
        {
            return TDB_BACKUPPLANDATARELService.GetRows("");
        }
        
        public static List<TDB_BACKUPPLANDATARELInfo> GetRowByID(string PlanID)
        {
            string whereStr = string.Format("where PLANID='{0}'", PlanID);
            return TDB_BACKUPPLANDATARELService.GetRows(whereStr);
        }
    }
}
