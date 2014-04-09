using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;

namespace Acc.Business.Model
{
    /// <summary>
    /// 模块命令
    /// </summary>
    [EntityClassAttribut("Acc_Bus_ModelCommand", "模块命令")]
    public class ModelCommand : BusinessBase
    {
        public ModelCommand()
        {
            this.SortIndex = "index";
        }
        /// <summary>
        /// 所在模块
        /// </summary>
        [EntityControl("模块", true, false, 2)]
        [EntityForeignKey(typeof(SystemModel), "ID", "ModelName")]
        [EntityField(38)]
        public int ParentID { get; set; }
        /// <summary>
        /// 命令
        /// </summary>
        [EntityControl("命令", true, true, 2)]
        [EntityField(50)]
        public string command { get; set; }
        /// <summary>
        /// 显示索引
        /// </summary>
        [EntityControl("显示索引", false, true, 1)]
        [EntityField(50)]
        public int index { get; set; }
        /// <summary>
        /// 显示名称
        /// </summary>
        [EntityControl("显示名称", false, true, 3)]
        [EntityField(50)]
        public string name { get; set; }
        /// <summary>
        /// 显示说明
        /// </summary>
        [EntityControl("显示说明", false, true, 4)]
        [EntityField(50)]
        public string title { get; set; }
        /// <summary>
        /// 是否禁用
        /// </summary>
        [EntityControl("是否禁用", false, true, 5)]
        [EntityField(1)]
        public bool Disabled { get; set; }
        /// <summary>
        /// 是否显示
        /// </summary>
        [EntityControl("是否显示", false, true, 6)]
        [EntityField(1)]
        public bool visible { get; set; }
        /// <summary>
        /// 脚本方法
        /// </summary>
        [EntityControl("脚本方法", false, true, 7)]
        [EntityField(1000)]
        public string onclick { get; set; }
        /// <summary>
        /// 显示图标
        /// </summary>
        [EntityControl("显示图标", false, true, 8)]
        [EntityField(50)]
        public string icon { get; set; }
        /// <summary>
        /// 编辑地址
        /// </summary>
        [EntityControl("编辑地址", false, true, 9)]
        [EntityField(50)]
        public string url { get; set; }
    }
}
