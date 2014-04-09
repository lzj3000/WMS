using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Way.EAP.DataAccess.Entity;
using Acc.Contract.MVC;

namespace Acc.Business.Model
{
    /// <summary>
    /// 计量单位
    /// </summary>
    [EntityClassAttribut("Acc_Bus_Unit", "计量单位")]
    public class Unit : BasicInfo
    {
        /// <summary>
        /// 计量单位名称
        /// </summary>
        [EntityControl("单位名称", false, true, 1)]
        [EntityField(50)]
        public string UNITNAME { get; set; }
       
    }
}
