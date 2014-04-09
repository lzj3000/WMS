using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Business.Model;
using Acc.Contract.MVC;
using Way.EAP.DataAccess.Entity;
using Acc.Contract.Data;
using Acc.Contract.Data.ControllerData;
using Way.EAP.DataAccess.Data;
using Acc.Contract;
using System.Web;
using System.IO;
using System.Data;

namespace Acc.Business.Controllers
{
    
    public class SystemModelController:BusinessController
    {
        public SystemModelController() : base(new SystemModel()) { }


        protected override void OnLoaded(loadItem item, ReadTable table)
        {
            if (table.rows.Columns.Contains("FilePath"))
            {
                foreach (DataRow row in table.rows.Rows)
                {
                    row["FilePath"] = "";
                }
            }
        }
        protected override ActionCommand[] OnInitCommand(ActionCommand[] commands)
        {
            return base.OnInitCommand(commands);
        }
        protected override void Remove()
        {
            if (this.ActionItem != null)
            {
                ((IEntityBase)this.ActionItem).StateBase = EntityState.Select;
                this.modelList.Add(this.ActionItem);
                this.modelList.Remove(this.ActionItem);
                SaveEvent eve = new SaveEvent();
                eve.Item = this.ActionItem;
                OnRemoveing(eve);
                if (!eve.Breakoff)
                {
                    try
                    {
                        this.modelList.Save();
                    }
                    catch (Exception e)
                    {
                        this.modelList.Clear();
                        throw e;
                    }
                    this.OnRemoveed(this.OldItem);
                }
            }
        }
        public override bool IsClearAway
        {
            get
            {
                return false;
            }
        }
        public override bool IsSubmit
        {
            get
            {
                return false;
            }
        }
        protected override void OnForeignLoading(IModel model, loadItem item)
        {
            if (model is SystemModel && this.fdata.filedname == ("SystemDirectoryID").ToUpper())
            {
                SystemDirectory sd = new SystemDirectory();
                item.rowsql = "select * from " + sd.ToString();
            }
            else
                base.OnForeignLoading(model, item);
        }
        [WhereParameter]
        public string file { get; set; }

        [ActionCommand(name = "上传控制器", title = "上传模块控制器DLL", index = 5, icon = "icon-add", onclick = "uploadDll", isselectrow = false)]
        public void modelUpload()
        {
            try
            {
                getController(this.file);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        [ActionCommand(name = "上传页文件", title = "上传模块控制器所包含页面文件或更新页面文件", index = 6, icon = "icon-add", onclick = "uploadDll", isselectrow = false)]
        public void modelUpload1()
        {
            setHtml(this.file);
        }
        private void setHtml(string filename)
        {
            IController[] cs = ControllerCenter.GetCenter.getAllController(filename, typeof(ControllerBase));
            Type type = cs[0].GetType();
            setHtml(type);
        }
        private void setHtml(Type type)
        {
            string[] rn = type.Assembly.GetManifestResourceNames();
            foreach (string s in rn)
            {
                string f = s.Substring(s.IndexOf(".Views.") + 7);
                string[] fs = f.Split('.');
                string fn = "";
                if (fs.Length > 2)
                {
                    for (int i = 0; i < fs.Length - 1; i++)
                    {
                        fn += "\\" + fs[i];
                    }
                    fn += "." + fs[fs.Length - 1];
                }
                else
                {
                    if (fs.Length == 2)
                        fn = "\\" + fs[0] + "." + fs[1];
                }
                fn = ControllerCenter.GetCenter.viewPath + fn;
                if (File.Exists(fn))
                {
                    File.Delete(fn);
                }
                Stream stream = type.Assembly.GetManifestResourceStream(s);
                using (FileStream fss = new FileStream(fn, FileMode.Create, FileAccess.Write))
                {
                    StreamWriter sw = new StreamWriter(fss);
                    StreamReader rad = new StreamReader(stream);
                    sw.Write(rad.ReadToEnd());
                    rad.Close();
                    sw.Flush();
                    sw.Close();
                }
            }
        }
        private void getController(string filename)
        {
            EntityList<SystemModel> list = new EntityList<SystemModel>(this.model.GetDataAction());
            list.GetData();
            IController[] cs = ControllerCenter.GetCenter.getAllController(filename, typeof(ControllerBase));
            SystemModel sm;
            InitData data;
            ModelCommand com;
            string fullname = "";
            Type type = null;
            foreach (IController c in cs)
            {
                ((ControllerBase)c).user = this.user;
                if (!c.IsSystemController) continue;
                type = c.GetType();
                fullname = type.FullName;
                sm = list.Find(delegate(SystemModel m) { return m.ControllerName.Equals(fullname); });
                if (sm == null)
                {
                    sm = new SystemModel();
                    sm.ControllerName = fullname;
                    sm.Url = c.ControllerPath();
                    sm.Remark = c.ControllerDescription();
                    sm.ModelName = c.ControllerName();
                    list.Add(sm);
                   // sm.FilePath = filename;
                }
                else
                {
                    sm.ControllerName = fullname;
                    sm.Url = c.ControllerPath();
                    sm.Remark = c.ControllerDescription();
                    sm.ModelName = c.ControllerName();
                   // sm.FilePath = filename;
                    sm.CommandItems.GetData();
                    sm.CommandItems.RemoveAll(delegate(ModelCommand mc) { return true; });
                    sm.DataItems.GetData();
                    sm.DataItems.RemoveAll(delegate(SystemModelData md) { return true; });
                }
                data = c.SystemInit();
                if (data != null)
                {
                    foreach (ActionCommand ac in data.commands)
                    {
                        com = new ModelCommand();
                        com.command = ac.command;
                        com.icon = ac.icon;
                        com.index = ac.index;
                        com.name = ac.name;
                        com.onclick = ac.onclick;
                        com.Disabled = ac.disabled;
                        com.title = ac.title;
                        com.url = ac.url;
                        com.visible = ac.visible;
                        sm.CommandItems.Add(com);
                    }
                    setModelData(data.modeldata, sm);
                }
            }
            list.Save();
            if (type != null)
            {
                setHtml(type);
            }
        }
        private void setModelData(ModelData m,SystemModel sm)
        {
            SystemModelData smd;
            foreach (ItemData id in m.childitem)
            {
                smd = new SystemModelData();
                smd.field = id.field;
                smd.index = id.index;
                smd.isedit = id.isedit;
                smd.iskey = id.iskey;
                smd.issearch = id.issearch;
                smd.length = id.length;
                smd.ModelName = m.name;
                smd.ModelTableName = m.tablename;
                smd.required = id.required;
                smd.title = id.title;
                smd.TitleName = m.title;
                smd.type = id.type;
                smd.visible = id.visible;
                sm.DataItems.Add(smd);
            }
            foreach (ModelData d in m.childmodel)
            {
                setModelData(d, sm);
            }
        }
        protected override string OnGetPath()
        {
            return "Views/manager/systemmodel.htm";
        }
    }
}
