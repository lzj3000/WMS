using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Business.Controllers;
using Acc.Business.WMS.Model;
using Acc.Contract.Data;
using Acc.Contract.Data.ControllerData;
using Way.EAP.DataAccess.Entity;
using Way.EAP.DataAccess.Data;
using System.Data;

namespace Acc.Business.WMS.Controllers
{
    public class AutoNextVal
    {
        /// <summary>
        /// 前缀
        /// </summary>
        public string qz { get; set; }
        /// <summary>
        /// 流水位数
        /// </summary>
        public int ws { get; set; }
        /// <summary>
        /// 生成数量
        /// </summary>
        public int sc { get; set; }
        /// <summary>
        /// 开始数
        /// </summary>
        public int ks { get; set; }
    }
    public class PortsController : BusinessController
    {
        /// <summary>
        /// 描述：托盘管理控制器
        /// 作者：路聪
        /// 创建日期:2013-01-10
        /// </summary>
        public PortsController() : base(new Ports()) { }

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
            return "托盘管理";
        }
        //菜单中url路径
        protected override string OnGetPath()
        {
            return "Views/WMS/Ports/Ports.htm";
        }
        //开发人
        protected override string OnGetAuthor()
        {
            return "路聪";
        }
        //说明
        protected override string OnGetControllerDescription()
        {
           
            return "托盘管理";
        }

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
                if (ac.command == "edit")
                {
                    ac.visible = false;
                }
            }
            return coms;
        }

        #region 初始化数据方法
        protected override void OnInitViewChildItem(Contract.Data.ModelData data, Contract.Data.ItemData item)
        {
            if (data.name.EndsWith("Ports"))
            {
                switch (item.field.ToLower())
                {
                    case "submiteddate":
                    case "submitedby":
                    case "modifiedby":
                    case "modifieddate":
                    case "reviewedby":
                    case "revieweddate":
                    case "issubmited":
                    case "isreviewed":
                    case "isdelete":
                    case "id":
                    case "code":
                    case "stay1":
                    case "stay2":
                    case "stay3":
                    case "stay4":
                    case "stay5":
                    case "portname":
                        item.visible = false;
                        item.isedit = false;
                        item.disabled = true;
                        break;
                    case "createdby":
                    case "creationdate":
                        item.visible = false;
                        item.disabled = true;
                        break;
                    default:
                        item.visible = true;
                        break;
                }
            }
        }

        #endregion

        //[ActionCommand(name = "批量打印条码", title = "批量选择待打印的条码", index = 6, icon = "icon-ok", onclick = "selectRequisition", isselectrow = true)]
        //public void selectPorts()
        //{
        //    //生成界面方法按钮用于权限控制，本方法无代码
        //}

        //[ActionCommand(name = "设置打印模板", title = "设置打印模板", index = 7, icon = "icon-ok", onclick = "SetPrintModel", isselectrow = false)]
        //public void SetPrintModel()
        //{
        //    //生成界面方法按钮用于权限控制，本方法无代码
        //}

        [ActionCommand(name = "生成托盘", title = "生成托盘", index = 8, icon = "icon-search", isselectrow = false, isalert = false, onclick = "openAutoNextPort")]
        public void test1()
        {
            Console.WriteLine("aaaa");
        }

        /// <summary>
        /// 接收参数
        /// </summary>
        [WhereParameter]
        public AutoNextVal anv { get; set; }
        public string AutoNext()
        {
            try
            {
                if (anv != null)
                {
                    string createdby = this.user.ID;
                    DateTime creationdate = DateTime.Now;
                    Ports ps = new Ports();
                    IDataAction action = this.model.GetDataAction();
                    string sql = "insert into " + ps.ToString() + "(PORTNO,PORTNAME,Code,STATUS,IsDisable,Createdby,Creationdate) values('{0}','{1}','{2}','0',0," + this.user.ID + ",'" + DateTime.Now.ToString() + "')";
                    string s1 = "";
                    string s2 = "";
                    string fm = "";
                    string yxsql = "select code from " + ps.ToString() + " where code='{0}'";
                    for (int i = 0; i < anv.ws; i++)
                    {
                        fm += "0";
                    }
                    action.StartTransation();
                    for (int i = anv.ks; i <= anv.ks + anv.sc; i++)
                    {
                        s1 = anv.qz + i.ToString(fm);
                        object obj = action.GetValue(string.Format(yxsql, s1));
                        if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
                            throw new Exception("异常：" + s1 + "编码已生成，不能重复生成！");
                        action.Execute(string.Format(sql, s1, s2, s1));
                    }
                    action.Commit();
                    action.EndTransation();
                }
                #region 不要
                //String[] str = id.Split(',');
                //try
                //{
                //    int endnum = int.Parse(str[0]);
                //    string sql = string.Empty;
                //    if (endnum < 10000)
                //    {

                //        BillNumberController bc = null;
                //        string portno = string.Empty;
                //        string portname = string.Empty;
                //        string status = "0";
                //        string createdby = this.user.ID;
                //        DateTime creationdate = DateTime.Now;
                //        Ports ps = new Ports();
                //        DataTable dt = this.model.GetDataAction().GetDataTable("select count(*) as cnum from " + ps.ToString() + "");
                //        int startnum = 1;
                //        if (Convert.ToInt32(dt.Rows[0]["cnum"]) > 0)
                //        {
                //            startnum = Convert.ToInt32(dt.Rows[0]["cnum"]) + 1;
                //            endnum += startnum;
                //        }
                //        for (int i = startnum; i <= endnum; i++)
                //        {
                //            bc = new BillNumberController();
                //            //循环开始数量到结束数量
                //            portno = "9" + i.ToString().PadLeft(5, '0');
                //            portname = portno + "托盘";
                //            sql = string.Format("insert into " + ps.ToString() + "(PORTNO,PORTNAME,STATUS,IsDisable,Code,Createdby,Creationdate) values('{0}','{1}',{2},{3},'{4}',{5},'{6}')", portno, portname, status, 0, portno, createdby, creationdate);
                //            this.model.GetDataAction().Execute(sql);
                //        }

                //    }
                //    else
                //    {
                //        throw new Exception("数量输入错误，请核对后录入");
                //    }
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return "";
        }
        protected override void OnRemoveing(SaveEvent item)
        {
            Ports ord = item.Item as Ports;
            if (ord.STATUS == "1")
            {
                throw new Exception("当前托盘占用，不可以删除");
            }
            else
            {
                base.OnRemoveing(item);
            }
        }
        //[ActionCommand(name = "禁用此项", title = "禁用此项（在系统中不能使用）", index = 8, icon = "icon-remove", isalert = true, isselectrow = true)]
        //public void isdisfalse()
        //{

        //    string tn = this.ActionItem.ToString();
        //    ///以下方法可变成多条启用
        //    IDataAction da = this.model.GetDataAction();
        //    if (this.ActionItem["ID"] != null)
        //    {
        //        string sql = "update " + tn + " set IsDisable = 1 where id = '" + this.ActionItem["ID"] + "'";
        //        da.Execute(sql);
        //    }
        //}


        [ActionCommand(name = "解除占用", title = "启用此项（在系统中可以使用）", index = 9, icon = "icon-ok", isalert = true, isselectrow = true)]
        public void isdistrue()
        {

            ///获取当前表名称
            string tn = this.ActionItem.ToString();
            ///以下方法可变成多条启用
            IDataAction da = this.model.GetDataAction();
            if (this.ActionItem["ID"] != null)
            {
                string sql = "update " + tn + " set status = 0 where id = '" + this.ActionItem["ID"] + "'";
                da.Execute(sql);
            }
        }
    }
}
