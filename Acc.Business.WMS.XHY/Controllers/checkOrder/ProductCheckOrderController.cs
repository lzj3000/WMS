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
    public class ProductCheckOrderController : XHY_CheckOrderController
    {
        public ProductCheckOrderController() : base(new ProCheckMaterials()) { }
        /// <summary>
        /// 描述：新华杨成品入库质检单控制器
        /// 作者：柳强
        /// 创建日期:2013-07-25
        /// </summary>

        #region 基础设置
      
        //显示在菜单
        protected override string OnControllerName()
        {
            return "生产入库质检";
        }
        //菜单中url路径
        protected override string OnGetPath()
        {
            return "Views/WMS/XHY/ProductCheck.htm";
        }

        //说明
        protected override string OnGetControllerDescription()
        {
            return "生产入库质检管理";
        }
        #endregion

        #region 初始化数据方法
        protected override void OnInitViewChildItem(Contract.Data.ModelData data, Contract.Data.ItemData item)
        {
            base.OnInitViewChildItem(data, item);
            //if (data.name.EndsWith("ProCheck"))
            //{
            //    data.title = "生产入库质检";
            //    switch (item.field.ToLower())
            //    {
            //        case "code":
            //            item.title = "生产质检单号";
            //            item.visible = true;
            //            item.disabled = true;
            //            break;        
            //        case "sourcecode":
            //            item.title = "成品入库单号";
            //            break;
            //    }
            //}
            if (data.name.EndsWith("ProCheckMaterials"))
            {
                data.title = "生产入库质检";
                switch (item.field.ToLower())
                {
                    case "code":
                        item.title = "生产质检单号";
                        item.visible = true;
                        item.disabled = true;
                        break;
                    case "sourcecode":
                        item.title = "生产赋码单号";
                        item.visible = true;
                        break;
                    case "batchno":
                        item.title = "生产批号";
                        break;
                    case "producetime":
                        item.title = "生产日期";
                        item.visible = true;
                        item.disabled = true;
                        break;
                }
            }
        }
        #endregion

        #region 重写基类方法
        public override string CheckTypeName()
        {
            return "2";
        }

        public override string SetRemark()
        {
            return "来源生产赋码单";
        }

        protected override void OnAdding(ControllerBase.SaveEvent item)
        {
            ProCheckMaterials pc = item.Item as ProCheckMaterials;
            
            base.OnAdding(item);

            if (!string.IsNullOrEmpty(pc.SourceCode) && !string.IsNullOrEmpty(pc.SourceID.ToString()))
            {
                string sql = "UPDATE dbo.Acc_WMS_InNoticeMaterials1 SET STAY3 = '是' WHERE code='" + pc.SourceCode + "'";
                this.model.GetDataAction().Execute(sql);
            }
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
        //    o.ControllerName = "Acc.Business.WMS.XHY.Controllers.ProduceInOrderController";
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
        //    ProCheckMaterials so = new ProCheckMaterials();
        //    ///入库单
        //    if (name != null && name == "Acc.Business.WMS.XHY.Controllers.ProduceInOrderController")
        //    {
        //        base.AddCheckMaterialsItem(item, name, so);
        //    }
        //}
        //#endregion

   
        #endregion
    }
}
