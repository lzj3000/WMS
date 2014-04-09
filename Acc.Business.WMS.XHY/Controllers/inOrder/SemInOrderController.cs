using System;
using System.Collections.Generic;
using System.Linq;
using Acc.Business.Controllers;
using Acc.Business.WMS.Model;
using Acc.Contract.Center;
using Acc.Contract.Data.ControllerData;
using Acc.Contract.MVC;
using Way.EAP.DataAccess.Entity;
using Acc.Contract;
using Acc.Business.Model;
using Acc.Contract.Data;
using Acc.Business.WMS.Controllers;
using Acc.Business.WMS.XHY.Model;
using Way.EAP.DataAccess.Regulation;

namespace Acc.Business.WMS.XHY.Controllers
{
    public class SemInOrderController : XHY_BusinessInOrderController
    {
        public SemInOrderController() : base(new StockInOrder()) { }
        /// <summary>
        /// 描述：新华杨半成品入库控制器
        /// 作者：柳强
        /// 创建日期:2013-10.09
        /// </summary>
        #region 页面设置

        //显示在菜单
        protected override string OnControllerName()
        {
            return "半成品入库";
        }
        //菜单中url路径
        protected override string OnGetPath()
        {
            return "Views/WMS/XHY/SemInOrder.htm";
        }
       
        //说明
        protected override string OnGetControllerDescription()
        {
            return "半成品入库管理";
        }

