using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model.Purview;
using Acc.Contract.MVC;
using System.Xml.Schema;

namespace Acc.Business.Model
{
    /// <summary>
    /// 职员
    /// </summary>
    [EntityClassAttribut("Acc_Bus_OfficeWorker", "职员",IsOnAppendProperty = true)]
    public class OfficeWorker : BasicInfo
    {
        public OfficeWorker()
        { 
           
        }
        public OfficeWorker[] GetOfficeWorker()
        {
            return null;
        }
        /// <summary>
        /// 登陆名
        /// </summary>
        [EntityControl("登陆名", false, true, 2)]
        [EntityField(200)]
        public string LoginName { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [EntityControl("名称", false, true, 2)]
        [EntityField(200,IsNotNullable=true)]
        public string WorkName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [EntityField(50)]
        public string WorkPassWord { get; set; }
        /// <summary>
        /// 是否可登录系统
        /// </summary>
        [EntityControl("是否登录", false, true, 6)]
        [EntityField(1)]
        public bool IsLoginUser { get; set; }

        [EntityControl("手机号码", false, true, 3)]
        [EntityField(12)]
        public int PhoneCode { get; set; }

        [EntityControl("电子邮件", false, true, 4)]
        [EntityField(50)]
        public string Email { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        //[EntityControl("角色", false, true, 5)]
        //[EntityForeignKey(typeof(Role), "ID", "RoleName")]
        //[EntityField(50)]
        //public int RoleID { get; set; }
        /// <summary>
        /// 所在部门
        /// </summary>
        [EntityControl("所在部门", false, true, 4)]
        [EntityForeignKey(typeof(Organization), "ID", "OrganizationName")]
        [EntityField(50,IsNotNullable=true)]
        public int OrganizationID { get; set; }
        /// <summary>
        /// 父ID
        /// </summary>
        [EntityControl("上级经理", false, true, 3)]
        [EntityForeignKey(typeof(OfficeWorker), "ID", "WorkName")]
        [EntityField(50)]
        public int ParentID { get; set; }

        private HierarchicalEntityView<OfficeWorker, OfficeWorker> _childItems;
        [HierarchicalEntityControl(visible=false)]
        public HierarchicalEntityView<OfficeWorker, OfficeWorker> ChildItems
        {
            get
            {
                if (_childItems == null)
                {
                    _childItems = new HierarchicalEntityView<OfficeWorker, OfficeWorker>(this); 
                }
                return _childItems;
            }
        }

        private HierarchicalEntityView<OfficeWorker, UserRole> _roles;

        [HierarchicalEntityControl(isadd = false, isedit = false, isselect = true, c = "Acc.Business.Controllers.RoleController",ischeck=true)]
        public HierarchicalEntityView<OfficeWorker, UserRole> Roles
        {
            get
            {
                if (_roles == null)
                    _roles = new HierarchicalEntityView<OfficeWorker, UserRole>(this);
                return _roles;
            }
        }

    }
}
