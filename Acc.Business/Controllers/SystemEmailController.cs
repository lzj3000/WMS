using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Business.Model;
using Acc.Contract.Data;
using Way.EAP.DataAccess.Data;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model.Purview;
using Acc.Contract;
using System.Net.Mail;

namespace Acc.Business.Controllers
{
    public class SystemEmailController : BusinessController
    {
        public SystemEmailController() : base(new SystemEmail()) { }

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
            }
        }

        [ActionCommand(name = "设置为默认", title = "保存系统Email信息相关使用设置", index = 5, icon = "icon-save",isalert=true)]
        public void test()
        {
            IDataAction action = this.model.GetDataAction();
            SystemEmail si = this.ActionItem as SystemEmail;
            string sql = string.Format("update Acc_Bus_SystemEmail set ISDEFAULT =1 where id='{0}'", si.ID);
            sql += string.Format("update Acc_Bus_SystemEmail set ISDEFAULT =0 where id!='{0}'", si.ID);
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
            return "Views/manager/systememail.htm"; 
        }

        protected override void OnAdding(SaveEvent item)
        {
            base.OnAdding(item);
        }

        /// <summary>
        /// 发 送 电 子 邮 件 的 方 法
        /// </summary>
        /// <param name="subject">电 子 邮 件 的 主 题 行</param>
        /// <param name="body">电 子 邮 件 的 正 文 </param>
        /// <param name="displayName">电 子 邮 件 收 件 人 地 址</param>
        /// <param name="IsbodyHtml">邮 件 正 文 是 否 为 HTML 格 式 的 值</param>
        /// <param name="toName">收 件 人 名 称</param>
        public virtual void Send(string userName, string controllerName, string displayName, SystemEmail refSe)
        {
            ////获取系统内配置邮件信息默认数据（唯一一条数据）
            EntityList<SystemEmail> seList = new EntityList<SystemEmail>(this.model.GetDataAction());
            seList.GetData("ISDEFAULT =1");
            ////优先使用自定义传递过来的邮件信息配置数据，其次使用系统配置，完成不同需求
            SystemEmail se = null;
            if (refSe != null)
            {
                se = refSe;
            }
            else
            {
                if (seList.Count > 0)
                {
                    se = seList[0];
                    
                }

                if (se != null)
                {
                    //// 实例化一个发件人地址及名称
                    MailAddress from = new MailAddress(se.EMAILNAME, se.EMAILPASS);
                    //// 实例化一个收件人地址及名称
                    MailAddress to = new MailAddress(displayName, displayName);
                    MailMessage mail = new MailMessage();
                    //// 设置此子邮件的发信人地址 
                    mail.From = from;
                    mail.To.Add(to);
                    //// 设置此电子邮箱的主题行
                    mail.Subject = se.EMAILSUBJECT;
                    //// 设置邮件的正文 
                    mail.Body = EmailContent(userName, controllerName, se.DEFAULTEMAIL);
                    //// 设置指示邮件正文是否为HTML格式的值
                    mail.IsBodyHtml = true;

                    using (SmtpClient client = new SmtpClient())
                    {

                        client.Host = se.EMAILHOST;
                        client.Credentials = new System.Net.NetworkCredential(se.EMAILNAME, se.EMAILPASS);
                        client.EnableSsl = true;
                        try
                        {
                            client.Send(mail);
                        }
                        catch (Exception)
                        {
                        }
                        finally
                        {
                            if (mail != null)
                            {
                                mail.Dispose();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 设置邮件内容
        /// </summary>
        /// <param name="userName">发件人姓名</param>
        /// <param name="controllerName">页面功能说明</param>
        /// <param name="content">邮件内容</param>
        /// <returns></returns>
        public virtual string EmailContent(string userName,string controllerName,string content)
        {
            if (content != "")
            {
                return content;
            }
            else
            {

                return "您好：<font style='color:red;'>" + userName + "</font>提交的<font style='color:red;'>" + controllerName + "</font>流程等待您处理！";
            }
        }


    }
}
