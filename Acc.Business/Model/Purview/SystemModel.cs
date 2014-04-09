using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Contract.MVC;
using Newtonsoft.Json;

namespace Acc.Business.Model
{
    /// <summary>
    /// 系统模块
    /// </summary>
    [EntityClassAttribut("Acc_Bus_SystemModel", "系统模块",IsOnAppendProperty=true)]
    public class SystemModel : BusinessBase
    {
        public SystemModel()
        {
            this.SortIndex = "OrderIndex";
        }
        /// <summary>
        /// 名称
        /// </summary>
        [EntityControl("名称", false, true, 2)]
        [EntityField(200)]
        public string ModelName { get; set; }

        [EntityControl("目录", false, true, 2)]
        [EntityForeignKey(typeof(SystemDirectory), "ID", "DirectoryName")]
        [EntityField(50)]
        public int SystemDirectoryID { get; set; }
        /// <summary>
        /// 控制器名称
        /// </summary>
        [EntityControl("控制器", true, true, 3)]
        [EntityField(500)]
        public string ControllerName { get; set; }
        /// <summary>
        /// Url路径
        /// </summary>
        [EntityControl("Url路径", false, true, 4)]
        [EntityField(100)]
        public string Url { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [EntityControl("排序索引", false, true, 5)]
        [EntityField(50)]
        public int OrderIndex { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [EntityField(2000)]
        public string Remark { get; set; }
        /// <summary>
        /// 是否禁用
        /// </summary>
        [EntityField(1)]
        public bool IsDisable { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        [JsonIgnore]
        [EntityField(500)]
        public string FilePath { get; set; }

        private HierarchicalEntityView<SystemModel, SystemModelData> _dataItems;
        [HierarchicalEntityControl(isadd = false, isremove = false)]
        public HierarchicalEntityView<SystemModel, SystemModelData> DataItems
        {
            get
            {
                if (_dataItems == null)
                    _dataItems = new HierarchicalEntityView<SystemModel, SystemModelData>(this);
                return _dataItems;
            }
        }
        private HierarchicalEntityView<SystemModel, ModelCommand> _commandItems;
        [HierarchicalEntityControl(isadd = false, isremove=false)]
        public HierarchicalEntityView<SystemModel, ModelCommand> CommandItems
        {
            get
            {
                if (_commandItems == null)
                    _commandItems = new HierarchicalEntityView<SystemModel, ModelCommand>(this);
                return _commandItems;
            }

        }
    }
}
