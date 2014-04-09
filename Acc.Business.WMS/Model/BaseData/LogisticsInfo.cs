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
    /// 描述：物流公司
    /// 作者：柳强
    /// 创建日期:2013-03-19
    /// </summary>
    [EntityClassAttribut("Acc_WMS_LogisticsInfo", "物流公司", IsOnAppendProperty = true)]
    public class LogisticsInfo : BasicInfo
    {
       
        /// <summary>
        /// 公司名称
        /// </summary>
        [EntityControl("公司名称", false, true, 2)]
        [EntityField(255, IsNotNullable = true)]
        public string LOGISTICSNAME { get; set; }

        /// <summary>
        /// 运单查询地址
        /// </summary>
        [EntityControl("运单查询地址", false, true, 3)]
        [EntityField(255)]
        public string LOGISTICSURL{ get; set; }

        /// <summary>
        /// 负责人电话
        /// </summary>
        [EntityControl("负责人电话", false, true, 4)]
        [EntityField(255, IsNotNullable = false)]
        public string TEL { get; set; }

        private HierarchicalEntityView<LogisticsInfo, ShippingOrder> _ShippingOrder;

        [HierarchicalEntityControl(disabled=true)]
        public HierarchicalEntityView<LogisticsInfo, ShippingOrder> ShippingOrder
        {
            get {
                if (_ShippingOrder == null)
                {
                    _ShippingOrder = new HierarchicalEntityView<LogisticsInfo, ShippingOrder>(this);
                }
                return _ShippingOrder;
            }
        }

        private HierarchicalEntityView<LogisticsInfo, StockOutOrder> _StockOutOrder;

        [HierarchicalEntityControl(disabled = true)]
        public HierarchicalEntityView<LogisticsInfo, StockOutOrder> StockOutOrder
        {
            get
            {
                if (_StockOutOrder == null)
                {
                    _StockOutOrder = new HierarchicalEntityView<LogisticsInfo, StockOutOrder>(this);
                }
                return _StockOutOrder;
            }
        }
        
    }
}
