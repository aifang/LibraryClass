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
            return TDB_THEMATICService.FindAll("");
        }
    }
}
