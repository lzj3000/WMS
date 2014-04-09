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
using Acc.Business.WMS.Model;

namespace Acc.Business.WMS.Model
{
    /// <summary>
    /// 描述：入库单明细表
    /// 作者：路聪
    /// 创建日期:2013-01-17
    /// </summary>
    [EntityClassAttribut("Acc_WMS_InNoticeMaterials1", "生产赋码单", IsOnAppendProperty = true)]
    public class StockInNoticeMaterials1 : OrderMaterialsCommon
    {
         [EntityControl("关联ID", false, false, 2)]
        public new int ID
        {
            get;
            set;
        }

        [EntityForeignKey(typeof(Organization), "ID", "OrganizationName")]
        public new string STAY5
        {
            get;
            set;
        } 
        /// <summary>
        /// 当前时间
        /// </summary>
        [EntityControl("当前时间", false, false, 2)]
        [EntityField(20)]
        public string TempTime { get; set; }
        /// <summary>
        /// 生产时间
        /// </summary>
        [EntityControl("生产日期", false, true, 8)]
        [EntityField(20)]
        public DateTime ProduceTime { get; set; }

        protected override string GetSearchSQL()
        {
            StockInOrderMaterials siom = new StockInOrderMaterials();
            string bm = this.ToString();
            string sql = base.GetSearchSQL();
            string nsql = "select " + bm + ".*,isnull(b.FINISHNUM,0) FINISHNUM," + bm + ".num-isnull(b.FINISHNUM,0) STAYNUM from (" + sql + ") " + bm
                + " left join (select SourceRowID,materialcode,sum(num) FINISHNUM from " + siom.ToString() + " group by  SourceRowID,materialcode)  b on b.SourceRowID=" + bm + ".id and b.materialcode = " + bm + ".materialcode";
            return nsql;
        }
      
        private HierarchicalEntityView<StockInNoticeMaterials1, FMInSequence> _InSequence;
        //<summary>
        //序列码
        //</summary>
        [HierarchicalEntityControl(isadd=false,isedit= false,ischeck = false)]
        public HierarchicalEntityView<StockInNoticeMaterials1, FMInSequence> InSequence
        {
            get
            {
                if (_InSequence == null)
                {
                    _InSequence = new HierarchicalEntityView<StockInNoticeMaterials1, FMInSequence>(this);
                }
                return _InSequence;
            }
        }
    }
}
