using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model;
using Acc.Contract.MVC;
using Way.EAP.DataAccess.Data;
using System.Data;

namespace Acc.Business.WMS.Model
{
    /// <summary>
    /// 描述：出库单明细表
    /// 作者：柳强
    /// 创建日期:2012-12-21
    /// </summary>
    [EntityClassAttribut("Acc_WMS_OutOrderMaterials", "出库单明细", IsOnAppendProperty = true)]
    public class StockOutOrderMaterials : OrderMaterialsCommon
    {
        public StockOutOrderMaterials[] getsi()
        {
            return null;
        }
        /// <summary>
        /// 来源单号
        /// </summary>
        [EntityControl("来源单号", true, true, 1)]
        [EntityForeignKey(typeof(StockOutOrder), "ID", "CODE")]
        [EntityField(8)]
        public int PARENTID { get; set; }
        /// <summary>
        /// 亚批次
        /// </summary>
        [EntityControl("亚批次", false, true, 12)]
        [EntityField(255)]
        public string SENBATCHNO { get; set; }
        /// <summary>
        /// 货位
        /// </summary>
        [EntityControl("货位", false, true, 13)]
        [EntityField(255)]
        [EntityForeignKey(typeof(WareHouse), "ID", "Code")]
        public string DEPOTWBS { get; set; }
        /// <summary>
        /// 托盘
        /// </summary>
        [EntityControl("托盘", false, true, 14)]
        [EntityField(255)]
        [EntityForeignKey(typeof(Ports), "ID", "PORTNO")]
        public string PORTNAME { get; set; }
        /// <summary>
        /// 测试关联质检单
        /// </summary>
        [EntityControl("是否质检", false, true, 15)]
        [EntityField(255)]
        [NotSearchAttribut]
        public string IsCheckOk { get; set; }

        protected override string GetSearchSQL()
        {
            ProCheckMaterials bc = new ProCheckMaterials();
            string n = this.ToString();
            string sql = "select DISTINCT " + n + ".*,CASE WHEN ISNULL(" + bc.ToString() + ".IsOk,'0') = '0' THEN '未质检' WHEN ISNULL(" + bc.ToString() + ".IsOk,'0') = '1'	then '合格' END  AS IsCheckOk  from (" + base.GetSearchSQL() + ") " + n
                + "  left join " + bc.ToString() + " on " + n + ".MATERIALCODE=" + bc.ToString() + ".CHECKWARE and "+n+".BatchNo = "+bc.ToString()+".BatchNo";
            return sql;
        }
      
        private HierarchicalEntityView<StockOutOrderMaterials, OutInSequence> _OutInSequence;
        /// <summary>
        /// 序列码
        /// </summary>
        [HierarchicalEntityControl(disabled = false)]
        public HierarchicalEntityView<StockOutOrderMaterials, OutInSequence> OutInSequence
        {
            get
            {
                if (_OutInSequence == null)
                {
                    _OutInSequence = new HierarchicalEntityView<StockOutOrderMaterials, OutInSequence>(this);
                }
                return _OutInSequence;
            }
        }

        ///// <summary>
        ///// 外系统数据关系集合
        ///// </summary>
        //private HierarchicalEntityView<StockOutOrderMaterials, StockOutOrderMaterialsMapping> _MapItems;
        //[HierarchicalEntityControl(visible = false)]
        //public HierarchicalEntityView<StockOutOrderMaterials, StockOutOrderMaterialsMapping> MapItems
        //{
        //    get
        //    {
        //        if (_MapItems == null)
        //            _MapItems = new HierarchicalEntityView<StockOutOrderMaterials, StockOutOrderMaterialsMapping>(this);
        //        return _MapItems;
        //    }
        //}
    }
}
