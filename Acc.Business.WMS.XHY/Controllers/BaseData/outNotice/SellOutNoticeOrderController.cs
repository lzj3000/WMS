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
    public class SellOutNoticeOrderController : XHY_OutNoticeOrderController
    {
        public SellOutNoticeOrderController(): base(new XHY_OutNoticeOrder()){}
        #region 页面设置
        public override bool IsPushDown
        {
            get
            {
                return true;
            }
        }
       
        //菜单中url路径
        protected override string OnGetPath()
        {
            return "Views/WMS/XHY/SellOutOrderNotice.htm";
        }

        protected override string OnControllerName()
        {
            return "销售出库通知";
        }

       
        #endregion

        #region 初始化数据方法
        protected override void OnInitViewChildItem(Contract.Data.ModelData data, Contract.Data.ItemData item)
        {
            base.OnInitViewChildItem(data, item);
            data.isadd = false;
            data.isedit = false;
            data.isremove = false;
            if (data.name.EndsWith("XHY_OutNoticeOrder"))
            {
                data.title = "销售出库通知";
                switch (item.field.ToLower())
                {
                    case "rowindex":
                         item.visible = true;
                        item.disabled = true;
                        break;
                    case "code":
                        item.title = "发货通知单号";
                        item.visible = true;
                        item.disabled = true;
                        break;
                    case "clientno": item.title = "客户编码";
                        item.index = 4;
                        item.disabled = true;
                        break;
                    case "sourcecode":
                        item.title = "销售订单号";
                        item.disabled = true;
                        break;
                    case "stay5":
                        item.visible = false;
                        item.title = "车间";
                        break;
                    case "sourcename":
                    case "sourceoutcode":
                    case "workerid":
                    case "bumen":
                    case "stocktype":
                    case "finishtime":
                    case "isdisable":
                    case "reviewedby":
                    case "revieweddate":
                    case "isreviewed":
                    case "state":
                    case "outrule":
                    case "issubmited":
                    case "submitedby":
                        item.visible = false;
                        break;
                    case "submiteddate":
                    case "modifiedby":
                    case "modifieddate": 
                        item.disabled = true;
                        break;
                    case "stay3":
                        item.disabled = true;
                        item.index = 5;
                        item.visible = true;
                        item.title = "已下推";
                        break;
                    case "creationdate":
                        item.index = 11;
                        item.disabled = true;
                        break;
                    case "createdby":
                        item.index = 10;
                        item.disabled = true;
                        break;
                    case "remark":
                        item.index = 7;
                        break;
                    case "khlx":
                        item.disabled = true;
                          item.index = 6;
                        break;
                    case "zdkh":
                        item.disabled = true;
                          item.index = 5;
                        break;
                    case "jhfs":
                        item.disabled = true;
                          item.index = 7;
                        break;
                    case "ywdw":
                        item.disabled = true;
                          item.index = 3;
                        break;


                }
            }
            if (data.name.EndsWith("StockOutNoticeMaterials"))
            {
                data.title = "销售出库通知明细";
                switch (item.field.ToLower())
                {
                    case "sourcecode":
                    case "parentid":
                    case "batchno":
                        item.visible = false;
                        break;
                    case "num":
                        item.title = "销售件数";
                        break;
                    case "stay6":
                        item.visible = true;
                        item.title = "销售数量(公斤)";
                        item.disabled = true;
                        break;
                    case "staynum":
                        item.title = "待操作数(件数)";
                        item.disabled = true;
                        break;
                    case "finishnum":
                        item.title = "已操作数(件数)";
                        item.disabled = true;
                        break;
                }
            }
        }
        #endregion

        #region 重写方法
        protected override void OnForeignLoading(IModel model, Contract.Data.ControllerData.loadItem item)
        {
            if (this.fdata.filedname == "MATERIALCODE")
            {
                item.rowsql = "select * from (" + item.rowsql + ") a where a.CommodityType='2'";
            }

            else
            {
                base.OnForeignLoading(model, item);
            }
        }

   

        protected override void OnEditing(ControllerBase.SaveEvent item)
        {
            base.OnEditing(item);
        }

        /// <summary>
        /// 设置stocktype =2
        /// </summary>
        /// <returns></returns>
        public override int SetStockType()
        {
            return 2;
        }
        /// <summary>
        /// 设置下推对象
        /// </summary>
        /// <returns></returns>
        public override ControllerBase PushController()
        {
            return new SellOutOrderController();
        }
        #endregion

        #region 下推

        public override string GetThisController()
        {
            return new SellOutOrderController().ToString();
        }

        protected override void SellOutOrder(XHY_StockOutOrder ebin,XHY_OutNoticeOrder cd)
        {
            XHY_BusinessOutOrderController so = new XHY_BusinessOutOrderController();
            EntityList<XHY_Materials> xmList = new EntityList<XHY_Materials>(this.model.GetDataAction());
            ebin.TOWHNO = 5;
            string cnamea = ebin.GetAppendPropertyKey("zdkh");
            string ccnamea = cd.GetAppendPropertyKey("zdkh");
            ebin.zdkh = cd.zdkh;
            ebin[cnamea] = cd[ccnamea];

            EntityList<XHY_Customer> xhyCustomer = new EntityList<XHY_Customer>(this.model.GetDataAction());
            xhyCustomer.GetData("ID='" + ebin.CLIENTNO + "'");
            if (xhyCustomer.Count > 0)
            {
                ebin.LXRName = xhyCustomer[0].LXRName;
                ebin.LXRPhone = xhyCustomer[0].LXRPhone;
                ebin.LXRAddress = xhyCustomer[0].LXRAddress;
                ebin.zdtel = xhyCustomer[0].zdtel;
        
            }

            EntityList<XHY_Customer> xhyzdCustomer = new EntityList<XHY_Customer>(this.model.GetDataAction());
            xhyzdCustomer.GetData("ID='" + ebin.zdkh + "'");
            if (xhyzdCustomer.Count > 0)
            {
                ebin.zdlxr = xhyzdCustomer[0].LXRName;
                ebin.zdphone = xhyzdCustomer[0].LXRPhone;
                ebin.zdaddress = xhyzdCustomer[0].LXRAddress;
                ebin.zdtel = xhyCustomer[0].zdtel;
            }
            for (int i = 0; i < ebin.Materials.Count; i++)
            {
                //ebin.Materials[i].MCODE = ebin.Materials[i].GetForeignObject<Materials>(this.model.GetDataAction()).Code;
                //ebin.Materials[i].FMODEL = ebin.Materials[i].GetForeignObject<Materials>(this.model.GetDataAction()).FMODEL;
                xmList.GetData("id='" + ebin.Materials[i].MATERIALCODE + "'");
                if (xmList.Count > 0)
                {
                    string wh = xmList[0].WeightNUM.ToString();
                    ebin.Materials[i].STAY5 = wh;//包装（单重）
                   // ebin.Materials[i].STAY6 = Convert.ToDouble(wh) * ebin.Materials[i].NUM;//数量kg
                }

            }
        }

        protected override void OnRemoveing(ControllerBase.SaveEvent item)
        {
            base.OnRemoveing(item);
        }

       #endregion
        /// <summary>
        /// 设置按钮显示名称
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        protected override ActionCommand[] OnInitCommand(ActionCommand[] commands)
        {
            ActionCommand[] coms = base.OnInitCommand(commands);
            //获取所有按钮集合
            foreach (ActionCommand ac in coms)
            {
                if (ac.command == "UnSubmitData")
                {
                    ac.visible = false;
                }
                if (ac.command == "add")
                {
                    ac.visible = false;
                }
                if (ac.command == "edit")
                {
                    ac.visible = true;
                }
               
                if (ac.command == "SubmitData")
                {
                    ac.visible = false;
                }
            }
            return coms;
        }

    }
}
