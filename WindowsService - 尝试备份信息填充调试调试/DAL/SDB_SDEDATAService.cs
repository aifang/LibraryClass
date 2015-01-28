using System;
using System.Collections.Generic;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using DAL;
using WindowsService.Model;

namespace WindowsService.DAL
{
    public class SDB_SDEDATAService
    {
        public static List<SDB_SDEDATAInfo> FindAll(string whereStr)
        {
            string sql = "select * from SDB_SDEDATA " + whereStr;
            List<SDB_SDEDATAInfo> list = GetValue(sql);
            return list;
        }
        private static List<SDB_SDEDATAInfo> GetValue(string sql)
        {
            List<SDB_SDEDATAInfo> list = new List<SDB_SDEDATAInfo>();
            OracleDataReader odr = DBHelper.GetReader(sql);
            while (odr.Read())
            {
                SDB_SDEDATAInfo ei = new SDB_SDEDATAInfo();
                ei.ID = odr["ID"].ToString();
                ei.SDEID = odr["SDEID"].ToString();
                ei.NAME = odr["NAME"].ToString();
                ei.USERNAME = odr["USERNAME"].ToString();
                ei.PASSWORD = odr["PASSWORD"].ToString();
                ei.VERSION = odr["VERSION"].ToString();
                ei.ISBACKUP = Convert.ToChar(odr["ISBACKUP"]);
                ei.NOTE = odr["NOTE"].ToString();
                list.Add(ei);
            }
            odr.Close();
            return list;
        }
    }
}
