using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;

namespace Acc.Business.Model
{
    public class CLG : BusinessBase
    {
        [EntityForeignKey(typeof(BusinessCommodity), "ID", "FNAME")]
        [EntityField(80)]
        public int COMMODITYID { get; set; }
        [EntityForeignKey(typeof(CommodityKey), "ID", "KEYNAME")]
        [EntityField(80)]
        public int KEYID { get; set; }
    }
    /// <summary>
    /// 商品逻辑主键
    /// </summary>
    public class CommodityKey : BusinessBase
    {
        /// <summary>
        /// 主键名称
        /// </summary>
        [EntityControl("属性名称", false, true,1)]
        [EntityField(80)]
        public string KEYNAME { get; set; }
        /// <summary>
        /// 主键属性类型
        /// </summary>
        [EntityControl("属性类型", false, true, 2)]
        [EntityField(80)]
        public string KEYTYPE { get; set; }

        private HierarchicalEntityView<CommodityKey, CommodityKeyItems> _items;

        public HierarchicalEntityView<CommodityKey, CommodityKeyItems> Items
        {
            get
            {
                if (_items == null)
                    _items = new HierarchicalEntityView<CommodityKey, CommodityKeyItems>(this);
                return _items;
            }

        }
    }
    /// <summary>
    /// 商品逻辑主键成员
    /// </summary>
    public class CommodityKeyItems : BusinessBase
    {
        /// <summary>
        /// 成员名称
        /// </summary>
        [EntityControl("成员名称", false, true, 1)]
        [EntityField(80)]
        public string KEYTEXT { get; set; }
        /// <summary>
        /// 成员值
        /// </summary>
        [EntityControl("成员值", false, true, 2)]
        [EntityField(80)]
        public string KEYVALUE { get; set; }
    }
}
