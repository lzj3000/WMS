using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Common;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace Acc.Business
{
    public class DbHelper
    {

        public static string URL = ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString;

        /// <summary>
        /// 根据存储过程和存储过程参数返回一个SqlDataReader对象
        /// </summary>
        /// <param name="value">这个参数是放存储过程名的</param>
        /// <param name="para">这个参数是放存储过程参数的</param>
        /// <returns>返回一个SqlDataReader对象</returns>
        public static SqlDataReader ExecuteReader(string value, SqlParameter[] para)
        {
            SqlConnection conn = new SqlConnection(URL);
            SqlCommand com = new SqlCommand(value, conn);
            com.CommandType = CommandType.StoredProcedure;
            if (para != null)
            {
                com.Parameters.AddRange(para);
            }
            conn.Open();
            SqlDataReader dr = com.ExecuteReader(CommandBehavior.CloseConnection);
                        return dr;
        }

        public static DataTable GetDataSet(string sql, params SqlParameter[] values)
        {
            DataSet ds = new DataSet();
            SqlConnection conn = new SqlConnection(URL);
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddRange(values);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            return ds.Tables[0];
        }

       

        /// <summary>
        /// 根据存储过程执行插入，删除，更改返回受影响行数
        /// </summary>
        /// <param name="values">这个参数是放存储过程名的</param>
        /// <param name="para">这个参数是放存储过程参数的</param>
        /// <returns>返回受影响行数</returns>
        public static int ExecuteNonQuery(string values, SqlParameter[] para)
        {
            int num = 0;
            using (SqlConnection con = new SqlConnection(URL))
            {
                con.Open();
                using (SqlCommand com = new SqlCommand(values, con))
                {
                    if (para != null)
                    {
                        com.Parameters.AddRange(para);
                        num = com.ExecuteNonQuery();
                    }
                }
            }
            return num;
        }

        /// <summary>
        /// 根据存储过程执行登陆返回受影响行数
        /// </summary>
        /// <param name="value">这个参数是放存储过程名的</param>
        /// <param name="para">这个参数是放存储过程参数的</param>
        /// <returns>返回受影响行数</returns>
        public static int ExecuteScalar(string value, SqlParameter[] para)
        {
            int num = 0;
            using (SqlConnection con = new SqlConnection(URL))
            {
                using (SqlCommand com = new SqlCommand(value, con))
                {
                    com.CommandType = CommandType.StoredProcedure;
                    if (para != null)
                    {
                        com.Parameters.AddRange(para);
                        con.Open();
                        num = Convert.ToInt32(com.ExecuteScalar());
                    }
                }
            }
            return num;
        }
    }
}