using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model;


namespace Acc.Business.WMS.Model
{
    /// <summary>
    /// 描述：入库序列码表
    /// 作者：路聪
    /// 创建日期:2012-12-17
    /// </summary>
    [EntityClassAttribut("Acc_WMS_OutInSequence", "出库单件码表", IsOnAppendProperty = false)]
    public class OutInSequence : BasicInfo
    {

        /// <summary>
        /// 出库单物料
        /// </summary>
        [EntityControl("出库单物料", false, false, 1)]
        [EntityForeignKey(typeof(StockOutOrderMaterials), "ID", "MATERIALCODE")]
        [EntityField(20)]
        public int OutOrderMATERIALID { get; set; }

        
        /// <summary>
        /// 序列码
        /// </summary>
        [EntityControl("单件码", false, true, 2)]
        [EntityField(200)]
        public string SEQUENCECODE { get; set; }

    }
}
