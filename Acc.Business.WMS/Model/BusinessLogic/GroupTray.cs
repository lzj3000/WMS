using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model;
using Acc.Business.WMS.Model;

namespace Acc.Business.WMS.Model
{
    /// <summary>
    /// 描述：终端功能配置
    /// 作者：张朝阳
    /// 创建日期:2013-08-02
    /// </summary>
    [EntityClassAttribut("Acc_WMS_GroupTray", "组盘记录表", IsOnAppendProperty = true)]
    public class GroupTray :BasicInfo
    {
        /// <summary>
        /// 入库单号
        /// </summary>
        [EntityControl("入库单号", false, true, 0)]
        [EntityField(50)]
        public string OrderCode{get;set;}
        /// <summary>
        /// 车间
        /// </summary>
        [EntityControl("车间", false, true, 1)]
        [EntityField(50)]
        [EntityForeignKey(typeof(Organization), "ID", "OrganizationName")]
        public string WHNAME{get;set;}
        /// <summary>
        /// 托盘编码
        /// </summary>
        [EntityControl("托盘编码", false, true, 2)]
        [EntityField(50)]
        [EntityForeignKey(typeof(Ports), "ID", "PORTNO")]
        public string TrayCode { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [EntityControl("名称", false, true, 3)]
        [EntityField(200)]
        [EntityForeignKey(typeof(Materials), "ID", "FNAME")]
        public string GNAME { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [EntityControl("编码", false, true,4)]
        [EntityField(50)]
        public string GCODE { get; set; }
        
        /// <summary>
        /// 批次
        /// </summary>
        [EntityControl("批次", false, true, 5)]
        [EntityField(50)]
        public string BatchNo { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        [EntityControl("数量", false, true, 6)]
        [EntityField(50)]
        public decimal TrayNum { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        [EntityControl("规格", false, true, 7)]
        [EntityField(50)]
        public string FModel { get; set; }

        /// <summary>
        /// 基本计量单位
        /// </summary>
        [EntityControl("基本计量单位", false, true, 8)]
        [EntityForeignKey(typeof(Unit), "ID", "UNITNAME")]
        [EntityField(80)]
        public int FUNITID { get; set; }       
    }
}
