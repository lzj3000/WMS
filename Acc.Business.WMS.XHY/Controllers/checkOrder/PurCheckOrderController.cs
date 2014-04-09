using System;
using System.Collections.Generic;
using System.Linq;
using Acc.Business.Controllers;
using Acc.Business.WMS.Model;
using Acc.Contract.Center;
using Acc.Contract.Data.ControllerData;
using Acc.Contract.MVC;
using Way.EAP.DataAccess.Entity;
using Acc.Business.WMS.Controllers;
using System.Data;
using Acc.Business.WMS.XHY.Model;
using Way.EAP.DataAccess.Regulation;
using Acc.Contract.Data;
using Acc.Business.Model;
namespace Acc.Business.WMS.XHY.Controllers
{
    public class PurCheckOrderController : XHY_CheckOrderController
    {
        public PurCheckOrderController() : base(new ProCheckMaterials()) { }
        #region 页面设置
       
        //显示在菜单
        protected override string OnControllerName()
        {
            return "采购入库质检";
        }
        //菜单中url路径
        protected override string OnGetPath()
        {
            return "Views/WMS/XHY/PurCheck.htm";
        }
       
        //说明
        protected override string OnGetControllerDescription()
        {
            return "采购质检管理";
        }
        #endregion

        #region 初始化数据方法
        protected override void OnInitViewChildItem(Contract.Data.ModelData data, Contract.Data.ItemData item)
        {
            base.OnInitViewChildItem(data, item);
            //if (data.name.EndsWith("ProCheck"))
            //{
            //    data.title = "采购入库质检";
            //    switch (item.field.ToLower())
            //    {
            //        case "code":
            //            item.title = "采购质检单号";
            //            item.visible = true;
            //            item.disabled = true;
            //            break;        
            //        case "sourcecode":
            //            item.title = "外购入库单号";
            //            break;
            //    }
            //}
            if (data.name.EndsWith("ProCheckMaterials"))
            {
                data.title = "采购入库质检";
                switch (item.field.ToLower())
                {
                    case "code":
                        item.title = "采购质检单号";
                        item.visible = true;
                        item.disabled = true;
                        break;
                    case "stay1":
                        item.title = "采购订单号";
                        item.visible = true;
                        item.disabled = true;
                        item.index = 2;
                        break;
                    case "sourcecode":
                        item.title = "外购入库单号";
                        item.visible = true;
                        break;
                   
                }
            }
        }
        #endregion

        #region 重写方法
        public override string CheckTypeName()
        {
            return "1";
        }

        public override string CreateBy()
        {
            return "1";
        }

        public override string SetRemark()
        {
            return "来源外购入库单";
        }

        //#region 观察方法

        ///// <summary>
        ///// 定义监控的类和方法（观察者模式）
        ///// </summary>
        ///// <returns></returns>
        //protected override ObserverClient[] OnGetObserverClient()
        //{
        //    List<ObserverClient> list = new List<ObserverClient>();
        //    ObserverClient o = new ObserverClient();
        //    o.ControllerName = "Acc.Business.WMS.XHY.Controllers.PurchaseInOrderController";
        //    o.MethodName = "SubmitData";
        //    o.ObserverType = CustomeEventType.Complete;
        //    o.IsAsynchronous = true;
        //    o.SourceController = this;
        //    list.Add(o);
        //    return list.ToArray();
        //}

        //protected override void InceptNotify(EventBase item)
        //{
        //    OnObserverProduction(item);
        //}

        ///// <summary>
        ///// 观察采购入库单(根据原单明细行物料大类分别创建质检单)
        ///// </summary>
        //protected void OnObserverProduction(EventBase item)
        //{
        //    string name = item.Controller.GetType().FullName;
        //    //EntityList<ProCheckMaterials> list= null;
        //    ProCheckMaterials so = new ProCheckMaterials();
        //    ///入库单
        //    if (name != null && name == "Acc.Business.WMS.XHY.Controllers.PurchaseInOrderController")
        //    {
        //        CreateBy();
        //        base.AddCheckMaterialsItem(item, name, so);
        //    }
        //}
    
        #endregion
    }
}
