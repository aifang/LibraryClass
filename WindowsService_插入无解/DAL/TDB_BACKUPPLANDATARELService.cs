using System;
using System.Collections.Generic;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using DAL;
using WindowsService.Model;

namespace WindowsService.DAL
{
     public class TDB_BACKUPPLANDATARELService
    {
         public static List<TDB_BACKUPPLANDATARELInfo> GetRows(string whereStr)
        {
            string sql = "select * from TDB_BACKUPPLANDATAREL " + whereStr;
            List<TDB_BACKUPPLANDATARELInfo> list = GetValue(sql);
            return list;
        }
         private static List<TDB_BACKUPPLANDATARELInfo> GetValue(string sql)
        {
            List<TDB_BACKUPPLANDATARELInfo> list = new List<TDB_BACKUPPLANDATARELInfo>();
            OracleDataReader odr = DBHelper.GetReader(sql);
            while (odr.Read())
            {
                TDB_BACKUPPLANDATARELInfo ei = new TDB_BACKUPPLANDATARELInfo();
                ei.PLANID = odr["PLANID"].ToString();
                ei.THEMATICID = odr["THEMATICID"].ToString();
                list.Add(ei);
            }
            odr.Close();
            return list;
        }
    }
}
