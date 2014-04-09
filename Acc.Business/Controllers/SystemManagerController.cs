using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Acc.Business.Model.Purview;
using Acc.Contract;
using Acc.Contract.Data;
using Acc.Contract.Data.ControllerData;
using Acc.Contract.MVC;
using Way.EAP.DataAccess.Entity;
using Acc.Contract.Center;
using Acc.Business.Model;

namespace Acc.Business.Controllers
{
    public class SystemManagerController : ControllerBase
    {
        public override bool IsClearAway
        {
            get
            {
                return false;
            }
        }
        public SystemManagerController() : base(new SystemUser()) { }
        protected override Contract.Data.ActionCommand[] OnInitCommand(Contract.Data.ActionCommand[] commands)
        {
            ActionCommand[] cmds = base.OnInitCommand(commands);
            foreach (ActionCommand c in cmds)
            {
                if (c.command == "qztc" || c.command == "showmsg" || c.command == "logcz")
                {
                    c.visible = true;
                }
                else
                    c.visible = false;
            }
            return cmds;
        }
        [ActionCommand(name = "强制退出", title = "强制选择的用户退出系统", index = 3, icon = "icon-remove", isalert = true)]
        public void qztc()
        {
            string id = this.ActionItem["ID"].ToString();
            if (!string.IsNullOrEmpty(id))
            {
                AcctrueUser u = UserCenter.GetCenter.GetUser(id);
                if (u != null)
                {
                    UserCenter.GetCenter.RemoveUser(u);
                }
            }
        }
        [WhereParameter]
        public string msg { get; set; }
        [ActionCommand(name = "发送消息", title = "发送管理员信息给所有登陆用户", index = 2, icon = "icon-ok", isalert = false, isselectrow = false)]
        public void showmsg()
        {
            AcctrueUser[] us = UserCenter.GetCenter.GetUser();
            foreach (AcctrueUser au in us)
            {
                UserMessgae um = new UserMessgae();
                um.IsTask = false;
                um.MsgTitle = "管理员信息";
                um.MsgDesc = msg;
                um.RecipientID = au.ID;
                um.RecipientName = au.name;
                um.SenderID = this.user.ID;
                um.SenderName = this.user.name;
                um.IsAlert = true;
                ControllerCenter.GetCenter.SendMessage(um);
            }
        }
        private string logid { get; set; }
        private EventBase logitem { get; set; }
        OverseeModel om;
        ObserverClient client;
        [ActionCommand(name = "监视操作", title = "监视选中用户的操作过程", index = 4, icon = "icon-search", isalert = false, isselectrow = true)]
        public void logcz()
        {
            if (this.ActionItem != null)
            {
                SystemUser uu = this.ActionItem as SystemUser;
                if (uu != null)
                {
                    client = new ObserverClient();
                    client.ControllerName = "*";
                    client.SourceController = this;
                    client.ObserverType = CustomeEventType.All;
                    logid = uu.ID.ToString();
                    client.InceptNotify = new InceptNotifyHander(nInceptNotify);
                    CommandEventCenter.GetCenter.RegisterObserver(client);
                    Way.EAP.Logging.LoggingFactory.IsSynchronized = true;
                    Way.EAP.Logging.LoggingFactory.DataAccessOperate -= new Way.EAP.Logging.DataAccessOperateHandler(LoggingFactory_DataAccessOperate);
                    Way.EAP.Logging.LoggingFactory.DataAccessOperate += new Way.EAP.Logging.DataAccessOperateHandler(LoggingFactory_DataAccessOperate);
                }
            }
        }
        public void unLogcz()
        {
            client = new ObserverClient();
            client.ControllerName = "*";
            client.SourceController = this;
            CommandEventCenter.GetCenter.UnRegisterObserver(client);
            Way.EAP.Logging.LoggingFactory.DataAccessOperate -= new Way.EAP.Logging.DataAccessOperateHandler(LoggingFactory_DataAccessOperate);
            client = null;
        }
        void nInceptNotify(EventBase item)
        {
            if (item.user.ID == logid)
            {
                string c = ((IController)item.Controller).ControllerName();
                if (om == null)
                {
                    om = new OverseeModel();
                    om.Controller = c;
                    om.MethodName = item.MethodName;
                    om.User = item.user.name;
                }
                else
                {
                    if (om.Controller == c && om.MethodName == item.MethodName)
                    {
                        if (item.EventType == CustomeEventType.Start)
                        {
                            om.Details.Clear();
                        }
                    }
                    else
                    {
                        om = new OverseeModel();
                        om.Controller = c;
                        om.MethodName = item.MethodName;
                        om.User = item.user.name;
                    }
                }
                switch (item.EventType)
                {
                    case CustomeEventType.Start:
                        om.State = "开始";
                        om.StartTime = item.StartTime.ToLongTimeString();
                        om.StopTime = "";
                        om.Time = "";
                        om.ErrorMsg = "";
                        break;
                    case CustomeEventType.Complete:
                        om.State = "完成";
                        om.StopTime = item.StopTime.ToLongTimeString();
                        om.Time = (item.StopTime - DateTime.Parse(om.StartTime)).TotalSeconds.ToString() + "秒";
                        break;
                    case CustomeEventType.Error:
                        om.State = "执行异常";
                        om.StopTime = item.StopTime.ToLongTimeString();
                        om.Time = (item.StopTime - DateTime.Parse(om.StartTime)).TotalSeconds.ToString() + "秒";
                        om.ErrorMsg = item.CustomeException.Message;
                        break;
                }
                if (item.CustomeData == null&&item.Controller!=null)
                {
                    BusinessBase eb = item.Controller.model as BusinessBase;
                    if (eb != null)
                    {
                        logitem = item;
                        eb.setLog(true);
                    }
                }
                if (item.CustomeData is BusinessBase)
                {
                    logitem = item;
                    ((BusinessBase)item.CustomeData).setLog(true);
                }
                ControllerCenter.GetCenter.SendMessage(this.user.ID, JSON.Serializer(om));
            }
        }
    
