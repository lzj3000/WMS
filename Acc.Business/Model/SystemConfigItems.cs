using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;

namespace Acc.Business.Model
{
    /// <summary>
    /// 系统配置管理
    /// </summary>
    [EntityClassAttribut("Acc_Bus_SystemConfigItems", "系统配置")]
    public class SystemConfigItems : BasicInfo
    {

        /// <summary>
        /// 是否默认项
        /// </summary>
        [EntityControl("是否默认项", false, true, 1)]
        [EntityField(5,IsNotNullable = false)]
        public bool ISDEFAULT { get; set; }

        /// <summary>
        /// 是否启用邮件发送（系统使用，提交流程时发送邮件是否启用）
        /// </summary>
        [EntityControl("是否启用邮件发送", false, true, 2)]
        [EntityField(50,IsNotNullable = false)]
        public bool IsEnableEmail { get; set; }

    }
}
