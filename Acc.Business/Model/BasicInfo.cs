using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Contract.MVC;

namespace Acc.Business.Model
{
    
    /// <summary>
    /// 基础资料
    /// </summary>
    public class BasicInfo : BusinessBase
    {
        public BasicInfo()
        {
            this.IsAddProperty = true;
        }
        /// <summary>
        /// 是否提交
        /// </summary>
        [EntityControl("是否提交", true, true, 98)]
        [EntityField(1)]
        public bool IsSubmited { get; set; }
        /// <summary>
        /// 提交日期
        /// </summary>
        [EntityControl("提交日期", true, true, 100)]
        [EntityField(50)]
        public DateTime Submiteddate { get; set; }
        /// <summary>
        /// 提交人
        /// </summary>
        [EntityForeignKey(typeof(OfficeWorker), "ID", "WorkName",IsChildAssociate=false)]
        [EntityControl("提交人", true, true, 99)]
        [EntityField(50)]
        public string Submitedby { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        [EntityForeignKey(typeof(OfficeWorker), "ID", "WorkName", IsChildAssociate = false)]
        [EntityControl("创建人", true, true, 96)]
        [EntityField(50)]
        public string Createdby { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        [EntityControl("创建日期", true, true, 97)]
        [EntityField(50)]
        public DateTime Creationdate { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        [EntityForeignKey(typeof(OfficeWorker), "ID", "WorkName", IsChildAssociate = false)]
        [EntityControl("修改人", true, false, 103)]
        [EntityField(50)]
        public string Modifiedby { get; set; }
        /// <summary>
        /// 修改日期
        /// </summary>
        [EntityControl("修改日期", true, false, 104)]
        [EntityField(50)]
        public DateTime Modifieddate { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        [EntityForeignKey(typeof(OfficeWorker), "ID", "WorkName", IsChildAssociate = false)]
        [EntityControl("审核人", true, true, 105)]
        [EntityField(50)]
        public string Reviewedby { get; set; }
        /// <summary>
        /// 审核日期
        /// </summary>
        [EntityControl("审核日期", true, true, 107)]
        [EntityField(50)]
        public DateTime Revieweddate { get; set; }
        /// <summary>
        /// 是否审核
        /// </summary>
        [EntityControl("是否审核", true, true, 104)]
        [EntityField(1)]
        public bool IsReviewed { get; set; }
       
        /// <summary>
        /// 代码
        /// </summary>
        [EntityControl("代码", false, true, 1)]
        [EntityField(100)]
        public string Code { get; set; }
      
        /// <summary>
        /// 是否禁用
        /// </summary>
        [EntityControl("是否禁用", false, true, 95)]
        [EntityField(50)]
        public bool IsDisable { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        [EntityField(50)]
        public bool IsDelete { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [EntityControl("备注", false, true, 150)]
        [EntityField(2000)]
        public string Remark { get; set; }
        /// <summary>
        /// 备用1
        /// </summary>
        [EntityControl("备用1", false, false, 151)]
        [EntityField(2000)]
        public virtual string STAY1 { get; set; }
        /// <summary>
        /// 备用2
        /// </summary>
        [EntityControl("备用2", false, false, 152)]
        [EntityField(2000)]
        public string STAY2 { get; set; }
        /// <summary>
        /// 备用3
        /// </summary>
        [EntityControl("备用3", false, false, 153)]
        [EntityField(2000)]
        public virtual string STAY3 { get; set; }
        /// <summary>
        /// 备用4
        /// </summary>
        [EntityControl("备用4", false, false, 154)]
        [EntityField(2000)]
        public virtual string STAY4 { get; set; }
        /// <summary>
        /// 备用5
        /// </summary>
        [EntityControl("备用5", false, false, 155)]
        [EntityField(2000)]
        public virtual string STAY5 { get; set; }

        private HierarchicalEntityView<BasicInfo, ProcessState> _processStateItems;

        [HierarchicalEntityControl(isparent=true,disabled=true,visible=false)]
        public HierarchicalEntityView<BasicInfo, ProcessState> ProcessStateItems
        {
            get
            {
                if (_processStateItems == null)
                {
                    _processStateItems = new HierarchicalEntityView<BasicInfo, ProcessState>(this);
                    _processStateItems.ChildCondition("TableName", "='" + this.ToString() + "'");
                }
                return _processStateItems;
            }
        }
       
    }
}
