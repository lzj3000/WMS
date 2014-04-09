using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Contract.MVC;
using Acc.Business.WMS.Model;
using Acc.Contract;
using System.Data;
using Acc.Business.Controllers;
using Way.EAP.DataAccess.Entity;
using Acc.Business.WMS.Controllers;
using Acc.Contract.Center;
using Way.EAP.DataAccess.Data;
using Way.EAP.DataAccess.Regulation;
using Acc.Contract.Data;
using Acc.Business.WMS.XHY.Model;
using Acc.Contract.Data.ControllerData;
using Acc.Business.Model;

namespace Acc.Business.WMS.XHY.Controllers
{
    public class ProduceInNoticeOrderController : XHY_InNoticeOrderController
    {
        public ProduceInNoticeOrderController() : base(new StockInNotice()) { }
        #region 页面设置

        //菜单中url路径
        protected override string OnGetPath()
        {
            return "Views/WMS/XHY/ProduceInNoticeOrder.htm";
        }

        /// <summary>
        /// 显示名
        /// </summary>
        /// <returns></returns>
        protected override string OnControllerName()
        {
            return "生产计划通知";
        }

        /// <summary>
        /// 启用下推
        /// </summary>
        public override bool IsPushDown
        {
            get { return true; }
        }

        #endregion



        #region 初始化数据方法
        protected override void OnInitViewChildItem(Contract.Data.ModelData data, Contract.Data.ItemData item)
        {
            base.OnInitViewChildItem(data, item);
            if (data.name.EndsWith("StockInNotice"))
            {
                data.title = "生产计划通知";
                switch (item.field.ToLower())
                {
                    case "code":
                        item.title = "生产计划单号";
                        item.visible = true;
                        item.disabled = true;
                        break;        
                    case "sourcename":
                    case "sourceoutcode":
                    case "workerid":
                    case "bumen":
                    case "stocktype":
                    case "finishtime":
                    case "sourcecode":
                    case "clientno":
                        item.visible = false;
                        break;
                    case "submiteddate":
                    case "submitedby":
                    case "createdby":
                    case "creationdate":
                    case "modifiedby":
                    case "modifieddate":
                    case "reviewedby":
                    case "revieweddate":
                    case "issubmited":
                    case "isreviewed":
                        item.disabled = true;
                        break;
                    case "stay5":
                        item.visible = true;
                        item.title = "车间";
                        item.index = 2;
                      
                        item.required = true;
                        break;
                
                }
            }
            if (data.name.EndsWith("StockInNoticeMaterials"))
            {
                data.title = "生产计划通知明细";
                switch (item.field.ToLower())
                {
                    case "code":
                    case "sourcecode":
                    case "parentid":
                        item.visible = false;
                        break;
                    case "num": 
                        item.title = "计划生产数量";
                        item.required = true;
                        break;
                    case "staynum":
                    case "finishnum":
                        item.visible = false;
                        break;
                    case "batchno":
                        item.visible = true;
                        item.title = "生产批号";
                        item.required = true;
                        break;
                    case "sourcename":
                    case "sourceoutcode":
                    case "workerid":
                    case "bumen":
                    case "stocktype":
                    case "finishtime":
                        item.visible = false;
                        break;
                    case "mcode":
                    case "fmodel":
                        item.disabled = true;
                        break;
                    case "materialcode":
                        item.required = true;
                        break;

                }
            }
        }
       
        protected override void OnForeignLoading(IModel model, loadItem item)
        {
            //////选择半成品项
            if (this.fdata.filedname == "MATERIALCODE")
            {
                item.rowsql = "select * from (" + item.rowsql + ") a where a.CommodityType=1";
               // item.rowsql = "select id,code,FNAME,FFULLNAME,ClassID,FUNITID,BATCH,STATUS,ISOVERIN,IsDisable,ISCHECKPRO,Remark,IsDelete from " + new BusinessCommodity().ToString() + " a where a.CommodityType=1";
            }
            //if (this.fdata.filedname == "MATERIALCODE")
            //{
            //    item.rowsql = "select * from (" + item.rowsql + ") a where a.CommodityType=1";
            //}
            if (this.fdata.filedname == "STAY5")
            {
                base.OnForeignLoading(model, item);
                //item.rowsql = "select id,CODE,OrganizationName,ParentID,IsDisable,Remark from  " + new Organization().ToString();
            }

            base.OnForeignLoading(model, item);
        }
        #endregion

        #region 重写方法
        public override int SetStockType()
        {
            return 2;
        }
        /// <summary>
        /// 设置下推对象
        /// </summary>
        /// <returns></returns>
        public override ControllerBase PushController()
        {
            return new SemInOrderController();
        }
        protected override void OnAdding(ControllerBase.SaveEvent item)
        {
            StockInNotice si = item.Item as StockInNotice;
            if(string.IsNullOrEmpty(si.STAY5))
            {
                throw new Exception ("异常：车间不能为空！");
            }
            base.OnAdding(item);
        }

        #endregion

        #region 下推功能
        public override string GetThisController()
        {
            return new SemInOrderController().ToString();
        }

        public override BusinessController bc()
        {
            return new SemInOrderController();
        }

        #endregion
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
                if (ac.command == "UnSubmitData")
                {
                    ac.visible = false;
                }
                if (ac.command == "add" || ac.command == "edit" || ac.command == "remove" || ac.command == "SubmitData")
                {
                    ac.visible = true;
                }
            }
            return coms;
            
        }
        protected override void OnRemoveing(ControllerBase.SaveEvent item)
        {
            BasicInfo sio = item.Item as BasicInfo;
            if (sio.Createdby == this.user.ID)
            {
                base.OnRemoveing(item);
            }
            else
            {
                throw new Exception("单据创建人与当前登陆人不符，不能删除！！");
            }
        }

    }
}
