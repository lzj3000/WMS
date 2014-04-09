using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model.Purview;
using Acc.Contract.MVC;
using System.Xml.Schema;
using Acc.Business.Model;

namespace Acc.Business.WMS.Model
{
    /// <summary>
    /// 职员--继承business职员
    /// </summary>
   [EntityClassAttribut("Acc_WMS_OWorker", "职员", IsOnAppendProperty = true)]
    public class OWorker : BasicInfo
    {
        /// <summary>
        /// 父ID
        /// </summary>
        [EntityControl("管理仓库", false, true, 3)]
        [EntityForeignKey(typeof(WareHouse), "ID", "WAREHOUSENAME")]
        [EntityField(50)]
        public int WHID { get; set; }

        /// <summary>
        /// 职员ID
        /// </summary>
        [EntityControl("管理员", false, true, 4)]
        [EntityForeignKey(typeof(OfficeWorker), "ID", "WorkName")]
        [EntityField(50)]
        public int WorkName { get; set; }
    }
}
