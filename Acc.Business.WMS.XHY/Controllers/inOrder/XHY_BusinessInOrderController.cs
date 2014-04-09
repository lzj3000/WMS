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
using Way.EAP.DataAccess.Data;
using System.Data;

namespace Acc.Business.WMS.XHY.Controllers
{
    public class XHY_BusinessInOrderController : StockInOrderController
    {
        public XHY_BusinessInOrderController() : base(new StockInOrder()) { }
        public XHY_BusinessInOrderController(IModel model) : base(model) { }
        /// <summary>
        /// 描述：新华杨基础入库控制器
        /// 作者：柳强
        /// 创建日期:2013-10-14
        /// </summary>

        #region 基础设置

        //是否启用  提交
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

        /// <summary>
        /// 是否启用下推
        /// </summary>
        public override bool IsPushDown
        {
            get
            {
                return false;
            }
        }

        public override bool IsReviewedState
        {
            get {
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
      
        //开发人
        protected override string OnGetAuthor()
        {
            return "柳强";
        }

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
                switch (item.field.ToLower())
                {
                    case "code":
                        item.title = "编码";
                        item.disabled = true;
                        break;
                    case "towhno":
                        item.required = true;
                        break;
                    case "rowindex":
                        item.visible = true;
                        item.disabled = true;
                        break;
                    case "isdisable":
                    case "finishtime":
                    case "inouttime":
                    case "stay1":
                    case "stay2":
                    case "stay3":
                    case "stay4":
                    case "stay5":
                        item.visible = false;
                        break;
                    case "createdby":
                    case "creationdate":
                    case "modifiedby":
                    case "modifieddate":
                    case "submiteddate":
                    case "submitedby":
                    case "issubmited":
                        item.disabled = true;
                        break;

                }
            }
            if (data.name.EndsWith("StockInOrderMaterials"))
            {
               
                switch (item.field.ToLower())
                {
                    case "rowindex":
                        item.visible = true;
                        item.disabled = true;
                        break;
                    case "materialcode":
                        item.visible = true;
                        item.title = "物料名称";
                        break;
                    case "mcode":
                        item.visible = true;
                        item.title = "物料编码";
                        item.disabled = true;
                        break;
                    case "fmodel":
                        item.visible = true;
                        item.title = "物料规格";
                        item.disabled = true;
                        break;
                    case "num":
                    case "batchno":
                        item.required = true;
                        item.visible = true;
                        break;
                    case "remark":
                        item.visible = true;
                        break;
                    case "code":
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
                        break;
                    case "portname":
                        item.title = "托盘编码";
                        item.required = true;
                        break;
                    default :
                        item.visible = false;
                        break;
                }
            }
            if (data.name.EndsWith("StockInNoticeMaterials"))
            {
                switch (item.field.ToLower())
                {
                    case "rowindex":
                    case "num":
                    case "batchno":
                        item.visible = true;
                        break;
                    case "finishnum":
                        item.title = "已操作数";
                        item.visible = true;
                        break;
                    case "staynum":
                        item.title = "待操作数";
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
                    default :
                        item.visible = false;
                        break;
                }
            }
            if (data.name.EndsWith("StockInNoticeMaterials1"))
            {
                data.visible = false;
            }
        }

        

        #endregion


        #region 重写方法

