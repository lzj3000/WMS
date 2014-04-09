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
using Way.EAP.DataAccess.Data;
using Acc.Business.Model;
using Acc.Contract.Data.ControllerData;

namespace Acc.Business.WMS.XHY.Controllers
{
    public class XHY_MaterialsController : MaterialsController
    {
        public XHY_MaterialsController() : base(new XHY_Materials()) { }
        public XHY_MaterialsController(IModel model) : base(model) { }

        public override bool IsSubmit
        {
            get
            {
                return false;
            }
        }
        public override bool IsPrint
        {
            get
            {
                return false;
            }
        }
        public override bool IsClearAway
        {
            get
            {
                return false ;
            }
        }
        #region 初始化数据方法
        protected override void OnInitViewChildItem(Contract.Data.ModelData data, Contract.Data.ItemData item)
        {
            base.OnInitViewChildItem(data, item);
            if (data.name.EndsWith("XHY_Materials"))
            {
                switch (item.field.ToLower())
                {
                    //原料隐藏单重
                    case "isreserve":
                    case "conversion":
                    case "loweststock":
                    case "outwarehouseype":
                    case "storeamount":
                    case "shelfunit":
                    case "alarmearlieramount":
                    case "islessin":
                    case "isoverin":
                    case "customean13":
                        item.visible = false;
                        break;
                    case "rowindex":
                    case "code":
                    case "createdby":
                    case "creationdate":
                        item.visible = true;
                        item.disabled = true;
                        break;
                    case "shelflife":
                        item.title = "保质期(天)";
                        item.visible = true;
                        item.disabled = false;
                        break;

                }
            }
         
        }

        protected override void OnAdding(ControllerBase.SaveEvent item)
        {
            base.OnAdding(item);
            Materials ms = item.Item as Materials;
            ms.CommodityType = SetStockType();
        }
        protected override void OnEditing(ControllerBase.SaveEvent item)
        {
            base.OnEditing(item);
            Materials ms = item.Item as Materials;
            ms.CommodityType = SetStockType();
        }
        #endregion

        /// <summary>
        /// 添加不同类型
        /// </summary>
        /// <param name="item"></param>
        /// <param name="stockType"></param>
        public virtual int SetStockType()
        {
            return -1;
        }

        /// <summary>
        /// 查询按钮，并添加查询条件
        /// </summary>
        /// <param name="m"></param>
        /// <param name="where"></param>
        protected override void OnGetWhereing(IModel m, List<SQLWhere> where)
        {
            base.OnGetWhereing(m, where);
            if (m is Materials)
            {
                SQLWhere w = new SQLWhere();
                w.ColumnName = "CommodityType";
                w.Value = SetStockType().ToString();
                where.Add(w);
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
            
            }
            return coms;
        }

        [ActionCommand(name = "禁用", title = "禁用（在系统中不能使用）", index = 8, icon = "icon-remove", isalert = true, isselectrow = true, onclick = "aa")]
        public void isdisfalse()
        {
            ///获取当前表名称
            //string tn = this.ActionItem.ToString();
            /////以下方法可变成多条启用
            //IDataAction da = this.model.GetDataAction();
            //if (this.ActionItem["ID"] != null)
            //{
            //    string sql = "update " + tn + " set IsDisable = 1 where id = '" + this.ActionItem["ID"] + "'";
            //    da.Execute(sql);
            //}
        }

        [WhereParameter]
        public string ids { get; set; }

        //禁用此项
        public void bb()
        {
            IDataAction da = this.model.GetDataAction();
            string[] StrParameterList = ids.Split(';');
            for (int i = 0; i < StrParameterList.Length; i++)
            {
                string sql = string.Format("update Acc_Bus_BusinessCommodity set IsDisable = 1 where id = '{0}'" 
                    ,StrParameterList[i].ToString());
                da.Execute(sql);
            }
        }



        [ActionCommand(name = "启用", title = "启用（在系统中可以使用）", index = 8, icon = "icon-ok", isalert = true, isselectrow = true, onclick = "cc")]
        public void isdistrue()
        {
            /////获取当前表名称
            //string tn = this.ActionItem.ToString();
            /////以下方法可变成多条启用
            //IDataAction da = this.model.GetDataAction();
            //if (this.ActionItem["ID"] != null)
            //{
            //    string sql = "update " + tn + " set IsDisable = 0 where id = '" + this.ActionItem["ID"] + "'";
            //    da.Execute(sql);
            //}
        }

        //启用此项
        public void dd()
        {
            IDataAction da = this.model.GetDataAction();
            string[] StrParameterList = ids.Split(';');
            for (int i = 0; i < StrParameterList.Length; i++)
            {
                string sql = string.Format("update Acc_Bus_BusinessCommodity set IsDisable =0 where id = '{0}'"
                    , StrParameterList[i].ToString());
                da.Execute(sql);
            }
        }
    }
}
