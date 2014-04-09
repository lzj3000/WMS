using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Business.WMS.Model;
using Acc.Business.Controllers;
using Acc.Contract;
using Acc.Contract.MVC;
using Way.EAP.DataAccess.Entity;
using Acc.Contract.Data.ControllerData;
using Acc.Contract.Data;
using System.Data;
using Way.EAP.DataAccess.Data;
using Acc.Business.Model;
using System.Collections;
using Acc.Contract.Center;

namespace Acc.Business.WMS.Controllers
{
    public class StockInOrderController : BusinessController
    {
        /// <summary>
        /// 描述：入库单控制器
        /// 作者：路聪
        /// 最后修改日期:2013-07-01
        /// </summary>
        public StockInOrderController() : base(new StockInOrder()) {
            /*
             //   重新绑定外键，暂时不要
            IEntityBase ieb = (IEntityBase)this.model;
            //ieb.ForeignKey += new ForeignKeyHandler(StockoutForeignKeyHandler);

            IHierarchicalEntityView[] view = ieb.GetChildEntityList();
            for (int i = 0; i < view.Length; i++)
            {
                if (view[i].ChildEntity.ToString().Equals("Acc_WMS_StockInNoticeMaterials", StringComparison.OrdinalIgnoreCase))
                {
                    ((IEntityBase)view[i].ChildEntity).ForeignKey += new ForeignKeyHandler(SendListForeignKeyHandler);
                }
            }
             */ 
        }
        /*
        #region 取消外键,并重新绑定
        /// <summary>
        /// 入库申请和入库主表绑定主外键
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="target"></param>
        void SendListForeignKeyHandler(EntityBase entity, Dictionary<string, EntityForeignKeyAttribute> target)
        {
            if (target.ContainsKey("PARENTID"))
            {
                target.Remove("PARENTID");
            }
            target.Add("PARENTID", new EntityForeignKeyAttribute(typeof(StockInOrder), "SourceID"));
        }
        #endregion
         */ 
        /*
        #region 动态绑定主外键
        /// <summary>
        /// 动态绑定外键
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="target"></param>
        void StockoutForeignKeyHandler(EntityBase entity, Dictionary<string, EntityForeignKeyAttribute> target)
        {
            if (target.ContainsKey("SourceCode"))
            {
                target.Remove("SourceCode");
            }
            target.Add("SourceCode", new EntityForeignKeyAttribute(typeof(StockInNotice), "ID", "Code"));
        }
        #endregion 动态绑定主外键
         */ 

        public StockInOrderController(IModel model) : base(model) { }
        //是否启用审核
        public override bool IsReviewedState
        {
            get
            {
                return true;
            }
        }
        //是否启用提交
        public override bool IsSubmit
        {
            get
            {
                return true;
            }
        }
        //是否启用回收站
        public override bool IsClearAway
        {
            get
            {
                return false;
            }
        }

        public override bool IsPushDown
        {
            get
            {
                return true;
            }
        }

        //显示在菜单
        protected override string OnControllerName()
        {
            return "入库单管理";
        }
        //菜单中url路径
        protected override string OnGetPath()
        {
            return "Views/WMS/StockInOrder/StockInOrder.htm";
        }
        //开发人
        protected override string OnGetAuthor()
        {
            return "路聪";
        }
        //说明
        protected override string OnGetControllerDescription()
        {
            return "入库单管理";
        }


        #region 初始化数据方法
        public virtual void OnInitOrderIn(Contract.Data.ModelData data, Contract.Data.ItemData item)
        {
            if (data.name.EndsWith("StockInOrder"))
            {
                switch (item.field.ToLower())
                {
                    case "issynchronous":
                        item.disabled = true;
                        break;
                }
            }

            if (data.name.EndsWith("StockInOrderMaterials"))
            {
                if (item.IsField("materialcode"))
                {
                    if (item.foreign != null)
                    {
                        //item.foreign.rowdisplay.Add("FMOBILE", "FMOBILE");//key是联系人表字段，value是客户表字段
                        item.foreign.rowdisplay.Add("FUNITID", "MATERIALPACKID");
                        item.foreign.rowdisplay.Add("FMODEL", "FMODEL");
                        item.foreign.rowdisplay.Add("CODE", "MCODE");
                    }
                }

            }

            if (data.name.EndsWith("Sequence"))
            {
                switch (item.field.ToLower())
                {
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
                    case "isdisable":
                    case "isdelete":
                    case "remark":
                    case "id":
                    case "code":
                    case "ordermaterialid":
                    case "stockinfomaterialsid":
                        item.visible = false;
                        break;
                    default:
                        item.visible = true;
                        break;
                }
            }
            if (data.name.EndsWith("StockInNoticeMaterials"))
            {
                data.title = "待入库明细";
                switch (item.field.ToLower())
                {
                    case "parentid":
                        StockInOrder order = new StockInOrder();
                        item.visible = false;
                        item.foreign.isfkey = true;
                        item.foreign.foreignfiled = "SOURCEID";
                        item.foreign.filedname = item.field;
                        //item.foreign.displayfield = ("ModelName").ToUpper();显示的名称
                        item.foreign.objectname = data.name;
                        item.foreign.foreignobject = order.GetType().FullName;
                        item.foreign.tablename = order.ToString();
                        break;
                }
            }
            //if (data.name.EndsWith("LogData"))
            //{
            //    //data.title = "待入库明细";
            //    switch (item.field.ToLower())
            //    {
            //        case "sourceid":
            //            StockInOrder order = new StockInOrder();
            //            item.visible = false;
            //            item.foreign.isfkey = true;
            //            item.foreign.foreignfiled = "ID";
            //            item.foreign.filedname = item.field;
            //            //item.foreign.displayfield = ("ModelName").ToUpper();显示的名称
            //            item.foreign.objectname = data.name;
            //            item.foreign.foreignobject = order.GetType().FullName;
            //            item.foreign.tablename = order.ToString();
            //            break;
            //    }
            //}
        }
        protected override void OnInitViewChildItem(Contract.Data.ModelData data, Contract.Data.ItemData item)
        {
            OnInitOrderIn(data, item);
        }

        #endregion

