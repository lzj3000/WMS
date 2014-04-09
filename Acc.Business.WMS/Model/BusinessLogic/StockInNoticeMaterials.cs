using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model;
using Way.EAP.DataAccess.Data;
using System.Data;
using Acc.Business.WMS.Controllers;
using Acc.Contract.MVC;

namespace Acc.Business.WMS.Model
{
    /// <summary>
    /// 描述：入库单明细表
    /// 作者：路聪
    /// 创建日期:2013-01-17
    /// </summary>
    [EntityClassAttribut("Acc_WMS_InNoticeMaterials", "入库通知明细", IsOnAppendProperty = true)]
    public class StockInNoticeMaterials : OrderMaterialsCommon, IEqualityComparer<StockInNoticeMaterials>
    {
        /// <summary>
        /// 来源单号
        /// </summary>
        [EntityControl("来源单号", true, true, 1)]
        [EntityForeignKey(typeof(StockInNotice), "ID", "CODE")]
        [EntityField(8)]
        public int PARENTID { get; set; }
        //protected override string GetSearchSQL()
        //{
        //    StockInOrderMaterials siom = new StockInOrderMaterials();
        //    string bm = this.ToString();
        //    string sql = base.GetSearchSQL();
        //    string nsql = "select " + bm + ".*,isnull(b.FINISHNUM,0) FINISHNUM," + bm + ".num-isnull(b.FINISHNUM,0) STAYNUM from (" + sql + ") " + bm
        //        + " left join (select SourceRowID,sum(num) FINISHNUM from " + siom.ToString() + " group by  SourceRowID)  b on b.SourceRowID=" + bm + ".id";
        //    return nsql;
        //}
        protected override string GetSearchSQL()
        {
            StockInOrderMaterials siom = new StockInOrderMaterials();
            string bm = this.ToString();
            string sql = base.GetSearchSQL();
            string nsql = "select " + bm + ".*,isnull(b.FINISHNUM,0) FINISHNUM," + bm + ".num-isnull(b.FINISHNUM,0) STAYNUM from (" + sql + ") " + bm
                + " left join (select SourceRowID,sum(num) FINISHNUM,materialcode from " + siom.ToString() + " group by  SourceRowID,materialcode)  b on b.SourceRowID=" + bm + ".id and  b.MATERIALCODE = Acc_WMS_InNoticeMaterials.MATERIALCODE";
            return nsql;
        }
        public bool Equals(StockInNoticeMaterials x, StockInNoticeMaterials y)
        {
            if (x.ID == y.ID && x.MATERIALCODE == y.MATERIALCODE)
                return true;
            return false;
        }

        public int GetHashCode(StockInNoticeMaterials obj)
        {
            return this.GetHashCode();
        }

        //private HierarchicalEntityView<StockInNoticeMaterials, InSequence> _InSequence;
        ////<summary>
        ////序列码
        ////</summary>
        //[HierarchicalEntityControl(isadd=false,isedit= false,ischeck = false)]
        //public HierarchicalEntityView<StockInNoticeMaterials, InSequence> InSequence
        //{
        //    get
        //    {
        //        if (_InSequence == null)
        //        {
        //            _InSequence = new HierarchicalEntityView<StockInNoticeMaterials, InSequence>(this);
        //        }
        //        return _InSequence;
        //    }
        //}
    }
}
