using System;
using System.Collections.Generic;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using DAL;
using WindowsService.Model;

namespace WindowsService.DAL
{
    public class SDB_SDEService
    {
        public static List<SDB_SDEInfo> FindAll(string whereStr)
        {
            string sql = "select * from SDB_SDE " + whereStr;
            List<SDB_SDEInfo> list = GetValue(sql);
            return list;
        }
        private static List<SDB_SDEInfo> GetValue(string sql)
        {
            List<SDB_SDEInfo> list = new List<SDB_SDEInfo>();
            OracleDataReader odr = DBHelper.GetReader(sql);
            while (odr.Read())
            {
                SDB_SDEInfo ei = new SDB_SDEInfo();
                ei.ID = odr["ID"].ToString();
                ei.SERVERID = odr["SERVERID"].ToString();
                ei.USERNAME = odr["USERNAME"].ToString();
                ei.PASSWORD = odr["PASSWORD"].ToString();
                ei.INSTANCE = odr["INSTANCE"].ToString();
                ei.INSTALLPATH = odr["INSTALLPATH"].ToString();
                ei.NOTE = odr["NOTE"].ToString();
                list.Add(ei);
            }
            odr.Close();
            return list;
        }
    }
}
