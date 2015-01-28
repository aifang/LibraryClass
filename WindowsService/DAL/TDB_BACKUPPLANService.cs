using System;
using System.Collections.Generic;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using DAL;
using WindowsService.Model;

namespace WindowsService.DAL
{
    public class TDB_BACKUPPLANService
    {
        public static List<TDB_BACKUPPLANInfo> FindAll(string whereStr)
        {
            string sql = "select * from TDB_BACKUPPLAN " + whereStr;
            List<TDB_BACKUPPLANInfo> list = GetValue(sql);
            return list;
        }
        private static List<TDB_BACKUPPLANInfo> GetValue(string sql)
        {
            List<TDB_BACKUPPLANInfo> list = new List<TDB_BACKUPPLANInfo>();
            OracleDataReader odr = DBHelper.GetReader(sql);
            while (odr.Read())
            {
                TDB_BACKUPPLANInfo ei = new TDB_BACKUPPLANInfo();
                ei.PLANID = odr["PLANID"].ToString();
                ei.PLANNAME = odr["PLANNAME"].ToString();
                ei.DATABASEID = odr["DATABASEID"].ToString();
                ei.PLANCREATOR = odr["PLANCREATOR"].ToString();
                ei.PLANCREATETIME = Convert.ToDateTime(odr["PLANCREATETIME"]);
                ei.PLANACTIVETIME = Convert.ToDateTime(odr["PLANACTIVETIME"]);
                ei.PLANACTIVEMETHOD =  Convert.ToInt32(odr["PLANACTIVEMETHOD"]);
                ei.EXECUTEFLAG = Convert.ToInt32( odr["EXECUTEFLAG"]);
                list.Add(ei);
            }
            odr.Close();
            return list;
        }
    }
}
