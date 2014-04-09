using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Business.WMS.Model;
using Acc.Business.Controllers;
using Way.EAP.DataAccess.Entity;
using Acc.Contract.Data;
using Acc.Business.Model;
using Acc.Contract.Data.ControllerData;
using Acc.Contract.MVC;

namespace Acc.Business.WMS.Controllers
{
    public class MaterialsController : BusinessController
    {
        
        /// <summary>
        /// 描述：产品信息控制器
        /// 作者：路聪
        /// 创建日期:2012-12-18
        /// </summary>
        public MaterialsController() : base(new Materials()) { }
        public MaterialsController(IModel model) : base(model) { }

        //显示在菜单
        protected override string OnControllerName()
        {
            return "产品信息管理";
        }
        //菜单中url路径
        protected override string OnGetPath()
        {
            return "Views/WMS/Materials/Materials.htm";
        }
        //开发人
        protected override string OnGetAuthor()
        {
            return "路聪";
        }
        //说明
        protected override string OnGetControllerDescription()
        {
            return "产品信息管理";
        }

        #region

        //[ActionCommand(name = "冻结", title = "冻结产品", index = 6, isalert = true, icon = "icon-search", onclick = "SetDisable")]
        //public void SetDisable()
        //{ }


        
        #endregion

        #region 初始化数据方法
        protected override void OnInitViewChildItem(Contract.Data.ModelData data, Contract.Data.ItemData item)
        {
            base.OnInitViewChildItem(data, item);
            if (data.name.EndsWith("Materials"))
            {
                switch (item.field.ToLower())
                {
                    case "code":
                        item.title = "商品编码";
                        break;
                    //case "isdisable":
                    //    item.title = "是否禁用";
                    //    item.visible = true;
                    //    break;
                }
            }
            if (data.name.EndsWith("MaterialUnit"))
            {
                data.visible = false;
                switch (item.field.ToLower())
                {
                    case "unitid":
                        item.title = "单位";
                        break;
                }
            }
            if (data.name.EndsWith("PackUnitList"))
            {
                data.visible = false;
                data.title = "商品BOM";
                switch (item.field.ToLower())
                {
                        /*
                    case "packunitcode":
                        item.title = "BOM产品";
                        item.visible = true;
                        break;
                    case "num":
                        item.title = "BOM数量";
                        item.visible = true;
                        break;
                    case "rowindex":
                        item.visible = true;
                        break;
                    case "remark":
                        item.visible = true;
                        break;
                    default:
                        item.visible = false;
                        break;
                         */ 
                }
            }
        }

        #endregion 

        protected override void OnAdding(Contract.MVC.ControllerBase.SaveEvent item)
        {
            base.OnAdding(item);
            Materials material = item.Item as Materials;
            ValidateInfo(material);
        }


        protected override void OnEditing(Contract.MVC.ControllerBase.SaveEvent item)
        {
            base.OnEditing(item);
            Materials material = item.Item as Materials;
            //EditValidateInfo(material);
            ValidateInfo(material);
        }

        /// <summary>
        /// 验证输入的信息是否可行
        /// </summary>
        /// <param name="order"></param>
        private void ValidateInfo(Materials material)
        {
            EntityList<Materials> list = new EntityList<Materials>(this.model.GetDataAction());
            EntityList<Materials> list1 = new EntityList<Materials>(this.model.GetDataAction());
            EntityList<Materials> list2 = new EntityList<Materials>(this.model.GetDataAction());
            EntityList<Materials> list3 = new EntityList<Materials>(this.model.GetDataAction());
            //说明是修改
            if (material.ID != 0)
            {
                list.GetData("Code = '" + material.Code + "' and id<>" + material.ID + "");
                list1.GetData("FNAME ='" + material.FNAME + "' and id<>" + material.ID + "");
                list2.GetData("FMODEL ='" + material.FMODEL + "' and id<>" + material.ID + "");
                list3.GetData("FNAME ='" + material.FNAME + "' and FMODEL ='" + material.FMODEL + "' and id<>" + material.ID + "");
            }
            else
            {
                list.GetData("Code = '" + material.Code + "'");
                list1.GetData("FNAME ='" + material.FNAME + "'");
                list2.GetData("FMODEL ='" + material.FMODEL + "'");
                list3.GetData("FNAME ='" + material.FNAME + "' and FMODEL ='" + material.FMODEL + "'");
            }
            if (list3.Count > 0)
            {
                    throw new Exception("产品商品名称和规格不可重复");
            }
            //if (list.Count > 0 && list1.Count > 0)
            //{
            //    throw new Exception("产品编码不可重复和商品名称不可重复");
            //}
            if (list.Count > 0)
            {
                throw new Exception("产品编码不可重复");
            }
            //else if(list1.Count >0)
            //{
            //    throw new Exception("商品名称不可重复");
            //}
            else if (material.NETWEIGHT < 0)
            {
                throw new Exception("请输入正确的产品净重量");
            }
            else if (material.PRICE < 0)
            {
                throw new Exception("请输入正确的产品价格");
            }
            else if (material.LOWESTSTOCK < 0)
            {
                throw new Exception("请输入正确的产品最低库存量");
            }
            else
            { 
                
            }
        }

        /// <summary>
        /// 修改验证
        /// </summary>
        /// <param name="material"></param>
        private void EditValidateInfo(Materials material)
        {
            if (material.NETWEIGHT < 0)
            {
                throw new Exception("请输入正确的产品净重量");
            }
            else if (material.PRICE < 0)
            {
                throw new Exception("请输入正确的产品价格");
            }
            else if (material.LOWESTSTOCK < 0)
            {
                throw new Exception("请输入正确的产品最低库存量");
            }
            else
            {

            }
        }
        //[ActionCommand(name = "批量打印产品条码", title = "批量选择待打印的产品编码", index = 6, icon = "icon-ok", onclick = "selectRequisition", isselectrow = true)]
        //public void selectPorts()
        //{
        //    //生成界面方法按钮用于权限控制，本方法无代码
        //}
        //[ActionCommand(name = "设置打印模板", title = "设置打印模板", index = 7, icon = "icon-ok", onclick = "SetPrintModel", isselectrow = false)]
        //public void SetPrintModel()
        //{
        //    //生成界面方法按钮用于权限控制，本方法无代码
        //}
    }
}
