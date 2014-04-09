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
using Acc.Business.Model;
using Way.EAP.DataAccess.Data;
using System.Data;
using Acc.Contract.Center;

namespace Acc.Business.WMS.Controllers
{
    public class StockOutOrderController : BusinessController
    {
        /// <summary>
        /// 描述：出库单控制器(爱创继承此控制器)
        /// 作者：柳强
        /// 创建日期:2012-12-21
        /// </summary>
        public StockOutOrderController() : base(new StockOutOrder()) {
            /*
            //重新绑定外键，暂时不要
            IEntityBase ieb = (IEntityBase)this.model;
            IHierarchicalEntityView[] view = ieb.GetChildEntityList();
            for (int i = 0; i < view.Length; i++)
            {
                if (view[i].ChildEntity.ToString().Equals("Acc_WMS_OutOrderMaterials", StringComparison.OrdinalIgnoreCase))
                {
                    ((IEntityBase)view[i].ChildEntity).ForeignKey += new ForeignKeyHandler(SendListForeignKeyHandler);
                }
            }
             */ 
        }
        #region 取消外键,并重新绑定
        /*
        /// <summary>
        /// 出库申请和出库主表绑定主外键
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="target"></param>
        void SendListForeignKeyHandler(EntityBase entity, Dictionary<string, EntityForeignKeyAttribute> target)
        {
            if (target.ContainsKey("PARENTID"))
            {
                target.Remove("PARENTID");
            }
            target.Add("PARENTID", new EntityForeignKeyAttribute(typeof(StockOutOrder), "SourceID"));
        }
         */ 
        #endregion


        public StockOutOrderController(IModel model) : base(model) { }

        #region 初始化数据方法

        //是否启用提交
        public override bool IsSubmit
        {
            get
            {
                return true;
            }
        }

        //是否启用审核
        public override bool IsReviewedState
        {
            get
            {
                return false;
            }
        }

        //是否启用回收站(未启用)
        public override bool IsClearAway
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 启用下推
        /// </summary>
        public override bool IsPushDown
        {
            get
            {
                return false;
            }
        }

        //显示在菜单
        protected override string OnControllerName()
        {
            return "出库单管理";
        }

        //菜单中url路径
        protected override string OnGetPath()
        {
            return "Views/WMS/StockOutOrder/StockOutOrder.htm";
        }

        //开发人
        protected override string OnGetAuthor()
        {
            return "柳强";
        }

        //说明
        protected override string OnGetControllerDescription()
        {
            return "出库单管理";
        }

