using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model;
using Acc.Business.Model.Interface;

namespace Acc.Business.Model.Interface
{
    /// <summary>
    /// 数据关系表
    /// </summary>
    [EntityClassAttribut("Acc_Bus_LogData", "同步日志", IsOnAppendProperty = true)]
    public class LogData : BusinessBase
    {
        /// <summary>
        /// 同步单据
        /// </summary>
        [EntityControl("同步单据", true, true, 0)]
        [EntityField(200, IsNotNullable = true)]
        public string LogName { get; set; }

        /// <summary>
        /// 关系映射文件名（xml）
        /// </summary>
        [EntityControl("关系映射文件名（xml）", true, false, 2)]
        [EntityField(200, IsNotNullable = true)]
        public string XmlName { get; set; }

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
        [EntityControl("源单据编号", true, true, 7)]
        [EntityField(50)]
        public string SourceCode { get; set; }

        /// <summary>
        /// 目标数据编号
        /// </summary>
        [EntityControl("目标单据编号", true, true, 8)]
        [EntityField(50)]
        public string TargetCode { get; set; }


        /// <summary>
        /// 操作日期
        /// </summary>
        [EntityControl("日志时间", true, true, 19)]
        [EntityField(50, IsNotNullable = true)]
        public DateTime OperateDate { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [EntityControl("描述", true, true, 20)]
        [EntityField(2000)]
        public string Remark { get; set; }

        /// <summary>
        /// 是否同步成功
        /// </summary>
        [EntityControl("是否同步成功", true, false, 6)]
        [EntityField(10, IsNotNullable = true)]
        public int IsOk { get; set; }

        /// <summary>
        /// 记录数据日志
        /// </summary>
        public void AddLogData(LogData log)
        {
            if (log != null)
            {
                log.OperateDate = DateTime.Now;
                string sqlLog = string.Format("insert Acc_Bus_LogData(LogName,SourceSystem,TargetSystem,SourceTable,TargetTable,SourceID,TargetID,SourceCode,TargetCode,OperateDate,Remark,XmlName,IsOk) values('{0}','{1}','{2}','{3}','{4}',{5},{6},'{7}','{8}','{9}','{10}','{11}',{12})"
                    , log.LogName, log.SourceSystem, log.TargetSystem, log.SourceTable, log.TargetTable
                    , log.SourceID, log.TargetID, log.SourceCode, log.TargetCode, log.OperateDate
                    , log.Remark, log.XmlName, log.IsOk);
                this.GetDataAction().Execute(sqlLog);
            }
        }


        /// <summary>
        /// 记录数据日志
        /// </summary>
        public void AddLogData(LogData log, MappingData md)
        {
            if (md != null)
            {
                md.OperateDate = DateTime.Now;
                string sqlLog = string.Format("insert Acc_Bus_LogData(LogName,SourceSystem,TargetSystem,SourceTable,TargetTable,SourceID,TargetID,SourceCode,TargetCode,OperateDate,Remark,XmlName,IsOk) values('{0}','{1}','{2}','{3}','{4}',{5},{6},'{7}','{8}','{9}','{10}','{11}',{12})"
                    , log.LogName, md.SourceSystem, md.TargetSystem, md.SourceTable, md.TargetTable
                    , md.SourceID, md.TargetID, md.SourceCode, md.TargetCode, md.OperateDate
                    , log.Remark, log.XmlName, log.IsOk);
                this.GetDataAction().Execute(sqlLog);
            }
        }
    }
}
