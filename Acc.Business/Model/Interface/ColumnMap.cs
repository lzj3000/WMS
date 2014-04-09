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
    /// 列映射关系表
    /// </summary>
    [EntityClassAttribut("Acc_Bus_ColumnMap", "列映射关系表", IsOnAppendProperty = true)]
    public class ColumnMap : BusinessBase
    {
        /// <summary>
        /// 列映射关系表
        /// 0 SourceName 源列名
        /// 1 TargetName 目标列名
        /// 2 IsSelect   是否查询（指对源列）
        /// 3 IsInsert   是否插入（指对目标列）
        /// 4 IsUpdate   是否更新（指对目标列）
        /// 5 Deluft     默认值（指对目标列）

        /// <summary>
        /// 映射名称
        /// </summary>
        [EntityControl("源列名", true, true, 0)]
        [EntityField(200, IsNotNullable = true)]
        public string SourceName { get; set; }

        /// <summary>
        /// 目标列名
        /// </summary>
        [EntityControl("目标列名", true, false, 1)]
        [EntityField(200, IsNotNullable = true)]
        public string TargetName { get; set; }

        /// <summary>
        /// 是否查询（指对源列）
        /// </summary>
        [EntityControl("是否查询", true, false, 2)]
        [EntityField(10)]
        public bool IsSelect { get; set; }

        /// <summary>
        /// 是否插入（指对目标列）
        /// </summary>
        [EntityControl("是否插入", true, false, 3)]
        [EntityField(10)]
        public bool IsInsert { get; set; }

        /// <summary>
        /// 是否更新（指对目标列）
        /// </summary>
        [EntityControl("是否更新", true, false, 4)]
        [EntityField(10)]
        public bool IsUpdate { get; set; }


        /// <summary>
        /// 默认值（指对目标列）
        /// </summary>
        [EntityControl("默认值", true, true, 5)]
        [EntityField(200)]
        public string Deluft { get; set; }




        /// <summary>
        /// 表映射关系
        /// </summary>
        [EntityControl("映射关系", true, true, 20)]
        [EntityForeignKey(typeof(MappingInfo), "ID", "MappingInfoName")]
        [EntityField(10, IsNotNullable = true)]
        public int MappingInfoID { get; set; }

    }
}
