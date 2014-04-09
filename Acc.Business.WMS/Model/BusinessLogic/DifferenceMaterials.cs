using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model;

namespace Acc.Business.WMS.Model
{
    /// <summary>
    /// 描述：盘点差异明细表
    /// 作者：柳强
    /// 创建日期:2013-06-07
    /// </summary>
    [EntityClassAttribut("Acc_WMS_DifferenceMaterials", "盘点差异明细表", IsOnAppendProperty = true)]
    public class DifferenceMaterials : BasicInfo
    {
        /// <summary>
        /// 产品
        /// </summary>
        [EntityControl("盘点产品名称", false, true, 1)]
        [EntityForeignKey(typeof(Materials), "ID", "FNAME")]
        [EntityField(100)]
        public string MATERIALCODE { get; set; }

        [EntityControl("产品编码", false, true, 2)]
        [EntityField(100)]
        public string NEWMCODE { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        [EntityControl("规格型号", false, true, 3)]
        [EntityField(255)]
        public string NEWFMODEL { get; set; }

        /// <summary>
        /// 盘点批次
        /// </summary>
        [EntityControl("盘点批次", false, true, 4)]
        [EntityField(255)]
        public string PBATCHNO { get; set; }

        /// <summary>
        /// 盘点位置
        /// </summary>
        [EntityControl("盘点位置", false, true, 5)]
        [EntityForeignKey(typeof(WareHouse), "ID", "WAREHOUSENAME")]
        [EntityField(30, IsNotNullable = true)]
        public string PDEPOTWBS { get; set; }


        /// <summary>
        /// 托盘编码
        /// </summary>
        [EntityControl("托盘编码", false, true, 6)]
        [EntityForeignKey(typeof(Ports), "ID", "PORTNO")]
        [EntityField(255)]
        public string PORTCODE { get; set; }

        /// <summary>
        /// 盘点数量
        /// </summary>
        [EntityControl("盘点数量", false, true, 6)]
        [EntityField(30, IsNotNullable = true)]
        public double PNUM { get; set; }

        /// <summary>
        /// 盘点单号
        /// </summary>
        [EntityControl("盘点单号", false, false, 7)]
        [EntityForeignKey(typeof(Difference), "ID", "Code")]
        [EntityField(10, IsNotNullable = true)]
        public int PARENTID { get; set; }

        /// <summary>
        /// 差异数量
        /// </summary>
        [EntityControl("差异数量", false, true, 15)]
        [EntityField(30)]
        public double CNUM { get; set; }

        /// <summary>
        /// 修正数量
        /// </summary>
        [EntityControl("修正数量", false, true, 9)]
        [EntityField(30)]
        public double UPDATENUM { get; set; }

        /// <summary>
        /// 库存产品
        /// </summary>
        [EntityControl("库存产品名称", false, true, 10)]
        [EntityForeignKey(typeof(Materials), "ID", "FNAME")]
        [EntityField(100)]
        public string KMATERIALCODE { get; set; }

        [EntityControl("库存产品编码", false, true, 11)]
        [EntityField(100)]
        public string KMCODE { get; set; }

        /// <summary>
        /// 库存批次
        /// </summary>
        [EntityControl("库存产品批次", false, true, 12)]
        [EntityField(255)]
        public string KBATCHNO { get; set; }

        /// <summary>
        /// 库存产品位置
        /// </summary>
        [EntityControl("库存产品位置", false, true, 13)]
        [EntityForeignKey(typeof(WareHouse), "ID", "WAREHOUSENAME")]
        [EntityField(30, IsNotNullable = false)]
        public int KPDEPOTWBS { get; set; }

        /// <summary>
        /// 库存产品数量
        /// </summary>
        [EntityControl("库存产品数量", false, true, 14)]
        [EntityField(30)]
        public double KNUM { get; set; }
    }
}
