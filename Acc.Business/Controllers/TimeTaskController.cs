using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Business.Model;
using Acc.Contract.Data;
using Acc.Contract.Data.ControllerData;
using System.Data;
using System.Reflection;
using Acc.Contract;
using Way.EAP.DataAccess.Entity;
using Acc.Contract.MVC;
using Way.EAP.DataAccess.Regulation;

namespace Acc.Business.Controllers
{
    /// <summary>
    /// 时间任务控制器
    /// </summary>
    public class TimeTaskController : BusinessController
    {
        public TimeTaskController() : base(new TimeTask()) { }
        //是否启用提交
        public override bool IsSubmit
        {
            get
            {
                return false;
            }
        }
        //是否启用回收站
        public override bool IsClearAway
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
        //显示在菜单
        protected override string OnControllerName()
        {
            return "自动任务管理";
        }
        //菜单中url路径
        protected override string OnGetPath()
        {
            return "Views/manager/timetask.htm";
        }
        //开发人
        protected override string OnGetAuthor()
        {
            return "李晓超";

        }
        //说明
        protected override string OnGetControllerDescription()
        {
            return "自动任务管理";
        }

        #region 界面显示相关操作
        protected override void OnInitViewChildItem(ModelData data, ItemData item)
        {
            if (item.IsField("ControllerName"))
            {
                item.foreign.isfkey = true;
                item.foreign.filedname = item.field;
                item.foreign.objectname = data.name;
                item.foreign.displayfield = ("ModelName").ToUpper();
                item.foreign.foreignobject = typeof(SystemModel).FullName;
                item.foreign.foreignfiled = ("ControllerName").ToUpper();
            }
        }
      
        protected override void OnForeignIniting(Contract.MVC.IModel model, Contract.Data.InitData data)
        {
            base.OnForeignIniting(model, data);
            if (fdata.foreignobject.EndsWith("SystemModel"))
            {
                foreach (ItemData item in data.modeldata.childitem)
                {
                    item.visible = false;
                    if (item.IsField("ModelName"))
                    {
                        item.visible = true;
                        item.title = "类名";
                    }
                    if (item.IsField("ControllerName"))
                    {
                        item.visible = true;
                        item.title = "全名"; 
                    }
                    if (item.IsField("Url"))
                    {
                        item.visible = true;
                        item.title = "类型";
                        item.index = 1;
                       // item.visible = false;
                        item.issearch = true;
                        item.comvtp.isvtp = true;
                        List<PropertyValueType> list=new List<PropertyValueType>();
                        PropertyValueType v1 = new PropertyValueType();
                        v1.Value = "0";
                        v1.Text="系统";
                        PropertyValueType v2 = new PropertyValueType();
                        v2.Value = "1";
                        v2.Text = "控制器";
                        PropertyValueType v3 = new PropertyValueType();
                        v3.Value = "2";
                        v3.Text = "接口";
                        list.Add(v1);
                        list.Add(v2);
                        list.Add(v3);
                        item.comvtp.items = list.ToArray();
                    }
                }
            }
        }
        protected override Contract.Data.ControllerData.ReadTable OnForeignLoad(Contract.MVC.IModel model, Contract.Data.ControllerData.loadItem item)
        {
            if (model is SystemModel)
            {
                int type = 0;
                foreach (SQLWhere sw in item.whereList)
                {
                    if (sw.ColumnName.EndsWith("url", StringComparison.CurrentCultureIgnoreCase))
                    {
                        type = int.Parse(sw.Value);
                        break;
                    }
                }
                ReadTable rt = new ReadTable();
                DataTable table = getForeignData(type);
                rt.rows = table;
                rt.total = table.Rows.Count;
                return rt;
            }
            else
                return base.OnForeignLoad(model, item);
        }
        private DataTable getForeignData(int type)
        {
            DataTable table = new DataTable();
            table.Columns.Add(("Url").ToUpper());
            table.Columns.Add(("ModelName").ToUpper());
            table.Columns.Add(("ControllerName").ToUpper());
            table.Columns.Add("M");
            table.Columns.Add("P");
            Type[] items = getAutoTaskClass(type);
            foreach (Type t in items)
            {
                MethodInfo[] ms = getAutoTaskMethod(t);
                if (ms.Length > 0)
                {
                    DataRow row = table.NewRow();
                    row[("Url").ToUpper()] = type;
                    row[("ModelName").ToUpper()] = t.Name;
                    row[("ControllerName").ToUpper()] = t.FullName;
                    List<PropertyValueType> ml = new List<PropertyValueType>();
                    foreach (MethodInfo i in ms)
                    {
                        PropertyValueType p1 = new PropertyValueType();
                        p1.Text = i.Name;
                        p1.Value = i.Name;
                        ml.Add(p1);
                    }
                    row["M"] = JSON.Serializer(ml.ToArray());
                    List<string> pl = new List<string>();
                    foreach (PropertyInfo p in getAutoTaskProperty(t))
                    {
                        pl.Add(p.Name);
                    }
                    row["P"] = JSON.Serializer(pl.ToArray());
                    table.Rows.Add(row);
                }
            }
            return table;
        }
        private Type[] getAutoTaskClass(int type)
        {
            List<Type> list = new List<Type>();
            if (type == 0)
            {
                Assembly[] apps = AppDomain.CurrentDomain.GetAssemblies();
                list.AddRange(autoType(apps));
            }
            if (type == 1)
            {
                string[] sfiles = System.IO.Directory.GetFiles(ControllerCenter.GetCenter.controllerPath, "*.dll", System.IO.SearchOption.TopDirectoryOnly);
                List<Assembly> list1 = new List<Assembly>();
                foreach (string file in sfiles)
                {
                    Assembly ass = Assembly.LoadFrom(file);
                    list1.Add(ass);
                }
                list.AddRange(autoType(list1.ToArray()));
            }
            if (type == 2)
            {
                string[] sfiles = System.IO.Directory.GetFiles(ControllerCenter.GetCenter.interfacePath, "*.dll", System.IO.SearchOption.TopDirectoryOnly);
                List<Assembly> list1 = new List<Assembly>();
                foreach (string file in sfiles)
                {
                    Assembly ass = Assembly.LoadFrom(file);
                    list1.Add(ass);
                }
                list.AddRange(autoType(list1.ToArray()));
            }
            return list.ToArray();
        }
        private Type[] autoType(Assembly[] apps)
        {
            List<Type> list = new List<Type>();
            foreach (Assembly ass in apps)
            {
                if (ass.FullName.StartsWith("Acc"))
                {
                    if (ass.FullName.StartsWith("AcctrueWMS.Bll")) continue;
                    Type[] ts = ass.GetExportedTypes();
                    foreach (Type t in ts)
                    {
                        object[] items = t.GetCustomAttributes(typeof(AutoTimeClass), false);
                        if (items.Length > 0)
                            list.Add(t);
                    }
                }
            }
            return list.ToArray();
        }
        private PropertyInfo[] getAutoTaskProperty(Type autoClass)
        {
            PropertyInfo[] items= autoClass.GetProperties();
            List<PropertyInfo> list = new List<PropertyInfo>();
            foreach (PropertyInfo info in items)
            {
                object[] os = info.GetCustomAttributes(typeof(AutoTimeProperty), false);
                if (os.Length > 0)
                    list.Add(info);
            }
            return list.ToArray();
        }
        private MethodInfo[] getAutoTaskMethod(Type autoClass)
        {
           MethodInfo[] items= autoClass.GetMethods();
           List<MethodInfo> list = new List<MethodInfo>();
           foreach (MethodInfo info in items)
           {
               object[] os = info.GetCustomAttributes(typeof(AutoTimeMethod), false);
               if (os.Length > 0)
                   list.Add(info);
           }
           return list.ToArray();
        }
        #endregion

