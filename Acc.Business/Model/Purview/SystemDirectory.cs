using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Contract.MVC;

namespace Acc.Business.Model
{
    /// <summary>
    /// 系统目录
    /// </summary>
    [EntityClassAttribut("Acc_Bus_SystemDirectory", "系统目录",IsOnAppendProperty=true)]
    public class SystemDirectory : BusinessBase
    {
        public SystemDirectory()
        {
            this.SortIndex = "OrderIndex";
        }
        /// <summary>
        /// 名称
        /// </summary>
        [EntityControl("名称", false, true, 1)]
        [EntityField(200)]
        public string DirectoryName { get; set; }

        [EntityControl("父目录", false, true, 2)]
        [EntityForeignKey(typeof(SystemDirectory), "ID", "DirectoryName")]
        [EntityField(50)]
        public int ParentID { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [EntityControl("排序索引", false, true, 3)]
        [EntityField(50)]
        public int OrderIndex { get; set; }

        private HierarchicalEntityView<SystemDirectory, SystemDirectory> _childItems;

        public HierarchicalEntityView<SystemDirectory, SystemDirectory> ChildItems
        {
            get
            {
                if (_childItems == null)
                    _childItems = new HierarchicalEntityView<SystemDirectory, SystemDirectory>(this);
                return _childItems;
            }
        }
        private HierarchicalEntityView<SystemDirectory, SystemModel> _modelItems;
        [HierarchicalEntityControl(isadd = false,isremove=false, isselect = true, c = "Acc.Business.Controllers.SystemModelController")]
        public HierarchicalEntityView<SystemDirectory, SystemModel> ModelItems
        {
            get
            {
                if (_modelItems == null)
                {
                    _modelItems = new HierarchicalEntityView<SystemDirectory, SystemModel>(this);
                    _modelItems.IsAssociateDelete = false;
                }
                return _modelItems;
            }
        }
    }
}
