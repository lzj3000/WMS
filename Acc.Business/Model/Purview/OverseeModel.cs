using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Contract.MVC;
using Way.EAP.DataAccess.Entity;

namespace Acc.Business.Model.Purview
{
    /// <summary>
    /// 监视模型
    /// </summary>
    [EntityClassAttribut("Acc_Bus_OverseeModel", "监视追踪管理")]
    public class OverseeModel : BusinessBase
    {
        /// <summary>
        /// 操作用户
        /// </summary>
        [EntityControl("用户", false, true, 1)]
        [EntityField(100)]
        public string User { get; set; }
        /// <summary>
        /// 控制器名称
        /// </summary>
        [EntityControl("控制器", false, true, 2)]
        [EntityField(500)]
        public string Controller { get; set; }
        /// <summary>
        /// 方法
        /// </summary>
        [EntityControl("方法", false, true, 3)]
        [EntityField(100)]
        public string MethodName { get; set; }

        /// <summary>
        /// 操作状态
        /// </summary>
        [EntityControl("状态", false, true, 4)]
        [EntityField(100)]
        public string State { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        [EntityControl("开始时间", false, true, 5)]
        [EntityField(100)]
        public string StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        [EntityControl("结束时间", false, true, 6)]
        [EntityField(100)]
        public string StopTime { get; set; }
        /// <summary>
        /// 耗时
        /// </summary>
        [EntityControl("耗时", false, true, 7)]
        [EntityField(100)]
        public string Time { get; set; }
        /// <summary>
        /// 异常信息
        /// </summary>
        [EntityControl("异常信息", false, true, 8)]
        [EntityField(1000)]
        public string ErrorMsg { get; set; }

        HierarchicalEntityView<OverseeModel, OverseeModelDetails> _details;

        public HierarchicalEntityView<OverseeModel, OverseeModelDetails> Details
        {
            get
            {
                if (_details == null)
                    _details = new HierarchicalEntityView<OverseeModel, OverseeModelDetails>(this);
                return _details;
            }
        }
       
    }

    [EntityClassAttribut("Acc_Bus_OverseeDetails", "监视追踪管理明细")]
    public class OverseeModelDetails : BusinessBase
    {
        [EntityForeignKey(typeof(OverseeModel), "ID")]
        public int ParentID { get; set; }

        [EntityControl("表名称", false, true, 1)]
        [EntityField(1000)]
        public string TableName { get; set; }
        [EntityControl("操作类型", false, true, 2)]
        [EntityField(50)]
        public string OperateType { get; set; }
        [EntityControl("执行索引", false, true, 3)]
        [EntityField(30)]
        public int ExecuteIndex { get; set; }
        [EntityControl("执行SQL", false, true, 4)]
        [EntityField(2000)]
        public string ExecuteSQL { get; set; }
        [EntityControl("撤销SQL", false, true, 5)]
        [EntityField(2000)]
        public string CancelSQL { get; set; }
    }
}
