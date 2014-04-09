using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace Acc.Manage.DesginManager
{
    public partial class Print : System.Web.UI.Page
    {
        public string barCodeImg = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            string tmpFile = Request.QueryString["tmpFile"];
            if (string.IsNullOrEmpty(tmpFile)) return;

            barCodeImg = Request.ApplicationPath + ConfigurationManager.AppSettings["barCodeTmpPath"] + "\\" + tmpFile + ".jpg";
        }
    }
}