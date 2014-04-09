using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace Acc.Manage.DesginManager
{
    public class SqlHelper
    {
        public static string cons = ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString;
        /// <summary>
        /// one
        /// </summary>
        /// <param name="sqlcmd">sql</param>
        /// <returns>object</returns>
        public static object ExeOnlyOne(string sqlcmd)
        {
            object oRs = null;
            using(SqlConnection con = new SqlConnection())
            {
                con.ConnectionString = cons;
                SqlCommand cmd = new SqlCommand(sqlcmd, con);
                con.Open();
                oRs = cmd.ExecuteScalar();
                con.Close();
            }
            return oRs;
        }
        public static object ExeOnlyOne(string sqlcmd,params SqlParameter[] pars)
        {
            object oRs = null;
            using (SqlConnection con = new SqlConnection())
            {
                con.ConnectionString = cons;
                SqlCommand cmd = new SqlCommand(sqlcmd, con);
                cmd.Parameters.AddRange(pars);
                con.Open();
                oRs = cmd.ExecuteScalar();
                con.Close();
            }
            return oRs;
        }

        /// <summary>
        /// insert update delete
        /// </summary>
        /// <param name="sqlcmd">sql</param>
        /// <returns>int</returns>
        public static int ExeCUD(string sqlcmd)
        {
            int iRs = -1;
            using (SqlConnection con = new SqlConnection())
            {
                con.ConnectionString = cons;
                SqlCommand cmd = new SqlCommand(sqlcmd, con);
                con.Open();
                iRs = cmd.ExecuteNonQuery();
                con.Close();
            }
            return iRs;
        }
        public static int ExeCUD(string sqlcmd,params SqlParameter[] pars)
        {
            int iRs = -1;
            using (SqlConnection con = new SqlConnection())
            {
                con.ConnectionString = cons;
                SqlCommand cmd = new SqlCommand(sqlcmd, con);
                cmd.Parameters.AddRange(pars);
                con.Open();
                iRs = cmd.ExecuteNonQuery();
                con.Close();
            }
            return iRs;
        }

        /// <summary>
        /// all
        /// </summary>
        /// <param name="sqlcmd">sql</param>
        /// <returns>DataTable</returns>
        public static DataTable ExeAll(string sqlcmd)
        {
            DataTable dt = null;
            using (SqlConnection con = new SqlConnection())
            {
                con.ConnectionString = cons;
                SqlDataAdapter sda = new SqlDataAdapter(sqlcmd,con);
                DataSet ds = new DataSet();
                sda.Fill(ds);
                dt = ds.Tables[0];
            }
            return dt;
        }
        public static DataTable ExeAll(string sqlcmd,params SqlParameter[] pars)
        {
            DataTable dt = null;
            using (SqlConnection con = new SqlConnection())
            {
                con.ConnectionString = cons;
                SqlDataAdapter sda = new SqlDataAdapter(sqlcmd, con);
                sda.SelectCommand.Parameters.AddRange(pars);
                DataSet ds = new DataSet();
                sda.Fill(ds);
                dt = ds.Tables[0];
            }
            return dt;
        }

        /// <summary>
        /// all
        /// </summary>
        /// <param name="sqlcmd">sql</param>
        /// <returns>SqlDataReader</returns>
        public static SqlDataReader ExeReader(string sqlcmd)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = cons;
            SqlCommand cmd = new SqlCommand(sqlcmd, con);
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            return sdr;
        }
        public static SqlDataReader ExeReader(string sqlcmd,params SqlParameter[] pars)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = cons;
            SqlCommand cmd = new SqlCommand(sqlcmd, con);
            cmd.Parameters.AddRange(pars);
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            return sdr;
        }
    }
}