using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Business.Model;
using Acc.Business.Controllers;
using Acc.Contract.MVC;

namespace Acc.Business.WMS.Controllers
{
    public class UnitController : BusinessController
    {
        public UnitController() : base(new Unit()) { }
        public UnitController(IModel model) : base(model) { }
        //是否启用回收站
        public override bool IsClearAway
        {
            get
            {
                return base.IsClearAway;
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
        //是否启用提交
        public override bool IsSubmit
        {
            get
            {
                return false;
            }
        }
        //显示在菜单
        protected override string OnControllerName()
        {
            return "单位管理";
        }
        //菜单中url路径
        protected override string OnGetPath()
        {
            return "Views/WMS/Materials/Unit.htm";
        }
        //开发人
        protected override string OnGetAuthor()
        {
            return "路聪";
        }
        //说明
        protected override string OnGetControllerDescription()
        {
            return "单位管理";
        }

        #region 初始化数据方法
        protected override void OnInitViewChildItem(Contract.Data.ModelData data, Contract.Data.ItemData item)
        {
            if (data.name.EndsWith("Unit"))
            {
                switch (item.field.ToLower())
                {
                    case "unitname":
                        item.disabled = false;
                        item.visible = true;
                        break;
                    case "issubmited":
                    case "submiteddate":
                    case "submitedby":
                    case "isreviewed":
                    case "reviewedby":
                    case "revieweddate":
                        item.visible = false;
                        break;
                }
            }
        }

        #endregion 

        
        #region 添加数据方法
        protected override void OnAdding(Contract.MVC.ControllerBase.SaveEvent item)
        {
            base.OnAdding(item);
        }


        protected override void OnEditing(ControllerBase.SaveEvent item)
        {
            base.OnAdding(item);
        }
        #endregion
    }
}
