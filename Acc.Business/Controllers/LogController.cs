using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Contract.Outside;
using Acc.Business.Controllers;
using Acc.Contract.Data;
using Way.EAP.DataAccess.Data;
using Way.EAP.DataAccess.Entity;
using Acc.Contract.MVC;
using System.Data;
using Acc.Business.Model.Interface;

namespace Acc.Business.Controllers
{
    public class LogController : Acc.Business.Controllers.BusinessController
    {
        public LogController() : base(new LogData()) { }

        //显示在菜单
        protected override string OnControllerName()
        {
            return "接口日志";
        }
        //菜单中url路径
        protected override string OnGetPath()
        {
            return "Views/manager/InterfaceLog.htm";
        }
        //开发人
        protected override string OnGetAuthor()
        {
            return "胡文杰";
        }
        //说明
        protected override string OnGetControllerDescription()
        {
            return "接口日志";
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
        /// 控制按钮
        /// </summary>
        protected override ActionCommand[] OnInitCommand(ActionCommand[] commands)
        {
            //获取所有按钮集合
            foreach (ActionCommand ac in commands)
            {
                if (ac.command == "remove" || ac.command == "add" || ac.command == "edit" || ac.command == "SubmitData")
                {
                    ac.visible = false;
                }
            }
            return base.OnInitCommand(commands);
        }

        /// <summary>
        /// 控制单据属性
        /// </summary>
        protected override void OnInitViewChildItem(ModelData data, ItemData item)
        {
            base.OnInitViewChildItem(data, item);
            //if (data.name.EndsWith("MappingInfo"))
            //{
            //    switch (item.field.ToLower())
            //    {
            //        case "targettable":
            //        case "targetmodel":
            //        case "sourcetable":
            //        case "sourcemodel":
            //        case "maxupdatetime":
            //        case "maxtimestamp":
            //        case "isvisible":
            //            item.disabled = true;
            //            item.visible = false;
            //            break;
            //        //case "templateid":
            //        case "mappinginfoname":
            //        case "sourcesystem":
            //        case "targetsystem":
            //        case "xmlname":
            //        case "transformcount":
            //        case "successcount":
            //        case "remark":
            //            item.disabled = true;
            //            break;
            //        default:
            //            break;
            //    }
            //}
            if (data.name.EndsWith("LogData"))
            {
                switch (item.field.ToLower())
                {
                    case "targettable":
                    case "sourcetable":
                    case "sourceid":
                    case "targetid":
                    case "xmlname":
                    case "isok":
                    case "sourcesystem":
                    case "targetsystem":
                    case "mappinginfoid":
                        item.disabled = true;
                        item.visible = false;
                        break;
                    case "logname":
                    case "sourcecode":
                    case "targetcode":
                    case "operatedate":
                    case "remark":
                        item.disabled = true;
                        break;
                    default:
                        break;
                }
            }

        }


        ///// <summary>
        ///// 转换数据
        ///// </summary>
        //[ActionCommand(name = "同步数据", title = "同步数据", index = 1, icon = "icon-ok", isselectrow = false)]
        //public void TransformData()
        //{
        //    LogData log = this.ActionItem as LogData;
        //    if(log.IsOk==1)
        //    {
        //        throw new Exception("数据已经同步成功！");
        //    }
        //    //if (string.IsNullOrEmpty(log.XmlName)||string.IsNullOrEmpty(log.LogName)||log.TargetID<1)
        //    if (string.IsNullOrEmpty(log.LogName) || log.TargetID < 1)
        //    { return; }

        //    switch (log.LogName)
        //    {
        //        case "外购入库单":
        //            RStockInOrderMappingController wgrk = new RStockInOrderMappingController();
        //            wgrk.TransformData(log.TargetID);
        //            break;
        //        case "半成品入库单":
        //            RSemInOrderMappingController bcprk = new RSemInOrderMappingController();
        //            bcprk.TransformData(log.TargetID);
        //            break;
        //        case "成品入库单":
        //            RProduceInOrderMappingController cprk = new RProduceInOrderMappingController();
        //            cprk.TransformData(log.TargetID);
        //            break;
        //        case "其他入库单":
        //            RRedStockOutOrderMappingController qtrk = new RRedStockOutOrderMappingController();
        //            qtrk.TransformData(log.TargetID);
        //            break;


        //        case "销售出库单":
        //            RStockOutOrderMappingController xsck = new RStockOutOrderMappingController();
        //            xsck.TransformData(log.TargetID);
        //            break;
        //        case "生产出库单":
        //            RPickOutOrderMappingController scck = new RPickOutOrderMappingController();
        //            scck.TransformData(log.TargetID);
        //            break;
        //        case "其他出库单":
        //            ROtherOutOrderMappingController qtck = new ROtherOutOrderMappingController();
        //            qtck.TransformData(log.TargetID);
        //            break;

        //    }


        //}


    }
}
