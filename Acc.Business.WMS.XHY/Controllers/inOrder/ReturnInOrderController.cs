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
using Way.EAP.DataAccess.Regulation;
using Acc.Business.WMS.XHY.Model;
using Acc.Contract.Data.ControllerData;
using Acc.Business.Model;
using Acc.Contract.Data;

namespace Acc.Business.WMS.XHY.Controllers
{
    public class ReturnInOrderController : XHY_BusinessInOrderController
    {
        public ReturnInOrderController() : base(new StockInOrder()) { }
        #region 页面设置
        //显示在菜单
        protected override string OnControllerName()
        {
            return "生产退料入库单";
        }
        //菜单中url路径
        protected override string OnGetPath()
        {
            return "Views/WMS/XHY/ReturnInOrder.htm";
        }
        //说明
        protected override string OnGetControllerDescription()
        {
            return "生产退料入库管理";
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
                            d.title = "生产退料单号";
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
        /// 控制按钮
        /// </summary>
        protected override ActionCommand[] OnInitCommand(ActionCommand[] commands)
        {
            //获取所有按钮集合
            foreach (ActionCommand ac in commands)
            {
                if (ac.command == "TransferData")
                {
                    ac.visible = false;
                }
            }
            return base.OnInitCommand(commands);
        }
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
                data.title = "生产退料入库";
                switch (item.field.ToLower())
                {
                    case "code":
                        item.title = "退料单号";
                        item.disabled = true;
                        break;
                    case "sourcecode":
                        item.title = "生产出库单号";
                        item.disabled = true;
                        item.required = true;
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
                    case "stay5":
                        item.visible = true;
                        item.title = "车间";
                        item.index = 3;
                        item.required = true;
                        item.disabled = true;

                        break;


                }
            }
            if (data.name.EndsWith("StockInOrderMaterials"))
            {
                data.isadd = false;
                data.isedit = true;
                data.title = "生产退料入库明细";
                switch (item.field.ToLower())
                {
                    case "batchno":
                        item.title = "批次号";
                        item.disabled = true;
                        break;
                    case "state":
                        item.visible = false;
                        break;
                    case "num":
                        item.title = "入库数量(公斤)";
                        break;
                    case "depotwbs":
                    case "portname":
                    case "stay1":
                    case "stay2":
                    case "stay3":
                    case "stay4":
                    case "stay5":
                        item.visible = false;
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
                data.visible = false;
                switch (item.field.ToLower())
                {
                    case "staynum":
                        item.visible = true;
                        item.title = "待入库数量";
                        break;
                }
            }
            if (data.name.EndsWith("LogData"))
            {
                data.visible = false;
            }
        }
        #endregion

        #region 重写方法
        protected override void OnAdding(ControllerBase.SaveEvent item)
        {
            StockInOrder si = AddDefault(item);
            if (si.Materials.Count <= 0) {
                throw new Exception("明细行没有数据不可以保存,请查看!");
            }
            ValidateSourceNum(si);
            base.OnAdding(item);
        }
        /// <summary>
        /// 验证货位的方法
        /// </summary>
        /// <param name="item"></param>
        public override void ValidateDepotwbs(ControllerBase.SaveEvent item)
        {
            //StockInOrder so = item.Item as StockInOrder;
            //for (int i = 0; i < so.Materials.Count; i++)
            //{
            //    WareHouse wh = so.Materials[i].GetForeignObject<WareHouse>(this.model.GetDataAction());
            //    if (wh != null)
            //    {
            //        if (wh.PARENTID != so.TOWHNO)
            //        {
            //            throw new Exception("序号:" + so.Materials[i].RowIndex + "货位不属于主单仓库中!");
            //        }
            //    }
            //}
        }

