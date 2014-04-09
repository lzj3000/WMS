using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Contract.MVC;

namespace Acc.Business.Model
{
    [EntityClassAttribut("Acc_Bus_TimeTask", "时间任务")]
    public class TimeTask : BusinessBase,IPropertyValueType
    {
       
        /// <summary>
        /// 运行方式,0=定时(每天)，1=循环(分钟)
        /// </summary>
        [ValueTypeProperty]
        [EntityControl("运行方式", false, true, 1)]
        [EntityField(38, IsNotNullable = true)]
        public int RunType { get; set; }
        /// <summary>
        /// 运行值
        /// </summary>
        [EntityControl("运行值", false, true, 2)]
        [EntityField(30, IsNotNullable = true)]
        public string Interval { get; set; }

        /// <summary>
        ///名称 
        /// </summary>
        [EntityField(500, IsNotNullable = true)]
        [EntityControl("名称", false, true, 3)]
        public string TaskName { get; set; }
        /// <summary>
        /// 自动运行类
        /// </summary>
        [EntityControl("自动运行类", false, true, 4)]
        [EntityField(500, IsNotNullable = true)]
        //[EntityForeignKey(typeof(SystemModel), "ControllerName", "ModelName")]
        public string ControllerName { get; set; }

        [EntityControl("运行命令", false, true, 5)]
        [EntityField(500, IsNotNullable = true)]
        public string ControllerCommand { get; set; }

        /// <summary>
        /// 是否启动
        /// </summary>
        [EntityControl("是否启动", true, true, 6)]
        [EntityField(1)]
        public bool IsSubmited { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        [EntityForeignKey(typeof(OfficeWorker), "ID", "WorkName", IsChildAssociate = false)]
        [EntityControl("创建人", true, true, 9)]
        [EntityField(50)]
        public string Createdby { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        [EntityControl("创建日期", true, true, 10)]
        [EntityField(50)]
        public DateTime Creationdate { get; set; }
        /// <summary>
        /// 最后运行时间
        /// </summary>
        [EntityControl("最后运行时间", true, true, 8)]
        [EntityField(50)]
        public DateTime Lastdate { get; set; }
        /// <summary>
        /// 运行状态,0=未运行,1=运行中
        /// </summary>
        [ValueTypeProperty]
        [EntityControl("运行状态", true, true, 7)]
        [EntityField(38)]
        public int RunState { get; set; }
       

        private HierarchicalEntityView<TimeTask, TimeTaskParameter> _parameterDetails;
        /// <summary>
        /// 参数明细
        /// </summary>
        [HierarchicalEntityControl(isadd=false,isremove=false)]
        public HierarchicalEntityView<TimeTask, TimeTaskParameter> ParameterDetails
        {
            get
            {
                if (_parameterDetails == null)
                {
                    _parameterDetails = new HierarchicalEntityView<TimeTask, TimeTaskParameter>(this);
                }
                return _parameterDetails;
            }
        }

      


        private HierarchicalEntityView<TimeTask, TimeTaskNotes> _notesDetails;
        /// <summary>
        /// 记录明细
        /// </summary>
        [HierarchicalEntityControl(disabled = true)]
        public HierarchicalEntityView<TimeTask, TimeTaskNotes> NotesDetails
        {
            get
            {
                if (_notesDetails == null)
                    _notesDetails = new HierarchicalEntityView<TimeTask, TimeTaskNotes>(this);
                return _notesDetails;
            }
           
        }


        #region IPropertyValueType 成员

        public PropertyValueType[] GetValueType(ValueTypeArgs valueArgs)
        {
            List<PropertyValueType> list = new List<PropertyValueType>();
            if (valueArgs.ColumnName == "RunType")
            {
                PropertyValueType p1 = new PropertyValueType();
                p1.Value = "0";
                p1.Text = "定时(每天)";
                PropertyValueType p2 = new PropertyValueType();
                p2.Value = "1";
                p2.Text = "循环(分钟)";
                list.Add(p1);
                list.Add(p2);
            }
            if (valueArgs.ColumnName == "RunState")
            {
                PropertyValueType p1 = new PropertyValueType();
                p1.Value = "0";
                p1.Text = "未运行";
                PropertyValueType p2 = new PropertyValueType();
                p2.Value = "1";
                p2.Text = "运行中";
                list.Add(p1);
                list.Add(p2);
            }
            return list.ToArray();
        }

        #endregion

    }
    [EntityClassAttribut("Acc_Bus_TimeTaskParameter", "任务参数")]
    public class TimeTaskParameter : BusinessBase
    {
        [EntityForeignKey(typeof(TimeTask), "ID", "TaskName")]
        [EntityField(38)]
        public int ParentID { get; set; }
        [EntityControl("参数",true, true, 2)]
        [EntityField(100)]
        public string field { get; set; }
        [EntityControl("参数值", false, true, 3)]
        [EntityField(2000)]
        public string parameterValue { get; set; }
    }
    [EntityClassAttribut("Acc_Bus_TimeTaskNotes", "运行记录")]
    public class TimeTaskNotes : BusinessBase
    {
        [EntityForeignKey(typeof(TimeTask), "ID", "TaskName")]
        [EntityField(38)]
        public int ParentID { get; set; }

        [EntityControl("开始时间", false, true, 1)]
        [EntityField(20)]
        public DateTime StartTime { get; set; }
        [EntityControl("结束时间", false, true, 2)]
        [EntityField(20)]
        public DateTime EndTime { get; set; }
        [EntityControl("是否成功", false, true, 3)]
        [EntityField(1)]
        public bool IsRun { get; set; }
        [EntityControl("运行结果", false, true, 3)]
        [EntityField(4000)]
        public string Results { get; set; }
        [EntityControl("异常信息", false, true, 4)]
        [EntityField(4000)]
        public string ErrorMsg { get; set; }
    }
}
