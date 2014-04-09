using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Business.Controllers;
using Acc.Business.WMS.Model;
using Acc.Contract.MVC;
using Acc.Contract.Data.ControllerData;
using Way.EAP.DataAccess.Entity;

namespace Acc.Business.WMS.Controllers
{
    public class LoanOrderController : BusinessController
    {
        /// <summary>
        /// 描述：借件单管理控制器
        /// 作者：路聪
        /// 创建日期:2013-03-28
        /// </summary>
        public LoanOrderController() : base(new LoanOrder()) { }

        //是否启用审核
        public override bool IsReviewedState
        {
            get
            {
                return true;
            }
        }
        //是否启用提交
        public override bool IsSubmit
        {
            get
            {
                return true;
            }
        }
        //是否启用回收站
        public override bool IsClearAway
        {
            get
            {
                return false;
            }
        }

        //显示在菜单
        protected override string OnControllerName()
        {
            return "借件单管理";
        }
        //菜单中url路径
        protected override string OnGetPath()
        {
            return "Views/WMS/Check/LoanOrder.htm";
        }
        //开发人
        protected override string OnGetAuthor()
        {
            return "路聪";
        }
        //说明
        protected override string OnGetControllerDescription()
        {
            return "借件单管理";
        }

        #region 初始化数据方法
        protected override void OnInitViewChildItem(Contract.Data.ModelData data, Contract.Data.ItemData item)
        {
            if (data.name.EndsWith("LoanOrder"))
           {
                switch (item.field.ToLower())
                {
                    case "submiteddate":
                    case "submitedby":
                    case "modifiedby":
                    case "modifieddate":
                    case "reviewedby":
                    case "revieweddate":
                    case "issubmited":
                    case "isreviewed":
                    case "isdisable":
                    case "isdelete":
                    case "id":
                    case "code":
                        item.visible = false;
                        item.isedit = false;
                        break;
                    default:
                        item.visible = true;
                        break;
                }
            }
            if (data.name.EndsWith("LoanOrderMaterials"))
            {
                switch (item.field.ToLower())
                {
                    case "submiteddate":
                    case "submitedby":
                    case "modifiedby":
                    case "modifieddate":
                    case "reviewedby":
                    case "revieweddate":
                    case "issubmited":
                    case "isreviewed":
                    case "isdisable":
                    case "isdelete":
                    case "id":
                    case "code":
                    case "loanorderno":
                        item.visible = false;
                        item.isedit = false;
                        break;
                    default:
                        item.visible = true;
                        break;
                }
            }
        }

        #endregion

        #region 添加数据方法
        protected override void OnAdding(Contract.MVC.ControllerBase.SaveEvent item)
        {
            base.OnAdding(item);
            LoanOrder lo=item.Item as LoanOrder;
            foreach (LoanOrderMaterials loms in lo.Materials)
            {
                EntityList<StockInfoMaterials> sooList = new EntityList<StockInfoMaterials>(this.model.GetDataAction());
                sooList.GetData(" Code='"+loms.MATERIALCODE+"' and HOUSECODE='"+loms.WAREHOUSENAME+"'");
                foreach (StockInfoMaterials sm in sooList)
                {
                    sm.FREEZENUM = loms.NUM;
                }
                sooList.Save();
            }
        }
        #endregion

        /// <summary>
        /// 选择仓库的事件
        /// </summary>
        /// <param name="model"></param>
        /// <param name="item"></param>
        protected override void OnForeignLoading(IModel model, loadItem item)
        {
            if (this.fdata.filedname == "MATERIALCODE")
            {
                LoanOrderMaterials lom=model as LoanOrderMaterials;
                item.rowsql = "select * from (" + item.rowsql + ") a where a.ID in (select Code from Acc_WMS_StockInfo_Materials where HOUSECODE='" + lom.WAREHOUSENAME + "' )";
            }
            if (this.fdata.filedname == "WAREHOUSENAME")
            {
                item.rowsql = "select * from (" + item.rowsql + ") a where a.ID IN ( select HOUSECODE from Acc_WMS_StockInfo_Materials )";
            }
            base.OnForeignLoading(model, item);
        }
    }
}
