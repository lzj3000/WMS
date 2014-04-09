using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model;
using Acc.Contract.MVC;
using System.Configuration;
using Way.EAP.DataAccess.Data;

namespace Acc.Business.Model
{
    /// <summary>
    /// 描述：Domian Object for 打印参数维护
    /// 作者：路聪
    /// 创建日期:2013-8-01
    /// </summary>
    [EntityClassAttribut("PrintTemplateParm", "打印参数维护", IsOnAppendProperty = true)]
    public class PrintTemplateParm : BasicInfo, IPropertyValueType
    {

        /// <summary>
        /// SID
        /// </summary>
        [EntityControl("SID", false, false, 1)]
        [EntityField(10, IsNotNullable = true)]
        public int SID { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        [EntityControl("类型", false, true, 2)]
        [EntityField(255, IsNotNullable = true)]
        [ValueTypeProperty]
        public string TypeName { get; set; }
        /// <summary>
        /// 属性
        /// </summary>
        [EntityControl("属性", false, true, 3)]
        [EntityField(200, IsNotNullable = true)]
        public string BindProperty { get; set; }
        /// <summary>
        /// 属性名称
        /// </summary>
        [EntityControl("属性名称", false, true, 4)]
        [EntityField(200, IsNotNullable = true)]
        public string BindPropertyName { get; set; }
        /// <summary>
        /// 排序号
        /// </summary>
        [EntityControl("排序号", true, false, 5)]
        [EntityField(4, IsNotNullable = false)]
        public int OrderNo { get; set; }
        /// <summary>
        /// 属性名称
        /// </summary>
        [EntityControl("属性名称", false, false, 6)]
        [EntityField(200, IsNotNullable = true)]
        public DateTime CreateDate { get; set; }

        public PropertyValueType[] GetValueType(ValueTypeArgs valueArgs)
        {
            List<PropertyValueType> list = new List<PropertyValueType>();
            if (valueArgs.ColumnName == "TypeName")
            {
                PropertyValueType pro = new PropertyValueType();
                pro.Text = "文本值";
                pro.Value = "Txt";
                PropertyValueType pro2 = new PropertyValueType();
                pro2.Text = "条码值";
                pro2.Value = "BarCode";
                list.Add(pro);
                list.Add(pro2);
            }
            return list.ToArray();
        }
    }
}
