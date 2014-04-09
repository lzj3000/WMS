using System;

using System.Collections.Generic;
using System.Text;
using System.Data;

namespace AcctrueTerminal.Model.BaseData
{    
   public class BasicInfo
    {      
        /// <summary>
        /// 是否提交
        /// </summary>
        public bool ISSUBMITED { get; set; }
        /// <summary>
        /// 提交日期
        /// </summary>
        public string SUBMITEDDATE { get; set; }
        /// <summary>
        /// 提交人
        /// </summary>
        public string SUBMITEDBY { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CREATEDBY { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public string CREATIONDATE { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public string MODIFIEDBY { get; set; }
        /// <summary>
        /// 修改日期
        /// </summary>
        public string MODIFIEDDATE { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        public string REVIEWEDBY { get; set; }
        /// <summary>
        /// 审核日期
        /// </summary>
        public string REVIEWEDDATE { get; set; }
        /// <summary>
        /// 是否审核
        /// </summary>
        public bool ISREVIEWED { get; set; }

        /// <summary>
        /// 代码
        /// </summary>
        public string CODE { get; set; }

        /// <summary>
        /// 是否禁用
        /// </summary>
        public bool ISDISABLE { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool ISDELETE { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string REMARK { get; set; }
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        public int ROWINDEX { get; set; }

        public string ACC_BUS_OFFICEWORKER_WORKNAME_SUBMITEDBY { get; set; }
        public string ACC_BUS_OFFICEWORKER_WORKNAME_CREATEDBY { get; set; }
        public string ACC_BUS_OFFICEWORKER_WORKNAME_MODIFIEDBY { get; set; }
        public string ACC_BUS_OFFICEWORKER_WORKNAME_REVIEWEDBY { get; set; }

    }
}
