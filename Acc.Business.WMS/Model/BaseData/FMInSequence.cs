using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model;


namespace Acc.Business.WMS.Model
{
    /// <summary>
    /// 描述：生产赋码单件码表
    /// 作者：柳强
    /// 创建日期:2013-11-04
    /// </summary>
    [EntityClassAttribut("Acc_WMS_FMInSequence", "生产赋码单件码表", IsOnAppendProperty = false)]
    public class FMInSequence : BasicInfo
    {
      
        /// <summary>
        /// 入库单编码
        /// </summary>
        [EntityControl("入库通知源单ID", false, false, 1)]
        [EntityForeignKey(typeof(StockInNoticeMaterials1), "ID", "Code")]
        [EntityField(10)]
        public int InNoticeMaterialsSourceID { get; set; }

     
        /// <summary>
        /// 序列码
        /// </summary>
        [EntityControl("序列码", false, true, 2)]
        [EntityField(200)]
        public string SEQUENCECODE { get; set; }

    }
}
