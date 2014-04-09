using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;

namespace Acc.Business.Model.Mapping
{
    /// <summary>
    /// 部门数据关系表
    /// </summary>
    [EntityClassAttribut("Acc_R_OrganizationMapping", "部门数据关系", IsOnAppendProperty = true)]
    public class OrganizationMapping : MappingInfo
    {
        /// <summary>
        /// 本系统ID
        /// </summary>
        [EntityControl("本系统ID", true, true, 1)]
        [EntityForeignKey(typeof(Organization), "ID", "ID")]
        [EntityField(10)]
        public int FID { get; set; }

    }
}
