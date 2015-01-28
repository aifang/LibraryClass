using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsService.DAL;
using WindowsService.Model;

namespace WindowsService.BLL
{
    public class TDB_BACKUPPLANManage
    {
        public static List<TDB_BACKUPPLANInfo> FingAll()
        {
            return TDB_BACKUPPLANService.FindAll("");
        }
    }
}
