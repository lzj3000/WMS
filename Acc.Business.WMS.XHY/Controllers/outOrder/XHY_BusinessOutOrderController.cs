using System;
using System.Collections.Generic;
using System.Linq;
using Acc.Business.Controllers;
using Acc.Business.WMS.Model;
using Acc.Contract;
using Acc.Contract.Center;
using Acc.Contract.Data.ControllerData;
using Acc.Contract.MVC;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model;
using Acc.Contract.Data;
using Acc.Business.WMS.Controllers;
using Way.EAP.DataAccess.Regulation;
using Acc.Business.WMS.XHY.Model;
using System.Data;
using Way.EAP.DataAccess.Data;

namespace Acc.Business.WMS.XHY.Controllers
{
    public class XHY_BusinessOutOrderController : StockOutOrderController
    {
        public XHY_BusinessOutOrderController() : base(new StockOutOrder()) { }
        public XHY_BusinessOutOrderController(IModel model) : base(model) { }
        /// <summary>
        /// 描述：新华杨基础出库控制器
        /// 作者：柳强
        /// 创建日期:2013-10-14
        /// </summary>

        #region 基础设置
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

        public override bool IsClearAway
        {
            get
            {
                return false;
            }
        }

        public override bool IsPrint
        {
            get
            {
                return false;
            }
        }

