using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Business.WMS.XHY.Model;
using Acc.Contract.MVC;
using Acc.Business.WMS.Model;
using Acc.Contract;
using Acc.Business.Controllers;
using Way.EAP.DataAccess.Entity;
using Acc.Contract.Center;
using Acc.Contract.Data;
using Acc.Business.WMS.Controllers;
using Way.EAP.DataAccess.Regulation;
using Acc.Contract.Data.ControllerData;
using Acc.Business.Model;

namespace Acc.Business.WMS.XHY.Controllers
{
    public class PurchaseInOrderController : XHY_BusinessInOrderController
    {
        public PurchaseInOrderController() : base(new StockInOrder()) { }
        #region 页面设置

        //显示在菜单
        protected override string OnControllerName()
        {
            return "外购入库单";
        }
        //菜单中url路径
        protected override string OnGetPath()
        {
            return "Views/WMS/XHY/PurchaseInOrder.htm";
        }
        //说明
        protected override string OnGetControllerDescription()
        {
            return "外购入库管理";
        }
        #endregion
        protected override void OnForeignIniting(IModel model, InitData data)
        {
            if (model is StockInOrder)
            {
                if (this.fdata.filedname == "SOURCECODE")
                {
                    foreach (ItemData d in data.modeldata.childitem)
                    {//Createdby Creationdate Submitedby  Submiteddate  IsSubmited
                        d.visible = false;
                        if (d.IsField("ROWINDEX"))
                        {
                            d.visible = true;
                            d.index = 1;
                        }
                        if (d.IsField("CREATEDBY"))
                        {
                            d.visible = true;
                            d.title = "创建人";
                        }
                        if (d.IsField("CREATIONDATE"))
                        {
                            d.visible = true;
                        }
                        if (d.IsField("SUBMITEDBY"))
                        {
                            d.visible = true;
                        }
                        if (d.IsField("SUBMITEDDATE"))
                        {
                            d.visible = true;
                        }
                        if (d.IsField("ISSSUBMITED"))
                        {
                            d.visible = true;
                        }
                        if (d.IsField("CODE"))
                        {
                            d.visible = true;
                            d.title = "收料通知单号";
                            d.index = 2;
                        }
                        if (d.IsField("CLIENTNO"))
                        {
                            d.visible = true;
                            d.title = "供应商编码";
                            d.index = 5;
                        }
                    } return;
                }
                else {
                    base.OnForeignIniting(model, data);
                }
            }
            else
            {
                base.OnForeignIniting(model, data);
            }
        }
        #region 初始化数据方法
        protected override void OnInitViewChildItem(Contract.Data.ModelData data, Contract.Data.ItemData item)
        {
            base.OnInitViewChildItem(data, item);
            if (data.name.EndsWith("StockInOrder"))
            {
                data.title = "外购入库";
                switch (item.field.ToLower())
                {
                    case "code":
                        item.title = "外购入库单号";
                        item.visible = true;
                        item.disabled = true;
                        break;
                    case "sourcecode":
                        item.title = "收料通知单号";
                        item.visible = true;
                        break;

                    case "clientno":
                        item.title = "供应商编码";
                        item.visible = true;
                        item.disabled = true;
                        break;
                    case "issynchronous":
                        item.visible = true;
                        break;

                    case "state":
                    case "sourcename":
                    case "sourceoutcode":
                    case "workerid":
                    case "bumen":
                    case "stocktype":
                        item.visible = false;
                        break;
                }
                if (item.IsField("SOURCECODE"))
                {
                    if (item.foreign != null)
                    {
                        item.foreign.rowdisplay.Add("CLIENTNO", "CLIENTNO");
                    }
                }
            }
            if (data.name.EndsWith("StockInOrderMaterials"))
            {
                data.title = "外购入库明细";
                switch (item.field.ToLower())
                {
                    case "batchno":
                        item.title = "批次号";
                        item.required = true;
                        break;
                    case "num": item.title = "入库数量";
                        break;
                    case "depotwbs":
                        item.title = "货位编码";
                        item.required = true;
                        break;
                    case "state":
                        item.visible = false;
                        break;
                    case "portname":
                        item.required = false;
                        break;
                }
                if (item.IsField("materialcode"))
                {
                    if (item.foreign != null)
                    {
                        item.foreign.rowdisplay.Add("STAY1".ToLower(), "num");
                    }
                }
            }
            if (data.name.EndsWith("InSequence"))
            {
                data.visible = false;
            }
            if (data.name.EndsWith("StockInNoticeMaterials"))
            {
                switch (item.field.ToLower())
                {
                    case "batchno":
                        item.visible = false;
                        break;
                    case "staynum":
                         item.visible =true;
                         item.title = "待操作数";
                        break;
                }
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

        #region 重写方法
        public override void ValidateBatchno(ControllerBase.SaveEvent item)
        {
            //base.ValidateBatchno(item);
            //string aa = DateTime.Now.ToString("yyMMdd");
            //StockInOrder stockinorder = item.Item as StockInOrder;
            //for (int i = 0; i < stockinorder.Materials.Count; i++)
            //{
            //    var sm = stockinorder.Materials[i];
            //    if (sm.BATCHNO.Length<6) {
            //            sm.BATCHNO = aa + sm.BATCHNO;
            //    }
            //    else if (sm.BATCHNO.Substring(0, 6)==aa)
            //    {
            //    }
            //    else if (sm.BATCHNO.Substring(0, 6) != aa)
            //    {
            //        sm.BATCHNO = aa + sm.BATCHNO;
            //    }
            //}
        }
        protected override void OnAdding(ControllerBase.SaveEvent item)
        {
            base.OnAdding(item);
        }

        protected override void OnEditing(ControllerBase.SaveEvent item)
        {
            base.OnEditing(item);
        }

        protected override void OnSubmitData(Business.Model.BasicInfo info)
        {
            base.OnSubmitData(info);
            InsertIntoProCheckMaterials(info);
        }

        /// <summary>
        /// 向外购入库质检单插入记录
        /// </summary>
        /// <param name="info"></param>
        private void InsertIntoProCheckMaterials(BasicInfo info)
        {
            StockInOrder sio =info as StockInOrder;
            sio = base.getinfo(sio.ID.ToString()) as StockInOrder;
           
            sio.Materials.DataAction = this.model.GetDataAction();
            sio.Materials.GetData();
            EntityList<ProCheckMaterials> list = new EntityList<ProCheckMaterials>(this.model.GetDataAction());
            ProCheckMaterials pcm;
            PurCheckOrderController pco = new PurCheckOrderController();
            BillNumberController bnc;
            StockInOrderMaterials sim;
            //var result = from o in sio.Materials group o by new { o.MATERIALCODE, o.BATCHNO } into g select new { g.Key, Totle = g.Sum(p => p.NUM), Items = g.ToList<StockInOrderMaterials>() };
            var result = LinqHelp.XHY_MergerStockInOrderMaterials(sio.Materials);
            foreach (var r in result)
            {
               bnc = new BillNumberController();
                pcm = new ProCheckMaterials();
                string strSql = string.Format("select * from " + pcm.ToString() + " where CHECKWARE='{0}' and BATCHNO ='{1}'", r.MATERIALCODE, r.BATCHNO);
                object obj = this.model.GetDataAction().GetValue(strSql);
                if (obj == null || string.IsNullOrEmpty(obj.ToString()))
                {
                    sim = r;
                    pcm.BATCHNO = sim.BATCHNO;
                    pcm.CHECKWARE = sim.MATERIALCODE;
                    if (string.IsNullOrEmpty(sio.SourceCode))
                    {
                        //pcm.STAY1 = sio.SourceCode
                    }
                    pcm.STAY1 = sio.SourceCode;
                    pcm.MCODE = sim.MCODE;
                    pcm.FMODEL = sim.FMODEL;
                    pcm.Createdby = sio.Createdby;
                    pcm.Creationdate = DateTime.Now;
                    pcm.IsOK = false;
                    pcm.SourceID = sio.ID;
                    pcm.SourceCode = sio.Code;
                    pcm.SourceController = "Acc.Business.WMS.XHY.Controllers.PurchaseInOrderController";
                    pcm.CheckTypeName = "1";
                    pcm.Code = bnc.GetBillNo(pco);
                    list.Add(pcm);
                }
            }
            list.Save();
        }

        /// <summary>
        /// 制定源单控制器为采购入库通知单
        /// </summary>
        /// <returns></returns>
        public override string InOrderController()
        {
            return new PurchaseInNoticeOrderController().ToString();
        }

        /// <summary>
        /// 设置stocktype = 2
        /// </summary>
        /// <returns></returns>
        public override int SetStockType()
        {
            return 2;
        }
        protected override void foreignInOrder(IModel model, loadItem item)
        {
            if (this.fdata.filedname == "SOURCECODE")
            {
                item.rowsql = "select * from (" + item.rowsql + ") a where a.code in(select code from " + this.fdata.tablename + " where stocktype =  " + new PurchaseInNoticeOrderController().SetStockType() + " and IsSubmited=1)";
            }
            else
            {
                base.foreignInOrder(model, item);
            }
        }
        public override string GetNoticeStayGrid()
        {
            return base.GetNoticeStayGrid();
        }
        #endregion





    }
}
