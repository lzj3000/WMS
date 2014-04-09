using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Business.Model.Mapping;
using Way.EAP.DataAccess.Entity;

namespace Acc.Business.WMS.Model.Mapping
{
    /// <summary>
    /// 描述：出库单明细数据关系
    /// 作者：胡文杰
    /// 创建日期:2013-08-13
    /// </summary>
    [EntityClassAttribut("Acc_R_StockOutOrderMaterialsMapping", "出库单明细数据关系", IsOnAppendProperty = true)]
    public class StockOutOrderMaterialsMapping : MappingInfo
    {
        /// <summary>
        /// 本系统ID
        /// </summary>
        [EntityControl("本系统ID", true, true, 1)]
        [EntityForeignKey(typeof(StockOutOrderMaterials), "ID", "ID")]
        [EntityField(10)]
        public int FID { get; set; }
    }
}
