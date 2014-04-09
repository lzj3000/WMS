using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Business.WMS.Model;
using Acc.Contract.MVC;
using Acc.Contract.Data.ControllerData;
using Acc.Business.Model;
using Acc.Contract.Data;
using Acc.Business.WMS.XHY.Model;
using Way.EAP.DataAccess.Entity;
using Acc.Contract;
using System.Data;
using Way.EAP.DataAccess.Regulation;

namespace Acc.Business.WMS.XHY.Controllers
{
    public class ReturnInNoticeOrderCotroller : XHY_InNoticeOrderController
    {
        public ReturnInNoticeOrderCotroller() : base(new StockInNotice()) { }
        //菜单中url路径
        protected override string OnGetPath()
        {
            return "Views/WMS/XHY/ReturnInNoticeOrder.htm";
        }
        /// <summary>
        /// 显示名
        /// </summary>
        /// <returns></returns>
        protected override string OnControllerName()
        {
            return "生产退料单";
        }
        public override bool IsPushDown
        {
            get
            {
                return true;
            }
        }
        protected override void OnRemoveing(ControllerBase.SaveEvent item)
        {
            base.OnRemoveing(item);
        }

        /// <summary>
        /// 加载待入库明细
        /// </summary>
        [WhereParameter]
        public string NoticeId { get; set; }
        public string GetNoticeStayGrid()
        {
            StockOutOrderMaterials oom = new StockOutOrderMaterials();
            StockOutOrder oo = new StockOutOrder();
            Materials ms = new Materials();
            string sql = "SELECT oom.MATERIALCODE,bc.FNAME AS F2 ,oom.MCODE,oom.FMODEL,oom.NUM,oom.BATCHNO FROM " + oom.ToString() + " oom INNER JOIN " + oo.ToString() + " oo ON oo.ID = oom.PARENTID inner join " + ms.ToString() + " bc on bc.id = oom.MATERIALCODE WHERE oo.code='" + NoticeId + "'";
            DataTable dt = this.model.GetDataAction().GetDataTable(sql);
            string str = JSON.Serializer(dt);
            return str;
        }

        #region 初始化数据方法
        protected override void OnInitViewChildItem(Contract.Data.ModelData data, Contract.Data.ItemData item)
        {
            base.OnInitViewChildItem(data, item);
            if (data.name.EndsWith("StockInNotice"))
            {
                if (item.IsField("SOURCECODE"))
                {
                    if (item.foreign != null)
                    {
                        item.foreign.rowdisplay.Add("STAY5", "stay5");
                    }
                }
                data.title = "生产退料单";
                switch (item.field.ToLower())
                {
                    case "code":
                        item.title = "生产退料单号";
                        item.visible = true;
                        item.disabled = true;
                        break;
                    case "sourcename":
                    case "sourceoutcode":
                    case "workerid":
                    case "bumen":
                    case "stocktype":
                    case "finishtime":

                    case "clientno":
                        item.visible = false;
                        break;
                    case "sourcecode":
                        item.title = "生产出库单号";
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
                    case "stay5":
                        item.visible = true;
                        item.title = "车间";
                        item.required = true;
                        item.index = 2;
                        item.disabled = true;
                        break;

                }
            }
            //if (data.name.EndsWith("StockInNoticeMaterials"))
            //{
            //    data.visible = false;
            //}
            if (data.name.EndsWith("StockInNoticeMaterials"))
            {
                data.title = "生产退料单明细";
                data.isadd = false;
                switch (item.field.ToLower())
                {
                    case "code":
                    case "sourcecode":
                    case "parentid":
                        item.visible = false;
                        break;
                    case "num":
                        item.title = "退料数量";
                        break;
                    case "fmodel":
                    case "mcode":
                        item.disabled = true;
                        break;
                    case "staynum":
                    case "finishnum":
                        item.visible = false;
                        break;
                    case "batchno":
                        item.visible = true;
                        item.title = "批次号";
                        item.disabled = true;
                        break;
                    case "sourcename":
                    case "sourceoutcode":
                    case "workerid":
                    case "bumen":
                    case "stocktype":
                    case "finishtime":
                        item.visible = false;
                        break;
                    case "materialcode":
                        item.disabled = true;
                        break;

                }
            }
        }
        protected override void OnForeignIniting(IModel model, InitData data)
        {
            if (model is StockInNotice)
            {
                if (this.fdata.filedname == "SOURCECODE")
                {
                    PickOutOrderController pnoc = new PickOutOrderController();
                    data.modeldata = pnoc.Idata.modeldata;
                }
            }
        }

