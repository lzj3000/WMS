using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model;

namespace Acc.Business.WMS.Model
{
    /// <summary>
    /// 描述：出入库流水表
    /// 作者：路聪
    /// 创建日期:2013-01-10
    /// </summary>
    [EntityClassAttribut("Acc_WMS_IntOutList", "组盘流水表", IsOnAppendProperty = true)]
    public class IntOutList : BasicInfo
    {
        /// <summary>
        /// 撤销
        /// </summary>
        [EntityControl("撤销", false, false, 1)]
        [EntityField(255, IsNotNullable = true)]
        public string CHEXIAO { get; set; }
        /// <summary>
        /// 物料编码
        /// </summary>
        [EntityControl("物料编码", false, true, 2)]
        [EntityField(255, IsNotNullable = true)]
        public string MATERIALCODE { get; set; }
        /// <summary>
        /// 物料名称
        /// </summary>
        [EntityControl("物料名称", false, true, 3)]
        [EntityField(255, IsNotNullable = true)]
        public string FNAME { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        [EntityControl("状态", false, true, 4)]
        [EntityField(255, IsNotNullable = true)]
        public string FLAG { get; set; }
        /// <summary>
        /// 入库单号
        /// </summary>
        [EntityControl("入库单号", false, true, 5)]
        [EntityField(255, IsNotNullable = true)]
        public string ORDERNO { get; set; }
        /// <summary>
        /// 货位
        /// </summary>
        [EntityControl("货位", false, true, 6)]
        [EntityField(255, IsNotNullable = true)]
        public string BINCODE { get; set; }
        /// <summary>
        /// 入库仓库
        /// </summary>
        [EntityControl("入库仓库", false, true, 7)]
        [EntityField(255, IsNotNullable = true)]
        public string HOUSENO { get; set; }
        /// <summary>
        /// 入库数量
        /// </summary>
        [EntityControl("入库数量", false, true, 8)]
        [EntityField(255, IsNotNullable = true)]
        public string INNUM { get; set; }
        /// <summary>
        /// 入库单位
        /// </summary>
        [EntityControl("入库单位", false, true, 9)]
        [EntityField(255, IsNotNullable = true)]
        public string UNIT { get; set; }
        /// <summary>
        /// 出库数量
        /// </summary>
        [EntityControl("出库数量", false, true, 10)]
        [EntityField(255, IsNotNullable = true)]
        public string OUTNUM { get; set; }
        /// <summary>
        /// 入库时间
        /// </summary>
        [EntityControl("入库时间", false, true, 11)]
        [EntityField(255, IsNotNullable = true)]
        public DateTime INTIME { get; set; }
        /// <summary>
        /// 出库时间
        /// </summary>
        [EntityControl("出库时间", false, true, 12)]
        [EntityField(255, IsNotNullable = true)]
        public DateTime OUTTIME { get; set; }
        /// <summary>
        /// 操作人编码
        /// </summary>
        [EntityControl("操作人编码", false, true, 13)]
        [EntityField(255, IsNotNullable = true)]
        public string USERCODE { get; set; }
        /// <summary>
        /// 操作人名称
        /// </summary>
        [EntityControl("操作人名称", false, true, 14)]
        [EntityField(255, IsNotNullable = true)]
        public string USERNAME { get; set; }
        /// <summary>
        /// 生产批号
        /// </summary>
        [EntityControl("生产批号", false, true, 15)]
        [EntityField(255, IsNotNullable = true)]
        public string BATCHNO { get; set; }
        /// <summary>
        /// 托盘编码
        /// </summary>
        [EntityControl("托盘编码", false, true, 16)]
        [EntityField(255, IsNotNullable = true)]
        public string PORTCODE { get; set; }
        /// <summary>
        /// 是否上传
        /// </summary>
        [EntityControl("是否上传", false, false, 17)]
        [EntityField(255, IsNotNullable = true)]
        public string ISUPLOAD { get; set; }
    }
}
