using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model;

namespace Acc.Business.Model
{
    /// <summary>
    /// 描述：包装管理明细
    /// 作者：路聪
    /// 创建日期:2012-12-28
    /// </summary>
    [EntityClassAttribut("Acc_WMS_PackUnitList", "产品BOM原料", IsOnAppendProperty = true)]
    public class PackUnitList : BasicInfo
    {

        [EntityPrimaryKey]
        [EntityControl("ID", false, true, 0)]
        [EntityField(IsIdentity = true)]
        public new int ID { get; set; }
        /// <summary>
        /// 包装物料编码
        /// </summary>
        [EntityControl("包装管理ID", false, false, 1)]
        [EntityForeignKey(typeof(PackUnit), "ID", "PACKNAME")]
        [EntityField(255)]
        public int PACKUNITID { get; set; }
        /// <summary>
        /// 包装物料编码
        /// </summary>
        [EntityControl("原料名称", false, true, 2)]
        [EntityForeignKey(typeof(BusinessCommodity), "ID", "FNAME")]
        [EntityField(255, IsNotNullable = true)]
        public string PACKUNITCODE { get; set; }
        /// <summary>
        /// 规格型号
        /// </summary>
        [NotSearchAttribut]
        [EntityControl("规格型号", true, true, 3)]
        [EntityField(200)]
        public string FMODEL { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        [EntityControl("数量", false, true, 4)]
        [EntityField(30, IsNotNullable = true)]
        public double NUM { get; set; }
        /// <summary>
        /// 源单编码
        /// </summary>
        [NotSearchAttribut]
        [EntityControl("商品编码", true, true, 1)]
        [EntityField(100)]
        public new string Code { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [EntityControl("排序", false, true, 100)]
        [EntityField(100)]
        public int ORDER { get; set; }


        protected override string GetSearchSQL()
        {
            string n = this.ToString();
            string sql = "select " + n + ".*,Acc_Bus_BusinessCommodity.Code as Code,Acc_Bus_BusinessCommodity.FMODEL as FMODEL  from (" + base.GetSearchSQL() + ") " + n + "  left join Acc_Bus_BusinessCommodity on " + n + ".PACKUNITCODE=Acc_Bus_BusinessCommodity.ID";
            return sql;
        }
    }
}
