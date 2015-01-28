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
            return SDB_SERVERService.FindAll("");
        }
    }
}
