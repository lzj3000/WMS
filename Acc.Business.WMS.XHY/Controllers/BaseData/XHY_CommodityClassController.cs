using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Business.Controllers;
using Acc.Business.WMS.Controllers;
using Acc.Business.WMS.Model;
using Acc.Business.WMS.XHY.Model;
using Way.EAP.DataAccess.Data;
using System.Data;
using Way.EAP.DataAccess.Entity;
using Acc.Contract.Data.ControllerData;
using Acc.Contract.Data;
using Acc.Contract.MVC;

namespace Acc.Business.WMS.XHY.Controllers
{
    public class XHY_CommodityClassController : CommodityClassController
    {
        //菜单中url路径
        protected override string OnGetPath()
        {
            return "Views/manager/commodityclass.htm";
        }

        public override bool IsSubmit
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

        /// <summary>
        /// 设置按钮显示
        /// </summary>
        protected override ActionCommand[] OnInitCommand(ActionCommand[] commands)
        {
            ActionCommand[] coms = base.OnInitCommand(commands);
            //获取所有按钮集合
            foreach (ActionCommand ac in coms)
            {
                if ( ac.command == "remove" || ac.command == "add" || ac.command == "edit")
                {
                    ac.visible = false;
                }
            }
            return coms;
        }
    }
}
