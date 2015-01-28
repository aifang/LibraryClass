using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsService.DAL;
using WindowsService.Model;

namespace WindowsService.BLL
{
    public class SDB_SDEManage
    {
        public static List<SDB_SDEInfo> FingAll()
        {
            return SDB_SDEService.FindAll("");
        }
    }
}
