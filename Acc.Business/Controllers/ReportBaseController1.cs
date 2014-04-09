using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Contract.MVC;
using Acc.Contract.Data.ControllerData;
using Newtonsoft.Json;
using System.Xml;
using System.Reflection;
using System.Data;
using Acc.Business.Model;
using Way.EAP.DataAccess.Data;
using Way.EAP.DataAccess.Regulation;
using Way.EAP.DataAccess.Entity;
using System.Data.SqlClient;
using System.Data.Common;
using System.Configuration;

namespace Acc.Business.Controllers
{
    public class ReportBaseController : ControllerBase
    {

        #region RDLC私有变量
        private List<ReportDataSet> reportDataSets;
        private List<Tablix> tablixes;
        private List<ReportParameter> reportParameters;
        #endregion

        protected override ReadTable OnSearchData(loadItem item)
        {
            return base.OnSearchData(item);
        }

        //where条件
        private Dictionary<string, WhereParameter> whereParams;

        public ReportBaseController()
            : base(new OfficeWorker())
        {
           
          
        }
        

        [WhereParameter]
        public string rdlc { set; get; }

        /// <summary>
        /// 返回结果集
        /// 暂时只支持DataTabe,后期扩展为DataSet
        /// </summary>
        /// <param name="sqlSelect"></param>
        /// <returns></returns>
        public virtual DataTable GetTable(string tableName, List<SQLWhere> whereList)
        {
            string sqlSelect = "SELECT * FROM " + tableName;
            string regulationType = "Oracle";
            IDataAction dataAction = this.model.GetDataAction();
            if (((EntityBase)this.model).Regulation.ToString().Contains("SQLServerRegulation"))
            {
                regulationType = "";
            }
            sqlSelect = sqlSelect + GetWhereString(whereList, regulationType);
            return dataAction.GetDataTable(sqlSelect);
        }

        /// <summary>
        /// 获取Where参数列表
        /// 用于构建查询
        /// </summary>
        protected void GetWhereParams()
        {
            whereParams = new Dictionary<string, WhereParameter>();
            //获取公共实例属性       
            PropertyInfo[] properties = this.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            foreach (PropertyInfo property in properties)
            {
                switch (property.Name)
                {
                    case "ActionItem":
                    case "fdata":
                    case "LoadItem":
                    case "outc":
                    case "printIndex":
                    case "rdlc":
                        break;
                    default:
                        if (property.IsDefined(typeof(WhereParameter), true))
                        {
                            WhereParameter param = property.GetCustomAttributes(typeof(WhereParameter), false)[0] as WhereParameter;
                            if (string.IsNullOrEmpty(param.field))
                            {
                                param.field = property.Name;
                            }
                            whereParams.Add(param.field, param);
                        }
                        break;
                }
            }
        }

