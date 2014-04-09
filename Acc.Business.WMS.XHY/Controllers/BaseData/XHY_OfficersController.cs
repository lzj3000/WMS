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
using Acc.Business.Model;

namespace Acc.Business.WMS.XHY.Controllers
{
    public class XHY_OfficersController : OfficeWorkerController
    {
        public XHY_OfficersController()
            : base(new OfficeWorker())
        {}
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

        //菜单中url路径
        protected override string OnGetPath()
        {
            return "Views/manager/officeworker.htm";
        }

        protected override void OnInitViewChildItem(ModelData data, ItemData item)
        {
            base.OnInitViewChildItem(data, item);
            if (data.name.EndsWith("OfficeWorker"))
            {
                switch (item.field.ToLower())
                {
                    case "createdby":
                    case "creationdate":
                        item.disabled = true;
                        break;
                    case "phonecode":
                        item.visible = false;
                        break;
                    default:
                        break;
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
                if (ac.command == "selectPorts" || ac.command == "remove" )
                {
                    ac.visible = false;
                }
                
            }
            return coms;
        }
       

        [ActionCommand(name = "禁用", title = "禁用（在系统中不能使用）", index = 8, icon = "icon-remove", isalert = true, isselectrow = true)]
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

        protected override void OnEditing(ControllerBase.SaveEvent item)
        {
            OfficeWorker ow = item.Item as OfficeWorker;
            if (ow.ID != 0)
            {
                EntityList<OfficeWorker> owList = new EntityList<OfficeWorker>(this.model.GetDataAction());
                owList.GetData("LoginName='" + ow.LoginName + "' and id <> "+ow.ID+"");
                if (owList.Count > 0)
                {
                    throw new Exception("异常：" + ow.LoginName + "登录名称已存在不能重复！");
                }
            }
            base.OnEditing(item);
        }
    }
}
