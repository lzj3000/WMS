using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Business.Model;

namespace Acc.Business.Controllers
{
    /// <summary>
    /// 商品类别控制器
    /// </summary>
    public class CommodityClassController : BusinessController
    {
        public CommodityClassController()
            : base(new CommodityClass())
        {
        
            
        }
        protected override void OnInitViewChildItem(Contract.Data.ModelData data, Contract.Data.ItemData item)
        {
            base.OnInitViewChildItem(data, item);

        }
        protected override string OnGetPath()
        {
            return "Views/manager/commodityclass.htm";
        }
    }
}
