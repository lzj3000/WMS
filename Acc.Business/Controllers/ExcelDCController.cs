using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Contract.Data;
using System.Data;
using System.IO;
using System.Web;
using Acc.Business.Controllers;
using Acc.Business.Model;
using System.Reflection;
using Way.EAP.DataAccess.Data;
using Way.EAP.DataAccess.Entity;
using Acc.Contract;
using Acc.Contract.Data.ControllerData;
using Acc.Contract.MVC;
using System.Xml;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Collections;

namespace Acc.Business.Controllers
{
    public class ExcelDCController : BusinessController
    {
        public ExcelDCController() : base(new BusinessBase()) { }
        protected override string OnGetPath()
        {
            return "Views/manager/Exceldc.htm";
        }
        protected override string OnControllerName()
        {
            return "Excel导出";
        }
        public BusinessController bc;
        #region 导出功能
        [WhereParameter]
        public string pagerow { get; set; }
        [WhereParameter]
        public string EOperate { get; set; }
        [WhereParameter]
        public string CurrentPage { get; set; }
        [WhereParameter]
        public string ExportType { get; set; }
        [WhereParameter]
        public string Zibiao { get; set; }
        [WhereParameter]
        public string  IsEmportZZ { get; set; }
        [WhereParameter]
        public string Controllername { get; set; }
        [WhereParameter]
        public string Page{ get; set; }
         [WhereParameter]
         public string   Rownum { get; set; }
         [WhereParameter]
         public string Sort { get; set; }
         [WhereParameter]
         public string Title { get; set; }
        public string Emport()
        {
            //根据页面传来值获得loadItem、控制器
            loadItem loaditem = new loadItem();
            loaditem.page = Page;
            loaditem.rows = Rownum;
            loaditem.sort = Sort;
            if (loaditem == null)
                return null;
            bc = (BusinessController)ControllerCenter.GetCenter.GetController(Controllername, this.user.tempid);
            //获取页数
            if (EOperate == "1")
            {
                string prePage = loaditem.page;
                EntityBase eb = bc.model as EntityBase;
                loaditem.page = "-1";
                //根据控制器实体获得查询sql
                string sql=((EntityBase)eb).Regulation.GetSearchSQL(eb);
                loaditem.rowsql = sql;
                IDataAction action = this.model.GetDataAction();
                DataTable t = action.GetDataTable(sql);
                //根据sql查询得到数据，按照pagerow算出分几个excel文件,返回页面
                int pagecount = t.Rows.Count / Convert.ToInt32(pagerow);
                if (t.Rows.Count % Convert.ToInt32(pagerow) > 0)
                    pagecount++;
                loaditem.page = prePage;
                return pagecount.ToString();
            }
            //导出数据
            else if (EOperate == "0")
            {
                string prePage = loaditem.page;
                string preRows = loaditem.rows;
                DataTable dt = GetData(CurrentPage, bc, loaditem);
                String wj = getWJ(Zibiao,bc);
                if (dt != null && dt.Rows.Count > 0)
                {
                    WebExport1.Export(Title + "(-" + loaditem.page + ")", ExportType, dt, Zibiao, wj, bc, selectFKey(), IsEmportZZ);
                }
                loaditem.rows = preRows;
                loaditem.page = prePage;
            }
            return null;
        }
        /// <summary>
        /// 获取外键
        /// </summary>
        /// <param name="zibiao">子集表名</param>
        /// <param name="bc">控制器</param>
        /// <returns></returns>
        public string getWJ(string zibiao,BusinessController bc)
        {
            if (IsEmportZZ.Equals("1"))
            {
                BasicInfo info = bc.model as BasicInfo;
                IEntityBase ieb = (IEntityBase)info;
                Dictionary<string, EntityFieldAttribute> fields = ieb.GetField();
                string auto = "";
                foreach (KeyValuePair<string, EntityFieldAttribute> kv in fields)
                {
                    if (kv.Value != null && kv.Value.IsIdentity)
                        auto = kv.Key;
                }
                IHierarchicalEntityView[] hev = ieb.GetChildEntityList();
                foreach (IHierarchicalEntityView v in hev)
                {

                    EntityBase eb = v.ChildEntity;
                    if (zibiao.Equals(eb.ToString()))
                    {
                        string rel = "";
                        Dictionary<string, EntityForeignKeyAttribute> items = ((IEntityBase)eb).GetForeignKey();
                        foreach (KeyValuePair<string, EntityForeignKeyAttribute> kv in items)
                        {
                            if (kv.Value != null)
                            {
                                if (kv.Value.FieldType.Equals(v.ParentEntity.GetType()) && kv.Value.IsChildAssociate)
                                {
                                    if (kv.Value.FieldName.Equals(auto, StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        return rel = v.ChildEntity.Regulation.GetDataBaseColumnName(kv.Key);
                                    }
                                }
                            }
                        }
                    }
                }
                return "";
            }
            else {
                return "";
            }
        }
        public DataTable GetData(string currentPage, BusinessController bc, loadItem loaditem)
        {
            EntityBase eb = bc.model as EntityBase;
            IDataAction action = this.model.GetDataAction();
            //获取当前页数据
            if (currentPage != "-1")
            {
                loaditem.page = (int.Parse(currentPage) + 1).ToString();
                loaditem.rows = pagerow.ToString();
            }
            
            //string sql = ((EntityBase)eb).Regulation.GetSearchSQL(eb, int.Parse(loaditem.page), int.Parse(loaditem.rows), loaditem.sort);

            bc.LoadItem = loaditem;
            ReadTable ttt = bc.SearchData(loaditem);
            //DataTable dts = ttt.rows;
            Dictionary<string, EntityForeignKeyAttribute> fy;
            fy = getST_WJ_items();
            DataTable nt = new DataTable();
            nt = ttt.rows.Copy();
            foreach (KeyValuePair<string, EntityForeignKeyAttribute> kv in fy)
            {
                string fn = eb.GetAppendPropertyKey(kv.Key);
                nt.Columns.Remove(kv.Key);
                nt.Columns[fn].ColumnName = kv.Key;
            }
            DataTable dtc = nt.Clone();
            return getControlfield(nt, dtc, "", bc);

        }
        /// <summary>
        /// 设置主表与子表的外键字段
        /// </summary>
        /// <returns></returns>
        protected virtual string selectFKey()
        {
            return "ID";
        }
        #endregion
        public DataTable getControlfield(DataTable dt, DataTable dtc, string zibiao, BusinessController bc)
        {
            IDataAction action = this.model.GetDataAction();
            DataTable result = null;
            Dictionary<string, string> dicColumns = new Dictionary<string, string>();
            if (zibiao.Equals(""))
            {
                foreach (ItemData idata in bc.Idata.modeldata.childitem)
                {
                    if (idata.visible)
                    {
                        dicColumns.Add(idata.field, idata.title);
                    }
                }
                dicColumns=Update_DC_Main_Field(dicColumns);
                dicColumns.Add(selectFKey(), selectFKey());
            }
            else
            {
                for (int m = 0; m < bc.Idata.modeldata.childmodel.Length; m++)
                {
                    if (bc.Idata.modeldata.childmodel[m].tablename == zibiao)
                    {
                        foreach (ItemData idata in bc.Idata.modeldata.childmodel[m].childitem)
                        {
                            if (idata.visible)
                            {
                                dicColumns.Add(idata.field, idata.title);
                            }
                        }
                          dicColumns=Update_DC_Part_Field(dicColumns);
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            //重命名列名
            for (int i = 0; i < dtc.Columns.Count; i++)
            {
                string cname = dtc.Columns[i].ColumnName;
                if (dicColumns.ContainsKey(cname.ToUpper()))
                {
                    dt.Columns[i].ColumnName = dicColumns[cname.ToUpper()];
                }
            }
            //移除多余的列
            for (int i = 0; i < dtc.Columns.Count; i++)
            {
                string cname = dtc.Columns[i].ColumnName;
                if (!dicColumns.ContainsKey(cname.ToUpper()))
                {
                    dt.Columns.Remove(cname.ToUpper());
                }
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                result = dt;
            }
            return dt;

        }
        /// <summary>
        /// 主集的外键
        /// </summary>
        /// <returns></returns>
        public virtual Dictionary<string, EntityForeignKeyAttribute> getST_WJ_items()
        {
            BasicInfo info = bc.model as BasicInfo;
            IEntityBase ieb = (IEntityBase)info;
            Dictionary<string, EntityForeignKeyAttribute> itemss = ((IEntityBase)ieb).GetForeignKey();
            return itemss;
        }
        /// <summary>
        /// 子集的外键
        /// </summary>
        /// <returns></returns>
        public virtual Dictionary<string, EntityForeignKeyAttribute> getST_WJ_Main(BusinessController bc, string zibiao)
        {
            BasicInfo info = bc.model as BasicInfo;
            IEntityBase ieb = (IEntityBase)info;
            IHierarchicalEntityView[] hev = ieb.GetChildEntityList();
            foreach (IHierarchicalEntityView v in hev)
            {

                EntityBase eb = v.ChildEntity;
                if (zibiao.Equals(eb.ToString()))
                {
                    return ((IEntityBase)eb).GetForeignKey();
                }
            }
            return null;
        }
        /// <summary>
        /// 导出的主集字段
        /// </summary>
        /// <param name="dicColumns"></param>
        /// <returns></returns>
        protected Dictionary<string, string> Update_DC_Main_Field(Dictionary<string, string> dicColumns)
        {
            return dicColumns;
        }
        /// <summary>
        /// 导出的子集字段
        /// </summary>
        /// <param name="dicColumns"></param>
        /// <returns></returns>
        protected Dictionary<string, string> Update_DC_Part_Field(Dictionary<string, string> dicColumns)
        {
            return dicColumns;
        } 
        public DataTable getZiji(string sql, string zibiao,BusinessController bc)
        {
            EntityBase eb = null;
            string sqlzi = "";
            BasicInfo info = bc.model as BasicInfo;
            IEntityBase ieb = (IEntityBase)info;
            IHierarchicalEntityView[] hev = ieb.GetChildEntityList();
            foreach (IHierarchicalEntityView v in hev)
            {
                eb = v.ChildEntity;
                if (zibiao.Equals(eb.ToString()))
                {
                    //list = v.GetEntityList();
                    //list.DataAction = action;
                    //list.GetData(sql);

                    sqlzi = ((IModel)v.ChildEntity).GetSearchSQL()+"     where  "+sql;
                    break;
                }
            }
            //if (list.Count > 0)
            //{
            //    DataTable dt = ToDataTable(list);//action.GetDataTable(sql);
            //    Dictionary<string, EntityForeignKeyAttribute> fy;
            //    fy = getST_WJ_Main(bc, zibiao);
            //    foreach (KeyValuePair<string, EntityForeignKeyAttribute> kv in fy)
            //    {
            //        string fn = eb.GetAppendPropertyKey(kv.Key);
            //        dt.Columns.Remove(kv.Key);
            //        dt.Columns[fn].ColumnName = kv.Key;
            //    }
            //    DataTable dtc = dt.Clone();

            //    return getControlfield(dt, dtc, zibiao, bc);
            //}
            //else
            //{
            //    return null;
            //}
            IDataAction action = this.model.GetDataAction();
            DataTable dt = action.GetDataTable(sqlzi);
            Dictionary<string, EntityForeignKeyAttribute> fy;
            fy = getST_WJ_Main(bc, zibiao);
            foreach (KeyValuePair<string, EntityForeignKeyAttribute> kv in fy)
            {
                string fn = eb.GetAppendPropertyKey(kv.Key);
                dt.Columns.Remove(kv.Key);
                dt.Columns[fn].ColumnName = kv.Key;
            }
            DataTable dtc = dt.Clone();
            return getControlfield(dt, dtc, zibiao, bc);
        }
        public static DataTable ToDataTable(IList list)
        {
            DataTable result = new DataTable();
            if (list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    result.Columns.Add(pi.Name, pi.PropertyType);
                }

                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        object obj = pi.GetValue(list[i], null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }
    }

  /// <summary>
  /// Web文件导出类
  /// </summary>
  public static class WebExport1
  {
      public static void Export(string filename, string exportTarget, DataTable dt, string zibiao, string wj, BusinessController bc, string fwj, string IsEmportZZ)
      {
          string exportType = exportTarget.ToUpper();
          string fullName;
          //获取扩展名
          fullName = string.Concat(filename, ".", exportType);
          //写入流
          switch (exportType)
          {
              case "XLS":
                  ExportToXls(dt, fullName, zibiao, wj, bc, fwj, IsEmportZZ);
                  break;
              case "XML":
                  ExportToXml(dt, fullName, zibiao, wj, bc, fwj, IsEmportZZ);
                  break;
          }
      }
      private static void ExportToXls(DataTable dt, string fullName, string zibiao, string wj, BusinessController bc, string fwj, string IsEmportZZ)
      {
          #region 方法1拼接字符串
          //string excelStr = "";
          //if (zibiao != null && !wj.Equals(""))
          //{
          //    //添加列名
          //    foreach (DataColumn dr in dt.Columns)
          //    {
          //        excelStr += dr.ColumnName + "\t";
          //    }
          //    excelStr += "\n";
          //    //添加记录
          //    for (int i = 0; i < dt.Rows.Count; i++)
          //    {
          //        ExcelDCController ed = new ExcelDCController();
          //        string sql = "select * from " + zibiao + "  where  " + wj + "= " + Convert.ToInt32(dt.Rows[i][dt.Columns.Count - 1].ToString());
          //        DataTable dtzi = ed.getZiji(sql, zibiao, bc);//
          //        for (int j = 0; j < dt.Columns.Count; j++)
          //        {
          //            excelStr += dt.Rows[i][j].ToString() + "\t";
          //        }
          //        excelStr += "\n";
          //        if (dtzi != null && dtzi.Rows.Count > 0)
          //        {
          //            excelStr += "\t" + "\t";
          //            foreach (DataColumn drzi in dtzi.Columns)
          //            {
          //                excelStr += drzi.ColumnName + "\t";
          //            }
          //            excelStr += "\n";
          //            //子数据
          //            for (int m = 0; m < dtzi.Rows.Count; m++)
          //            {
          //                excelStr += "\t" + "\t";
          //                for (int n = 0; n < dtzi.Columns.Count; n++)
          //                {
          //                    excelStr += dtzi.Rows[m][n].ToString() + "\t";
          //                }
          //                excelStr += "\n";
          //            }

          //        }
          //    }
          //}
          //else
          //{
          //    //添加列名
          //    foreach (DataColumn dr in dt.Columns)
          //    {
          //        excelStr += dr.ColumnName + "\t";
          //    }
          //    excelStr += "\n";
          //    //添加记录
          //    for (int i = 0; i < dt.Rows.Count; i++)
          //    {
          //        for (int j = 0; j < dt.Columns.Count; j++)
          //        {
          //            excelStr += dt.Rows[i][j].ToString() + "\t";
          //        }
          //        excelStr += "\n";
          //    }
          //}
          //excelStr = excelStr.Substring(0, excelStr.Length - 2);
          ////输出EXCEL 
          //HttpResponse rs = System.Web.HttpContext.Current.Response;
          //rs.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
          //rs.HeaderEncoding = System.Text.Encoding.UTF8;
          //rs.Charset = "UTF-8";
          //rs.AppendHeader("Content-Disposition", "attachment;filename=" + fullName);
          //rs.ContentType = "application/ms-excel";
          //rs.Write(excelStr);
          #endregion
          #region 方法2 NPOI
            MemoryStream ms = new MemoryStream();
            ExcelDCController ed = new ExcelDCController();
            IWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet();
            IRow headerRow = sheet.CreateRow(0);
            // handling header.
            foreach (DataColumn column in dt.Columns)
                headerRow.CreateCell(column.Ordinal).SetCellValue(column.Caption);//If Caption not set, returns the ColumnName value
            // handling value.
            int rowIndex = 1;
            foreach (DataRow row in dt.Rows)
            {
                IRow dataRow = sheet.CreateRow(rowIndex);
                foreach (DataColumn column in dt.Columns)
                {
                    dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                }
                DataTable dtzi =new DataTable();
                int mark = 0;
                if (IsEmportZZ.Equals("1"))
                {
                    //string sql = "select * from " + zibiao + "  where  " + wj + "= " + Convert.ToInt32(row[fwj].ToString());
                    string sql =  wj + "= " + Convert.ToInt32(row[fwj].ToString());
                    dtzi = ed.getZiji(sql, zibiao, bc);
                    mark = 0;
                    #region 添加子集
                    if (dtzi != null && dtzi.Rows.Count > 0)
                    {
                        rowIndex = rowIndex + 1;
                        IRow headerRowzi = sheet.CreateRow(rowIndex);
                        // 子集handling header.
                        foreach (DataColumn columnzi in dtzi.Columns)
                            headerRowzi.CreateCell(columnzi.Ordinal).SetCellValue(columnzi.Caption);//If Caption not set, returns the ColumnName value
                        // 子集handling value.
                        int rowIndexzi = rowIndex + 1;
                        rowIndex = rowIndexzi;
                        foreach (DataRow rowzi in dtzi.Rows)
                        {
                            IRow dataRowzi = sheet.CreateRow(rowIndexzi);
                            foreach (DataColumn columnzi in dtzi.Columns)
                            {
                                dataRowzi.CreateCell(columnzi.Ordinal).SetCellValue(rowzi[columnzi].ToString());
                            }
                            rowIndexzi++;
                            mark = rowIndexzi;
                        }
                    }
                    else {
                        rowIndex++;
                        continue;
                    }
                    #endregion
                }
                if (dtzi.Rows.Count==0)
                    rowIndex++;
                else
                    rowIndex = mark;
            }
            AutoSizeColumns(sheet);
            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;
       
          ////输出EXCEL 
          HttpResponse rs = System.Web.HttpContext.Current.Response;
          rs.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
          rs.HeaderEncoding = System.Text.Encoding.UTF8;
          rs.Charset = "UTF-8";
          rs.AppendHeader("Content-Disposition", "attachment;filename=" + fullName);
          rs.ContentType = "application/ms-excel";
          rs.BinaryWrite(ms.ToArray());
          #endregion
           
      }
      /// <summary>
      /// 自动设置Excel列宽
      /// </summary>
      /// <param name="sheet">Excel表</param>
      private static void AutoSizeColumns(ISheet sheet)
      {
          if (sheet.PhysicalNumberOfRows > 0)
          {
              IRow headerRow = sheet.GetRow(0);

              for (int i = 0, l = headerRow.LastCellNum; i < l; i++)
              {
                  sheet.AutoSizeColumn(i);
              }
          }
      }
      private static void ExportToXml(DataTable dt, string fullName, string zibiao, string wj, BusinessController bc, string fwj, string IsEmportZZ)
      {
          MemoryStream ms = new MemoryStream();
          XmlWriterSettings XmlSet = new XmlWriterSettings();
          XmlSet.Indent = true;
          XmlSet.NewLineOnAttributes = false;
          try
          {
              using (XmlWriter XmlWr = XmlWriter.Create(ms,XmlSet))
              {
                  XmlWr.WriteStartDocument(); //声明xml的开始
                  XmlWr.WriteStartElement("DocumentElement"); //声明根元素
                  foreach (DataRow dr in dt.Rows)
                  {
                      XmlWr.WriteStartElement("temp");
                      foreach (DataColumn column in dt.Columns)
                      {
                          XmlWr.WriteStartElement(column.ColumnName);
                          XmlWr.WriteValue(dr[column].ToString());
                          XmlWr.WriteEndElement();
                      }
                      #region 添加子集xml数据
                      if (IsEmportZZ.Equals("1"))
                      {
                          ExcelDCController ed = new ExcelDCController();
                          string sql = "select * from " + zibiao + "  where  " + wj + "= " + Convert.ToInt32(dr[fwj].ToString());
                          DataTable dtzi = ed.getZiji(sql, zibiao, bc);
                          if (dtzi != null && dtzi.Rows.Count > 0)
                          {
                              foreach (DataRow drz in dtzi.Rows)
                              {
                                  XmlWr.WriteStartElement("childtemp");
                                  for (int j = 0; j < (dtzi.Columns.Count - 1); j++)
                                  {
                                      XmlWr.WriteStartElement(dtzi.Columns[j].ColumnName);
                                      XmlWr.WriteValue(drz[j].ToString());
                                      XmlWr.WriteEndElement();
                                  }
                                  XmlWr.WriteEndElement();
                              }
                          }
                      }
                      #endregion
                      //循环节点结束
                      XmlWr.WriteEndElement();
                  }
                  XmlWr.WriteEndElement(); //结束根元素
                  XmlWr.WriteEndDocument(); //结束xml
                  XmlWr.Flush();
                  XmlWr.Close();
              }
              ms.Position = 0;
              ms.Flush();
              StreamReader reader = new StreamReader(ms, Encoding.UTF8);
              string aaa = reader.ReadToEnd();

              HttpResponse rs = System.Web.HttpContext.Current.Response;
              //rs.ContentEncoding = System.Text.Encoding.UTF8;
              //rs.HeaderEncoding = System.Text.Encoding.UTF8;
              //rs.Charset = "UTF-8";
              //rs.AppendHeader("Content-Disposition", "attachment;filename=" + fullName);
              //rs.ContentType = "text/xml";
              //rs.BinaryWrite(ms.ToArray());
              //rs.Write(builder);

              rs.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
              rs.AppendHeader("Content-Disposition", "attachment;filename=" + fullName);
              rs.ContentType = "text/xml";
              rs.BinaryWrite(ms.ToArray());
          }
          catch (Exception x) { }
      }

  }
}
