using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Business.WMS.Model;
using Acc.Business.Controllers;
using Way.EAP.DataAccess.Entity;
using Acc.Contract.Data;

namespace Acc.Business.WMS.Controllers
{
    public class LogisticsInfoController : BusinessController
    {
        
        /// <summary>
        /// 描述：物流公司信息控制器
        /// 作者：柳强
        /// 创建日期:2013-03-19
        /// </summary>
        public LogisticsInfoController() : base(new LogisticsInfo()) { }

        //显示在菜单
        protected override string OnControllerName()
        {
            return "物流公司管理";
        }
        //菜单中url路径
        protected override string OnGetPath()
        {
            return "Views/WMS/WMSBasicInfo/LogisticsInfo.htm";
        }
        //开发人
        protected override string OnGetAuthor()
        {
            return "柳强";
        }
        //说明
        protected override string OnGetControllerDescription()
        {
            return "物流公司管理";
        }

        #region 初始化数据方法
        protected override void OnInitViewChildItem(Contract.Data.ModelData data, Contract.Data.ItemData item)
        {
            if (data.name.EndsWith("LogisticsInfo"))
            {
                switch (item.field.ToLower())
                {
                    case "reviewedby":
                    case "revieweddate":
                    case "issubmited":
                    case "isreviewed":
                    case "submiteddate":
                    case "submitedby":
                    case "isdisable":
                    case "isdelete":
                    case "stay1":
                    case "stay2":
                    case "stay3":
                    case "stay4":
                    case "stay5":
                        item.visible = false;
                        break;
                    case "createdby":
                    case "creationdate":
                    case "modifiedby":
                    case "modifieddate":
                    case "code":
                        item.visible = true;
                        item.disabled = true;
                        break;
                }
            }

            if (data.name.EndsWith("StockOutOrder"))
            {
                switch (item.field.ToLower())
                {

                    case "createdby":
                    case "creationdate":
                    case "modifiedby":
                    case "modifieddate":
                    case "reviewedby":
                    case "revieweddate":
                    case "issubmited":
                    case "isreviewed":
                    case "submiteddate":
                    case "submitedby":
                    case "isdisable":
                    case "isdelete":
                    case "remark":
                    case "stay1":
                    case "stay2":
                    case "stay3":
                    case "stay4":
                    case "stay5":
                    case "sourcename":
                    case "sourceoutcode":
                    case "state":
                    case "typevalue":
                    case "finishtime":
                    case "inouttime":
                    case "stocktype":
                    case "logcode":
                        item.visible = false;
                        break;
                    case "sourcecode":
                        item.title = "销售出库通知单号";
                        break;
                    case "code":
                        item.title = "销售出库单号";
                        break;
                    case "clientno":
                        item.title = "客户名称";
                        break;
                    case "lxrname":
                    case "lxrphone":
                    case "faddress":
                        item.visible = true;
                        break;
                    case "towhno":
                        item.title = "出库仓库";
                        break;
                }
            }
            if (data.name.EndsWith("ShippingOrder"))
            {
                data.visible = false;
                //switch (item.field.ToLower())
                //{
                //    case "submiteddate":
                //    case "submitedby":
                //    case "modifiedby":
                //    case "modifieddate":
                //    case "reviewedby":
                //    case "revieweddate":
                //    case "isdisable":
                //    case "createdby":
                //    case "creationdate":
                //    case "isdelete":
                //    case "id":
                //    case "remark":
                //    case "logisticscompany":
                //    case "shippingtype":
                //        //case "reviewedby":
                //        item.visible = false;
                //        break;
                //    default:
                //        item.visible = true;
                //        break;
                //}
            }
          
        }

        #endregion

        public override bool IsSubmit
        {
            get
            {
                return false;
            }
        }

        public override bool IsClearAway
        {
            get
            {
                return false ;
            }
        }

        public override bool IsPrint
        {
            get
            {
                return false ;
            }
        }

        protected override void OnEditing(Contract.MVC.ControllerBase.SaveEvent item)
        {
             LogisticsInfo i = item.Item as LogisticsInfo;
             if (i != null)
             {
                 if (i.IsSubmited == false)
                 {
                     base.OnEditing(item);
                 }
                 else
                 {
                     throw new Exception("异常：单据已经提交不能编辑！");
                 }
             }
            
        }
    }
}
