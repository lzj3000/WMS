using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;

namespace Acc.Business.Model.Interface
{
    /// <summary>
    /// 数据关系表
    /// </summary>
    [EntityClassAttribut("Acc_Bus_MappingData", "数据关系记录", IsOnAppendProperty = true)]
    public class MappingData : BusinessBase
    {
        /// <summary>
        /// 映射关系
        /// </summary>
        [EntityControl("映射关系", true, true, 0)]
        [EntityForeignKey(typeof(MappingInfo), "ID", "MappingInfoName")]
        [EntityField(10, IsNotNullable = true)]
        public int MappingInfoID { get; set; }


        /// <summary>
        /// 源系统
        /// </summary>
        [EntityControl("源系统", true, true, 1)]
        [EntityField(50, IsNotNullable = true)]
        public string SourceSystem { get; set; }

        /// <summary>
        /// 目标系统
        /// </summary>
        [EntityControl("目标系统", true, true, 2)]
        [EntityField(50, IsNotNullable = true)]
        public string TargetSystem { get; set; }

        /// <summary>
        /// 源表名
        /// </summary>
        [EntityControl("源表名", true, false, 3)]
        [EntityField(50, IsNotNullable = true)]
        public string SourceTable { get; set; }

        /// <summary>
        /// 目标表名
        /// </summary>
        [EntityControl("目标表名", true, false, 4)]
        [EntityField(50, IsNotNullable = true)]
        public string TargetTable { get; set; }

        /// <summary>
        /// 源数据ID
        /// </summary>
        [EntityControl("源数据ID", true, false, 5)]
        [EntityField(50, IsNotNullable = true)]
        public int SourceID { get; set; }

        /// <summary>
        /// 目标数据ID
        /// </summary>
        [EntityControl("目标数据ID", true, false, 6)]
        [EntityField(50, IsNotNullable = true)]
        public int TargetID { get; set; }

        /// <summary>
        /// 源数据编号
        /// </summary>
        [EntityControl("源数据编号", true, true, 7)]
        [EntityField(50)]
        public string SourceCode { get; set; }

        /// <summary>
        /// 目标数据编号
        /// </summary>
        [EntityControl("目标数据编号", true, true, 8)]
        [EntityField(50)]
        public string TargetCode { get; set; }


        /// <summary>
        /// 操作日期
        /// </summary>
        [EntityControl("操作日期", true, true, 19)]
        [EntityField(50, IsNotNullable = true)]
        public DateTime OperateDate { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [EntityControl("描述", true, true, 20)]
        [EntityField(2000)]
        public string Remark { get; set; }

        /// <summary>
        /// 记录数据关系
        /// </summary>
        public void AddMappingData(MappingData md)
        {
            if (md != null)
            {
                md.OperateDate = DateTime.Now;
                string sqlMapping = string.Format("insert Acc_Bus_MappingData(MappingInfoID,SourceSystem,TargetSystem,SourceTable,TargetTable,SourceID,TargetID,SourceCode,TargetCode,OperateDate,Remark) values({0},'{1}','{2}','{3}','{4}',{5},{6},'{7}','{8}','{9}','{10}')"
                    , md.MappingInfoID, md.SourceSystem, md.TargetSystem, md.SourceTable, md.TargetTable, md.SourceID, md.TargetID, md.SourceCode, md.TargetCode, md.OperateDate, md.Remark);
                this.GetDataAction().Execute(sqlMapping);
            }
        }


    }
}
