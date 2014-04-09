using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model;
using Way.EAP.DataAccess.Data;
using System.Data;
using Acc.Contract.MVC;

namespace Acc.Business.WMS.Model
{
    /// <summary>
    /// 描述：单据明细基表
    /// 作者：路聪
    /// 创建日期:2013-10-11
    /// </summary>
    public class OrderMaterialsCommon : BasicInfo
    {
        /// <summary>
        /// 源单ID
        /// </summary>
        [EntityField(100)]
        public int SourceID { get; set; }
        /// <summary>
        /// 源单编码
        /// </summary>
        [EntityControl("源单编码", false, true, 1)]
        //[EntityForeignKey(typeof(OrderCommon), "CODE", "CODE")]
        [EntityField(100)]
        public string SourceCode { get; set; }
        /// <summary>
        /// 源单控制器
        /// </summary>
        [EntityField(100)]
        public string SourceController { get; set; }
        /// <summary>
        /// 源行ID
        /// </summary>
        [EntityField(10)]
        public int SourceRowID { get; set; }
        /// <summary>
        /// 源名称
        /// </summary>
        [EntityField(1000)]
        public string SourceName { get; set; }
        /// <summary>
        /// 源表
        /// </summary>
        [EntityField(1000)]
        public string SourceTable { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        [EntityControl("产品名称", false, true, 2)]
        [EntityForeignKey(typeof(Materials), "ID", "FNAME")]
        [EntityField(255)]
        public string MATERIALCODE { get; set; }
        /// <summary>
        /// 产品编码
        /// </summary>
        //[NotSearchAttribut]
        [EntityControl("产品编码", false, true, 3)]
        [EntityField(255)]
        public virtual string MCODE { get; set; }
        /// <summary>
        /// 规格型号
        /// </summary>
        //[NotSearchAttribut]
        [EntityControl("规格型号", false, true, 4)]
        [EntityField(255)]
        public virtual string FMODEL { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        [EntityControl("数量", false, true, 5)]
        [EntityField(18)]
        public double NUM { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        [EntityControl("单位", false, true, 6)]
        [EntityForeignKey(typeof(Unit), "ID", "UNITNAME")]
        [EntityField(80)]
        public int FUNITID { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        [EntityControl("价格)", false, false, 7)]
        [EntityField(10)]
        public double PRICE { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        [EntityControl("状态", false, false, 8)]
        [EntityField(255)]
        [ValueTypeProperty]
        public int STATE { get; set; }
        /// <summary>
        /// 批次号
        /// </summary>
        [EntityControl("批次号", false, true, 9)]
        [EntityField(255)]
        public string BATCHNO { get; set; }
        /// <summary>
        /// 完成数量
        /// </summary>
        [NotSearchAttribut]
        [EntityControl("完成数量", false, true, 10)]
        [EntityField(18)]
        public double FINISHNUM { get; set; }
        /// <summary>
        /// 待操作数量
        /// </summary>
        [NotSearchAttribut]
        [EntityControl("待操作数量", false, true, 11)]
        [EntityField(18)]
        public double STAYNUM { get; set; }

        /// <summary>
        /// 备用字段6
        /// </summary>
        [EntityControl("备用字段6", false, false, 12)]
        [EntityField(18)]
        public double STAY6 { get; set; }

        /// <summary>
        /// 备用字段7
        /// </summary>
        [EntityControl("备用字段7", false, false, 13)]
        [EntityField(18)]
        public double STAY7 { get; set; }

        //protected override string GetSearchSQL()
        //{
        //    BusinessCommodity bc = new BusinessCommodity();
        //    string n = this.ToString();
        //    string sql = "select " + n + ".*," + bc.ToString() + ".FMODEL as FMODEL," + bc.ToString() + ".Code as MCODE  from (" + base.GetSearchSQL() + ") " + n
        //        + "  left join " + bc.ToString() + " on " + n + ".MATERIALCODE=" + bc.ToString() + ".ID";
        //    return sql;
        //}

        /// <summary>
        /// 得到已经下推总数量
        /// </summary>
        /// <param name="tableName">目标表名</param>
        /// <param name="rowid">目标行的sourceRowID</param>
        /// <param name="materialcode">目标行产品ID</param>
        /// <returns></returns>
        public double GetNum(string tableName,int rowid,string materialcode)
        {
            string sql = "select num from {0} where id={1} and materialcode='{2}'";
            IDataAction action = this.GetDataAction();
            DataTable o = action.GetDataTable(string.Format(sql,tableName, rowid,materialcode));
            if (o.Rows.Count > 0)
                return Convert.ToDouble(o.Rows[0]["num"]);
            else
                return 0;
        }
       
        /// <summary>
        /// 重载方法得到已经下推的数量
        /// </summary>
        /// <param name="tableName">已经下推的目标表名称</param>
        /// <param name="materialCode">物料id</param>
        /// <param name="rowid">行ID</param>
        /// <returns></returns>
        public double GetSourceNum(string tableName, int rowid, string materialCode, int parentid)
        {
            string sql = "select sourcerowid,sum(num) num from " + this.ToString() + " where SourceTable='" + tableName + "' and sourcerowid=" + rowid + " and materialCode= '" + materialCode + "' ";
            IDataAction action = this.GetDataAction();
            if (parentid != 0)
            {
                sql = sql + " and parentid<>'" + parentid + "'";
            }
            sql = sql + " group by SourceRowID ";
            DataTable o = action.GetDataTable(sql);
            if (o.Rows.Count > 0)
                return Convert.ToDouble(o.Rows[0]["num"]);
            else
                return 0;
        }
        /// <summary>
        /// 添加时调用的验证已经完成的数量
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="rowid"></param>
        /// <param name="materialCode"></param>
        /// <returns></returns>
        public double GetSourceNum(string tableName, int rowid, string materialCode)
        {
          return  this.GetSourceNum(tableName, rowid, materialCode, 0);
        }
        /// <summary>
        /// 重载方法得到已经生成明细的数量
        /// </summary>
        /// <param name="tableName">已经下推的目标表名称</param>
        /// <param name="materialCode">物料id</param>
        /// <param name="rowid">行ID</param>
        /// <returns></returns>
        public double GetSourceNum(string tableName,int id, int sourcerowid, string materialCode)
        {
            string sql = "select sourcerowid,sum(num) num from " + this.ToString() + " where SourceTable='" + tableName + "' and sourcerowid=" + sourcerowid + " and materialCode= '" + materialCode + "' ";
            IDataAction action = this.GetDataAction();
            if (this.ID != 0)
            {
                sql = sql + " and ID<>'" + id + "'";
            }
            sql = sql + " group by SourceRowID ";
            DataTable o = action.GetDataTable(sql);
            if (o.Rows.Count > 0)
                return Convert.ToDouble(o.Rows[0]["num"]);
            else
                return 0;
        }

    }
}
