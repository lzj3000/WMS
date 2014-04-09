using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Business.Model;
using Acc.Contract.Data;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model.Purview;
using Acc.Contract;
using System.Net.Mail;
using Way.EAP.DataAccess.Data;
using System.Data;
using Acc.Contract.Data.ControllerData;

namespace Acc.Business.Controllers
{
    public class SystemInfoController : BusinessController
    {
        public SystemInfoController() : base(new SystemInfo()) { }

        protected override void OnInitViewChildItem(Acc.Contract.Data.ModelData data, Acc.Contract.Data.ItemData item)
        {
            switch (item.field.ToLower())
            {
                case "code":
                case "rowindex":
                case "issubmited":
                case "submiteddate":
                case "submitedby":
                case "modifiedby":
                case "modifieddate":
                case "reviewedby":
                case "revieweddate":
                case "isreviewed":
                    item.visible = false;
                    break;
                case "isselected":
                    item.disabled = true;
                    break;
            }
        }
        //public string GetSystemInfo()
        //{
        //    EntityList<SystemInfo> list = new EntityList<SystemInfo>(this.model.GetDataAction());
        //    list.GetData();
        //    return EntityHelper.ToJSON(list[0]);
        //}
        [ActionCommand(name = "保存设置", title = "设置为默认样式", index = 5, icon = "icon-save", isalert = true)]
        public void test()
        {
            IDataAction action = this.model.GetDataAction();
            SystemInfo si = this.ActionItem as SystemInfo;
            string sql = string.Format("update acc_bus_systeminfo set IsSelected =1 where id='{0}'", si.ID);
            sql += string.Format("update acc_bus_systeminfo set IsSelected =0 where id!='{0}'", si.ID);
            action.Execute(sql);

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
        protected override string OnGetPath()
        {
            return "Views/manager/systeminfo.htm";
        }

         //<summary>
         //配置页面信息
         //</summary>
         //<returns></returns>
        public string GetSystemInfo()
        {
            IDataAction action = this.model.GetDataAction();
            string sql = string.Format("select * from acc_bus_systeminfo where isselected=1");
            DataTable dt = action.GetDataTable(sql);
            return Acc.Contract.JSON.Serializer(dt);
        }

        /// <summary>
        /// 配置页面信息
        /// </summary>
        /// <returns></returns>
        public string GetSystemInfoDetails()
        {
            IDataAction action = this.model.GetDataAction();
            string sql = string.Format("select * from acc_bus_systeminfodetials where parentid in(select id from acc_bus_systeminfo where isselected=1)");
            DataTable dt = action.GetDataTable(sql);
            foreach (DataRow item in dt.Rows)
            {
                DataTable d = action.GetDataTable(item["SqlStr"].ToString());
                foreach (DataRow i in d.Rows)
                {
                    action.Execute("update acc_bus_systeminfodetials set num = " + Convert.ToInt32(i["num"]) + " where sqlstr='" + item["SqlStr"] + "' ");
                }

            }

            return Acc.Contract.JSON.Serializer(dt);
        }
    }
}
