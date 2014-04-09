using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Acc.Business.WMS.Model;
using Acc.Manage.DesginManager;


namespace AcctrueWMS.Web
{
    public partial class SaleList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {


            if (!Page.IsPostBack)
            {

                if (Request.QueryString["ID"] != string.Empty)
                {
                    DataTable dt = BindGV();

                    this.GridView1.DataSource = BindGV1();
                    this.GridView1.DataBind();
                }



            }
        }
        private DataTable BindGV1()
        {
            string xsid = Request.QueryString["ID"].ToString();
            DataTable Sourcedt = SqlHelper.ExeAll("SELECT om.MCODE,bc.FNAME,om.FMODEL,om.STAY5 bz,'KG' dw,SUM(om.STAY7) num,SUM(NUM) jian FROM dbo.Acc_WMS_OutOrderMaterials om INNER JOIN dbo.Acc_Bus_BusinessCommodity bc ON bc.ID= om.MATERIALCODE WHERE PARENTID = '" + xsid + "' GROUP BY om.MCODE,bc.FNAME,om.FMODEL,om.STAY5");
            return Sourcedt;
        }
      
        public string khlx, code, warehousename, customername, lxrname, lxrphone, lxraddress, zdkh
, zdlxr, zdtel, zdphone, zdaddress, logisticsname, yf, creationdate, remark, ywdw,jhfs = "";
        private DataTable BindGV()
        {
            string xsid = Request.QueryString["ID"].ToString();
            DataTable Sourcedt = SqlHelper.ExeAll("SELECT oo.khlx,oo.Code,wh.warehousename,bc.CUSTOMERNAME,oo.LXRName,oo.LXRPHONE,oo.LXRADDRESS,bc1.CUSTOMERNAME zdkh,oo.zdlxr,oo.zdtel,oo.zdphone,oo.zdaddress,lif.LOGISTICSNAME,oo.yf,CONVERT(varchar(100), oo.Creationdate, 102) Creationdate,oo.Remark,oo.ywdw,oo.jhfs FROM dbo.Acc_WMS_OutOrder oo left JOIN dbo.Acc_WMS_WareHouse wh ON wh.ID = oo.TOWHNO left JOIN dbo.Acc_Bus_BusinessCustomer bc ON bc.ID = oo.CLIENTNO left JOIN dbo.Acc_Bus_BusinessCustomer bc1 ON bc1.ID = oo.zdkh left JOIN dbo.Acc_WMS_LogisticsInfo lif ON lif.ID= oo.LogCode where oo.id='" + xsid + "'");
            if (Sourcedt.Rows.Count <= 0)
            {
                throw new Exception("登录超时请重新登录！");
            }
           
           
            khlx = Sourcedt.Rows[0]["khlx"].ToString();
            code=Sourcedt.Rows[0]["code"].ToString();
            warehousename = Sourcedt.Rows[0]["warehousename"].ToString();
            customername = Sourcedt.Rows[0]["customername"].ToString();
            lxrname = Sourcedt.Rows[0]["lxrname"].ToString();
            lxrphone = Sourcedt.Rows[0]["lxrphone"].ToString();
            lxraddress = Sourcedt.Rows[0]["lxraddress"].ToString();
            zdkh = Sourcedt.Rows[0]["zdkh"].ToString();
            zdlxr = Sourcedt.Rows[0]["zdlxr"].ToString();
            zdtel = Sourcedt.Rows[0]["zdtel"].ToString();
            zdphone = Sourcedt.Rows[0]["zdphone"].ToString();
            zdaddress = Sourcedt.Rows[0]["zdaddress"].ToString();
            logisticsname = Sourcedt.Rows[0]["logisticsname"].ToString();
            yf = Sourcedt.Rows[0]["yf"].ToString();
            creationdate = Sourcedt.Rows[0]["creationdate"].ToString();
            remark = Sourcedt.Rows[0]["remark"].ToString();
            ywdw = Sourcedt.Rows[0]["ywdw"].ToString();
            jhfs = Sourcedt.Rows[0]["jhfs"].ToString();
          
            return Sourcedt;
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}