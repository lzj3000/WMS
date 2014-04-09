using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Contract.MVC;
using Way.EAP.DataAccess.Entity;

namespace Acc.Business.Model
{
   public class ProcessStateView:BusinessBase
    {
        /// <summary>
        /// 单据ID
        /// </summary>
         [EntityControl("待处理单据", true, true, 6)]
       [EntityPrimaryKey]
        public int TaskID { get; set; }

        /// <summary>
        /// 处理成员描述
        /// </summary>
        [EntityControl("任务描述", true, true, 1)]
        public string NodeDesc { get; set; }

        /// <summary>
        /// 处理状态
        /// </summary>
        [EntityControl("处理状态", true, true, 2)]
        public string NodeName { get; set; }
        /// <summary>
        /// 提交人名称
        /// </summary>
        [EntityControl("提交人", true, true,3)]
        public string SubmitUser { get; set; }

        /// <summary>
        /// 操作状态
        /// </summary>
        [EntityControl("操作状态", true, true, 5)]
        public string WorkState { get; set; }
        /// <summary>
        /// 到达时间
        /// </summary>
        [EntityControl("到达时间", true, true, 6)]    
        public DateTime ArrivalTime { get; set; }

        /// <summary>
        /// 处理结果
        /// </summary>
        [EntityControl("备注", true, true, 7)]
         public string Opinion { get; set; }

        /// <summary>
        /// 处理结果
        /// </summary>
        [EntityControl("页面", true, true, 6)]
        public string URL { get; set; }

       [EntityControl("处理时间", true, true, 6)]   
        public DateTime ProcessingTime { set; get; }

        protected override string GetSearchSQL()
        {
            return "SELECT distinct t.TaskID,t.WorkUserId,t.ProcessingTime, t.NodeDesc,t.NodeName,t.SubmitUser,t.WorkState,t.ArrivalTime,t1.Url,t.Opinion FROM Acc_Bus_ProcessState t Left join Acc_Bus_SystemModel t1 on t.controllerName=t1.controllerName";
        }
    }
}
