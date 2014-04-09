using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Business.Controllers;
using Acc.Business.WMS.Model;
using Way.EAP.DataAccess.Entity;
using Acc.Contract.Data.ControllerData;
using Acc.Contract.MVC;
using Acc.Contract.Center;
using Acc.Contract.Data;

namespace Acc.Business.WMS.Controllers
{
    public class StockInNoticeController : BusinessController
    {
        /// <summary>
        /// 描述：入库通知控制器
        /// 作者：路聪
        /// 创建日期:2013-01-17
        /// </summary>
        public StockInNoticeController() : base(new StockInNotice()) {
            /*
             //重新绑定外键，暂时不要
            IEntityBase ieb = (IEntityBase)this.model;
            IHierarchicalEntityView[] view = ieb.GetChildEntityList();
            for (int i = 0; i < view.Length; i++)
            {
                if (view[i].ChildEntity.ToString().Equals("Acc_WMS_InNoticeMaterials", StringComparison.OrdinalIgnoreCase))
                {
                    ((IEntityBase)view[i].ChildEntity).ForeignKey += new ForeignKeyHandler(SendListForeignKeyHandler);
                }
            }
             */ 
        }
        public StockInNoticeController(IModel model) : base(model) { }
        #region 取消外键,并重新绑定
        /*
        /// <summary>
        /// 因为入库单控制器重新绑定了PARENTID的外键，所以这里需要重新绑定一下
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="target"></param>
        void SendListForeignKeyHandler(EntityBase entity, Dictionary<string, EntityForeignKeyAttribute> target)
        {
            if (target.ContainsKey("PARENTID"))
            {
                target.Remove("PARENTID");
            }
            target.Add("PARENTID", new EntityForeignKeyAttribute(typeof(StockInNotice), "ID"));
        }
         */ 
        #endregion



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

        //显示在菜单
        protected override string OnControllerName()
        {
            return "入库通知管理";
        }
        //菜单中url路径
        protected override string OnGetPath()
        {
            return "Views/WMS/StockInOrder/StockInNotice.htm";
        }
        //开发人
        protected override string OnGetAuthor()
        {
            return "路聪";
        }
        //说明
        protected override string OnGetControllerDescription()
        {
            return "入库通知管理";
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

        #region 公共方法
        /// <summary>
        /// 自动获取入库通知号
        /// </summary>
        /// <returns></returns>
        public string GetStockInNoticeOrder()
        {
            return WMShelp.GetInOrderNo("LSD",this.model);
        }
        #endregion

        #region 初始化数据方法
        protected override void OnInitViewChildItem(Contract.Data.ModelData data, Contract.Data.ItemData item)
        {
            if (data.name.EndsWith("StockInNotice"))
            {
                switch (item.field.ToLower())
                {
                    case "modifiedby":
                    case "modifieddate":
                    case "isdelete":
                    case "id":
                    case "rowindex":
                    case "state":
                    case "ordertype":
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
            if (data.name.EndsWith("StockInNoticeMaterials"))
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
                    case "isdelete":
                    case "id":
                    case "parentid":
                    case "staynum":
                    case "orderid":
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

        #endregion

    
        #region 添加数据方法
        protected override void OnAdding(Contract.MVC.ControllerBase.SaveEvent item)
        {
            base.OnAdding(item);
            //MergerOrderMaterials(item);
            //new SystemEmailController().Send("mr_liuqiang@163.com","123");
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="item"></param>
        protected override void OnEditing(ControllerBase.SaveEvent item)
        {
            base.OnEditing(item);
            StockInNotice sin = item.Item as StockInNotice;
            if (!sin.IsSubmited && !sin.IsReviewed)
            {
                ///调用添加的方法
               // Merger(item);
            }
            else
            {
                throw new Exception("异常：只有新建状态下的单据可以进行编辑操作！");
            }
        }

      

        /// <summary>
        /// 合并出库通知（仅限编辑）
        /// </summary>
        /// <param name="item"></param>
        public static void Merger(ControllerBase.SaveEvent item)
        {
            StockInNotice son = item.Item as StockInNotice;
            var result = from o in son.Materials group o by new { o.MATERIALCODE } into g select new { g.Key, Totle = g.Sum(p => p.NUM), Items = g.ToList<StockInNoticeMaterials>() };
            foreach (var group in result)
            {
                StockInNoticeMaterials m = group.Items.Find(delegate(StockInNoticeMaterials mm) { return ((IEntityBase)mm).StateBase == EntityState.Select; });
                if (m == null)
                    m = group.Items[0];
                m.NUM = group.Totle;
                if (m.NUM <= 0)
                {
                    throw new Exception("明细行数量错误");
                }
                List<StockInNoticeMaterials> removelist = group.Items.FindAll(delegate(StockInNoticeMaterials mm) { return mm != m; });
                removelist.ForEach(delegate(StockInNoticeMaterials mm)
                {
                    son.Materials.Remove(mm);
                });
            }
        }

        /// <summary>
        /// 合并同行（仅限添加）
        /// </summary>
        /// <param name="item"></param>
        public void MergerOrderMaterials(ControllerBase.SaveEvent item)
        {
            StockInNotice inOrder = item.Item as StockInNotice;
            if (inOrder != null)
            {
                ///获取当前出库通知子级的所有数据的集合
                var list = inOrder.Materials;

                //两个要保存的list
                EntityList<StockInNotice> sooList = new EntityList<StockInNotice>(this.model.GetDataAction());
                if (list.Count > 0)
                {
                    try
                    {
                        ///合并完成的结果
                        var linqList = LinqHelp.MergerStockNoticeOrderMaterials(list);
                        inOrder.Materials.Clear();
                        foreach (var i in linqList)
                        {
                            if (i.NUM > 0)
                            {
                                inOrder.Materials.Add(i);
                            }
                            else
                            {
                                throw new Exception("明细行数量错误");
                            }
                        }
                        sooList.Add(inOrder);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("异常" + ex.Message + "");
                    }
                }
            }
        }
        #endregion

        /// <summary>
        /// 选择事件
        /// </summary>
        /// <param name="model"></param>
        /// <param name="item"></param>
        protected override void OnForeignLoading(IModel model, loadItem item)
        {
            if (this.fdata.filedname == "MATERIALCODE")
            {
                item.rowsql = "select * from (" + item.rowsql + ") a where a.IsDelete='0' ";
            }
            base.OnForeignLoading(model, item);
        }
    }
}
