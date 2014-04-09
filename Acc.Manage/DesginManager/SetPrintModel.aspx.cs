using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Acc.Manage.DesginManager;

namespace Acc.Manage
{
    public partial class SetPrintModel : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string controllername = Request["strController"].ToString().Trim();
                string strSQL = "select * from PrintTemplate";
                DataTable dt = SqlHelper.ExeAll(strSQL);
                DropDownList1.DataSource = dt.DefaultView;
                DropDownList1.DataValueField = dt.Columns["ID"].ColumnName;
                DropDownList1.DataTextField = dt.Columns["Name"].ColumnName;
                DropDownList1.DataBind();
                DropDownList1.Items.Insert(0, new ListItem("请选择...", "")); 

                strSQL = "select PrintTemplate.Name  from Acc_Bus_SetPrintModel left join PrintTemplate on Acc_Bus_SetPrintModel.PRINTTEMPLATEID=PrintTemplate.ID where Acc_Bus_SetPrintModel.CONTROLLERNAME='" + controllername + "'";
                dt = SqlHelper.ExeAll(strSQL);
                if (dt.Rows.Count>0)
                {
                    ListItem item = DropDownList1.Items.FindByText(dt.Rows[0][0].ToString());
                    if (item != null)
                    {
                        item.Selected = true;
                    }  
                } 
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string controllername = Request["strController"].ToString().Trim();
            string strSQL = "select * from Acc_Bus_SetPrintModel where CONTROLLERNAME='" + controllername + "'";
            DataTable dt = SqlHelper.ExeAll(strSQL);
            if (DropDownList1.Text != "")
            {
                if (dt.Rows.Count > 0)
                {
                    strSQL = " update  Acc_Bus_SetPrintModel set PRINTTEMPLATEID=" + DropDownList1.SelectedValue + " where CONTROLLERNAME='" + controllername + "'";
                }
                else
                {
                    strSQL = " insert into  Acc_Bus_SetPrintModel(PRINTTEMPLATEID,CONTROLLERNAME) values(" + DropDownList1.SelectedValue + ",'" + controllername + "')";
                }
                SqlHelper.ExeOnlyOne(strSQL);
                Response.Write("<script type='text/javascript'>alert(\"保存成功！\"); window.close();</script>");
            }
            else
                Label1.Text = "请选择打印模板！";

        }
    }
}