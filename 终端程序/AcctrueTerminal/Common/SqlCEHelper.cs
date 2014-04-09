using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlServerCe;
using System.Data;
using System.Data.Common;
using System.Xml;

namespace AcctrueTerminal.Common
{
    /// <summary>
    /// 描述：SQLCE类库
    /// 作者：张朝阳
    /// 创建日期:2013-08-02
    /// </summary>
    public class SqlCEHelper
    {
        private static void PrepareCommand(DbCommand command, DbConnection connection, DbTransaction transaction, CommandType commandType, string commandText, DbParameter[] commandParameters)
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

        private static void AttachParameters(DbCommand command, DbParameter[] commandParameters)
        {
            foreach (DbParameter p in commandParameters)
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
            return ExecuteNonQuery(connectionString, commandType, commandText, (SqlCeParameter[])null);
        }

        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, params SqlCeParameter[] commandParameters)
        {
            using (SqlCeConnection cn = new SqlCeConnection(connectionString))
            {
                cn.Open();
                return ExecuteNonQuery(cn, commandType, commandText, commandParameters);
            }
        }

        public static int ExecuteNonQuery(SqlCeConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteNonQuery(connection, commandType, commandText, (SqlCeParameter[])null);
        }

        public static int ExecuteNonQuery(SqlCeConnection connection, CommandType commandType, string commandText, params SqlCeParameter[] commandParameters)
        {
            SqlCeCommand cmd = new SqlCeCommand();
            PrepareCommand(cmd, connection, (SqlCeTransaction)null, commandType, commandText, commandParameters);
            int retval = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return retval;
        }
        public static int ExecuteNonQuery(SqlCeTransaction transaction, CommandType commandType, string commandText)
        {
            return ExecuteNonQuery(transaction, commandType, commandText, (SqlCeParameter[])null);
        }

        public static int ExecuteNonQuery(SqlCeTransaction transaction, CommandType commandType, string commandText, params SqlCeParameter[] commandParameters)
        {
            SqlCeCommand cmd = new SqlCeCommand();
            PrepareCommand(cmd,transaction.Connection,transaction, commandType,commandText,commandParameters);
            int retval = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return retval;
        }
        #endregion ExecuteNonQuery

        #region ExecuteDataSet

