using System;
using System.Collections.Generic;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using DAL;
using WindowsService.Model;

namespace WindowsService.DAL
{
    public  class TDB_DATAAUTOUPDATEService
    {
        public static List<TDB_DATAAUTOUPDATEInfo> getRows(string whereStr)
        {
            string sql = "select * from TDB_DATAAUTOUPDATE " + whereStr;
            List<TDB_DATAAUTOUPDATEInfo> list = GetValue(sql);
            return list;
        }
        private static List<TDB_DATAAUTOUPDATEInfo> GetValue(string sql)
        {
            List<TDB_DATAAUTOUPDATEInfo> list = new List<TDB_DATAAUTOUPDATEInfo>();
            OracleDataReader odr = DBHelper.GetReader(sql);
            while (odr.Read())
            {
                TDB_DATAAUTOUPDATEInfo ei = new TDB_DATAAUTOUPDATEInfo();
                ei.ID =Convert.ToInt32(odr["ID"]);
                ei.OLDBACKDATABASEID = odr["OLDBACKDATABASEID"].ToString();
                ei.OLDDATABASEID = odr["OLDDATABASEID"].ToString();
                ei.OLDDATASET = odr["OLDDATASET"].ToString();
                ei.OLDDATALAYER = odr["OLDDATALAYER"].ToString();
                ei.TARGETBACKDATABASEID = odr["TARGETBACKDATABASEID"].ToString();
                ei.TARGETDATABASEID = odr["TARGETDATABASEID"].ToString();
                ei.TARGETDATASET = odr["TARGETDATASET"].ToString();
                ei.TARGETDATALAYER = odr["TARGETDATALAYER"].ToString();
                ei.UPDATETIME = odr["UPDATETIME"].ToString();
                ei.CREATTIME = odr["CREATTIME"].ToString();
                ei.LASTUPDATETIME = odr["LASTUPDATETIME"].ToString();
                list.Add(ei);
            }
            odr.Close();
            return list;
        }
    }
}
