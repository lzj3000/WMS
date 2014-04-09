using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Business.Controllers;
using Way.EAP.DataAccess.Data;
using System.Data;
using Acc.Contract.MVC;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model;

namespace Acc.Business.Controllers
{
    /// <summary>
    /// 描述：单据编号控制器
    /// 作者：胡文杰
    /// 创建日期:2012-12-18
    /// </summary>
    public class BillNumberController : BusinessController
    {
        public BillNumberController() : base(new BillNumber()) { }

        //显示在菜单
        protected override string OnControllerName()
        {
            return "单据编号管理";
        }
        //菜单中url路径
        protected override string OnGetPath()
        {
            return "Views/manager/BillNumber.htm";
        }
        //开发人
        protected override string OnGetAuthor()
        {
            return "胡文杰";
        }
        //说明
        protected override string OnGetControllerDescription()
        {
            return "单据编号管理";
        }

        protected override void OnInitViewChildItem(Acc.Contract.Data.ModelData data, Acc.Contract.Data.ItemData item)
        {

            #region 单据字段状态控制
            //公共部分
            switch (item.field.ToLower())
            {
                case "issubmited":
                case "submiteddate":
                case "submitedby":
                case "modifiedby":
                case "modifieddate":
                case "reviewedby":
                case "revieweddate":
                case "isreviewed":
                case "creationdate":
                case "code":
                    item.visible = false;
                    break;
                case "fmaxnumber":
                    item.disabled = true;
                    break;
            }
           
            #endregion
        }

        protected override void OnLoaded(Contract.Data.ControllerData.loadItem item, Contract.Data.ControllerData.ReadTable table)
        {
            base.OnLoaded(item, table);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="item"></param>
        protected override void OnAdding(ControllerBase.SaveEvent item)
        {
            base.OnAdding(item);
            BillNumber bn = item.Item as BillNumber;
            if (bn != null)
            {
                if (bn.FPARTNUMBERLENGTH>10)
                {
                    throw new Exception("单据流水长度不能超出10位，请重新设置规则！");
                }
            }
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="item"></param>
        protected override void OnEditing(SaveEvent item)
        {
            base.OnEditing(item);
            BillNumber bn = item.Item as BillNumber;
            if (bn != null)
            {
                if (bn.ISCLEARMAXNUM == true)
                {
                    bn.FMAXNUMBER = 0;//清空流水号
                }
            }
        }

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
            bn.FTABLENAME=con.model.ToString();//表名
            bn.FPARTMARK = con.GetType().Name.Substring(0, 2).ToUpper();//前缀部分-控制器名称的前两位字母作为默认前缀
            bn.FPARTDATESTYLE = "";//日期部分
            bn.FPARTNUMBERLENGTH = 4;//流水长度
            bn.FMAXNUMBER = 1;//当前最大流水号

            //1.获取规则
            string sqlGetRule = string.Format("update Acc_Bus_BillNumber set fmaxnumber=fmaxnumber+1 where FTABLENAME='{0}' and FCONTROLLERNAME='{1}';select FPARTMARK,FPARTDATESTYLE,FPARTNUMBERLENGTH,FMAXNUMBER from Acc_Bus_BillNumber where FTABLENAME='{2}' and FCONTROLLERNAME='{3}';", bn.FTABLENAME, bn.FCONTROLLERNAME, bn.FTABLENAME, bn.FCONTROLLERNAME);
            IDataAction action = con.model.GetDataAction();
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
                string sqlAddNewRule = string.Format("insert into Acc_Bus_BillNumber(FBILLNAME,FCONTROLLERNAME,FTABLENAME,FPARTMARK,FPARTDATESTYLE,FPARTNUMBERLENGTH,FMAXNUMBER,ISCLEARMAXNUM,IsDelete) values('{0}','{1}','{2}','{3}','{4}',{5},{6},{7},{8});select FPARTMARK,FPARTDATESTYLE,FPARTNUMBERLENGTH,FMAXNUMBER from Acc_Bus_BillNumber;",bn.FBILLNAME,bn.FCONTROLLERNAME,bn.FTABLENAME,bn.FPARTMARK,bn.FPARTDATESTYLE,bn.FPARTNUMBERLENGTH,bn.FMAXNUMBER,0,0);
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
            IDataAction action = table.GetDataAction();
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
    }
}
