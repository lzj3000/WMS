using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Contract.MVC;
using Way.EAP.DataAccess.Entity;

namespace Acc.Business.Model.Purview
{
    public class SystemUser : BusinessBase
    {
        [EntityPrimaryKey]
        [EntityControl("ID", false, false, 1)]
        [EntityField(200)]
        public override int ID
        {
            get
            {
                return base.ID;
            }
            set
            {
                base.ID = value;
            }
        }
        /// <summary>
        /// 部门
        /// </summary>
        [EntityControl("部门", false, true, 1)]
        [EntityField(200)]
        public string bm { get; set; }
        /// <summary>
        /// 职员
        /// </summary>
        [EntityControl("用户", false, true, 2)]
        [EntityField(200)]
        public string ry { get; set; }
        /// <summary>
        /// 登陆时间
        /// </summary>
        [EntityControl("登陆时间", false, true, 3)]
        [EntityField(200)]
        public DateTime dlsj { get; set; }
        /// <summary>
        /// 操作模块
        /// </summary>
        [EntityControl("最后使用功能", false, true, 4)]
        [EntityField(200)]
        public string czmk { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        [EntityControl("最后使用时间", false, true, 5)]
        [EntityField(200)]
        public DateTime czsj { get; set; }

        [EntityControl("IP地址", false, true, 6)]
        [EntityField(200)]
        public string IP { get; set; }

        [EntityControl("登陆类型", false, true, 7)]
        [EntityField(200)]
        public string LType { get; set; }

    }
}
