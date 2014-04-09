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
    public class SemMaterialsController : XHY_MaterialsController
    {
        public SemMaterialsController() : base(new XHY_Materials()) { }

        #region 初始化数据方法
        //显示在菜单
        protected override string OnControllerName()
        {
            return "半成品商品";
        }
        //菜单中url路径
        protected override string OnGetPath()
        {
            return "Views/WMS/XHY/SemMaterials.htm";
        }
        //说明
        protected override string OnGetControllerDescription()
        {
            return "半成品商品管理";
        }
        #endregion

        #region 初始化数据方法
        protected override void OnInitViewChildItem(Contract.Data.ModelData data, Contract.Data.ItemData item)
        {
            base.OnInitViewChildItem(data, item);
            if (data.name.EndsWith("XHY_Materials"))
            {
                data.title = "半成品商品";
                switch (item.field.ToLower())
                {
                    //原料隐藏单重
                    case "weightnum":
                        item.visible = false;
                        break;
                }
            }
        
        }

        /// <summary>
        /// 半成品
        /// </summary>
        /// <returns></returns>
        public override int SetStockType()
        {
            return 1;
        }
        #endregion

        protected override void OnForeignLoading(IModel model, Contract.Data.ControllerData.loadItem item)
        {
            base.OnForeignLoading(model, item);
            if (this.fdata.filedname == "PACKUNITCODE")
            {
                item.rowsql = "select * from Acc_Bus_BusinessCommodity a where a.CommodityType='1'";
            }
        }
    }
}
