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
    /// 描述：出库通知
    /// 作者：柳强
    /// 创建日期:2013-04-01
    /// </summary>
    [EntityClassAttribut("Acc_WMS_OutNotice", "出库通知", IsOnAppendProperty = true)]
    public class StockOutNotice : OrderCommon
    {
        public StockOutNotice[] GetOutNotice()
        {
            return null;
        }
        /// <summary>
        /// 源名称
        /// </summary>
        [EntityControl("源单编码", false, true, 2)]
        [EntityField(100)]
        public virtual string SourceCode { get; set; }

        private HierarchicalEntityView<StockOutNotice, StockOutNoticeMaterials> _Materials;
        /// <summary>
        /// 出库单出库物料集合
        /// </summary>
        public HierarchicalEntityView<StockOutNotice, StockOutNoticeMaterials> Materials
        {
            get
            {
                if (_Materials == null)
                {
                    _Materials = new HierarchicalEntityView<StockOutNotice, StockOutNoticeMaterials>(this);
                }
                return _Materials;
            }
        }

        
        ///// <summary>
        ///// 外系统数据关系集合
        ///// </summary>
        //private HierarchicalEntityView<StockOutNotice, StockOutNoticeMapping> _MapItems;
        //[HierarchicalEntityControl(visible = false)]
        //public HierarchicalEntityView<StockOutNotice, StockOutNoticeMapping> MapItems
        //{
        //    get
        //    {
        //        if (_MapItems == null)
        //            _MapItems = new HierarchicalEntityView<StockOutNotice, StockOutNoticeMapping>(this);
        //        return _MapItems;
        //    }
        //}

        #region IPropertyValueType 成员

        public PropertyValueType[] GetValueType(ValueTypeArgs valueArgs)
        {
            List<PropertyValueType> list = new List<PropertyValueType>();
            if (valueArgs.ColumnName == "ORDERTYPE")
            {
                PropertyValueType pro = new PropertyValueType();
                pro.Text = "入库计划通知";
                pro.Value = "1";
                PropertyValueType pro1 = new PropertyValueType();
                pro1.Text = "出库计划通知";
                pro1.Value = "2";
                PropertyValueType pro2 = new PropertyValueType();
                pro2.Text = "采购计划通知";
                pro2.Value = "3";
                list.Add(pro);
                list.Add(pro1);
                list.Add(pro2);
            }

            return list.ToArray();
        }

        #endregion
    }
}
