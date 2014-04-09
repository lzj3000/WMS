using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model;
using Way.EAP.DataAccess.Data;
using Acc.Contract.MVC;

namespace Acc.Business.WMS.Model
{
    /// <summary>
    /// 描述：入库单表
    /// 作者：路聪
    /// 创建日期:2013-01-17
    /// </summary>
    [EntityClassAttribut("Acc_WMS_InNotice", "入库通知", IsOnAppendProperty = true)]
    public class StockInNotice : OrderCommon
    {
        [EntityForeignKey(typeof(Organization), "ID", "OrganizationName")]
        public new string STAY5
        {
            get;
            set;
        } 
        /// <summary>
        /// 源名称
        /// </summary>
        [EntityForeignKey(typeof(StockOutOrder), "CODE", "CODE")]
        [EntityControl("源单编码", false, true, 2)]
        [EntityField(100)]
        public virtual string SourceCode { get; set; }
        private HierarchicalEntityView<StockInNotice, StockInNoticeMaterials> _Materials;
        /// <summary>
        /// 入库单入库物料集合
        /// </summary>
        public HierarchicalEntityView<StockInNotice, StockInNoticeMaterials> Materials
        {
            get
            {
                if (_Materials == null)
                {
                    _Materials = new HierarchicalEntityView<StockInNotice, StockInNoticeMaterials>(this);
                }
                return _Materials;
            }
        }

        ///// <summary>
        ///// 外系统数据关系集合
        ///// </summary>
        //private HierarchicalEntityView<StockInNotice, StockInNoticeMapping> _MapItems;
        //[HierarchicalEntityControl(visible = false)]
        //public HierarchicalEntityView<StockInNotice, StockInNoticeMapping> MapItems
        //{
        //    get
        //    {
        //        if (_MapItems == null)
        //            _MapItems = new HierarchicalEntityView<StockInNotice, StockInNoticeMapping>(this);
        //        return _MapItems;
        //    }
        //}

    }
}
