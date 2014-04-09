using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model;
using Acc.Contract.MVC;
using Acc.Business.Model.Interface;

namespace Acc.Business.WMS.Model
{

    /// <summary>
    /// 描述：入库单表
    /// 作者：路聪
    /// 创建日期:2012-12-17
    /// </summary>
    [EntityClassAttribut("Acc_WMS_InOrder", "入库单", IsOnAppendProperty = true)]
    public class StockInOrder : OrderCommon
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
        [EntityForeignKey(typeof(StockInNotice), "CODE", "CODE")]
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
        public DateTime INOUTTIME { get; set; }


        private HierarchicalEntityView<StockInOrder, StockInOrderMaterials> _Materials;
        /// <summary>
        /// 入库单入库物料集合
        /// </summary>
        public HierarchicalEntityView<StockInOrder, StockInOrderMaterials> Materials
        {
            get
            {
                if (_Materials == null)
                {
                    _Materials = new HierarchicalEntityView<StockInOrder, StockInOrderMaterials>(this);
                    //_Materials.AttachCondition("SourceCode", "ORDERNO");
                    //_Materials.AttachCondition("PRODUCTIONLINE", "BATCHNO");
                }
                return _Materials;
            }
        }

        private HierarchicalEntityView<StockInOrder, StockInNoticeMaterials> _StayMaterials;
        /// <summary>
        /// 入库单待入库明细
        /// </summary>
        [HierarchicalEntityControl(isadd = false, isedit = false, isremove = false)]
        public HierarchicalEntityView<StockInOrder, StockInNoticeMaterials> StayMaterials
        {
            get
            {
                if (_StayMaterials == null)
                {
                    _StayMaterials = new HierarchicalEntityView<StockInOrder, StockInNoticeMaterials>(this);
                    _StayMaterials.IsAssociateInsert = false;
                    _StayMaterials.IsAssociateDelete = false;
                    _StayMaterials.IsAssociateUpdate = false;
                    // _StayMaterials.AttachCondition("SourceID", "PARENTID");
                }
                return _StayMaterials;
            }
        }
        private HierarchicalEntityView<StockInOrder, StockInNoticeMaterials1> _StayMaterials1;
        /// <summary>
        ///附码单待入库明细
        /// </summary>
        [HierarchicalEntityControl(isadd = false, isedit = false, isremove = false)]
        public HierarchicalEntityView<StockInOrder, StockInNoticeMaterials1> StayMaterials1
        {
            get
            {
                if (_StayMaterials1 == null)
                {
                    _StayMaterials1 = new HierarchicalEntityView<StockInOrder, StockInNoticeMaterials1>(this);
                    _StayMaterials1.IsAssociateInsert = false;
                    _StayMaterials1.IsAssociateDelete = false;
                    _StayMaterials1.IsAssociateUpdate = false;
                    // _StayMaterials.AttachCondition("SourceID", "PARENTID");
                }
                return _StayMaterials1;
            }
        }
        /// <summary>
        /// 外系统数据同步日志
        /// </summary>
        private HierarchicalEntityView<StockInOrder, LogData> _LogItems;
        //[HierarchicalEntityControl(visible = false)]
        [HierarchicalEntityControl(isadd = false, isedit = false, isremove = false)]
        public HierarchicalEntityView<StockInOrder, LogData> LogItems
        {
            get
            {
                if (_LogItems == null)
                    _LogItems = new HierarchicalEntityView<StockInOrder, LogData>(this);
                _LogItems.ChildCondition("SourceTable", "='" + this.ToString() + "'");
                return _LogItems;
            }
        }


        public PropertyValueType[] GetValueType(ValueTypeArgs valueArgs)
        {
            List<PropertyValueType> list = new List<PropertyValueType>();
            if (valueArgs.ColumnName == "STATE")
            {

                PropertyValueType pro = new PropertyValueType();
                pro.Text = "新建";
                pro.Value = "0";
                PropertyValueType pro1 = new PropertyValueType();
                pro1.Text = "分派";
                pro1.Value = "1";
                PropertyValueType pro2 = new PropertyValueType();
                pro2.Text = "完成";
                pro2.Value = "2";
                list.Add(pro);
                list.Add(pro1);
                list.Add(pro2);
            }
            return list.ToArray();
        }

    }
}
