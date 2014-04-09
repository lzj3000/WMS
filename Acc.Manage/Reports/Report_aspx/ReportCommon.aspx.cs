using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Acc.Business.Controllers;
using Acc.Contract.MVC;
using Microsoft.Reporting.WebForms;
using System.Data;
using System.Xml;
using Way.EAP.DataAccess.Regulation;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Acc.Manage.Reports.Report_aspx
{

    public partial class ReportCommon : System.Web.UI.Page
    {

        #region 私有变量
        private string tableName = string.Empty;
        private string dataSetName = string.Empty;
        private string wheres = string.Empty;
        private string rdlc = string.Empty;
        private string guid = string.Empty;
        private string controllerName = string.Empty;
        private ReportBaseController reportController;
        private List<SQLWhere> SQLWheres = new List<SQLWhere>();
        #endregion

        #region RDLC私有变量
        private List<ReportDataSet> reportDataSets = new List<ReportDataSet>();
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            guid = this.Request.QueryString["guid"];
            tableName = this.Request.QueryString["table"];
            dataSetName = this.Request.QueryString["dataset"];
            wheres = this.Request.QueryString["wheres"];
            if (!string.IsNullOrEmpty(wheres))
            {
                SQLWheres = JsonConvert.DeserializeObject<List<SQLWhere>>(wheres);
            }
            controllerName = this.Request.QueryString["c"];
            rdlc = this.Server.MapPath("..\\Report_rdlc\\" + this.Request.QueryString["rdlc"]);
            if (reportController == null)
            {
                if (string.IsNullOrEmpty(controllerName))
                {
                    reportController = new ReportBaseController();
                }
                else
                {
                    reportController = (ReportBaseController)ControllerCenter.GetCenter.GetController(controllerName, guid);
                }
            }
            if (!IsPostBack)
            {
                BindReportViewer(dataSetName, tableName, wheres);
            }
        }


        /// <summary>
        /// 绑定数据
        /// </summary>
        private void BindReportViewer(string dataset, string table, string sqlWhere)
        {
            ReportViewCommon.LocalReport.ReportPath = rdlc;
            ReportViewCommon.LocalReport.DataSources.Clear();
            ReportDataSource rds = new ReportDataSource(dataset, GetDataTable(table, SQLWheres));
            ReportViewCommon.LocalReport.DataSources.Add(rds);
            ReportViewCommon.LocalReport.Refresh();

        }

        /// <summary>
        /// 根据rdlc动态生成参数
        /// </summary>
        /// <param name="rdlcPath"></param>
        /// <returns></returns>
        private DataTable GetDataTable(string tableName, List<SQLWhere> whereList)
        {
            return reportController.GetTable(tableName, whereList);
        }

        /// <summary>
        /// 钻取子报表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ReportViewCommon_Drillthrough(object sender, DrillthroughEventArgs e)
        {
            rdlc = this.Server.MapPath("..\\Report_rdlc\\" + e.ReportPath + ".rdlc");
            LocalReport lr = (LocalReport)e.Report;

            IList<ReportParameter> ps = lr.OriginalParametersToDrillthrough;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(rdlc);

            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
            nsmgr.AddNamespace("ab", "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition");
            nsmgr.AddNamespace("rd", "http://schemas.microsoft.com/SQLServer/reporting/reportdesigner");

            //解析DataSet
            XmlNodeList xnlDataSets = xmlDoc.GetElementsByTagName("DataSet");
            foreach (XmlNode ds in xnlDataSets)
            {
                ReportDataSet pds = new ReportDataSet();
                pds.Name = ds.Attributes["Name"].Value;
                XmlNode tableNode = ds.SelectSingleNode("rd:DataSetInfo/rd:TableName", nsmgr);
                if (tableNode != null)
                {
                    pds.TableName = tableNode.InnerText;
                }
                //将参数添加到查询条件
                List<SQLWhere> wheres = new List<SQLWhere>();
                XmlNodeList filters = xmlDoc.GetElementsByTagName("Filter");
                foreach (XmlNode filter in filters)
                {
                    SQLWhere where = new SQLWhere();
                    XmlNode colum = filter.SelectSingleNode("ab:FilterExpression", nsmgr);
                    if (colum != null)
                    {
                        string pattern = "(!\\w+.)";
                        Match m = Regex.Match(colum.InnerText, pattern);
                        if (m.Success)
                        {
                            where.ColumnName = m.Value.Trim(new char[] { '!', '.' });
                        }
                        XmlNode reportParam = filter.SelectSingleNode("ab:FilterValues/ab:FilterValue", nsmgr);
                        Match m1 = Regex.Match(reportParam.InnerText, pattern);
                        if (m1.Success)
                        {
                            string result = m1.Value.Trim(new char[] { '!', '.' });
                            foreach (ReportParameter item in ps)
                            {
                                if (item.Name.Equals(result, StringComparison.OrdinalIgnoreCase))
                                {
                                    XmlNodeList fields = xmlDoc.GetElementsByTagName("Field");
                                    foreach (XmlNode field in fields)
                                    {
                                        XmlNode f = field.SelectSingleNode("ab:DataField", nsmgr);
                                        if (f.InnerText.Equals(where.ColumnName, StringComparison.OrdinalIgnoreCase))
                                        {
                                            XmlNode t = field.SelectSingleNode("rd:TypeName", nsmgr);
                                            if(t.InnerText.EndsWith("String"))
                                            {
                                                where.Symbol = "like";
                                            }
                                            else if (t.InnerText.EndsWith("DateTime"))
                                            {
                                                where.Type ="date";
                                            }
                                            else
                                            {
                                                where.Symbol = "=";
                                            }
                                        }
                                    }
                                    where.Value = item.Values[0];                                  
                                    wheres.Add(where);
                                    break;
                                }
                            }
                        }
                    }
                }
                lr.DataSources.Add(new ReportDataSource(pds.Name, GetDataTable(pds.TableName, wheres)));
            }
        }

        #region RDLC节点类

        public class ReportDataSet
        {
            public List<ReportField> Fields = new List<ReportField>();
            public string TableName { set; get; }
            public string Name { set; get; }
        }

        public class ReportField
        {
            /// <summary>
            /// 绑定字段
            /// </summary>
            public string DataField;
            /// <summary>
            /// 字段类型
            /// </summary>
            public string TypeName;
            /// <summary>
            /// 字段显示名称
            /// </summary>
            public string Title;
            /// <summary>
            /// 字段是否显示
            /// </summary>
            public bool Visible;
        }

        #endregion
    }
}
