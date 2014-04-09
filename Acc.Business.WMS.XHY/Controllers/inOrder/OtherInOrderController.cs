using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Business.WMS.XHY.Model;
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
using Acc.Contract.Data.ControllerData;
using Acc.Contract.Data;

namespace Acc.Business.WMS.XHY.Controllers
{
    public class OtherInOrderController : XHY_BusinessInOrderController
    {
        public OtherInOrderController(): base(new XHY_StockInOrder()){}
        #region 页面设置
        //显示在菜单
        protected override string OnControllerName()
        {
            return "其他入库";
        }
        //菜单中url路径
        protected override string OnGetPath()
        {
            return "Views/WMS/XHY/OtherInOrder.htm";
        }
        //说明
        protected override string OnGetControllerDescription()
        {
            return "其他入库管理";
        }
        #endregion

        #region 初始化数据方法
        protected override void OnInitViewChildItem(Contract.Data.ModelData data, Contract.Data.ItemData item)
        {
            base.OnInitViewChildItem(data, item);
            if (data.name.EndsWith("StockInOrder"))
            {
                data.title = "其他入库";
                switch (item.field.ToLower())
                {
                    case "code":
                        item.title = "其他入库单号";
                        item.disabled = true;
                        break;

                    case "onrevieweddata":
                        item.visible = false;
                        break;
                    case "sourcecode":
                        item.visible = false;
                        break;
                    case "state":
                    case "sourcename":
                    case "sourceoutcode":
                    case "workerid":
                    case "stocktype":
                        item.visible = false;
                        break;
                    case "clientno":
                        item.title = "客户编码";
                        item.required = false;
                        break;
                    case "k3inordertype":
                        item.required = true;
                        item.visible = true;
                        break;
                    case "issynchronous":
                        item.visible = true;
                        break;
                    case "bumen":
                        item.visible = true;
                        item.required = true;
                        break;
                    case "towhno":
                        item.required = true;
                        break;
                }
            }
            if (data.name.EndsWith("StockInOrderMaterials"))
            {
                data.title = "其他入库明细";
                switch (item.field.ToLower())
                {
                    case "mcode":
                    case "fmodel":
                        item.disabled = true;
                        break;
                    case "code":
                    case "sourcecode":
                    case "parentid":
                    case "state":
                    case "reviewedby":
                    case "revieweddate":
                    case "isreviewed":
                    case "submiteddate":
                    case "submitedby":
                    case "createdby":
                    case "creationdate":
                    case "modifiedby":
                    case "modifieddate":
                    case "issubmited":
                    case "isdisable":
                        item.visible = false;
                        break;
                    case "depotwbs":
                        item.title = "货位编码";
                        item.required = false;
                        break;
                    case "portname":
                        item.title = "托盘编码";
                        item.required = false;
                        break;
                    case "batchno":
                        item.required = false;
                        break;
                    case "materialcode":
                        item.required = true;
                        break;
                }
            }
            if (data.name.EndsWith("StockInNoticeMaterials"))
            {
                data.visible = false;
            }
            if (data.name.EndsWith("LogData"))
            {
                switch (item.field.ToLower())
                {

                    case "logname":
                    case "sourcesystem":
                    case "sourcecode":
                    case "targetsystem":
                    case "targetcode":
                        item.visible = false;
                        break;
                }
            }
        }
        #endregion
        protected override void OnForeignIniting(IModel model, InitData data)
        {
            if (model is StockInOrder)
            {
                if (this.fdata.filedname == "CLIENTNO")
                {
                    foreach (ItemData d in data.modeldata.childitem)
                    {//序号、仓库编码Code、名称WAREHOUSENAME、类型WHTYPE、上级PARENTID、是否禁用IsDisable、备注Remark
                        d.visible = false;
                        if (d.IsField("ROWINDEX"))
                        {
                            d.visible = true;
                        }
                        if (d.IsField("CODE"))
                        {
                            d.visible = true;
                            d.title = "企业编码";
                        }
                        if (d.IsField("CUSTOMERNAME"))
                        {
                            d.visible = true;
                        }
                        if (d.IsField("FTYPE"))
                        {
                            d.visible = true;
                        }
                        if (d.IsField("FREGIONID"))
                        {
                            d.visible = true;
                        }
                        if (d.IsField("ISDISABLE"))
                        {
                            d.visible = true;
                        }
                        if (d.IsField("REMARK"))
                        {
                            d.visible = true;
                        }
                    }
                }
                else { base.OnForeignIniting(model, data); }
            }
            else { 
             base.OnForeignIniting(model, data); 
            }
        }
        #region 重写方法
        /// <summary>
        /// 无源单控制器，设置为“”
        /// </summary>
        /// <returns></returns>
        public override string InOrderController()
        {
            return "";
        }
        //SetStockType
        public override int SetStockType()
        {
            return 4;
        }

        protected override void OnAdding(ControllerBase.SaveEvent item)
        {
            NewVali(item);
            base.OnAdding(item);
        }

       

        private void NewVali(ControllerBase.SaveEvent item)
        {
            XHY_StockInOrder si = item.Item as XHY_StockInOrder;
            EntityList<XHY_Customer> ct = new EntityList<XHY_Customer>(this.model.GetDataAction());
            ct.GetData("ID='" + si.CLIENTNO + "'");
            if (si.K3InOrderType == 2)
            {
                if (ct[0].FTYPE != "1")
                {
                    throw new Exception("K3单据类型选择为 其他入库时，客户栏必需选填为供应商！");
                }
            }
            if (si.K3InOrderType == 3)
            {
                if (ct[0].FTYPE != "2")
                {
                    throw new Exception("K3单据类型选择为 红字销售出库时，客户栏必需选填为客户！");
                }
            }
        }

        public override void ValidatePort(ControllerBase.SaveEvent item)
        {
            
        }

        public override void ValidateDepotwbs(ControllerBase.SaveEvent item)
        {
            base.ValidateDepotwbs(item);
        }
        /// <summary>
        /// 验证明细行批次的方法
        /// </summary>
        /// <param name="item"></param>
        public override void ValidateBatchno(ControllerBase.SaveEvent item)
        {
            //StockInOrder stockinorder = item.Item as StockInOrder;
            //for (int i = 0; i < stockinorder.Materials.Count; i++)
            //{
            //    var sm = stockinorder.Materials[i];
            //    Materials a = sm.GetForeignObject<Materials>(this.model.GetDataAction());
            //    if (a.BATCH == true && sm.BATCHNO == "")
            //    {
            //        throw new Exception("序号:" + sm.RowIndex + " 商品名称:" + a.FNAME + "  商品编码：" + a.Code + "  为批次管理产品，批次号不能为空！");
            //    }
            //}
        }
        protected override void OnEditing(ControllerBase.SaveEvent item)
        {
            NewVali(item);
            base.OnEditing(item);
        }
        #endregion

       

        protected override void CheckTransformData()
        {
            base.CheckTransformData();
            XHY_StockInOrder mi = this.ActionItem as XHY_StockInOrder;
            if (mi.K3InOrderType == 2 || mi.K3InOrderType == 3)
            {
            }
            else 
            {
                throw new Exception("未选择K3单据同步类型，不符合同步条件！");
            }
        }

    }
}
