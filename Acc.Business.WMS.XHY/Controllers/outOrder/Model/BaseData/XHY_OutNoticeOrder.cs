using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Business.WMS.Model;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model;

namespace Acc.Business.WMS.XHY.Model
{
    public class XHY_OutNoticeOrder : StockOutNotice,IPropertyValueType
    {
        [EntityForeignKey(typeof(Organization), "ID", "OrganizationName")]
        public new string STAY5
        {
            get;
            set;
        } 
        [EntityControl("出库规则",  false,true, 4)]
        [EntityField(200)]
        [ValueTypeProperty]
        public string OutRule { get; set; }

        [EntityControl("客户类型", false, true,5)]
        [EntityField(200)]
        public string khlx { get; set; }

        [EntityControl("交货方式", false, true, 6)]
        [EntityField(200)]
        public string jhfs { get; set; }


        [EntityControl("业务单位", false, true, 7)]
        [EntityField(200)]
        public string ywdw { get; set; }

        [EntityForeignKey(typeof(XHY_Customer), "ID", "CUSTOMERNAME")]
        [EntityControl("终端客户", false, true, 8)]
        [EntityField(10)]
        public int zdkh { get; set; }



        public PropertyValueType[] GetValueType(ValueTypeArgs valueArgs)
        {
            List<PropertyValueType> list = new List<PropertyValueType>();

            if (valueArgs.ColumnName == "STOCKTYPE")
            {
                PropertyValueType pro = new PropertyValueType();
                pro.Text = "销售出库通知";
                pro.Value = "0";
                PropertyValueType pro1 = new PropertyValueType();
                pro1.Text = "生产领用通知";
                pro1.Value = "1";
                list.Add(pro);
                list.Add(pro1);
            }
            if (valueArgs.ColumnName == "OutRule")
            {
                PropertyValueType pro = new PropertyValueType();
                pro.Text = "先进先出";
                pro.Value = "0";
                PropertyValueType pro1 = new PropertyValueType();
                pro1.Text = "先进后出";
                pro1.Value = "1";

                list.Add(pro);
                list.Add(pro1);
            }
            return list.ToArray();
        }
    }
}
