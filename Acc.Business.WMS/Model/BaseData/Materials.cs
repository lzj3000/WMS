using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model;
using Acc.Contract.MVC;
using Way.EAP.DataAccess.Data;

namespace Acc.Business.WMS.Model
{
    /// <summary>
    /// 描述：产品表
    /// 作者：路聪
    /// 创建日期:2012-12-18
    /// 柳强修改，继承BusinessCommodity这个类
    /// </summary>
    //[EntityClassAttribut("Acc_WMS_Materials", "产品信息", IsOnAppendProperty = true)]
    public class Materials : BusinessCommodity, IPropertyValueType
    {
        public Materials[] GetMaterials()
        {
            return null;
        }
        public Materials()
        {
          
        }
        ///// <summary>
        ///// 产品名称
        ///// </summary>
        //[EntityControl("产品名称", false, true, 2)]
        //[EntityField(255, IsNotNullable = true)]
        //public string FNAME { get; set; }
        /// <summary>
        /// 产品净重量
        /// </summary>
        [EntityControl("净重量", false, false, 3)]
        [EntityField(10, IsNotNullable = false)]
        public double NETWEIGHT { get; set; }
        ///// <summary>
        ///// 分类编码
        ///// </summary>
        //[EntityControl("分类编码", false, true, 4)]
        //[EntityForeignKey(typeof(Material_Categories), "ID", "CATEGORYNAME")]
        //[EntityField(255)]
        //public string CATEGORYCODE { get; set; }
        /// <summary>
        /// 价格(kg/元)
        /// </summary>
        [EntityControl("价格(kg/元)", false, false, 5)]
        [EntityField(10, IsNotNullable = false)]
        public double PRICE { get; set; }
        ///// <summary>
        ///// 单位
        ///// </summary>
        //[EntityControl("单位", false, true, 6)]
        //[EntityField(10)]
        //public string UNIT { get; set; }
        ///// <summary>
        ///// 单位2
        ///// </summary>
        //[EntityControl("单位2", false, false, 7)]
        //[EntityField(10)]
        //public string UNIT2 { get; set; }
        ///// <summary>
        ///// 单位3
        ///// </summary>
        //[EntityControl("单位3", false, false, 8)]
        //[EntityField(10)]
        //public string UNIT3 { get; set; }
        /// <summary>
        /// 批次管理
        /// </summary>
        [EntityControl("批次管理", false, true, 9)]
        [EntityField(20)]
        [ValueTypeProperty]
        public bool BATCH { get; set; }

        /// <summary>
        /// 是否备货
        /// </summary>
        [EntityControl("是否备货", false, true, 9)]
        [EntityField(20)]
        public bool IsReserve { get; set; }

