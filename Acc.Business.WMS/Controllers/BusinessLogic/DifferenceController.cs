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
    public class DifferenceController : BusinessController
    {
        /// <summary>
        /// 盘点差异控制器
        /// 创建人：柳强
        /// 创建时间：2013-06-07
        /// </summary>
        public DifferenceController() : base(new Difference()) { }
        public DifferenceController(IModel model) : base(model) { }

        public override bool IsReviewedState
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

        public override bool IsClearAway
        {
            get
            {
                return false;
            }
        }

        protected override string OnControllerName()
        {
            return "盘点差异管理";
        }

        //菜单中url路径
        protected override string OnGetPath()
        {
            return "Views/WMS/Check/Difference.htm";
        }   
        //开发人
        protected override string OnGetAuthor()
        {
            return "柳强";
        }
        //说明
        protected override string OnGetControllerDescription()
        {
            return "盘点差异管理";
        }

        public override bool IsSubmit
        {
            get
            {
                return false;
            }
        }

        #region 初始化数据方法
        protected override void OnInitViewChildItem(Contract.Data.ModelData data, Contract.Data.ItemData item)
        {
            if (data.name.EndsWith("Difference"))
            {
                switch (item.field.ToLower())
                {
                    case "submiteddate":
                    case "submitedby":
                    //case "createdby":
                    //case "creationdate":
                    case "reviewedby":
                    case "revieweddate":
                    case "isdisable":
                    case "isdelete":
                    case "parentid":
                    case "issubmited":
                    case "isreviewed":
                    case "code":
                        item.disabled = true;
                        item.visible = false;
                        break;
                    case "sourcecode":
                        item.title = "盘点单号";
                        item.visible = true;
                        break;

                }

            }

            if (data.name.EndsWith("DifferenceMaterials"))
            {
                data.disabled = true;
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
                    case "issubmited":
                    case "isreviewed":
                    case "sourcecode":
                    case "code":
                    case "stay1":
                    case "stay2":
                    case "stay3":
                    case "stay4":
                    case "stay5":
                    case "parentid":
                    case "kmaterialcode":
                    case "kmcode":
                    case "kbatchno":
                    case "kpdepotwbs":
                    case "updatenum":
                        item.visible = false;
                        break;
                    case "materialcode":
                        item.index = 1;
                        break;
                    case "newmcode":
                         item.index = 2;
                        break;
                    case "newfmodel":
                         item.index = 3;
                        break;
                    case "pbatchno":
                        item.index = 4;
                        break;
                    case "pdepotwbs":
                         item.index = 5;
                        break;
                    case "portcode":
                         item.index = 6;
                        break;
                    case "pnum":
                         item.index = 7;
                        break;
                    case "knum":
                         item.index =8;
                        break;
                    case "cnum":
                         item.index = 9;
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
            ValidateItem(item);
            base.OnAdding(item);
        }

        /// <summary>
        /// 验证数据正确性
        /// </summary>
        /// <param name="item"></param>
        private void ValidateItem(ControllerBase.SaveEvent item)
        {
            Difference bc = item.Item as Difference;
            foreach (var i in bc.Materials)
            {
                
                if (i.UPDATENUM < 0)
                {
                    throw new Exception("异常：变更数量不能小于0");
                }
                if (i.UPDATENUM > i.PNUM)
                {
                    throw new Exception("异常：变更数量大于盘点数量");
                }
            }
            
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="item"></param>
        protected override void OnEditing(Contract.MVC.ControllerBase.SaveEvent item)
        {
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
                if (ac.command.ToLower() == "add" || ac.command.ToLower() == "edit" || ac.command.ToLower() == "remove")
                {
                    ac.visible = false;
                }
            }
            return coms;
        }
    }
}
