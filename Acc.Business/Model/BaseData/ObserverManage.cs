using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Contract.MVC;
using Way.EAP.DataAccess.Entity;

namespace Acc.Business.Model
{
    /// <summary>
    /// 观察器模型
    /// </summary>
    [EntityClassAttribut(false)]
    public class ObserverManage : ModelBase
    {
        [EntityPrimaryKey]
        [EntityControl("ID", false, false, 1)]
        [EntityField(200)]
        public string ID { get; set; }
        /// <summary>
        /// 源控制器
        /// </summary>
        [EntityControl("源控制器", false, true, 1)]
        [EntityField(200)]
        public string SourceController { get; set; }

        /// <summary>
        /// 目标控制器
        /// </summary>
        [EntityControl("目标控制器", false, true, 2)]
        [EntityField(200)]
        public string ControllerName { get; set; }
        /// <summary>
        /// 是否异步
        /// </summary>
        [EntityControl("是否异步", false, true, 6)]
        [EntityField(200)]
        public bool IsAsynchronous { get; set; }
        /// <summary>
        /// 是否异常中断
        /// </summary>
        [EntityControl("是否异常中断", false, true, 5)]
        [EntityField(200)]
        public bool IsErrorStop { get; set; }
        /// <summary>
        /// 客户要观察的控制器
        /// </summary>
        [EntityControl("事件", false, true, 3)]
        [EntityField(200)]
        public string MethodName { get; set; }
        /// <summary>
        /// 观察类型
        /// </summary>
        [EntityControl("观察类型", false, true, 4)]
        [EntityField(200)]
        public string ObserverType { get; set; }

    }
}
