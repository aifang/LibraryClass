using System;
using System.Collections.Generic;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using DAL;
using WindowsService.Model;

namespace WindowsService.DAL
{
    class TDB_THEMATICService
    {
        public static List<TDB_THEMATICInfo> GetRows(string whereStr)
        {
            string sql = "select * from TDB_THEMATIC " + whereStr;
            List<TDB_THEMATICInfo> list = GetValue(sql);
            return list;
        }
        private static List<TDB_THEMATICInfo> GetValue(string sql)
        {
            List<TDB_THEMATICInfo> list = new List<TDB_THEMATICInfo>();
            OracleDataReader odr = DBHelper.GetReader(sql);
            while (odr.Read())
            {
                TDB_THEMATICInfo ei = new TDB_THEMATICInfo();
                ei.ID = odr["ID"].ToString();
                ei.TEMPLATEID = odr["TEMPLATEID"].ToString();
                ei.NAME = odr["NAME"].ToString();
                ei.STORENAME = odr["STORENAME"].ToString();
                ei.AREACODE = odr["AREACODE"].ToString();
                ei.SCALECODE = odr["SCALECODE"].ToString(); ;
                ei.ANNUALCODE = odr["ANNUALCODE"].ToString();
                ei.GEOREFCODE = odr["GEOREFCODE"].ToString();
                ei.NOTE = odr["NOTE"].ToString();
                ei.METADATA = odr["METADATA"].ToString();
                ei.DATATYPE = odr["DATATYPE"].ToString();
                list.Add(ei);
            }
            odr.Close();
            return list;
        }
    }
}
