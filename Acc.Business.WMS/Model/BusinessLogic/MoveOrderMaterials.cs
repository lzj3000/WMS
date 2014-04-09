using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model;

namespace Acc.Business.WMS.Model
{
    /// <summary>
    /// 描述：移位单明细
    /// 作者：路聪
    /// 创建日期:2013-2-27
    /// </summary>
    [EntityClassAttribut("Acc_WMS_MoveOrderMaterials", "移位单明细管理", IsOnAppendProperty = true)]
    public class MoveOrderMaterials : BasicInfo
    {
        /// <summary>
        /// 源单数据模型
        /// </summary>
        [EntityField(100)]
        public int SourceID { get; set; }
        /// <summary>
        /// 源单控制器
        /// </summary>
        [EntityField(1000)]
        public string SourceController { get; set; }
        /// <summary>
        /// 源单编码
        /// </summary>
        [EntityControl("源单编码", true, false, 20)]
        [EntityField(100)]
        public string SourceCode { get; set; }
        /// <summary>
        /// 入库单号
        /// </summary>
        [EntityControl("移位单号", false, true, 21)]
        [EntityForeignKey(typeof(MoveOrder), "ID", "Code")]
        [EntityField(38)]
        public int PID { get; set; }
        /// <summary>
        /// 源位置
        /// </summary>
        [EntityControl("源位置", false, true, 6)]
        [EntityForeignKey(typeof(WareHouse), "ID", "Code")]
        [EntityField(38)]
        public int FROMDEPOT { get; set; }
        /// <summary>
        /// 源托盘
        /// </summary>
        [EntityControl("源托盘", false, true, 7)]
        [EntityForeignKey(typeof(Ports), "ID", "PORTNO")]
        [EntityField(255)]
        public string FROMPORT { get; set; }
        /// <summary>
        /// 产品
        /// </summary>
        [EntityControl("产品名称", false, true, 1)]
        [EntityForeignKey(typeof(Materials), "ID", "FNAME")]
        [EntityField(255)]
        public string MATERIALCODE { get; set; }

        [EntityControl("产品编码", false, true, 2)]
        [EntityField(255)]
        public virtual string MCODE { get; set; }
        /// <summary>
        /// 规格型号
        /// </summary>
        //[NotSearchAttribut]
        [EntityControl("规格型号", false, true,3)]
        [EntityField(255)]
        public virtual string FMODEL { get; set; }
        /// <summary>
        /// 批次
        /// </summary>
        [EntityControl("批次", false, true, 4)]
        [EntityField(255)]
        public string BATCHNO { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        [EntityControl("数量", false, true, 5)]
        [EntityField(38)]
        public double NUM { get; set; }

        [EntityControl("目标位置", false, true, 8)]
        [EntityForeignKey(typeof(WareHouse), "ID", "WAREHOUSENAME")]
        [EntityField(38)]
        public int TODEPOT { get; set; }
        /// <summary>
        /// 目标托盘
        /// </summary>
        [EntityControl(" 目标托盘", false, true, 9)]
        [EntityForeignKey(typeof(Ports), "ID", "PORTNO")]
        [EntityField(255)]
        public string TOPORT { get; set; }
    }
}
