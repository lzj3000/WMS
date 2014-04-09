using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model;


namespace Acc.Business.WMS.Model
{
    /// <summary>
    /// 描述：库存序列码表
    /// 作者：路聪
    /// 创建日期:2012-12-17
    /// </summary>
    [EntityClassAttribut("Acc_WMS_StockSequence", "库存序列码表", IsOnAppendProperty = false)]
    public class StockSequence : BasicInfo
    {
        /// <summary>
        /// 库存物料id
        /// </summary>
        [EntityControl("库存物料id", false, false, 1)]
        [EntityForeignKey(typeof(StockInfoMaterials), "ID", "Code")]
        [EntityField(20)]
        public float STOCKINFOMATERIALSID { get; set; }

        /// <summary>
        /// 序列码
        /// </summary>
        [EntityControl("序列码", false, false, 2)]
        [EntityField(20)]
        public string SEQUENCECODE { get; set; }
    }
}
