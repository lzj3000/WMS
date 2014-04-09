using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Business.Controllers;
using Way.EAP.DataAccess.Entity;
using Acc.Contract.Data.ControllerData;
using Way.EAP.DataAccess.Data;
using Acc.Contract.Data;
using Acc.Contract;
using Way.EAP.DataAccess.Regulation;
using Acc.Business.WMS.Model;

namespace Acc.Business.WMS.XHY.Controllers
{
    public class CustomersController : BusinessController
    {
        #region 继承方法
        public CustomersController() : base(new XHY_Customer()) { }
        
        //是否启用提交
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
        //是否启用回收站
        public override bool IsClearAway
        {
            get
            {
                return false;
            }
        }
        protected override void OnInitViewChildItem(ModelData data, ItemData item)
        {
            base.OnInitViewChildItem(data, item);

            if (data.name.EndsWith("XHY_Customer"))
            {
                switch (item.field.ToLower())
                {
                    case "iscustomer":
                    case "issuppliers":
                    case "bark":
                    case "barkcode":
                    case "faddress":
                    case "fcustomerid":
                    case "ffax":
                    case "fhomepage":
                    case "fphone":
                    case "fregionid":
                    case "ftrade":
                    case "registrationid":
                    case "taxpayeridentificationnumber":
                        item.visible = false;
                        break;
                }

            }

        }
        //显示在菜单
        protected override string OnControllerName()
        {
            return "往来单位管理"; 
        }
        //菜单中url路径
        protected override string OnGetPath()
        {
            return "Views/CRM/customers.htm";
        }
        //开发人
        protected override string OnGetAuthor()
        {
            return "柳强";
        }
        //说明
        protected override string OnGetControllerDescription()
        {
            return "往来单位管理";
        }
        #endregion

        #region 重写方法
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
                if (ac.command == "add")
                {
                    ac.visible = false;
                }
                if (ac.command == "remove")
                {
                    ac.visible = false;
                }
                if (ac.command == "edit")
                {
                    ac.visible = false;
                }
            }
            return coms;
        }
        #endregion

        #region 添加按钮
        [ActionCommand(name = "禁用", title = "禁用（在系统中不能使用）", index = 8, icon = "icon-search", isalert = true, isselectrow = true, issplit = true, splitname = "isdistrue")]
        public void isdisfalse()
        {

            string tn = this.ActionItem.ToString();
            ///以下方法可变成多条启用
            IDataAction da = this.model.GetDataAction();
            if (this.ActionItem["ID"] != null)
            {
                string sql = "update " + tn + " set IsDisable = 1 where id = '" + this.ActionItem["ID"] + "'";
                da.Execute(sql);
            }
        }


        [ActionCommand(name = "启用", title = "启用（在系统中可以使用）", index = 9, icon = "icon-search", isalert = true, isselectrow = true)]
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

        [ActionCommand(name = "添加到物流公司", title = "将此客户添加到物流公司信息中", index =10, icon = "icon-ok", isalert = true, isselectrow = true)]
        public void isaddtolog()
        {
            string tn = this.ActionItem.ToString();
            ///以下方法可变成多条启用
            IDataAction da = this.model.GetDataAction();
            XHY_Customer c = base.getinfo(this.ActionItem["ID"].ToString()) as XHY_Customer;
            EntityList<LogisticsInfo> linfo = new EntityList<LogisticsInfo>(da);
            linfo.GetData("code='" + c.Code+ "'");
            if (linfo.Count > 0)
            {
                throw new Exception("异常：已添加到物流公司信息中，不能重复添加！");
            }
            if (this.ActionItem["ID"] != null)
            {
                string sql = string.Format("insert into " + new LogisticsInfo().ToString() + "(code,LOGISTICSNAME,Createdby,Creationdate) values('{0}','{1}','{2}','{3}')", c.Code, c.CUSTOMERNAME, this.user.ID,DateTime.Now);
                
                da.Execute(sql);
            }
        }
        #endregion
    }
}

