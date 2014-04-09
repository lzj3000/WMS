using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model;

namespace Acc.Business.WMS.Model
{
    /// <summary>
    /// 描述：批次变更明细
    /// 作者：柳强
    /// 创建日期:2013-06-06
    /// </summary>
    [EntityClassAttribut("Acc_WMS_BatchChange", "库存批次管理", IsOnAppendProperty = true)]
    public class BatchChange : BasicInfo
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
        private HierarchicalEntityView<BatchChange, BatchChangeMaterials> _childItems;

        public HierarchicalEntityView<BatchChange, BatchChangeMaterials> ChildItems
        {
            get
            {
                if (_childItems == null)
                    _childItems = new HierarchicalEntityView<BatchChange, BatchChangeMaterials>(this);
                return _childItems;
            }
        }
    }
}
