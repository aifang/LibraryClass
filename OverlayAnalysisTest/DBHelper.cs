using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using System.Data.OracleClient;

namespace DAL
{
    public static class DBHelper
    {
        private static OracleConnection connection;
        //private static readonly string connstr = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=ExceptionsCollection.mdb;Persist Security Info=False;";
        //private static readonly string connstr = @"data source=orcl;user=plot;password=123;providerName=System.Data.OracleClient";
        private static readonly string connstr = @"data source=orcl;user=DLDATACENTER;password=DLDATACENTER;";//providerName=System.Data.OracleClient";
        /// <summary>
        /// 获取连接
        /// </summary>
        public static OracleConnection Connection
        {
            get 
            {
                if (connection == null)
                {
                    connection = new OracleConnection(connstr);
                    connection.Open();
                }
                else if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                else if (connection.State == ConnectionState.Broken)
                {
                    connection.Close();
                    connection.Open();
                }
                return connection;
            }
        }
        public static int ExcuteCommand(string safeSql)
        {
            OracleCommand cmd = new OracleCommand(safeSql, Connection);            
            int result = cmd.ExecuteNonQuery();
            return result;
        }
        public static int ExcuteCommand(string sql, params OracleParameter[] values)
        {
            OracleCommand cmd = new OracleCommand(sql, Connection);
            cmd.Parameters.AddRange(values);
            int result = cmd.ExecuteNonQuery();
            return result;
        }
        public static OracleDataReader GetReader(string safeSql)
        {
            OracleCommand cmd = new OracleCommand(safeSql, Connection);
            OracleDataReader reader = cmd.ExecuteReader();
            return reader;
        }
        public static OracleDataReader GetReader(string safeSql, params OracleParameter[] values)
        {
            OracleCommand cmd = new OracleCommand(safeSql, Connection);
            cmd.Parameters.AddRange(values);
            OracleDataReader reader = cmd.ExecuteReader();
            return reader;
        }
        public static DataTable GetDataSet(string safeSql)
        {
            DataSet ds = new DataSet();
            OracleCommand cmd = new OracleCommand(safeSql, Connection);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            da.Fill(ds);
            return ds.Tables[0];
        }
        public static DataTable GetDataSet(string sql, params OracleParameter[] values)
        {
            DataSet ds = new DataSet();
            OracleCommand cmd = new OracleCommand(sql, Connection);
            cmd.Parameters.AddRange(values);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            da.Fill(ds);
            return ds.Tables[0];
        }
    }
}
