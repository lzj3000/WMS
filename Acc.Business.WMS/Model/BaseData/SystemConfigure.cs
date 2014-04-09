using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model;

namespace Acc.Business.WMS.Model
{
    /// <summary>
    /// 描述：WMS系统配置
    /// 作者：路聪
    /// 创建日期:2013-01-10
    /// </summary>
    [EntityClassAttribut("Acc_WMS_SystemConfigure", "WMS系统配置", IsOnAppendProperty = true)]
    public class SystemConfigure : BasicInfo,IPropertyValueType
    {
        /// <summary>
        /// 托盘编码
        /// </summary>
        [EntityControl("托盘编码", false, true, 1)]
        [EntityField(255, IsNotNullable = true)]
        public string PORTNO { get; set; }
        /// <summary>
        /// 托盘名称
        /// </summary>
        [EntityControl("托盘名称", false, true, 2)]
        [EntityField(255, IsNotNullable = true)]
        public string PORTNAME { get; set; }
        /// <summary>
        /// 托盘状态
        /// </summary>
        [EntityControl("托盘状态", false, true, 3)]
        [EntityField(255, IsNotNullable = true)]
        [ValueTypeProperty]
        public string STATUS { get; set; }

        #region IPropertyValueType 成员

        public PropertyValueType[] GetValueType(ValueTypeArgs valueArgs)
        {
            List<PropertyValueType> list = new List<PropertyValueType>();
            if (valueArgs.ColumnName == "STATUS")
            {
                PropertyValueType pro = new PropertyValueType();
                pro.Text = "可用";
                pro.Value = "0";
                PropertyValueType pro1 = new PropertyValueType();
                pro1.Text = "占用";
                pro1.Value = "1";
                list.Add(pro);
                list.Add(pro1);
            }
            return list.ToArray();
        }

        #endregion
    }
}
