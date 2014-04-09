using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Contract.MVC;

namespace Acc.Business.Model
{
    /// <summary>
    /// 系统信息-子集
    /// </summary>
    [EntityClassAttribut("Acc_Bus_SystemInfoDetials", "系统信息子集")]
    public class SystemInfoDetials : BusinessBase,IPropertyValueType
    {

        /// <summary>
        /// 父级
        /// </summary>
        [EntityControl("父级ID", false, false, 2)]
        [EntityForeignKey(typeof(SystemInfo), "ID", "Code")]
        [EntityField(20)]
        public int ParentID { get; set; }
        /// <summary>
        /// 显示名称
        /// </summary>
        [EntityControl("显示名称", false, true, 3)]
        [EntityField(200)]
        public string DisplayName { get; set; }
        /// <summary>
        /// 待处理数量
        /// </summary>
        [EntityControl("待处理数量", true, false, 4)]
        [EntityField(20)]
        public int NUM { get; set; }

        /// <summary>
        /// sql语句
        /// </summary>
        [EntityControl("显示数据的sql", false, true, 5)]
        [EntityField(200)]
        [ValueTypeProperty]
        public string SqlStr { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        [EntityControl("是否显示", false, true, 6)]
        [EntityField(2)]
        public bool IsSelected { get; set; }

        /// <summary>
        /// 页面名称
        /// </summary>
        [EntityControl("页面名称", false, true, 7)]
        [EntityField(200,IsNotNullable=true)]
        public string PageName { get; set; }

        /// <summary>
        /// 页面URL
        /// </summary>
        [EntityControl("页面URL", false, true,8)]
        [EntityField(200, IsNotNullable = true)]
        public string PageRUL { get; set; }

        public PropertyValueType[] GetValueType(ValueTypeArgs valueArgs)
        {
            List<PropertyValueType> list = new List<PropertyValueType>();
            if (valueArgs.ColumnName == "SqlStr")
            {
                PropertyValueType pro = new PropertyValueType();
                pro.Text = "负库存预警";
                //pro.Value = "select count(*) as num from acc_wms_stockInfo_materials where num < 0";
                pro.Value = "select count(*) as num from acc_wms_stockInfo_materials";
                PropertyValueType pro1 = new PropertyValueType();
                pro1.Text = "入库待处理";
                pro1.Value = "select count(*) as num from acc_wms_stockInOrder where issubmited =1 and isreviewed=0";
                PropertyValueType pro2 = new PropertyValueType();
                pro2.Text = "出库待处理";
                pro2.Value = "select count(*) as num from acc_wms_stockOutOrder where issubmited =1 and isreviewed=0";
                PropertyValueType pro3 = new PropertyValueType();
                pro3.Text = "盘点待处理";
                pro3.Value = "select count(*) as num from Acc_WMS_CheckOrder where issubmited =1";
                list.Add(pro);
                list.Add(pro1);
                list.Add(pro2);
                list.Add(pro3);

            }
            return list.ToArray();
        }
    }
}
