using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Business.Controllers;
using Acc.Business.WMS.Model;
using Way.EAP.DataAccess.Entity;
using Acc.Contract.Data.ControllerData;
using Acc.Contract.MVC;
using System.Data;
using Acc.Contract.Data;

namespace Acc.Business.WMS.Controllers
{
    public class StockOutNoticeController : BusinessController
    {
        /// <summary>
        /// 描述：出库通知控制器
        /// 作者：柳强
        /// 创建日期:2013-02-19
        /// </summary>
        public StockOutNoticeController(IModel model) : base(model) { }
        public StockOutNoticeController() : base(new StockOutNotice()) {
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
        /// 因为出库单控制器重新绑定了PARENTID的外键，所以这里需要重新绑定一下
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="target"></param>
        void SendListForeignKeyHandler(EntityBase entity, Dictionary<string, EntityForeignKeyAttribute> target)
        {
            if (target.ContainsKey("PARENTID"))
            {
                target.Remove("PARENTID");
            }
            target.Add("PARENTID", new EntityForeignKeyAttribute(typeof(StockOutNotice), "ID"));
        }
         */
        #endregion


        #region 初始化数据方法
        #region 基础设置
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
                return true;
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

        //显示在菜单
        protected override string OnControllerName()
        {
            return "出库通知管理";
        }
        //菜单中url路径
        protected override string OnGetPath()
        {
            return "Views/WMS/StockOutOrder/StockOutNotice.htm";
        }
        //开发人
        protected override string OnGetAuthor()
        {
            return "柳强";
        }
        //说明
        protected override string OnGetControllerDescription()
        {
            return "出库通知管理";
        }
        #endregion

        /// <summary>
        /// 初始化显示与否
        /// </summary>
        /// <param name="data"></param>
        /// <param name="item"></param>
        protected override void OnInitViewChildItem(Contract.Data.ModelData data, Contract.Data.ItemData item)
        {
            if (data.name.EndsWith("StockOutNotice"))
            {
                switch (item.field.ToLower())
                {
                    case "submiteddate":
                    case "submitedby":
                    case "modifiedby":
                    case "modifieddate":
                    case "isdisable":
                    case "createdby":
                    case "creationdate":
                    case "isdelete":
                    case "id":
                    case "state":
                        item.visible = false;
                        item.isedit = false;
                        break;
                    case "isreviewed":
                    case "reviewedby":
                    case "revieweddate":
                        item.disabled = true;
                        item.visible = false;
                        break;
                    default:
                        item.visible = true;
                        break;
                }
            }
            if (data.name.EndsWith("StockOutNoticeMaterials"))
            {
                switch (item.field.ToLower())
                {
                    case "submiteddate":
                    case "submitedby":
                    case "modifiedby":
                    case "modifieddate":
                    case "reviewedby":
                    case "revieweddate":
                    case "issubmited":
                    case "isreviewed":
                    case "isdisable":
                    case "isdelete":
                    case "createdby":
                    case "creationdate":
                    case "sourceno":
                    case "id":
                    case "code":
                    case "orderno":
                    case "parentid":
                    case "staynum":
                        item.visible = false;
                        item.isedit = false;
                        break;
                    default:
                        item.visible = true;
                        break;
                    case "finishnum":
                        item.isedit = false;
                        break;
                }
                if (item.IsField("materialcode"))
                {
                    if (item.foreign != null)
                    {
                        //item.foreign.rowdisplay.Add("FMOBILE", "FMOBILE");//key是联系人表字段，value是客户表字段
                        item.foreign.rowdisplay.Add("FMODEL", "FMODEL");
                        item.foreign.rowdisplay.Add("CODE", "MCODE");
                    }
                }
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
                if (ac.command == "ReviewedData")
                {
                    ac.visible = false;
                }
                if (ac.command == "UnSubmitData")
                {
                    ac.visible = false;
                }
              
            }
            return coms;
        }

        ///// <summary>
        ///// 自动获取出库通知号
        ///// </summary>
        ///// <returns></returns>
        //public string GetStockOutNoticeOrder()
        //{
        //    return "TZD" + WMShelp.GetStockOutOrderNo(this.model);
        //}

        /// <summary>
        /// 选择产品时事件
        /// </summary>
        /// <param name="model"></param>
        /// <param name="item"></param>
        protected override void OnForeignLoading(IModel model, loadItem item)
        {
            if (this.fdata.filedname == "MATERIALCODE")
            {
                StockOutOrder order = model as StockOutOrder;
                item.rowsql = "select * from (" + item.rowsql + ") a where a.isdelete = 0";
            }
            base.OnForeignLoading(model, item);
        }
        #endregion

        #region 重写方法（添加、编辑）

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="item"></param>
        protected override void OnAdding(Contract.MVC.ControllerBase.SaveEvent item)
        {
            base.OnAdding(item);
            AddNotice(item);
            //Merger(item);
        }
      
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="item"></param>
        protected override void OnEditing(ControllerBase.SaveEvent item)
        {
            base.OnEditing(item);
            AddNotice(item);
            //MergeroOutNoticeMaterials(item);
            //Merger(item);
           
        }


        protected void volipush(StockInNotice sin)
        {
            if (sin.IsReviewed == false)
            {
                throw new Exception("异常:单据未审核，不能下推！");
            }
        }
        protected override EntityBase OnConvertItem(ControllerAssociate ca, EntityBase actionItem)
        {
            EntityBase eb = null;
            //StockOutNotice cd = actionItem as StockOutNotice;
            ////volipush(cd);
            //cd.Materials.DataAction = this.model.GetDataAction();
            //cd.Materials.GetData();//获取子集的数据
             eb = base.OnConvertItem(ca, actionItem) as StockOutOrder;
            if (eb == null) {
                 eb = base.OnConvertItem(ca, actionItem) as StockInNotice;
            }
            //if (eb != null)
            //{
            //    Dictionary<string, EntityForeignKeyAttribute> dic = ((IEntityBase)eb).GetForeignKey();
            //    EntityForeignKeyAttribute f = dic["ORDERNO"];
            //    string name = f.DisplayName() + "_ORDERNO";
            //    eb[name] = actionItem["ORDERNO"];
            //}
            return eb;
        }
        /// <summary>
        /// 单据转换
        /// </summary>
        /// <returns></returns>
        protected override ControllerAssociate[] DownAssociate()
        {
            List<ControllerAssociate> list = new List<ControllerAssociate>();

            //出库通知转换出库单
            this.InnoticeToInorderAssociate(list);

            return list.ToArray();
        }

        /// <summary>
        /// 合并出库通知
        /// </summary>
        /// <param name="item"></param>
        private static void Merger(ControllerBase.SaveEvent item)
        {
            StockOutNotice son = item.Item as StockOutNotice;
            var result = from o in son.Materials group o by new { o.MATERIALCODE } into g select new { g.Key, Totle = g.Sum(p => p.NUM), Items = g.ToList<StockOutNoticeMaterials>() };
            foreach (var group in result)
            {
                StockOutNoticeMaterials m = group.Items.Find(delegate(StockOutNoticeMaterials mm) { return ((IEntityBase)mm).StateBase == EntityState.Select; });
                if (m == null)
                    m = group.Items[0];
                m.NUM = group.Totle;
                List<StockOutNoticeMaterials> removelist = group.Items.FindAll(delegate(StockOutNoticeMaterials mm) { return mm != m; });
                removelist.ForEach(delegate(StockOutNoticeMaterials mm)
                {
                    son.Materials.Remove(mm);
                });
            }
        }
        #endregion

        #region 业务处理方法
        /// <summary>
        /// 出库库通知-出库单(下推关联)
        /// </summary>
        /// <param name="list"></param>
        private void InnoticeToInorderAssociate(List<ControllerAssociate> list)
        {
            //控制器转换器(入库通知-入库单)
            ControllerAssociate InnoticeToInorder = new ControllerAssociate(this, new StockOutOrderController());
            //单据属性映射
            PropertyMap mapORDERID = new PropertyMap();
            mapORDERID.TargerProperty = "OUTTIME";
            mapORDERID.SourceProperty = "SUBMITEDDATE";
            InnoticeToInorder.Convert.AddPropertyMap(mapORDERID);


            //实体类型转换器（入库通知明细-入库单明细）
            ConvertAssociate NoticeMaterialsToOrderMaterials = new ConvertAssociate();
            NoticeMaterialsToOrderMaterials.SourceType = typeof(StockOutNoticeMaterials);//下推来源单据子集
            NoticeMaterialsToOrderMaterials.TargerType = typeof(StockOutOrderMaterials);//下推目标单据子集
            PropertyMap mapMATERIALSID = new PropertyMap();//ID
            mapMATERIALSID.SourceProperty = "ID";
            mapMATERIALSID.TargerProperty = "SourceCode";

            PropertyMap mapFCPIDOLD = new PropertyMap();//产品代码(原)
            mapFCPIDOLD.SourceProperty = "MATERIALCODE";
            mapFCPIDOLD.TargerProperty = "MATERIALCODE";

            NoticeMaterialsToOrderMaterials.AddPropertyMap(mapMATERIALSID);
            NoticeMaterialsToOrderMaterials.AddPropertyMap(mapFCPIDOLD);
            InnoticeToInorder.AddConvert(NoticeMaterialsToOrderMaterials);

            list.Add(InnoticeToInorder);
        }

        /// <summary>
        /// 添加出库通知方法
        /// </summary>
        /// <param name="item"></param>
        private static void AddNotice(Contract.MVC.ControllerBase.SaveEvent item)
        {
            //明确类型时使用赋值
            StockOutNotice order = item.Item as StockOutNotice;
            if (order != null)
            {
                order.Materials.ForEach(delegate(StockOutNoticeMaterials mm) {
                  //  mm.SourceCode = order.TZORDERNO;
            });

            //子表关联引用赋值（本表id=子表父级id）
                //IEntityBase ieb = (IEntityBase)order;
                //IHierarchicalEntityView[] views = ieb.GetChildEntityList();
                //foreach (IHierarchicalEntityView v in views)
                //{
                //    if (v.ChildEntity is StockOutNoticeMaterials)
                //    {
                //        IEntityList list = v.GetEntityList();
                //        foreach (StockOutNoticeMaterials cd in list)
                //        {
                //            cd.SourceCode = order.ORDERNO;
                //        }
                //    }
                //}
            }
        }

        /// <summary>
        /// 合并同行
        /// </summary>
        /// <param name="item"></param>
        private void MergeroOutNoticeMaterials(ControllerBase.SaveEvent item)
        {
            StockOutNotice outNotice = item.Item as StockOutNotice;
            ///获取当前出库通知子级的所有数据的集合
            var list = outNotice.Materials;

            //两个要保存的list
            EntityList<StockOutNotice> sonList = new EntityList<StockOutNotice>(this.model.GetDataAction());
            if (list.Count > 0)
            {
                try
                {
                    ///合并完成的结果
                    var linqList = LinqHelp.MergerStockOutNoticeMaterials(list);
                    outNotice.Materials.Clear();
                    foreach (var i in linqList)
                    {
                        outNotice.Materials.Add(i);
                    }
                    sonList.Add(outNotice);
                }
                catch (Exception ex)
                {
                    throw new Exception("异常" + ex.Message + "");
                }

                sonList.Save();
            }
            
        }
        #endregion

    }
}
