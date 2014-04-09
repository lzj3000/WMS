using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;

namespace Acc.Business.Model.Purview
{
    /// <summary>
    /// 职员角色表
    /// </summary>
    [EntityClassAttribut("Acc_Bus_UserRole", "职员角色", IsOnAppendProperty = true)]
    public class UserRole : BusinessBase
    {
        public UserRole[] GetUR()
        {
            return null;
        }
        [EntityControl("职员", false, false, 1)]
        [EntityForeignKey(typeof(OfficeWorker), "ID", "WorkName")]
        [EntityField(50)]
        public int UserID { get; set; }

        [EntityControl("角色", false, true, 2)]
        [EntityForeignKey(typeof(Role), "ID", "RoleName")]
        [EntityField(50)]
        public int RoleID { get; set; }
    }
}
