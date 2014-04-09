using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model;
using Acc.Business.WMS.Controllers;
using Acc.Contract.MVC;
using Acc.Business.Model.Purview;
using Acc.Business.Model.Interface;

namespace Acc.Business.WMS.Model
{
    /// <summary>
    /// 描述：出库单表
    /// 作者：柳强
    /// 创建日期:2012-12-21
    /// </summary>
    [EntityClassAttribut("Acc_WMS_OutOrder", "出库单", IsOnAppendProperty = true)]
    public class StockOutOrder : OrderCommon
    {
        [EntityForeignKey(typeof(Organization), "ID", "OrganizationName")]
        public new string STAY5
        {
            get;
            set;
        } 

        /// <summary>
        /// 源单编码
        /// </summary>
        [EntityControl("源单编码", false, true, 1)]
        [EntityForeignKey(typeof(StockOutNotice), "CODE", "CODE")]
        [EntityField(100)]
        public string SourceCode { get; set; }
        /// <summary>
        /// 仓库
        /// </summary>
        [EntityControl("仓库", false, true, 7)]
        [EntityForeignKey(typeof(WareHouse), "ID", "WAREHOUSENAME")]
        [EntityField(255)]
        public int TOWHNO { get; set; }
        /// <summary>
        /// 收发货时间
        /// </summary>
        [EntityControl("收发货时间", false, true, 8)]
        [EntityField(255)]
        public  DateTime INOUTTIME { get; set; }

        /// <summary>
        /// 运输公司
        /// </summary>
        [EntityControl("运输公司", false, true, 9)]
        [EntityForeignKey(typeof(LogisticsInfo), "ID", "LOGISTICSNAME")]
        [EntityField(100)]
        public int LogCode { get; set; }

        private HierarchicalEntityView<StockOutOrder, StockOutOrderMaterials> _Materials;
        /// <summary>
        /// 出库单出库物料集合
        /// </summary>
        public HierarchicalEntityView<StockOutOrder, StockOutOrderMaterials> Materials
        {
            get
            {
                if (_Materials == null)
                    _Materials = new HierarchicalEntityView<StockOutOrder, StockOutOrderMaterials>(this);
                return _Materials;
            }
        }



        private HierarchicalEntityView<StockOutOrder, StockOutNoticeMaterials> _StayMaterials;
        /// <summary>
        /// 出库单待出库明细
        /// </summary>
        [HierarchicalEntityControl(isadd = false, isedit = false, isremove = false)]
        public HierarchicalEntityView<StockOutOrder, StockOutNoticeMaterials> StayMaterials
        {
            get
            {
                if (_StayMaterials == null)
                    _StayMaterials = new HierarchicalEntityView<StockOutOrder, StockOutNoticeMaterials>(this);
                _StayMaterials.IsAssociateInsert = false;
                _StayMaterials.IsAssociateDelete = false;
                _StayMaterials.IsAssociateUpdate = false;
                return _StayMaterials;
            }
        }

        /// <summary>
        /// 外系统数据同步日志
        /// </summary>
        private HierarchicalEntityView<StockOutOrder, LogData> _LogItems;
        //[HierarchicalEntityControl(visible = false)]
        [HierarchicalEntityControl(isadd = false, isedit = false, isremove = false)]
        public HierarchicalEntityView<StockOutOrder, LogData> LogItems
        {
            get
            {
                if (_LogItems == null)
                    _LogItems = new HierarchicalEntityView<StockOutOrder, LogData>(this);
                _LogItems.ChildCondition("SourceTable", "='" + this.ToString() + "'");
                return _LogItems;
            }
        }

        #region IPropertyValueType 成员

        public PropertyValueType[] GetValueType(ValueTypeArgs valueArgs)
        {
            List<PropertyValueType> list = new List<PropertyValueType>();
            if (valueArgs.ColumnName == "STOCKTYPE")
            {
                PropertyValueType pro = new PropertyValueType();
                pro.Text = "销售出库";
                pro.Value = "1";
                PropertyValueType pro1 = new PropertyValueType();
                pro1.Text = "维修出库";
                pro1.Value = "2";
                PropertyValueType pro2 = new PropertyValueType();
                pro2.Text = "采购出库";
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
