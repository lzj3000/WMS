using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model;
using Acc.Contract.MVC;

namespace Acc.Business.WMS.Model
{
    /// <summary>
    /// 描述：出库通知明细表
    /// 作者：柳强
    /// 创建日期:2013-02-19
    /// </summary>
    [EntityClassAttribut("Acc_WMS_OutNoticeMaterials", "出库通知明细", IsOnAppendProperty = true)]
    public class StockOutNoticeMaterials : OrderMaterialsCommon
    {
        /// <summary>
        /// 来源单号
        /// </summary>
        [EntityControl("来源单号", true, true, 1)]
        [EntityForeignKey(typeof(StockOutNotice), "ID", "CODE")]
        [EntityField(8)]
        public int PARENTID { get; set; }
        protected override string GetSearchSQL()
        {
            StockOutOrderMaterials siom = new StockOutOrderMaterials();
            string bm = this.ToString();
            string sql = base.GetSearchSQL();
            string nsql = "select "+bm+".*,isnull(b.FINISHNUM,0) FINISHNUM,"+bm+".num-isnull(b.FINISHNUM,0) STAYNUM from (" + sql + ") " + bm
                + " left join (select SourceRowID,sum(num) FINISHNUM from " + siom.ToString() + " group by  SourceRowID)  b on b.SourceRowID=" + bm + ".id";
            return nsql;
        }
        ///// <summary>
        ///// 外系统数据关系集合
        ///// </summary>
        //private HierarchicalEntityView<StockOutNoticeMaterials, StockOutNoticeMaterialsMapping> _MapItems;
        //[HierarchicalEntityControl(visible = false)]
        //public HierarchicalEntityView<StockOutNoticeMaterials, StockOutNoticeMaterialsMapping> MapItems
        //{
        //    get
        //    {
        //        if (_MapItems == null)
        //            _MapItems = new HierarchicalEntityView<StockOutNoticeMaterials, StockOutNoticeMaterialsMapping>(this);
        //        return _MapItems;
        //    }
        //}
    }
}
