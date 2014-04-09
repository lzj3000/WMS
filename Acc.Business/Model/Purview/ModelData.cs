using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Contract.MVC;
using Way.EAP.DataAccess.Entity;

namespace Acc.Business.Model
{
    /// <summary>
    /// 模块数据
    /// </summary>
    [EntityClassAttribut("Acc_Bus_ModelData", "模块数据")]
    public class SystemModelData : BusinessBase
    {
        public SystemModelData()
        {
            this.SortIndex = "index";
        }
        /// <summary>
        /// 所在模块
        /// </summary>
        [EntityControl("模块", true, false, 2)]
        [EntityForeignKey(typeof(SystemModel), "ID", "ModelName")]
        [EntityField(50)]
        public int ParentID { get; set; }
        /// <summary>
        /// 是否主键
        /// </summary>
        [EntityControl("是否主键", false, false, 3)]
        [EntityField(1)]
        public bool iskey { get; set; }
        /// <summary>
        /// 字段名
        /// </summary>
        [EntityControl("字段名", true, true, 4)]
        [EntityField(80)]
        public string field { get; set; }
        /// <summary>
        /// 显示名称
        /// </summary>
        [EntityControl("显示名称", false, true, 5)]
        [EntityField(80)]
        public string title { get; set; }
        /// <summary>
        /// 显示索引
        /// </summary>
        [EntityControl("显示索引", false, true, 6)]
        [EntityField(80)]
        public int index { get; set; }
        /// <summary>
        /// 是否禁用
        /// </summary>
        [EntityControl("是否禁用", false, true, 7)]
        [EntityField(1)]
        public bool disabled { get; set; }
        /// <summary>
        /// 是否显示
        /// </summary>
        [EntityControl("是否显示", false, true, 8)]
        [EntityField(1)]
        public bool visible { get; set; }
        /// <summary>
        /// 是否编辑
        /// </summary>
        [EntityControl("是否编辑", false, true, 9)]
        [EntityField(1)]
        public bool isedit { get; set; }
        /// <summary>
        /// 显示类型
        /// </summary>
        [EntityControl("显示类型", false, false, 10)]
        [EntityField(50)]
        public string type { get; set; }
        /// <summary>
        /// 是否可空
        /// </summary>
        [EntityControl("是否可空", false, true, 11)]
        [EntityField(1)]
        public bool required { get; set; }
        /// <summary>
        /// 字段长度
        /// </summary>
        [EntityControl("字段长度", false, false, 12)]
        [EntityField(50)]
        public int length { get; set; }
        /// <summary>
        /// 是否查询
        /// </summary>
        [EntityControl("是否查询", false, true, 13)]
        [EntityField(1)]
        public bool issearch { get; set; }
        /// <summary>
        /// 模型名称
        /// </summary>
        [EntityControl("模型名称", false, false, 1)]
        [EntityField(500)]
        public string ModelName { get; set; }

        /// <summary>
        /// 模型显示名称
        /// </summary>
        [EntityControl("模型显示名称", true, true, 2)]
        [EntityField(500)]
        public string TitleName { get; set; }

        /// <summary>
        /// 模型表名
        /// </summary>
        [EntityControl("模型表名", true, false, 10)]
        [EntityField(500)]
        public string ModelTableName { get; set; }
    }
}
