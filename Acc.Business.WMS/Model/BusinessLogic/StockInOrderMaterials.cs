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
    /// 描述：入库单明细表
    /// 作者：路聪
    /// 创建日期:2012-12-18
    /// </summary>
    [EntityClassAttribut("Acc_WMS_InOrderMaterials", "入库单明细", IsOnAppendProperty = true)]
    public class StockInOrderMaterials : OrderMaterialsCommon, IPropertyValueType
    {
        public StockInOrderMaterials[] getsi()
        {
            return null;
        }
        /// <summary>
        /// 来源单号
        /// </summary>
        [EntityControl("来源单号", true, true, 1)]
        [EntityForeignKey(typeof(StockInOrder), "ID", "CODE")]
        [EntityField(8)]
        public int PARENTID { get; set; }
        /// <summary>
        /// 亚批次
        /// </summary>
        [EntityControl("亚批次", false, true, 12)]
        [EntityField(255)]
        public string SENBATCHNO { get; set; }
        /// <summary>
        /// 货位
        /// </summary>
        [EntityControl("货位", false, true, 13)]
        [EntityField(255)]
        [EntityForeignKey(typeof(WareHouse), "ID", "Code")]
        public string DEPOTWBS { get; set; }
        /// <summary>
        /// 托盘
        /// </summary>
        [EntityControl("托盘", false, true, 14)]
        [EntityField(255)]
        [EntityForeignKey(typeof(Ports), "ID", "PORTNO")]
        public string PORTNAME { get; set; }

        PropertyValueType[] IPropertyValueType.GetValueType(ValueTypeArgs e)
        {
            List<PropertyValueType> list = new List<PropertyValueType>();
            if (e.ColumnName == "CHECKTYPE")
            {
                PropertyValueType vt = new PropertyValueType();
                vt.Text = "待检验";
                vt.Value = "0";
                list.Add(vt);
                PropertyValueType vt1 = new PropertyValueType();
                vt1.Text = "合格";
                vt1.Value = "1";
                list.Add(vt1);
                PropertyValueType vt2 = new PropertyValueType();
                vt2.Text = "不合格";
                vt2.Value = "2";
                list.Add(vt2);
                PropertyValueType vt3 = new PropertyValueType();
                vt2.Text = "免质检";
                vt2.Value = "3";
                list.Add(vt3);
                //hidisger service 微软大数据展示
            }

            return list.ToArray();
        }


        private HierarchicalEntityView<StockInOrderMaterials, InSequence> _InSequence;
         //<summary>
         //序列码
         //</summary>
        [HierarchicalEntityControl(disabled=false,isadd = true,isedit=true,isremove=true)]
        public HierarchicalEntityView<StockInOrderMaterials, InSequence> InSequence
        {
            get
            {
                if (_InSequence == null)
                {
                    _InSequence = new HierarchicalEntityView<StockInOrderMaterials, InSequence>(this);
                }
                return _InSequence;
            }
        }
    }
}