        /// <summary>
        /// 验证托盘的方法
        /// </summary>
        /// <param name="item"></param>
        public override void ValidatePort(ControllerBase.SaveEvent item)
        {
            //StockInOrder so = item.Item as StockInOrder;
            //for (int i = 0; i < so.Materials.Count; i++)
            //{
            //    // Ports pt = so.Materials[i].GetForeignObject<Ports>(this.model.GetDataAction());
            //    if (so.Materials[i].PORTNAME == "")
            //    {
            //        throw new Exception("序号:" + so.Materials[i].RowIndex + "托盘不能为空！");
            //    }
            //}
        }
        /// <summary>
        /// 验证源单数量是否正确
        /// </summary>
        /// <param name="stockinorder"></param>
        /// <param name="i"></param>
        public override void ValidateSourceNum(StockInOrder sin)
        {
            for (int i = 0; i < sin.Materials.Count; i++)
            {
                if (sin.Materials[i].NUM <= 0)
                {
                    throw new Exception("序号：" + sin.Materials[i].RowIndex + " 商品编码" + sin.Materials[i].MCODE + " 请输入正确的数量");
                }
            }

            var result = from o in sin.Materials group o by new { o.SourceRowID } into g select new { g.Key, Totle = g.Sum(p => p.NUM), Items = g.ToList<StockInOrderMaterials>() };
            foreach (var group in result)
            {
                StockInOrderMaterials m = group.Items.Find(delegate(StockInOrderMaterials mm) { return ((IEntityBase)mm).StateBase == EntityState.Select; });
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
                    double bcnum = m.GetSourceNum(m.SourceTable, m.SourceRowID, m.MATERIALCODE, sin.ID);
                    if (group.Totle > snum)
                        throw new Exception("序号:" + m.RowIndex + " 商品名称:" + a.FNAME + "  商品编码：" + a.Code + " 数量超过源单数量");
                    if (group.Totle + bcnum > snum)
                        throw new Exception("序号:" + m.RowIndex + " 商品名称:" + a.FNAME + "  商品编码：" + a.Code + " 数量超过源单数量");
                }
            }

        }


        protected override void OnEditing(ControllerBase.SaveEvent item)
        {
            StockInOrder si = AddDefault(item);
            if (si.Materials.Count <= 0)
            {
                throw new Exception("明细行没有数据不可以保存,请查看!");
            }
            ValidateSourceNum(si);
            base.OnEditing(item);
        }

        /// <summary>
        /// 暂时未制定源单控制器
        /// </summary>
        /// <returns></returns>
        public override string InOrderController()
        {
            return new PickOutOrderController().ToString();
        }

        /// <summary>
        ///  设置stocktype = 3
        /// </summary>
        /// <returns></returns>
        public override int SetStockType()
        {
            return 3;
        }
        protected override void foreignInOrder(IModel model, loadItem item)
        {
            if (this.fdata.filedname == "SOURCECODE")
            {
                //item.rowsql = "select * from (" + item.rowsql + ") a where a.code in(select code from " + new StockInNotice().ToString() + " where stocktype = " + new ReturnInNoticeOrderCotroller().SetStockType() + " and IsSubmited=1)";
                item.rowsql = "select * from (" + item.rowsql + ") a where a.code in(select code from " + this.fdata.tablename + " where stocktype = " + new  PickOutOrderController().SetStockType() + " and IsSubmited=1)";
            }
            else if (this.fdata.filedname == "TOWHNO")
            {
                item.rowsql = "select * from (" + item.rowsql + ") a where a.isdisable =0 and WHTYPE=3";
            }
            else
            {
                base.foreignInOrder(model, item);
            }
        }
        public override string GetNoticeStayGrid()
        {
            StockOutOrderMaterials oom = new StockOutOrderMaterials();
            StockOutOrder oo = new StockOutOrder();
            Materials ms = new Materials();
            string sql = "SELECT oom.MATERIALCODE,bc.FNAME AS F2 ,oom.MCODE,oom.FMODEL,oom.NUM,oom.BATCHNO FROM " + oom.ToString() + " oom INNER JOIN " + oo.ToString() + " oo ON oo.ID = oom.PARENTID inner join " + ms.ToString() + " bc on bc.id = oom.MATERIALCODE WHERE oo.code='" + NoticeId + "'";
            DataTable dt = this.model.GetDataAction().GetDataTable(sql);
            string str = JSON.Serializer(dt);
            return str;
        }

        /// <summary>
        /// 添加默认值
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public StockInOrder AddDefault(ControllerBase.SaveEvent item)
        {
            StockInOrder si = item.Item as StockInOrder;
            EntityList<StockOutOrder> list = new EntityList<StockOutOrder>(this.model.GetDataAction());
            list.GetData(" Code='" + si.SourceCode + "'");
            list[0].Materials.DataAction = this.model.GetDataAction();
            list[0].Materials.GetData();
            si.SourceController = "Acc.Business.WMS.XHY.Controllers.PickOutOrderController";
            si.SourceID = list[0].ID;
            for (int i = 0; i < list[0].Materials.Count; i++)
            {
                for (int j = 0; j < si.Materials.Count; j++)
                {
                    if (si.Materials[j].MATERIALCODE == list[0].Materials[i].MATERIALCODE)
                    {
                        si.Materials[i].SourceRowID = list[0].Materials[i].ID;
                        si.Materials[j].SourceTable = list[0].Materials[i].ToString();
                    }
                }
            }
            return si;
        }
        #endregion
    }
}
