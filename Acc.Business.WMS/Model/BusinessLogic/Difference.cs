using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model;

namespace Acc.Business.WMS.Model
{
    /// <summary>
    /// 描述：盘点差异表
    /// 作者：柳强
    /// 创建日期:2013-06-07
    /// </summary>
    [EntityClassAttribut("Acc_WMS_Difference", "盘点差异表", IsOnAppendProperty = true)]
    public class Difference : BasicInfo
    {
        /// <summary>
        /// 源单数据模型
        /// </summary>
        [EntityField(100)]
        public string SourceID { get; set; }
        /// <summary>
        /// 源单控制器
        /// </summary>
        [EntityField(100)]
        public string SourceController { get; set; }
        /// <summary>
        /// 源单编码
        /// </summary>
        [EntityControl("源单编码", false, false, 1)]
        [EntityField(100)]
        public string SourceCode { get; set; }


        /// <summary>
        /// 盘点仓库
        /// </summary>
        [EntityControl("仓库", false, true, 5)]
        [EntityForeignKey(typeof(WareHouse), "ID", "WAREHOUSENAME")]
        [EntityField(10)]
        public string TOWHNO { get; set; }
        //private HierarchicalEntityView<Difference, DifferenceMaterials> _childItems;

        //public HierarchicalEntityView<Difference, DifferenceMaterials> ChildItems
        //{
        //    get
        //    {
        //        if (_childItems == null)
        //            _childItems = new HierarchicalEntityView<Difference, DifferenceMaterials>(this);
        //        return _childItems;
        //    }
        //}

        private HierarchicalEntityView<Difference, DifferenceMaterials> _Materials;
        /// <summary>
        /// 盘点差异物料集合
        /// </summary>
        public HierarchicalEntityView<Difference, DifferenceMaterials> Materials
        {
            get
            {
                if (_Materials == null)
                    _Materials = new HierarchicalEntityView<Difference, DifferenceMaterials>(this);
                return _Materials;
            }
        }

    }
}