        /// <summary>
        ///解析RDLC
        ///暂时只支持表   矩形、图表待扩展
        /// </summary>
        /// <param name="xmlDoc"></param>
        protected void InitRdlc()
        {

            //获取和设置包含该应用程序的目录的名称。
            string stmp = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Reports\\Report_rdlc\\";

            reportDataSets = new List<ReportDataSet>();
            tablixes = new List<Tablix>();
            reportParameters = new List<ReportParameter>();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(stmp + rdlc);

            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
            nsmgr.AddNamespace("ab", "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition");
            nsmgr.AddNamespace("rd", "http://schemas.microsoft.com/SQLServer/reporting/reportdesigner");

            //解析DataSet
            XmlNodeList xnlDataSets = xmlDoc.GetElementsByTagName("DataSet");
            foreach (XmlNode ds in xnlDataSets)
            {
                ReportDataSet pds = new ReportDataSet();
                pds.Name = ds.Attributes["Name"].Value;
                XmlNodeList xnlFileds = ds.SelectNodes("ab:Fields/ab:Field", nsmgr);
                foreach (XmlNode fid in xnlFileds)
                {
                    ReportField rfid = new ReportField();
                    rfid.Visible = false;
                    foreach (XmlNode child in fid.ChildNodes)
                    {
                        if (child.Name.Equals("DataField", StringComparison.OrdinalIgnoreCase))
                        {
                            rfid.DataField = child.InnerText;
                        }
                        else if (child.Name.Equals("rd:TypeName", StringComparison.OrdinalIgnoreCase))
                        {
                            rfid.TypeName = child.InnerText;
                        }
                    }
                    pds.Fields.Add(rfid);
                }
                XmlNode tableNode = ds.SelectSingleNode("rd:DataSetInfo/rd:TableName", nsmgr);
                if (tableNode != null)
                {
                    pds.TableName = tableNode.InnerText;
                }
                reportDataSets.Add(pds);
            }

            //解析Tablix
            XmlNodeList xnlTablixes = xmlDoc.GetElementsByTagName("Tablix");
            foreach (XmlNode tbl in xnlTablixes)
            {
                Tablix tablix = new Tablix();
                tablix.Name = tbl.Attributes["Name"].Value;
                tablix.DataSetName = tbl.SelectSingleNode("ab:DataSetName", nsmgr).InnerText;

                ReportDataSet rds = reportDataSets.Select(r => r).Where(rd => rd.Name == tablix.DataSetName).FirstOrDefault();

                //暂时规定必须有两列 一列Title，一列内容
                XmlNodeList xnlRows = tbl.SelectNodes("ab:TablixBody/ab:TablixRows/ab:TablixRow", nsmgr);
                XmlNodeList titleCells = xnlRows[0].SelectNodes("ab:TablixCells/ab:TablixCell", nsmgr);
                XmlNodeList bodyCells = xnlRows[1].SelectNodes("ab:TablixCells/ab:TablixCell", nsmgr);

                List<TablixCell> rows = new List<TablixCell>();
                for (int i = 0; i < bodyCells.Count; i++)
                {
                    TablixCell cell = new TablixCell();
                    XmlNode titlecolumn = titleCells[i].SelectSingleNode("ab:CellContents/ab:Textbox/ab:Paragraphs/ab:Paragraph/ab:TextRuns/ab:TextRun/ab:Value", nsmgr);
                    XmlNode bodycolumn = bodyCells[i].SelectSingleNode("ab:CellContents/ab:Textbox", nsmgr);
                    //报错
                    XmlNodeList reportcolumn = bodyCells[i].SelectNodes("ab:CellContents/ab:Textbox/ab:ActionInfo/ab:Actions/ab:Action/ab:Drillthrough/ab:ReportName", nsmgr);
                    cell.Title = titlecolumn.InnerText;
                    cell.Name = bodycolumn.Attributes["Name"].Value;


                    ReportField filed = rds.Fields.Select(f => f).Where(fld => fld.DataField == cell.Name).FirstOrDefault();
                    if (filed != null)
                    {
                        filed.Title = cell.Title;
                        filed.Visible = true;
                    }
                    if (reportcolumn != null && reportcolumn.Count > 0)
                    {
                        cell.SubreportName = reportcolumn[0].InnerText;
                        XmlNodeList paramters = bodyCells[i].SelectNodes("ab:CellContents/ab:ActionInfo/ab:Actions/ab:Action/ab:Drillthrough/ab:ReportName/ab:Parameters/ab:Parameter", nsmgr);
                        foreach (XmlNode p in paramters)
                        {
                            ReportParameter rp = new ReportParameter();
                            rp.Name = p.Attributes["Name"].Value;
                            cell.Parameters.Add(rp);
                        }
                    }
                    rows.Add(cell);
                }
                tablix.TablixRow = rows;
                tablixes.Add(tablix);
            }

            //解析参数
            XmlNodeList xnlParams = xmlDoc.GetElementsByTagName("ReportParameter");
            foreach (XmlNode param in xnlParams)
            {
                ReportParameter rp = new ReportParameter();
                foreach (XmlNode c in param.ChildNodes)
                {
                    rp.Name = c.Name;
                    if (c.Name == "DataType")
                    {
                        rp.DataType = c.InnerText;
                    }
                    else if (c.Name == "Prompt")
                    {
                        rp.Prompt = c.InnerText;
                    }
                }
                reportParameters.Add(rp);
            }
        }

