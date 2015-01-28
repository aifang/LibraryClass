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
            return TDB_BACKUPPLANDATARELService.FindAll("");
        }
    }
}
