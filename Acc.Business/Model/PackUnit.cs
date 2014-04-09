using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model;

namespace Acc.Business.Model
{
    /// <summary>
    /// 描述：包装管理
    /// 作者：路聪
    /// 创建日期:2012-12-17
    /// </summary>
    [EntityClassAttribut("Acc_WMS_PackUnit", "产品BOM列表", IsOnAppendProperty = true)]
    public class PackUnit : BasicInfo
    { /// <summary>
        /// 成品项
        /// </summary>
        [EntityControl("成品项", false, true, 1)]
        [EntityForeignKey(typeof(BusinessCommodity), "ID", "FNAME")]
        [EntityField(255, IsNotNullable = true)]
        public int MATERIALID { get; set; }
        /// <summary>
        /// 包装编码
        /// </summary>
        [EntityControl("编码", false, true, 2)]
        //[EntityForeignKey(typeof(Materials), "ID", "FNAME")]
        [EntityField(255, IsNotNullable = true)]
        public string PACKCODE { get; set; }
        /// <summary>
        /// 包装名称
        /// </summary>
        [EntityControl("产品BOM名称", false, true, 3)]
        [EntityField(255, IsNotNullable = true)]
        public string PACKNAME { get; set; }

        private HierarchicalEntityView<PackUnit, PackUnitList> _Materials;
        /// <summary>
        /// 包装物料列表集合
        /// </summary>
        public HierarchicalEntityView<PackUnit, PackUnitList> Materials
        {
            get
            {
                if (_Materials == null)
                {
                    _Materials = new HierarchicalEntityView<PackUnit, PackUnitList>(this);
                    //_Materials.AttachCondition("ID", "ORDERNO");
                    //_Materials.AttachCondition("PRODUCTIONLINE", "BATCHNO");
                }
                return _Materials;
            }
        }
    }
}
