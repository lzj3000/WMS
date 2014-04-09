using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Data;
using Acc.Manage.DesginManager;
using Acc.Business.Model;

namespace Acc.Manage.Views
{
    public partial class Report_StockInOrder : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Button1_Click(null, null);
            } 
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            DataTable dt = SqlHelper.ExeAll("select * from Acc_View_StockInOrder " + GetSqlWhere());


            ReportViewer1.LocalReport.ReportPath = MapPath("..\\Report_rdlc\\Report_StockInOrder.rdlc");
            ReportViewer1.LocalReport.ReportEmbeddedResource = "..\\Report_rdlc\\Report_StockInOrder.rdlc";
         

            ReportDataSource rds = new ReportDataSource("DataSet1", dt);
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(rds);
            ReportViewer1.LocalReport.Refresh();

        }

        public string GetSqlWhere()
        {
            string strWhere = " where 1=1 ";
            if (WareHouse.Text != "")
            {
                strWhere += " and WAREHOUSENAME like '%" + WareHouse.Text + "%'"; 
            }
            if (Fname.Text != "")
                strWhere += " and fname like '%" + Fname.Text + "%'";
            if (Code.Text != "")
                strWhere += " and mcode like '%" + Code.Text + "%'";
            if (BeginDate.Text != "")
                strWhere += " and Creationdate >= '" + BeginDate.Text + "'";
            if (EndDate.Text != "")
                strWhere += " and Creationdate < '" + EndDate.Text + "'";
            return strWhere;
        }
    }
}