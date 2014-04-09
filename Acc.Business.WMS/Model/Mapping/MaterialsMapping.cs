using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model.Mapping;

namespace Acc.Business.WMS.Model.Mapping
{
    /// <summary>
    /// 描述：产品数据关系
    /// 作者：秦丹
    /// 创建日期:2013-09-12
    /// </summary>
    [EntityClassAttribut("Acc_R_MaterialsMapping", "产品数据关系", IsOnAppendProperty = true)]
    public class MaterialsMapping : MappingInfo
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
