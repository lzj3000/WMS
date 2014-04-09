using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Acc.Business.Model;
using Acc.Contract.Center;
using Acc.Contract.Data;
using Acc.Contract.Data.ControllerData;
using Acc.Contract.MVC;
using Way.EAP.DataAccess.Entity;

namespace Acc.Business.Controllers
{
    public class ObserverManageController : ControllerBase
    {
        public ObserverManageController() : base(new ObserverManage()) { }
        public override bool IsClearAway
        {
            get
            {
                return false;
            }
        }
        protected override Contract.Data.ActionCommand[] OnInitCommand(Contract.Data.ActionCommand[] commands)
        {
            ActionCommand[] cmds = base.OnInitCommand(commands);
            foreach (ActionCommand c in cmds)
            {
                if (c.command == "qztc")
                {
                    c.visible = true;
                }
                else
                    c.visible = false;
            }
            return cmds;
        }
        [ActionCommand(name = "初始外系统接口", title = "初始外系统接口控制器", index = 1, icon = "icon-search", isalert = false,isselectrow=false)]
        public void qztc()
        {
            OutsideSystemCenter.GetCenter.initOutsideSystem();
        }
        protected override Contract.Data.ControllerData.ReadTable OnSearchData(Contract.Data.ControllerData.loadItem item)
        {
           
            ReadTable table = new ReadTable();
            EntityList<ObserverManage> list = new EntityList<ObserverManage>();
            ObserverClient[] ocs = CommandEventCenter.GetCenter.GetObserverClientList();
            foreach (ObserverClient o in ocs)
            {
                ObserverManage om = new ObserverManage();
                om.SourceController = o.SourceController.SystemInit().title;
                Type type = ControllerCenter.GetCenter.getType(o.ControllerName);
                if (type != null)
                {
                    IController ic = Activator.CreateInstance(type) as IController;
                    om.ControllerName = ic.ControllerName();
                    om.IsAsynchronous = o.IsAsynchronous;
                    om.IsErrorStop = o.IsErrorStop;
                    if (string.IsNullOrEmpty(o.MethodName))
                        om.MethodName = "全部";
                    else
                    {
                       InitData data= ic.SystemInit();
                       foreach (ActionCommand ac in data.commands)
                       {
                           if (ac.command == o.MethodName)
                           {
                               om.MethodName = ac.name;
                               break;
                           }
                       }
                       if (string.IsNullOrEmpty(om.MethodName))
                           om.MethodName = o.MethodName + "(异常！该方法无法观察。)";
                    }
                    om.ObserverType = Enum.GetName(typeof(CustomeEventType), o.ObserverType);
                }
                list.Add(om);
            }
            table.rows = list.ToTable();
            if (table.rows != null)
            {
                if (item.whereList != null && item.whereList.Length > 0 && table.rows != null && table.rows.Rows.Count > 0)
                {
                    string w = this.GetWhereSQL(item, this.model);
                    w = w.Replace("ObserverManage.", "");
                    DataRow[] rows = table.rows.Select(w);
                    if (rows != null && rows.Length > 0)
                    {
                        DataTable t = table.rows.Clone();
                        foreach (DataRow r in rows)
                            t.ImportRow(r);
                        table.rows = t;
                    }
                }
                foreach (DataColumn c in table.rows.Columns)
                {
                    c.ColumnName = c.ColumnName.ToUpper();
                }
                table.total = list.Count;
            }
            return table;
        }
    }
}
