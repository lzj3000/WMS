using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Contract.MVC;
using Way.EAP.DataAccess.Data;

namespace Acc.Business.Model.Interface
{
    /// <summary>
    /// 数据关系表
    /// </summary>
    [EntityClassAttribut("Acc_Bus_MappingInfo", "映射关系",IsOnAppendProperty=true)]
    public class MappingInfo : BusinessBase
    {
        /// <summary>
        /// 接口模版
        /// </summary>
        [EntityControl("接口模版", true, true, 0)]
        [EntityForeignKey(typeof(InterfaceTemplate), "ID", "TemplateName")]
        [EntityField(10, IsNotNullable = true)]
        public int TemplateID { get; set; }

        /// <summary>
        /// 映射名称
        /// </summary>
        [EntityControl("映射名称", true, true, 1)]
        [EntityField(200, IsNotNullable = true)]
        public string MappingInfoName { get; set; }

        /// <summary>
        /// 关系映射文件名（xml）
        /// </summary>
        [EntityControl("关系映射文件名（xml）", true, false, 2)]
        [EntityField(200, IsNotNullable = true)]
        public string XmlName { get; set; }

        /// <summary>
        /// 源系统
        /// </summary>
        [EntityControl("源系统", true, true, 3)]
        [EntityField(50, IsNotNullable = true)]
        public string SourceSystem { get; set; }

        /// <summary>
        /// 目标系统
        /// </summary>
        [EntityControl("目标系统", true, true, 4)]
        [EntityField(50, IsNotNullable = true)]
        public string TargetSystem { get; set; }

        /// <summary>
        /// 源表名
        /// </summary>
        [EntityControl("源表名", true, false, 5)]
        [EntityField(50, IsNotNullable = true)]
        public string SourceTable { get; set; }

        /// <summary>
        /// 目标表名
        /// </summary>
        [EntityControl("目标表名", true, false, 6)]
        [EntityField(50, IsNotNullable = true)]
        public string TargetTable { get; set; }

        /// <summary>
        /// 源模型名称（全名称）
        /// </summary>
        [EntityControl("源模型名称", true, false, 7)]
        [EntityField(200)]
        public string SourceMODEL { get; set; }

        /// <summary>
        /// 目标模型名称（全名称）
        /// </summary>
        [EntityControl("目标模型名称", true, false, 8)]
        [EntityField(200)]
        public string TargetMODEL { get; set; }

        /// <summary>
        /// 转换数量
        /// </summary>
        [EntityControl("转换数量", true, true, 15)]
        [EntityField(10)]
        public int TransformCount { get; set; }

        /// <summary>
        /// 成功转换数量
        /// </summary>
        [EntityControl("成功转换数量", true, true, 8)]
        [EntityField(10)]
        public int SuccessCount { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [EntityControl("描述", true, true, 20)]
        [EntityField(2000)]
        public string Remark { get; set; }

        /// <summary>
        /// 记录k3本次更新最大时间戳
        /// </summary>
        [EntityControl("本次更新最大时间戳", true, false, 21)]
        [EntityField(32)]
        public int maxTimestamp { get; set; }

        /// <summary>
        /// 记录k3本次更新最大时间
        /// </summary>
        [EntityControl("本次更新最后日期", true, false, 19)]
        [EntityField(50)]
        public DateTime maxUpdateTime { get; set; }

        /// <summary>
        /// 是否是单据,0是基础数据，1是单据
        /// </summary>
        [EntityControl("是否是单据", true, false, 22)]
        [EntityField(1)]
        public int ISORDER { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        [EntityControl("是否显示", true, false, 30)]
        [EntityField(50)]
        public string Visible { get; set; }

        /// <summary>
        /// 父ID
        /// </summary>
        [EntityControl("上级映射", true, false, 31)]
        [EntityForeignKey(typeof(MappingInfo), "ID", "MappingInfoName")]
        [EntityField(50)]
        public int ParentID { get; set; }


        

        /// <summary>
        /// 数据关系集合-列映射关系表
        /// </summary>
        private HierarchicalEntityView<MappingInfo, MappingInfo> _MapChild;

        //[HierarchicalEntityControl(visible = false)]
        [HierarchicalEntityControl(isremove = false, isadd = false, isedit = false)]
        public HierarchicalEntityView<MappingInfo, MappingInfo> MapChild
        {
            get
            {
                if (_MapChild == null)
                    _MapChild = new HierarchicalEntityView<MappingInfo, MappingInfo>(this);
                return _MapChild;
            }
        }

        /// <summary>
        /// 数据关系集合-数据
        /// </summary>
        private HierarchicalEntityView<MappingInfo, MappingData> _MapItems;

        //[HierarchicalEntityControl(visible = false)]
        [HierarchicalEntityControl(isremove = false, isadd = false, isedit = false)]
        public HierarchicalEntityView<MappingInfo, MappingData> MapItems
        {
            get
            {
                if (_MapItems == null)
                    _MapItems = new HierarchicalEntityView<MappingInfo, MappingData>(this);
                return _MapItems;
            }
        }



        ///// <summary>
        ///// 数据关系集合-列映射关系表
        ///// </summary>
        //private HierarchicalEntityView<MappingInfo, ColumnMap> _MapColumns;

        //[HierarchicalEntityControl(visible = false)]
        ////[HierarchicalEntityControl(isremove = false,isadd=false,isedit=false)]
        //public HierarchicalEntityView<MappingInfo, ColumnMap> MapColumns
        //{
        //    get
        //    {
        //        if (_MapColumns == null)
        //            _MapColumns = new HierarchicalEntityView<MappingInfo, ColumnMap>(this);
        //        return _MapColumns;
        //    }
        //}

    }
}
