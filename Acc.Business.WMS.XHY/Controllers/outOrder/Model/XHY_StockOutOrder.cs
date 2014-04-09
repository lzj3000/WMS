using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model;
using Acc.Contract.MVC;
using Acc.Business.WMS.Model;

namespace Acc.Business.WMS.XHY.Model
{
    /// <summary>
    /// 描述：新花样出库单表
    /// 作者：柳强
    /// 创建日期:2013-10-31
    /// </summary>
    public class XHY_StockOutOrder:StockOutOrder,IPropertyValueType
    {
        public XHY_StockOutOrder[] getsi()
        {
            return null;
        }
        ///<summary>
        ///备用字段
        /// </summary>
        [EntityControl("购货单位联系人", false, true, 7)]
        [EntityField(200)]
        public string LXRName { get; set; }
        ///<summary>
        ///备用字段
        /// </summary>
        [EntityControl("联系电话", false, true, 8)]
        [EntityField(200)]
        public string LXRPhone { get; set; }
        ///<summary>
        ///备用字段
        /// </summary>
        [EntityControl("购货单位地址", false, true, 9)]
        [EntityField(200)]
        public string LXRAddress { get; set; }

        ///<summary>
        ///备用字段
        /// </summary>
         [EntityForeignKey(typeof(XHY_Customer), "ID", "CUSTOMERNAME")]
        [EntityControl("终端客户", false, true, 10)]
        [EntityField(10)]
        public int zdkh { get; set; }

        ///<summary>
        ///备用字段
        /// </summary>
        [EntityControl("终端联系人", false, true, 11)]
        [EntityField(200)]
        public string zdlxr { get; set; }
        ///<summary>
        ///备用字段
        /// </summary>
        [EntityControl("终端电话", false, true, 12)]
        [EntityField(200)]
        public string zdtel { get; set; }
        ///<summary>
        ///备用字段
        /// </summary>
        [EntityControl("终端手机", false, true, 12)]
        [EntityField(200)]
        public string zdphone { get; set; }
        ///<summary>
        ///备用字段
        /// </summary>
        [EntityControl("终端客户地址", false, true, 13)]
        [EntityField(200)]
        public string zdaddress { get; set; }

        [EntityControl("客户类型", false, true, 14)]
        [EntityField(200)]
        public string khlx { get; set; }

        [EntityControl("交货方式", false, true,15)]
        [EntityField(200)]
        public string jhfs { get; set; }


        [EntityControl("业务单位", false, true, 16)]
        [EntityField(200)]
        public string ywdw { get; set; }

        [EntityControl("运费", false, true, 17)]
        [EntityField(100)]
        public string yf { get; set; }
        ///<summary>
        ///KG/单品
        /// </summary>
        [EntityControl("K3单据类型", false, false, 20)]
        [EntityField(10)]
        [ValueTypeProperty]
        public int K3OutOrderType { get; set; }

        public PropertyValueType[] GetValueType(ValueTypeArgs valueArgs)
        {
            List<PropertyValueType> list = new List<PropertyValueType>();
            if (valueArgs.ColumnName == "K3OutOrderType")
            {
                PropertyValueType pro = new PropertyValueType();
                pro.Text = "无";
                pro.Value = "1";
                PropertyValueType pro1 = new PropertyValueType();
                pro1.Text = "其他出库单";
                pro1.Value = "2";
                list.Add(pro);
                list.Add(pro1);

            }
            return list.ToArray();
        }
    }
}