        protected override void OnForeignLoading(IModel model, loadItem item)
        {
            //////选择半成品项
            string W = "";
            //foreach (SQLWhere w in item.whereList)
            //    W += w.where("and   ");
            if (item.whereList.Length > 0)
            {
                string file = item.whereList[0].ColumnName;
                string symol = item.whereList[0].Symbol;
                string value = item.whereList[0].Value;
                file = file.Substring(file.LastIndexOf('.'));
                file = "a" + file;

                W = "and  " + file + "  " + symol + "   " + value;
            }
            if (this.fdata.filedname == "SOURCECODE")
            {
                PickOutOrderController pnoc = new PickOutOrderController();
                string sql = pnoc.model.GetSearchSQL();
                //item.rowsql = "select * from " + new StockOutNotice().ToString() + " a where  a.STOCKTYPE=" + new ProduceOutNoticeOrderController().SetStockType();
                item.rowsql = "select * from  (" + sql + ")  a where  IsSubmited=1 and a.STOCKTYPE=" + pnoc.SetStockType() + "  " + W;
            }
            else
            {
                base.OnForeignLoading(model, item);
            }
        }
        #endregion
        #region 重写方法
        public override int SetStockType()
        {
            return 3;
        }
        /// <summary>
        /// 设置下推对象
        /// </summary>
        /// <returns></returns>
        public override ControllerBase PushController()
        {
            return new ReturnInOrderController();
        }
        protected override void OnAdding(ControllerBase.SaveEvent item)
        {
            StockInNotice si = AddDefault(item);
            ValidateSourceNum(si);
            base.OnAdding(item);
        }

        protected override void OnEditing(ControllerBase.SaveEvent item)
        {
            StockInNotice si = AddDefault(item);
            ValidateSourceNum(si);
            base.OnEditing(item);
        }

        /// <summary>
        /// 添加默认值
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private StockInNotice AddDefault(ControllerBase.SaveEvent item)
        {
            StockInNotice si = item.Item as StockInNotice;
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
                        si.Materials[i].SourceRowID = list[0].Materials[j].ID;
                        si.Materials[j].SourceTable = list[0].Materials[j].ToString();
                    }
                }
            }
            return si;
        }

        /// <summary>
        /// 验证源单数量是否正确
        /// </summary>
        /// <param name="stockinorder"></param>
        /// <param name="i"></param>
        public virtual void ValidateSourceNum(StockInNotice sin)
        {
            for (int i = 0; i < sin.Materials.Count; i++)
            {
                if (sin.Materials[i].NUM <= 0)
                {
                    throw new Exception("序号：" + sin.Materials[i].RowIndex + " 商品编码" + sin.Materials[i].MCODE + " 请输入正确的数量");
                }
            }
            
            var result = from o in sin.Materials group o by new { o.SourceRowID } into g select new { g.Key, Totle = g.Sum(p => p.NUM), Items = g.ToList<StockInNoticeMaterials>() };
            foreach (var group in result)
            {
                StockInNoticeMaterials m = group.Items.Find(delegate(StockInNoticeMaterials mm) { return ((IEntityBase)mm).StateBase == EntityState.Select; });
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
        #endregion
        #region 下推功能
        public override string GetThisController()
        {
            return new ReturnInOrderController().ToString();
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
                if (ac.command == "UnSubmitData" || ac.command == "ReviewedData")
                {
                    ac.visible = false;
                }
                if (ac.command == "add" || ac.command == "edit" || ac.command == "remove" || ac.command == "SubmitData")
                {
                    ac.visible = true;
                }
            }
            return coms;
        }
    }
}
