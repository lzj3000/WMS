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
using Acc.Contract.Data;
using Way.EAP.DataAccess.Regulation;
using Acc.Contract;
using Acc.Business.WMS.XHY.Model;
using System.Data;
namespace Acc.Business.WMS.XHY.Controllers
{
    public class OtherOutOrderController : StockOutOrderController
    {
        public OtherOutOrderController() : base(new XHY_StockOutOrder()) { }
        /// <summary>
        /// 描述：新华杨其他出库控制器
        /// 作者：柳强
        /// 创建日期:2013-08-29
        /// </summary>
      
        #region 基础设置
        //是否启用审核--未启用因为审核人不可以为当前登陆人
       
        //显示在菜单
        protected override string OnControllerName()
        {
            return "其他出库";
        }
        //菜单中url路径
        protected override string OnGetPath()
        {
            return "Views/WMS/XHY/OtherOutOrder.htm";
        }
       
        //说明
        protected override string OnGetControllerDescription()
        {
            return "其他出库";
        }
        #endregion

        #region 初始化数据方法
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
                data.title = "其他出库";
                switch (item.field.ToLower())
                {
                    case "code":
                        item.title = "其他出库单号";
                        item.visible = true;
                        item.disabled = true;
                        break;        
                    case "id":
                    case "state":
                    case "saleuser": 
                    case "sourcecode":
                    case "stay8":
                    case "stay9":
                    case "stay10":

                        item.visible = false;
                        item.isedit = false;
                        break;
                    case "submiteddate":
                    case "submitedby":
                    case "createdby":
                    case "creationdate":
                    case "modifiedby":
                    case "modifieddate":
                    case "reviewedby":
                    case "revieweddate":
                    case "issubmited":
                    case "isreviewed":
                        item.disabled = true;
                        break;
                    //case "sourcecode":
                    //    if (item.foreign != null)
                    //        item.foreign.isfkey = false;
                    //    break;
                    case "k3outordertype":
                        item.visible = true;
                        item.required = true;
                        break;
                    case "bumen":
                        item.visible = true;
                        item.required = true;
                        break;
                    case "clientno":
                        item.title = "客户编码";
                        item.required = false;
                        break;
                    case "towhno":
                        item.required = true;
                        break;
                    case "workerid":
                        item.required = false;
                        item.visible = true;
                        break;
                }
            }
            if (data.name.EndsWith("XHY_StockOutOrder"))
            {
                switch (item.field.ToLower())
                {
                    case "bumen":
                    
                        item.visible = true;
                        item.disabled = false;
                        item.required = true;
                        break;
                    case "issynchronous":
                          item.visible = true;
                        item.disabled = true;
                        break;
                    case "sourcename":
                    case "sourceoutcode":
                    case "stay1"://备用字段
                    case "stay2":
                    case "stay3":
                    case "stay4":
                    case "stay5":
                    case "finishtime":
                        item.visible = false;
                        break;
                }
            }
            if (data.name.EndsWith("StockOutOrderMaterials"))
            {
                data.title = "其他出库明细";
                switch (item.field.ToLower())
                {
                    case "id":
                    case "code":
                    case "senbatchno":
                    case "price":
                    case "parentid":
                    case "state":
                    case "ischeckok":
                    case "stay1"://备用字段
                    case "stay2":
                    case "stay3":
                    case "stay4":
                    case "stay5":
                    case "funitid"://单位
                    case "stay6":
                    case "stay7":
                    case "finishnum":
                    case "staynum":
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
                        item.disabled = false;
                        item.required = false;
                        break;
                    case "num":
                        item.required = true;
                        break;


                }
            }
            if (data.name.EndsWith("StockOutNoticeMaterials"))
            {
                data.visible = false;
            }
            if (data.name.EndsWith("InSequence"))
            {
                switch (item.field.ToLower())
                {

                    case "sequencecode":
                        item.visible = true;
                        break;
                    default:
                        item.visible = false;
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
        protected override void OnForeignIniting(IModel model, InitData data)
        {
            if (model is StockOutOrderMaterials)
            {
                if (this.fdata.filedname == "MATERIALCODE")
                {
                    foreach (ItemData d in data.modeldata.childitem)
                    {//Code,FNAME,FMODEL
                        d.visible = false;
                        if (d.IsField("ID"))
                            d.visible = true;
                        if (d.IsField("Code"))
                        {
                            d.visible = true;
                            d.title = "商品编码";
                        }
                        if (d.IsField("FNAME"))
                            d.visible = true;
                        if (d.IsField("FMODEL"))
                            d.visible = true;
                        if (d.IsField("BATCH"))
                        {
                            d.visible = true;
                        }
                        if (d.IsField("CommodityType"))
                        {
                            d.visible = true;
                        }
                        if (d.IsField("WeightNUM"))
                        {
                            d.visible = true;
                        }
                    }
                    return;
                }
               

                #region 不要
                //return;
                //if ( this.fdata.filedname == "PORTNAME")
                //{
                //    data.modeldata.tablename = "Acc_WMS_InfoMaterials";
                //    data.modeldata.name = "Acc.Business.WMS.Model.StockInfoMaterials";
                //    data.modeldata.title = "库存产品表";
                //    foreach (ItemData d in data.modeldata.childitem)
                //    {
                //        d.visible = false;

                //        if (d.IsField("STAY1"))//Code
                //        {
                //            d.visible = true;
                //            d.title = "PORTCODE";
                //        }
                //        if (d.IsField("PORTNO"))//DEPOTWBS
                //        {
                //            d.visible = true;
                //            d.title = "DEPOTWBS";
                //        }
                //        if (d.IsField("PORTNAME"))//PORTCODE   ID,PORTCODE,DEPOTWBS,Code
                //        {
                //            d.visible = true;
                //            d.title = "Code";
                //        }
                //    }
                //    return;
                //}
                //if(this.fdata.filedname == "DEPOTWBS" ){
                //    data.modeldata.tablename = "Acc_WMS_InfoMaterials";
                //    data.modeldata.name = "Acc.Business.WMS.Model.StockInfoMaterials";
                //    data.modeldata.title = "库存产品表";
                //    foreach (ItemData d in data.modeldata.childitem)
                //    {
                //        d.visible = false;

                //        if (d.IsField("CODE"))//Code
                //        {
                //            d.visible = true;
                //            d.title = "PORTCODE";
                //        }
                //        if (d.IsField("WAREHOUSENAME"))//DEPOTWBS
                //        {
                //            d.visible = true;
                //            d.title = "DEPOTWBS";
                //        }
                //        if (d.IsField("ADDRESS"))//PORTCODE   ID,PORTCODE,DEPOTWBS,Code
                //        {
                //            d.visible = true;
                //            d.title = "Code";
                //        }
                //    }
                //    return;
                //}
                #endregion

            }
            else if (model is LogisticsInfo)
            {
                if (this.fdata.filedname == "LogCode")
                {
                    foreach (ItemData d in data.modeldata.childitem)
                    {
                        d.visible = false;
                        if (d.IsField("ROWINDEX"))
                        {
                            d.visible = true;
                        }
                        if (d.IsField("Code"))
                        {
                            d.visible = true;
                            d.title = "物流公司编码";
                        }
                        if (d.IsField("Remark"))
                            d.visible = true;
                        if (d.IsField("LOGISTICSNAME"))
                            d.visible = true;
                        if (d.IsField("LOGISTICSURL"))
                            d.visible = true;
                        if (d.IsField("TEL"))
                            d.visible = true;
                    }
                }
            }
            else if (model is StockOutOrder)
            {
                if (this.fdata.filedname == "TOWHNO")
                {
                    foreach (ItemData d in data.modeldata.childitem)
                    {//序号、仓库编码Code、名称WAREHOUSENAME、类型WHTYPE、上级PARENTID、是否禁用IsDisable、备注Remark
                        d.visible = false;
                        if (d.IsField("ROWINDEX"))
                        {
                            d.visible = true;
                        }
                        if (d.IsField("Code"))
                        {
                            d.visible = true;
                            d.title = "仓库编码";
                        }
                        if (d.IsField("WAREHOUSENAME"))
                            d.visible = true;
                        if (d.IsField("WHTYPE"))
                            d.visible = true;
                        if (d.IsField("PARENTID"))
                            d.visible = true;
                        if (d.IsField("IsDisable"))
                            d.visible = true;
                        if (d.IsField("Remark"))
                            d.visible = true;
                    }
                    return;
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
        
        #endregion

        #region 重写方法

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
            }
            return coms;
        }

        /// <summary>
        /// 设置stockType = 4
        /// </summary>
        /// <returns></returns>
        public int SetStockType()
        {
            return 4;
        }
        /// <summary>
        /// 查询按钮，并添加查询条件
        /// </summary>
        /// <param name="m"></param>
        /// <param name="where"></param>
        protected override void OnGetWhereing(IModel m, List<SQLWhere> where)
        {
            base.OnGetWhereing(m, where);
            if (m is StockOutOrder)
            {
                SQLWhere w = new SQLWhere();
                w.ColumnName = "STOCKTYPE";
                w.Value = SetStockType().ToString();
                where.Add(w);
            }
        }
        protected override void OnAdding(ControllerBase.SaveEvent item)
        {
            XHY_StockOutOrder soo = item.Item as XHY_StockOutOrder;
            soo.STOCKTYPE = SetStockType();
            base.OnAdding(item);
        }

        
      
        /// <summary>
        /// 验证货位的方法
        /// </summary>
        /// <param name="item"></param>
        public void ValidateDepotwbs(ControllerBase.SaveEvent item)
        {
            StockOutOrder so = item.Item as StockOutOrder;
            for (int i = 0; i < so.Materials.Count; i++)
            {
                WareHouse wh = so.Materials[i].GetForeignObject<WareHouse>(this.model.GetDataAction());
                if (wh.PARENTID != so.TOWHNO)
                {
                    throw new Exception("序号:" + so.Materials[i].RowIndex + "货位不属于主单仓库中！");
                }
            }
        }

        /// <summary>
        /// 验证托盘的方法
        /// </summary>
        /// <param name="item"></param>
        public void ValidatePort(ControllerBase.SaveEvent item)
        {
            //StockOutOrder so = item.Item as StockOutOrder;
            //for (int i = 0; i < so.Materials.Count; i++)
            //{
            //    //Ports pt = so.Materials[i].GetForeignObject<Ports>(this.model.GetDataAction());
            //    if (so.Materials[i].PORTNAME == "")
            //    {
            //        throw new Exception("序号:" + so.Materials[i].RowIndex + "托盘不能为空！");
            //    }
            //}
        }
        #endregion

        protected void CheckTransformData()
        {
            StockOutOrder mi = this.ActionItem as StockOutOrder;
            if (!mi.IsSubmited)
            {
                throw new Exception("单据未提交，不符合同步条件！");
            }
            if (mi.IsSynchronous)
            {
                throw new Exception("单据已经同步成功，不能再次同步！");
            }
            //base.CheckTransformData();
            XHY_StockOutOrder mi1 = this.ActionItem as XHY_StockOutOrder;
            if (mi1.K3OutOrderType!=2)
            {
                throw new Exception("未选择K3单据同步类型，不符合同步条件！");
            }
        }

        protected override void OnSubmitData(Business.Model.BasicInfo info)
        {
            StockOutOrder so = info as StockOutOrder;
            if (so.IsSubmited == true)
            {
                throw new Exception("异常：单据已提交不能重复提交！");
            }
            so.Materials.DataAction = this.model.GetDataAction();
            so.Materials.GetData();
            if (so.Materials.Count == 0)
            {
                throw new Exception("单据无明细数据不能提交！");
            }
            base.OnSubmitData(info);
            Ports pt = new Ports();
            UpdatePortStatus(so, pt);
        }

        /// <summary>
        /// 提交后更改托盘为可用
        /// </summary>
        /// <param name="order">当前出库单</param>
        /// <param name="pt">托盘对象</param>
        public void UpdatePortStatus(StockOutOrder order, Ports pt)
        {
            for (int i = 0; i < order.Materials.Count; i++)
            {
                string sql = "select portcode,sum(num) num from acc_wms_infomaterials where portcode='" + order.Materials[i].PORTNAME + "'  group by portcode";
                DataTable dt = this.model.GetDataAction().GetDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToInt32(dt.Rows[0]["num"]) == 0)
                    {
                        string ptsql = "update " + pt.ToString() + " set STATUS='0' where id='" + order.Materials[i].PORTNAME + "'";
                        this.model.GetDataAction().Execute(ptsql);
                    }
                }
            }
        }


        #region 接口相关
        /// <summary>
        /// 初始化--动态设置日志类与本单据model的外键关系
        /// </summary>
        /// <param name="data"></param>
        protected override void OnInitView(ModelData data)
        {
            base.OnInitView(data);
            if (data.name.EndsWith("LogData"))
            {
                foreach (ItemData d in data.childitem)
                {
                    if (d.IsField("SourceID"))
                    {
                        d.visible = false;
                        d.foreign.isfkey = true;
                        d.foreign.foreignfiled = "ID";
                        d.foreign.filedname = d.field;
                        d.foreign.objectname = data.name;
                        d.foreign.parenttablename = data.tablename;
                        d.foreign.foreignobject = this.model.GetType().FullName;
                        d.foreign.tablename = this.model.ToString();

                    }
                }
            }
        }

        /// <summary>
        /// 添加按钮（同步到K3）
        /// </summary>
        [ActionCommand(name = "同步到K3", title = "将该单据数据同步到K3系统", index = 9, icon = "icon-ok", isalert = true)]
        public void TransferData()
        {
            CheckTransformData();

        }

        //protected virtual void CheckTransformData()
        //{
        //    StockOutOrder mi = this.ActionItem as StockOutOrder;
        //    if (!mi.IsSubmited)
        //    {
        //        throw new Exception("单据未提交，不符合同步条件！");
        //    }
        //    if (mi.IsSynchronous)
        //    {
        //        throw new Exception("单据已经同步成功，不能再次同步！");
        //    }
        //}
        #endregion
     }
}