        #region 添加数据方法
        protected override void OnAdding(Contract.MVC.ControllerBase.SaveEvent item)
        {
           //// MergerOrderMaterials(item);
           // //AddInOrder(item);
           // StockInOrder stockinorder = item.Item as StockInOrder;
           // EntityList<StockInNotice> noticelist = new EntityList<StockInNotice>(this.model.GetDataAction());
           // noticelist.GetData(" code='" + stockinorder.SourceCode + "'");
           // if (noticelist.Count>0)
           // {
           //     stockinorder.SourceID = noticelist[0].ID;
           // }
            base.OnAdding(item);
            //huwei(item,"1");
        }
        protected override void OnRemoveing(SaveEvent item)
        {
            base.OnRemoveing(item);
           // huwei1(item, "0");
        }
        protected override void OnEdited(EntityBase item)
        {
            base.OnEdited(item);

        }
        //public void huwei(BasicInfo item, string status)
        //{
        //    StockInOrder ord = item as StockInOrder;
        //    WareHouse wh = new WareHouse();
        //    Ports p = new Ports();
        //    if (ord.Materials.Count == 0)
        //    {
        //        ord.Materials.DataAction = this.model.GetDataAction();
        //        ord.Materials.GetData();
        //    }
        //        for (int i = 0; i < ord.Materials.Count; i++)
        //        {
        //            //string sql = "update " + wh.ToString() + " set STATUS='" + status + "' where id = '" + ord.Materials[i].DEPOTWBS + "'";
        //            string sql = "update " + p.ToString() + " set STATUS ='" + status + "'  where id='" + ord.Materials[i].PORTNAME + "'";
        //            this.model.GetDataAction().Execute(sql);
        //        }

        //}
        //public void huwei1(SaveEvent item, string status)
        //{
        //    StockInOrder ord = item.Item as StockInOrder;
        //    WareHouse wh = new WareHouse();
        //    Ports p = new Ports();
        //        ord.Materials.DataAction = this.model.GetDataAction();
        //        ord.Materials.GetData("parentid='" + ord.ID + "'");
        //        for (int i = 0; i < ord.Materials.Count; i++)
        //        {
        //            //string sql = "update " + wh.ToString() + " set STATUS='" + status + "' where id = '" + ord.Materials[i].DEPOTWBS + "'";
        //            string sql = ";update " + p.ToString() + " set STATUS ='" + status + "'  where id='" + ord.Materials[i].PORTNAME + "'";
        //            this.model.GetDataAction().Execute(sql);
        //        }
        //}

        public void tuanpan(SaveEvent item, string status)
        {
            StockInOrder ord = item.Item as StockInOrder;
            StockInOrderMaterials sm = new StockInOrderMaterials();
            if (ord.SourceCode != null)
            {
                ///如果目标单据的sourceId =0 代表没有使用下推方法，则赋值SourceID和SourceController
                EntityList<StockInOrderMaterials> sin = new EntityList<StockInOrderMaterials>(this.model.GetDataAction());
                sin.GetData("code='" + ord.SourceCode + "'");
                if (sin.Count > 0)
                {
                    for (int i = 0; i < sin.Count; i++)
                    {
                        string sql = "update " + new Ports().ToString() + " set status='" + status + "' where id = '" + sin[i].PORTNAME + "'";
                        this.model.GetDataAction().Execute(sql);
                    }
                }
            }
        }

        /// <summary>
        /// 合并同行
        /// </summary>
        /// <param name="item"></param>
        public virtual void MergerOrderMaterials(ControllerBase.SaveEvent item)
        {
            StockInOrder inOrder = item.Item as StockInOrder;
            ///获取当前出库通知单子级的所有数据的集合
            var list = inOrder.Materials;

            //两个要保存的list
            EntityList<StockInOrder> sooList = new EntityList<StockInOrder>(this.model.GetDataAction());
            if (list.Count > 0)
            {
                try
                {
                    ///合并完成的结果
                    var linqList = LinqHelp.MergerStockInOrderMaterials(list);
                    inOrder.Materials.Clear();
                    foreach (var i in linqList)
                    {
                        inOrder.Materials.Add(i);
                    }
                    sooList.Add(inOrder);
                }
                catch (Exception ex)
                {
                    throw new Exception("异常" + ex.Message + "");
                }
            }
        }

        ///// <summary>
        ///// 添加数据时的处理
        ///// </summary>
        ///// <param name="item"></param>
        //public virtual void AddInOrder(ControllerBase.SaveEvent item)
        //{
        //    StockInOrder stockinorder = item.Item as StockInOrder;
        //    EntityList<StockInOrder> aaa = new EntityList<StockInOrder>(this.model.GetDataAction());
        //    aaa.GetData(" id=" + stockinorder.ID);
        //    if (stockinorder != null)
        //    {
        //        EntityList<WareHouse> whList = new EntityList<WareHouse>(this.model.GetDataAction());
        //        whList.GetData("id='" + stockinorder.TOWHNO + "'");
        //        if (whList.Count > 0)
        //        {
        //            if (whList[0].ISOFFER)
        //            {
        //                ValidateWareHouse(stockinorder);
        //            }
        //        }
        //        for (int i = 0; i < stockinorder.Materials.Count; i++)
        //        {
        //            var sm = stockinorder.Materials[i];
        //        Materials a = sm.GetForeignObject<Materials>(this.model.GetDataAction());
        //        double tempNum = sm.NUM;
        //        stockinorder.Materials.ForEach(c =>
        //        {
        //            var group = stockinorder.Materials.Where(aa => aa.MATERIALCODE == c.MATERIALCODE);
        //            c.NUM = group.Sum(x => x.NUM);
        //            tempNum = c.NUM;
        //        });
        //        if (tempNum > Convert.ToDouble(sm.GetNum(new StockInNoticeMaterials().ToString(), sm.SourceRowID, sm.MATERIALCODE)))
        //            throw new Exception("序号:" + sm.RowIndex + " 商品名称:" + a.FNAME + "  商品编码：" + a.Code + " 数量超过源单数量");
        //        //验证数据是否有效
        //        ValidateInfo(sm);
        //        //if(sm.SourceRowID
        //        //保存下推前测试
        //        //if(stockinorder.SourceController!="Acc.Business.AcctrueWMS.Controllers.AcctrueSellOrderController")
        //        //if (sm.NUM > Convert.ToDouble(sm.GetNum(sm.MATERIALCODE, sm.SourceRowID) - sm.GetSourceNum(this.ToString(), sm.SourceRowID, sm.MATERIALCODE,0, "入库")) && sm.GetNum(sm.MATERIALCODE, sm.SourceRowID) != 0)

        //        if (sm.NUM + sm.GetSourceNum(new StockInNoticeMaterials().ToString(),sm.ID, sm.SourceRowID, sm.MATERIALCODE) > Convert.ToDouble(sm.GetNum(new StockInNoticeMaterials().ToString(), sm.SourceRowID, sm.MATERIALCODE)) && Convert.ToDouble(sm.GetNum(new StockInNoticeMaterials().ToString(), sm.SourceRowID, sm.MATERIALCODE)) > 0)
        //        {
        //            throw new Exception("序号:"+sm.RowIndex+" 商品名称:" + a.FNAME + "  商品编码：" + a.Code + " 数量超过源单数量");
        //        }
        //        //stockinorder.Materials.RemoveAll(delegate(StockInOrderMaterials m)
        //        //{
        //        //    return m.NUM <= 0;
        //        //});
        //        //得到物料

