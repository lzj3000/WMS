using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Business.Model.Mapping;
using Way.EAP.DataAccess.Entity;

namespace Acc.Business.WMS.Model.Mapping
{
    /// <summary>
    /// 描述：入库通知数据关系
    /// 作者：秦丹
    /// 创建日期:2013-08-29
    /// </summary>
    [EntityClassAttribut("Acc_R_StockInNoticeMapping", "入库通知数据关系", IsOnAppendProperty = true)]
    public class StockInNoticeMapping : MappingInfo
    {
        /// <summary>
        /// 本系统ID
        /// </summary>
        [EntityControl("本系统ID", true, true, 1)]
        [EntityForeignKey(typeof(StockInNotice), "ID", "ID")]
        [EntityField(10)]
        public int FID { get; set; }

    }
}
