using System;

using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.IO;
using System.Data;

namespace AcctrueTerminal.Common
{
   public class DataBaseConnection
    {      
        private static SQLiteConnection Ceconn = null;
        private static SQLiteTransaction sTran = null;
        private static string SqliteConnString = "Persist Security Info = False;Data Source=" + (System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase)) + "\\AcctrueWmsDataBase.db3";
        
        #region  数据库连接
        public static SQLiteConnection GetSQLiteConn()
        {
            try
            {
                if (Ceconn == null)
                {                   
                    Ceconn = new SQLiteConnection(SqliteConnString);
                    if (Ceconn.State == ConnectionState.Open)
                    {
                        Ceconn.Close();
                    }
                    if (Ceconn.State == ConnectionState.Closed)
                    {
                        Ceconn.Open();
                    }
                }
            }
            catch (Exception ex)
            {
                Ceconn.Close();
                throw ex;
            }
            return Ceconn;
        }
        #endregion

        #region 事务处理
        public static SQLiteTransaction StartSqlliteTransaction()
        {
            GetSQLiteConn();
            if (sTran == null)
            {
                sTran = Ceconn.BeginTransaction();
            }
            return sTran;
        }

        public static void CloseConn()
        {
            Ceconn.Close();
            Ceconn.Dispose();
            Ceconn = null;
        }

        public static void Commint()
        {
            sTran.Commit();
            Ceconn.Close();
            Ceconn.Dispose();
            Ceconn = null;
            sTran = null;
        }

        public static void Rollback()
        {
            sTran.Rollback();
            Ceconn.Close();
            Ceconn.Dispose();
            Ceconn = null;
            sTran = null;
        }
        #endregion
    }
}