        /// <summary>
        /// 最低库存量
        /// </summary>
        [EntityControl("最低库存量", false, false, 10)]
        [EntityField(20)]
        public double LOWESTSTOCK { get; set; }
        /// <summary>
        /// 换算单位
        /// </summary>
        [EntityControl("换算单位", false, false, 11)]
        [EntityField(255)]
        public string CONVERSION { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        [EntityControl("状态", false, true, 12)]
        [EntityField(255)]
        [ValueTypeProperty]
        public string STATUS { get; set; }
        /// <summary>
        /// 出库规则
        /// </summary>
        [EntityControl("出库规则", false, true, 12)]
        [EntityField(255)]
        [ValueTypeProperty]
        public int OUTWAREHOUSEYPE { get; set; }
        /// <summary>
        /// 默认存放数量
        /// </summary>
        [EntityControl("默认存放数量", false, true, 13)]
        [EntityField(10, IsNotNullable = true)]
        public double STOREAMOUNT { get; set; }
        /// <summary>
        /// 保质期
        /// </summary>
        [EntityControl("保质期(天)", false, false, 14)]
        [EntityField(10, IsNotNullable = false)]
        public double SHELFLIFE { get; set; }
        /// <summary>
        /// 保质期单位
        /// </summary>
        [EntityControl("保质期单位", false, false, 15)]
        [EntityField(255, IsNotNullable = false)]
        [ValueTypeProperty]
        public string SHELFUNIT { get; set; }
        /// <summary>
        /// 预警提前时限
        /// </summary>
        [EntityControl("预警提前时限", false, false, 16)]
        [EntityField(10)]
        public double ALARMEARLIERAMOUNT { get; set; }
        ///<summary>
        /// 是否序列码
        /// </summary>
        [EntityControl("序列码管理", false, false, 17)]
        [EntityField(20)]
        public bool SEQUENCECODE { get; set; }

        ///<summary>
        /// 是否允许负数库存
        /// </summary>
        [EntityControl("是否允许负库存", false, true, 18)]
        [EntityField(20)]
        public bool NUMSTATE { get; set; }

        ///<summary>
        /// 是否允许少收
        /// </summary>
        [EntityControl("是否允许少收", false, true, 19)]
        [EntityField(20)]
        public bool ISLESSIN { get; set; }

        ///<summary>
        /// 是否允许超收
        /// </summary>
        [EntityControl("是否允许超收", false, true, 20)]
        [EntityField(20)]
        public bool ISOVERIN { get; set; }

        ///是否质检
        /// </summary>
        [EntityControl("是否质检", false, true, 19)]
        [EntityField(20, IsNotNullable = true)]
        public bool ISCHECKPRO { get; set; }
        
        /// <summary>
        /// 包装名称
        /// </summary>
        //[EntityControl("包装名称", false, true, 17)]
        //[EntityForeignKey(typeof(PackUnit), "ID", "PACKNAME")]
        //[EntityField(255)]
        //public string PACKCODE { get; set; }

        //[EntityControl("库存数量",false,true,18)]
        //[EntityForeignKey(typeof(StockInfoMaterials),"ID","NUM")]
        //[EntityField(255)]
        //public string NUM { get; set; }

        //protected override string GetSearchSQL()
        //{
        //    string sql1 =  base.GetSearchSQL();
        //    return sql1;
        //}

        private HierarchicalEntityView<Materials, PackUnitList> _childItems;

        //[HierarchicalEntityControl(isadd = false, isedit = false, isselect = true, c = "Acc.Business.WMS.Controllers.PackUnitController")]
        public HierarchicalEntityView<Materials, PackUnitList> ChildItems
        {
            get
            {
                if (_childItems == null)
                    _childItems = new HierarchicalEntityView<Materials, PackUnitList>(this);
                return _childItems;
            }
        }

        private HierarchicalEntityView<Materials, MaterialUnit> _MaterialUnit;

        [HierarchicalEntityControl(isadd = false, isedit = false, isselect = true, c = "Acc.Business.WMS.Controllers.UnitController")]
        public HierarchicalEntityView<Materials, MaterialUnit> MaterialUnit
        {
            get
            {
                if (_MaterialUnit == null)
                    _MaterialUnit = new HierarchicalEntityView<Materials, MaterialUnit>(this);
                return _MaterialUnit;
            }
        }
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
                pro1.Text = "冻入";
                pro1.Value = "1";
                PropertyValueType pro2 = new PropertyValueType();
                pro2.Text = "冻出";
                pro2.Value = "2";
                PropertyValueType pro3 = new PropertyValueType();
                pro3.Text = "不可用";
                pro3.Value = "3";
                list.Add(pro);
                list.Add(pro1);
                list.Add(pro2);
                list.Add(pro3);
            }
            if (valueArgs.ColumnName == "SHELFUNIT")
            {
                PropertyValueType pro = new PropertyValueType();
                pro.Text = "小时";
                pro.Value = "0";
                PropertyValueType pro1 = new PropertyValueType();
                pro1.Text = "天";
                pro1.Value = "1";
                PropertyValueType pro2 = new PropertyValueType();
                pro2.Text = "月";
                pro2.Value = "2";
                PropertyValueType pro3 = new PropertyValueType();
                pro3.Text = "年";
                pro3.Value = "3";
                list.Add(pro);
                list.Add(pro1);
                list.Add(pro2);
                list.Add(pro3);
            }
            if (valueArgs.ColumnName == "CommodityType")
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
            if (valueArgs.ColumnName == "BATCH")
            {
                PropertyValueType vt = new PropertyValueType();
                vt.Text = "否";
                vt.Value = "0";
                list.Add(vt);
                PropertyValueType vt1 = new PropertyValueType();
                vt1.Text = "是";
                vt1.Value = "1";
                list.Add(vt1);
            }
            if (valueArgs.ColumnName == "OUTWAREHOUSEYPE")
            {
                PropertyValueType vt = new PropertyValueType();
                vt.Text = "任意出库";
                vt.Value = "0";
                list.Add(vt);
                PropertyValueType vt1 = new PropertyValueType();
                vt1.Text = "先入先出";
                vt1.Value = "1";
                list.Add(vt1);
                PropertyValueType vt2 = new PropertyValueType();
                vt2.Text = "先入后出";
                vt2.Value = "2";
                list.Add(vt2);
            }
            return list.ToArray();
        }

        #endregion
    }
}
