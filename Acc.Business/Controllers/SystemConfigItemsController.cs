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

namespace Acc.Business.Controllers
{
    public class SystemConfigItemsController : BusinessController
    {
        public SystemConfigItemsController() : base(new SystemConfigItems()) { }

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
                case "isdisable":
                    item.disabled = true;
                    break;
            }
        }

        [ActionCommand(name = "设置默认", title = "保存系统配置项信息", index = 5, icon = "icon-save",isalert=true)]
        public void test()
        {
            IDataAction action = this.model.GetDataAction();
            SystemConfigItems si = this.ActionItem as SystemConfigItems;
            string sql = string.Format("update Acc_Bus_SystemConfigItems set ISDEFAULT =1 where id='{0}'", si.ID);
            sql += string.Format("update Acc_Bus_SystemConfigItems set ISDEFAULT =0 where id!='{0}'", si.ID);
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
            return "Views/manager/systemeconfigitems.htm"; 
        }
    }
}
