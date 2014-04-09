using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model;

namespace Acc.Business.WMS.Model
{
    /// <summary>
    /// 描述：组盘流水表
    /// 作者：路聪
    /// 创建日期:2013-01-10
    /// </summary>
    [EntityClassAttribut("Acc_WMS_SetPortRecord", "组盘流水表", IsOnAppendProperty = true)]
    public class SetPortRecord : BasicInfo
    {
        /// <summary>
        /// 入库单号
        /// </summary>
        [EntityControl("入库单号", false, true, 1)]
        [EntityField(255)]
        public string ORDERNO { get; set; }
        /// <summary>
        /// 托盘编码
        /// </summary>
        [EntityControl("托盘编码", false, true, 2)]
        [EntityField(255, IsNotNullable = true)]
        public string PORTNO { get; set; }
        /// <summary>
        /// 生产线
        /// </summary>
        [EntityControl("生产线", false, true, 3)]
        [EntityField(255)]
        public string PRODUCTIONLINE { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        [EntityControl("数量", false, true, 4)]
        [EntityField(255, IsNotNullable = true)]
        public string NUM { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [EntityControl("创建时间", false, true, 5)]
        [EntityField(255, IsNotNullable = true)]
        public string CREATETIME { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        [EntityControl("创建人", false, true, 6)]
        [EntityField(255, IsNotNullable = true)]
        public string CREATEUSER { get; set; }
        /// <summary>
        /// 包装0
        /// </summary>
        [EntityControl("包装0", false, true, 7)]
        [EntityField(255)]
        public string PACKNUM0 { get; set; }
        /// <summary>
        /// 包装1
        /// </summary>
        [EntityControl("包装1", false, true, 8)]
        [EntityField(255)]
        public string PACKNUM1 { get; set; }
        /// <summary>
        /// 包装2
        /// </summary>
        [EntityControl("包装2", false, true, 9)]
        [EntityField(255)]
        public string PACKNUM2 { get; set; }
        /// <summary>
        /// 仓库
        /// </summary>
        [EntityControl("仓库", false, true, 10)]
        [EntityField(255)]
        public string HOUSECODE { get; set; }
        /// <summary>
        /// 货位编码
        /// </summary>
        [EntityControl("货位编码", false, true, 11)]
        [EntityField(255, IsNotNullable = true)]
        public string BINCODE { get; set; }
        /// <summary>
        /// 组盘类型
        /// </summary>
        [EntityControl("组盘类型", false, true, 13)]
        [EntityField(255, IsNotNullable = true)]
        public string ZUPANTYPE { get; set; }
        /// <summary>
        /// 物料编码
        /// </summary>
        [EntityControl("物料编码", false, true, 14)]
        [EntityField(255, IsNotNullable = true)]
        public string MATERIALCODE { get; set; }
    }
}
