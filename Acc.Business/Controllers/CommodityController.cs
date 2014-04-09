using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Business.Model;
using Way.EAP.DataAccess.Data;

namespace Acc.Business.Controllers
{
    /// <summary>
    /// 商品控制器
    /// </summary>
    public class CommodityController : BusinessController
    {
        public CommodityController() : base(new BusinessCommodity()) {
        
            
        }
    }
}
