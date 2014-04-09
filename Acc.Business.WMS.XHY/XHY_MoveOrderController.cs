using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Business.Controllers;
using Acc.Business.WMS.Model;
using Acc.Contract.Data;
using Way.EAP.DataAccess.Data;
using Way.EAP.DataAccess.Entity;
using Acc.Contract.MVC;
using Acc.Business.WMS.Controllers;
using System.Data;

namespace Acc.Business.WMS.XHY.Controllers
{
    public class XHY_MoveOrderController : MoveOrderController
    {
        /// <summary>
        /// 描述：移位单控制器
        /// 作者：路聪
        /// 创建日期:2013-02-27
        /// </summary>
        public XHY_MoveOrderController() : base(new MoveOrder()) { }
        public XHY_MoveOrderController(IModel model) : base(model) { }

        //是否启用审核
        public override bool IsReviewedState
        {
            get
            {
                return true;
            }
        }
        //是否启用提交
        public override bool IsSubmit
        {
            get
            {
                return true;
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

        #region 初始化数据方法
        protected override void OnInitViewChildItem(Contract.Data.ModelData data, Contract.Data.ItemData item)
        {
            if (data.name.EndsWith("MoveOrder"))
            {
                switch (item.field.ToLower())
                {
                    case "modifiedby":
                    case "modifieddate":
                    case "isdisable":
                    case "isdelete":
                    case "id":
                        item.visible = false;
                        item.isedit = false;
                        break;
                    case "code":
                        item.title = "编码";
                        item.visible = true;
                        item.disabled = true;
                        break;
                    case "rowindex":
                         item.visible = true;
                        item.disabled = true;
                        break;
                    case "submiteddate":
                    case "submitedby":
                    case "createdby":
                    case "creationdate":
                    case "reviewedby":
                    case "revieweddate":
                    case "issubmited":
                    case "isreviewed":
                    case "stay1":
                    case "stay2":
                    case "stay3":
                    case "stay4":
                    case "stay5":
                        item.disabled = true;
                        item.visible = true;
                        break;
                }
            }
            if (data.name.EndsWith("StockSequence"))
            {
                data.visible = false;
            }
            if (data.name.EndsWith("MoveOrderMaterials"))
            {
                if (item.field == "DEPOTWBS")
                {
                    item.index = 3;
                   
                }
                
                if (item.field == "NEWCODE")
                {
                    if (item.foreign != null)
                    {
                        item.foreign.rowdisplay.Add("Code", "NEWMNAME");
                        item.foreign.rowdisplay.Add("MCODE", "NEWMCODE");
                        item.foreign.rowdisplay.Add("FMODEL", "NEWFMODEL");
                        item.foreign.rowdisplay.Add("ISLOCK", "ISLOCK");
                        item.foreign.rowdisplay.Add("BATCHNO", "BATCHNO");
                        item.foreign.rowdisplay.Add("SENBATCHNO", "SENBATCHNO");
                        item.foreign.rowdisplay.Add("DEPOTWBS", "DEPOTWBS");
                        item.foreign.rowdisplay.Add("NUM", "NUM");
                        item.foreign.rowdisplay.Add("LASTINTIME", "LASTINTIME");
                        item.foreign.rowdisplay.Add("STATUS", "STATUS");
                        item.foreign.rowdisplay.Add("SALEAREA", "SALEAREA");
                        item.foreign.rowdisplay.Add("PORTCODE", "PORTCODE");
                        item.foreign.rowdisplay.Add("HOUSECODE", "HOUSECODE");
                        item.foreign.rowdisplay.Add("ORDERNO", "ORDERNO");
                        item.foreign.rowdisplay.Add("FREEZENUM", "FREEZENUM");

                    }
                }
                switch (item.field.ToLower())
                {
                    case "createdby":
                    case "creationdate":
                    case "submiteddate":
                    case "submitedby":
                    case "modifiedby":
                    case "modifieddate":
                    case "reviewedby":
                    case "revieweddate":
                    case "issubmited":
                    case "isreviewed":
                    case "isdisable":
                    case "isdelete":
                    case "id":
                    case "code":
                    case "stay1":
                    case "stay2":
                    case "stay3":
                    case "stay4":
                    case "stay5":
                        item.visible = false;
                        item.isedit = false;
                        break;
                    default:
                        item.visible = true;
                        break;
                }
            }
        }

        #endregion

        //显示在菜单
        protected override string OnControllerName()
        {
            return "移位单管理";
        }
        //菜单中url路径
        protected override string OnGetPath()
        {
            return "Views/WMS/Materials/MoveOrder.htm";
        }
        //开发人
        protected override string OnGetAuthor()
        {
            return "柳强";
        }
        //说明
        protected override string OnGetControllerDescription()
        {
            return "移位单管理";
        }

        protected override void OnAdding(Contract.MVC.ControllerBase.SaveEvent item)
        {
            base.OnAdding(item);
        }

        

        protected override void OnEditing(Contract.MVC.ControllerBase.SaveEvent item)
        {
            base.OnEditing(item);
        }

        public override void MoveDepots(Contract.MVC.ControllerBase.SaveEvent item)
        {
            MoveOrder mo = item.Item as MoveOrder;
            MoveOrderMaterials mom = mo.Materials[0];
            EntityList<StockInfoMaterials> comList = new EntityList<StockInfoMaterials>(this.model.GetDataAction());
            comList.GetData(" Code='" + mom.MATERIALCODE + "' and BATCHNO='" + mom.BATCHNO + "' and DEPOTWBS=" + mom.FROMDEPOT + " and PORTCODE="+mom.FROMPORT);
            EntityList<StockInfoMaterials> sifms = new EntityList<StockInfoMaterials>(this.model.GetDataAction());
            sifms.GetData(" Code='" + mom.MATERIALCODE + "' and BATCHNO='" + mom.BATCHNO + "' and DEPOTWBS=" + mom.TODEPOT + " and PORTCODE="+mom.TOPORT);
            base.MoveDepots(item);
            if (comList.Count > 0)
            {
                string strSql = "update Acc_WMS_InfoMaterials set FMODEL='" + comList[0].FMODEL + "',STATUS='" + comList[0].STATUS + "',LASTINTIME='" + comList[0].LASTINTIME + "',ORDERNO='" + comList[0].ORDERNO + "',Createdby='" + comList[0].Createdby + "',Creationdate='" + comList[0].Creationdate + "',Remark='" + comList[0].Remark + "',MCODE='" + comList[0].MCODE + "' where Code='" + mom.MATERIALCODE + "' and BATCHNO='" + mom.BATCHNO + "' and DEPOTWBS=" + mom.FROMDEPOT + " and PORTCODE=" + mom.FROMPORT;
                if(sifms.Count==0)
                    strSql += " update Acc_WMS_InfoMaterials set FMODEL='" + comList[0].FMODEL + "',STATUS='" + comList[0].STATUS + "',LASTINTIME='" + comList[0].LASTINTIME + "',ORDERNO='" + comList[0].ORDERNO + "',Createdby='" + comList[0].Createdby + "',Creationdate='" + comList[0].Creationdate + "',Remark='" + comList[0].Remark + "',MCODE='" + comList[0].MCODE + "' where Code='" + mom.MATERIALCODE + "' and BATCHNO='" + mom.BATCHNO + "' and DEPOTWBS=" + mom.TODEPOT + " and PORTCODE=" + mom.TOPORT;
                this.model.GetDataAction().Execute(strSql);
            }
        }

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
                if (ac.command == "UnReviewedData")
                {
                    ac.visible = false;
                }
                if (ac.command == "add")
                {
                    ac.visible = false;
                }
                if (ac.command == "edit")
                {
                    ac.visible = false;
                }
                if (ac.command == "SubmitData")
                {
                    ac.visible = false;
                }
                if (ac.command == "ReviewedData")
                {
                    ac.visible = false;
                }
                if (ac.command == "remove")
                {
                    ac.visible = false;
                }
              
            }
            return coms;
        }
    }
        
}
