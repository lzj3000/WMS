using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model;

namespace Acc.Business.WMS.Model
{
    /// <summary>
    /// 描述：包装出入库管理
    /// 作者：路聪
    /// 创建日期:2012-12-25
    /// </summary>
    [EntityClassAttribut("Acc_WMS_PackUnitRecord", "包装出入库管理", IsOnAppendProperty = true)]
    class PackUnitRecord : BasicInfo
    {
        /// <summary>
        /// 包装名称
        /// </summary>
        [EntityControl("包装名称", false, true, 1)]
        [EntityForeignKey(typeof(PackUnit), "ID", "PACKNAME")]
        [EntityField(255, IsNotNullable = true)]
        public string PACKNAME { get; set; }
        /// <summary>
        ///数量
        /// </summary>
        [EntityControl("数量", false, true, 2)]
        [EntityField(255, IsNotNullable = true)]
        public float NUM { get; set; }
    }
}
