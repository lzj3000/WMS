using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Business.Model;
using Acc.Contract.Data;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model.Purview;
using Acc.Contract;
using Way.EAP.DataAccess.Regulation;

namespace Acc.Business.Controllers
{
    public class RoleController : BusinessController
    {
        public RoleController() : base(new Role()) { }

        protected override void OnInitViewChildItem(Acc.Contract.Data.ModelData data, Acc.Contract.Data.ItemData item)
        {
            switch (item.field.ToLower())
            {
                case "code":
                case "rowindex":
                case "issubmited":
                case "submiteddate":
                case "submitedby":
                case "modifiedby":
                case "modifieddate":
                case "reviewedby":
                case "revieweddate":
                case "isreviewed":
                    item.visible = false;
                    break;
            }
        }

        [ActionCommand(name = "保存权限", title = "保存角色成员相关权限设置", index = 5, icon = "icon-save", onclick = "saverole", isalert = true)]
        public void test()
        {
            Role role = this.ActionItem as Role;
            if (role != null)
            {
                if (role.Createdby != this.user.ID && !this.user.IsAdministrator)
                {
                    throw new Exception("异常：" + role.RoleName + "角色不是由你创建，不能修改角色权限!");
                }
                Role rr = new Role();
                rr.ID = role.ID;
                rr.RoleModelItems.DataAction = model.GetDataAction();
                rr.RoleModelItems.GetData();
                rr.RoleModelItems.RemoveAll(delegate(RoleModel m) { return true; });
                rr.RoleModelItems.Save();
                rr.Users.DataAction = model.GetDataAction();
                rr.Users.GetData();
                rr.Users.RemoveAll(delegate(UserRole m) { return true; });
                rr.Users.Save();
                role.RoleModelItems.ForEach(delegate(RoleModel rm)
                {
                    rm.RoleCommands.ForEach(delegate(RoleCommand rc)
                    {
                        rc.visible = false;
                        rc.Disabled = true;
                    });
                });
                this.edit();
            }
        }
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
            return "Views/manager/role.htm"; 
        }
        protected override void OnGetWhereing(Contract.MVC.IModel m, List<SQLWhere> where)
        {
            base.OnGetWhereing(m, where);
            if (m is Role)
            {
                if (!this.user.IsAdministrator)
                {
                    SQLWhere w = new SQLWhere();
                    w.ColumnName = "ID";
                    w.Symbol = "in";
                    w.Value = "(select RoleID from Acc_Bus_UserRole where UserID=" + this.user.ID + ")";
                    where.Add(w);
                }
            }
        }
    }
}