        /// <summary>
        ///前台调用
        /// </summary>
        /// <returns></returns>
        public string Getwheres()
        {
            GetWhereParams();
            InitRdlc();
            if (whereParams == null || whereParams.Count == 0)
            {
                ReportDataSetToWhere();
            }
            whereParams.Add("DataSetName", new WhereParameter(reportDataSets[0].Name));
            whereParams.Add("TableName", new WhereParameter(reportDataSets[0].TableName));

            string result = JsonConvert.SerializeObject(whereParams);
            return result;
        }

        /// <summary>
        /// 将DataSet转换成WhereParamter
        /// </summary>
        private void ReportDataSetToWhere()
        {
            foreach (ReportField field in reportDataSets[0].Fields)
            {
                if (field.Visible)
                {
                    WhereParameter where = new WhereParameter();
                    switch (field.TypeName)
                    {
                        case "System.Boolean":
                            where.wType = WhereParameter.WhereType.ComBox;
                            where.title = field.Title;
                            where.field = field.DataField;
                            where.comboxvalue = "[{ \"Text\": \"否\", \"Value\": 0 }, { \"Text\": \"是\", \"Value\": 1}]";
                            break;
                        case "System.DateTime":
                            where.wType = WhereParameter.WhereType.DateTime;
                            where.title = field.Title;
                            where.field = field.DataField;
                            break;
                        default:
                            where.wType = WhereParameter.WhereType.String;
                            where.title = field.Title;
                            where.field = field.DataField; break;
                    }
                    whereParams.Add(where.field, where);
                }
            }
        }

        /// <summary>
        /// 后台获取Where语句
        /// </summary>
        /// <param name="whereList"></param>
        /// <param name="sqlType"></param>
        /// <returns></returns>
        private string GetWhereString(List<SQLWhere> whereList, string sqlType)
        {
            StringBuilder sb = new StringBuilder(" where 1=1");
            if (whereList.Count > 0)
            {
                foreach (SQLWhere where in whereList)
                {
                    if (where.Symbol == "like")
                    {
                        where.Value = "'%" + where.Value + "%'";
                        sb.Append(" and " + where.where());
                    }
                    else if (where.Type == "int")
                    {
                        sb.Append(" and " + where.ColumnName + " " + where.Symbol + " " + where.Value);
                    }
                    else if (where.Type == "date" || where.Type == "datetime")
                    {
                        if (string.IsNullOrEmpty(sqlType))
                        {
                            sb.Append(" and " + where.where());
                        }
                        else
                        {
                            DateTime dt = DateTime.Parse(where.Value);
                            sb.Append(" and " + where.ColumnName + " " + where.Symbol + " to_date('" + dt.ToString("yyyy-MM-dd HH:mm:ss") + "','yyyy-mm-dd hh24:mi:ss')");
                        }
                    }
                }
            }
            return sb.ToString();
        }

        #region RDLC节点类

        public class ReportDataSet
        {
            public List<ReportField> Fields = new List<ReportField>();
            public string TableName { set; get; }
            public string Name { set; get; }
        }

        public class Tablix
        {
            public string Name { set; get; }
            public string DataSetName { set; get; }
            public List<TablixCell> TablixRow = new List<TablixCell>();
        }

        public class TablixCell
        {
            public string Name { set; get; }
            public string Title { set; get; }
            public string SubreportName { set; get; }
            public List<ReportParameter> Parameters = new List<ReportParameter>();
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

        public class ReportParameter
        {
            public string Name { set; get; }
            public string DataType { set; get; }
            public string Prompt { set; get; }
        }

        #endregion

        //[WhereParameter(defultvalue = "0", comboxvalue = "[{ \"Text\": \"否\", \"Value\": 0 }, { \"Text\": \"是\", \"Value\": 1}]", wType = WhereParameter.WhereType.ComBox, title = "是否提交", field = "IsSubmit")]
        //public string IsSubmit { get; set; }
    }
}
