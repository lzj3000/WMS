using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;

namespace Acc.Business.Model
{
    [EntityClassAttribut("Acc_Bus_ProcessState", "流程状态")]
    public class ProcessState : BusinessBase
    {
        public ProcessState()
        {
            this.WorkType = 1;
        }
        /// <summary>
        /// 单据ID
        /// </summary>
        [EntityField(50)]
        [EntityForeignKey(typeof(BasicInfo),"ID")]
        public int TaskID { get; set; }
        /// <summary>
        /// 表名称
        /// </summary>
        [EntityField(100)]
        public string TableName { get; set; }
        /// <summary>
        /// 控制器名称
        /// </summary>
        [EntityField(100)]
        public string ControllerName { get; set; }
        /// <summary>
        /// 处理成员描述
        /// </summary>
        [EntityControl("任务描述", true, true, 1)]
        [EntityField(100)]
        public string NodeDesc { get; set; }
        /// <summary>
        /// 节点编码
        /// </summary>
        [EntityField(50)]
        public string NodeCode { get; set; }
        /// <summary>
        /// 处理状态
        /// </summary>
        [EntityControl("处理状态", true, true, 1)]
        [EntityField(50)]
        public string NodeName { get; set; }
        /// <summary>
        /// 提交人名称
        /// </summary>
        [EntityControl("提交人", true, true, 1)]
        [EntityField(50)]
        public string SubmitUser { get; set; }
        /// <summary>
        /// 提交人ID
        /// </summary>
        [EntityField(50)]
        public string SubmitUserID { get; set; }
        /// <summary>
        /// 待处理人名称
        /// </summary>
        [EntityControl("接收人", true, true, 2)]
        [EntityField(50)]
        public string WorkUser { get; set; }
        /// <summary>
        /// 待处理人ID
        /// </summary>
        [EntityField(50)]
        public string WorkUserID { get; set; }
        /// <summary>
        /// 待处理ID的类型,值为3种----1、人员，2、角色，3、组织机构,默认为1人员
        /// </summary>
        [EntityField(50)]
        public int WorkType { get; set; }
        /// <summary>
        /// 是否允许编辑数据
        /// </summary>
        [EntityField(50)]
        public bool IsEditData { get; set; }
        /// <summary>
        /// 操作状态
        /// </summary>
        [EntityControl("操作状态", true, true, 3)]
        [EntityField(50)]
        public string WorkState { get; set; }
        /// <summary>
        /// 到达时间
        /// </summary>
        [EntityControl("到达时间", true, true, 4)]
        [EntityField(50)]
        public DateTime ArrivalTime { get; set; }

        /// <summary>
        /// 处理时间
        /// </summary>
        [EntityControl("处理时间", true, true, 5)]
        [EntityField(50)]
        public DateTime ProcessingTime { get; set; }
        /// <summary>
        /// 处理结果
        /// </summary>
        [EntityControl("备注", true, true, 5)]
        [EntityField(4000)]
        public string Opinion { get; set; }

    }
}
