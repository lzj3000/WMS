using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Business.Model;
using Acc.Contract;
using Acc.Contract.MVC;
using Acc.Contract.Data.ControllerData;
using Way.EAP.DataAccess.Entity;
using Way.EAP.DataAccess.Data;

namespace Acc.Business.Controllers
{
    public enum RunType
    {
        /// <summary>
        /// 定时(每天)
        /// </summary>
        day = 0,
        /// <summary>
        /// 循环(分钟)
        /// </summary>
        time = 1
    }
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class AutoTimeClass : Attribute
    {
        public AutoTimeClass()
        {
            ry = 0;
        }
        /// <summary>
        /// 运行方式
        /// </summary>
        public RunType ry { get; set; } 
    }
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class AutoTimeProperty : Attribute
    { }
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class AutoTimeMethod : Attribute
    { }
    /// <summary>
    /// 时间任务中心
    /// </summary>
    public class TimeTaskCenter
    {
        const int forTime = 60000;
        private TimeTaskCenter() { }
        private static TimeTaskCenter _center;
        private static readonly object _lock = new object();
        private readonly System.Timers.Timer time = new System.Timers.Timer(forTime);
        private readonly List<TimeTask> taskItems = new List<TimeTask>();
        private string tempid = int.MaxValue.ToString();
        public static TimeTaskCenter GetCenter
        {
            get
            {
                if (_center == null)
                {
                    lock (_lock)
                    {
                        if (_center == null)
                        {
                            _center = new TimeTaskCenter();
                            ControllerCenter.GetCenter.AsyncController += new ControllerCenter.AsyncControllerExc(GetCenter_AsyncController);
                            _center.time.Elapsed += new System.Timers.ElapsedEventHandler(_center.time_Elapsed);
                            _center.time.Start();
                        }
                    }
                }
                return _center;
            }
        }
        public static void ApplicationStart()
        {
            TimeTask task = new TimeTask();
            EntityList<TimeTask> list = new EntityList<TimeTask>(task.GetDataAction());
            list.GetData("IsSubmited=1");
            if (list.Count > 0)
            {
                foreach (TimeTask t in list)
                {
                    t.ParameterDetails.GetData();
                    TimeTaskCenter.GetCenter.taskItems.Add(t);
                }
            }
        }
        void time_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            DateTime dt = e.SignalTime;
            foreach (TimeTask t in taskItems)
            {
                if (t.RunState == 1) continue;
                if (t.RunType == 0)
                {
                    int h = dt.Hour;
                    int m = dt.Minute;
                    string[] st= t.Interval.Split(':');
                    if (st.Length == 2)
                    {
                        int sh = int.Parse(st[0]);
                        int sm = int.Parse(st[1]);
                        if (h == sh)
                        {
                            TimeSpan ts = TimeSpan.FromMinutes(m);
                            TimeSpan hs = TimeSpan.FromMinutes(sm);
                            if (ts == hs || hs.Add(TimeSpan.FromSeconds(forTime)) == ts)
                            {
                                RunTaskhandel dth = new RunTaskhandel(runTask);
                                dth.BeginInvoke(t, dt, null, null);
                            }
                        }
                    }
                }
                if (t.RunType == 1)
                {
                    TimeSpan ts = dt - t.Lastdate;
                    int m = ts.Minutes;
                    int sm = int.Parse(t.Interval);
                    if (m > sm)
                    {
                        RunTaskhandel dth = new RunTaskhandel(runTask);
                        dth.BeginInvoke(t, dt, null, null);
                    }
                }
            }
        }
        public void StartTask(TimeTask task)
        {
            if (!taskItems.Contains(task))
            {
                task.ParameterDetails.GetData();
                taskItems.Add(task);
                runTask(task,DateTime.Now);
            }
        }
        public void StopTask(TimeTask task)
        {
            taskItems.Remove(task);
        }
        private delegate void RunTaskhandel(TimeTask task, DateTime time);
        private void runTask(TimeTask task, DateTime time)
        {
            try
            {
                task.RunState = 1;
                AcctrueUser user = UserCenter.GetCenter.GetUser(tempid);
                if (user == null)
                {
                    user = new AcctrueUser();
                    user.IsAdministrator = true;
                    user.ID = tempid;
                    user.IsLogin = true;
                    user.name = "自动运行任务用户";
                    user.LoginTime = DateTime.Now;
                    UserCenter.GetCenter.AddUser(user);
                }
                ControllerBase cb = ControllerCenter.GetCenter.GetController(task.ControllerName, user.tempid);
                foreach (TimeTaskParameter tt in task.ParameterDetails)
                {
                    cb.setPropertyValue(tt.field, tt.parameterValue);
                }
                task.Lastdate = time;
                submitItem item = new submitItem();
                item.system = "";
                item.cname = task.ControllerName;
                item.cmd = task.ControllerCommand;
                item.controller = cb;
                item.IsAsy = true;
                item.Tag1 = task;
                item.user = user;
                ControllerCenter.GetCenter.submit(item, user.tempid);
            }
            catch (Exception e)
            { 
               
            }
        }

        static void GetCenter_AsyncController(submitItem item)
        {
            try
            {
                item.user.UpdateActionTime = DateTime.Now;
                //if (item.user.name == "自动运行任务用户")
                //    UserCenter.GetCenter.RemoveUser(item.user);
                TimeTask task = item.Tag1 as TimeTask;
                task.RunState = 0;
                if (task != null)
                {
                    IDataAction action = task.GetDataAction(); 
                    TimeTaskNotes ttn = new TimeTaskNotes();
                    bool run = false;
                    if (string.IsNullOrEmpty(item.error))
                        run = true;
                    string sql = "insert into " + ttn.ToString() + "(ParentID,StartTime,EndTime,Results,ErrorMsg,IsRun) values "
                                  + "(" + task.ID + ",'" + task.Lastdate + "','" + DateTime.Now + "','" + item.results + "','" + item.error + "','" + run + "')";
                    action.Execute(sql);
                    sql = "update " + task.ToString() + " set RunState=0,Lastdate='" + task.Lastdate + "' where id=" + task.ID;
                    action.Execute(sql);
                    //ttn.EndTime = DateTime.Now;
                    //ttn.StartTime = task.Lastdate;
                    //ttn.Results = item.results;
                    //ttn.ErrorMsg = item.error;
                    //ttn.IsRun = true;
                    //task.NotesDetails.Add(ttn);
                    //EntityList<TimeTask> list = new EntityList<TimeTask>(task.GetDataAction());
                    //list.Add(task);
                    //list.Save();
                }
            }
            catch (Exception e)
            { 
               
            }
        }
    }
}