        //        if (a.BATCH == true && sm.BATCHNO == "")
        //        {
        //            throw new Exception("序号:" + sm.RowIndex + " 商品名称:" + a.FNAME + "  商品编码：" + a.Code + "  为批次管理产品，批次号不能为空！");
        //        }
        //        /*
        //        sm.ORDERNO = stockinorder.ID.ToString();
        //        sm.PORTNO = "1";
        //        sm.DEPOTWBS = "1";
        //        //判断添加的数据中序列码的正确性
        //        ValiSelfSequence(sm);
        //        if (m.SEQUENCECODE == true && sm.NUM != sm.InSequence.Count)
        //        {
        //            throw new Exception("异常：" + m.FNAME + " 入库数量和序列号数不相符！");
        //        }

        //        foreach (InSequence sq in sm.InSequence)
        //        {
        //            ValiSequence(sq);
        //            sq.ORDERMATERIALID = sm.ID;
        //        }
        //            */
        //    }
               
                
        //    }
        //}

        ///// <summary>
        ///// 验证入库权限是否可以通过
        ///// </summary>
        ///// <param name="stockinorder"></param>
        //public virtual void ValidateWareHouse(StockInOrder stockinorder)
        //{
        //    EntityList<OWorker> ow = new EntityList<OWorker>(this.model.GetDataAction());
        //    ow.GetData();
        //    if (ow.Count > 0)
        //    {
        //        int res = 0;
        //        ow.GetData("workName='" + this.user.ID + "'");
        //        if (ow.Count > 0)
        //        {
        //            for (int i = 0; i < ow.Count; i++)
        //            {
        //                EntityList<WareHouse> wh = new EntityList<WareHouse>(this.model.GetDataAction());
        //                wh.GetData("(type=" + ow[i].WHID + " or id=" + ow[i].WHID + ") and ISOFFER=1");
        //                for (int j = 0; j < wh.Count; j++)
        //                {
        //                    if (Convert.ToInt32(stockinorder.TOWHNO) == wh[j].ID)
        //                    {
        //                        res = 1;

        //                    }

        //                }
                       
        //            }
        //            if (res == 0)
        //            {
        //                throw new Exception("异常：没有入库到此仓库的权限");
        //            }
        //        }
        //        else
        //        {
        //            throw new Exception("异常：没有入库到此仓库的权限");
        //        }
        //    }
        //}

        ///// <summary>
        ///// 验证数据的正确性
        ///// </summary>
        ///// <param name="sm"></param>
        //private void ValidateInfo(StockInOrderMaterials sm)
        //{
        //    //ValiInNoticeMaterials(sm);
        //    //ValiOutOrderMaterials(sm);
        //    //判断出库明细数量不能小于0
        //    if (sm.NUM <= 0)
        //    {
        //        throw new Exception("序号："+sm.RowIndex+" 商品编码"+sm.MCODE+" 请输入正确的数量");
        //    }
        //    //判断出库产品价格不能小于0
        //    //if (sm.PRICE <= 0)
        //    //{
        //    //    throw new Exception("请输入正确的产品价格");
        //    //}
        //}

        /*
        /// <summary>
        /// 判断实体中物料是不是在生产计划中
        /// </summary>
        /// <param name="sm"></param>
        private void ValiInNoticeMaterials(StockInOrderMaterials sm)
        {
            string str = "N";
            EntityList<StockFinishInOrder> listsq = new EntityList<StockFinishInOrder>(this.model.GetDataAction());
            listsq.GetData("ID=" + sm.PARENTID);
            StockFinishInOrder sfi = listsq[0];// sm.GetForeignObject<StockFinishInOrder>(this.model.GetDataAction());
            if (sfi.SourceController == "Acc.Business.WMS.Controllers.AcctrueProductionPlanController")
            {
                StockInNotice sin = sfi.GetForeignObject<StockInNotice>(this.model.GetDataAction());
                foreach (StockInNoticeMaterials siom in sin.Materials)
                {
                    if (sm.MATERIALCODE == siom.MATERIALCODE)
                    {
                        str="Y";
                    }
                }
            }
            if (str == "N")
            {
                Materials ms = sm.GetForeignObject<Materials>(this.model.GetDataAction());
                throw new Exception(ms.FNAME + " 不存在该单据");
            }
        }


        /// <summary>
        /// 
        /// 判断实体中物料是不是在出库明细中
        /// </summary>
        /// <param name="sm"></param>
        private void ValiOutOrderMaterials(StockInOrderMaterials sm)
        {
            string str = "N";
            EntityList<StockFinishInOrder> listsq = new EntityList<StockFinishInOrder>(this.model.GetDataAction());
            listsq.GetData("ID=" + sm.PARENTID);
            StockInOrder sfi = listsq[0];// sm.GetForeignObject<StockInOrder>(this.model.GetDataAction());
            if (sfi.SourceController == "Acc.Business.WMS.Controllers.AcctrueSellOrderController")
            {
                StockOutOrder soo = sfi.GetForeignObject<StockOutOrder>(this.model.GetDataAction());
                foreach (StockOutOrderMaterials siom in soo.Materials)
                {
                    if (sm.MATERIALCODE == siom.MATERIALCODE)
                    {
                        str="Y";
                    }
                }
            }
            if (str == "N")
            {
                Materials ms = sm.GetForeignObject<Materials>(this.model.GetDataAction());
                throw new Exception(ms.FNAME + " 不存在该单据");
            }
        }
        */

        
        ///// <summary>
        ///// 添加时验证序列码的正确性
        ///// </summary>
        ///// <param name="sm"></param>
        ///// <param name="son"></param>
        //private void ValiSelfSequence(StockInOrderMaterials stockinordermaterials)
        //{
        //    Materials m = stockinordermaterials.GetForeignObject<Materials>(this.model.GetDataAction());
        //    for (int i = 0; i < stockinordermaterials.InSequence.Count; i++)
        //    {
        //        for (int j = i + 1; j < stockinordermaterials.InSequence.Count; j++)
        //        {
        //            if (stockinordermaterials.InSequence[i].SEQUENCECODE == stockinordermaterials.InSequence[j].SEQUENCECODE)
        //            {
        //                throw new Exception(m.FNAME + "中有相同的序列码，操作终止！");
        //            }
        //        }
        //    }
        //}
          

        ///// <summary>
        ///// 验证序列码的正确性
        ///// </summary>
        ///// <param name="sq"></param>
        //private void ValiSequence(InSequence sq)
        //{
        //    EntityList<InSequence> listsq = new EntityList<InSequence>(this.model.GetDataAction());
        //    listsq.GetData("ID=" + sq.ID);
        //    string strwhere = " and  SEQUENCECODE='" + sq.SEQUENCECODE + "'";
        //    //判断有没有id
        //    if (listsq.Count > 0)  //有id
        //    {
        //        foreach (InSequence seq in listsq)
        //        {
        //            //有ID的情况下判断序列码是不是相同
        //            if (seq.SEQUENCECODE != sq.SEQUENCECODE)//不同的情况下
        //            {
        //                //在判断表中是不是序列码的唯一性
        //                SoleSequence(sq);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        SoleSequence(sq);
        //    }
        //}

