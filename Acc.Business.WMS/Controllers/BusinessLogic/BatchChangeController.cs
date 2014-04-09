using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Business.Controllers;
using Acc.Business.WMS.Model;
using Way.EAP.DataAccess.Data;
using System.Data;
using Way.EAP.DataAccess.Entity;
using Acc.Contract.Data;
using Acc.Contract.MVC;
using Acc.Contract.Data.ControllerData;
using Acc.Business.Model;

namespace Acc.Business.WMS.Controllers
{
    public class BatchChangeController : BusinessController
    {
        /// <summary>
        /// 批次更改管理控制器
        /// 创建人：柳强
        /// 创建时间：2013-05-31
        /// </summary>
        public BatchChangeController() : base(new BatchChange()) { }

        public override bool IsReviewedState
        {
            get
            {
                return true;
            }
        }

        protected override string OnControllerName()
        {
            return "库存批次变更管理";
        }

        //菜单中url路径
        protected override string OnGetPath()
        {
            return "Views/WMS/Materials/BatchChange.htm";
        }   
        //开发人
        protected override string OnGetAuthor()
        {
            return "柳强";
        }
        //说明
        protected override string OnGetControllerDescription()
        {
            return "库存批次变更管理";
        }

        #region 初始化数据方法
        protected override void OnInitViewChildItem(Contract.Data.ModelData data, Contract.Data.ItemData item)
        {
            if (data.name.EndsWith("BatchChange"))
            {
                switch (item.field.ToLower())
                {
                    case "submiteddate":
                    case "submitedby":
                    case "createdby":
                    case "creationdate":
                    case "reviewedby":
                    case "revieweddate":
                    case "issubmited":
                    case "isreviewed":
                    case "isdisable":
                    case "isdelete":
                    case "code":
                        item.disabled = true;
                        item.visible = true;
                        break;
                    case "remark":
                        item.visible = true;
                        item.title = "修改原因";
                        break;
                }

            }
            if (data.name.EndsWith("StockSequence"))
            {
                data.visible = false;
            }
            if (data.name.EndsWith("BatchChangeMaterials"))
            {
                if (item.field == "NEWCODE")
                {
                    if (item.foreign != null)
                    {
                        item.foreign.rowdisplay.Add("Code", "NEWMNAME");
                        item.foreign.rowdisplay.Add("MCODE", "NEWMCODE");
                        item.foreign.rowdisplay.Add("FMODEL", "NEWFMODEL");
                        item.foreign.rowdisplay.Add("ISLOCK", "ISLOCK");
                        item.foreign.rowdisplay.Add("BATCHNO", "BATCHNO");
                        item.foreign.rowdisplay.Add("SENBATCHNO", "SENBATCHNO");
                        item.foreign.rowdisplay.Add("DEPOTWBS", "DEPOTWBS");
                        item.foreign.rowdisplay.Add("NUM", "NUM");
                        item.foreign.rowdisplay.Add("LASTINTIME", "LASTINTIME");
                        item.foreign.rowdisplay.Add("STATUS", "STATUS");
                        item.foreign.rowdisplay.Add("SALEAREA", "SALEAREA");
                        item.foreign.rowdisplay.Add("PORTCODE", "PORTCODE");
                        item.foreign.rowdisplay.Add("HOUSECODE", "HOUSECODE");
                        item.foreign.rowdisplay.Add("ORDERNO", "ORDERNO");
                        item.foreign.rowdisplay.Add("FREEZENUM", "FREEZENUM");
 
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
                    case "isdisable":
                    case "isdelete":
                    case "mcode":
                    case "fmodel":
                    case "code":
                    case "portcode":
                    case "housecode"://修改存储位置为可用，代替移位单功能
                    case "orderno":
                    case "salearea":
                    case "status":
                    case "lastouttime":
                    case "parentid":
                        item.visible = false;
                        break;
                    case "newcode":
                    case "newnum":
                    case "newbatchno":
                        item.disabled = false;
                        break;
                    default:
                        item.visible = true;
                        item.disabled = true;
                        break;
                }
                
            }
           
        }

        #endregion

        protected override void OnAdding(ControllerBase.SaveEvent item)
        {
            Merger(item);
            ValidateItem(item);
            base.OnAdding(item);
        }

        /// <summary>
        /// 验证数据正确性
        /// </summary>
        /// <param name="item"></param>
        private void ValidateItem(ControllerBase.SaveEvent item)
        {
            BatchChange bc = item.Item as BatchChange;
            foreach (var i in bc.ChildItems)
            {
                string name = i.GetForeignObject<Materials>(this.model.GetDataAction()).FNAME;
                if (i.NEWNUM == 0)
                {
                    throw new Exception("异常："+name+" 变更数量不能为空");
                }
                if (i.NUM < i.NEWNUM)
                {
                    throw new Exception("异常："+name+" 变更数量大于库存数量");
                }
                if (i.BATCHNO == i.NEWBATCHNO)
                {
                    throw new Exception("异常："+name+" 批次未变更");
                }
            }
            
        }

        /// <summary>
        /// 合并简写方法
        /// </summary>
        /// <param name="item"></param>
        private static void Merger(ControllerBase.SaveEvent item)
        {
            BatchChange son = item.Item as BatchChange;
            var result = from o in son.ChildItems group o by new { o.NEWCODE, o.BATCHNO, o.NEWBATCHNO, o.NEWFMODEL } into g select new { g.Key, Totle = g.Sum(p => p.NEWNUM), Items = g.ToList<BatchChangeMaterials>() };
            foreach (var group in result)
            {
                BatchChangeMaterials m = group.Items.Find(delegate(BatchChangeMaterials mm) { return ((IEntityBase)mm).StateBase == EntityState.Select; });
                if (m == null)
                    m = group.Items[0];
                m.NEWNUM = group.Totle;
                List<BatchChangeMaterials> removelist = group.Items.FindAll(delegate(BatchChangeMaterials mm) { return mm != m; });
                removelist.ForEach(delegate(BatchChangeMaterials mm)
                {
                    son.ChildItems.Remove(mm);
                });
            }
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="item"></param>
        protected override void OnEditing(Contract.MVC.ControllerBase.SaveEvent item)
        {
            Merger(item);
            ValidateItem(item);
            base.OnEditing(item);
        }

        protected override void OnReviewedData(Business.Model.BasicInfo info)
        {
            base.OnReviewedData(info);
        }

        /// <summary>
        /// 点击库存项（筛选：库存大于0 且库存项有批次）
        /// </summary>
        /// <param name="model"></param>
        /// <param name="item"></param>
        protected override void OnForeignLoading(IModel model, loadItem item)
        {
            if (this.fdata.filedname == "NEWCODE")
            {
                item.rowsql = "select * from ("+item.rowsql+") a where a.num >0.0000 and a.batchno != ''";
            }
            base.OnForeignLoading(model, item);
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
                if (ac.command == "UnReviewedData")
                {
                    ac.visible = false;
                }

            }
            return coms;
        }
    }
}
