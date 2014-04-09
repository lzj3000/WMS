using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model;

namespace Acc.Business.WMS.Model
{
    /// <summary>
    /// 物料—包装 关系中间表
    /// </summary>
    [EntityClassAttribut("Acc_WMS_MaterialPack", "产表BOM清单", IsOnAppendProperty = true)]
    public class MaterialPack : BusinessBase
    {
        public MaterialPack[] GetMP()
        {
            return null;
        }
        [EntityControl("物料", false, false, 1)]
        [EntityForeignKey(typeof(Materials), "ID", "FNAME")]
        [EntityField(50)]
        public int MATERIALID { get; set; }

        [EntityControl("BOM名称", false, true, 2)]
        [EntityForeignKey(typeof(PackUnit), "ID", "PACKNAME")]
        [EntityField(50)]
        public int PACKUNITID { get; set; }
    }
}
