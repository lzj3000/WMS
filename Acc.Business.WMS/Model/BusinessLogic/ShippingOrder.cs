using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model;
using Acc.Business.WMS.Controllers;
using Acc.Contract.MVC;

namespace Acc.Business.WMS.Model
{
    /// <summary>
    /// 描述：发运通知表
    /// 作者：柳强
    /// 创建日期:2013-03-13
    /// </summary>
    [EntityClassAttribut("Acc_WMS_ShippingOrder", "发运单", IsOnAppendProperty = true)]
    public class ShippingOrder : OrderCommon, IPropertyValueType
    {
        /// <summary>
        /// 源单编码
        /// </summary>
        [EntityControl("源单编码", false, false, 1)]
        [EntityField(100)]
        public string SourceCode { get; set; }

        /// <summary>
        /// 来源单号
        /// </summary>
        [EntityControl("来源单号", false, true, 2)]
        [EntityField(255, IsNotNullable = true)]
        [EntityForeignKey(typeof(StockOutOrder), "ID", "Code")]
        public int PARENTNO { get; set; }

        /// <summary>
        /// 发运类型
        /// </summary>
        [EntityControl("发运类型", false, true, 3)]
        [EntityField(255, IsNotNullable = true)]
        [ValueTypeProperty]
        public int SHIPPINGTYPE { get; set; }

        /// <summary>
        /// 物流公司
        /// </summary>
        [EntityControl("物流公司", false, true, 4)]
        [EntityField(255)]
        [EntityForeignKey(typeof(LogisticsInfo), "ID", "LOGISTICSNAME")]
        public int LOGISTICSCOMPANY { get; set; }

        /// <summary>
        /// 物流单号
        /// </summary>
        [EntityControl("物流单号", false, true, 5)]
        [EntityField(255)]
        public string LOGISTICSNO { get; set; }

        /// <summary>
        /// 收货地址省
        /// </summary>
        [EntityControl("收货地址省/自治区", false, true, 6)]
        [EntityField(255, IsNotNullable = true)]
        public string RECEIVINGADDRESS1 { get; set; }

        /// <summary>
        /// 收货地址市
        /// </summary>
        [EntityControl("市区/县", false, true, 7)]
        [EntityField(255)]
        public string RECEIVINGADDRESS2 { get; set; }
        /// <summary>
        /// 收货地址镇
        /// </summary>
        [EntityControl("区/镇/乡/村/街道", false, true, 8)]
        [EntityField(255)]
        public string RECEIVINGADDRESS3 { get; set; }

        /// <summary>
        /// 收货人姓名
        /// </summary>
        [EntityControl("收货人姓名", false, true, 9)]
        [EntityField(255, IsNotNullable = true)]
        public string RECEIVINGUSER { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        [EntityControl("联系电话", false, true, 10)]
        [EntityField(255, IsNotNullable = true)]
        public string TEL { get; set; }

        /// <summary>
        /// 发货时间
        /// </summary>
        [EntityControl("发运时间", false, true, 11)]
        [EntityField(255)]
        public DateTime OUTTIME { get; set; }

        private HierarchicalEntityView<ShippingOrder, ShippingOrderMaterials> _Materials;
        /// <summary>
        /// 出库单出库物料集合
        /// </summary>
        public HierarchicalEntityView<ShippingOrder, ShippingOrderMaterials> Materials
        {
            get
            {
                if (_Materials == null)
                    _Materials = new HierarchicalEntityView<ShippingOrder, ShippingOrderMaterials>(this);
                return _Materials;
            }
        }


        #region IPropertyValueType 成员

        public PropertyValueType[] GetValueType(ValueTypeArgs valueArgs)
        {
            List<PropertyValueType> list = new List<PropertyValueType>();
            if (valueArgs.ColumnName == "SHIPPINGTYPE")
            {
                PropertyValueType pro = new PropertyValueType();
                pro.Text = "销售发运";
                pro.Value = "1";
                PropertyValueType pro1 = new PropertyValueType();
                pro1.Text = "维修发运";
                pro1.Value = "2";
                PropertyValueType pro2 = new PropertyValueType();
                pro2.Text = "采购发运";
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
