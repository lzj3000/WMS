using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model.Mapping;

namespace Acc.Business.Model.Mapping
{
    /// <summary>
    /// 描述：客户数据关系
    /// 作者：秦丹
    /// 创建日期:2013-09-6
    /// </summary>
    [EntityClassAttribut("Acc_R_CustomerMapping", "客户数据关系", IsOnAppendProperty = true)]
    public class CustomerMapping : MappingInfo
    {
        /// <summary>
        /// 本系统ID
        /// </summary>
        [EntityControl("本系统ID", true, true, 1)]
        [EntityForeignKey(typeof(BusinessCustomer), "ID", "ID")]
        [EntityField(10)]
        public int FID { get; set; }

    }
}
