using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model;
using Acc.Business.WMS.Controllers;
using Acc.Contract.MVC;

namespace Acc.Business.WMS.Model
{
    /// <summary>
    /// 描述：发运详单表
    /// 作者：柳强
    /// 创建日期:2013-03-13
    /// </summary>
    [EntityClassAttribut("Acc_WMS_ShippingOrderMaterials", "发运明细单", IsOnAppendProperty = true)]
    public class ShippingOrderMaterials : OrderMaterialsCommon
    {

        /// <summary>
        /// 来源运单号
        /// </summary>
        [EntityControl("来源单号", false, true, 1)]
        [EntityField(255)]
        [EntityForeignKey(typeof(ShippingOrder), "ID", "CODE")]
        public int PARENTID { get; set; }

        /// <summary>
        /// 发运产品
        /// </summary>
        [EntityControl("发运产品", false, true, 2)]
        [EntityField(255, IsNotNullable = true)]
        [EntityForeignKey(typeof(Materials), "ID", "FNAME")]
        public string SHIPPINGMATERIALS { get; set; }

        /// <summary>
        /// 发运数量
        /// </summary>
        [EntityControl("发运数量", false, true, 5)]
        [EntityField(255, IsNotNullable = true)]
        public string SHIPPINGNUM { get; set; }

    }
}
