using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model;
using Acc.Contract.MVC;

namespace Acc.Business.WMS.Model
{
    /// <summary>
    /// 仓库信息类
    /// 创建时间：2012-12-24
    /// 创建人：柳强
    /// </summary>
    [EntityClassAttribut("Acc_WMS_WareHouse","仓库管理",IsOnAppendProperty =true)]
    public class WareHouse : BasicInfo,IPropertyValueType
    {

        [EntityControl("名称",false,true,2)]
        [EntityField(50,IsNotNullable = true)]
        public string WAREHOUSENAME { get; set; }

        [EntityControl("地址", false, true, 3)]
        [EntityField(50, IsNotNullable = false)]
        public string ADDRESS { get; set; }

        [EntityControl("管理员电话", false, true, 4)]
        [EntityField(50)]
        public string PICPHONE { get; set; }


        [EntityControl("类型", false, true, 5)]
        [EntityField(50,IsNotNullable=true)]
        [ValueTypeProperty]
        public int WHTYPE { get; set; }


        [EntityControl("标识类型", false, true, 5)]
        [EntityField(50, IsNotNullable = true)]
        [EntityForeignKey(typeof(WareHouse), "ID", "CODE")]
        public int type { get; set; }

        /// <summary>
        /// 父ID
        /// </summary>
        [EntityControl("上级", false, true, 6)]
        [EntityForeignKey(typeof(WareHouse), "ID", "WAREHOUSENAME")]
        [EntityField(50)]
        public int PARENTID { get; set; }

        /// <summary>
        /// 是否允许0库存
        /// </summary>
        [EntityControl("是否允许0库存", false, false, 7)]
        [EntityField(50, IsNotNullable = true)]
        [ValueTypeProperty]
        public bool ISDELSTATUS { get; set; }

        /// <summary>
        /// 货位状态
        /// </summary>
        [EntityControl("货位状态", false, false, 8)]
        [EntityField(50)]
        [ValueTypeProperty]
        public string STATUS { get; set; }

        /// <summary>
        /// 组织机构
        /// </summary>
        [EntityControl("部门", false, true, 9)]
        [EntityField(50, IsNotNullable = false)]
        [EntityForeignKey(typeof(Organization), "ID", "ORGANIZATIONNAME")]
        public int ORGANIZATIONID { get; set; }

        /// <summary>
        /// 是否添加权限管理
        /// </summary>
        [EntityControl("是否设置管理员", false, true, 9)]
        [EntityField(50, IsNotNullable = true)]
        public bool ISOFFER { get; set; }


        private HierarchicalEntityView<WareHouse, WareHouse> _childItems;

        public HierarchicalEntityView<WareHouse, WareHouse> ChildItems
        {
            get
            {
                if (_childItems == null)
                    _childItems = new HierarchicalEntityView<WareHouse, WareHouse>(this);
                return _childItems;
            }
        }

        //private HierarchicalEntityView<WareHouse, StockInfoMaterials> _childInfo;
        //[HierarchicalEntityControl(disabled = true)]
        //public HierarchicalEntityView<WareHouse, StockInfoMaterials> ChildInfo
        //{
        //    get
        //    {
        //        if (_childInfo == null)
        //            _childInfo = new HierarchicalEntityView<WareHouse, StockInfoMaterials>(this);
        //        return _childInfo;
        //    }
        //}

        private HierarchicalEntityView<WareHouse, OWorker> _childOffice;
        [HierarchicalEntityControl(isselect = true, c = "Acc.Business.WMS.XHY.Controllers.XHY_OfficersController", isadd = false, isedit = false)]
        public HierarchicalEntityView<WareHouse, OWorker> ChildOffice
        {
            get
            {
                if (_childOffice == null)
                    _childOffice = new HierarchicalEntityView<WareHouse, OWorker>(this);
                return _childOffice;
            }
        }

      
        #region IPropertyValueType 成员

        PropertyValueType[] IPropertyValueType.GetValueType(ValueTypeArgs valueArgs)
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
            if (valueArgs.ColumnName == "WHTYPE")
            {
                PropertyValueType proC = new PropertyValueType();
                proC.Text = "仓库"; proC.Value = "0";
                PropertyValueType proH = new PropertyValueType();
                proH.Text = "货区"; proH.Value = "1";
                PropertyValueType proW = new PropertyValueType();
                proW.Text = "货位"; proW.Value = "2";
                PropertyValueType proHC = new PropertyValueType();
                proHC.Text = "虚拟仓库"; proHC.Value = "3";
                list.Add(proC);
                list.Add(proH);
                list.Add(proW);
                list.Add(proHC);
            }

            if (valueArgs.ColumnName == "ISOFFER")
            {
                PropertyValueType pro3 = new PropertyValueType();
                pro3.Text = "否";
                pro3.Value = "0";
                PropertyValueType pro4 = new PropertyValueType();
                pro4.Text = "是";
                pro4.Value = "1";
                list.Add(pro3);
                list.Add(pro4);
            }
           
            return list.ToArray();
        }

        #endregion
    }
}
