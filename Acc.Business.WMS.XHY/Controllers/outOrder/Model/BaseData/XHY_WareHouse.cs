using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Business.Model;
using Acc.Business.WMS.Model;
using Way.EAP.DataAccess.Data;
using Way.EAP.DataAccess.Entity;
using Acc.Contract.MVC;


namespace Acc.Business.WMS.XHY.Model
{
    public class XHY_WareHouse : WareHouse, IPropertyValueType
    {
       
        [EntityControl("是否补货", false, true,1)]
        [EntityField(200)]
        public bool ISReStock { get; set; }

        [EntityControl("最小库存量(KG)", false, true, 2)]
        [EntityField(20,IsNotNullable =true)]
        public double MinNum { get; set; }

        [EntityControl("最大库存量(KG)", false, true, 3)]
        [EntityField(20,IsNotNullable = true)]
        public string ProduceMan { get; set; }
    }
}