        /// <summary>
        /// 初始化显示字段
        /// </summary>
        /// <param name="data"></param>
        /// <param name="item"></param>
        protected override void OnInitViewChildItem(Contract.Data.ModelData data, Contract.Data.ItemData item)
        {

            if (data.name.EndsWith("StockOutOrder"))
            {

                switch (item.field.ToLower())
                {
                    case "id":
                    case "modifiedby":
                    case "modifieddate":
                    case "isdisable":
                    case "isdelete":
                    case "state":
                    case "carphone":
                    case "stocktype":
                        item.visible = false;
                        break;
                    case "code":
                        item.visible = true;
                        item.isedit = false;
                        item.disabled = true;
                        item.title = "出库单号";
                        break;
                    case "sourcecode":
                        item.visible = true;
                        item.title = "来源单号";
                        item.required = true;
                        break;
                 
                    case "towhno":
                        item.title = "出库仓库";
                        item.required = true;
                        break;
                    case "bumen":
                        item.disabled = true;
                        break;
                    case "isreviewed":
                    case "reviewedby":
                    case "revieweddate":
                        item.disabled = true;
                        item.visible = false;
                        break;
                    case "issynchronous":
                        item.disabled = true;
                        break;
                    default:
                        item.visible = true;
                        break;
                }
            }

            if (data.name.EndsWith("StockOutOrderMaterials"))
            {
                //产品编码带出规格型号
                if (item.field == "MATERIALCODE")
                {
                    if (item.foreign != null)
                    {
                        item.foreign.rowdisplay.Add("FMODEL", "FMODEL");
                        item.foreign.rowdisplay.Add("CODE", "MCODE");
                    }
                }
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
                    case "id":
                    case "code":
                    case "morderno":
                    case "sourceno":
                    case "num2":
                    case "num3":
                    case "ckmxorderno":
                    case "sourcecode"://源单编码
                    case "price":
                    case "parentid":
                        item.visible = false;
                        item.disabled = true;
                        break;
                    case "fmodel":
                    case "mcode":
                        item.disabled = true;
                        item.visible = true;
                        break;
                    case "finishnum":
                        item.disabled = true;
                        break;
                    default:
                        item.visible = true;
                        break;
                }
            }
            if (data.name.EndsWith("StockOutNoticeMaterials"))
            {
                data.title = "待出库明细";
                switch (item.field.ToLower())
                {
                    case "parentid":
                        item.visible = false;
                        item.foreign.isfkey = true;
                        item.foreign.foreignfiled = "SOURCEID";
                        item.foreign.filedname = item.field;
                        //item.foreign.displayfield = ("ModelName").ToUpper();显示的名称
                        item.foreign.objectname = data.name;
                        item.foreign.foreignobject = "Acc.Business.WMS.Model.StockOutOrder";
                        item.foreign.tablename = "Acc_WMS_OutOrder";
                        break;
                }
            }
        }

    
        ///<summary>
        ///选择仓库和货区及货位的事件
        ///</summary>
        ///<param name="model"></param>
        ///<param name="item"></param>
        protected override void OnForeignLoading(IModel model, loadItem item)
        {
            ///选择仓库的sql
            if (this.fdata.filedname == "TOWHNO")
            {
                StockOutOrder order = model as StockOutOrder;
                item.rowsql = "select * from (" + item.rowsql + ") a where a.ParentId=0";
            }
            if (this.fdata.filedname == "DEPOTWBS")
            {
                item.rowsql = "select * from (" + item.rowsql + ") a where WHTYPE='2'";
            }
            base.OnForeignLoading(model, item);
        }
        #endregion

        #region 重写方法

        #region 添加数据方法
        protected override void OnAdding(ControllerBase.SaveEvent item)
        {

            /////逻辑处理（修改单据状态）
            //AddOutOrder(item);
            /////合并相同数据行
            //MergerOrderMaterials(item);
            //StockOutOrder order = item.Item as StockOutOrder;
            //EntityList<StockOutNotice> noticelist = new EntityList<StockOutNotice>(this.model.GetDataAction());
            //noticelist.GetData(" code='" + order.SourceCode + "'");
            //if (noticelist.Count > 0)
            //{
            //    order.SourceID = noticelist[0].ID;
            //}
            base.OnAdding(item);
        }

        #endregion

        #region 编辑数据方法
        /// <summary>
        /// 编辑方法
        /// </summary>
        /// <param name="item"></param>
        protected override void OnEditing(ControllerBase.SaveEvent item)
        {

            base.OnEditing(item);
            //StockOutOrder order = item.Item as StockOutOrder;
            //if (!order.IsSubmited && !order.IsReviewed)
            //{
            //    ///调用添加的方法
            //    AddOutOrder(item);
            //    Merger(item);
            //    UpdateStayMaterials(item);
            //}
            //else
            //{
            //    throw new Exception("异常：只有新建状态下的单据可以进行编辑操作！");
            //}
        }

        //protected void UpdateStayMaterials(ControllerBase.SaveEvent item)
        //{
        //    StockOutOrder son = item.Item as StockOutOrder;
        //    if (son.ID != 0)
        //    {
        //        for (int i = 0; i < son.Materials.Count; i++)
        //        {
        //            if (son.Materials[i].ID == 0)
        //            {
        //                EntityList<StockOutNoticeMaterials> list = new EntityList<StockOutNoticeMaterials>(this.model.GetDataAction());
        //                list.GetData(" PARENTID=" + son.SourceID + " and MATERIALCODE='" + son.Materials[i].MATERIALCODE + "'");
        //                if (list.Count > 0)
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
        ///// （编辑）合并出库通知---编辑时调用
        ///// </summary>
        ///// <param name="item"></param>
        //protected virtual EntityList<StockOutOrderMaterials> Merger(ControllerBase.SaveEvent item)
        //{
        //    StockOutOrder son = item.Item as StockOutOrder;
        //    var result = from o in son.Materials group o by o.MATERIALCODE into g select new { g.Key, Totle = g.Sum(p => p.NUM), Items = g.ToList<StockOutOrderMaterials>() };
        //    foreach (var group in result)
        //    {
        //        StockOutOrderMaterials m = group.Items.Find(delegate(StockOutOrderMaterials mm) { return ((IEntityBase)mm).StateBase == EntityState.Select; });
        //        if (m == null)
        //            m = group.Items[0];
        //        m.NUM = group.Totle;
        //        List<StockOutOrderMaterials> removelist = group.Items.FindAll(delegate(StockOutOrderMaterials mm) { return mm != m; });
        //        removelist.ForEach(delegate(StockOutOrderMaterials mm)
        //        {
        //            son.Materials.Remove(mm);
        //        });
        //    }
        //    return son.Materials;
        //}
        #endregion