        /// <summary>
        /// 半成品特殊，启用审核
        /// </summary>
        public override bool IsReviewedState
        {
            get
            {
                return true;
            }
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
                            d.title = "生产计划单号";
                            d.index = 2;
                        }
                        if (d.IsField("STAY5"))
                        {
                            d.visible = true;
                            d.title = "车间";
                            d.index = 3;
                        }
                    } return;
                }
                else
                {
                    base.OnForeignIniting(model, data);
                }
            }
            else
            {
                base.OnForeignIniting(model, data);
            }
        }
        #region 初始化数据方法

       
        /// <summary>
        /// 初始化显示与否
        /// </summary>
        /// <param name="data"></param>
        /// <param name="item"></param>
        protected override void OnInitViewChildItem(Contract.Data.ModelData data, Contract.Data.ItemData item)
        {

            base.OnInitViewChildItem(data, item);
            if (data.name.EndsWith("StockInOrder"))
            {
                if (item.IsField("SOURCECODE"))
                {
                    if (item.foreign != null)
                    {
                        item.foreign.rowdisplay.Add("STAY5", "stay5");
                    }
                }
                data.title = "半成品入库";
                switch (item.field.ToLower())
                {
                    case "code":
                        item.disabled = true;
                        item.title = "半成品入库单号";
                        break;
                    case "sourcecode":
                        item.title = "生产计划通知单号";
                        break;
                    case "state":
                    case "sourcename":
                    case "sourceoutcode":
                    case "workerid":
                    case "bumen":
                    case "stocktype":
                    case "clientno":
                        item.visible = false;
                        break;
                    case "reviewedby":
                    case "revieweddate":
                        item.visible = true;
                        item.disabled = true;
                        break;
                    case "issubmited":
                    case "isreviewed":
                    case "submiteddate":
                    case "submitedby":
                        item.disabled = true;
                        item.visible = true;
                        break;
                    case "issynchronous":
                        item.visible = true;
                        break;
                    case "stay5":
                        item.visible = true;
                        item.title = "车间";
                        item.disabled = true;
                        item.index = 4;
                        item.required = true;
                        item.disabled = true;
                        break;
                }
            }
            if (data.name.EndsWith("StockInOrderMaterials"))
            {
                data.title = "半成品入库明细";
                switch (item.field.ToLower())
                {
                    case "num2":
                        item.visible = true;
                        item.title = "已操作数量";
                        break;
                    case "num":
                        item.title = "入库数量";
                        item.disabled = false;
                        break;
                    case "batchno":
                        item.disabled = true;
                        item.title = "生产批号";
                        break;
                    case "state":
                        item.visible = false;
                        break;
                    case "depotwbs":
                        item.required = false;
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
                        item.foreign.rowdisplay.Add("STAY2".ToLower(), "batchno");
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
                    case "staynum":
                        item.visible = true;
                        item.title = "待操作数";
                        break;
                }
            }
            if (data.name.EndsWith("ProcessState"))
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
        //protected override void OnInitViewChildItem(Acc.Contract.Data.ModelData data, Acc.Contract.Data.ItemData item)
        //{
        //    //【合同子集】产品信息--商品带出信息
        //    if (item.IsField("FCPID"))
        //    {
        //        if (item.foreign != null)
        //        {
        //            item.foreign.rowdisplay.Add("FNAME".ToUpper(), "FCPNAME");//参数1：数据来源字段，参数2：目标字段
        //            item.foreign.rowdisplay.Add("FModel".ToUpper(), "FCPMODEL");
        //            item.foreign.rowdisplay.Add("FUnitID".ToUpper(), "FUNITID");
        //        }
        //    }
        //}
        #endregion

        #region 重写方法

        protected override void OnAdding(ControllerBase.SaveEvent item)
        {
            base.OnAdding(item);
        }

        public override void ValidatePort(ControllerBase.SaveEvent item)
        {
            
        }

        protected override void OnEditing(ControllerBase.SaveEvent item)
        {
            base.OnEditing(item);
        }

        /// <summary>
        /// 添加时制定源单控制器为生产计划通知单
        /// </summary>
        /// <returns></returns>
        public override string InOrderController()
        {
            return new ProduceInNoticeOrderController().ToString();
        }

        public override int SetStockType()
        {
            return 5;
        }

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
                    ac.visible = true;
                }
                if (ac.command == "UnReviewedData")
                {
                    ac.visible = false;
                }
            }
            return coms;
        }
        protected override void foreignInOrder(IModel model, loadItem item)
        {
            if (this.fdata.filedname == "SOURCECODE")
            {
                item.rowsql = "select * from (" + item.rowsql + ") a where a.code in(select code from " + this.fdata.tablename + " where stocktype = " + new ProduceInNoticeOrderController().SetStockType() + " and IsSubmited=1)";
            }
            else
            {
                base.foreignInOrder(model, item);

            }
        }

        protected override void OnSubmitData(BasicInfo info)
        {
            StockInOrder si = base.getinfo(this.ActionItem["ID"].ToString()) as StockInOrder;
            si.Materials.DataAction = this.model.GetDataAction();
            si.Materials.GetData();
            for (int i = 0; i < si.Materials.Count; i++)
            {
                if (!string.IsNullOrEmpty(si.Materials[i].DEPOTWBS) && si.Materials[i].DEPOTWBS != "0")
                {
                }
                else
                {
                    throw new Exception("异常：" + si.Materials[i].RowIndex + "行 产品编码：" + si.Materials[i].MCODE+ " 货位不能为空，请添加货位后进行提交！");
                }
            }
            base.OnSubmitData(info);
        }
        protected override void OnReviewedData(BasicInfo info)
        {
            StockInOrder si = base.getinfo(this.ActionItem["ID"].ToString()) as StockInOrder;
            if (si.IsSubmited == false)
            {
                throw new Exception("异常：单据未提交，请提交后审核");
            }
            base.OnReviewedData(info);
            SemReviewedData(info);
        }

        private void SemReviewedData(BasicInfo info)
        {
            StockInOrder si = base.getinfo(info.ID.ToString()) as StockInOrder;
            string sql = "update " + si.ToString() + " set IsReviewed=1,Reviewedby=" + this.user.ID + ",Revieweddate='" + DateTime.Now + "' where id ='" + si.ID + "'";
            this.model.GetDataAction().Execute(sql);
        }
        #endregion 
        public override string GetNoticeStayGrid()
        {
            return base.GetNoticeStayGrid();
        }

        protected override void CheckTransformData()
        {
            base.CheckTransformData();
            StockInOrder mi = this.ActionItem as StockInOrder;
            if (!mi.IsSubmited)
            {
                throw new Exception("单据未审核，不符合同步条件！");
            }

        }
        
    }
}
