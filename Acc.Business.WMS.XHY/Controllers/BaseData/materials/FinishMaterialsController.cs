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
using Acc.Contract.Data;

namespace Acc.Business.WMS.XHY.Controllers
{
    public class FinishMaterialsController : XHY_MaterialsController
    {
        public FinishMaterialsController() : base(new XHY_Materials()) { }
        #region 初始化数据方法
        //显示在菜单
        protected override string OnControllerName()
        {
            return "成品商品";
        }
        //菜单中url路径
        protected override string OnGetPath()
        {
            return "Views/WMS/XHY/FinishMaterials.htm";
        }
        //说明
        protected override string OnGetControllerDescription()
        {
            return "成品商品管理";
        }
        #endregion

        #region 初始化数据方法
        protected override void OnInitViewChildItem(Contract.Data.ModelData data, Contract.Data.ItemData item)
        {
            base.OnInitViewChildItem(data, item);
            if (data.name.EndsWith("XHY_Materials"))
            {
                data.title = "成品商品";
                switch (item.field.ToLower())
                {
                    case "ffullname":
                    case "fname":
                    case "classid":
                    case "funitid":
                    case "commoditytype":
                    case "fmodel":
                    case "batch":
                    case "status":
                    case "numstate":
                    case "isdisable":
                        item.visible = true;
                        item.disabled = true;
                        break;
                    case "sequencecode":
                        item.visible = true;
                        break;
                    case "ischeckpro":
                        item.visible = false;
                        break;

                }
            }
        }

        /// <summary>
        /// 设置按钮显示名称
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        protected override ActionCommand[] OnInitCommand(ActionCommand[] commands)
        {
            ActionCommand[] coms = base.OnInitCommand(commands);
            //获取所有按钮集合
            foreach (ActionCommand ac in coms)
            {
                if (ac.command == "add")
                {
                    ac.visible = false;
                }
                if (ac.command == "remove")
                {
                    ac.visible = false;
                }
                if (ac.command == "edit")
                {
                    ac.visible = true;
                }
            }
            return coms;
        }

        /// <summary>
        /// 成品商品
        /// </summary>
        /// <returns></returns>
        public override int SetStockType()
        {
            return 2;
        }

        #endregion

        protected override void OnForeignLoading(IModel model, Contract.Data.ControllerData.loadItem item)
        {
            base.OnForeignLoading(model, item);
            if (this.fdata.filedname == "PACKUNITCODE")
            {
                item.rowsql = "select * from Acc_Bus_BusinessCommodity a where a.CommodityType='2'";
            }
        }

        protected override void OnEditing(ControllerBase.SaveEvent item)
        {
            //base.OnEditing(item);
        }
    }
}
