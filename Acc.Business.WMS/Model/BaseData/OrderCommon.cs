using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model;
using Way.EAP.DataAccess.Data;
using System.Data;

namespace Acc.Business.WMS.Model
{
    /// <summary>
    /// 描述：单据基表
    /// 作者：路聪
    /// 创建日期:2013-3-7
    /// </summary>
    public class OrderCommon : BasicInfo
    {
        /// <summary>
        /// 源单ID
        /// </summary>
        [EntityField(100)]
        public virtual int SourceID { get; set; }

        /// <summary>
        /// 源单控制器
        /// </summary>
        [EntityField(100)]
        public string SourceController { get; set; }
        /// <summary>
        /// 源表
        /// </summary>
        [EntityField(1000)]
        public string SourceTable { get; set; }
        /// <summary>
        /// 源名称
        /// </summary>
        [EntityControl("源名称", false, true, 1)]
        [EntityField(100)]
        public string SourceName { get; set; }
        /// <summary>
        /// 外系统源单编码
        /// </summary>
        [EntityControl("外系统源单编码", false, true, 1)]
        [EntityField(100)]
        public string SourceOutCode { get; set; }
        /// <summary>
        /// 客户编号
        /// </summary>
        [EntityControl("往来单位", false, true, 2)]
        [EntityForeignKey(typeof(BusinessCustomer), "ID", "CUSTOMERNAME")]
        [EntityField(255)]
        public string CLIENTNO { get; set; }
        /// <summary>
        /// 销售人员
        /// </summary>
        [EntityControl("销售人员", false, true, 3)]
        [EntityField(255)]
        [EntityForeignKey(typeof(OfficeWorker), "ID", "WorkName")]
        public string WORKERID { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        [EntityControl("部门", false, true, 4)]
        [EntityForeignKey(typeof(Organization), "ID", "OrganizationName")]
        [EntityField(255)]
        public string BUMEN { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        [EntityControl("类型", false, true, 5)]
        [EntityField(12)]
        public int STOCKTYPE { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [EntityControl("状态", false, true, 6)]
        [EntityField(255)]
        [ValueTypeProperty]
        public int STATE { get; set; }
        
        /// <summary>
        /// 是否已同步
        /// </summary>
        [EntityControl("是否已同步", true, false, 7)]
        [EntityField(1)]
        public bool IsSynchronous { get; set; }

        /// <summary>
        /// 完成时间
        /// </summary>
        [EntityControl("完成时间", false, true, 7)]
        [EntityField(255)]
        public DateTime FINISHTIME { get; set; }

        public double GetSourceNum(string c, int rowid)
        {
            string sql = "select SourceController,sourcerowid,sum(num) num from " + this.ToString() + " where SourceController='{0}' and sourcerowid={1} ";
            IDataAction action = this.GetDataAction();
            if (this.ID != 0)
            {
                sql = sql + " and ID<>'" + this.ID + "' ";
            }
            sql = sql + " group by SourceController,sourcerowid ";
            DataTable o = action.GetDataTable(string.Format(sql, c, rowid));
            if (o.Rows.Count > 0)
                return Convert.ToDouble(o.Rows[0]["num"]);
            else
                return 0;
        }

        public double GetNum(int sourcerowid)
        {
            string sql = "select num from " + this.SourceTable + " where id={0}";
            IDataAction action = this.GetDataAction();
            DataTable o = action.GetDataTable(string.Format(sql, sourcerowid));
            if (o.Rows.Count > 0)
                return Convert.ToDouble(o.Rows[0]["num"]);
            else
                return 0;
        }
    }
}
