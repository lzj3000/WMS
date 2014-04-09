using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Business.Controllers;
using Acc.Business.WMS.Model;
using Acc.Contract.Data;

namespace Acc.Business.WMS.Controllers
{
    public class GroupTrayController : BusinessController
    {
        public GroupTrayController() : base(new GroupTray()) { }
        /// <summary>
        /// 显示在菜单
        /// </summary>
        /// <returns></returns>
        protected override string OnControllerName()
        {
            return "组盘管理";
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
                return false;
            }
        }

        /// <summary>
        /// 菜单中URL路径
        /// </summary>
        /// <returns></returns>
        protected override string OnGetPath()
        {
            return "Views/WMS/Ports/GroupTray.htm";
        }

        /// <summary>
        /// 开发人
        /// </summary>
        /// <returns></returns>
        protected override string OnGetAuthor()
        {
            return "张朝阳";
        }
        protected override void OnForeignIniting(Contract.MVC.IModel model, InitData data)
        {
            object o = this;
            base.OnForeignIniting(model, data);
        }
        protected override void OnInitViewChildItem(ModelData data, ItemData item)
        {
            base.OnInitViewChildItem(data, item);
            if (data.name.EndsWith("GroupTray"))
            {
                if (item.IsField("Gname"))
                {
                    if (item.foreign != null)
                    {                       
                        item.foreign.rowdisplay.Add("Code", "GCODE");
                        item.foreign.rowdisplay.Add("FMODEL", "FMODEL");
                        item.foreign.rowdisplay.Add("FUNITID", "FUNITID");
                    }
                }
                switch (item.field.ToLower())
                {
                    case "ordercode":
                         item.index = 0;
                         item.visible = true;
                         item.title = "生产赋码单号";
                        break;
                    case "whname":
                         item.index = 1;
                         item.visible = true;
                         break;
                    case "traycode":
                         item.index = 2;
                         item.visible = true;
                         break;
                    case "gname":                        
                        item.visible = true;
                        item.index = 3;
                        break;
                    case "gcode":                       
                        item.visible = true;
                        item.index = 4;
                        break;
                    case "batchno":
                         item.index = 5;
                         item.visible = true;
                         break;
                    case "traynum":
                         item.index = 6;
                         item.visible = true;
                         break;
                    case "fmodel":
                         item.index = 7;
                         item.visible = true;
                         item.title = "单件码";
                         break;
                    case "funitid":                   
                         item.index = 8;
                         item.visible = true;
                         break;                       
                    default:
                        item.visible = false;
                        break;
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

            }
        }

        /// <summary>
        /// 说明
        /// </summary>
        /// <returns></returns>
        protected override string OnGetControllerDescription()
        {
            return "组盘管理";
        }

        protected override void OnAdding(Contract.MVC.ControllerBase.SaveEvent item)
        {
            base.OnAdding(item);
        }

        protected override void OnEditing(Contract.MVC.ControllerBase.SaveEvent item)
        {
            base.OnEditing(item);
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
                if (ac.command == "UnSubmitData")
                {
                    ac.visible = false;
                }
                if (ac.command == "add")
                {
                    ac.visible = false;
                }
                if (ac.command == "edit")
                {
                    ac.visible = false;
                }

                if (ac.command == "SubmitData")
                {
                    ac.visible = false;
                }
            }
            return coms;
        }
    }
}
