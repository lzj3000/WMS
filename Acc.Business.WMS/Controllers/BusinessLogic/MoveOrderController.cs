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
using Acc.Contract.Data.ControllerData;

namespace Acc.Business.WMS.Controllers
{
    public class MoveOrderController : BusinessController
    {
        /// <summary>
        /// 描述：移位单控制器
        /// 作者：路聪
        /// 创建日期:2013-02-27
        /// </summary>
        public MoveOrderController() : base(new MoveOrder()) { }
        public MoveOrderController(IModel model) : base(model) { }

        //是否启用审核
        public override bool IsReviewedState
        {
            get
            {
                return false
;
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
                        item.title = "移位单号";
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
                    case "pid":
                        item.disabled = true;
                        item.visible = false;
                        break;
                }
            }
            if (data.name.EndsWith("StockSequence"))
            {
                data.visible = false;
            }
            if (data.name.EndsWith("MoveOrderMaterials"))
            {
                data.disabled = true;
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
                    case "sourcecode":
                    case "parentid":
                    case "pid":
                        item.visible = false;
                        item.isedit = false;
                        break;
                    case "materialcode":
                        item.index = 1;
                        break;
                    case "mcode":
                        item.index = 2;
                        break;
                    case "fmodel":
                        item.index = 3;
                        break;
                    case "batchno":
                        item.index = 4;
                        break;
                    case "num":
                        item.index = 5;
                        break;
                    case "fromdepot":
                        item.index = 6;
                        break;
                    case "fromport":
                        item.index = 7;
                        break;
                    case "todepot":
                        item.index = 8;
                        break;
                    case "toport":
                        item.index = 9;
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
            MoveDepots(item);
            base.OnAdding(item);
        }



        protected override void OnEditing(Contract.MVC.ControllerBase.SaveEvent item)
        {
            MoveDepots(item);
            base.OnEditing(item);
        }

        [WhereParameter]
        public List<InfoInSequence> InfoSequences { get; set; }

        public virtual void MoveDepots(Contract.MVC.ControllerBase.SaveEvent item)
        {
            IDataAction action = this.model.GetDataAction();
            try
            {
                action.StartTransation();//开启事物
                MoveOrder mo = item.Item as MoveOrder;
                StockInfoMaterials sm;
                StockInfoMaterialsController sims = new StockInfoMaterialsController();
                foreach (MoveOrderMaterials mom in mo.Materials)
                {
                    sm = new StockInfoMaterials();
                    sm.DEPOTWBS = mom.FROMDEPOT.ToString();
                    sm.PORTCODE = mom.FROMPORT;
                    sm.NUM = mom.NUM;
                    sm.Code = mom.MATERIALCODE;
                    sm.BATCHNO = mom.BATCHNO;
                    sm.WAREHOUSEID = mo.DEPOTWBS;
                    sm.MCODE = mom.GetForeignObject<Materials>().Code;
                    sm.FMODEL = mom.GetForeignObject<Materials>().FMODEL;
                    if (InfoSequences != null)
                    {
                        for (int i = 0; i < InfoSequences.Count; i++)
                        {
                            sm.StockSequence.Add(InfoSequences[i]);
                        }
                    }
                    sims.putin(sm, "出库", action);
                    if (mom.TODEPOT > 0)
                        sm.DEPOTWBS = mom.TODEPOT.ToString();
                    if (!string.IsNullOrEmpty(mom.TOPORT))
                        sm.PORTCODE = mom.TOPORT;
                    sims.putin(sm, "入库", action);
                }
                action.Commit();
                #region 不要
                //StockInfoMaterials smy = null; //源托盘仓库
                //StockInfoMaterials sm = null; //目标托盘仓库
                //Materials ms = null;
                //MoveOrderMaterials mom = mo.Materials[0];
                //EntityList<StockInfoMaterials> ListYkc = new EntityList<StockInfoMaterials>(action);
                //EntityList<StockInfoMaterials> ListMku = new EntityList<StockInfoMaterials>(action);
                //StockInfoMaterialsController sims = new StockInfoMaterialsController();
                ////判断是否目标货位是否为0，不为0说明是托盘移到货位，为0说明是托盘移动到托盘
                //if (mom.TODEPOT != 0)
                //{
                //    ListYkc.GetData("ID=" + mom.STAY1);
                //    ms = ListYkc[0].GetForeignObject<Materials>(action);
                //    ListYkc[0].NUM = mom.NUM;
                //    smy = ListYkc[0];
                //    sims.putin(ListYkc[0], "出库", action);
                //    smy.DEPOTWBS = mom.TODEPOT.ToString();
                //    sims.putin(smy, "入库", action);
                //}
                ////托盘移到托盘
                //else
                //{
                //    ListYkc.GetData("ID=" + mom.STAY1);
                //    ms = ListYkc[0].GetForeignObject<Materials>(action);
                //    smy = ListYkc[0];
                //    smy.NUM = mom.NUM;
                //    //查找目标信息，如果能找到说明移到的目标托盘存在仓库中，如果找不到说明是新托盘
                //    Ports ps = mom.GetForeignObject<Ports>(action);
                //    ListMku.GetData("Code='" + smy.Code + "' and WAREHOUSEID=" + smy.WAREHOUSEID + " and PORTCODE='" + ps.Code + "' ");
                //    if (ListMku.Count > 0)
                //    {
                //        ListMku[0].NUM = mom.NUM;
                //        sims.putin(smy, "出库", action);
                //        sims.putin(ListMku[0], "入库", action);
                //    }
                //    //找不到目标信息，说明是新托盘
                //    else
                //    {
                //        sm = new StockInfoMaterials();
                //        sm.Code = smy.Code;
                //        sm.MCODE = smy.MCODE;
                //        sm.BATCHNO = smy.BATCHNO;
                //        sm.WAREHOUSEID = smy.WAREHOUSEID;
                //        sm.PORTCODE = mom.TOPORT;
                //        sm.LASTINTIME = smy.LASTINTIME;
                //        sm.NUM = mom.NUM;
                //        sims.putin(smy, "出库", action);
                //        sims.putin(sm, "入库", action);
                //    }
                //}
                //action.Commit();
                #endregion
            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                action.EndTransation();//结束事物
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

        /*
        /// <summary>
        /// 审核移位单
        /// </summary>
        /// <param name="info"></param>
        protected override void OnReviewedData(Business.Model.BasicInfo info)
        {
            MoveOrder mo = info as MoveOrder;
            mo.Materials.GetData();
            if (mo.Materials.Count > 0)
            {
                string sql="";
                foreach (var item in mo.Materials)
                {
                    if (item.TODEPOT != item.FROMDEPOT)
                    {
                        sql += "update acc_wms_stockinfo_materials set depotwbs='" + item.TODEPOT + "',remark='来源移位单号：" + mo.Code + "' where id='" + item.NEWCODE + "'";
                    }
                    else
                    {
                        throw new Exception("异常：库存项"+item.NEWCODE+"目标位置与原位置相同");
                    }
                    IDataAction action = this.model.GetDataAction();
                    try
                    {
                        action.Execute(sql);
                    }
                    catch(Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
                base.OnReviewedData(info);
            }
            else
            {
                throw new Exception("异常：明细行数据为空");
            }
        } */
    }

}