        ///// <summary>
        ///// 判断序列码的唯一性
        ///// </summary>
        ///// <param name="sq"></param>
        //private void SoleSequence(InSequence sq)
        //{
        //    string strwhere = " and  SEQUENCECODE='" + sq.SEQUENCECODE + "'";
        //    int i = WMShelp.ValiSole(sq, strwhere);
        //    if (i > 0)
        //    {
        //        throw new Exception("已存在 " + sq.SEQUENCECODE + " 该序列码！");
        //    }

        //}
        #endregion

        #region 修改数据方法
        protected override void OnEditing(ControllerBase.SaveEvent item)
        {
            ////Merger(item);
            ////AddInOrder(item);
            //UpdateStayMaterials(item);
            base.OnAdding(item);
        }

        //protected void UpdateStayMaterials(ControllerBase.SaveEvent item)
        //{
        //    StockInOrder son = item.Item as StockInOrder;
        //    if (son.ID!=0)
        //    {
        //        for (int i = 0; i < son.Materials.Count;i++ )
        //        {
        //            if (son.Materials[i].ID==0)
        //            {
        //                EntityList<StockInNoticeMaterials> list = new EntityList<StockInNoticeMaterials>(this.model.GetDataAction());
        //                list.GetData(" PARENTID=" + son.SourceID + " and MATERIALCODE='" + son.Materials[i].MATERIALCODE + "'");
        //                if (list.Count>0)
        //                {
        //                    list[0].FINISHNUM = list[0].FINISHNUM + son.Materials[i].NUM;
        //                    list.Save();
        //                }
        //                //else
        //                //    throw new Exception("该单据中待操作明细没有该产品！");
        //            }
        //        }
        //    }
        //}

        ///// <summary>
        ///// 合并出库通知单
        ///// </summary>
        ///// <param name="item"></param>
        //private static void Merger(ControllerBase.SaveEvent item)
        //{
        //    StockInOrder son = item.Item as StockInOrder;
        //    var result = from o in son.Materials group o by o.MATERIALCODE into g select new { g.Key, Totle = g.Sum(p => p.NUM), Items = g.ToList<StockInOrderMaterials>() };
        //    foreach (var group in result)
        //    {
        //        StockInOrderMaterials m = group.Items.Find(delegate(StockInOrderMaterials mm) { return ((IEntityBase)mm).StateBase == EntityState.Select; });
        //        if (m == null)
        //            m = group.Items[0];
        //        m.NUM = group.Totle;
        //        List<StockInOrderMaterials> removelist = group.Items.FindAll(delegate(StockInOrderMaterials mm) { return mm != m; });
        //        removelist.ForEach(delegate(StockInOrderMaterials mm)
        //        {
        //            son.Materials.Remove(mm);
        //        });
        //    }
        //}

        #endregion

        #region 获取按钮方法
        protected override ActionCommand[] OnInitCommand(ActionCommand[] commands)
        {
            //获取所有按钮集合
            foreach (ActionCommand ac in commands)
            {
                if (ac.command == "UnReviewedData")
                {
                    ac.visible = false;
                    //ac.name = "分派";
                    //ac.title = "分派提交单据";
                }
            }
            return base.OnInitCommand(commands);
        }
        /// <summary>
        /// 审核数据验证和修改库存
        /// </summary>
        /// <param name="info"></param>
        public virtual void OnWmsInOrderReviewedData(BasicInfo info)
        {
            ValiOnReview(info);
            LostBill(info);
        }
        /// <summary>
        /// 审核，并相应改变库存产品数量
        /// </summary>
        /// <param name="info"></param>
        protected override void OnReviewedData(BasicInfo info)
        {
            
            OnWmsInOrderReviewedData(info);
        }

        /// <summary>
        /// 撤销审核按钮
        /// </summary>
        /// <param name="info"></param>
        protected override void OnUnReviewedData(BasicInfo info)
        {
            ////直接撤销库存的数量
            /////修改库存为负数
            //StockInOrder order = GetInfo(info);
            //for (int i = 0; i < order.Materials.Count; i++)
            //{
            //    order.Materials[i].NUM = order.Materials[i].NUM * -1;
            //}
            //LostBill(info);
            /////修改完成后负负得正数，否则入库单据数量会为负数
            //for (int i = 0; i < order.Materials.Count; i++)
            //{
            //    order.Materials[i].NUM = order.Materials[i].NUM *-1;
            //}
            //已经用观察者
            base.OnUnReviewedData(info);
        }

        /// <summary>
        /// 得到当前操作父级子集数据
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private StockInOrder GetInfo(BasicInfo info)
        {
            StockInOrder order = info as StockInOrder;
            order.Materials.DataAction = this.model.GetDataAction();
            order.Materials.GetData();
            return order;
        }

        /// <summary>
        /// 验证数据的正确性
        /// </summary>
        /// <param name="sm"></param>
        /// <param name="son"></param>
        public virtual void ValiOnReview(BasicInfo info)
        {
            if (!info.IsReviewed)
            {
                StockInOrder order = GetInfo(info);
                ValiData(info);
            }
            else
            {
                throw new Exception("异常：单据已审核，不能再次审核！");
            }
        }

        /// <summary>
        /// 提交按钮
        /// </summary>
        /// <param name="info"></param>
        protected override void OnSubmitData(BasicInfo info)
        {
            ValiOnSubmit(info);
            base.OnSubmitData(info);
           
            //huwei(info, "1");

        }
        /// <summary>
        /// 提交验证
        /// </summary>
        /// <param name="info"></param>
        public virtual void ValiOnSubmit(BasicInfo info)
        {
            if (!info.IsSubmited)
            {
            }
            else
            {
                throw new Exception("异常：单据已提交，不能再次提交！");
            }
            //判断是否启用审核，如果没启用则直接入库
            if (!this.IsReviewedState)
            {
                ValiData(info);
                LostBill(info);
            }
        }
        /// <summary>
        /// 验证数据是否可以保存
        /// </summary>
        /// <param name="info"></param>
        private void ValiData(BasicInfo info)
        {
           
            StockInOrder order = info as StockInOrder;
                            order.Materials.DataAction = this.model.GetDataAction();
                order.Materials.GetData();
            if (order.Materials.Count > 0)
            {
                EntityList<Materials> ms = new EntityList<Materials>(this.model.GetDataAction());
                for (int i = 0; i < order.Materials.Count; i++)
                {
                    ms.GetData("id='" + order.Materials[i].MATERIALCODE + "'");
                    if (ms.Count > 0)
                    {
                        switch (ms[0].STATUS)
                        {
                            case "1": throw new Exception(ms[0].FNAME + " 状态为冻入，不可进行入库操作");
                            case "3": throw new Exception(ms[0].FNAME + " 状态为不可用，不可进行出入库操作");
                        }
                    }
                }
            }
            else
            {
                //throw new Exception("异常：单据无明细数据，不能提交！");
            }
        }

