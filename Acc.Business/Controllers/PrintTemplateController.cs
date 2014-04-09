using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Business.Controllers;
using Acc.Contract.Data;
using Acc.Contract.Data.ControllerData;
using Acc.Business.Model;
using System.Configuration;
using Way.EAP.DataAccess.Data;

namespace Acc.Business.Controllers
{
    public class PrintTemplateController : BusinessController
    {
        /// <summary>
        /// 描述：打印模板主表
        /// 作者：路聪
        /// 创建日期:2013-07-19
        /// </summary>
        public PrintTemplateController() : base(new PrintTemplate()) { }

        //是否启用审核
        public override bool IsReviewedState
        {
            get
            {
                return false;
            }
        }
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

        //显示在菜单
        protected override string OnControllerName()
        {
            return "打印模板管理";
        }
        //菜单中url路径
        protected override string OnGetPath()
        {
            return "Views/manager/PrintTemplate.htm";
        }
        //开发人
        protected override string OnGetAuthor()
        {
            return "路聪07-19";
        }
        //说明
        protected override string OnGetControllerDescription()
        {
            return "打印模板管理";
        }

        #region 初始化数据方法

        #endregion

        protected override ActionCommand[] OnInitCommand(ActionCommand[] commands)
        {
            //获取所有按钮集合
            foreach (ActionCommand ac in commands)
            {
                if (ac.command == "add")
                {
                    ac.visible = false;
                }
                if (ac.command == "edit")
                {
                    ac.visible = false;
                }
                if (ac.command == "UnSubmitData")
                {
                    ac.visible = false;
                }
                if (ac.command == "ReviewedData")
                {
                    ac.visible = false;
                }
            }
            return commands;
        }

        [ActionCommand(name = "添加模板", title = "添加打印模板", index = 6, icon = "icon-ok", onclick = "addMb", isselectrow = false)]
        public void addMb()
        {
            //生成界面方法按钮用于权限控制，本方法无代码
        }

        //[ActionCommand(name = "参数维护", title = "参数维护", index = 6, icon = "icon-ok", onclick = "whdata", isselectrow = false)]
        //public void whdata()
        //{
        //    //生成界面方法按钮用于权限控制，本方法无代码
        //}

        [ActionCommand(name = "编辑", title = "编辑模板", index = 6, icon = "icon-ok", onclick = "xiugaidata", isselectrow = true)]
        public void xiugaidata()
        {
            //生成界面方法按钮用于权限控制，本方法无代码
        }

    }
}
