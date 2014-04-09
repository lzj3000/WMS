using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Business.WMS.Model;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model;

namespace Acc.Business.WMS.XHY.Model
{
    public class XHY_InNoticeOrder:StockInNotice,IPropertyValueType
    {

        public PropertyValueType[] GetValueType(ValueTypeArgs valueArgs)
        {
            List<PropertyValueType> list = new List<PropertyValueType>();

            if (valueArgs.ColumnName == "STOCKTYPE")
            {
                PropertyValueType pro = new PropertyValueType();
                pro.Text = "采购入库通知";
                pro.Value = "0";
                PropertyValueType pro1 = new PropertyValueType();
                pro1.Text = "生产计划通知";
                pro1.Value = "1";
                PropertyValueType pro2 = new PropertyValueType();
                pro2.Text = "生产赋码通知";
                pro2.Value = "2";
                list.Add(pro);
                list.Add(pro1);
                list.Add(pro2);
            }
            return list.ToArray();
        }
    }
}