       protected override void OnForeignIniting(IModel model, InitData data)
       {
           if (model is StockInOrderMaterials)
           {
               if (this.fdata.filedname == "MATERIALCODE")
               {
                   StockInNoticeMaterials m = new StockInNoticeMaterials();
                   
                   foreach (ItemData d in data.modeldata.childitem)
                   {
                       d.visible = false;
                       if (d.IsField("FMODEL"))//Code
                       {
                           
                           d.visible = true;
                           //d.title = "FMODEL";
                       } if (d.IsField("FNAME"))//Code
                       {
                           d.visible = true;
                           d.title = "商品名称";
                       }
                       if (d.IsField("Code"))//Code
                       {
                           d.visible = true;
                           d.title = "商品编码";
                       }
                       if (d.IsField("STAY1"))//Code
                       {
                           d.visible = true;
                           d.title = "待入库数量";
                       }
                       if (d.IsField("STAY2"))//Code
                       {
                           d.visible = true;
                           d.title = "批次";
                       }
                   }
               }
               //字段序号、托盘编码PORTNO、PORTNAME、托盘状态STATUS、是否禁用IsDisable、备注Remark  
                if (this.fdata.filedname == "PORTNAME")
                {
                    foreach (ItemData d in data.modeldata.childitem)
                    {
                        d.visible = false;
                        if (d.IsField("ROWINDEX"))
                        {
                            d.visible = true;
                        }
                        if (d.IsField("PORTNO"))
                            d.visible = true;
                        if (d.IsField("STATUS"))
                            d.visible = true;
                        if (d.IsField("IsDisable"))
                            d.visible = true;
                        if (d.IsField("Remark"))
                            d.visible = true;
                    }
                    return;
                }
               //序号、代码Code、上级PARENTID、类型WHTYPE、是否禁用IsDisable、备注Remark
                if (this.fdata.filedname == "DEPOTWBS")
                {
                    foreach (ItemData d in data.modeldata.childitem)
                    {
                        d.visible = false;
                        if (d.IsField("ROWINDEX"))
                        {
                            d.visible = true;
                        }
                        if (d.IsField("CODE"))
                        {
                            d.visible = true;
                            d.title = "货位编码";
                        }
                        if (d.IsField("PARENTID"))
                            d.visible = true;
                        if (d.IsField("WHTYPE"))
                            d.visible = true;
                        if (d.IsField("IsDisable"))
                            d.visible = true;
                        if (d.IsField("Remark"))
                            d.visible = true;
                    }
                    return;
                }
           }
           if (model is StockInOrder)
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
               if (this.fdata.filedname == "STAY5")
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
                           d.title = "车间编码";
                       }
                       if (d.IsField("OrganizationName"))
                           d.visible = true;
                       if (d.IsField("ParentID"))
                           d.visible = true;
                       if (d.IsField("IsDisable"))
                           d.visible = true;
                       if (d.IsField("Remark"))
                           d.visible = true;
                   }
                   return;
               }
               }
       }
       protected virtual void foreignInOrder(IModel model, loadItem item)
       {
           if (this.fdata.filedname == "TOWHNO")
           {
               item.rowsql = "select * from  (" + item.rowsql + ")  a where isdisable =0 and WHTYPE=0";
           }
           ///选择客户不包括已禁用的
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
       protected virtual void foreignInOrderMaterials(IModel model, loadItem item)
       {
           string W = "";
           //foreach (SQLWhere w in item.whereList)
           //    W += w.where("and   ");
           if (item.whereList.Length > 0)
           {
               string file = item.whereList[0].ColumnName;
               string symol = item.whereList[0].Symbol;
               string value = item.whereList[0].Value;
               file = file.Substring(file.IndexOf('.'));
               if (file.Equals(".STAY1"))
               {
                   symol = "=";
                   value = value.Substring(2, value.Length - 4);
               }
               switch (file)
               {
                   case ".FNAME": file = "c" + file; break;
                   case ".FMODEL": file = "c" + file; break;
                   case ".CODE": file = "c" + file; break;
                   case ".STAY1": file = "b." + "NUM"; break;
                   case ".STAY2": file = "b." + "BATCHNO"; break;
               }
               W = "and  " + file + "  " + symol + "   " + value;
           }
           ///选择可用的产品
           if (this.ActionItem == null)
           {
               base.OnForeignLoading(model, item);
           }
           else
           {
               if (this.fdata.filedname == "MATERIALCODE" && this.ActionItem["SourceCode"] != null)
               {
                   if (this.ActionItem["SourceCode"].ToString() == "")
                   {
                       throw new Exception("请先检查通知单是否选择");
                   }

                   item.rowsql = string.Format("select c.ID,c.FNAME,c.FMODEL,c.Code,b.NUM STAY1,b.BATCHNO STAY2 from {0} a left join {1} b on a.ID=b.PARENTID left join {2} c on b.MATERIALCODE=c.ID where a.Code='{3}'" + W + "", new StockInNotice().ToString(), new StockInNoticeMaterials().ToString(), this.fdata.tablename, this.ActionItem["SourceCode"]);
               }
               if (this.fdata.filedname == "MATERIALCODE" && this.ActionItem["SourceCode"] == null)
               {
                   base.OnForeignLoading(model, item);
               }
               //
               if (this.fdata.filedname == "PORTNAME")
               {
                   if (this.ActionItem["TOWHNO"].ToString() == "0")
                   {
                       throw new Exception("请先检查仓库是否选择");
                   }
                   else
                   {
                       item.rowsql = "select *  from  (" + item.rowsql + ")  p where p.IsDisable =0 and  p.STATUS=0";
                   }
               }
               if (this.fdata.filedname == "DEPOTWBS")
               {
                   if (this.ActionItem["TOWHNO"].ToString() == "0")
                   {
                       throw new Exception("请单击上一步检查仓库是否选择");
                   }
                   else
                   {
                       //string W="";
                       //foreach (SQLWhere w in item.whereList)
                       //    W += w.where("a");
                       item.rowsql = "select * from (" + item.rowsql + ") a where WHTYPE=2 and a.STATUS=0 and a.PARENTID='" + this.ActionItem["TOWHNO"] + "'";
                   }
               }
           }
       }
       protected override void OnForeignLoading(IModel model, loadItem item)
       {
          
               if (model is StockInOrder)
               {
                   foreignInOrder(model, item);
               }
               if (model is StockInOrderMaterials)
               {
                   foreignInOrderMaterials(model, item);
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
            if (m is StockInOrder)
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

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="item"></param>
        protected override void OnAdding(ControllerBase.SaveEvent item)
        {
            XHY_Add(item);
            base.OnAdding(item);
            StockInOrder sio = item.Item as StockInOrder;
            ///设置明细行创建时间
            for (int i = 0; i < sio.Materials.Count; i++)
            {
                sio.Materials[i].Creationdate = DateTime.Now;
            }
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="item"></param>
        protected override void OnEditing(ControllerBase.SaveEvent item)
        {
            StockInOrder sio = item.Item as StockInOrder;
            if(sio.IsSubmited== true)
            {
                throw new Exception("异常：单据已提交不能编辑！");
            }
            XHY_Edit(item);
            base.OnEditing(item);
            ///设置明细行更改时间
            for (int i = 0; i < sio.Materials.Count; i++)
            {
                sio.Materials[i].Modifieddate = DateTime.Now;
            }
        }

        protected override void OnSubmitData(BasicInfo info)
        {
            StockInOrder sio = info as StockInOrder;
            if (sio.IsSubmited == true)
            {
                throw new Exception("异常：单据已提交不能重复提交！");
            }
            sio.Materials.DataAction = this.model.GetDataAction();
            sio.Materials.GetData();
            if (sio.Materials.Count == 0)
            {
                throw new Exception("单据无明细数据不能提交！");
            }
            //for (int i = 0; i < sio.Materials.Count; i++)
            //{
            //    if (sio.Materials[i].DEPOTWBS == "")
            //    {
            //        throw new Exception("序号：" + sio.Materials[i].RowIndex + "  产品：" + sio.Materials[i].MCODE + "单据无货位数据不能提交！");
            //    }

            //    //if (sio.Materials[i].PORTNAME == "")
            //    //{
            //    //    throw new Exception("序号：" + sio.Materials[i].RowIndex + " 产品："+sio.Materials[i].MCODE+" 单据无托盘数据不能提交！");
            //    //}

            //}

            base.OnSubmitData(info);
        }

        protected override void OnRemoveing(ControllerBase.SaveEvent item)
        {
            StockInOrder sio = item.Item as StockInOrder;
            if (sio.Createdby == this.user.ID)
            {
                base.OnRemoveing(item);
            }
            else
            {
                throw new Exception("单据创建人与当前登陆人不符，不能删除！！");
            }
        }
        #endregion

        public virtual void XHY_Add(ControllerBase.SaveEvent item)
        {
            AddDefault(item);
            ValidateAddInNum(item);
            ValidateBatchno(item);
            ValidateDepotwbs(item);
            ///验证通过调用设置值方法
            StockInOrder sio = item.Item as StockInOrder;
            ///设置单据类型
            sio.STOCKTYPE = SetStockType();
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
            ValidateEditInNum(item);
            ValidateBatchno(item);
            ValidateDepotwbs(item);
            ValidatePort(item);
            ValidateWarehouse(item);
            ///验证通过调用设置值方法
            StockInOrder sio = item.Item as StockInOrder;
            ///重新设置明细行创建时间
            for (int i = 0; i < sio.Materials.Count; i++)
            {
                sio.Materials[i].Modifieddate = DateTime.Now;
                sio.Materials[i].Modifiedby = this.user.ID;
            }
           
        }

        /// <summary>
        /// （新增）验证数量的方法
        /// </summary>
        /// <param name="item"></param>
        public virtual void ValidateAddInNum(ControllerBase.SaveEvent item)
        {
            StockInOrder stockinorder = item.Item as StockInOrder;
            for (int i = 0; i < stockinorder.Materials.Count; i++)
            {
                ValidateInNumIsTrue(stockinorder, i);
            }
            ValidateSourceNum(stockinorder);
        }

        /// <summary>
        /// （编辑）验证数量的方法
        /// </summary>
        /// <param name="item"></param>
        public virtual void ValidateEditInNum(ControllerBase.SaveEvent item)
        {
            StockInOrder stockinorder = item.Item as StockInOrder;
            for (int i = 0; i < stockinorder.Materials.Count; i++)
            {
                ValidateInNumIsTrue(stockinorder, i);
            }
            ValidateSourceNum(stockinorder);
        }

        /// <summary>
        /// 验证输入的数量是否正确
        /// </summary>
        /// <param name="stockinorder"></param>
        /// <param name="i"></param>
        public virtual void ValidateInNumIsTrue(StockInOrder stockinorder, int i)
        {
            if (stockinorder.Materials[i].NUM <= 0)
            {
                throw new Exception("序号：" + stockinorder.Materials[i].RowIndex + " 商品编码" + stockinorder.Materials[i].MCODE + " 请输入正确的数量");
            }
        }

        /// <summary>
        /// 验证源单数量是否正确
        /// </summary>
        /// <param name="stockinorder"></param>
        /// <param name="i"></param>
        public virtual void ValidateSourceNum(StockInOrder stockinorder)
        {
            var result = from o in stockinorder.Materials group o by new { o.SourceRowID } into g select new { g.Key, Totle = g.Sum(p => p.NUM), Items = g.ToList<StockInOrderMaterials>() };
            foreach (var group in result)
            {
                StockInOrderMaterials m = group.Items.Find(delegate(StockInOrderMaterials mm) { return ((IEntityBase)mm).StateBase == EntityState.Select; });
                if (m == null)
                    m = group.Items[0];

                if (group.Totle <= 0)
                {
                    throw new Exception("明细行数量错误");
                }
                if (m.SourceRowID > 0)
                {
                    Materials a = m.GetForeignObject<Materials>(this.model.GetDataAction());
                    double snum = m.GetNum(m.SourceTable, m.SourceRowID, m.MATERIALCODE);//获取总共下推的数量
                    double bcnum = m.GetSourceNum(m.SourceTable, m.SourceRowID, m.MATERIALCODE, stockinorder.ID);//获取已经完成的数量
                    //m.NUM当前页面数量的总和（相同行id）
                    if (group.Totle > snum)
                        throw new Exception("序号:" + m.RowIndex + " 商品名称:" + a.FNAME + "  商品编码：" + a.Code + " 数量超过源单数量");
                    if (group.Totle > snum- bcnum)
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
            StockInOrder stockinorder = item.Item as StockInOrder;
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
            StockInOrder so = item.Item as StockInOrder;
            for (int i = 0; i < so.Materials.Count; i++)
            {
                WareHouse wh = so.Materials[i].GetForeignObject<WareHouse>(this.model.GetDataAction());
                if (wh != null)
                {
                    if (wh.PARENTID != so.TOWHNO)
                    {
                        throw new Exception("序号:" + so.Materials[i].RowIndex + "货位不属于主单仓库中!");
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
            StockInOrder so = item.Item as StockInOrder;
            if (so.STOCKTYPE != new SemInOrderController().SetStockType() && so.STOCKTYPE != new PurchaseInOrderController().SetStockType() && so.STOCKTYPE != new OtherInOrderController().SetStockType())
            {
                for (int i = 0; i < so.Materials.Count; i++)
                {
                    // Ports pt = so.Materials[i].GetForeignObject<Ports>(this.model.GetDataAction());

                    if (so.Materials[i].PORTNAME == "")
                    {
                        throw new Exception("序号:" + so.Materials[i].RowIndex + "托盘不能为空！");
                    }
                }
            }
        }


        /// <summary>
        /// 验证仓库的方法（是否有权限录入到此仓库和不能为空）
        /// </summary>
        /// <param name="item"></param>
        public virtual void ValidateWarehouse(ControllerBase.SaveEvent item)
        {
            StockInOrder sin = item.Item as StockInOrder;
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

            #region 不要
            //EntityList<OWorker> ow = new EntityList<OWorker>(this.model.GetDataAction());
            //ow.GetData();
            //if (ow.Count > 0)
            //{
            //    int res = 0;
            //    ow.GetData("workName='" + this.user.ID + "'");
            //    if (ow.Count > 0)
            //    {
            //        for (int i = 0; i < ow.Count; i++)
            //        {
            //            EntityList<WareHouse> wh = new EntityList<WareHouse>(this.model.GetDataAction());
            //            wh.GetData("(type=" + ow[i].WHID + " or id=" + ow[i].WHID + ") and ISOFFER=1");
            //            for (int j = 0; j < wh.Count; j++)
            //            {
            //                if (Convert.ToInt32(sin.TOWHNO) == wh[j].ID)
            //                {
            //                    res = 1;
            //                }
            //            }
            //        }
            //        if (res == 0)
            //        {
            //            throw new Exception("异常：没有入库到此仓库的权限");
            //        }
            //    }
            //    else
            //    {
            //        throw new Exception("异常：没有入库到此仓库的权限");
            //    }
            //}
            #endregion
        }
        //public virtual string GetNoticeStayGrid()
        //{
        //    return "";
        //}
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
        /// 子类中添加源单控制器，重写此方法添加不同名称
        /// </summary>
        /// <returns></returns>
        public virtual string InOrderController()
        {
            return "";
        }

        /// <summary>
        /// 添加页面默认值（主表：SourceController、SourceID、SourceCode）、（子表：SourceRowID、SourceTable）
        /// </summary>
        /// <param name="item"></param>
        public virtual void AddDefault(ControllerBase.SaveEvent item)
        {
            StockInOrder ord = item.Item as StockInOrder;
            StockInNoticeMaterials sm = new StockInNoticeMaterials();
            if (ord.SourceCode != null)
            {
                StockInNotice notice= ord.GetForeignObject<StockInNotice>(this.model.GetDataAction());
                ///如果目标单据的sourceId =0 代表没有使用下推方法，则赋值SourceID和SourceController
                if (notice!=null)
                {
                    ord.SourceID = notice.ID;
                    ord.SourceController = InOrderController();
                    notice.Materials.DataAction = this.model.GetDataAction();
                    notice.Materials.GetData();
                    for (int i = 0; i < notice.Materials.Count; i++)
                    {
                        for (int j = 0; j < ord.Materials.Count; j++)
                        {
                            if (!string.IsNullOrEmpty(notice.Materials[i].BATCHNO.Trim()))
                            {
                                if (notice.Materials[i].MATERIALCODE.Trim() == ord.Materials[j].MATERIALCODE.Trim() && notice.Materials[i].BATCHNO.Trim() == ord.Materials[j].BATCHNO.Trim())
                                {
                                    ord.Materials[j].SourceRowID = notice.Materials[i].ID;
                                    ord.Materials[j].SourceTable = sm.ToString();
                                }
                            }
                            else
                            {
                                if (notice.Materials[i].MATERIALCODE.Trim() == ord.Materials[j].MATERIALCODE.Trim())
                                {

                                    ord.Materials[j].SourceRowID = notice.Materials[i].ID;
                                    ord.Materials[j].SourceTable = sm.ToString();
                                }
                            }

                        }
                    }
                }
            }
        }

        /// <summary>
        /// 添加时根据源单sourcecode 得到源单明细数据
        /// </summary>
        [WhereParameter]
        public string NoticeId { get; set; }
        public virtual string GetNoticeStayGrid()
        {
            EntityList<StockInNotice> List = new EntityList<StockInNotice>(this.model.GetDataAction());
            List.GetData("Code='" + NoticeId + "'");
            EntityList<StockInNoticeMaterials> List1 = new EntityList<StockInNoticeMaterials>(this.model.GetDataAction());
            List1.GetData("PARENTID='" + List[0].ID + "'");
            string str = JSON.Serializer(List1.ToArray());
            return str;
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

        protected virtual void CheckTransformData()
        {
            StockInOrder mi = this.ActionItem as StockInOrder;
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
