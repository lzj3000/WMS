using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Xml;
using Acctrue.Equipment.Print;
using System.Data;
using System.Text;

namespace Acc.Manage.DesginManager
{
    public partial class PrintSet : System.Web.UI.Page
    {
      
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindPage();               
            }
        }

        private void BindPage()
        {
            this.DropDownList1.Items.Clear();
            this.DropDownList1.DataSource = new PrintTemplateDao().GetAll();
            this.DropDownList1.DataTextField = "Name";
            this.DropDownList1.DataValueField = "ID";
            this.DropDownList1.DataBind();
            if (Request["strController"] != null)
            {
                string strSQL = "select PrintTemplate.Name  from Acc_Bus_SetPrintModel left join PrintTemplate on Acc_Bus_SetPrintModel.PRINTTEMPLATEID=PrintTemplate.ID where Acc_Bus_SetPrintModel.CONTROLLERNAME='" + Request["strController"].ToString().Trim() + "'";
                DataTable dt = SqlHelper.ExeAll(strSQL);
                if (dt.Rows.Count > 0)
                {
                    ListItem item = DropDownList1.Items.FindByText(dt.Rows[0][0].ToString());
                    if (item != null)
                    {
                        item.Selected = true;
                    }
                }
            }
        }

        protected void btn_PP_Click(object sender, EventArgs e)
        {
            string strFunName = "PrinterParameter.GetProNoCodePrintParameter";// this.txtfunName.Text;
            string parameter = Request["selectPortsRows"].ToString().Trim();// this.txtparameter.Text;

            string[] StrParameterList = null;
            if (string.IsNullOrEmpty(strFunName))
            {
                throw new Exception("获取打印参数的方法不允许为空！");
            }

            if (!string.IsNullOrEmpty(parameter))
            {
                StrParameterList = parameter.Split(',');
            }

            printAX.PrintParameter = CreatePrintParameter(strFunName, StrParameterList);
            printAX.PrintDesigner = GetPrintDesigner();
            printAX.Print();
        }

        //利用反射产生获取打印参数的方法
        private PrintParameter CreatePrintParameter(string strFunName, string[] parameterList)
        {
            try
            {
                string[] strList = strFunName.Split('.');
                if (strList.Length != 2)
                {
                    throw new Exception("获取打印参数的方法格式不正确！");
                }
                string mapName = strList[0];
                string funName = strList[1];
                XmlDocument xmlDoc = new XmlDocument();
                string xmlPath = HttpContext.Current.Server.MapPath(@"~/SysFiles/Startup/Alias.xml");
                xmlDoc.Load(xmlPath);
                XmlNode mapNode = xmlDoc.SelectSingleNode(string.Format("/alias/add[@key='{0}']", mapName));
                if (mapNode == null)
                {
                    throw new Exception("未配置映射对照关系！");
                }
                string[] mapList = mapNode.Attributes["value"].Value.Split(',');
                string assemblyType = mapList[0];
                string assemblyName = mapList[1];
                object assembly = CreateInstance(assemblyName, assemblyType);
                if (assembly != null)
                {
                    Type myType = getMapType(assemblyName, assemblyType);
                    MethodInfo getParameterMethod = myType.GetMethod(funName);
                    if (parameterList != null && !string.IsNullOrEmpty(parameterList.ToString()) && parameterList.Length > 0)
                    {
                        return getParameterMethod.Invoke(assembly, parameterList) as PrintParameter;
                    }
                    else
                    {
                        return getParameterMethod.Invoke(assembly, null) as PrintParameter;
                    }
                }
                else
                {
                    throw new Exception("反射发生异常！");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private Acctrue.Equipment.Print.Template.PrintDesigner GetPrintDesigner()
        {
            //获取前台传送物料编码
            StringBuilder sb = new StringBuilder();
            string queryString = Request["selectPortsRows"];
            string[] ss1 = queryString.Split(';');
            for (int i = 0; i < ss1.Length; i++)
            {
                sb.Append(ss1[i].Split('|')[0] + ",");
            }
            Acctrue.Equipment.Print.Template.PrintDesigner saveObject = null;
            {
                if (this.DropDownList1.SelectedValue.Length > 0)
                {
                    byte[] pbytes = new PrintTemplateDao().GetObjectById(this.DropDownList1.SelectedValue).DesignData;
                    if (pbytes.Length > 0)
                    {
                        saveObject = new Acctrue.Equipment.Print.Template.PrintDesigner(pbytes);
                        saveObject.ItemList[0].ValueText = sb.Remove(sb.Length - 1, 1).ToString();
                    }
                }
            }
            return saveObject;

        }

        /// <summary>
        /// 反射实体对象
        /// </summary>
        /// <param name="assemblyName">程序集名</param>
        /// <param name="assemblyType">实体对象名</param>
        /// <returns></returns>
        public object CreateInstance(string assemblyName, string assemblyType)
        {
            Assembly asse = Assembly.Load(assemblyName);
            Type t = asse.GetType(assemblyType);
            object obj = Activator.CreateInstance(t);
            return obj;
        }

        public Type getMapType(string assemblyName, string assemblyType)
        {
            Assembly asse = Assembly.Load(assemblyName);
            return asse.GetType(assemblyType);
        }
    }
}