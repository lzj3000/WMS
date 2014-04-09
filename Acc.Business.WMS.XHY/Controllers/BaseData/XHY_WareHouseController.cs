using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Business.Controllers;
using Acc.Business.WMS.Controllers;
using Acc.Business.WMS.Model;
using Acc.Business.WMS.XHY.Model;
using Way.EAP.DataAccess.Data;
using System.Data;
using Way.EAP.DataAccess.Entity;
using Acc.Contract.Data.ControllerData;
using Acc.Contract.Data;
using Acc.Contract.MVC;

namespace Acc.Business.WMS.XHY.Controllers
{
    public class XHY_WareHouseController : WareHouseController
    {
        public XHY_WareHouseController(): base(new WareHouse())
        {
        }

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
        /// <summary>
        /// 仓库管理控制器
        /// 创建人：柳强
        /// 创建时间：2012-12-24
        /// </summary>


        //菜单中url路径
        protected override string OnGetPath()
        {
            return "Views/WMS/WareHouse/XHY_WareHouse.htm";
        }
        protected override void OnInitViewChildItem(ModelData data, ItemData item)
        {
            base.OnInitViewChildItem(data, item);
            if (data.name.EndsWith("WareHouse"))
            {
                switch (item.field.ToLower())
                {
                    case "submiteddate":
                    case "submitedby":
                    //case "createdby":
                    //case "creationdate":
                    case "modifiedby":
                    case "modifieddate":
                    case "reviewedby":
                    case "revieweddate":
                    case "isdelete":
                    case "state":
                    case "id":
                    case "address":
                    case "carphone":
                    case "stocktype":
                    case "sourcecode":
                    case "wbscode":
                    case "type":
                    case "stay1":
                    case "stay2":
                    case "stay3":
                    case "stay4":
                    case "stay5":
                    case "isdelstatus":
                        item.visible = false;
                        break;
                    case "creationdate":
                    case "createdby":
                    case "issubmited":
                    case "isreviewed":
                        item.disabled = true;
                        break;
                    case "remark":
                        item.title = "地址";
                        item.visible = true;
                        item.disabled = true;
                        break;
                    case "parentid":
                        item.title = "父级";
                        item.visible = true;
                        item.disabled = true;
                        break;
                    case "isdisable":
                        item.visible = true;
                        item.disabled = true;
                        break;
                    case "warehousename":
                        item.disabled = false;
                        break;
                    default:
                        item.visible = true;
                        item.disabled = true;
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
                if ( ac.command == "remove")
                {
                    ac.visible = false;
                }
            }
            return coms;
        }


        //[ActionCommand(name = "打印货位条码", title = "批量选择待打印的货位条码", index = 6, icon = "icon-print", onclick = "selectRequisition", isselectrow = true)]
        //public void selectPorts()
        //{
        //    //生成界面方法按钮用于权限控制，本方法无代码
        //}

        /// <summary>
        /// 根据货区查询它下面的货位
        /// </summary>
        [WhereParameter]
        public string areaNameList { get; set; }
        public string GetAreaNameList()
        {
            WareHouse wh = new WareHouse();
            IDataAction action = this.model.GetDataAction();
            string sql = string.Format("select sm.Code,sm.WareHouseName from " + wh.ToString() + " sm where WHTYPE=2 and parentID = '" + areaNameList + "'");
            DataTable dt = action.GetDataTable(sql);
            if (dt.Rows.Count == 0)
            {
                sql = string.Format("select sm.Code,sm.WareHouseName from " + wh.ToString() + " sm where id='"+areaNameList+"'");
                dt = action.GetDataTable(sql);
                
                //throw new Exception("异常：无货位可打印");
            }
            return Acc.Contract.JSON.Serializer(dt);
        }

        [WhereParameter]
        public WareHouseArges args { get; set; }

        public void CreateLocator()
        {
            if (args != null)
            {
                if (this.ActionItem == null)
                    throw new Exception("异常：未选择仓库不能生成货位！请选择仓库后重试。");
                if (this.ActionItem["WHTYPE"].ToString() != "0")
                    throw new Exception("异常：" + this.ActionItem["WAREHOUSENAME"].ToString() + "不是仓库类型不能生成货位！请选择仓库后重试。");
                string tableName = this.ActionItem.ToString();
                string parentid = this.ActionItem["ID"].ToString();
                string yxsql = "select code from " + tableName + " where code='{0}' and parentid={1}";
                string sql = "insert into " + tableName + "(WAREHOUSENAME,WHTYPE,PARENTID,STATUS,type,code,ISDELSTATUS,ISOFFER) values('{0}',2," + parentid + ",0," + parentid + ",'{0}',0,0)";
                string hw = "";
                string hws = "";
                string lws = "";
                for (int i = 0; i < args.hws; i++)
                    hws += "0";
                for (int i = 0; i < args.lws; i++)
                    lws += "0";
                IDataAction action = this.model.GetDataAction();
                action.StartTransation();
                for (int h = args.hs; h < args.he + 1; h++)
                {
                    for (int l = args.ls; l < args.le + 1; l++)
                    {
                        for (int c = 1; c < args.cs + 1; c++)
                        {
                            hw = args.hqz + h.ToString(hws) + args.lqz + l.ToString(lws) + args.cqz + c.ToString();
                            object obj = action.GetValue(string.Format(yxsql, hw, parentid));
                            if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
                                throw new Exception("异常：" + hw + "货位已生成，不能重复生成！");
                            action.Execute(string.Format(sql, hw));
                        }
                    }
                }
                action.Commit();
                action.EndTransation();
            }
        }
        [ActionCommand(name = "禁用", title = "禁用（在系统中不能使用）", index = 3, icon = "icon-remove", isalert = true, isselectrow = true)]
        public void isdisfalse()
        {
            ///获取当前表名称
            string tn = this.ActionItem.ToString();
            ///以下方法可变成多条启用
            IDataAction da = this.model.GetDataAction();
            if (this.ActionItem["ID"] != null)
            {
                string sql = "update " + tn + " set IsDisable = 1 where id = '" + this.ActionItem["ID"] + "'";
                da.Execute(sql);
            }
        }


        [ActionCommand(name = "启用", title = "启用（在系统中可以使用）", index = 8, icon = "icon-ok", isalert = true, isselectrow = true, issplit = true, splitname = "isdisfalse")]
        public void isdistrue()
        {
            ///获取当前表名称
            string tn = this.ActionItem.ToString();
            ///以下方法可变成多条启用
            IDataAction da = this.model.GetDataAction();
            if (this.ActionItem["ID"] != null)
            {
                string sql = "update " + tn + " set IsDisable = 0 where id = '" + this.ActionItem["ID"] + "'";
                da.Execute(sql);
            }
        }
        protected override void OnForeignLoading(IModel model, loadItem item)
        {
            base.OnForeignLoading(model, item);
        }
    }
    public class WareHouseArges
    {
        public WareHouseArges()
        {
            this.hqz = "H";
            this.lqz = "-L";
            this.cqz = "-";
            this.hws = 3;
            this.lws = 3;
        }
        /// <summary>
        /// 行前缀
        /// </summary>
        public string hqz { get; set; }
        /// <summary>
        /// 列前缀
        /// </summary>
        public string lqz { get; set; }
        /// <summary>
        /// 层前缀
        /// </summary>
        public string cqz { get; set; }
        /// <summary>
        /// 行位数
        /// </summary>
        public int hws { get; set; }
        /// <summary>
        /// 列位数
        /// </summary>
        public int lws { get; set; }
        /// <summary>
        /// 行开始数
        /// </summary>
        public int hs { get; set; }
        /// <summary>
        /// 行结束数
        /// </summary>
        public int he { get; set; }
        /// <summary>
        /// 列开始数
        /// </summary>
        public int ls { get; set; }
        /// <summary>
        /// 列结束数
        /// </summary>
        public int le { get; set; }
        /// <summary>
        /// 层数
        /// </summary>
        public int cs { get; set; }
    }
}
