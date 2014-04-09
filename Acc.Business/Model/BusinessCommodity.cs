using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Contract.MVC;

namespace Acc.Business.Model
{
    /// <summary>
    /// 商品类型别
    /// </summary>
    [EntityClassAttribut("Acc_Bus_CommodityClass", "商品大类", IsOnAppendProperty = true)]
    public class CommodityClass : BasicInfo
    {
        /// <summary>
        /// 类别名称
        /// </summary>
       [EntityControl("名称", false, true, 1)]
       [EntityField(100)]
        public string ClassName { get; set; }
        /// <summary>
        /// 父类ID
        /// </summary>
        [EntityControl("父类",false,true,2)]
        [EntityForeignKey(typeof(CommodityClass), "ID", "ClassName")]
        [EntityField(10)]
        public int ParentID { get; set; }

        private HierarchicalEntityView<CommodityClass, CommodityClass> _childClass;

        public HierarchicalEntityView<CommodityClass, CommodityClass> ChildClass
        {
            get
            {
                if (_childClass == null)
                    _childClass = new HierarchicalEntityView<CommodityClass, CommodityClass>(this);
                return _childClass;
            }
        }

    }
    public enum CommodityTypeItem
    {
        原料,
        半成品,
        成品
    }
    /// <summary>
    /// 业务商品
    /// </summary>
    [EntityClassAttribut("Acc_Bus_BusinessCommodity", "业务商品", IsOnAppendProperty = true)]
    public class BusinessCommodity : BasicInfo, IPropertyValueType
    {
        /// <summary>
        /// 商品名称
        /// </summary>
        [EntityControl("商品名称", false, true, 1)]
        [EntityField(80)]
        public string FNAME { get; set; }
        /// <summary>
        /// 全名
        /// </summary>
        [EntityControl("全名", false, true, 2)]
        [EntityField(250)]
        public string FFULLNAME { get; set; }
        ///// <summary>
        ///// 助记码
        ///// </summary>
        //[EntityControl("助记码", false, true, 5)]
        //[EntityField(50)]
        //public string FHELPCODE { get; set; }
        /// <summary>
        /// EAN13 条码
        /// </summary>
        [EntityControl("EAN13条码", false, true, 4)]
        [EntityField(13)]
        public string CUSTOMEAN13 { get; set; }
        /// <summary>
        /// 基本计量单位
        /// </summary>
        [EntityControl("基本计量单位", false, true, 5)]
        [EntityForeignKey(typeof(Unit), "ID", "UNITNAME")]
        [EntityField(80)]
        public int FUNITID { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        [EntityControl("规格型号", false, true, 6)]
        [EntityField(255)]
        public string FMODEL { get; set; }
        /// <summary>
        /// 类别ID
        /// </summary>
        [EntityControl("大类", false, true, 3)]
        [EntityField(10)]
        [EntityForeignKey(typeof(CommodityClass), "ID", "ClassName")]
        public int ClassID { get; set; }

        /// <summary>
        /// 商品类型
        /// </summary>
        [EntityControl("商品类型", false, true, 7)]
        [EntityField(10)]
        [ValueTypeProperty]
        public int CommodityType { get; set; }

        /// <summary>
        /// 备用6
        /// </summary>
        [EntityField(2000)]
        public string STAY6 { get; set; }
        /// <summary>
        /// 备用7
        /// </summary>
        [EntityField(2000)]
        public string STAY7 { get; set; }
        /// <summary>
        /// 备用8
        /// </summary>
        [EntityField(2000)]
        public string STAY8 { get; set; }
        /// <summary>
        /// 备用9
        /// </summary>
        [EntityField(2000)]
        public string STAY9 { get; set; }
        /// <summary>
        /// 备用10
        /// </summary>
        [EntityField(2000)]
        public string STAY10 { get; set; }

        //private HierarchicalEntityView<BusinessCommodity, CLG> _commodityKeyItems;

        //public HierarchicalEntityView<BusinessCommodity, CLG> CLGS
        //{
        //    get
        //    {
        //        if (_commodityKeyItems == null)
        //            _commodityKeyItems = new HierarchicalEntityView<BusinessCommodity, CLG>(this);
        //        return _commodityKeyItems;
        //    }
        //}

        private HierarchicalEntityView<BusinessCommodity, CommodityBom> _bomDetails;
         [HierarchicalEntityControl(visible = false)]
        public HierarchicalEntityView<BusinessCommodity, CommodityBom> BomDetails
        {
            get
            {
                if (_bomDetails == null)
                {
                    _bomDetails = new HierarchicalEntityView<BusinessCommodity, CommodityBom>(this);
                   // ((IEntityList)_bomDetails).ForeignKey += BusinessCommodity_ForeignKey;
                }
                return _bomDetails;
            }
        }

        void BusinessCommodity_ForeignKey(EntityBase sender, Dictionary<string, EntityForeignKeyAttribute> items)
        {
            throw new NotImplementedException();
        }
        #region IPropertyValueType 成员

        public PropertyValueType[] GetValueType(ValueTypeArgs e)
        {
            return OnGetValueType(e);
        }
        protected virtual PropertyValueType[] OnGetValueType(ValueTypeArgs e)
        {
            List<PropertyValueType> list = new List<PropertyValueType>();

            if (e.ColumnName == "CommodityType")
            {
                PropertyValueType vt = new PropertyValueType();
                vt.Text = "原料";
                vt.Value = "0";
                list.Add(vt);
                PropertyValueType vt1 = new PropertyValueType();
                vt1.Text = "半成品";
                vt1.Value = "1";
                list.Add(vt1);
                PropertyValueType vt2 = new PropertyValueType();
                vt2.Text = "成品";
                vt2.Value = "2";
                list.Add(vt2);
            }
            return list.ToArray();
        }
        #endregion
    }
    [EntityClassAttribut("Acc_Bus_CommodityBom", "商品BOM", IsOnAppendProperty = true)]
    public class CommodityBom : BusinessBase
    {
        [EntityField(10)]
        [EntityForeignKey(typeof(BusinessCommodity), "ID")]
        public int ParentID { get; set; }
        /// <summary>
        /// BOM产品
        /// </summary>
        [EntityControl("BOM产品", false, true, 1)]
        [EntityField(10)]
        public int CommodityID { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        [EntityControl("BOM数量", false, true, 2)]
        [EntityField(10)]
        public double NUM { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [EntityControl("备注", false, true, 150)]
        [EntityField(2000)]
        public string Remark { get; set; }
    }
}
