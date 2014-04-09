using System;
using System.Collections.Generic;
using System.Linq;
using Acc.Business.Controllers;
using Acc.Business.WMS.Model;
using Acc.Business.WMS.XHY.Model;
using Acc.Contract;
using Acc.Contract.Center;
using Acc.Contract.Data.ControllerData;
using Acc.Contract.MVC;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model;
using Acc.Contract.Data;
using Acc.Business.WMS.Controllers;
using Way.EAP.DataAccess.Regulation;
using System.Data;

namespace Acc.Business.WMS.XHY.Controllers
{
    public class DistributionOutOrderController : StockOutOrderController
    {
        /// <summary>
        /// 描述：新华杨配货管理控制器
        /// 作者：柳强
        /// 创建日期:2013-09-05
        /// </summary>


        #region 基础设置
        //显示在菜单
        protected override string OnControllerName()
        {
            return "配货管理";
        }
        //菜单中url路径
        protected override string OnGetPath()
        {
            return "Views/WMS/XHY/DistributionOutOrder.htm";
        }

        //说明
        protected override string OnGetControllerDescription()
        {
            return "配货管理";
        }

        /// <summary>
        /// 重写下推，隐藏了基类中的下推
        /// </summary>
        public override bool IsPushDown
        {
            get
            {
                return false;
            }
        }


        /// <summary>
        /// 初始化显示与否
        /// </summary>
        /// <param name="data"></param>
        /// <param name="item"></param>
        protected override void OnInitViewChildItem(Contract.Data.ModelData data, Contract.Data.ItemData item)
        {
            base.OnInitViewChildItem(data, item);
            if (data.name.EndsWith("StockOutOrder"))
            {
                //根据销售人员带出所在部门
                if (item.field == "SALEUSER")
                {
                    if (item.foreign != null)
                    {
                        item.foreign.rowdisplay.Add("OrganizationID".ToUpper(), "BUMEN");
                    }
                }
                data.title = "配送管理";
                switch (item.field.ToLower())
                {
                    case "carphone":
                    case "state":
                    case "stocktype":
                        item.visible = false;
                        break;
                    case "sourcecode":
                        item.visible = true;
                        item.title = "来源单号";
                        //item.foreign.isfkey = false;
                        //item.foreign.foreignobject = null;
                        break;
                    case "bumen":
                        item.visible = true;
                        break;
                }
            }
        }

        #endregion

        #region 取消外键
        protected override Contract.Data.ControllerData.ReadTable OnSearchData(Contract.Data.ControllerData.loadItem item)
        {
            ((IEntityBase)this.model).ForeignKey += new ForeignKeyHandler(StockPurchaseInOrderController_ForeignKey);
            return base.OnSearchData(item);
        }

        void StockPurchaseInOrderController_ForeignKey(EntityBase sender, Dictionary<string, EntityForeignKeyAttribute> items)
        {
            if (items.ContainsKey("SourceCode"))
            {
                items.Remove("SourceCode");
            }
        }
        #endregion


        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="item"></param>
        protected override void OnAdding(ControllerBase.SaveEvent item)
        {

            base.OnAdding(item);
            StockOutOrder ord = item.Item as StockOutOrder;
            ord.STOCKTYPE = 1;
        }


        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="item"></param>
        protected override void OnEditing(ControllerBase.SaveEvent item)
        {


            base.OnEditing(item);
            StockOutOrder ord = item.Item as StockOutOrder;
            ord.STOCKTYPE = 1;
        }

        protected override void OnSubmitData(BasicInfo info)
        {
            base.OnSubmitData(info);
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="info"></param>
        protected override void OnReviewedData(BasicInfo info)
        {

            base.OnReviewedData(info);
        }


        [WhereParameter]
        public string NoticeId { get; set; }
        public string GetNoticeStayGrid()
        {
            EntityList<StockInNoticeMaterials> List1 = new EntityList<StockInNoticeMaterials>(this.model.GetDataAction());
            List1.GetData("code='" + Convert.ToInt32(NoticeId) + "'");
            List<StockInNoticeMaterials> list = new List<StockInNoticeMaterials>();
            list = List1;
            string str = JSON.Serializer(list.ToArray());
            return str;
        }

        /// <summary>
        /// 定义监控的类和方法（观察者模式）
        /// </summary>
        /// <returns></returns>
        protected override ObserverClient[] OnGetObserverClient()
        {
            List<ObserverClient> list = new List<ObserverClient>();
            ObserverClient o = new ObserverClient();
            o.ControllerName = "Acc.Business.PRO.XHY.Controllers.XHYProductionPlanController";
            o.MethodName = "SubmitData";
            o.ObserverType = CustomeEventType.Complete;
            o.IsAsynchronous = true;
            o.SourceController = this;
            list.Add(o);
            return list.ToArray();
        }

        protected override void InceptNotify(EventBase item)
        {
            OnObserverProduction(item);
        }


        /// <summary>
        /// 观察生产计划单创建配货申请
        /// </summary>
        protected virtual void OnObserverProduction(EventBase item)
        {

            string name = item.Controller.GetType().FullName;
            EntityList<StockOutOrder> list = null;
            StockOutOrder so = new StockOutOrder();
            ///入库单
            if (name != null && name == "Acc.Business.PRO.XHY.Controllers.XHYProductionPlanController")
            {
                string title = ((IController)item.Controller).ControllerName();
                if (item.MethodName == "SubmitData" && item.EventType == CustomeEventType.Complete)
                {
                    DataTable dtCount = this.model.GetDataAction().GetDataTable("select * from Acc_WMS_InNotice sn where sn.id = '" + item.CustomeData["id"] + "'");
                    for (int i = 0; i < dtCount.Rows.Count; i++)
                    {
                        BillNumberController bnc = new BillNumberController();
                        PurCheckOrderController bc = new PurCheckOrderController();
                        so.Code = bnc.GetBillNo(bc);
                        so.Createdby = item.user.ID;
                        so.Creationdate = DateTime.Now;
                        so.SourceController = name;
                        so.SourceID = Convert.ToInt32(item.CustomeData["id"]);
                        so.SourceCode = item.CustomeData["Code"].ToString();
                        so.Remark = "来源生产计划";
                     
                        list = new EntityList<StockOutOrder>(this.model.GetDataAction());
                        list.Add(so);
                        list.Save();
                        ////添加出库单子集
                        DataTable dt =
                            this.model.GetDataAction()
                                .GetDataTable("select * from Acc_WMS_InNoticeMaterials sm where sm.parentid='" +
                                              item.CustomeData["id"] + "'");
                        foreach (DataRow e in dt.Rows)
                        {

                            StockOutOrderMaterials sm = new StockOutOrderMaterials();
                            sm.SourceCode = so.Code;
                            sm.SourceController = this.ToString();
                            sm.SourceID = so.ID;
                            sm.SourceRowID = Convert.ToInt32(e["ID"]);
                            sm.SourceTable = e.ToString();
                            sm.MATERIALCODE = e["MATERIALCODE"].ToString();
                            sm.FMODEL = e["FMODEL"].ToString();
                            sm.MCODE = e["MCODE"].ToString();
                            sm.NUM = Convert.ToDouble(e["NUM"]);
                            so.Materials.Add(sm);
                        }
                        ///重新保存出库单     

                        list.Save();

                    }

                }
            }
        }
    }
}
