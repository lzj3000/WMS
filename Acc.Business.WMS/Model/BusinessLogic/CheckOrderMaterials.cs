using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model;

namespace Acc.Business.WMS.Model
{
    /// <summary>
    /// 描述：盘点明细管理
    /// 作者：路聪
    /// 创建日期:2012-12-26
    /// </summary>
    [EntityClassAttribut("Acc_WMS_CheckOrderMaterials", "盘点明细管理", IsOnAppendProperty = true)]
    public class CheckOrderMaterials : BasicInfo
    {
        /// <summary>
        /// 盘点单号
        /// </summary>
        [EntityControl("盘点单号", false, false, 1)]
        [EntityForeignKey(typeof(CheckOrder), "ID", "Code")]
        [EntityField(255)]
        public int PARENTID { get; set; }

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
        [EntityControl("盘点批次", false, true, 5)]
        [EntityField(255)]
        public string PBATCHNO { get; set; }

        /// <summary>
        /// 盘点位置
        /// </summary>
        [EntityControl("盘点位置", false, true, 4)]
        [EntityForeignKey(typeof(WareHouse), "ID", "WAREHOUSENAME")]
        [EntityField(30, IsNotNullable = true)]
        public string PDEPOTWBS { get; set; }


        /// <summary>
        /// 托盘编码
        /// </summary>
        [EntityControl("托盘编码", false, true, 5)]
        [EntityForeignKey(typeof(Ports), "ID", "PORTNO")]
        [EntityField(255)]
        public string PORTCODE { get; set; }

        /// <summary>
        /// 盘点数量
        /// </summary>
        [EntityControl("盘点数量", false, true, 6)]
        [EntityField(30, IsNotNullable = true)]
        public double PNUM { get; set; }
    }

}