        public override bool IsReviewedState
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
            if (data.name.EndsWith("XHY_StockOutOrder"))
            {
                switch (item.field.ToLower())
                {
                    case "rowindex":
                    case "code":
                        item.visible = true;
                        item.disabled = true;
                        break;
                    case "issynchronous":
                        item.visible = true;
                        break;
                    case "revieweddate":
                    case "reviewedby":
                    case "isreviewed":
                    case "isdisable":
                    case "finishtime":
                    case "inouttime":
                    case "stay1":
                    case "stay2":
                    case "stay3":
                    case "stay4":
                    case "stay5":
                    case "sourcename":
                    case "sourceoutcode":
                        item.visible = false;
                        break;

                }
            }
            if (data.name.EndsWith("StockOutOrder"))
            {
                switch (item.field.ToLower())
                {
                    case "rowindex":
                    case "code":
                        item.visible = true;
                        item.disabled = true;
                        break;
                    case "revieweddate":
                    case "reviewedby":
                    case "isreviewed":
                    case "isdisable":
                    case "finishtime":
                    case "inouttime":
                    case "stay1":
                    case "stay2":
                    case "stay3":
                    case "stay4":
                    case "stay5":
                    case "sourcename":
                    case "sourceoutcode":
                        item.visible = false;
                        break;

                }
            }
            if (data.name.EndsWith("StockOutOrderMaterials"))
            {
                switch (item.field.ToLower())
                {
                    case "rowindex":
                    case "code":
                        item.visible = true;
                        item.disabled = true;
                        break;
                    case "materialcode":
                        item.visible = true;
                        item.required = true;
                        item.title = "物料名称";
                        break;
                    case "mcode":
                        item.visible = true;
                        item.title = "物料编码";
                        break;
                    case "fmodel":
                        item.visible = true;
                        item.title = "物料规格";
                        break;
                    case "batchno":
                        item.disabled = true;
                        break;
                    case "stay1"://备用字段
                    case "stay2":
                    case "stay3":
                    case "stay4":
                    case "stay5":
                    case "funitid"://单位
                    case "price":
                    case "stay6":
                    case "stay7":
                    case "senbatchno":
                    case "finishnum":
                    case "staynum":
                        item.visible = false;
                        break;
                    case "depotwbs":
                        item.title = "货位编码";
                        item.required = true;
                        item.foreign.rowdisplay.Add("BATCHNO", "BATCHNO");
                        break;
                    case "portname":
                        item.title = "托盘编码";
                        item.required = true;
                        item.foreign.rowdisplay.Add("BATCHNO", "BATCHNO");
                        break;
                }
            }
            if (data.name.EndsWith("StockOutNoticeMaterials"))
            {
                switch (item.field.ToLower())
                {
                    case "rowindex":
                    case "num":
                    case "finishnum":
                    case "staynum":
                        item.visible = true;
                        break;
                    case "materialcode":
                        item.visible = true;
                        item.title = "物料名称";
                        break;
                    case "mcode":
                        item.visible = true;
                        item.title = "物料编码";
                        break;
                    case "fmodel":
                        item.visible = true;
                        item.title = "物料规格";
                        break;
                    default:
                        item.visible = false;
                        break;
                }

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

        #endregion

        protected void OnInitDeportOrPort(StockOutOrderMaterials model, InitData data)
        {
            StockInfoView v = new StockInfoView();
            data.modeldata = ((IModel)v).GetModelData();
            data.modeldata.title = "库存产品表";
            foreach (ItemData d in data.modeldata.childitem)
            {
                if (d.IsField("SPCODE") ||
                d.IsField("SPNAME"))
                    d.visible = false;
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
                        if (d.IsField("STAY2"))//Code
                        {
                            d.visible = true;
                            d.title = "批次";
                        }
                    }
                    return;
                }
                else
                {
                    OnInitDeportOrPort((StockOutOrderMaterials)model, data);
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

        #region 重写方法

        protected virtual void foreignOutOrder(IModel model, loadItem item)
        {
            if (this.fdata.filedname == "TOWHNO")
            {
                item.rowsql = "select * from (" + item.rowsql + ") a where a.isdisable =0 and WHTYPE=0";
            }
            if (this.fdata.filedname == "CLIENTNO")
            {
                item.rowsql = "select * from (" + item.rowsql + ") a where a.isdisable =0";
            }
            if (this.fdata.filedname == "SOURCECODE")
            {
                //item.rowsql = "select * from (" + item.rowsql + ") a where a.code in(select code from " + new StockInNotice().ToString() + " where stocktype = " + new PurchaseInNoticeOrderController().SetStockType() + " and IsSubmited=1)";
                item.rowsql = "select * from (" + item.rowsql + ") a where a.code in(select code from " + this.fdata.tablename + " where stocktype = " + SetStockType() + " and IsSubmited=1)";
            }
        }
        protected virtual void foreignOutOrderMaterials(IModel model, loadItem item)
        {
            StockOutOrderMaterials ziji = model as StockOutOrderMaterials;
            StockInfoView view = null;
            if (this.ActionItem == null)
            {
                base.OnForeignLoading(model, item);
            }
            ///选择可用的产品
            else if (this.fdata.filedname == "MATERIALCODE" && this.ActionItem["SourceCode"] == null)
            {
                if (this.ActionItem["TOWHNO"].ToString() == "0")
                    throw new Exception("仓库不能为空！请先选择仓库。");

                item.rowsql = "select * from (" + item.rowsql + ") a where  a.id in(select code from " + new StockInfoMaterials().ToString() + "   where  WAREHOUSEID='" + this.ActionItem["TOWHNO"].ToString() + "')";
            }
            else if (this.fdata.filedname == "MATERIALCODE" && this.ActionItem["SourceCode"].ToString() != "0")
            {
                item.rowsql = "select ID,Code,FNAME,FMODEL, from (" + item.rowsql + ") a where  a.isdisable=0 and a.id in(select MATERIALCODE from " + new StockOutNoticeMaterials().ToString() + " sim INNER JOIN " + new StockOutNotice().ToString() + " si on si.ID= sim.PARENTID  where si.code ='" + this.ActionItem["SourceCode"] + "')";
            }

            else if (this.fdata.filedname == "PORTNAME" || this.fdata.filedname == "DEPOTWBS")
            {
                #region 条件查询
                string W = "";
                //foreach (SQLWhere w in item.whereList)
                //    W += w.where("and   ");
                if (item.whereList.Length > 0)
                {
                    string file = item.whereList[0].ColumnName;
                    string symol = item.whereList[0].Symbol;
                    string value = item.whereList[0].Value;
                    file = file.Substring(file.LastIndexOf('.'));
                    //if (file.Equals(".NUM"))
                    //{
                    //    symol = "=";
                    //    value = value.Substring(2, value.Length - 4);
                    //}
                    switch (file)
                    {
                        case ".SPCODE":
                        case ".SPNAME":
                        case ".KNAME":
                        case ".HWCODE":
                        case ".TPCODE":
                        case ".NUM":
                        case ".STAY11":
                        default: file = file.Substring(1);
                            break;
                    }
                    W = "and  " + file + "  " + symol + "   " + value;
                }
                #endregion
                if (this.ActionItem["TOWHNO"].ToString() == "0")
                    throw new Exception("仓库不能为空！请先选择仓库。");
                if (string.IsNullOrEmpty(ziji.MATERIALCODE))
                    throw new Exception("物料不能为空！请先选择物料。");
                view = new StockInfoView();
                view.KID = int.Parse(this.ActionItem["TOWHNO"].ToString());
                view.SPID = int.Parse(ziji.MATERIALCODE);
                if (!string.IsNullOrEmpty(ziji.DEPOTWBS))
                    view.HWID = int.Parse(ziji.DEPOTWBS);
                if (!string.IsNullOrEmpty(ziji.PORTNAME))
                    view.TPID = int.Parse(ziji.PORTNAME);
                item.sort = "";
                item.rowsql = "";
                item.rowsql = view.getGetSearchSQL() + "   " + W;
                view.Dispose();
            }
            #region 不要
            //if (this.fdata.filedname == "PORTNAME")
            //{
            //    if (this.ActionItem["TOWHNO"] == null)
            //        throw new Exception("仓库不能为空！请先选择仓库。");
            //    if (ziji.MATERIALCODE == "" || this.ActionItem["sourcecode"].ToString() == "")
            //    {
            //        throw new Exception("请单击上一步检查仓库、物料、原单编号是否选择");
            //    }
            //    else
            //    {
            //        StockInfoMaterials sim = new StockInfoMaterials();
            //        string s = ((IEntityBase)sim).GetSearchSQL();
            //        string sql = "select id, PORTCODE as STAY1,DEPOTWBS as PORTNO,Code as PORTNAME  from (" + s + ") a where a.WAREHOUSEID=" + this.ActionItem["TOWHNO"] + " and a.Code=" + ziji.MATERIALCODE;
            //        item.rowsql = sql;

            //    }
            //}
            //if (this.fdata.filedname == "DEPOTWBS")
            //{

            //     if (this.ActionItem["TOWHNO"].ToString() == "" || ziji.MATERIALCODE == "" || this.ActionItem["sourcecode"].ToString() == "")
            //     {
            //         throw new Exception("请单击上一步检查仓库、物料、原单编号是否选择");
            //     }
            //     else
            //     {
            //         StockInfoView view = new StockInfoView();
            //         view.KID = int.Parse(this.ActionItem["TOWHNO"].ToString());
            //         view.SPID = int.Parse(ziji.MATERIALCODE);
            //         item.rowsql = ((IEntityBase)view).GetSearchSQL();
            //     }
            //}
            #endregion
        }
        protected override void OnForeignLoading(IModel model, loadItem item)
        {

            if (model is StockOutOrder)
            {
                foreignOutOrder(model, item);
            }
            if (model is StockOutOrderMaterials)
            {
                foreignOutOrderMaterials(model, item);
            }

        }
        protected override void OnForeignLoaded(IModel model, ReadTable table)
        {
            if (model is StockOutOrderMaterials)
            {
                if (this.fdata.filedname == "PORTNAME")
                {
                    table.rows.Columns.Add(this.fdata.displayfield);
                    foreach (DataRow r in table.rows.Rows)
                    {
                        if (r["TPID"].Equals(""))
                        {
                            r["ID"] = 0;
                        }
                        else
                        {
                            r["ID"] = r["TPID"];
                        }
                        r[this.fdata.displayfield] = r["TPCODE"];
                    }
                }
                if (this.fdata.filedname == "DEPOTWBS")
                {
                    table.rows.Columns.Add(this.fdata.displayfield);
                    foreach (DataRow r in table.rows.Rows)
                    {
                        if (r["HWID"].Equals(""))
                        {
                            r["ID"] = 0;
                        }
                        else
                        {
                            r["ID"] = r["HWID"];
                        }
                        r[this.fdata.displayfield] = r["HWCODE"];
                    }
                }
            }
            else
                base.OnForeignLoaded(model, table);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="item"></param>
        protected override void OnAdding(ControllerBase.SaveEvent item)
        {
            XHY_Add(item);
            base.OnAdding(item);
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="item"></param>
        protected override void OnEditing(ControllerBase.SaveEvent item)
        {
            StockOutOrder sio = item.Item as StockOutOrder;
            if (sio.IsSubmited == true)
            {
                throw new Exception("异常：单据已提交不能编辑！");
            }
            XHY_Edit(item);
            base.OnEditing(item);
        }

        protected override void OnRemoveing(ControllerBase.SaveEvent item)
        {
            BasicInfo sio = item.Item as BasicInfo;
            if (sio.Createdby == this.user.ID)
            {
                base.OnRemoveing(item);
            }
            else
            {
                throw new Exception("单据创建人与当前登陆人不符，不能删除！！");
            }
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

        public virtual void XHY_Add(ControllerBase.SaveEvent item)
        {
            AddDefault(item);
            //ValidataCheckOrder(item);销售出库单需要调用此方法
            ///验证通过调用设置值方法
            StockOutOrder sio = item.Item as StockOutOrder;

            ///设置单据类型
            sio.STOCKTYPE = SetStockType();
            ValidateAddInNum(item);
            ValidateBatchno(item);
            ValidateDepotwbs(item);
            ValidatePort(item);
            ValidateWarehouse(item);

            ///设置明细行创建时间
            for (int i = 0; i < sio.Materials.Count; i++)
            {
                sio.Materials[i].Creationdate = DateTime.Now;
                sio.Materials[i].Createdby = this.user.ID;
            }
        }

        public virtual void XHY_Edit(ControllerBase.SaveEvent item)
        {
            AddDefault(item);
            //ValidataCheckOrder(item);销售出库单需要调用此方法
            ValidateEditInNum(item);
            ValidateBatchno(item);
            ValidateDepotwbs(item);
            ValidatePort(item);
            ValidateWarehouse(item);
            ///验证通过调用设置值方法
            StockOutOrder sio = item.Item as StockOutOrder;
            ///重新设置明细行创建时间
            for (int i = 0; i < sio.Materials.Count; i++)
            {
                sio.Materials[i].Modifieddate = DateTime.Now;
                sio.Materials[i].Modifiedby = this.user.ID;
            }
        }

        protected override void OnSubmitData(BasicInfo info)
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
        public virtual void UpdatePortStatus(StockOutOrder order, Ports pt)
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


        /// <summary>
        /// 验证出库产品是否通过质检
        /// </summary>
        /// <param name="item"></param>
        public virtual void ValidataCheckOrder(StockOutOrder so)
        {
            ValidateCheckOrderMaterials(so);
        }

        /// <summary>
        /// 验证出库产品是否通过质检
        /// </summary>
        /// <param name="so"></param>
        private void ValidateCheckOrderMaterials(StockOutOrder so)
        {
            ProCheckMaterials pm = new ProCheckMaterials();
            DataTable dt = null;

            for (int i = 0; i < so.Materials.Count; i++)
            {
                Materials ms = so.Materials[i].GetForeignObject<Materials>(this.model.GetDataAction());
                ///验证出库产品是否需要质检
                if (!string.IsNullOrEmpty(so.Materials[i].PORTNAME))
                {
                    if (ms.ISCHECKPRO == true)
                    {
                        string msql = "select * from " + pm.ToString() + " pm where pm.BATCHNO ='" + so.Materials[i].BATCHNO + "' and pm.CHECKWARE = '" + so.Materials[i].MATERIALCODE + "'";
                        dt = this.model.GetDataAction().GetDataTable(msql);
                        if (dt.Rows.Count == 0)
                        {
                            throw new Exception("异常：序号：" + so.Materials[i].RowIndex + " 出库产品编码" + so.Materials[i].MCODE + " 批次不存在质检单中");
                        }
                        if (Convert.ToInt32(dt.Rows[0]["IsOk"]) == 0)
                        {
                            throw new Exception("异常：序号" + so.Materials[i].RowIndex + " 产品编码" + so.Materials[i].MCODE + " 出库批次未质检通过");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// （新增）验证数量的方法
        /// </summary>
        /// <param name="item"></param>
        public virtual void ValidateAddInNum(ControllerBase.SaveEvent item)
        {
            StockOutOrder stockoutorder = item.Item as StockOutOrder;
            for (int i = 0; i < stockoutorder.Materials.Count; i++)
            {
                ValidateInNumIsTrue(stockoutorder, i);
            }
            ValidateSourceNum(stockoutorder);
        }

        /// <summary>
        /// （编辑）验证数量的方法
        /// </summary>
        /// <param name="item"></param>
        public virtual void ValidateEditInNum(ControllerBase.SaveEvent item)
        {
            StockOutOrder stockoutorder = item.Item as StockOutOrder;
            for (int i = 0; i < stockoutorder.Materials.Count; i++)
            {
                ValidateInNumIsTrue(stockoutorder, i);
            }
            ValidateSourceNum(stockoutorder);
        }

        /// <summary>
        /// 验证输入的数量是否正确
        /// </summary>
        /// <param name="stockinorder"></param>
        /// <param name="i"></param>
        public virtual void ValidateInNumIsTrue(StockOutOrder stockoutorder, int i)
        {
            if (stockoutorder.Materials[i].NUM <= 0)
            {
                throw new Exception("序号：" + stockoutorder.Materials[i].RowIndex + " 商品编码" + stockoutorder.Materials[i].MCODE + " 请输入正确的数量");
            }
        }

        /// <summary>
        /// 验证源单数量是否正确
        /// </summary>
        /// <param name="stockinorder"></param>
        /// <param name="i"></param>
        public virtual void ValidateSourceNum(StockOutOrder stockoutorder)
        {
            var result = from o in stockoutorder.Materials group o by new { o.SourceRowID } into g select new { g.Key, Totle = g.Sum(p => p.NUM), Items = g.ToList<StockOutOrderMaterials>() };
            foreach (var group in result)
            {

                StockOutOrderMaterials m = group.Items.Find(delegate(StockOutOrderMaterials mm) { return ((IEntityBase)mm).StateBase == EntityState.Select; });
                if (m == null)
                    m = group.Items[0];
                //m.NUM = group.Totle;
                if (group.Totle <= 0)
                {
                    throw new Exception("明细行数量错误");
                }
                if (m.SourceRowID > 0)
                {
                    Materials a = m.GetForeignObject<Materials>(this.model.GetDataAction());
                    double snum = m.GetNum(m.SourceTable, m.SourceRowID, m.MATERIALCODE);
                    double bcnum = m.GetSourceNum(m.SourceTable, m.SourceRowID, m.MATERIALCODE, stockoutorder.ID);
                    if (group.Totle > snum)
                        throw new Exception("序号:" + m.RowIndex + " 商品名称:" + a.FNAME + "  商品编码：" + a.Code + " 数量超过源单数量");
                    if (group.Totle > snum - bcnum)
                        throw new Exception("序号:" + m.RowIndex + " 商品名称:" + a.FNAME + "  商品编码：" + a.Code + " 数量超过源单数量.");
                }
            }

        }

        /// <summary>
        /// 验证明细行批次的方法
        /// </summary>
        /// <param name="item"></param>
        public virtual void ValidateBatchno(ControllerBase.SaveEvent item)
        {
            StockOutOrder stockinorder = item.Item as StockOutOrder;
            for (int i = 0; i < stockinorder.Materials.Count; i++)
            {
                var sm = stockinorder.Materials[i];
                Materials a = sm.GetForeignObject<Materials>(this.model.GetDataAction());
                if (a.BATCH == true && sm.BATCHNO == "")
                {
                    throw new Exception("序号:" + sm.RowIndex + " 商品名称:" + a.FNAME + "  商品编码：" + a.Code + "  为批次管理产品，批次号不能为空！");
                }
            }
        }

        /// <summary>
        /// 验证货位的方法
        /// </summary>
        /// <param name="item"></param>
        public virtual void ValidateDepotwbs(ControllerBase.SaveEvent item)
        {
            StockOutOrder so = item.Item as StockOutOrder;
            for (int i = 0; i < so.Materials.Count; i++)
            {
                WareHouse wh = so.Materials[i].GetForeignObject<WareHouse>(this.model.GetDataAction());
                if (wh != null)
                {
                    if (wh.PARENTID != so.TOWHNO)
                    {
                        throw new Exception("序号:" + so.Materials[i].RowIndex + "货位不属于主单仓库中！");
                    }
                }
            }
        }

        /// <summary>
        /// 验证托盘的方法
        /// </summary>
        /// <param name="item"></param>
        public virtual void ValidatePort(ControllerBase.SaveEvent item)
        {
            StockOutOrder so = item.Item as StockOutOrder;
            if (so.STOCKTYPE != new PickOutOrderController().SetStockType() && so.STOCKTYPE != new OtherOutOrderController().SetStockType())
            {
                for (int i = 0; i < so.Materials.Count; i++)
                {
                    //Ports pt = so.Materials[i].GetForeignObject<Ports>(this.model.GetDataAction());
                    if (so.Materials[i].PORTNAME == "")
                    {
                        throw new Exception("序号:" + so.Materials[i].RowIndex + "托盘不能为空！");
                    }
                }
            }
        }

        /// <summary>
        /// 验证仓库的方法（是否有权限选择出此仓库和不能为空）
        /// </summary>
        /// <param name="item"></param>
        public virtual void ValidateWarehouse(ControllerBase.SaveEvent item)
        {
            StockOutOrder sin = item.Item as StockOutOrder;
            if (sin.TOWHNO == 0)
            {
                throw new Exception("异常：仓库不能为空！");
            }
            IDataAction action = this.model.GetDataAction();
            WareHouse wh = sin.GetForeignObject<WareHouse>(action);
            wh.ChildOffice.DataAction = action;
            wh.ChildOffice.GetData();
            if (wh.ISOFFER && wh.ChildOffice.Count > 0)
            {
                bool iswork = wh.ChildOffice.Exists(delegate(OWorker ow) { return ow.ID.ToString() == this.user.ID; });
                if (!iswork)
                    throw new Exception("异常：登陆人+" + this.user.name + ",不是" + wh.WAREHOUSENAME + "的管理员，不能操作仓库业务！");
            }
        }

        /// <summary>
        /// 添加不同类型 -1是默认值 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="stockType"></param>
        public virtual int SetStockType()
        {
            return -1;
        }

        /// <summary>
        /// 重写方法（设置出库单的源单控制器）
        /// </summary>
        /// <returns></returns>
        public virtual string OutOrderController()
        {
            return "";
        }

        /// <summary>
        /// 添加页面默认值（主表：SourceController、SourceID、SourceCode）、（子表：SourceRowID、SourceTable）
        /// </summary>
        /// <param name="item"></param>
        public virtual void AddDefault(ControllerBase.SaveEvent item)
        {
            StockOutOrder ord = item.Item as StockOutOrder;
            StockOutNoticeMaterials sm = new StockOutNoticeMaterials();
            if (ord.SourceCode != null)
            {
                //StockOutNotice notice = ord.GetForeignObject<StockOutNotice>(this.model.GetDataAction());
                EntityList<StockOutNotice> notice = new EntityList<StockOutNotice>(this.model.GetDataAction());
                notice.GetData("code='"+ord.SourceCode+"'");
                ///如果目标单据的sourceId =0 代表没有使用下推方法，则赋值SourceID和SourceController
                if (notice.Count>0)
                {
                    ord.SourceID = notice[0].ID;
                    ord.SourceController = OutOrderController();
                    var a = notice[0].Materials;
                    a.DataAction = this.model.GetDataAction();
                   a.GetData();
                    for (int i = 0; i <  a.Count; i++)
                    {
                        for (int j = 0; j < ord.Materials.Count; j++)
                        {
                            if (!string.IsNullOrEmpty(a[i].BATCHNO.Trim()))
                            {
                                if (a[i].MATERIALCODE == ord.Materials[j].MATERIALCODE && a[i].BATCHNO.Trim() == ord.Materials[j].BATCHNO.Trim())
                                {
                                    ord.Materials[j].SourceRowID = a[i].ID;
                                    ord.Materials[j].SourceTable = sm.ToString();
                                }
                            }
                            else
                            {
                                if (a[i].MATERIALCODE == ord.Materials[j].MATERIALCODE && ord.Materials[j].SourceRowID==0)
                                {
                                    ord.Materials[j].SourceRowID = a[i].ID;
                                    ord.Materials[j].SourceTable = sm.ToString();
                                }
                            }
                        }
                    }

                }
            }
        }

        [WhereParameter]
        public string NoticeId { get; set; }
        public virtual string GetNoticeStayGrid()
        {
            EntityList<StockOutNoticeMaterials> List1 = new EntityList<StockOutNoticeMaterials>(this.model.GetDataAction());
            EntityList<StockOutNotice> soList = new EntityList<StockOutNotice>(this.model.GetDataAction());
            soList.GetData("code='" + NoticeId + "'");
            List1.GetData("parentId='" + soList[0].ID + "'");
            List<StockOutNoticeMaterials> list = new List<StockOutNoticeMaterials>();
            list = List1;
            string str = JSON.Serializer(list.ToArray());
            return str;
        }

        #endregion

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

        protected virtual void CheckTransformData()
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
        }
        #endregion
    }
}
