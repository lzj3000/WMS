using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Business.Model;

namespace Acc.Business.Controllers
{
    /// <summary>
    /// 系统目录管理
    /// </summary>
    public class SystemDirectoryController:BusinessController
    {
        public SystemDirectoryController() : base(new SystemDirectory()) { }

        public override bool IsClearAway
        {
            get
            {
                return false;
            }
        }
        public override bool IsSubmit
        {
            get
            {
                return false;
            }
        }
        protected override string OnGetPath()
        {
            return "Views/manager/systemdirectory.htm";
        }
        public override bool IsSystemController
        {
            get
            {
                return true;
            }
        }
        protected override void OnForeignLoading(Contract.MVC.IModel model, Contract.Data.ControllerData.loadItem item)
        {
            if (model is SystemModel && !this.user.IsAdministrator && this.fdata != null && this.fdata.objectname == "Acc.Business.Model.ModelCommand")
            {
                SystemModel m = model as SystemModel;
                item.rowsql = "select * from (" + item.rowsql + ") a where a.command not in (select c.command from Acc_Bus_UserRole a,Acc_Bus_RoleModel b,Acc_Bus_RoleCommand c"
+ " where a.RoleID=b.RoleID and a.UserID=" + this.user.ID + " and b.ID=c.RoleModelID and ParentID=" + m.ID + ")";
            }
            else
                base.OnForeignLoading(model, item);
        }

        public override Contract.Data.ControllerData.ReadTable SearchTreeData(Contract.Data.ControllerData.loadItem item)
        {
            return base.SearchTreeData(item);
        }
    }
}
