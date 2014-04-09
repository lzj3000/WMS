using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model.Mapping;

namespace Acc.Business.WMS.Model.Mapping
{
    /// <summary>
    /// 描述：入库通知明细数据关系
    /// 作者：秦丹
    /// 创建日期:2013-08-29
    /// </summary>
    [EntityClassAttribut("Acc_R_StockInNoticeMapping", "入库通知明细数据关系", IsOnAppendProperty = true)]
    public class StockInNoticeMaterialsMapping : MappingInfo
    {
        /// <summary>
        /// 本系统ID
        /// </summary>
        [EntityControl("本系统ID", true, true, 1)]
        [EntityForeignKey(typeof(StockInNoticeMaterials), "ID", "ID")]
        [EntityField(10)]
        public int FID { get; set; }

    }
}
