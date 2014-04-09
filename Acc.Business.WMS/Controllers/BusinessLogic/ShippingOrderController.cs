using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Business.WMS.Model;
using Acc.Business.Controllers;
using Way.EAP.DataAccess.Entity;
using Acc.Contract.Data;
using Way.EAP.DataAccess.Data;
using Acc.Business.Model;

namespace Acc.Business.WMS.Controllers
{
    public class ShippingOrderController : BusinessController
    {
        
        /// <summary>
        /// 描述：发运通知控制器
        /// 作者：柳强
        /// 创建日期:2012-12-18
        /// </summary>
        public ShippingOrderController() : base(new ShippingOrder()) { }

        //显示在菜单
        protected override string OnControllerName()
        {
            return "发运通知";
        }
        //菜单中url路径
        protected override string OnGetPath()
        {
            return "Views/WMS/StockOutOrder/ShippingOrder.htm";
        }
        //开发人
        protected override string OnGetAuthor()
        {
            return "柳强";
        }
        //说明
        protected override string OnGetControllerDescription()
        {
            return "发运通知管理";
        }

        /// <summary>
        /// 启用提交
        /// </summary>
        public override bool IsSubmit
        {
            get
            {
                return true;
            }
        }

        public override bool IsReviewedState
        {
            get
            {
                return true;
            }
        }

        public override bool IsClearAway
        {
            get
            {
                return false;
            }
        }

        //#region

        //[ActionCommand(name = "冻结", title = "冻结发运单", index = 6, isalert = true, icon = "icon-search", onclick = "SetDisable")]
        //public void SetDisable()
        //{ }
        //#endregion

        #region 初始化数据方法
        protected override void OnInitViewChildItem(Contract.Data.ModelData data, Contract.Data.ItemData item)
        {
            if (data.name.EndsWith("ShippingOrder"))
            {
                switch (item.field.ToLower())
                {
                    //case "submiteddate":
                    //case "submitedby":
                    //case "createdby":
                    //case "creationdate":
                    case "modifiedby":
                    case "modifieddate":
                    //case "reviewedby":
                    //case "revieweddate":
                    case "isdisable":
                    case "isdelete":
                    case "remark":
                    case "id":
                    case "unit2":
                    case "unit3":
                    case "conversion":
                    case "alarmearlieramount":
                    case "state":
                    case "stocktype":
                    case "parentno":
                        item.visible = false;
                        break;
                    case "submiteddate":
                    case "submitedby":
                    case "createdby":
                    case "creationdate":
                    case "reviewedby":
                    case "revieweddate":
                    case "issubmited":
                    case "isreviewed":
                    case "sourcecode":
                        item.disabled = true;
                        item.visible = true;
                        break;
                    case "code":
                        item.disabled = true;
                        item.title = "发运单号";
                        break;
                    default:
                        item.visible = true;
                        break;
                }
            }
            if (data.name.EndsWith("ShippingOrderMaterials"))
            {
                data.isadd = false;
                data.isedit = false;
                data.isremove = false;
                switch (item.field.ToLower())
                {
                    case "shippingmaterials":
                    case "shippingnum":
                    case "batchno":
                    case "senbatchno":
                    case "mcode":
                    case "fmodel":
                        item.visible = true;
                        break;
                    default :
                        item.visible = false;
                        break;
                }
            }
        }


        ///// <summary>
        ///// 自动获取发运通知号
        ///// </summary>
        ///// <returns></returns>
        //public string GetShippingOrder()
        //{
        //    return "FYD"+WMShelp.GetStockOutOrderNo(this.model);
        //}

        #endregion 

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="item"></param>
        protected override void OnAdding(Contract.MVC.ControllerBase.SaveEvent item)
        {
            base.OnAdding(item);

        }

        protected override void OnEditing(Contract.MVC.ControllerBase.SaveEvent item)
        {
            base.OnEditing(item);
        }

        //private void NewMethod(BasicInfo item)
        //{
        //    EntityList<ShippingOrder> soList = new EntityList<ShippingOrder>(this.model.GetDataAction());
        //    ShippingOrder so = item as ShippingOrder;
        //    soList.Add(so);
        //    soList.Save();
        //    EntityList<LogisticsShipping> lsList = new EntityList<LogisticsShipping>(this.model.GetDataAction());
        //    LogisticsShipping ls = new LogisticsShipping();
        //    lsList.GetData("LOGISTICSID = '" + so.LOGISTICSCOMPANY + "' and SHIPPINGORDERID = '" + so.ID + "' and SourceCode='" + so.Code + "'");

        //    ls.SHIPPINGORDERID = so.ID;
        //    ls.LOGISTICSID = so.LOGISTICSCOMPANY;
        //    ls.CUSTOMERID = so.CLIENTNO;
        //    switch (so.SHIPPINGTYPE)
        //    {
        //        case 1:
        //            ls.SHIPPINGTYPEID = "销售发运"; break;
        //        case 2:
        //            ls.SHIPPINGTYPEID = "维修发运"; break;
        //        case 3:
        //            ls.SHIPPINGTYPEID = "采购发运"; break;
        //    }
        //    ls.LOGISTICSNOID = so.LOGISTICSNO;
        //    ls.SourceID = so.ID.ToString();
        //    ls.SourceController = "Acc.Business.WMS.Controllers.ShippingOrderController";
        //    ls.SourceCode = so.Code;
        //    string sql = "";
        //    for (int i = 0; i < lsList.Count; i++)
        //    {
        //        sql += "delete from Acc_WMS_LogisticsShipping where id in('"+lsList[i].ID+"');";
                
        //    }
        //    IDataAction action = this.model.GetDataAction();
        //    action.Execute(sql);
        //    lsList.Clear();
        //    lsList.Add(ls);
        //    lsList.Save();
        //}

        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="info"></param>
        protected override void OnSubmitData(Business.Model.BasicInfo info)
        {
            base.OnSubmitData(info);
            

        }

        protected override void OnReviewedData(Business.Model.BasicInfo info)
        {
            base.OnReviewedData(info);
            ShippingOrder order = info as ShippingOrder;
            order.Materials.GetData();
            ///获取出库通知和子集的数据
            var soo = order.GetForeignObject<StockOutOrder>(this.model.GetDataAction());
            soo.Materials.GetData();
            IDataAction action = this.model.GetDataAction();
            string sql = "";
            for (int i = 0; i < order.Materials.Count; i++)
            {
                sql += "update Acc_WMS_OutOrderMaterials set NUMDH = '" + order.Materials[i].SHIPPINGNUM + "' where materialcode = '" + order.Materials[i].SHIPPINGMATERIALS + "' and parentID = '" + order.Materials[i].SourceID + "';";
            }
            action.Execute(sql);
        }

        protected override void OnForeignLoading(Contract.MVC.IModel model, Contract.Data.ControllerData.loadItem item)
        {
            base.OnForeignLoading(model, item);
            if (this.fdata.filedname == "LOGISTICSCOMPANY")
            {
                item.rowsql = "select * from (" + item.rowsql + ") a where a.issubmited='1' ";
            }
        }
    }
}