        #region 删除数据方法
        protected override void OnRemoveing(ControllerBase.SaveEvent item)
        {
            StockOutOrder order = item.Item as StockOutOrder;
            if (!order.IsSubmited && !order.IsReviewed)
            {
                base.OnRemoveing(item);
            }
            else
            {
                throw new Exception("异常：只有新建状态下的单据可以进行删除操作！");
            }

        }

        #endregion
        #endregion

        #region 业务处理自定义方法
        ///// <summary>
        ///// 自动获取发运通知号
        ///// </summary>
        ///// <returns></returns>
        //public string GetStockOutOrder()
        //{
        //    return "CKD" + WMShelp.GetStockOutOrderNo(this.model);
        //}

        ///// <summary>
        ///// 添加数据时的处理
        ///// </summary>
        ///// <param name="item"></param>
        //private void AddOutOrder(ControllerBase.SaveEvent item)
        //{
        //    StockOutOrder order = item.Item as StockOutOrder;
        //    ///判断当前登陆人是否有仓库的权限
        //    /// 
        //    EntityList<WareHouse> whList = new EntityList<WareHouse>(this.model.GetDataAction());
        //    whList.GetData("id='" + order.TOWHNO + "'");
        //    if (whList.Count > 0)
        //    {
        //        if (whList[0].ISOFFER)
        //        {
        //            ValidateWareHouse(order);
        //        }
        //    }
        //    StockOutNoticeMaterials som = new StockOutNoticeMaterials();
           
        //        for (int i = 0; i <  order.Materials.Count; i++)
        //        {
        //            var sm = order.Materials[i];
        //        if (sm.NUM <= 0)
        //        {
        //            throw new Exception("请输入正确的出库数量");
        //        }
        //        Materials a = sm.GetForeignObject<Materials>(this.model.GetDataAction());
        //        double tempNum = 0;
        //        order.Materials.ForEach(c =>
        //        {
        //            var group = order.Materials.Where(aa => aa.MATERIALCODE == c.MATERIALCODE);
        //            c.NUM = group.Sum(x => x.NUM);
        //            tempNum = c.NUM;
        //        });
        //        //if ((sm.NUM+sm.GetSourceNum("" + new StockOutNoticeMaterials().ToString() + "", sm.SourceRowID, sm.MATERIALCODE))) > Convert.ToDouble(sm.GetNum(new StockOutNoticeMaterials().ToString(), sm.SourceRowID, sm.MATERIALCODE))))
        //        if ((tempNum + sm.GetSourceNum("" + som.ToString() + "", sm.SourceRowID, sm.MATERIALCODE)) > Convert.ToDouble(sm.GetNum(new StockOutNoticeMaterials().ToString(), sm.SourceRowID, sm.MATERIALCODE)))
        //        {
        //            throw new Exception("行:" + sm.RowIndex + " 商品名称:" + a.FNAME + "  商品编码：" + a.Code + " 数量超过源单数量");
        //        }
        //        //order.Materials.RemoveAll(delegate(StockOutOrderMaterials m)
        //        //{
        //        //    return m.NUM <= 0;
        //        //});
               
