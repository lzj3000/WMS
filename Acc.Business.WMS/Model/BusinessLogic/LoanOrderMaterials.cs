using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model;

namespace Acc.Business.WMS.Model
{
    /// <summary>
    /// 描述：借件单明细管理
    /// 作者：路聪
    /// 创建日期:2013-03-28
    /// </summary>
    [EntityClassAttribut("Acc_WMS_LoanOrderMaterials", "借件单明细管理", IsOnAppendProperty = true)]
    public class LoanOrderMaterials : BasicInfo
    {
        /// <summary>
        /// 盘点单号
        /// </summary>
        [EntityControl("借件单号", false, true, 1)]
        [EntityForeignKey(typeof(LoanOrder), "ID", "LOANORDERNO")]
        [EntityField(255, IsNotNullable = true)]
        public string LOANORDERNO { get; set; }
        /// <summary>
        /// 仓库名称
        /// </summary>
        [EntityControl("仓库名称", false, true, 2)]
        [EntityForeignKey(typeof(WareHouse), "ID", "WAREHOUSENAME")]
        [EntityField(50)]
        public string WAREHOUSENAME { get; set; }
        /// <summary>
        /// 物料编码
        /// </summary>
        [EntityControl("物料编码", false, true, 3)]
        [EntityForeignKey(typeof(Materials), "ID", "FNAME")]
        [EntityField(255)]
        public string MATERIALCODE { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        [EntityControl("数量", false, true, 4)]
        [EntityField(30)]
        public double NUM { get; set; }
        /// <summary>
        /// 借件人
        /// </summary>
        [EntityControl("借件人", false, true, 5)]
        [EntityField(255)]
        public string LOANER { get; set; }
    }
}
