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
using Acc.Contract.Data.ControllerData;

//Acc.Business.Controllers.DataInitializeController
namespace Acc.Business.Controllers
{
    /// <summary>
    /// 描述：数据初始化控制器
    /// 作者：胡文杰
    /// 创建日期:2013-09-02
    /// </summary>
    public class DataInitializeController : BusinessController
    {
        public DataInitializeController() : base(new SystemModel()) { }

        //显示在菜单
        protected override string OnControllerName()
        {
            return "数据初始化";
        }
        //菜单中url路径
        protected override string OnGetPath()
        {
            return "Views/manager/DataInitialize.htm";
        }
        //开发人
        protected override string OnGetAuthor()
        {
            return "胡文杰";
        }
        //说明
        protected override string OnGetControllerDescription()
        {
            return "数据初始化";
        }

        /// <summary>
        /// 按钮控制
        /// </summary>
        protected override ActionCommand[] OnInitCommand(ActionCommand[] commands)
        {
            //获取所有按钮集合
            foreach (ActionCommand ac in commands)
            {
                if (ac.command == "remove" || ac.command == "edit" || ac.command == "add" || ac.command == "SubmitData")
                {
                    ac.visible = false;

                }
            }
            return base.OnInitCommand(commands);
        }

        protected override void OnInitViewChildItem(ModelData data, ItemData item)
        {
            if (data.name.EndsWith("SystemModelData") || data.name.EndsWith("ModelCommand"))
            {
                data.visible = false;
            }
        }

        /// <summary>
        /// 删除所有表
        /// </summary>
        [ActionCommand(name = "系统初始化", title = "系统初始化", index = 1, icon = "icon-ok", isselectrow = false)]
        public void InitializeSystem()
        {
            IDataAction da = this.model.GetDataAction();
            //获取所有表
            string sqlGetAllTables = string.Format("select name  from sysobjects where type ='U'");
            DataTable dtTables = da.GetDataTable(sqlGetAllTables);
            if(dtTables.Rows.Count==0){return;}
            foreach (DataRow dr in dtTables.Rows)
            {
                string tableName = dr["name"].ToString();
                switch (tableName)
                {
                    //case "Acc_Bus_SystemModel"://系统模块
                    //case "Acc_Bus_ModelCommand"://模块命令
                    //case "Acc_Bus_ModelData"://模块数据
                    //case "Acc_Bus_OverseeModel"://监视追踪管理
                    //case "Acc_Bus_Role"://角色
                    //case "Acc_Bus_RoleModel"://角色模块
                    case "Acc_Bus_BasicData":
                    case "Acc_Bus_ModelCommand":
                    case "Acc_Bus_ModelData":
                    case "Acc_Bus_OverseeModel":
                    case "Acc_Bus_Region":
                    case "Acc_Bus_Role":
                    case "Acc_Bus_RoleCommand":
                    case "Acc_Bus_RoleModel":
                    case "Acc_Bus_RoleModelData":
                    case "Acc_Bus_SetPrintModel":
                    case "Acc_Bus_SystemDirectory":
                    case "Acc_Bus_SystemEmail":
                    case "Acc_Bus_SystemInfo":
                    case "Acc_Bus_SystemInfoDetials":
                    case "Acc_Bus_SystemModel":
                    case "Acc_Bus_UserRole":
                    case "Acc_WMS_MobileSetModel":
                    case "Acc_WMS_MobileSetModelList":
                    case "Acc_WMS_MobileSetProject":
                    case "PrintTemplate":
                    case "PrintTemplateHistory":
                    case "PrintTemplateParm":
                    case "Acc_Bus_OverseeDetails":
                        //关键表-无操作
                        break;
                    case "Acc_Bus_OfficeWorker":
                        //职员表-保留管理员
                        string sqlDelRows = string.Format("if exists(select * from sysobjects where name='{0}') begin delete [{1}] where id>1 end", tableName, tableName);
                        da.Execute(sqlDelRows);
                        break;
                    default:
                        //其他表-删除表内容
                        string sqlDelTable = string.Format("if exists(select * from sysobjects where name='{0}') begin delete [{1}] end", tableName, tableName);
                        da.Execute(sqlDelTable);
                        //其他表-删除表结构
                        //string sqlDropTable = string.Format("if exists(select * from sysobjects where name='{0}') begin drop table [{1}] end", tableName, tableName);
                        //da.Execute(sqlDropTable);
                        break;
                }

            }
        }

