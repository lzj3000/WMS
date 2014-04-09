using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Contract.MVC;
using Acc.Business.Model;

namespace Acc.Business.WMS.Model
{
    /// <summary>
    /// 往来企业(新华杨客户）
    /// </summary>
    public class XHY_Customer : BusinessCustomer
    {
        /// <summary>
        /// 购货单位联系人
        /// </summary>
        [EntityControl("购货单位联系人", false, true, 3)]
        [EntityField(100)]
        public string LXRName { get; set; }

        /// <summary>
        /// 联系电话  
        /// </summary>
        [EntityControl("移动电话", false, true, 4)]
        [EntityField(100)]
        public string LXRPhone { get; set; }

        /// <summary>
        /// 联系电话  
        /// </summary>
        [EntityControl("购货单位地址", false, true, 5)]
        [EntityField(100)]
        public string LXRAddress { get; set; }

        /// <summary>
        /// 联系电话  
        /// </summary>
        [EntityControl("联系电话", false, true, 6)]
        [EntityField(100)]
        public string zdtel { get; set; }

    }
}
