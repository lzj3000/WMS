using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Contract.MVC;
using Acc.Business.WMS.Model;
using Acc.Contract;
using System.Data;
using Acc.Business.Controllers;
using Way.EAP.DataAccess.Entity;
using Acc.Business.WMS.Controllers;
using Acc.Contract.Center;
using Way.EAP.DataAccess.Data;
using Way.EAP.DataAccess.Regulation;
using Acc.Contract.Data;
using Acc.Business.WMS.XHY.Model;
using Acc.Business.Model;

namespace Acc.Business.WMS.XHY.Controllers
{
    public class PurchaseInNoticeOrderController : XHY_InNoticeOrderController
    {
        public PurchaseInNoticeOrderController() : base(new StockInNotice()) { }
        #region 页面设置

        //菜单中url路径
        protected override string OnGetPath()
        {
            return "Views/WMS/XHY/PurchaseInNoticeOrder.htm";
        }

        /// <summary>
        /// 显示名
        /// </summary>
        /// <returns></returns>
        protected override string OnControllerName()
        {
            return "采购入库通知";
        }

        public override bool IsPushDown
        {
            get
            {
                return true;
            }
        }
        #endregion

        #region 初始化数据方法
        protected override void OnInitViewChildItem(Contract.Data.ModelData data, Contract.Data.ItemData item)
        {
            base.OnInitViewChildItem(data, item);
            if (data.name.EndsWith("StockInNotice"))
            {
                data.title = "采购入库通知";
                switch (item.field.ToLower())
                {
                    case "code":
                        item.title = "收料通知单号";
                        item.disabled = true;
                        break;
                    case "submiteddate":
                    case "submitedby":
                    case "createdby":
                    case "creationdate":
                    case "modifiedby":
                    case "modifieddate":
                    case "issubmited":
                    case "isreviewed":
                    case "remark":
                        item.disabled = true;
                        break;
                    case "clientno":
                        item.title = "供应商编码";
                        item.disabled = true;
                        break;
                    case "sourcecode":
                        item.title = "采购订单号";
                        item.visible = true;
                        item.disabled = true;
                        item.foreign.isfkey = false;
                        break;
                    case "sourcename":
                    case "sourceoutcode":
                    case "workerid":
                    case "bumen":
                    case "stocktype":
                    case "finishtime":
                        item.visible = false;
                        break;
                    case "stay3":
                        item.visible = true;
                        item.title = "计划到货时间";
                        break;

                }
            }
            if (data.name.EndsWith("StockInNoticeMaterials"))
            {
                data.isadd = false;
                data.isedit = false;
                data.isremove = false;
                data.title = "采购入库通知明细";
                switch (item.field.ToLower())
                {
                    case "code":
                    case "sourcecode":
                    case "parentid":
                    case "state":////状态
                    case "createdby":
                    case "creationdate":
                        item.visible = false;
                        break;
                    case "num":
                        item.title = "采购数量";
                        break;
                    case "finishnum":
                        item.disabled = true;
                        break;
                    case "staynum":
                        item.disabled = true;
                        item.title = "待操作数";
                        break;
                   
                }
            }
        }
        #endregion

        #region 重写方法
        protected override void OnForeignLoading(IModel model, Contract.Data.ControllerData.loadItem item)
        {
            base.OnForeignLoading(model, item);
            ///选择客户不包括已禁用的
            if (this.fdata.filedname == "MATERIALCODE")
            {
                item.rowsql = "select * from (" + item.rowsql + ") mc where mc.CommodityType =0";
               // item.rowsql = "select id,code,FNAME,FFULLNAME,ClassID,FUNITID,BATCH,STATUS,ISOVERIN,IsDisable,ISCHECKPRO,Remark,IsDelete from " + new BusinessCommodity().ToString() + " a where a.CommodityType=0";
            }
        }

        /// <summary>
        /// 设置stocktype=1
        /// </summary>
        /// <returns></returns>
        public override int SetStockType()
        {
            return 1;
        }


        protected override void OnRemoveing(ControllerBase.SaveEvent item)
        {
            base.OnRemoveing(item);
        }
        /// <summary>
        /// 设置下推对象
        /// </summary>
        /// <returns></returns>
        public override ControllerBase PushController()
        {
            return new PurchaseInOrderController();
        }
        public override BusinessController bc()
        {
            return new PurchaseInOrderController();
        }
        #endregion

        #region 下推功能
        public override string GetThisController()
        {
            return new PurchaseInOrderController().ToString();
        }
     
        #endregion
    }
}