        /// <summary>
        /// 验证数据的正确性
        /// </summary>
        /// <param name="sm"></param>
        /// <param name="son"></param>
        private bool ValiSubmit(StockInOrder sm)
        {
            //判断入库单状态
            if (sm.IsSubmited == true)
            {
                throw new Exception("异常：只有新建状态下才可以进行分派操作！");
            }
            else
            {
                return true;
            }
        }
        #endregion
        
        #region 添加按钮
        //[ActionCommand(name = "入库", title = "入库", index = 10, icon = "icon-ok", isalert = true)]
        /// <summary>
        /// 审核时，修改库存
        /// </summary>
        /// <param name="info"></param>
        public virtual void LostBill(BasicInfo info)
        {
           
            IDataAction action = this.model.GetDataAction();
            //验证盘点单是否已经创建并提交未审核状态。如果已提交未审核且为静态盘点，则不能出入库操作。
            CheckPDOrder(action);
            StockInOrder stockinorder = info as StockInOrder;
            Ports ps = null;
            Materials m = null;
            StockInfoMaterialsController simc = new StockInfoMaterialsController();
            StockInfoMaterials sm  = null;
            stockinorder.Materials.DataAction = action;
            stockinorder.Materials.GetData();
            try
            {
                //开始事务
                action.StartTransation();
                sm.ORDERNO = stockinorder.Code;
                for (int a = 0; a < stockinorder.Materials.Count; a++)
                {
                    StockInOrderMaterials sim = stockinorder.Materials[a];
                    sm = new StockInfoMaterials();

                    if (!string.IsNullOrEmpty(sim.PORTNAME))
                    {
                        string portNo = sm.GetAppendPropertyKey("PORTCODE");
                        string portNo1 = sim.GetAppendPropertyKey("PORTNAME");
                        sm.PORTCODE = sim.PORTNAME;
                        sm[portNo] = sim[portNo1];
                    }
                    ///设置入库货位和托盘为code值
                    if (!string.IsNullOrEmpty(sim.DEPOTWBS))
                    {
                        string depotwbsCode = sm.GetAppendPropertyKey("DEPOTWBS");
                        string depotwbsCode1 = sim.GetAppendPropertyKey("DEPOTWBS");
                        sm.DEPOTWBS = sim.DEPOTWBS;
                        sm[depotwbsCode] = sim[depotwbsCode1];
                    }


                    //设置库存项
                    SetStockInfo(stockinorder, sim, sm, action);
                    ///插入入库单三级表到库存单件码
                    InsertInfoSequence(sim, sm, action);
                    if (this.OnControllerName() == "其他入库")
                    {
                        sm.STAY5 = "qt";
                    }
                    if (m.ISCHECKPRO == false)
                    {
                        sm.STATUS = "2";
                    }
                    else
                    {
                        ///如果启用质检，则验证库存或质检单中状态
                        CheckProCheckMaterials(action, sm, sim);
                    }
                    simc.putin(sm, "入库", action);
                }
                action.Commit();

            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                action.EndTransation();
            }
        }

        public virtual void CheckProCheckMaterials(IDataAction action, StockInfoMaterials sm, StockInOrderMaterials sim)
        {
            string sql = "SELECT * FROM dbo.Acc_WMS_ProCheckMaterials WHERE CHECKWARE = '" + sim.MATERIALCODE + "' AND BATCHNO='" + sim.BATCHNO + "'";
            string infosql = "SELECT STATUS FROM dbo.Acc_WMS_Infomaterials WHERE code = '" + sim.MATERIALCODE + "' AND BATCHNO='" + sim.BATCHNO + "'";
            DataTable dtpro = action.GetDataTable(sql);
            DataTable dtpro1 = action.GetDataTable(infosql);
            if (dtpro.Rows.Count > 0)
            {
                if (dtpro.Rows[0]["IsOk"].ToString().ToLower() == "true")
                    sm.STATUS = "2";

            }
            else if (dtpro1.Rows.Count > 0)
            {
                for (int i = 0; i < dtpro1.Rows.Count; i++)
                {
                    if (dtpro1.Rows[i]["STATUS"].ToString() == "2")
                    {
                        sm.STATUS = "2";
                    }
                }
            }
            else
            {
                sm.STATUS = "1";
            }
        }

        /// <summary>
        /// 检查盘点单
        /// </summary>
        /// <param name="action"></param>
        public virtual void CheckPDOrder(IDataAction action)
        {
            EntityList<CheckOrder> dfList = new EntityList<CheckOrder>(action);
            dfList.GetData();
            foreach (var item in dfList)
            {
                if (item.IsSubmited == true && item.IsReviewed == false)
                {

                    if (item.CheckOrderType == "1")
                    {
                        throw new Exception("盘点单：" + item.Code + " 为静态盘点已提交未审核请稍后执行入库操作！");
                    }
                }
            }
        }

        /// <summary>
        /// 插入单件码(插入库存单件码)
        /// </summary>
        /// <param name="sim"></param>
        /// <param name="sm"></param>
        private void InsertInfoSequence(StockInOrderMaterials sim, StockInfoMaterials sm,IDataAction action)
        {
            sim.InSequence.DataAction = action;
            sim.InSequence.GetData();
            if (sim.InSequence.Count > 0)
            {
                EntityList<InfoInSequence> iisqList = new EntityList<InfoInSequence>(action);
                InfoInSequence iis = null;
                foreach (InSequence sq in sim.InSequence)
                {
                    iis = new InfoInSequence();
                    iis.SEQUENCECODE = sq.SEQUENCECODE;
                    sm.StockSequence.Add(iis);
                }
            }
        }