        public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText)
        {
            return ExecuteDataset(connectionString, commandType, commandText, (SqlCeParameter[])null);
        }

        public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText, params SqlCeParameter[] commandParameters)
        {
            using (SqlCeConnection cn = new SqlCeConnection(connectionString))
            {
                cn.Open();
                return ExecuteDataset(cn, commandType, commandText, commandParameters);
            }
        }

        public static DataSet ExecuteDataset(SqlCeConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteDataset(connection, commandType, commandText, (SqlCeParameter[])null);
        }

        public static DataSet ExecuteDataset(SqlCeConnection connection, CommandType commandType, string commandText, params SqlCeParameter[] commandParameters)
        {
            SqlCeCommand cmd = new SqlCeCommand();
            PrepareCommand(cmd, connection, (SqlCeTransaction)null, commandType, commandText, commandParameters);
            SqlCeDataAdapter da = new SqlCeDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);		
            cmd.Parameters.Clear();
            return ds;
        }

        public static DataSet ExecuteDataset(SqlCeTransaction transaction, CommandType commandType, string commandText)
        {
            return ExecuteDataset(transaction, commandType, commandText, (SqlCeParameter[])null);
        }

        public static DataSet ExecuteDataset(SqlCeTransaction transaction, CommandType commandType, string commandText, params SqlCeParameter[] commandParameters)
        {
            SqlCeCommand cmd = new SqlCeCommand();
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters);
            SqlCeDataAdapter da = new SqlCeDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            cmd.Parameters.Clear();
            return ds;
        }
        #endregion ExecuteDataSet

        #region ExecuteDataTable
        public static DataTable ExecuteDataTable(string connectionString, CommandType commandType, string commandText)
        {
            return ExecuteDataTable(connectionString, commandType, commandText, (SqlCeParameter[])null);
        }
        public static DataTable ExecuteDataTable(string connectionString, CommandType commandType, string commandText, params SqlCeParameter[] commandParameters)
        {
            using (SqlCeConnection cn = new SqlCeConnection(connectionString))
            {
                cn.Open();
                return ExecuteDataTable(cn, commandType, commandText, commandParameters);
            }
        }

        public static DataTable ExecuteDataTable(SqlCeConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteDataTable(connection, commandType, commandText, (SqlCeParameter[])null);
        }

        public static DataTable ExecuteDataTable(SqlCeConnection connection, CommandType commandType, string commandText, params SqlCeParameter[] commandParameters)
        {
            SqlCeCommand cmd = new SqlCeCommand();
            PrepareCommand(cmd, connection, (SqlCeTransaction)null, commandType, commandText, commandParameters);
            SqlCeDataAdapter da = new SqlCeDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);	
            cmd.Parameters.Clear();
            return dt;
        }
        public static DataTable ExecuteDataTable(SqlCeTransaction transaction, CommandType commandType, string commandText)
        {
            return ExecuteDataTable(transaction, commandType, commandText, (SqlCeParameter[])null);
        }

        public static DataTable ExecuteDataTable(SqlCeTransaction transaction, CommandType commandType, string commandText, params SqlCeParameter[] commandParameters)
        {
            //create a command and prepare it for execution
            SqlCeCommand cmd = new SqlCeCommand();
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters);
            SqlCeDataAdapter da = new SqlCeDataAdapter(cmd);
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
        private enum SqlCeConnectionOwnership
        {
            /// <summary>Connection is owned and managed by SqlHelper</summary>
            Internal,
            /// <summary>Connection is owned and managed by the caller</summary>
            External
        }

        private static SqlCeDataReader ExecuteReader(SqlCeConnection connection, SqlCeTransaction transaction, CommandType commandType, string commandText, SqlCeParameter[] commandParameters, SqlCeConnectionOwnership connectionOwnership)
        {
            SqlCeCommand cmd = new SqlCeCommand();
            PrepareCommand(cmd, connection, transaction, commandType, commandText, commandParameters);

            SqlCeDataReader dr;

            if (connectionOwnership == SqlCeConnectionOwnership.External)
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

        public static SqlCeDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText)
        {
            return ExecuteReader(connectionString, commandType, commandText, (SqlCeParameter[])null);
        }

        public static SqlCeDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText, params SqlCeParameter[] commandParameters)
        {
            SqlCeConnection cn = new SqlCeConnection(connectionString);
            cn.Open();
            try
            {
                return ExecuteReader(cn, null, commandType, commandText, commandParameters, SqlCeConnectionOwnership.Internal);
            }
            catch
            {
                cn.Close();
                throw;
            }
        }

        public static SqlCeDataReader ExecuteReader(SqlCeConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteReader(connection, commandType, commandText, (SqlCeParameter[])null);
        }

        public static SqlCeDataReader ExecuteReader(SqlCeConnection connection, CommandType commandType, string commandText, params SqlCeParameter[] commandParameters)
        {
            //pass through the call to the private overload using a null transaction value and an externally owned connection
            return ExecuteReader(connection, (SqlCeTransaction)null, commandType, commandText, commandParameters, SqlCeConnectionOwnership.External);
        }

        public static SqlCeDataReader ExecuteReader(SqlCeTransaction transaction, CommandType commandType, string commandText)
        {
            return ExecuteReader(transaction, commandType, commandText, (SqlCeParameter[])null);
        }

        public static SqlCeDataReader ExecuteReader(SqlCeTransaction transaction, CommandType commandType, string commandText, params SqlCeParameter[] commandParameters)
        {
            return ExecuteReader((SqlCeConnection)transaction.Connection, transaction, commandType, commandText, commandParameters, SqlCeConnectionOwnership.External);
        }

        #endregion ExecuteReader

        #region ExecuteScalar

        public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText)
        {
            return ExecuteScalar(connectionString, commandType, commandText, (SqlCeParameter[])null);
        }

        public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText, params SqlCeParameter[] commandParameters)
        {
            using (SqlCeConnection cn = new SqlCeConnection(connectionString))
            {
                cn.Open();
                return ExecuteScalar(cn, commandType, commandText, commandParameters);
            }
        }
        public static object ExecuteScalar(SqlCeConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteScalar(connection, commandType, commandText, (SqlCeParameter[])null);
        }

        public static object ExecuteScalar(SqlCeConnection connection, CommandType commandType, string commandText, params SqlCeParameter[] commandParameters)
        {
            SqlCeCommand cmd = new SqlCeCommand();
            PrepareCommand(cmd, connection, (SqlCeTransaction)null, commandType, commandText, commandParameters);
            object retval = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return retval;
        }

        public static object ExecuteScalar(SqlCeTransaction transaction, CommandType commandType, string commandText)
        {
            return ExecuteScalar(transaction, commandType, commandText, (SqlCeParameter[])null);
        }

        public static object ExecuteScalar(SqlCeTransaction transaction, CommandType commandType, string commandText, params SqlCeParameter[] commandParameters)
        {
            SqlCeCommand cmd = new SqlCeCommand();
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters);
            object retval = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return retval;
        }
        #endregion ExecuteScalar

    }
}
