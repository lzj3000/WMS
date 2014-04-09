using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model;

namespace Acc.Business.WMS.Model
{
    /// <summary>
    /// 描述：移位单
    /// 作者：路聪
    /// 创建日期:2013-2-27
    /// </summary>
    [EntityClassAttribut("Acc_WMS_MoveOrder", "移位单管理", IsOnAppendProperty = true)]
    public class MoveOrder : BasicInfo
    {
        /// <summary>
        /// 目标位置
        /// </summary>
        [EntityControl("移位仓库", false, true, 7)]
        [EntityForeignKey(typeof(WareHouse), "ID", "WAREHOUSENAME")]
        [EntityField(38)]
        public int DEPOTWBS { get; set; }

        private HierarchicalEntityView<MoveOrder, MoveOrderMaterials> _Materials;
        /// <summary>
        /// 入库单入库物料集合
        /// </summary>
        public HierarchicalEntityView<MoveOrder, MoveOrderMaterials> Materials
        {
            get
            {
                if (_Materials == null)
                {
                    _Materials = new HierarchicalEntityView<MoveOrder, MoveOrderMaterials>(this);
                }
                return _Materials;
            }
        }
    }
}
