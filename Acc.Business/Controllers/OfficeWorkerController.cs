using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Contract.MVC;
using Acc.Business.Model;
using Acc.Contract.Data;
using Way.EAP.DataAccess.Entity;
using Acc.Contract.Data.ControllerData;
using Way.EAP.DataAccess.Regulation;

namespace Acc.Business.Controllers
{
    public class OfficeWorkerController : BusinessController
    {
        public OfficeWorkerController() : base(new OfficeWorker()) { }
        public OfficeWorkerController(IModel model) : base(model) { }
        //是否启用审核
        //public override bool IsReviewedState
        //{
        //    get
        //    {
        //        return true;
        //    }
        //}
        //是否启用提交
        public override bool IsSubmit
        {
            get
            {
                return base.IsSubmit;
            }
        }
        //是否启用回收站
        public override bool IsClearAway
        {
            get
            {
                return base.IsClearAway;
            }
        }
        //显示在菜单
        protected override string OnControllerName()
        {
            return "职员管理";
        }
        //菜单中url路径
        protected override string OnGetPath()
        {
            return "Views/manager/officeworker.htm";
        }
        //开发人
        protected override string OnGetAuthor()
        {
            return "李晓超";

        }
        //说明
        protected override string OnGetControllerDescription()
        {
            return "公司职员管理";
        }
       
       
        protected override void OnAdding(ControllerBase.SaveEvent item)
        {
            EntityList<OfficeWorker> list = new EntityList<OfficeWorker>(this.model.GetDataAction());
            list.GetData("LoginName='" + item.Item["LoginName"] + "'");
            if (list.Count > 0)
            {
                item.Breakoff = true;
                throw new Exception("登陆名已存在，不能重复，请更改后重试！");
            }
            ((OfficeWorker)item.Item).WorkPassWord = "acctrue";
            base.OnAdding(item);
        }
        //[ActionCommand(name = "批量打印员工条码", title = "批量选择待打印的员工编码", index = 6, icon = "icon-ok", onclick = "selectRequisition", isselectrow = true)]
        //public void selectPorts()
        //{
        //    //生成界面方法按钮用于权限控制，本方法无代码
        //}
        protected override void OnGetWhereing(IModel m, List<Way.EAP.DataAccess.Regulation.SQLWhere> where)
        {
            base.OnGetWhereing(m, where);
            if (m is OfficeWorker)
            {
                SQLWhere w = new SQLWhere();
                w.ColumnName = "LoginName";
                w.Symbol = "<>";
                w.Value = "admin";
                where.Add(w);
            }
        }
    }
}
