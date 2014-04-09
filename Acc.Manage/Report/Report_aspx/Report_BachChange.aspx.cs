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
using Way.EAP.DataAccess.Entity;
using Acc.Business.WMS.Model;
using Acc.Contract.MVC;
using Acc.Contract;
using Acc.Business.Controllers;
using Acc.Business.WMS.Controllers;


namespace Acc.Manage.Views
{
    public partial class Report_BachChange : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ControllerBase aa = new ControllerBase();
            
            
            if (!IsPostBack)
            {
                Button1_Click(null, null);
            } 
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            DataTable dt = SqlHelper.ExeAll("select * from Acc_WMS_BatchChangeView " + GetSqlWhere());


            ReportViewer1.LocalReport.ReportPath = MapPath("..\\Report_rdlc\\Report_BachChange.rdlc");
            ReportViewer1.LocalReport.ReportEmbeddedResource = "..\\Report_rdlc\\Report_BachChange.rdlc";
            

            ReportDataSource rds = new ReportDataSource("DataSet1", dt);
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(rds);
            ReportViewer1.LocalReport.Refresh();

        }

        public string GetSqlWhere()
        {

            //if (user.IsAdministrator)
            //{
            //    EntityList<WareHouse> listsq = new EntityList<WareHouse>(this.model.GetDataAction());
            //    listsq.GetData("id=" + WareHouse.Text);
            //    WareHouse ww = new WareHouse();
            //    ww = listsq[0];
            //    if (user.ID != ww.MANAGEID)
            //    {
            //        throw new Exception("您不是仓库管理员，没有更改库存数据的权限");
            //    }
            //}
            string strWhere = " where 1=1 ";
            if (WareHouse.Text != "")
            {
                strWhere += " and WAREHOUSENAME like '%" + WareHouse.Text + "%'"; 
            }
            if (Fname.Text != "")
                strWhere += " and FNAME like '%" + Fname.Text + "%'";
            if (Code.Text != "")
                strWhere += " and mcode like '%" + Code.Text + "%'";
            
            return strWhere;
        }
    }
}