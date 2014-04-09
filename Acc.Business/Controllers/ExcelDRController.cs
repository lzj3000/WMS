using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Business.Controllers;
using Acc.Contract.Data.ControllerData;
using System.Data;
using System.Data.OleDb;
using System.IO;
using Acc.Contract;
using Acc.Contract.Data;
using Way.EAP.DataAccess.Data;
using Acc.Business.Model;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Acc.Contract.MVC;
using Way.EAP.DataAccess.Entity;

namespace Acc.Business.Controllers
{
    public class ExcelDRController : BusinessController
    {
        public ExcelDRController() : base(new BusinessBase()) { }
        protected override string OnGetPath()
        {
            return "Views/manager/Exceldr.htm";
        }
        protected override string OnControllerName()
        {
            return "Excel导入";
        }
        IDataAction action;
        public BusinessController bc { get; set; }
        BillNumberController bnc = new BillNumberController();
        #region 导入功能
        private int pagerow = 20;
        [WhereParameter]
        public string Excel_Workspace { get; set; }//选择的sheelt工作区名称
        [WhereParameter]
        public string IsHeader { get; set; }//Excel第一行作为表头0,否侧1
        [WhereParameter]
        public string IOperate { get; set; }
        [WhereParameter]
        public string TargetTable { get; set; }//导入的表
        [WhereParameter]
        public string TargetTableP { get; set; }//导入子集Excel时，父表
        [WhereParameter]
        public string FilePath { get; set; }
        [WhereParameter]
        public string ParentAndChild_field { get; set; }//对照字段
        [WhereParameter]
        public string Controllername { get; set; }
         [WhereParameter]
        public string IsMasterTable { get; set; }
        [WhereParameter]
        public ImportMapping[] MappingInfo { get; set; }
        public class ImportMapping
        {
            public string ScName { get; set; }
            public string TcName { get; set; }
            public bool IsAuto { get; set; }
            public string Value { get; set; }
            public string Type { get; set; }
            public string FZGX { get; set; }
        }
        public virtual string Import()
        {
            //获取源列名
            if (IOperate == "1")
            {
                //取Excel源列名,并返回前十条数据
                return GetExcelSourceColumns(FilePath, 10);
            }
            //导入
            else if (IOperate == "2")
            {
                //获取Excel源数据
                DataTable dtImport = GetSourceTable(FilePath, IsHeader, Excel_Workspace);
                //根据映射关系转换数据
                DataTable dtnow = ConvertData(dtImport, MappingInfo);
                //导入数据
                return ImportData(dtnow, MappingInfo).ToString();
            }
            else
            {
                return null;
            }
        }
        public virtual DataTable GetSourceTable(string file, string iscolunum, string sheelname)
        {
            DataTable dtImport = WebImport1.ExcelToDt(FilePath, IsHeader, Excel_Workspace);
            return dtImport;
        }
        protected virtual string GetExcelSourceColumns(string filePath, int num)
        {

            DataTable dt = GetColumns(FilePath, 10);
            if (dt == null)
            {
                return "0";
            }
            DataTable dtr = dt.Clone();
            for (int i = 0; i < num; i++)
            {
                if (i == dt.Rows.Count)
                    break;
                dtr.Rows.Add(dt.Rows[i].ItemArray);
            }

            return JSON.Serializer(dtr.Columns);
        }
        /// <summary>
        /// 取源表的列名,并返回前n条数据
        /// </summary>
        /// <param name="filePath">上传文件全路径</param>
        /// <param name="num">返回记录数</param>
        /// <returns></returns>
        public DataTable GetColumns(string filePath, int num)
        {
            return WebImport1.ExcelToDt(filePath, IsHeader, Excel_Workspace);
        }
        /// <summary>
        /// 转换数据（通过映射关系）
        /// </summary>
        /// <param name="dt">源数据</param>
        /// <param name="mi">映射关系</param>
        /// <returns></returns>
        protected virtual DataTable ConvertData(DataTable dt, ImportMapping[] mi)
        {
            //创建待导入的DataTable对象
            DataTable dtnow = new DataTable();
            foreach (ImportMapping i in mi)
            {
                if (!string.IsNullOrEmpty(i.ScName))
                {
                    dtnow.Columns.Add(i.TcName);
                }
            }
            if (dtnow.Columns.Count > 0)
            {
                //导入数据
                foreach (DataRow dr in dt.Rows)
                {
                    DataRow drnow = dtnow.NewRow();
                    foreach (ImportMapping i in mi)
                    {
                        if (!string.IsNullOrEmpty(i.ScName))
                        {
                            //赋值
                            drnow[i.TcName] = dr[i.ScName];
                            //默认值处理
                            if (!string.IsNullOrEmpty(i.Value))
                            {
                                drnow[i.TcName] = i.Value;
                            }
                        }
                    }
                    dtnow.Rows.Add(drnow);
                }
                return dtnow;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 根据父与子对照字段获得该条的父ID
        /// </summary>
        /// <param name="i"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string getFZDZ(ImportMapping i, DataRow dr, string array,bool isWJ)
        {
            StringBuilder sqlwhere = new StringBuilder();
            if (isWJ)
            {
                sqlwhere.Append(FKey() + "=" + Convert.ToInt32(dr[i.TcName]));
            }
            else
            {

                switch (i.Type.ToLower())
                {
                    case "int":
                        sqlwhere.Append(array + "=" + Convert.ToInt32(dr[i.TcName]));
                        break;
                    case "dint":
                        sqlwhere.Append(array + "=" + Convert.ToDecimal(dr[i.TcName]));
                        break;
                    case "bool":
                        int bl = (Convert.ToString(dr[i.TcName])).ToUpper() == "TRUE" ? 1 : 0;
                        sqlwhere.Append(array + "=" + (Convert.ToString(dr[i.TcName])).ToUpper() == "TRUE" ? 1 : 0);
                        break;
                    case "string":
                    case "date":
                    default:
                        sqlwhere.Append(array + "='" + Convert.ToString(dr[i.TcName]) + "'");
                        break;
                }
            }
            string sql = "select  " + FKey() + "  from " + TargetTableP + "  where " + sqlwhere.ToString();
            DataTable dt_id = action.GetDataTable(sql);
            if (dt_id.Rows.Count == 1)
            {
                return dt_id.Rows[0][FKey()].ToString();
            }
            else if (dt_id.Rows.Count > 1)
            {
                return "2";
            }
            else
            {
                return "0";
            }
        }
        /// <summary>
        /// 对照字段中的父表中要查询的字段；可扩展
        /// </summary>
        /// <returns></returns>
        protected virtual string FKey()
        {
            return "ID";
        }
        /// <summary>
        /// 对照字段中的子表中要插入的字段；可扩展
        /// </summary>
        /// <returns></returns>
        protected virtual string Z_FKey()
        {
            return "FID";
        }
        /// <summary>
        /// 假设的对照字段；可扩展
        /// </summary>
        /// <returns></returns>
        protected virtual string suppose_Field()
        {
            return "CODE";
        }
        /// <summary>
        /// 判断插入的表是否是主表
        /// </summary>
        /// <returns>true标示是主表</returns>
        public bool ismastertable()
        {
            if (IsMasterTable.Equals("0"))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// 主子对照字段
        /// </summary>
        /// <returns></returns>
        public string[] Z_Z_Field()
        {
            if (ParentAndChild_field != null)
            {
                string[] array1 = ParentAndChild_field.Split(';');
                string[] array = array1[0].Split(',');
                return array;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 获取插入的列名
        /// </summary>
        /// <param name="im"></param>
        /// <returns></returns>
        public virtual string getInsertZD(List<ImportMapping> im)
        {
            var sortedList = im.OrderByDescending(a => a.TcName).ThenBy(a => a.TcName);
            im = sortedList.ToList(); 
            string strColumns = "ISDELETE,";
            foreach (ImportMapping i in im)
            {
                strColumns += i.TcName + ",";
            }
            if (IsMasterTable.Equals("0"))
            {
                strColumns += Z_FKey();
            }
            else
            {
                strColumns = strColumns.Substring(0, strColumns.Length - 1);
            }
            return strColumns;
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
        public virtual Dictionary<string, EntityForeignKeyAttribute> getST_WJ_Main()
        {
             BasicInfo info = bc.model as BasicInfo;
             IEntityBase ieb = (IEntityBase)info;
             IHierarchicalEntityView[] hev = ieb.GetChildEntityList();
             foreach (IHierarchicalEntityView v in hev)
             {

                 EntityBase eb = v.ChildEntity;
                 if (TargetTable.Equals(eb.ToString()))
                 {
                     return ((IEntityBase)eb).GetForeignKey();
                 }
             }
            return null;
        }
        /// <summary>
        /// 获取插入的值
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="im"></param>
        /// <returns></returns>
        public virtual string getInsertValue(DataRow dr, List<ImportMapping> im,DataTable dt, IDataAction action)
        {
            var sortedList = im.OrderByDescending(a => a.TcName).ThenBy(a => a.TcName);
            im = sortedList.ToList(); //这个时候会排序,
            double WeightNUM=0.0;
            string strValues = "0,";//CREATEDBY,APPLYID;
            int id = 0;
            Dictionary<string, EntityForeignKeyAttribute> itemss;
            if (IsMasterTable.Equals("0"))
            {
                itemss=getST_WJ_Main();
            }else{
                itemss= getST_WJ_items();
            }
            int rownum = dt.Rows.IndexOf(dr)+1;
            foreach (ImportMapping i in im)
            {
               //实体外键处理的方法，可以重写
              bool isWJ= Entity_Foreign_Key_Processing(i, dr, WeightNUM, itemss, rownum,action);
                #region 插入子集
                if (IsMasterTable.Equals("0"))
                {
                    if (i.TcName == Z_Z_Field()[0])
                    {
                        if (dr[Z_Z_Field()[0]].ToString().Equals(""))
                        {
                            throw new Exception("所选的对照字段" + Z_Z_Field()[0] + "值不可以为空，请查看Excel文件!");
                        }
                        //Excel中唯一
                        //int num = 0;
                        //foreach (DataRow drcode in dt.Rows)
                        //{
                        //    if (dr[i.TcName].ToString().Equals(drcode[i.TcName]))
                        //    {
                        //        num++;
                        //    }
                        //}
                        //if (num != 1) {
                        //    throw new Exception("Excel中存在多个相同的" + i.TcName + "值，不可以导入，请查看Excel文件");
                        //}
                        //父表中唯一，存在
                        string result = getFZDZ(i, dr, Z_Z_Field()[1], isWJ);
                        if (result.Equals("0"))
                        {
                            throw new Exception("所选的对照字段在父表中没有找到对应的ID，请检查Excel数据!");
                        }
                        else if (result.Equals("2"))
                        {
                            throw new Exception("所选的对照字段在父表中找到多个对应的ID，请检查对照关系!");
                        }
                        else
                        {
                            id = Convert.ToInt32(result);
                        }
                        //目标表中唯一
                        //string sql = "select " + i.TcName + "  from  " + TargetTable;
                        //DataTable dtCode = action.GetDataTable(sql);
                        //if (dtCode.Rows.Count > 0)
                        //{
                        //    foreach (DataRow drcode in dtCode.Rows)
                        //    {
                        //        if (dr[i.TcName].ToString().Equals(drcode[i.TcName]))
                        //        {
                        //            throw new Exception(i.TcName + "是唯一列，数据库中以存在该值，不可以导入，请查看Excel文件");
                        //        }
                        //    }
                        //}
                    }
                }
                #endregion
                //自动编码处理
                if (i.IsAuto)
                {
                        //自动生成编码
                    string code = GetBillNo(bc);//bnc.GetBillNo(bc);
                        strValues += "'" + code + "',";
                        continue;
                }
                switch (i.Type.ToLower())
                {
                    case "int":
                        strValues += "" + Convert.ToInt32(dr[i.TcName]) + ",";
                        break;
                    case "dint":
                        strValues += "" + Convert.ToDecimal(dr[i.TcName]) + ",";
                        break;
                    case "bool":
                        int bl = (Convert.ToString(dr[i.TcName])).ToUpper() == "TRUE" ? 1 : 0;
                        strValues += "" + bl + ",";
                        break;
                    case "string":
                    case "date":
                    default:
                        strValues += "'" + Convert.ToString(dr[i.TcName]) + "',";
                        break;
                }
            }
            if (IsMasterTable.Equals("0"))
            {
                strValues += "'" + id + "'";
            }
            else
            {
                strValues = strValues.Substring(0, strValues.Length - 1);
            }
            return strValues;
        }

        public virtual bool Entity_Foreign_Key_Processing(ImportMapping i, DataRow dr, double WeightNUM, Dictionary<string, EntityForeignKeyAttribute> itemss, int rownum, IDataAction action)
        {
            bool isWJ = false;
            #region 实体外键处理
            //BasicInfo info = bc.model as BasicInfo;
            //IEntityBase ieb = (IEntityBase)info;
            //Dictionary<string, EntityForeignKeyAttribute> itemss = ((IEntityBase)ieb).GetForeignKey();
            foreach (KeyValuePair<string, EntityForeignKeyAttribute> kv in itemss)
            {
                /*
                 *  /// <summary>
                /// 申请人ID
                /// </summary>
                [EntityControl("申请人", false, true, 2)]
                [EntityForeignKey(typeof(OfficeWorker), "ID", "WorkName")]
                [EntityField(50, IsNotNullable = true)]
                public int ApplyID { get; set; }
                 */
                string dd = kv.Key;
                if (i.TcName.Equals(dd.ToUpper()))
                {
                    isWJ = true;
                    string fjtable = kv.Value.ObjectName;//Acc_Bus_OfficeWorker
                    string fjvalue = kv.Value.DisplayField;//WorkName
                    string fjid = kv.Value.FieldName;//ID
                    string fjsql = Construction_of_SQL(dd, fjid, fjtable, fjvalue, dr, i);
                    //string fjsql = "select " + fjid + " from  " + fjtable + "  where  " + fjvalue+"='"+dr[i.TcName]+"'";
                    DataTable fj_value_table = action.GetDataTable(fjsql);
                    if (fj_value_table.Rows.Count == 1)
                    {
                        dr[i.TcName] = fj_value_table.Rows[0][fjid].ToString();
                    }
                    else if (fj_value_table.Rows.Count > 1)
                    {

                        //throw new Exception("外键表" + fjtable + "中" + fjvalue + "对应多个" + fjid+",请查看外键表");
                        throw new Exception(i.ScName + ":" + dr[i.TcName] + "在外键表中没有找到，请检查Excel第" + rownum + "行");
                    }
                    else
                    {
                        //throw new Exception("外键表" + fjtable + "中" + fjvalue + "不存在对应" + fjid + ",请查看外键表");
                        throw new Exception(i.ScName + ":" + dr[i.TcName] + "在外键表中没有找到，请检查Excel第" + rownum + "行");
                    }
                }
                else
                {
                    continue;
                }
            }
            return isWJ;
            #endregion
        }
        public virtual string Construction_of_SQL(string key,string fjid, string fjtable, string fjvalue, DataRow dr, ImportMapping i)
        {
            return  "select " + fjid + " from  " + fjtable + "  where  " + fjvalue+"='"+dr[i.TcName]+"'";
        }

       
        #region 自动生成单据编号
        
     
        /// <summary>
        /// 自动生成单据编号
        /// </summary>
        /// <returns>单据编号</returns>
        /// <summary>
        /// 自动生成单据编号
        /// </summary>
        /// <returns>单据编号</returns>
        public string GetBillNo(BusinessController con)
        {
            string billNo = "";

            BillNumber bn = new BillNumber();
            bn.FBILLNAME = ((IController)con).ControllerName();//单据名称
            bn.FCONTROLLERNAME = con.GetType().FullName;//控制器全名称
            bn.FTABLENAME = con.model.ToString();//表名
            bn.FPARTMARK = con.GetType().Name.Substring(0, 2).ToUpper();//前缀部分-控制器名称的前两位字母作为默认前缀
            bn.FPARTDATESTYLE = "";//日期部分
            bn.FPARTNUMBERLENGTH = 4;//流水长度
            bn.FMAXNUMBER = 1;//当前最大流水号

            //1.获取规则
            string sqlGetRule = string.Format("update Acc_Bus_BillNumber set fmaxnumber=fmaxnumber+1 where FTABLENAME='{0}' and FCONTROLLERNAME='{1}';select FPARTMARK,FPARTDATESTYLE,FPARTNUMBERLENGTH,FMAXNUMBER from Acc_Bus_BillNumber where FTABLENAME='{2}' and FCONTROLLERNAME='{3}';", bn.FTABLENAME, bn.FCONTROLLERNAME, bn.FTABLENAME, bn.FCONTROLLERNAME);
            DataTable dtRule = action.GetDataTable(sqlGetRule);
            if (dtRule.Rows.Count > 0)
            {
                //1.获取规则
                bn.FPARTMARK = dtRule.Rows[0]["FPARTMARK"].ToString();
                bn.FPARTDATESTYLE = dtRule.Rows[0]["FPARTDATESTYLE"].ToString();
                bn.FPARTNUMBERLENGTH = int.Parse(dtRule.Rows[0]["FPARTNUMBERLENGTH"].ToString());
                bn.FMAXNUMBER = int.Parse(dtRule.Rows[0]["FMAXNUMBER"].ToString());

                if (bn.FMAXNUMBER.ToString().Length > bn.FPARTNUMBERLENGTH)
                {
                    throw new Exception("单据最大流水号超过限定流水长度，请重新设置单据单据编号规则！");
                }
                //2根据规则生成单据编号
                bn.FPARTDATESTYLE = GetPartDate(bn.FPARTDATESTYLE);//转换日期部分
                billNo = CreateNumber(con.model, bn);

                //3.修改单据的最大流水记录
                string sqlUpdateMaxNumber = string.Format("update Acc_Bus_BillNumber set fmaxnumber={0} where FTABLENAME='{1}' and FCONTROLLERNAME='{2}';select FPARTMARK,FPARTDATESTYLE,FPARTNUMBERLENGTH,FMAXNUMBER from Acc_Bus_BillNumber;", bn.FMAXNUMBER, bn.FTABLENAME, bn.FCONTROLLERNAME);
                action.GetDataTable(sqlUpdateMaxNumber);
            }
            else
            {
                //如果没有设置规则，则按默认规则生成单据编号
                string sqlAddNewRule = string.Format("insert into Acc_Bus_BillNumber(FBILLNAME,FCONTROLLERNAME,FTABLENAME,FPARTMARK,FPARTDATESTYLE,FPARTNUMBERLENGTH,FMAXNUMBER,ISCLEARMAXNUM,IsDelete) values('{0}','{1}','{2}','{3}','{4}',{5},{6},{7},{8});select FPARTMARK,FPARTDATESTYLE,FPARTNUMBERLENGTH,FMAXNUMBER from Acc_Bus_BillNumber;", bn.FBILLNAME, bn.FCONTROLLERNAME, bn.FTABLENAME, bn.FPARTMARK, bn.FPARTDATESTYLE, bn.FPARTNUMBERLENGTH, bn.FMAXNUMBER, 0, 0);
                action.GetDataTable(sqlAddNewRule);
                return CreateNumber(con.model, bn);
            }

            return billNo;
        }

        /// <summary>
        /// 递归生成非已有单据编号
        /// </summary>
        private string CreateNumber(IModel table, BillNumber bn)
        {
            //根据规则拼接单据编号
            string billNo = SpliceNumber(bn);
            //判断数据库是否已经存在该编号
            if (!IsExistNum(table, bn, billNo))
            {
                return billNo;
            }
            else
            {
                bn.FMAXNUMBER += 1;//当前流水
                return CreateNumber(table, bn);
            }
        }

        /// <summary>
        /// 根据规则拼接成单据编号
        /// </summary>
        private string SpliceNumber(BillNumber bn)
        {
            string billNo = "";
            //int replaceLength = bn.FPARTNUMBERLENGTH - bn.FMAXNUMBER.ToString().Length;//补0长度=流水长度-当前流水长度
            //string replaceString = "";//流水号补0部分
            //for (int i = 0; i < replaceLength; i++)
            //{
            //    replaceString += "0";
            //}
            //billNo = bn.FPARTMARK + bn.FPARTDATESTYLE+replaceString  + bn.FMAXNUMBER.ToString();//单据编号=前缀+日期+补0+当前流水号
            billNo = bn.FPARTMARK + bn.FPARTDATESTYLE + bn.FMAXNUMBER.ToString().PadLeft(bn.FPARTNUMBERLENGTH, '0');//单据编号=前缀+日期+当前流水号(补0)
            return billNo;
        }

        /// <summary>
        /// 判断单据编号是否已经存在
        /// </summary>
        private bool IsExistNum(IModel table, BillNumber bn, string billNo)
        {
            string sqlNo = string.Format("select * from {0} where Code='{1}'", bn.FTABLENAME, billNo);
            DataTable dtNo = action.GetDataTable(sqlNo);
            if (dtNo.Rows.Count > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 转换日期部分
        /// </summary>
        private string GetPartDate(string partDateType)
        {
            string partDate = "";
            switch (partDateType)
            {
                case "yy":
                    partDate = DateTime.Now.Year.ToString().Substring(2);
                    break;
                case "yymm":
                    partDate = DateTime.Now.Year.ToString().Substring(2) + DateTime.Now.Month.ToString("00");
                    break;
                default:
                    break;
            }
            return partDate;
        }
        #endregion
        /// <summary>
        /// 导入数据（通过映射配置）
        /// </summary>
        /// <param name="dt">导入数据</param>
        /// <param name="mi">映射配置</param>
        /// <returns></returns>
        protected virtual string ImportData(DataTable dt, ImportMapping[] mi)
        {
            bc = (BusinessController)ControllerCenter.GetCenter.GetController(Controllername, this.user.tempid);
            if (dt != null && dt.Rows.Count > 0)
            {
                List<ImportMapping> im = mi.Where(F => F.ScName != null && F.ScName != "").Select(F => F).ToList();
                //筛选有效数据列
                string strColumns = getInsertZD(im);
                action =this.model.GetDataAction();
                action.StartTransation();
                foreach (DataRow dr in dt.Rows)
                {
                    try
                    {
                        string strValues = getInsertValue(dr, im,dt,action);
                        string strSql = "insert into " + TargetTable + "(" + strColumns + ") values(" + strValues + ")";
                        action.Execute(strSql);
                       
                    }
                    catch (Exception e)
                    {
                        action.EndTransation();
                        return e.Message;
                    }
                }
                action.Commit();
                action.EndTransation();
                if (File.Exists(FilePath))
                {
                    File.Delete(FilePath);
                }
                return "1";
            }
            return "0";
        }
        #endregion

        [WhereParameter]
        public string FilePath1 { get; set; }
        /// <summary>
        /// 获取Excel的工作区
        /// </summary>
        /// <returns></returns>
        public string GetExcelTables()
        {
            #region 普通方法
            //将Excel架构存入数据里
            //DataTable dt = new DataTable();
            //// ArrayList TablesList = new ArrayList();
            //StringBuilder sb = new StringBuilder();
            //if (File.Exists(FilePath1))
            //{
            //    string strConn = "Provider=Microsoft.Ace.OleDb.12.0;" + "data source=" + FilePath1 + ";Extended Properties='Excel 12.0; HDR=YES; IMEX=1'";
            //    using (OleDbConnection conn = new OleDbConnection(strConn))
            //    {
            //        try
            //        {
            //            conn.Open();
            //            dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
            //        }
            //        catch (Exception ex)
            //        {
            //            throw new Exception("异常" + ex.Message + "");
            //        }
            //        //获取数据表个数
            //        int tablecount = dt.Rows.Count;
            //        for (int i = 0; i < tablecount; i = i + 1)
            //        {
            //            string tablename = dt.Rows[i][2].ToString().Trim().TrimEnd('$');
            //            sb.Append(tablename).Append(',');
            //        }
            //    }
            //}
            //return sb.ToString().Substring(0, sb.ToString().Length - 1);
            #endregion
            #region NPOI
            FileStream file = new FileStream(FilePath1, FileMode.Open, FileAccess.Read);
            StringBuilder sb = new StringBuilder();
            string result = FilePath1.Substring(FilePath1.LastIndexOf('.'));
            IWorkbook wb = null;
            //XSSFWorkbook wb = new XSSFWorkbook(file);
            if (result.ToLower().Equals(".xls"))
            {
                wb = new HSSFWorkbook(file);
            }
            else if (result.ToLower().Equals(".xlsx"))
            {
                wb = new XSSFWorkbook(file);
            }
            else
            {
                return null;
            }
            int num = wb.NumberOfSheets;
            for (int i = 0; i < num; i++)
            {
                string name = wb.GetSheetAt(i).SheetName;
                sb.Append(name).Append(',');
            }
            return sb.ToString().Substring(0, sb.ToString().Length - 1);
            #endregion

        }
    }
    #region Web文件导入导出
    public static class WebImport1
    {
        public static DataTable Import(string file, string Select2, string sheel)
        {
            if (string.IsNullOrEmpty(file))
                return null;
            DataTable dt = ExcelToDt(file, Select2, sheel);
            return dt;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Path">路径</param>
        /// <param name="Select2">EXcel第一行是否作为标题</param>
        /// <param name="sheel">选择的Excel工作区</param>
        /// <returns></returns>
        public static DataTable ExcelToDt(string Path, string Select2, string sheel)
        {
            #region 普通方法
            //string strConn = "";
            //if (Select2 == "0")
            //{
            //    strConn = "Provider=Microsoft.Ace.OleDb.12.0;" + "data source=" + Path + ";Extended Properties='Excel 12.0; HDR=YES; IMEX=1'";
            //}
            //else
            //{
            //    strConn = "Provider=Microsoft.Ace.OleDb.12.0;" + "data source=" + Path + ";Extended Properties='Excel 12.0; HDR=NO; IMEX=1'";
            //}
            ////string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + Path + ";Extended Properties=Excel 8.0;";
            //OleDbConnection conn = new OleDbConnection(strConn);
            //conn.Open();
            //string strExcel = "";
            //OleDbDataAdapter adap = null;
            //DataTable dt = null;
            //strExcel = "select * from [" + sheel + "$]";
            //adap = new OleDbDataAdapter(strExcel, strConn);
            //dt = new DataTable();
            //adap.Fill(dt);
            //return dt;
            #endregion
            #region NPOI
            string result = Path.Substring(Path.LastIndexOf('.'));
            if (result.ToLower().Equals(".xls"))
            {
                return ExcelToTableForXLS(Path, sheel);
            }
            else if (result.ToLower().Equals(".xlsx"))
            {
                return ExcelToTableForXLSX(Path, sheel);
            }
            else
            {
                return null;
            }
            #endregion

        }
        #region Excel2003
        public static DataTable ExcelToTableForXLS(string file, string sheel)
        {
            DataTable dt = new DataTable();
            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                HSSFWorkbook hssfworkbook = new HSSFWorkbook(fs);
                ISheet sheet = hssfworkbook.GetSheet(sheel);

                //表头   
                IRow header = sheet.GetRow(sheet.FirstRowNum);
                if (header != null)
                {
                    List<int> columns = new List<int>();
                    for (int i = 0; i < header.LastCellNum; i++)
                    {
                        object obj = GetValueTypeForXLS(header.GetCell(i) as HSSFCell);
                        if (obj == null || obj.ToString() == string.Empty)
                        {
                            dt.Columns.Add(new DataColumn("Columns" + i.ToString()));
                            //continue;   
                        }
                        else
                            dt.Columns.Add(new DataColumn(obj.ToString()));
                        columns.Add(i);
                    }
                    //数据   
                    for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
                    {
                        DataRow dr = dt.NewRow();
                        bool hasValue = false;
                        foreach (int j in columns)
                        {
                            dr[j] = GetValueTypeForXLS(sheet.GetRow(i).GetCell(j) as HSSFCell);
                            if (dr[j] != null && dr[j].ToString() != string.Empty)
                            {
                                hasValue = true;
                            }
                        }
                        if (hasValue)
                        {
                            dt.Rows.Add(dr);
                        }
                    }
                }
                else
                {
                    return null;
                }

            }
            return dt;
        }
        /// <summary>   
        /// 获取单元格类型(xls)   
        /// </summary>   
        /// <param name="cell"></param>   
        /// <returns></returns>   
        private static object GetValueTypeForXLS(HSSFCell cell)
        {
            if (cell == null)
                return null;
            switch (cell.CellType)
            {
                case CellType.BLANK:
                    return string.Empty;
                case CellType.BOOLEAN:
                    return cell.BooleanCellValue.ToString();
                case CellType.ERROR:
                    return cell.ErrorCellValue.ToString();
                case CellType.NUMERIC:
                case CellType.Unknown:
                default:
                    return cell.ToString();//This is a trick to get the correct value of the cell. NumericCellValue will return a numeric value no matter the cell value is a date or a number
                case CellType.STRING:
                    return cell.StringCellValue;
                case CellType.FORMULA:
                    try
                    {
                        HSSFFormulaEvaluator e = new HSSFFormulaEvaluator(cell.Sheet.Workbook);
                        e.EvaluateInCell(cell);
                        return cell.ToString();
                    }
                    catch
                    {
                        return cell.NumericCellValue.ToString();
                    }
            }
        }
        #endregion

        #region Excel2007
        /// <summary>   
        /// 将Excel文件中的数据读出到DataTable中(xlsx)   
        /// </summary>   
        /// <param name="file"></param>   
        /// <returns></returns>   
        public static DataTable ExcelToTableForXLSX(string file, string sheel)
        {
            DataTable dt = new DataTable();
            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                XSSFWorkbook xssfworkbook = new XSSFWorkbook(fs);
                ISheet sheet = xssfworkbook.GetSheet(sheel);

                //表头   
                IRow header = sheet.GetRow(sheet.FirstRowNum);
                if (header != null)
                {
                    List<int> columns = new List<int>();
                    for (int i = 0; i < header.LastCellNum; i++)
                    {
                        object obj = GetValueTypeForXLSX(header.GetCell(i) as XSSFCell);
                        if (obj == null || obj.ToString() == string.Empty)
                        {
                            dt.Columns.Add(new DataColumn("Columns" + i.ToString()));
                            //continue;   
                        }
                        else
                            dt.Columns.Add(new DataColumn(obj.ToString()));
                        columns.Add(i);
                    }
                    //数据   
                    for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
                    {
                        DataRow dr = dt.NewRow();
                        bool hasValue = false;
                        foreach (int j in columns)
                        {
                            dr[j] = GetValueTypeForXLSX(sheet.GetRow(i).GetCell(j) as XSSFCell);
                            if (dr[j] != null && dr[j].ToString() != string.Empty)
                            {
                                hasValue = true;
                            }
                        }
                        if (hasValue)
                        {
                            dt.Rows.Add(dr);
                        }
                    }
                }
                else
                {
                    return null;
                }
            }
            return dt;
        }
        /// <summary>   
        /// 获取单元格类型(xlsx)   
        /// </summary>   
        /// <param name="cell"></param>   
        /// <returns></returns>   
        private static object GetValueTypeForXLSX(XSSFCell cell)
        {
            if (cell == null)
                return null;
            switch (cell.CellType)
            {
                case CellType.BLANK:
                    return string.Empty;
                case CellType.BOOLEAN:
                    return cell.BooleanCellValue.ToString();
                case CellType.ERROR:
                    return cell.ErrorCellValue.ToString();
                case CellType.NUMERIC:
                    return getData(cell.ToString());
                //DateTime now = DateTime.Now;
                //if (DateTime.TryParse(cell.StringCellValue, out now))
                //{
                //    return now;
                //}
                case CellType.Unknown:
                default:
                    return cell.ToString();
                case CellType.STRING:
                    return cell.StringCellValue;
                case CellType.FORMULA:
                    try
                    {
                        XSSFFormulaEvaluator e = new XSSFFormulaEvaluator(cell.Sheet.Workbook);
                        e.EvaluateInCell(cell);
                        return cell.ToString();
                    }
                    catch
                    {
                        return cell.NumericCellValue.ToString();
                    }
            }
        }
        public static string getData(string ss)
        {
            //03-Feb-2013
            if (ss.Contains('-'))
            {
                string[] array = ss.Split('-');
                if (array.Length > 0)
                {
                    switch (array[1])
                    {
                        case " Jan":
                            return array[0] + "-" + "1" + "-" + array[2];
                        case "Feb":
                            return array[0] + "-" + "2" + "-" + array[2];
                        case "Mar":
                            return array[0] + "-" + "3" + "-" + array[2];
                        case "Apr":
                            return array[0] + "-" + "4" + "-" + array[2];
                        case "May":
                            return array[0] + "-" + "5" + "-" + array[2];
                        case "Jun":
                            return array[0] + "-" + "6" + "-" + array[2];
                        case "Jul":
                            return array[0] + "-" + "7" + "-" + array[2];
                        case "Aug":
                            return array[0] + "-" + "8" + "-" + array[2];
                        case "Sep":
                            return array[0] + "-" + "9" + "-" + array[2];
                        case "Oct":
                            return array[0] + "-" + "10" + "-" + array[2];
                        case "Nov":
                            return array[0] + "-" + "11" + "-" + array[2];
                        case "Dec":
                            return array[0] + "-" + "12" + "-" + array[2];

                    }
                }
            }
            else
            {
                return ss;
            }

            return "";
        }
        #endregion
    }
    #endregion
}
