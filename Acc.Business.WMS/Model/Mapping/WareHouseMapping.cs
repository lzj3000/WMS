using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Business.Model.Mapping;
using Way.EAP.DataAccess.Entity;

namespace Acc.Business.WMS.Model.Mapping
{
    /// <summary>
    /// 描述：仓库数据关系
    /// 作者：胡文杰
    /// 创建日期:2013-08-15
    /// </summary>
    [EntityClassAttribut("Acc_R_WareHouseMapping", "仓库数据关系", IsOnAppendProperty = true)]
    public class WareHouseMapping : MappingInfo
    {
        /// <summary>
        /// 本系统ID
        /// </summary>
        [EntityControl("本系统ID", true, true, 1)]
        [EntityForeignKey(typeof(WareHouse), "ID", "ID")]
        [EntityField(10)]
        public int FID { get; set; }

        ///// <summary>
        ///// 本仓位类
        ///// </summary>
        //[EntityControl("本仓位类", true, true, 2)]
        //[EntityField(10)]
        //public int FStockTypeID { get; set; }

                //PropertyValueType proC = new PropertyValueType();
                //proC.Text = "仓库"; proC.Value = "0";
                //PropertyValueType proH = new PropertyValueType();
                //proH.Text = "货区"; proH.Value = "1";
                //PropertyValueType proW = new PropertyValueType();
                //proW.Text = "货位"; proW.Value = "2";
                //PropertyValueType proHC = new PropertyValueType();
                //proHC.Text = "货层"; proHC.Value = "3";
    }
}