        //        ///成品、原料出库判断下推目标中的数量方法
        //        /*
        //        if (ms.Count > 0)
        //        {
        //            if (ms[0].CommodityType == 2)
        //            {
        //                if (order.Materials[i].NUM > order.Materials[i].GetXTNum(order.Materials[i].MATERIALCODE, order.Materials[i].SourceRowID) - order.Materials[i].GetSourceNum(order.Materials[i].SourceController, order.Materials[i].SourceRowID, order.Materials[i].MATERIALCODE, order.STOCKTYPE, "入库") && order.Materials[i].GetXTNum(order.Materials[i].MATERIALCODE, order.Materials[i].SourceRowID) != 0)
        //                {
        //                    throw new Exception("商品名称:" + ms[0].FNAME + " 商品编码:" + ms[0].Code + "  数量超过源单数量");
        //                }
        //            }
        //            else
        //            {
        //                if (order.Materials[i].SourceRowID > 0)
        //                {
        //                    if (order.Materials[i].SourceTable == "Acc_Crm_DispatchNoteData")
        //                    {
        //                        if (order.Materials[i].NUM > order.Materials[i].GetNum(order.Materials[i].MATERIALCODE, order.Materials[i].SourceRowID) - order.Materials[i].GetSourceNum(order.Materials[i].SourceController, order.Materials[i].SourceRowID, order.Materials[i].MATERIALCODE, order.STOCKTYPE, "入库") && order.Materials[i].GetNum(order.Materials[i].MATERIALCODE, order.Materials[i].SourceRowID) != 0)
        //                        {
        //                            throw new Exception("商品名称:" + ms[0].FNAME + " 商品编码:" + ms[0].Code + "  数量超过源单数量");
        //                        }
        //                    }
        //                    else if (order.Materials[i].NUM > order.Materials[i].GetNum(order.Materials[i].MATERIALCODE, order.Materials[i].SourceRowID) - order.Materials[i].GetSourceNum(order.Materials[i].SourceController, order.Materials[i].SourceRowID, order.Materials[i].MATERIALCODE, order.STOCKTYPE, "入库") && order.Materials[i].GetNum(order.Materials[i].MATERIALCODE, order.Materials[i].SourceRowID) != 0)
        //                    {
        //                        throw new Exception("商品名称:" + ms[0].FNAME + " 商品编码:" + ms[0].Code + "  数量超过源单数量");
        //                    }
        //                }
        //                //else
        //                //{
        //                //    throw new Exception(ms[0].FNAME + " 不存在源单中，请核对");
        //                //}
        //            }
        //        }
        //         */ 

        //        //order.Materials.RemoveAll(delegate(StockOutOrderMaterials m)
        //        //{
        //        //    return m.NUM <= 0;
        //        //});

        //        ///修改出库单子级的orderNo为父级code的ID
        //        //order.Materials[i].PARENTID = order.ID;
        //        //order.Materials[i].SourceCode = order.SourceCode;
        //        //string strSql = " AA FROM dbo.Acc_WMS_StockInfo_Materials WHERE Code='' AND DEPOTWBS IN(";
        //        //strSql += "SELECT DEPOTWBS FROM dbo.Acc_WMS_OutOrderMaterials WHERE PARENTID NOT IN (SELECT ID FROM dbo.Acc_WMS_OutOrder  WHERE IsReviewed=0))  ORDER BY Creationdate ";
        //        //if (order.Materials[i].DEPOTWBS == "")
        //        //{
        //        //    if (ms.Count > 0)
        //        //    {
        //        //        if (ms[0].OUTWAREHOUSEYPE == 0)
        //        //            order.Materials[i].DEPOTWBS = "1";
        //        //        if (ms[0].OUTWAREHOUSEYPE == 1)
        //        //            order.Materials[i].DEPOTWBS = this.model.GetDataAction().GetValue("SELECT DEPOTWBS" + strSql).ToString();
        //        //        if (ms[0].OUTWAREHOUSEYPE == 2)
        //        //            order.Materials[i].DEPOTWBS = this.model.GetDataAction().GetValue("SELECT DEPOTWBS" + strSql + "DESC").ToString();
        //        //    }
        //        //}
        //        //if (order.Materials[i].PORTNAME == "")
        //        //    if (ms.Count > 0)
        //        //    {
        //        //        if (ms[0].OUTWAREHOUSEYPE == 0)
        //        //            order.Materials[i].PORTNAME = "1";
        //        //        if (ms[0].OUTWAREHOUSEYPE == 1)
        //        //            order.Materials[i].PORTNAME = this.model.GetDataAction().GetValue("SELECT PORTCODE" + strSql).ToString();
        //        //        if (ms[0].OUTWAREHOUSEYPE == 2)
        //        //            order.Materials[i].PORTNAME = this.model.GetDataAction().GetValue("SELECT PORTCODE" + strSql + "DESC").ToString();
        //        //    }
        //        ///验证子级数据的正确性
        //        //ValidateInfo(order.Materials[i], null);
        //    }
        //}

