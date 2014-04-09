using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model;

namespace Acc.Business.WMS.Model
{
    /// <summary>
    /// 物料—单位 关系中间表
    /// </summary>
    [EntityClassAttribut("Acc_WMS_MaterialUnit", "物料单位", IsOnAppendProperty = true)]
    public class MaterialUnit : BusinessBase
    {
        public MaterialUnit[] GetMU()
        {
            return null;
        }
        [EntityControl("物料", false, false, 1)]
        [EntityForeignKey(typeof(Materials), "ID", "FNAME")]
        [EntityField(50)]
        public int MATERIALID { get; set; }

        [EntityControl("单位", false, true, 2)]
        [EntityForeignKey(typeof(Acc.Business.Model.Unit), "ID", "UnitName")]
        [EntityField(50)]
        public int UNITID { get; set; }
    }
}
