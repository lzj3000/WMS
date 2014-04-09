using System;
using Acc.Business.Model;
using Acc.Business.WMS.Controllers;
using Acc.Business.WMS.Model;
using Acc.Contract.Data;
using Acc.Contract.Data.ControllerData;
using Way.EAP.DataAccess.Entity;
using System.Collections.Generic;
using Acc.Contract;

namespace Acc.Business.WMS.XHY.Controllers
{
    public class SamplingOutOrderController : StockOutOrderController
    {
        /// <summary>
        ///     描述：新华杨取样出库控制器
        ///     作者：柳强
        ///     创建日期:2013-08-29
        /// </summary>

        #region 初始化数据方法

        #region 基础设置

        //显示在菜单
        protected override string OnControllerName()
        {
            return "取样出库";
        }

        //菜单中url路径
        protected override string OnGetPath()
        {
            return "Views/WMS/XHY/SamplingOutOrder.htm";
        }

        //说明
        protected override string OnGetControllerDescription()
        {
            return "取样出库管理";
        }

        /// <summary>
        ///     重写下推，隐藏了基类中的下推
        /// </summary>
        public override bool IsPushDown
        {
            get { return true; }
        }

        #endregion

        /// <summary>
        ///     初始化显示与否
        /// </summary>
        /// <param name="data"></param>
        /// <param name="item"></param>
        protected override void OnInitViewChildItem(ModelData data, ItemData item)
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
                data.title = "取样出库";
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

        //#region 取消外键
        //protected override Contract.Data.ControllerData.ReadTable OnSearchData(Contract.Data.ControllerData.loadItem item)
        //{
        //    ((IEntityBase)this.model).ForeignKey += new ForeignKeyHandler(StockPurchaseInOrderController_ForeignKey);
        //    return base.OnSearchData(item);
        //}

        //void StockPurchaseInOrderController_ForeignKey(EntityBase sender, Dictionary<string, EntityForeignKeyAttribute> items)
        //{
        //    if (items.ContainsKey("SourceCode"))
        //    {
        //        items.Remove("SourceCode");
        //    }
        //}
        //#endregion

        /// <summary>
        ///     添加
        /// </summary>
        /// <param name="item"></param>
        protected override void OnAdding(SaveEvent item)
        {
            base.OnAdding(item);
            var ord = item.Item as StockOutOrder;
            ord.STOCKTYPE = 3;
        }


        /// <summary>
        ///     编辑
        /// </summary>
        /// <param name="item"></param>
        protected override void OnEditing(SaveEvent item)
        {
            base.OnEditing(item);
            var ord = item.Item as StockOutOrder;
            ord.STOCKTYPE = 3;
        }

        protected override void OnSubmitData(BasicInfo info)
        {
            base.OnSubmitData(info);
        }

        /// <summary>
        ///     审核
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
            EntityList<StockOutNoticeMaterials> List1 = new EntityList<StockOutNoticeMaterials>(this.model.GetDataAction());
            List1.GetData(" PARENTID='" + Convert.ToInt32(NoticeId) + "'");
            List<StockOutNoticeMaterials> list = new List<StockOutNoticeMaterials>();
            list = List1;
            string str = JSON.Serializer(list.ToArray());
            return str;
        }
    }
}
