using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Contract.Outside;
using Acc.Business.Controllers;
using Acc.Contract.Data;
using Way.EAP.DataAccess.Data;
using Way.EAP.DataAccess.Entity;
using Acc.Contract.MVC;
using System.Data;
using Acc.Business.Model.Interface;

//Acc.Business.Controllers.InterfaceTemplateController
namespace Acc.Business.Controllers
{
    public class InterfaceTemplateController : BusinessController
    {
        public InterfaceTemplateController() : base(new InterfaceTemplate()) { }

        //显示在菜单
        protected override string OnControllerName()
        {
            return "接口模版";
        }
        //菜单中url路径
        protected override string OnGetPath()
        {
            return "Views/manager/InterfaceTemplate.htm";
        }
        //开发人
        protected override string OnGetAuthor()
        {
            return "胡文杰";
        }
        //说明
        protected override string OnGetControllerDescription()
        {
            return "接口模版";
        }

        public override bool IsPrint
        {
            get
            {
                return false;
            }
        }

        public override bool IsClearAway
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 控制按钮
        /// </summary>
        protected override ActionCommand[] OnInitCommand(ActionCommand[] commands)
        {
            //获取所有按钮集合
            foreach (ActionCommand ac in commands)
            {
                if (ac.command == "SubmitData")
                {
                    ac.visible = false;
                }
            }
            return base.OnInitCommand(commands);
        }

        #region 增删改查
        /// <summary>
        /// 添加
        /// </summary>
        protected override void OnAdding(ControllerBase.SaveEvent item)
        {
            EntityList<InterfaceTemplate> list = new EntityList<InterfaceTemplate>(this.model.GetDataAction());
            list.GetData("TemplateName='" + item.Item["TemplateName"] + "'");
            if (list.Count > 0)
            {
                item.Breakoff = true;
                throw new Exception("模版名已存在，不能重复，请更改后重试！");
            }
            base.OnAdding(item);
        }

        protected override void OnAdded(EntityBase item)
        {
            base.OnAdded(item);
            //转换关系只能添加一次
            IEntityList check=new EntityList<MappingInfo>(this.model.GetDataAction());
            check.GetData();
            if (check.Count > 0) 
            { 
                return;
            }
            //根据模版添加数据转换关系
            InterfaceTemplate bill = item as InterfaceTemplate;
            if (bill != null)
            {
                //IEntityList list = XMLParse.XmlMappingInfoConfig("k3_DataTransform.xml", bill.ID);
                IEntityList list = XMLParse.XmlMappingInfoConfig("MappingInfo.xml", bill.ID);
                list.DataAction = this.model.GetDataAction();
                list.Save();
            }
            ////U8调用
            //if (bill != null)
            //{
            //    IEntityList list = XMLParse.XmlConfig("DataMapping.xml", bill.ID);
            //    list.DataAction = this.model.GetDataAction();
            //    list.Save();
            //}
        }


        /// <summary>
        /// 删除
        /// </summary>
        protected override void OnRemoveing(ControllerBase.SaveEvent item)
        {
            //InterfaceTemplate bill = item.Item as InterfaceTemplate;
            //((IEntityBase)bill).StateBase = EntityState.Select;
            //if (bill != null)
            //{
            //    IEntityList list = new EntityList<DataTransform>(this.model.GetDataAction());
            //    list.GetData();
            //    for (int i = 0; i <= list.Count; i++)
            //    {
            //    }
            //    foreach (DataTransform dtf in list)
            //    {
            //        if (dtf.TemplateID == bill.ID)
            //        {
            //            list.Remove(dtf);
            //        }
            //    }
            //    list.Save();
            //}
            base.OnRemoveing(item);
        }
        #endregion
        

        [ActionCommand(name = "测试连接", title = "测试连接", index = 1, icon = "icon-ok", isselectrow = false)]
        public void TestConnection()
        {
            InterfaceTemplate it = this.ActionItem as InterfaceTemplate;
            if (it != null)
            {
                //外系统连接
                //IDataAction da = new DataBaseManage(new SQLServerDataBase("192.168.100.126", "AIS20130801171831", "sa", "sa_AccTrue"));
                //IDataAction da = new DataBaseManage(new SQLServerDataBase(it.ServerIP, it.DBName, it.DBUserName, "sa_AccTrue"));
                IDataAction da = new DataBaseManage(new SQLServerDataBase(it.ServerIP, it.DBName, it.DBUserName, it.DBPassword));
                string sql = string.Format("select count(*) from t_item");
                DataTable result= da.GetDataTable(sql);
                if (result.Rows.Count > 0)
                {
                    throw new Exception("连接成功！");
                }
                else
                {
                    throw new Exception("连接异常！");
                }

            }
        }
    }
}