        protected override void OnAdding(Contract.MVC.ControllerBase.SaveEvent item)
        {
            TimeTask task = item.Item as TimeTask;
            if (task != null)
            {
                EntityList<TimeTask> list = new EntityList<TimeTask>(this.model.GetDataAction());
                list.GetData("ControllerName='" + task.ControllerName + "' and ControllerCommand='" + task.ControllerCommand + "'");
                if (list.Count > 0)
                {
                    throw new Exception(task.ControllerName + "类的" + task.ControllerCommand + "方法，已创建自动运行任务，不能重复创建！");
                }
                task.Createdby = this.user.ID;
                task.Creationdate = DateTime.Now;
            }
        }
        protected override void OnRemoveing(Contract.MVC.ControllerBase.SaveEvent item)
        {
            TimeTask task = item.Item as TimeTask;
            if (task != null)
            {
                if (task.IsSubmited)
                    throw new Exception("错误：任务已启动不能删除！");
            }
        }
        [ActionCommand(name = "启动", title = "启动自动任务执行", index = 4, icon = "icon-ok", isalert = true, editshow = true)]
        public void StartTask()
        {
            if (((IEntityBase)this.ActionItem).StateBase != EntityState.Select)
                throw new Exception("错误：请先保存任务后再点启动！");
            this.modelList.GetData("ID=" + this.ActionItem["ID"]);
            if (this.modelList.Count > 0)
            {
                TimeTask task = this.modelList[0] as TimeTask;
                if (!task.IsSubmited)
                {
                    try
                    {
                        task.IsSubmited = true;
                        this.modelList.Save();
                        TimeTaskCenter.GetCenter.StartTask(task);
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                    finally
                    {
                        this.modelList.Clear();
                    }
                }
                else
                {
                    throw new Exception("错误：" + task.TaskName + "任务已启动不能再次启动！");
                }
            }
        }
        [ActionCommand(name = "停止", title = "停止自动任务执行", index = 5, icon = "icon-no", isalert = true, editshow = true)]
        public void StopTask()
        {
            if (((IEntityBase)this.ActionItem).StateBase != EntityState.Select)
                throw new Exception("错误：请先保存任务后再点停止！");
            this.modelList.GetData("ID="+this.ActionItem["ID"]);
            if (this.modelList.Count > 0)
            {

                TimeTask task = this.modelList[0] as TimeTask;
                if (task.IsSubmited)
                {
                    try
                    {
                        TimeTaskCenter.GetCenter.StopTask(task);
                        task.IsSubmited = false;
                        this.modelList.Save();
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                    finally
                    {
                        this.modelList.Clear();
                    }
                }
                else
                {
                    throw new Exception("错误：" + task.TaskName + "任务已停止不能再次停止！");
                }
            }
        }

    }
}
