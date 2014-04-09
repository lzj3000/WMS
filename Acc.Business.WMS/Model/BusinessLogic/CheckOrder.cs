using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model;

namespace Acc.Business.WMS.Model
{
    /// <summary>
    /// 描述：盘点单管理
    /// 作者：路聪
    /// 创建日期:2012-12-25
    /// </summary>
    [EntityClassAttribut("Acc_WMS_CheckOrder", "盘点单管理", IsOnAppendProperty = true)]
    public class CheckOrder : BasicInfo, IPropertyValueType
    {
        /// <summary>
        /// 盘点单据名称
        /// </summary>
        [EntityControl("盘点名称", false, true, 1)]
        [EntityField(50)]
        public string ORDERNAME { get; set; }

        /// <summary>
        /// 盘点位置
        /// </summary>
        [EntityControl("盘点位置", false, true, 2)]
        [EntityForeignKey(typeof(WareHouse), "ID", "WAREHOUSENAME")]
        [EntityField(50, IsNotNullable = true)]
        public string WAREHOUSENAME { get; set; }

        /// <summary>
        /// 盘点产品大类
        /// </summary>
        [EntityControl("盘点产品大类", false, true, 3)]
        [EntityField(10)]
        [EntityForeignKey(typeof(CommodityClass), "ID", "ClassName")]
        public int ClassID { get; set; }
        /// <summary>
        /// 盘点产品
        /// </summary>
        [EntityControl("盘点产品", false, true, 4)]
        [EntityForeignKey(typeof(Materials), "ID", "FNAME")]
        [EntityField(255, IsNotNullable = false)]
        public string MATERIALCODE { get; set; }

        /// <summary>
        /// 盘点方式
        /// </summary>
        [EntityControl("盘点方式", false, true, 5)]
        [EntityField(255, IsNotNullable = true)]
        [ValueTypeProperty]
        public string CheckOrderType { get; set; }


        /// <summary>
        /// 盘点父级
        /// </summary>
        [EntityControl("盘点父级", false, true, 6)]
        [EntityField(255)]
        [EntityForeignKey(typeof(CheckOrder), "ID", "ORDERNAME")]
        public int ParentId { get; set; }

        private HierarchicalEntityView<CheckOrder, CheckOrderMaterials> _CheckOrderMaterials;
        /// <summary>
        /// 产品分类内产品信息集合
        /// </summary>
        public HierarchicalEntityView<CheckOrder, CheckOrderMaterials> CheckOrderMaterials
        {
            get
            {
                if (_CheckOrderMaterials == null)
                    _CheckOrderMaterials = new HierarchicalEntityView<CheckOrder, CheckOrderMaterials>(this);
                return _CheckOrderMaterials;
            }
        }

        public PropertyValueType[] GetValueType(ValueTypeArgs args)
        {
            List<PropertyValueType> list = new List<PropertyValueType>();
            if (args.ColumnName == "CheckOrderType")
            {
                PropertyValueType pro1 = new PropertyValueType();
                pro1.Value = "1";
                pro1.Text = "静态盘点";
                PropertyValueType pro2 = new PropertyValueType();
                pro2.Value = "2";
                pro2.Text = "动态盘点";
                list.Add(pro1); list.Add(pro2);
            }
            return list.ToArray();
        }
    }
}
