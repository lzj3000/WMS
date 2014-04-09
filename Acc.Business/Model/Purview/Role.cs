using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Way.EAP.DataAccess.Data;
using Acc.Business.Model.Purview;

namespace Acc.Business.Model
{
    /// <summary>
    /// 角色 
    /// </summary>
    [EntityClassAttribut("Acc_Bus_Role", "角色", IsOnAppendProperty = true)]
    public class Role : BasicInfo
    {
        public Role[] GetRole()
        {
            return null;
        }
        /// <summary>
        /// 名称
        /// </summary>
        [EntityControl("名称", false, true, 2)]
        [EntityField(200)]
        public string RoleName { get; set; }
        /// <summary>
        /// 角色标识
        /// </summary>
        [EntityField(50)]
        public string RoleMark { get; set; }
        /// <summary>
        /// 是否系统内制角色
        /// </summary>
        [EntityField(1)]
        [EntityControl("系统角色", true, true, 4)]
        public bool IsSystem { get; set; }


        private HierarchicalEntityView<Role, RoleModel> _roleModelItems;

        public HierarchicalEntityView<Role, RoleModel> RoleModelItems
        {
            get
            {
                if (_roleModelItems == null)
                    _roleModelItems = new HierarchicalEntityView<Role, RoleModel>(this);
                return _roleModelItems;
            }
        }

        private HierarchicalEntityView<Role, UserRole> _users;

        public HierarchicalEntityView<Role, UserRole> Users
        {
            get
            {
                if (_users == null)
                    _users = new HierarchicalEntityView<Role, UserRole>(this);
                return _users;
            }
        }
        protected override void SaveFirst(EntitySaveArgs args)
        {
            if (args.EntityState == EntityState.Delete)
            {
                if (this.IsSystem)
                {
                    throw new Exception("异常：系统内制角色，不允许删除！");
                }
            }
            if (args.EntityState == EntityState.Update)
            {
                if (this.IsDisable && this.IsSystem)
                {
                    throw new Exception("异常：系统内制角色，不允许禁用！");
                }
            }
            base.SaveFirst(args);
        }
        public void CreateBusinessRole()
        {
            Role role = new Role();
            role.RoleName = "系统管理员";
            role.IsSystem = true;
            role.RoleMark = "admin";
            role.Remark = "系统管理员角色拥有系统最高权限。";
            SetSystemRole(role);

            //Role gg = new Role();
            //gg.RoleName = "公共角色";
            //gg.IsSystem = true;
            //gg.RoleMark = "public";
            //gg.Remark = "公共角色，该角色下权限自动分配给所有系统内用户。";
           // SetSystemRole(gg);
        }

        public virtual void SetSystemRole(Role item)
        {
            if (string.IsNullOrEmpty(item.RoleMark))
            {
                throw new Exception("异常：系统内制角色，标识不允许为空！");
            }
            item.IsSystem = true;
            IDataAction action = this.GetDataAction();
            string sql = "select * from " + this.ToString() + " where RoleMark='{0}'";
            object obj = action.GetValue(string.Format(sql, item.RoleMark));
            if (obj == null || string.IsNullOrEmpty(obj.ToString()))
            {

                ((IEntityBase)item).StateBase = EntityState.Insert;
                item.Save(action);
                OfficeWorker ow = new OfficeWorker();
                RoleModel rm = new RoleModel();
                RoleCommand rc = new RoleCommand();
                SystemModel sm = new SystemModel();
                SystemDirectory sd = new SystemDirectory();
                if (item.RoleMark == "admin")
                {
                    sql = "select * from Acc_Bus_OfficeWorker where LoginName='admin'";
                    obj = action.GetValue(sql);
                    if (obj == null || string.IsNullOrEmpty(obj.ToString()))
                    {
                        ow.LoginName = "admin";
                        ow.WorkName = "系统管理员";
                        ow.IsDelete = false;
                        ow.IsLoginUser = true;
                        ow.IsDisable = false;
                        ow.IsSubmited = true;
                        ow.IsReviewed = true;
                        ow.WorkPassWord = "acctrue";
                        ow.Roles.Add(new UserRole() { RoleID = item.ID });
                        ((IEntityBase)ow).StateBase = EntityState.Insert;
                        ((IEntityBase)ow).Save(action);
                    }
                }
            }
        }
    }
}