        /// <summary>
        /// 初始化选中模块,及该模块下关联的所有表
        /// </summary>
        [ActionCommand(name = "初始化", title = "初始化", index = 1, icon = "icon-ok", isselectrow = false, onclick = "GetSelectModels")]//onclick = "GetSelectModels"执行前台脚本，获取所选ids
        public void Initialize()
        {
            //IDataAction da = this.model.GetDataAction();
            //SystemModel bill = this.ActionItem as SystemModel;
            //if (bill != null)
            //{
            //    bill.DataItems.DataAction = da;
            //    bill.DataItems.GetData();
            //    if (bill.DataItems.Count == 0) { return; }
            //    for (int i = 0; i < bill.DataItems.Count; i++)
            //    {
            //        SystemModelData smd = bill.DataItems[i];
            //        //避免重复删除
            //        if (i != 0 && smd.ModelTableName == bill.DataItems[i - 1].ModelTableName)
            //        {
            //            continue;
            //        }
            //        if (smd.ModelTableName != "Acc_Bus_ProcessState")
            //        {
            //            //删除表
            //            string sql = string.Format("if exists(select * from sysobjects where name='{0}') begin drop table {1} end", smd.ModelTableName, smd.ModelTableName);
            //            da.Execute(sql);
            //            //删除该表相关的流程记录
            //            string sqlProcessState = string.Format("delete from Acc_Bus_ProcessState where TableName='{0}'", smd.ModelTableName);
            //            da.Execute(sqlProcessState);

            //        }

            //    }
            //}

        }

        /// <summary>
        /// 传递参数：传递前台所选ids
        /// </summary>
        [WhereParameter]
        public string ids { get; set; }

        //初始化选中模块
        public void InitializeSelectModels()//供前台脚本函数GetSelectModels调用
        {
            IDataAction da = this.model.GetDataAction();
            string[] StrParameterList = ids.Split(';');
            for (int a = 0; a < StrParameterList.Length; a++)
            {
                IEntityList billList = new EntityList<SystemModel>(da);
                billList.GetData("ID=" + StrParameterList[a].ToString());//根据id找出所选模块
                if (billList.Count == 0) { continue; }
                SystemModel bill = billList[0] as SystemModel;
                bill.DataItems.DataAction = da;
                bill.DataItems.GetData();//模块相关联的数据
                if (bill.DataItems.Count == 0) { continue; }
                for (int i = 0; i < bill.DataItems.Count; i++)
                {
                    SystemModelData smd = bill.DataItems[i];
                    //避免重复删除
                    if (i != 0 && smd.ModelTableName == bill.DataItems[i - 1].ModelTableName)
                    {
                        continue;
                    }
                    if (smd.ModelTableName != "Acc_Bus_ProcessState")
                    {
                        //删除表
                        string sql = string.Format("if exists(select * from sysobjects where name='{0}') begin drop table {1} end", smd.ModelTableName, smd.ModelTableName);
                        da.Execute(sql);
                        //删除该表相关的流程记录
                        string sqlProcessState = string.Format("delete from Acc_Bus_ProcessState where TableName='{0}'", smd.ModelTableName);
                        da.Execute(sqlProcessState);

                    }

                }

            }

            
        }





        /// <summary>
        /// 删除所有模块下关联的表--初始整个系统
        /// </summary>
        public void InitializeAll()
        {
            IEntityList modelList = new EntityList<SystemModel>(this.model.GetDataAction());//外系统数据集
            modelList.GetData();
            if (modelList.Count == 0) { return; }
            foreach (SystemModel bill in modelList)
            {

                if (bill != null)
                {
                    bill.DataItems.DataAction = this.model.GetDataAction();
                    bill.DataItems.GetData();
                    //foreach (SystemModelData smd in bill.DataItems)
                    for (int i = 0; i < bill.DataItems.Count; i++)
                    {
                        SystemModelData smd = bill.DataItems[i];
                        //避免重复删除
                        if (i != 0 && smd.ModelTableName == bill.DataItems[i - 1].ModelTableName)
                        {
                            continue;
                        }
                        if (smd.ModelTableName != "Acc_Bus_ProcessState")
                        {
                            //删除表
                            string sql = string.Format("if exists(select * from sysobjects where name='{0}') begin drop table {1} end", smd.ModelTableName, smd.ModelTableName);
                            this.model.GetDataAction().Execute(sql);
                            //删除该表相关的流程记录
                            string sqlProcessState = string.Format("delete from Acc_Bus_ProcessState where TableName='{0}'", smd.ModelTableName);
                            this.model.GetDataAction().Execute(sqlProcessState);

                        }

                    }
                }
            }

        }

        
    }
}