        ///// <summary>
        ///// 合并同行(出库产品明细行)
        ///// </summary>
        ///// <param name="item"></param>
        //public void MergerOrderMaterials(ControllerBase.SaveEvent item)
        //{
        //    StockOutOrder outOrder = item.Item as StockOutOrder;
        //    ///获取当前出库通知子级的所有数据的集合
        //    var list = outOrder.Materials;

        //    //两个要保存的list
        //    EntityList<StockOutOrder> sooList = new EntityList<StockOutOrder>(this.model.GetDataAction());
        //    if (list.Count > 0)
        //    {
        //        try
        //        {
        //            ///合并完成的结果
        //            var linqList = LinqHelp.MergerStockOutOrderMaterials(list);
        //            outOrder.Materials.Clear();
        //            foreach (var i in linqList)
        //            {
        //                outOrder.Materials.Add(i);
        //            }
        //            sooList.Add(outOrder);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw new Exception("异常" + ex.Message + "");
        //        }
        //    }
        //}

        ///// <summary>
        ///// 验证出库填写的数据的正确性
        ///// </summary>
        ///// <param name="sm">验证产品出库明细的数据</param>
        ///// <param name="son"></param>
        //private void ValidateInfo(StockOutOrderMaterials sm, StockOutNotice son)
        //{
        //    //判断出库明细数量不能小于0
        //    if (sm.NUM < 0)
        //    {
        //        throw new Exception("请输入正确的出库数量");
        //    }
        //    //Materials m = sm.GetForeignObject<Materials>(this.model.GetDataAction());
        //    ////判断产品是否允许负库存，也就是出库数量大于出库数量，如果允许则不判断库存是否满足条件
        //    //if (m.NUMSTATE == false)
        //    //{
        //    //    ///验证出库数量是否大于库存数量
        //    //    if (sm.NUM > GetMaterialNumByCode(m.Code))
        //    //    {
        //    //        throw new Exception("出库数量不能大于库存数量");
        //    //    }
        //    //}
        //    /////验证实际出库数量是否大于单据出库数量
        //    //if (sm.NUM > son.Materials[0].NUM)
        //    //{
        //    //    throw new Exception("实际出库数量大于通知据数量");
        //    //}
        //}

        ///// <summary>
        ///// 根据产品编码查询库存，作为出库不允许负数时候的判断条件
        ///// </summary>
        ///// <param name="materialCode">参数产品编码</param>
        ///// <returns>返回产品数量</returns>
        //private Decimal GetMaterialNumByCode(string materialCode)
        //{
        //    return WMShelp.GetMaterialNumByCode(materialCode, this.model);
        //}

        #endregion

        #region 添加按钮方法（出库流程方法）

        /// <summary>
        /// 重写提交
        /// </summary>
        /// <param name="info"></param>
        protected override void OnSubmitData(BasicInfo info)
        {
            if (!info.IsSubmited)
            {
                StockOutOrder order = info as StockOutOrder;
                order.Materials.DataAction = this.model.GetDataAction();
                order.Materials.GetData();
               
                if (order.Materials.Count > 0)
                {
                    base.OnSubmitData(order);
                 
                }
                else
                {
                    throw new Exception("异常：单据无明细数据，不能提交！");
                }
            }
            else
            {
                throw new Exception("异常：单据已提交，不能再次提交！");
            }
            //判断是否启用审核，如果没启用则直接出库
            if (!this.IsReviewedState)
            {
                OutWarehouse(info);
            }
        }

        
        /// <summary>
        /// 审核按钮
        /// </summary>
        /// <param name="info"></param>
        protected override void OnReviewedData(BasicInfo info)
        {
            if (!info.IsReviewed)
            {
                OutWarehouse(info);
                base.OnReviewedData(info);
            }
            else
            {
                throw new Exception("异常：单据已审核，不能再次审核！");
            }

        }

