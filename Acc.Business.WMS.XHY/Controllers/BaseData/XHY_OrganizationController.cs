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
    public class XHY_OrganizationController : OrganizationController
    {

        //菜单中url路径
        protected override string OnGetPath()
        {
            return "Views/manager/organization.htm";
        }

        protected override void OnInitViewChildItem(Acc.Contract.Data.ModelData data, Acc.Contract.Data.ItemData item)
        {

            switch (item.field.ToLower())
            {
                case "isdisable":
                    item.disabled = true;
                    break;
            }
        }


        public override bool IsSubmit
        {
            get
            {
                return true;
            }
        }

        public override bool IsPrint
        {
            get
            {
                return true;
            }
        }

        public override bool IsClearAway
        {
            get
            {
                return true;
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
                   // ac.visible = false;
                }
            }
            return coms;
        }




        [ActionCommand( name = "禁用", title = "禁用（在系统中不能使用）", index = 8, icon = "icon-remove", isalert = true, isselectrow = true)]
        public void isdisfalse()
        {
            ///获取当前表名称
            string tn = this.ActionItem.ToString();
            ///以下方法可变成多条启用
            IDataAction da = this.model.GetDataAction();
            if (this.ActionItem["ID"] != null)
            {
                string sql = "update " + tn + " set IsDisable = 1 where id = '" + this.ActionItem["ID"] + "'";
                da.Execute(sql);
            }
        }


        [ActionCommand(name = "启用", title = "启用（在系统中可以使用）", index = 8, icon = "icon-ok", isalert = true, isselectrow = true)]
        public void isdistrue()
        {
            ///获取当前表名称
            string tn = this.ActionItem.ToString();
            ///以下方法可变成多条启用
            IDataAction da = this.model.GetDataAction();
            if (this.ActionItem["ID"] != null)
            {
                string sql = "update " + tn + " set IsDisable = 0 where id = '" + this.ActionItem["ID"] + "'";
                da.Execute(sql);
            }
        }
    }
}