        /// <summary>
        /// 设置新库存
        /// </summary>
        /// <param name="stockinorder"></param>
        /// <param name="sim"></param>
        /// <param name="sm"></param>
        public virtual void SetStockInfo(StockInOrder stockinorder, StockInOrderMaterials sim, StockInfoMaterials sm, IDataAction action)
        {
            Materials ms = sim.GetForeignObject<Materials>(action);
            if (ms != null)
            {
                switch (ms.STATUS)
                {
                    case "1": throw new Exception("产品名称：" + ms.FNAME + "  产品编码：" + ms.Code + "  状态为冻入，不可进行入库操作");
                    case "3": throw new Exception("产品名称：" + ms.FNAME + "  产品编码：" + ms.Code + "  状态为不可用，不可进行出入库操作");
                }
                //改变库存数量
                sm.Code = ms.ID.ToString();
                sm.MCODE = ms.Code;
                sm.FMODEL = ms.FMODEL;
                if (ms.BATCH == false)
                {
                    if (sim.BATCHNO != "")
                    {
                        throw new Exception("产品名称：" + ms.FNAME + "  产品编码：" + ms.Code + "不允许批次管理，不可录入批次");
                    }
                    sm.BATCHNO = null;
                }
                else
                {
                    sm.BATCHNO = sim.BATCHNO;
                }
                sm.WAREHOUSEID = stockinorder.TOWHNO;
                sm.SENBATCHNO = sim.SENBATCHNO;
                if (stockinorder.STOCKTYPE == 1)
                {
                    ///针对新华扬如果为成品入库则添加最后入库时间为入库单中的生产日期，其他则添加系统当前时间
                    sm.LASTINTIME = stockinorder.INOUTTIME;
                }
                else
                {
                       sm.LASTINTIME = DateTime.Now;
                }
                sm.Creationdate = DateTime.Now;
                sm.NUM = sim.NUM;
                sm.Remark = "最后操作：" + this.OnControllerName();
            }
        }

        /// <summary>
        /// 验证入库单数据的正确性
        /// </summary>
        /// <param name="sm"></param>
        /// <param name="son"></param>
        private bool ValiInorder(StockInOrder sm)
        {
            //判断入库单状态
            //if (sm.IsSubmited==false)
            //{
            //    throw new Exception("异常：只有提交状态下才可以进行入库操作！");
            //}
            //else
            //{
            //    //return true;
            //}
            /*
            EntityList<StockInNoticeMaterials> snm = new EntityList<StockInNoticeMaterials>(this.model.GetDataAction());
            snm.GetData(" PARENTID=" + sm.SourceCode);
            //snm = sm.StayMaterials;
            var linqList = LinqHelp.MergerStockInOrderMaterials(sm.Materials);
            StockInOrderMaterials a = null;
            StockInNoticeMaterials b = null;
            Materials c = null;
            for (int i = 0; i < linqList.Count; i++)
            { 
                a=linqList[i];
                //sm.StayMaterials.GetData();
                for (int m = 0; m < snm.Count; m++)
                {
                    b = snm[m];
                    if(a.MATERIALCODE==b.MATERIALCODE)
                    {
                        c = a.GetForeignObject<Materials>(this.model.GetDataAction());
                        //超收
                        if (a.NUM > b.NUM - b.FINISHNUM  && c.ISOVERIN==false)
                        {
                            throw new Exception("异常：产品 "+c.FNAME+" 入库数量超过计划数量，并且该产品不允许超收操作！");
                        }
                        //少收
                        if (a.NUM < b.NUM - b.FINISHNUM && c.ISLESSIN == false)
                        {
                            throw new Exception("异常：产品 " + c.FNAME + " 入库数量少于计划数量，并且该产品不允许少收操作！");
                        }
                    }
                }
            }
             */ 
            return true;

        }

        /// <summary>
        /// 验证组盘表数据的正确性
        /// </summary>
        /// <param name="si"></param>
        /// <returns></returns>
        private bool ValiSetPortRecord(StockInOrder si, StockInOrderMaterials sim)
        {
            #region 爱创业务须需要屏蔽
            /*
            EntityList<SetPortRecord> splist = new EntityList<SetPortRecord>(this.model.GetDataAction());
            splist.GetData();
            splist.GetData("PORTNO= '" + si["ID"].ToString() + "' and PORTNO='" + sim.PORTNO + "' and MATERIALCODE='" + sim.MATERIALCODE + "' and    IsDelete is null ");
            if (splist.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
             */ 
            #endregion
            return true;
        }

      
        /// <summary>
        /// 验证仓库托盘和货位
        /// </summary>
        /// <param name="portno">托盘号</param>
        /// <param name="value">托盘状态</param>
        public void validepotandport(string depot, string portno)
        {
            EntityList<StockInfoMaterials> list1 = new EntityList<StockInfoMaterials>(this.model.GetDataAction());
            list1.GetData(" IsDelete='0' and PORTCODE= '" + portno + "' ");
            //仓库里有这个托盘
            if (list1.Count > 0)
            {
                foreach (StockInfoMaterials sm in list1)
                {
                    //仓库里有这个托盘，但是货位不相等，所以不能插入，如果货位相等则可以插入
                    if (sm.DEPOTWBS.ToString().Equals(depot))
                    {
                        throw new Exception("异常：仓库已经存在该托盘！");
                    }
                }
            }
            //仓库里没有这个托盘
            else
            {
                EntityList<WareHouse> listwh = new EntityList<WareHouse>(this.model.GetDataAction());
                listwh.GetData(" ID='" + depot + "' ");
                foreach(WareHouse wh in listwh)
                {
                    if (wh.STATUS == "0")
                    {
                        throw new Exception("异常：货位处于占用状态！");
                    }
                }
            }
        }

        /// <summary>
        /// 控制加载外键页面显示列顺序
        /// </summary>
        /// <param name="model"></param>
        /// <param name="data"></param>
        protected override void OnForeignIniting(IModel model, InitData data)
        {
            //data.modeldata.childitem[0].index
            base.OnForeignIniting(model, data);
        }
        /// <summary>
        /// 更改货位状态
        /// </summary>
        /// <param name="portno">托盘号</param>
        /// <param name="value">托盘状态</param>
        public void changewarehouse(string depot, string value)
        {
            EntityList<WareHouse> list1 = new EntityList<WareHouse>(this.model.GetDataAction());
            list1.GetData("ID= '" + depot + "' ");
            foreach (WareHouse wh in list1)
            {
                wh.STATUS = value;
            }
            list1.Save();
        }

        /// <summary>
        /// 更改订单状态
        /// </summary>
        /// <param name="portno">订单号</param>
        /// <param name="value">订单状态</param>
        public void changeorder(string orderid, string value)
        {
            EntityList<StockInOrder> list1 = new EntityList<StockInOrder>(this.model.GetDataAction());
            list1.GetData("ID= '" + orderid + "' ");
            foreach (StockInOrder wh in list1)
            {
                wh.STATE = Convert.ToInt32(value);
            }
            list1.Save();
        }

