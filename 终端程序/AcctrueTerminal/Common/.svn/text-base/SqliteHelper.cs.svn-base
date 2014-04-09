using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SQLite;
using System.Collections;
using System.Data.Common;

namespace AcctrueTerminal.Common
{
   public class SqliteHelper
    {
        /// <summary>
        /// 描述：SQLite类库
        /// 作者：张朝阳
        /// 创建日期:2013-08-02
        /// </summary>
        private static void PrepareCommand(SQLiteCommand command, SQLiteConnection connection, SQLiteTransaction transaction, CommandType commandType, string commandText, SQLiteParameter[] commandParameters)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            command.Connection = connection;
            command.CommandText = commandText;
            if (transaction != null)
            {
                command.Transaction = transaction;
            }
            command.CommandType = commandType;

            //attach the command parameters if they are provided
            if (commandParameters != null)
            {
                AttachParameters(command, commandParameters);
            }
            return;
        }

        private static void AttachParameters(SQLiteCommand command, SQLiteParameter[] commandParameters)
        {
            foreach (SQLiteParameter p in commandParameters)
            {
                if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
                {
                    p.Value = DBNull.Value;
                }
                command.Parameters.Add(p);
            }
        }

        #region ExecuteNonQuery

        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText)
        {
            return ExecuteNonQuery(connectionString, commandType, commandText, (SQLiteParameter[])null);
        }

        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, params SQLiteParameter[] commandParameters)
        {
            using (SQLiteConnection cn = new SQLiteConnection(connectionString))
            {
                cn.Open();
                return ExecuteNonQuery(cn, commandType, commandText, commandParameters);
            }
        }

        public static int ExecuteNonQuery(SQLiteConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteNonQuery(connection, commandType, commandText, (SQLiteParameter[])null);
        }

        public static int ExecuteNonQuery(SQLiteConnection connection, CommandType commandType, string commandText, params SQLiteParameter[] commandParameters)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            PrepareCommand(cmd, connection, (SQLiteTransaction)null, commandType, commandText, commandParameters);
            int retval = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return retval;
        }
        public static int ExecuteNonQuery(SQLiteTransaction transaction, CommandType commandType, string commandText)
        {
            return ExecuteNonQuery(transaction, commandType, commandText, (SQLiteParameter[])null);
        }

        public static int ExecuteNonQuery(SQLiteTransaction transaction, CommandType commandType, string commandText, params SQLiteParameter[] commandParameters)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters);
            int retval = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return retval;
        }
        #endregion ExecuteNonQuery

        #region ExecuteDataSet

        public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText)
        {
            return ExecuteDataset(connectionString, commandType, commandText, (SQLiteParameter[])null);
        }

        public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText, params SQLiteParameter[] commandParameters)
        {
            using (SQLiteConnection cn = new SQLiteConnection(connectionString))
            {
                cn.Open();
                return ExecuteDataset(cn, commandType, commandText, commandParameters);
            }
        }

        public static DataSet ExecuteDataset(SQLiteConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteDataset(connection, commandType, commandText, (SQLiteParameter[])null);
        }

        public static DataSet ExecuteDataset(SQLiteConnection connection, CommandType commandType, string commandText, params SQLiteParameter[] commandParameters)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            PrepareCommand(cmd, connection, (SQLiteTransaction)null, commandType, commandText, commandParameters);
            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            cmd.Parameters.Clear();
            return ds;
        }

        public static DataSet ExecuteDataset(SQLiteTransaction transaction, CommandType commandType, string commandText)
        {
            return ExecuteDataset(transaction, commandType, commandText, (SQLiteParameter[])null);
        }

        public static DataSet ExecuteDataset(SQLiteTransaction transaction, CommandType commandType, string commandText, params SQLiteParameter[] commandParameters)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters);
            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            cmd.Parameters.Clear();
            return ds;
        }
        #endregion ExecuteDataSet

        #region ExecuteDataTable
        public static DataTable ExecuteDataTable(string connectionString, CommandType commandType, string commandText)
        {
            return ExecuteDataTable(connectionString, commandType, commandText, (SQLiteParameter[])null);
        }
        public static DataTable ExecuteDataTable(string connectionString, CommandType commandType, string commandText, params SQLiteParameter[] commandParameters)
        {
            using (SQLiteConnection cn = new SQLiteConnection(connectionString))
            {
                cn.Open();
                return ExecuteDataTable(cn, commandType, commandText, commandParameters);
            }
        }

        public static DataTable ExecuteDataTable(SQLiteConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteDataTable(connection, commandType, commandText, (SQLiteParameter[])null);
        }

        public static DataTable ExecuteDataTable(SQLiteConnection connection, CommandType commandType, string commandText, params SQLiteParameter[] commandParameters)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            PrepareCommand(cmd, connection, (SQLiteTransaction)null, commandType, commandText, commandParameters);
            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cmd.Parameters.Clear();
            return dt;
        }
        public static DataTable ExecuteDataTable(SQLiteTransaction transaction, CommandType commandType, string commandText)
        {
            return ExecuteDataTable(transaction, commandType, commandText, (SQLiteParameter[])null);
        }

        public static DataTable ExecuteDataTable(SQLiteTransaction transaction, CommandType commandType, string commandText, params SQLiteParameter[] commandParameters)
        {
            //create a command and prepare it for execution
            SQLiteCommand cmd = new SQLiteCommand();
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters);
            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cmd.Parameters.Clear();
            return dt;
        }
        #endregion ExecuteDataTable

        #region ExecuteReader
        /// <summary>
        /// this enum is used to indicate whether the connection was provided by the caller, or created by SqlHelper, so that
        /// we can set the appropriate CommandBehavior when calling ExecuteReader()
        /// </summary>
        private enum SQLiteConnectionOwnership
        {
            /// <summary>Connection is owned and managed by SqlHelper</summary>
            Internal,
            /// <summary>Connection is owned and managed by the caller</summary>
            External
        }

        private static SQLiteDataReader ExecuteReader(SQLiteConnection connection, SQLiteTransaction transaction, CommandType commandType, string commandText, SQLiteParameter[] commandParameters, SQLiteConnectionOwnership connectionOwnership)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            PrepareCommand(cmd, connection, transaction, commandType, commandText, commandParameters);

            SQLiteDataReader dr;

            if (connectionOwnership == SQLiteConnectionOwnership.External)
            {
                dr = cmd.ExecuteReader();
            }
            else
            {
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            cmd.Parameters.Clear();

            return dr;
        }

        public static SQLiteDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText)
        {
            return ExecuteReader(connectionString, commandType, commandText, (SQLiteParameter[])null);
        }

        public static SQLiteDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText, params SQLiteParameter[] commandParameters)
        {
            SQLiteConnection cn = new SQLiteConnection(connectionString);
            cn.Open();
            try
            {
                return ExecuteReader(cn, null, commandType, commandText, commandParameters, SQLiteConnectionOwnership.Internal);
            }
            catch
            {
                cn.Close();
                throw;
            }
        }

        public static SQLiteDataReader ExecuteReader(SQLiteConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteReader(connection, commandType, commandText, (SQLiteParameter[])null);
        }

        public static SQLiteDataReader ExecuteReader(SQLiteConnection connection, CommandType commandType, string commandText, params SQLiteParameter[] commandParameters)
        {
            //pass through the call to the private overload using a null transaction value and an externally owned connection
            return ExecuteReader(connection, (SQLiteTransaction)null, commandType, commandText, commandParameters, SQLiteConnectionOwnership.External);
        }

        public static SQLiteDataReader ExecuteReader(SQLiteTransaction transaction, CommandType commandType, string commandText)
        {
            return ExecuteReader(transaction, commandType, commandText, (SQLiteParameter[])null);
        }

        public static SQLiteDataReader ExecuteReader(SQLiteTransaction transaction, CommandType commandType, string commandText, params SQLiteParameter[] commandParameters)
        {
            return ExecuteReader((SQLiteConnection)transaction.Connection, transaction, commandType, commandText, commandParameters, SQLiteConnectionOwnership.External);
        }

        #endregion ExecuteReader

        #region ExecuteScalar

        public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText)
        {
            return ExecuteScalar(connectionString, commandType, commandText, (SQLiteParameter[])null);
        }

        public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText, params SQLiteParameter[] commandParameters)
        {
            using (SQLiteConnection cn = new SQLiteConnection(connectionString))
            {
                cn.Open();
                return ExecuteScalar(cn, commandType, commandText, commandParameters);
            }
        }
        public static object ExecuteScalar(SQLiteConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteScalar(connection, commandType, commandText, (SQLiteParameter[])null);
        }

        public static object ExecuteScalar(SQLiteConnection connection, CommandType commandType, string commandText, params SQLiteParameter[] commandParameters)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            PrepareCommand(cmd, connection, (SQLiteTransaction)null, commandType, commandText, commandParameters);
            object retval = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return retval;
        }

        public static object ExecuteScalar(SQLiteTransaction transaction, CommandType commandType, string commandText)
        {
            return ExecuteScalar(transaction, commandType, commandText, (SQLiteParameter[])null);
        }

        public static object ExecuteScalar(SQLiteTransaction transaction, CommandType commandType, string commandText, params SQLiteParameter[] commandParameters)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters);
            object retval = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return retval;
        }
        #endregion ExecuteScalar
    }
}
