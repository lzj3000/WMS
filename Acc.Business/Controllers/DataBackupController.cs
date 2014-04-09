using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Business.Controllers;
using Way.EAP.DataAccess.Data;
using System.Data;
using Acc.Contract.MVC;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model;
using Acc.Contract.Data;
using System.Configuration;
using System.Web.Configuration; 

//Acc.Business.Controllers.DataBackupController
namespace Acc.Business.Controllers
{
    /// <summary>
    /// 描述：数据备份控制器
    /// 作者：胡文杰
    /// 创建日期:2013-09-02
    /// </summary>
    public class DataBackupController : BusinessController
    {
        public DataBackupController() : base(new DataBackup()) { }

        //显示在菜单
        protected override string OnControllerName()
        {
            return "数据备份";
        }
        //菜单中url路径
        protected override string OnGetPath()
        {
            return "Views/manager/DataBackup.htm";
        }
        //开发人
        protected override string OnGetAuthor()
        {
            return "胡文杰";
        }
        //说明
        protected override string OnGetControllerDescription()
        {
            return "数据备份";
        }

        /// <summary>
        /// 单据字段状态控制
        /// </summary>
        protected override void OnInitViewChildItem(Acc.Contract.Data.ModelData data, Acc.Contract.Data.ItemData item)
        {
            #region 单据字段状态控制
            //商机
            if (data.name.EndsWith("DataBackup"))
            {
                switch (item.field.ToLower())
                {
                    case "remark":
                    case "createdby":
                    case "creationdate":
                    case "backdirectory":
                    case "filename":
                        item.visible = true;
                        break;
                    case "code":
                        item.visible = true;
                        item.disabled = true;
                        break;
                    default:
                        item.visible = false;
                        break;
                }
            }

            #endregion
        }

        /// <summary>
        /// 按钮控制
        /// </summary>
        protected override ActionCommand[] OnInitCommand(ActionCommand[] commands)
        {
            //获取所有按钮集合
            foreach (ActionCommand ac in commands)
            {
                if ( ac.command == "SubmitData")
                {
                    ac.visible = false;

                }
            }
            return base.OnInitCommand(commands);
        }



        /// <summary>
        /// 备份数据
        /// </summary>
        [ActionCommand(name = "备份", title = "备份", index = 1, icon = "icon-ok", isselectrow = false)]
        public void Backup()
        {
            string BackDirectory = AppDomain.CurrentDomain.BaseDirectory + "BackUp\\";
            //string BackDirectory = "D:\\backup\\";
            IDataAction da=this.model.GetDataAction();
            DataBackup bill = this.ActionItem as DataBackup;
            if (bill != null)
            {
                //避免重复删除
                if (string.IsNullOrEmpty(BackDirectory) || string.IsNullOrEmpty(bill.FileName))
                {
                    throw new Exception("请完整填写备份条件！");
                }
                else if (bill.FileName.Contains('.'))
                {
                    throw new Exception("文件名称中不能包含“.”字符！");
                }
                else
                {
                    // 备份数据库To disk='\\192.168.100.118\D$\qqq\NorthwindCS_Mirror_21.bak'
                    string backPath = string.Format(@"'\\{0}\{1}{2}.bak'", da.DataClient.ServerAddess, BackDirectory.Replace(":", "$"), bill.FileName);
                    string sql = string.Format("BACKUP DATABASE [{0}] TO DISK ={1} with init", da.DataClient.ServerName, backPath);
                    this.model.GetDataAction().Execute(sql);

                }
            }

        }

        /// <summary>
        /// 还原数据
        /// </summary>
        [ActionCommand(name = "还原", title = "还原", index = 2, icon = "icon-ok", isselectrow = false)]
        public void ReStore()
        {
            //string BackDirectory = AppDomain.CurrentDomain.BaseDirectory + "Interface\\BackUp\\";
            string BackDirectory = "D:\\backup\\";
            
            IDataAction da = this.model.GetDataAction();
            DataBackup bill = this.ActionItem as DataBackup;
            if (bill != null)
            {
                if (string.IsNullOrEmpty(BackDirectory) || string.IsNullOrEmpty(bill.FileName))
                {
                    throw new Exception("请完整填写还原条件！");
                }
                else
                {
                    // 还原数据库
                    string restore = string.Format(@"'\\{0}\{1}{2}.bak'", da.DataClient.ServerAddess, BackDirectory.Replace(":", "$"), bill.FileName);
                    //string restore = string.Format(@"use master RESTORE DATABASE [Acctrue_CRM_0415] FROM DISK='\\192.168.100.118\D$\wwwww\NorthwindCS_Mirror_21.bak' WITH REPLACE ");
                    //string backPath = string.Format(@"'\\{0}\{1}{2}.bak'", da.DataClient.ServerAddess, bill.BackDirectory.Replace(":", "$"), bill.FileName);
                    string sql = string.Format("use master RESTORE DATABASE [{0}] FROM DISK={1}  WITH REPLACE ", da.DataClient.ServerName+"aa", restore);
                    this.model.GetDataAction().Execute(sql);
                    this.SetWebConfig("database", da.DataClient.ServerName + "aa");
                }
            }
        }

        public void SetWebConfig(string key, string value)
        {
            string configPath = AppDomain.CurrentDomain.BaseDirectory + "Web.config";
            Configuration config = WebConfigurationManager.OpenWebConfiguration(configPath);  
            AppSettingsSection appSetting = (AppSettingsSection)config.GetSection("appSettings");
            if (appSetting.Settings[key] == null)//如果不存在此节点,则添加  
            {
                appSetting.Settings.Add(key, value);
            }
            else//如果存在此节点,则修改  
            {
                appSetting.Settings[key].Value = value;
            }
            config.Save();
            //config = null; 

        }  

        


    }
}
