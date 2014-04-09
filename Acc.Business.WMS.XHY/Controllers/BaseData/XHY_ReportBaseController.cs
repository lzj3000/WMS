using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Business.Controllers;
using System.Data.SqlClient;
using System.Data;
using Way.EAP.DataAccess.Entity;
using Way.EAP.DataAccess.Data;

namespace Acc.Business.WMS.XHY.Controllers
{
    public class XHY_ReportBaseController : ReportBaseController
    {
        public override System.Data.DataTable GetTable(string tableName, List<Way.EAP.DataAccess.Regulation.SQLWhere> whereList)
        {
            DataTable dt = null;
            IDataAction dataAction = this.model.GetDataAction();
            if (tableName.Substring(0, 2) == "up")
            {
                string wh = string.Empty;
                string fname = string.Empty;
                string date = string.Empty;
                for (int i = 0; i < whereList.Count; i++)
                {
                    if (whereList[i].ColumnName.ToLower() == "riqi")
                    {
                        date = whereList[i].Value;
                    }
                    if (whereList[i].ColumnName.ToLower() == "wh")
                    {
                        wh = whereList[i].Value;
                    }
                }
                SqlParameter[] paras = new SqlParameter[] { new SqlParameter("@date", DbType.String) { Value = date }, new SqlParameter("@wh", DbType.String) { Value = wh } };
                dt = DbHelper.GetDataSet(tableName, paras);
            }
            else
            {
                string sqlSelect = "SELECT * FROM " + tableName;
                string regulationType = "Oracle";
                if (((EntityBase)this.model).Regulation.ToString().Contains("SQLServerRegulation"))
                {
                    regulationType = "";
                }
                sqlSelect = sqlSelect + base.GetWhereString(whereList, regulationType);
                dt = dataAction.GetDataTable(sqlSelect);
            }
            return dt;
        }
    }
}
