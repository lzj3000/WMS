using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Contract.MVC;
using Acc.Business.WMS.Model;
using Acc.Contract;
using System.Data;
using Acc.Business.Controllers;
using Way.EAP.DataAccess.Entity;
using Acc.Business.WMS.Controllers;
using Way.EAP.DataAccess.Regulation;
using Acc.Business.WMS.XHY.Model;

namespace Acc.Business.WMS.XHY.Controllers
{
    public class CrudeMaterialsController : XHY_MaterialsController
    {
        public CrudeMaterialsController() : base(new XHY_Materials()) { }
        #region 初始化数据方法
        //显示在菜单
        protected override string OnControllerName()
        {
            return "原料商品";
        }
        //菜单中url路径
        protected override string OnGetPath()
        {
            return "Views/WMS/XHY/CrudeMaterials.htm";
        }
        //说明
        protected override string OnGetControllerDescription()
        {
            return "原料管理";

        }
        #endregion

        #region 初始化数据方法
        protected override void OnInitViewChildItem(Contract.Data.ModelData data, Contract.Data.ItemData item)
        {
           
            if (data.name.EndsWith("XHY_Materials"))
            {
                data.title = "原料商品";
                switch (item.field.ToLower())
                {
                        //原料隐藏单重
                    case "weightnum":
                        item.visible = false;
                        break;
                }
            }
            if (data.name == "Acc.Business.Model.PackUnitList")
            {
                data.disabled = true;
            }
            base.OnInitViewChildItem(data, item);

        }

        /// <summary>
        /// 原料
        /// </summary>
        /// <returns></returns>
        public override int SetStockType()
        {
            return 0;
        }
        
        #endregion

       
    }
}
