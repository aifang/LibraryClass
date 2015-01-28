using System;
using System.Collections.Generic;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using DAL;
using WindowsService.Model;

namespace WindowsService.DAL
{
    public class SDB_SERVERService
    {
        public static List<SDB_SERVERInfo> GetRows(string whereStr)
        {
            string sql = "select * from SDB_SERVER " + whereStr;
            List<SDB_SERVERInfo> list = GetValue(sql);
            return list;
        }
        private static List<SDB_SERVERInfo> GetValue(string sql)
        {
            List<SDB_SERVERInfo> list = new List<SDB_SERVERInfo>();
            OracleDataReader odr = DBHelper.GetReader(sql);
            while (odr.Read())
            {
                SDB_SERVERInfo ei = new SDB_SERVERInfo();
                ei.ID = odr["ID"].ToString();
                ei.NUM = odr["NUM"].ToString();
                ei.NAME = odr["NAME"].ToString();
                ei.IP = odr["IP"].ToString();
                ei.CONFIG = odr["CONFIG"].ToString();
                ei.ISCLUSTER = Convert.ToChar(odr["ISCLUSTER"]);
                ei.CLUSTERADDRESS = odr["CLUSTERADDRESS"].ToString();
                list.Add(ei);
            }
            odr.Close();
            return list;
        }
    }
}
