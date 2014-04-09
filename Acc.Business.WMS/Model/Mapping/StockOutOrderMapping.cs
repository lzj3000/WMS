using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Business.Model.Mapping;
using Way.EAP.DataAccess.Entity;

namespace Acc.Business.WMS.Model.Mapping
{
    /// <summary>
    /// 描述：出库单数据关系
    /// 作者：秦丹
    /// 创建日期:2013-5-15
    /// </summary>
    [EntityClassAttribut("Acc_R_StockOutOrderMapping", "出库单数据关系", IsOnAppendProperty = true)]
    public class StockOutOrderMapping : MappingInfo
    {
        /// <summary>
        /// 本系统ID
        /// </summary>
        [EntityControl("本系统ID", true, true, 1)]
        [EntityForeignKey(typeof(StockOutOrder), "ID", "ID")]
        [EntityField(10)]
        public int FID { get; set; }
    }
}
