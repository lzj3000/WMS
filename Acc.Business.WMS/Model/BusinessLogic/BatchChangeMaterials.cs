using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model;

namespace Acc.Business.WMS.Model
{
    /// <summary>
    /// 描述：批次变更明细
    /// 作者：柳强
    /// 创建日期:2013-06-06
    /// </summary>
    [EntityClassAttribut("Acc_WMS_BatchChangeMaterials", "库存批次变更明细", IsOnAppendProperty = true)]
    public class BatchChangeMaterials : StockInfoMaterials
    {
        /// <summary>
        /// 来源单号
        /// </summary>
        [EntityControl("来源单号", false, true, 1)]
        [EntityForeignKey(typeof(BatchChange), "ID", "Code")]
        [EntityField(255, IsNotNullable = true)]
        public int PARENTID { get; set; }

        /// <summary>
        /// 产品
        /// </summary>
        [EntityControl("库存项", false, true, 1)]
        [EntityForeignKey(typeof(StockInfoMaterials), "ID", "Code")]
        [EntityField(100, IsNotNullable = true)]
        public string NEWCODE { get; set; }

        [EntityControl("产品编码", false, true, 2)]
        [EntityField(100)]
        public string NEWMCODE { get; set; }

        [EntityControl("产品名称", false, true, 3)]
        [EntityForeignKey(typeof(Materials), "ID", "FNAME")]
        [EntityField(100)]
        public string NEWMNAME { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        [EntityControl("规格型号", false, true, 4)]
        [EntityField(255)]
        public string NEWFMODEL { get; set; }

        /// <summary>
        /// 变更数量
        /// </summary>
        [EntityControl("变更数量", false, true, 5)]
        [EntityField(18, IsNotNullable = true)]
        public double NEWNUM { get; set; }

        /// <summary>
        /// 变更批次
        /// </summary>
        [EntityControl("变更批次", false, true, 6)]
        [EntityField(255, IsNotNullable = false)]
        public string NEWBATCHNO { get; set; }

    }
}