        ///// <summary>
        ///// 验证入库权限是否可以通过
        ///// </summary>
        ///// <param name="stockinorder"></param>
        //public virtual void ValidateWareHouse(StockOutOrder stockinorder)
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

        public void OutWarehouse(BasicInfo info)
        {
            IDataAction action = this.model.GetDataAction();
            EntityList<Difference> dfList = new EntityList<Difference>(action);
            dfList.GetData();
            foreach (var item in dfList)
            {
                if (item.IsSubmited == true && item.IsReviewed == false)
                {
                    throw new Exception("盘点差异单：" + item.Code + " 已提交未审核请稍后执行出库操作！");
                }
            }
             
             try
             {
                 action.StartTransation();
                 StockInfoMaterialsController smc = new StockInfoMaterialsController();
                 StockOutOrder order = info as StockOutOrder;
                 if (order!=null)
                 {
                     order.Materials.DataAction = action;
                     order.Materials.GetData();
                     var result = from o in order.Materials group o by new { o.MATERIALCODE,o.BATCHNO,o.DEPOTWBS,o.PORTNAME } into g select new { g.Key, Totle = g.Sum(p => p.NUM), Items = g.ToList<StockOutOrderMaterials>() };
                     List<StockOutOrderMaterials> list = new List<StockOutOrderMaterials>();
                     foreach (var ggg in result)
                         list.Add(ggg.Items[0]);
                     if (list.Count>0)
                     {
                         StockInfoMaterials infoList = new StockInfoMaterials();
                         EntityList<Materials> ms = new EntityList<Materials>(action);
                         InfoInSequence iis = null;
                         for (int i = 0; i < list.Count; i++)
                         {
                             ms.GetData("id='" + list[i].MATERIALCODE + "'");
                             if (ms.Count > 0)
                             {
                                 switch (ms[0].STATUS)
                                 {
                                     case "2": throw new Exception("产品名称：" + ms[0].FNAME + "  产品编码：" + ms[0].Code + "  状态为冻出，不可进行出库操作");
                                     case "3": throw new Exception("产品名称：" + ms[0].FNAME + "  产品编码：" + ms[0].Code + "  状态为不可用，不可进行出入库操作");
                                 }
                                 infoList.Code = ms[0].ID.ToString();
                                 if (order.TOWHNO == 0)
                                 {
                                     throw new Exception("请填写出库仓库");
                                 }
                                 else
                                 {
                                     infoList.WAREHOUSEID = order.TOWHNO;
                                 }

                                 if (list[i].DEPOTWBS == "")
                                 {
                                     list[i].DEPOTWBS = order.TOWHNO.ToString();
                                 }
                                 else
                                 {

                                     string depotwbsCode = infoList.GetAppendPropertyKey("DEPOTWBS");
                                     string depotwbsCode1 = list[i].GetAppendPropertyKey("DEPOTWBS");
                                     infoList.DEPOTWBS = list[i].DEPOTWBS;
                                     infoList[depotwbsCode] = list[i][depotwbsCode1];
                                 }
                                 if (list[i].PORTNAME != "")
                                 {
                                     string portNo = infoList.GetAppendPropertyKey("PORTCODE");
                                     string portNo1 = list[i].GetAppendPropertyKey("PORTNAME");
                                     infoList.PORTCODE = list[i].PORTNAME;
                                     infoList[portNo] = list[i][portNo1];
                                 }

                                 if (ms[0].BATCH == false)
                                 {
                                     list[i].BATCHNO = "";

                                 }
                                 else
                                 {
                                     infoList.BATCHNO = list[i].BATCHNO;
                                     if (list[i].BATCHNO == "")
                                     {
                                         throw new Exception("产品名称：" + ms[0].FNAME + "  产品编码：" + ms[0].Code + "必须允许批次管理，请输入录入批次");
                                     }
                                 }
                                 infoList.SENBATCHNO = list[i].SENBATCHNO == "" ? "" : list[i].SENBATCHNO;
                                 infoList.NUM = Convert.ToDouble(list[i].NUM);
                                 infoList.LASTINTIME = DateTime.Now;
                                 infoList.Remark = "最后操作：" + this.OnControllerName();
                                 if (this.OnControllerName() == "其他出库" || this.OnControllerName()=="生产出库")
                                 {
                                     infoList.STAY5 = "qt";
                                 }
                                 ///更改库存方法
                                 ///将出库单件码添加到出库对象中
                                 if (list[i].OutInSequence.Count <= 0)
                                 {
                                     infoList.StockSequence.Clear();
                                     list[i].OutInSequence.DataAction = action;
                                     list[i].OutInSequence.GetData();
                                     for (int j = 0; j < list[i].OutInSequence.Count; j++)
                                     {
                                         iis = new InfoInSequence();
                                         iis.SEQUENCECODE = list[i].OutInSequence[j].SEQUENCECODE;
                                         infoList.StockSequence.Add(iis);
                                     }
                                 }
                                 infoList.LASTOUTTIME = DateTime.Now;
                                 smc.putin(infoList, "出库", action);
                             }
                         }
                     }
                     else
                     {
                         throw new Exception("异常：单据无明细数据不能出库！");
                     }
                 }
                 else if (order.IsReviewed == true && order.IsSubmited == true)
                 {
                     throw new Exception("异常：已经完成审核！");
                 }
                 else
                 {
                     throw new Exception("异常：单据未提交不能执行审核操作！");
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

        #endregion

        ///// <summary>
        ///// 单据转换
        ///// </summary>
        ///// <param name="ca"></param>
        ///// <param name="actionItem"></param>
        ///// <returns></returns>
        //protected override EntityBase OnConvertItem(ControllerAssociate ca, EntityBase actionItem)
        //{
        //    //下推前判断
        //    WMShelp.IsPush(this);
        //    StockOutOrder cd = actionItem as StockOutOrder;
        //    cd.Materials.DataAction = this.model.GetDataAction();
        //    cd.Materials.GetData();//获取子集的数据
        //    EntityBase eb = base.OnConvertItem(ca, actionItem);
        //    ///下推其他入库
        //    //eb = PushInOrder(eb, cd);
        //    return eb;
        //}

        ///// <summary>
        ///// 出库下推判断
        ///// </summary>
        ///// <param name="eb"></param>
        ///// <param name="cd"></param>
        ///// <returns></returns>
        //protected virtual EntityBase PushInOrder(EntityBase eb, StockOutOrder cd)
        //{
        //    if (eb is StockInOrder)
        //    {
        //        StockInOrder ebin = eb as StockInOrder;
        //        ebin.SourceCode = cd.SourceCode;
        //        ebin.STATE = 1;
        //        ebin.STOCKTYPE = 3;
        //        ebin.IsSubmited = false;
        //        ebin.IsReviewed = false;
        //        ebin.SourceID = cd.ID;
        //        ebin.SourceCode = cd.Code;
        //        ebin.SourceController = this.ToString();
        //        ebin.Code = "";
        //        ebin.Submitedby = "";
        //        ebin.Submiteddate = DateTime.MinValue;
        //        ebin.Reviewedby = "";
        //        ebin.Revieweddate = DateTime.MinValue;
        //        ebin.Modifiedby = "";
        //        ebin.Modifieddate = DateTime.MinValue;
        //        foreach (StockInOrderMaterials m in ebin.Materials)
        //        {
        //            //m.NUM = m.NUM - m.GetSourceNum(m.SourceController, m.SourceRowID, m.MATERIALCODE, ebin.STOCKTYPE, "入库");
        //        }
        //        ebin.Materials.RemoveAll(delegate(StockInOrderMaterials m)
        //        {
        //            return m.NUM <= 0;
        //        });

        //        if (ebin.Materials.Count <= 0)
        //        {
        //            throw new Exception("下推完成不能重复下推");
        //        }
        //        eb = ebin;
        //    }
        //    return eb;
        //}


        ///// <summary>
        ///// 单据转换
        ///// </summary>
        ///// <returns></returns>
        //protected override ControllerAssociate[] DownAssociate()
        //{
        //    List<ControllerAssociate> list = new List<ControllerAssociate>();
        //    //入库单下推其他出库单
        //    OutOhtherOrder(list);
        //    return list.ToArray();
        //}
        ///// <summary>
        ///// 应用类中重写此方法
        ///// </summary>
        ///// <returns></returns>
        //protected virtual StockInOrderController GetDownOrder()
        //{
        //    return new StockInOrderController();
        //}
        ///// <summary>
        ///// 入库单-出库单(下推关联)
        ///// </summary>
        ///// <param name="list"></param>
        //private void OutOhtherOrder(List<ControllerAssociate> list)
        //{
        //    //控制器转换器(入库单-出库单)
        //    // ControllerAssociate bl = new ControllerAssociate(this, new StockElseInOrderController());
        //    ControllerAssociate bl = new ControllerAssociate(this, GetDownOrder());

        //    //单据属性映射
        //    PropertyMap mapORDERID = new PropertyMap();
        //    ///单号ID --来源单号
        //    mapORDERID.TargerProperty = "SourceCode";
        //    mapORDERID.SourceProperty = "Code";
        //    bl.Convert.AddPropertyMap(mapORDERID);


        //    //实体类型转换器（入库单-出库单）
        //    ConvertAssociate NoticeMaterialsToOrderMaterials = new ConvertAssociate();
        //    NoticeMaterialsToOrderMaterials.SourceType = typeof(StockOutOrderMaterials);//下推来源单据子集
        //    NoticeMaterialsToOrderMaterials.TargerType = typeof(StockInOrderMaterials);//下推目标单据子集
        //    YS(NoticeMaterialsToOrderMaterials);
        //    bl.AddConvert(NoticeMaterialsToOrderMaterials);
        //    list.Add(bl);
        //}

        ///// <summary>
        ///// 映射
        ///// </summary>
        ///// <param name="c"></param>
        //public void YS(ConvertAssociate c)
        //{
        //    PropertyMap mapMATERIALSID = new PropertyMap();//ID
        //    mapMATERIALSID.SourceProperty = "CODE";
        //    mapMATERIALSID.TargerProperty = "SourceCode";
        //    c.AddPropertyMap(mapMATERIALSID);

        //    PropertyMap map11 = new PropertyMap();//ID
        //    map11.SourceProperty = "p.ID";
        //    map11.TargerProperty = "SourceID";
        //    PropertyMap map12 = new PropertyMap();//ID
        //    map12.TargerProperty = "SourceController";
        //    map12.IsValue = true;
        //    map12.Value = this.ToString();

        //    PropertyMap map13 = new PropertyMap();//ID
        //    map13.SourceProperty = "ID";
        //    map13.TargerProperty = "SourceRowID";

        //    PropertyMap map14 = new PropertyMap();//ID
        //    map14.SourceProperty = "p.Code";
        //    map14.TargerProperty = "SourceCode";

        //    PropertyMap map15 = new PropertyMap();//ID
        //    map15.TargerProperty = "SourceName";
        //    map15.IsValue = true;
        //    map15.Value = ((IController)this).ControllerName();

        //    PropertyMap map16 = new PropertyMap();//ID
        //    map16.TargerProperty = "SourceTable";
        //    map16.IsValue = true;
        //    map16.Value = new StockOutOrderMaterials().ToString();

        //    c.AddPropertyMap(map11);
        //    c.AddPropertyMap(map12);
        //    c.AddPropertyMap(map13);
        //    c.AddPropertyMap(map14);
        //    c.AddPropertyMap(map15);
        //    c.AddPropertyMap(map16);
        //}

        //[ActionCommand(name = "打印出库产品条码", title = "打印出库产品条码", index = 6, icon = "icon-ok", onclick = "PrintCode", isselectrow = true)]
        //public void Prit()
        //{
        //    //生成界面方法按钮用于权限控制，本方法无代码
        //}

        [WhereParameter]
        public string orderid { get; set; }
        public string GetCode()
        {
            string id = "";
            StockOutOrder m = new StockOutOrder();// this.ActionItem as StockInOrder;
            EntityList<StockOutOrder> sio = new EntityList<StockOutOrder>(this.model.GetDataAction());
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
