using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;

namespace Acc.Business.Model
{
    /// <summary>
    /// 发送消息邮件设置
    /// </summary>
    [EntityClassAttribut("Acc_Bus_SystemEmail", "系统邮件")]
    public class SystemEmail : BasicInfo
    {
        /// <summary>
        /// 邮件标题
        /// </summary>
        [EntityControl("邮件标题", false, true, 1)]
        [EntityField(50, IsNotNullable = true)]
        public string EMAILSUBJECT { get; set; }

        /// <summary>
        /// 邮件账号
        /// </summary>
        [EntityControl("邮件账号", false, true, 2)]
        [EntityField(50,IsNotNullable = true)]
        public string EMAILNAME { get; set; }

        /// <summary>
        /// 邮件密码
        /// </summary>
        [EntityControl("邮件密码", false, true, 3)]
        [EntityField(50, IsNotNullable = true)]
        public string EMAILPASS { get; set; }

        /// <summary>
        /// 主机名称或IP地址
        /// </summary>
        [EntityControl("主机名称或IP地址", false, true, 4)]
        [EntityField(50, IsNotNullable = true)]
        public string EMAILHOST { get; set; }

        /// <summary>
        /// 默认邮件正文
        /// </summary>
        [EntityControl("默认邮件正文", false, true, 5)]
        [EntityField(200)]
        public string DEFAULTEMAIL { get; set; }

        /// <summary>
        /// 邮件来自
        /// </summary>
        [EntityControl("邮件来自", false, true, 6)]
        [EntityField(200)]
        public string EMAILFROM { get; set; }


        /// <summary>
        /// 是否默认
        /// </summary>
        [EntityControl("是否默认", false, true, 7)]
        [EntityField(5)]
        public bool ISDEFAULT { get; set; }
    }
}
