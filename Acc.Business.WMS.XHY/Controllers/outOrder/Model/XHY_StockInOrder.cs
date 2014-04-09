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
    /// 描述：新花样入库单表
    /// 作者：柳强
    /// 创建日期:2013-10-31
    /// </summary>
    public class XHY_StockInOrder : StockInOrder, IPropertyValueType
    {
        public XHY_StockInOrder[] getsi()
        {
            return null;
        }
        ///<summary>
        ///KG/单品
        /// </summary>
        [EntityControl("K3单据类型", false, false, 20)]
        [EntityField(10)]
        [ValueTypeProperty]
        public int K3InOrderType { get; set; }
        /// <summary>
        /// 源单编码
        /// </summary>
        [EntityControl("源单编码", false, true, 1)]
        [EntityForeignKey(typeof(StockInNoticeMaterials1), "CODE", "CODE")]
        [EntityField(100)]
        public new string SourceCode { get; set; }


        public PropertyValueType[] GetValueType(ValueTypeArgs valueArgs)
        {
            List<PropertyValueType> list = new List<PropertyValueType>();
            if (valueArgs.ColumnName == "K3InOrderType")
            {
                PropertyValueType pro = new PropertyValueType();
                pro.Text = "无";
                pro.Value = "1";
                PropertyValueType pro1 = new PropertyValueType();
                pro1.Text = "其他入库单";
                pro1.Value = "2";
                PropertyValueType pro2 = new PropertyValueType();
                pro2.Text = "红字销售出库单";
                pro2.Value = "3";
                list.Add(pro);
                list.Add(pro1);
                list.Add(pro2);

            }
            return list.ToArray();
        }
    }
}
