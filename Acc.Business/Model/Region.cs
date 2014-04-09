using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Contract.MVC;

namespace Acc.Business.Model
{
    /// <summary>
    /// 地区
    /// </summary>
    [EntityClassAttribut("Acc_Bus_Region", "地区", IsOnAppendProperty = true)]
    public class Region : BasicInfo
    {
        /// <summary>
        /// 名称
        /// </summary>
        [EntityControl("名称", false, true, 2)]
        [EntityField(200)]
        public string RegionName { get; set; }
        /// <summary>
        /// 父ID
        /// </summary>
        [EntityControl("父地区", false, true, 2)]
        [EntityForeignKey(typeof(Region), "ID", "RegionName")]
        [EntityField(50)]
        public int ParentID { get; set; }

        private HierarchicalEntityView<Region, Region> _childItems;

        public HierarchicalEntityView<Region, Region> ChildItems
        {
            get
            {
                if (_childItems == null)
                    _childItems = new HierarchicalEntityView<Region, Region>(this);
                return _childItems;
            }
        }

    }
}