        /// <summary>
        /// 组盘
        /// </summary>
        /// <param name="portno">托盘号</param>
        /// <param name="num">数量</param>
        public void zupan(string portno,string materialcode,string num)
        {
            EntityList<Ports> list1= new EntityList<Ports>(this.model.GetDataAction());
            list1.GetData("PORTNO= '" + portno + "' ");
            EntityList<Materials> list2 = new EntityList<Materials>(this.model.GetDataAction());
            list2.GetData("Code= '" + materialcode + "' ");

            EntityList<SetPortRecord> splist = new EntityList<SetPortRecord>(this.model.GetDataAction());
            SetPortRecord setportrecord = new SetPortRecord();
            foreach (Ports pt in list1)
            {
               setportrecord.PORTNO = pt.ID.ToString();
            }
            foreach (Materials mt in list2)
            {
                setportrecord.MATERIALCODE = mt.Code;
            }
            setportrecord.CREATETIME = DateTime.Now.ToString();
            setportrecord.NUM = num;
            setportrecord.CREATEUSER = this.user.ID.ToString();
            setportrecord.BINCODE = "13796000";
            setportrecord.ZUPANTYPE="0";
            splist.Add(setportrecord);
            splist.Save();
        }

        /// <summary>
        /// 上架,货位的改变
        /// </summary>
        /// <param name="portno">托盘码</param>
        /// <param name="depot">货位编码</param>
        public void shangjia(string portno, string depot)
        {
            string portid="";
            string depotid="";
            EntityList<Ports> list1 = new EntityList<Ports>(this.model.GetDataAction());
            list1.GetData("PORTNO= '" + portno + "' ");
            foreach (Ports pt in list1)
            {
                portid = pt.ID.ToString();
            }
            EntityList<WareHouse> list2 = new EntityList<WareHouse>(this.model.GetDataAction());
            list2.GetData("WBSCODE= '" + depot + "' ");
            foreach (WareHouse wh in list2)
            {
                depotid = wh.ID.ToString();
            }
            EntityList<SetPortRecord> list3 = new EntityList<SetPortRecord>(this.model.GetDataAction());
            list3.GetData("PORTNO= '" + portid + "' and  IsDelete='0'");
            foreach (SetPortRecord spr in list3)
            {
                spr.BINCODE = depotid;
            }
            list3.Save();
        }
        #endregion

        #region 公共方法

        [WhereParameter]
        public string MaterialsId { get; set; }
        /// <summary>
        /// 判断物料的单位，如果是一个则返回值，否则返回空
        /// </summary>
        /// <returns></returns>
        public string VoliGetUnit()
        {
            EntityList<MaterialUnit> List1 = new EntityList<MaterialUnit>(this.model.GetDataAction());
            List1.GetData(" MATERIALID='" + MaterialsId + "'");
            if (List1.Count==1)
            {
                foreach (MaterialUnit mu in List1)
                {
                    return mu.ID.ToString();
                }
            }
            return "";
        }

        /// <summary>
        /// 得到产品下面的原料
        /// </summary>
        /// <returns></returns>
        public string GetGrid()
        {
            EntityList<PackUnit> List1 = new EntityList<PackUnit>(this.model.GetDataAction());
            List1.GetData(" MATERIALID='" + Convert.ToInt32(MaterialsId) + "'");
            List<StockInOrderMaterials> list = new List<StockInOrderMaterials>();
            if (List1.Count != 0)
            {
                PackUnit pu = List1[0];
                pu.Materials.GetData();
                foreach (PackUnitList pul in pu.Materials)
                {
                    StockInOrderMaterials sm = new StockInOrderMaterials();
                    sm.NUM = pul.NUM;
                    sm.MATERIALCODE = pul.PACKUNITCODE.ToString();
                    string name = sm.GetAppendPropertyKey("MATERIALCODE");
                    sm[name] = pul.GetAppendPropertyValue("PACKUNITCODE");
                    list.Add(sm);
                }
            }
            //return list.ToArray();
            string str=JSON.Serializer(list.ToArray());
            return str;
        }

        
        #endregion

        //protected override ControllerBase[] OnObserverTarger()
        //{
        //    //定义监控对象
        //    List<ControllerBase> list = new List<ControllerBase>();
        //    list.Add(new StockOutOrderController());//监控发货申请
        //    return list.ToArray();
        //}

        protected  override void InceptNotify(EventBase item)
        {
            if (item.EventType == CustomeEventType.Complete)
            {
                StockOutOrderController sc = item.Controller as StockOutOrderController;
                if (sc != null)
                {
                    //提交
                    if (item.MethodName == "SubmitData")
                    {
                        StockOutOrder son = item.CustomeData as StockOutOrder;
                        EntityList<StockInOrder> List1 = new EntityList<StockInOrder>(this.model.GetDataAction());
                        EntityList<StockInOrderMaterials> List2 = new EntityList<StockInOrderMaterials>(this.model.GetDataAction());
                        EntityList<StockOutOrderMaterials> List3 = new EntityList<StockOutOrderMaterials>(this.model.GetDataAction());
                        List3.GetData(" CKMXORDERNO=" + son.ID);
                        StockInOrder sin = new StockInOrder();
                        StockInOrderMaterials sinm = new StockInOrderMaterials();
                        //sin.Code = GetStockInOrder();
                        sin.SourceCode = son.ID.ToString();
                        sin.STATE = 1;
                        sin.STOCKTYPE = son.STOCKTYPE;
                        sin.CLIENTNO = son.CLIENTNO;
                        sin.TOWHNO = son.TOWHNO;
                        List1.Add(sin);
                        List1.Save();
                        foreach (StockOutOrderMaterials sonm in List3)
                        {
                            sinm.SourceController = "StockOutOrderController";
                            sinm.SourceCode = son.Code;
                            sinm.PARENTID = sin.ID;
                            sinm.MATERIALCODE = sonm.MATERIALCODE;
                            sinm.NUM = Convert.ToDouble(sonm.NUM);
                            sinm.PRICE = sonm.PRICE;
                            sinm.SourceRowID = sonm.ID;
                            List2.Add(sinm);
                        }
                        List2.Save();
                    }
                }
            }
        }

        /// <summary>
        /// 选择仓库的事件
        /// </summary>
        /// <param name="model"></param>
        /// <param name="item"></param>
        protected override void OnForeignLoading(IModel model, loadItem item)
        {
            //if (this.fdata.filedname == "MATERIALCODE")
            //{
            //    item.rowsql = "select * from (" + item.rowsql + ") a where a.Code in (select m.Code from Acc_WMS_StockInNoticeMaterials sinm left join Acc_WMS_Materials m on sinm.MATERIALCODE=m.ID  where ORDERNO='" + this.ActionItem["ORDERNO"].ToString() + "' )";
            //}
            if (this.fdata.filedname == "TOWHNO")
            {
                item.rowsql = "select * from (" + item.rowsql + ") a where WHTYPE='0' ";
            }
            if (this.fdata.filedname == "DEPOTWBS")
            {
                item.rowsql = "select * from (" + item.rowsql + ") a where WHTYPE='2'";
            }
            if (this.fdata.filedname == "PORTNO")
            {
                item.rowsql = "select * from (" + item.rowsql + ") a where a.STATUS='0'";
            }
            if (this.fdata.filedname == "MATERIALPACKID")
            {
                StockInOrderMaterials StockInOrderMaterials = model as StockInOrderMaterials;
                item.rowsql = "select * from (" + item.rowsql + ") a where a.MATERIALID='" + StockInOrderMaterials.MATERIALCODE + "'";
            }
            base.OnForeignLoading(model, item);
        }

