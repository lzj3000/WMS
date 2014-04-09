using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Business.Model;

namespace Acc.Business.Controllers
{
    class UnitController : BusinessController
    {
        public UnitController() : base(new Unit()) { }
        //是否启用回收站
        public override bool IsClearAway
        {
            get
            {
                return base.IsClearAway;
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
            return "";
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
    }
}