        void LoggingFactory_DataAccessOperate(Way.EAP.Logging.LogDataAccess log)
        {
            if (logitem != null && om != null)
            {
                OverseeModelDetails omd = new OverseeModelDetails();
                omd.CancelSQL = log.CancelSQL;
                omd.ExecuteIndex = om.Details.Count + 1;
                omd.ExecuteSQL = log.ExecuteSQL;
                omd.OperateType = log.OperateType;
                omd.TableName = log.TableName;
                om.Details.Add(omd);
            }
        }
        protected override void OnInitViewChildItem(ModelData data, ItemData item)
        {
            base.OnInitViewChildItem(data, item);
            if (item.type == "date")
            {
                item.issearch = false;
            }
        }
       
        protected override Contract.Data.ControllerData.ReadTable OnSearchData(Contract.Data.ControllerData.loadItem item)
        {
            ReadTable table = new ReadTable();
            EntityList<SystemUser> list = new EntityList<SystemUser>();
            AcctrueUser[] us = UserCenter.GetCenter.GetUser();
            foreach (AcctrueUser u in us)
            {
                SystemUser uu = new SystemUser();
                uu.ID = int.Parse(u.ID);
                uu.bm = u.OrganizationName;
                uu.ry = u.name;
                uu.dlsj = u.LoginTime;
                uu.IP = u.IP;
                uu.LType = u.LoginType;
                if (u.ReadData != null)
                {
                    uu.czmk = u.ReadData;
                    uu.czsj = u.UpdateActionTime;
                }
                list.Add(uu);
            }
            table.rows = list.ToTable();

            if (item.whereList != null && item.whereList.Length > 0 && table.rows != null && table.rows.Rows.Count > 0)
            {
                string w = this.GetWhereSQL(item, this.model);
                w = w.Replace("SystemUser.", "");
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
            return table;
        }
    }
}