        /// <summary>
        /// 单据转换
        /// </summary>
        /// <param name="ca"></param>
        /// <param name="actionItem"></param>
        /// <returns></returns>
        protected override EntityBase OnConvertItem(ControllerAssociate ca, EntityBase actionItem)
        {
            //下推前判断
            WMShelp.IsPush(this);
            StockInOrder cd = actionItem as StockInOrder;
            cd.Materials.DataAction = this.model.GetDataAction();
            cd.Materials.GetData();//获取子集的数据
            EntityBase eb = base.OnConvertItem(ca, actionItem);
            ///下推其他出库
            eb = PushOutOrder(eb, cd);
            return eb;
        }

        /// <summary>
        /// 入库下推判断
        /// </summary>
        /// <param name="eb"></param>
        /// <param name="cd"></param>
        /// <returns></returns>
        protected EntityBase PushOutOrder(EntityBase eb, StockInOrder cd)
        {
            if (eb is StockOutOrder)
            {
                StockOutOrder ebin = eb as StockOutOrder;
                ebin.SourceCode = cd.SourceCode;
                ebin.STATE = 1;
                ebin.STOCKTYPE = 3;
                ebin.IsSubmited = false;
                ebin.IsReviewed = false;
                ebin.SourceID = cd.ID;
                ebin.SourceCode = cd.Code;
                ebin.SourceController = this.ToString();
                ebin.Code = "";
                ebin.Submitedby="";
                ebin.Submiteddate=DateTime.MinValue;
                ebin.Reviewedby = "";
                ebin.Revieweddate = DateTime.MinValue;
                ebin.Modifiedby = "";
                ebin.Modifieddate = DateTime.MinValue;
                foreach (StockOutOrderMaterials m in ebin.Materials)
                {
                   // m.NUM = m.NUM - m.GetSourceNum(this.ToString(), m.SourceRowID,m.MATERIALCODE,0,"出库");
                }
                ebin.Materials.RemoveAll(delegate(StockOutOrderMaterials m)
                {
                    return m.NUM <= 0;
                });

                if (ebin.Materials.Count <= 0)
                {
                    throw new Exception("下推完成不能重复下推");
                }
                eb = ebin;
            }
            return eb;
        }


        /// <summary>
        /// 单据转换
        /// </summary>
        /// <returns></returns>
        protected override ControllerAssociate[] DownAssociate()
        {
            List<ControllerAssociate> list = new List<ControllerAssociate>();
            //入库单下推其他出库单
            OutOhtherOrder(list);
            return list.ToArray();
        }
        /// <summary>
        /// 应用类中重写此方法
        /// </summary>
        /// <returns></returns>
        protected virtual StockOutOrderController GetDownOrder()
        {
            return new StockOutOrderController();
        }

        /// <summary>
        /// 入库单-出库单(下推关联)
        /// </summary>
        /// <param name="list"></param>
        private void OutOhtherOrder(List<ControllerAssociate> list)
        {
            //控制器转换器(入库单-出库单)
            ControllerAssociate bl = new ControllerAssociate(this, GetDownOrder());
            //单据属性映射
            PropertyMap mapORDERID = new PropertyMap();
            ///单号ID --来源单号
            mapORDERID.TargerProperty = "SourceCode";
            mapORDERID.SourceProperty = "Code";
            bl.Convert.AddPropertyMap(mapORDERID);


            //实体类型转换器（入库单-出库单）
            ConvertAssociate NoticeMaterialsToOrderMaterials = new ConvertAssociate();
            NoticeMaterialsToOrderMaterials.SourceType = typeof(StockInOrderMaterials);//下推来源单据子集
            NoticeMaterialsToOrderMaterials.TargerType = typeof(StockOutOrderMaterials);//下推目标单据子集
            YS(NoticeMaterialsToOrderMaterials);
            bl.AddConvert(NoticeMaterialsToOrderMaterials);
            list.Add(bl);
        }

        /// <summary>
        /// 映射
        /// </summary>
        /// <param name="c"></param>
        public void YS(ConvertAssociate c)
        {
            PropertyMap mapMATERIALSID = new PropertyMap();//ID
            mapMATERIALSID.SourceProperty = "CODE";
            mapMATERIALSID.TargerProperty = "SourceCode";
            c.AddPropertyMap(mapMATERIALSID);

            PropertyMap map11 = new PropertyMap();//ID
            map11.SourceProperty = "p.ID";
            map11.TargerProperty = "SourceID";
            PropertyMap map12 = new PropertyMap();//ID
            map12.TargerProperty = "SourceController";
            map12.IsValue = true;
            map12.Value = this.ToString();


            PropertyMap map13 = new PropertyMap();//ID
            map13.SourceProperty = "SourceRowID";
            map13.TargerProperty = "SourceRowID";

            PropertyMap map14 = new PropertyMap();//ID
            map14.SourceProperty = "p.Code";
            map14.TargerProperty = "SourceCode";

            PropertyMap map15 = new PropertyMap();//ID
            map15.TargerProperty = "SourceName";
            map15.IsValue = true;
            map15.Value = ((IController)this).ControllerName();

            PropertyMap map16 = new PropertyMap();//ID
            map16.TargerProperty = "SourceTable";
            map16.IsValue = true;
            map16.Value = new StockOutOrderMaterials().ToString();

            c.AddPropertyMap(map11);
            c.AddPropertyMap(map12);
            c.AddPropertyMap(map13);
            c.AddPropertyMap(map14);
            c.AddPropertyMap(map15);
            c.AddPropertyMap(map16);
        }

        //[ActionCommand(name = "打印入库产品条码", title = "打印入库产品条码", index = 6, icon = "icon-ok", onclick = "PrintCode", isselectrow = true)]
        //public void Prit()
        //{
        //    //生成界面方法按钮用于权限控制，本方法无代码
        //}


        [WhereParameter]
        public string orderid { get; set; }
        public string GetCode()
        {
            string id = "";
            StockInOrder m = new StockInOrder();// this.ActionItem as StockInOrder;
            EntityList<StockInOrder> sio = new EntityList<StockInOrder>(this.model.GetDataAction());
            sio.GetData("ID='" + orderid + "'");
            m = sio[0];
            m.Materials.GetData();
            for (int i = 0; i < m.Materials.Count; i++)
            {
                if (id == "")
                    id = m.Materials[i].MCODE + "|" + m.Materials[i].MATERIALCODE;
                else
                    id += ";" + m.Materials[i].MCODE + "|" + m.Materials[i].MATERIALCODE;
            }
            return id;
        }

    }
}
