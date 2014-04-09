using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Acc.Business.Controllers;
using Acc.Business.WMS.Model;
using Acc.Contract.Data;
using System.Collections;
using Acc.Contract.MVC;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model;
using Acc.Contract.Data.ControllerData;

namespace Acc.Business.WMS.Controllers
{
    public class CheckOrderController : BusinessController
    {
        /// <summary>
        /// 描述：盘点单管理控制器
        /// 作者：路聪
        /// 创建日期:2012-12-25
        /// </summary>
        public CheckOrderController() : base(new CheckOrder())
        {
        }

        public CheckOrderController(IModel model) : base(model)
        {
        }

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
            return "盘点单管理";
        }

        //菜单中url路径
        protected override string OnGetPath()
        {
            return "Views/WMS/Check/CheckOrder.htm";
        }

        //开发人
        protected override string OnGetAuthor()
        {
            return "路聪";
        }

        //说明
        protected override string OnGetControllerDescription()
        {
            return "盘点单管理";
        }

        #region 初始化数据方法

        protected override ActionCommand[] OnInitCommand(ActionCommand[] commands)
        {
            ActionCommand[] coms = base.OnInitCommand(commands);
            //获取所有按钮集合
            foreach (ActionCommand ac in coms)
            {
                if (ac.command == "importin")
                {
                    ac.onclick = "ImportWindow(this)";
                }
                if (ac.command == "UnReviewedData")
                {
                    ac.visible = false;
                }
            }
            return coms;
        }

        protected override void OnInitViewChildItem(Contract.Data.ModelData data, Contract.Data.ItemData item)
        {
            base.OnInitViewChildItem(data, item);
            if (data.name.EndsWith("CheckOrder"))
            {
                switch (item.field.ToLower())
                {

                    case "submiteddate":
                    case "submitedby":
                    case "reviewedby":
                    case "revieweddate":
                    case "createdby":
                    case "creationdate":
                    case "code":
                    case "parentid":

                        item.disabled = true;
                        item.visible = true;
                        break;
                    case "modifiedby":
                    case "modifieddate":
                    case "isdisable":
                    case "isdelete":
                    case "id":
                    case "stay1":
                    case "stay2":
                    case "stay3":
                    case "stay4":
                    case "stay5":
                    case "classid":
                        item.visible = false;
                        break;
                    case "ordername":
                    case "warehousename":
                    case "materialcode":
                    case "checkordertype":
                        item.visible = true;
                        item.disabled = false;
                        break;
                    case "issubmited":
                    case "isreviewed":
                        item.disabled = true;
                        break;
                    default:
                        item.visible = true;
                        break;

                }
            }
            if (data.name.EndsWith("CheckOrderMaterials"))
            {
                data.disabled = true;
                if (item.field == "MATERIALCODE")
                {
                    if (item.foreign != null)
                    {
                        item.foreign.rowdisplay.Add("Code", "NEWMCODE");
                        item.foreign.rowdisplay.Add("FMODEL", "NEWFMODEL");
                    }
                }
                switch (item.field.ToLower())
                {
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
                    case "isdisable":
                    case "isdelete":
                    case "parentid":
                    case "stay1":
                    case "stay2":
                    case "stay3":
                    case "stay4":
                    case "stay5":
                    case "code":
                    case "sourcecode":
                        item.visible = false;
                        break;
                    case "materialcode":
                    case "pnum":
                    case "pbatchno":
                    case "pdepotwbs":
                        item.disabled = false;
                        break;
                    default:
                        item.visible = true;
                        item.disabled = true;
                        break;
                }
            }
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="item"></param>
        protected override void OnAdding(Contract.MVC.ControllerBase.SaveEvent item)
        {
            //Merger(item);
            ValidateItem(item);
            base.OnAdding(item);

        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="item"></param>
        protected override void OnEditing(Contract.MVC.ControllerBase.SaveEvent item)
        {
            // Merger(item);
            ValidateItem(item);
            base.OnEditing(item);
        }

        ///<summary>
        ///选择仓库和货区及货位的事件
        ///</summary>
        ///<param name="model"></param>
        ///<param name="item"></param>
        protected override void OnForeignLoading(IModel model, loadItem item)
        {
            ///选择仓库的sql
            if (this.fdata.filedname == "WAREHOUSENAME")
            {
                item.rowsql = "select * from (" + item.rowsql + ") a where a.ParentId=0";
            }
          
            base.OnForeignLoading(model, item);
        }

        protected override void OnForeignIniting(IModel model, InitData data)
        {
            if (model is CheckOrder)
            {
                //序号、编码code、名称FNAME、全名FFULLNAME、大类ClassID，基本计量单位FUNITID、批次管理BATCH、状态STATUS、是否迅速超出ISOVERIN是否禁用IsDisable、是否质检ISCHECKPRO、备注Remark
                if (this.fdata.filedname == "MATERIALCODE")
                {
                    foreach (ItemData d in data.modeldata.childitem)
                    {
                        d.visible = false;
                        if (d.IsField("Code"))
                        {
                            d.visible = true;
                            d.title = "商品编码";
                        }
                        if (d.IsField("FNAME"))
                            d.visible = true;
                        if (d.IsField("FFULLNAME"))
                            d.visible = true;
                        if (d.IsField("ClassID"))
                            d.visible = true;
                        if (d.IsField("FUNITID"))
                            d.visible = true;
                        if (d.IsField("BATCH"))
                            d.visible = true;
                        if (d.IsField("STATUS"))
                            d.visible = true;
                        if (d.IsField("ISOVERIN"))
                            d.visible = true;
                        if (d.IsField("IsDisable"))
                            d.visible = true;
                        if (d.IsField("Remark"))
                            d.visible = true;
                        if (d.IsField("CommodityType"))
                            d.visible = true;
                    }
                    return;
                }
                if (this.fdata.filedname == "WAREHOUSENAME")
                {
                    foreach (ItemData d in data.modeldata.childitem)
                    {//序号、仓库编码Code、名称WAREHOUSENAME、类型WHTYPE、上级PARENTID、是否禁用IsDisable、备注Remark
                        d.visible = false;
                        if (d.IsField("ROWINDEX"))
                        {
                            d.visible = true;
                        }
                        if (d.IsField("Code"))
                        {
                            d.visible = true;
                            d.title = "仓库编码";
                        }
                        if (d.IsField("WAREHOUSENAME"))
                            d.visible = true;
                        if (d.IsField("WHTYPE"))
                            d.visible = true;
                        if (d.IsField("PARENTID"))
                            d.visible = true;
                        if (d.IsField("IsDisable"))
                            d.visible = true;
                        if (d.IsField("Remark"))
                            d.visible = true;
                    }
                    return;
                }
            }
        }

        /// <summary>
        /// 合并简写方法
        /// </summary>
        /// <param name="item"></param>
        private static void Merger(ControllerBase.SaveEvent item)
        {
            CheckOrder son = item.Item as CheckOrder;
            var result = from o in son.CheckOrderMaterials
                group o by new {o.NEWMCODE, o.PBATCHNO, o.PDEPOTWBS, o.NEWFMODEL}
                into g
                select new {g.Key, Totle = g.Sum(p => p.PNUM), Items = g.ToList<CheckOrderMaterials>()};
            foreach (var group in result)
            {
                CheckOrderMaterials m =
                    group.Items.Find(
                        delegate(CheckOrderMaterials mm) { return ((IEntityBase) mm).StateBase == EntityState.Select; });
                if (m == null)
                    m = group.Items[0];
                m.PNUM = group.Totle;
                List<CheckOrderMaterials> removelist =
                    group.Items.FindAll(delegate(CheckOrderMaterials mm) { return mm != m; });
                removelist.ForEach(delegate(CheckOrderMaterials mm)
                {
                    son.CheckOrderMaterials.Remove(mm);
                });
            }
        }

        /// <summary>
        /// 验证数据正确性
        /// </summary>
        /// <param name="item"></param>
        private void ValidateItem(ControllerBase.SaveEvent item)
        {
            CheckOrder bc = item.Item as CheckOrder;
            EntityList<Materials> ms = new EntityList<Materials>(this.model.GetDataAction());
            EntityList<CheckOrder> co = new EntityList<CheckOrder>(this.model.GetDataAction());
            if (bc.ID != 0)
            {
                co.GetData("ORDERNAME = '" + bc.ORDERNAME + "' and id<>" + bc.ID + "");
            }
            else
                co.GetData("ORDERNAME='" + bc.ORDERNAME + "'");
            if (co.Count > 0)
            {
                throw new Exception("异常：盘点名称重复");
            }
            //foreach (var i in bc.CheckOrderMaterials)
            //{
            //    ms.GetData("id='" + i.MATERIALCODE + "'");

            //    if (bc.WAREHOUSENAME != null) ///盘点位置不为空
            //    {
            //        WareHouse wh = i.GetForeignObject<WareHouse>(this.model.GetDataAction());
            //        if (wh.PARENTID.ToString() != bc.WAREHOUSENAME)
            //        {
            //            throw new Exception("异常：“" + ms[0].FNAME + "”存储位置与单据不匹配");
            //        }
            //    }
            //    if (i.PNUM < 0)
            //    {
            //        throw new Exception("异常：“" + ms[0].FNAME + "”数量出现负数");
            //    }
            //    if (bc.ClassID > 0)
            //    {
            //        if (ms[0].ClassID != bc.ClassID)
            //        {
            //            throw new Exception("异常：“" + ms[0].FNAME + "”所属大类与单据不匹配");
            //        }
            //    }
            //    if (bc.MATERIALCODE != null && bc.MATERIALCODE != string.Empty)
            //    {
            //        if (i.Code != bc.MATERIALCODE)
            //        {
            //            throw new Exception("异常：“" + ms[0].FNAME + "”与单据不匹配");
            //        }
            //    }
            //}

        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="info"></param>
        protected override void OnSubmitData(Business.Model.BasicInfo info)
        {
            base.OnSubmitData(info);
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="info"></param>
        protected override void OnReviewedData(Business.Model.BasicInfo info)
        {
            CheckOrder co = base.getinfo(this.ActionItem["ID"].ToString()) as CheckOrder;
            co.CheckOrderMaterials.DataAction = this.model.GetDataAction();
            co.CheckOrderMaterials.GetData();
            if(co.CheckOrderMaterials.Count==0)
            {
                throw new Exception("异常：“盘点明细不能为空！");
            }
            base.OnReviewedData(info);

        }

        #region

        //[ActionCommand(name = "生成盘点差异", title = "生成盘点差异表", index = 6, isalert = true, icon = "icon-search")]
        //public void SetViewRDLC()
        //{
        //    BasicInfo iv = this.ActionItem as BasicInfo;
        //    EntityList<CheckOrder> coList = new EntityList<CheckOrder>(this.model.GetDataAction());
        //    coList.GetData("id='" + iv.ID + "'");
        //    foreach (var item in coList)
        //    {
        //        if (!item.IsSubmited)
        //        {
        //            throw new Exception("异常：单据未提交，不能生成盘点差异！");
        //        }
        //        if (!item.IsReviewed)
        //        {
        //            throw new Exception("异常：单据未审核，不能生成盘点差异！");
        //        }
        //    }


        //    ///盘点单及明细行
        //    CheckOrder bc = this.ActionItem as CheckOrder;
        //    bc.CheckOrderMaterials.DataAction = this.model.GetDataAction();
        //    bc.CheckOrderMaterials.GetData();

        //    EntityList<StockInfoMaterials> infoList = new EntityList<StockInfoMaterials>(this.model.GetDataAction());
        //    ///创建盘点差异单据
        //    EntityList<Difference> dfList = new EntityList<Difference>(this.model.GetDataAction());
        //    Difference dif = new Difference();
        //    dif.SourceID = bc.ID.ToString();
        //    dif.SourceController = this.OnControllerName();
        //    dif.SourceCode = bc.Code;
        //    dif.Code = new BillNumberController().GetBillNo(this);
        //    dif.Createdby = this.user.ID;
        //    dif.Creationdate = DateTime.Now;
        //    dfList.Add(dif);
        //    for (int i = 0; i < bc.CheckOrderMaterials.Count; i++)
        //    {
        //        var temp = bc.CheckOrderMaterials[i];
        //        ///根据盘点明细比对库存
        //        infoList.GetData("Code='" + temp.MATERIALCODE + "' and DEPOTWBS='" + temp.PDEPOTWBS + "' and BATCHNO='" +
        //                         temp.PBATCHNO + "' ");

        //        dfList.Save();
        //        DifferenceMaterials dm = new DifferenceMaterials();
        //        ///设置盘点差异表，盘点表字段值
        //        // dm.PARENTID = bc.ID;
        //        dm.MATERIALCODE = temp.MATERIALCODE;
        //        dm.NEWMCODE = temp.NEWMCODE;
        //        dm.PBATCHNO = temp.PBATCHNO;
        //        dm.PNUM = temp.PNUM;
        //        dm.PDEPOTWBS = temp.PDEPOTWBS;
        //        if (infoList.Count > 0)
        //        {
        //            ///设置盘点差异表，库存表字段值
        //            dm.KMATERIALCODE = infoList[0].Code;
        //            dm.KMCODE = infoList[0].MCODE;
        //            dm.KBATCHNO = infoList[0].BATCHNO;
        //            dm.KPDEPOTWBS = infoList[0].DEPOTWBS;
        //            dm.KNUM = infoList[0].NUM;
        //        }
        //        else
        //        {
        //            dm.KMATERIALCODE = "";
        //            dm.KMCODE = "";
        //            dm.KBATCHNO = "";
        //            dm.KPDEPOTWBS = 0;
        //            dm.KNUM = 0;
        //            dm.KPDEPOTWBS = 0;
        //        }
        //        ///设置差异和调整数量
        //        dm.CNUM = dm.PNUM - dm.KNUM;
        //        dif.Materials.Add(dm);
        //        dfList.Save();

        //    }
        //}
        [ActionCommand(name = "生成盘点差异", title = "生成盘点差异表", index = 6, isalert = true, icon = "icon-search")]
        public void SetViewRDLC()
        {
            CheckOrder iv = base.getinfo(this.ActionItem["ID"].ToString()) as CheckOrder;
            EntityList<CheckOrder> coList = new EntityList<CheckOrder>(this.model.GetDataAction());
            coList.GetData("id='" + iv.ID + "'");

                if (!iv.IsSubmited)
                {
                    throw new Exception("异常：单据未提交，不能生成盘点差异！");
                }
                if (!iv.IsReviewed)
                {
                    throw new Exception("异常：单据未审核，不能生成盘点差异！");
                }
                EntityList<Difference> dfList1 = new EntityList<Difference>(this.model.GetDataAction());
                dfList1.GetData("SourceCode='"+iv.Code+"'");
                if (dfList1.Count > 0)
                {
                    throw new Exception("异常：已生成完成盘点差异，不能重复生成盘点差异！");
                }


            iv.CheckOrderMaterials.DataAction = this.model.GetDataAction();
            iv.CheckOrderMaterials.GetData();
            StockInfoMaterials sim = new StockInfoMaterials();
            //EntityList<StockInfoMaterials> infoList = new EntityList<StockInfoMaterials>(this.model.GetDataAction());
            DataTable infoList = null;
            string infoListSql = "select * from "+sim.ToString()+" where 1=1 ";
            ///创建盘点差异单据
            EntityList<Difference> dfList = new EntityList<Difference>(this.model.GetDataAction());

            Difference dif = new Difference();
            //dif.TOWHNO = iv.WAREHOUSENAME;
            dif.SourceID = iv.ID.ToString();
            dif.SourceController = this.ToString();
            dif.SourceCode = iv.Code;
            dif.Code = new BillNumberController().GetBillNo(this);
            dif.Createdby = this.user.ID;
            dif.Creationdate = DateTime.Now;
            dfList.Add(dif);
            for (int i = 0; i < iv.CheckOrderMaterials.Count; i++)
            {
                var temp = iv.CheckOrderMaterials[i];
                ///根据盘点明细比对库存
                ////获取库存项
                infoListSql += "and Code='" + temp.MATERIALCODE + "' and DEPOTWBS='" + temp.PDEPOTWBS + "'";
                if (temp.PBATCHNO != "")
                {
                    infoListSql += " and BATCHNO='" + temp.PBATCHNO + "'";
                }
                if (temp.PORTCODE != "")
                {
                    infoListSql += " and PORTCODE='" + temp.PORTCODE + "'";
                }

                infoList = this.model.GetDataAction().GetDataTable(infoListSql);
                ////如果是动态盘点就执行以下
                if (iv.CheckOrderType == "2")
                {
                    DataTable dtInOrder = GetInOrder(iv.Submiteddate, temp);
                    if (dtInOrder.Rows.Count > 0)
                    {
                        infoList.Rows[0]["NUM"] = Convert.ToInt32(infoList.Rows[0]["NUM"]) -
                                                  Convert.ToInt32(dtInOrder.Rows[0]["NUM"]);
                    }
                    DataTable dtOutOrder = GetOutOrder(iv.Submiteddate, temp);
                    if (dtOutOrder.Rows.Count > 0)
                    {
                        infoList.Rows[0]["NUM"] = Convert.ToInt32(infoList.Rows[0]["NUM"]) +
                                                  Convert.ToInt32(dtOutOrder.Rows[0]["NUM"]);
                    }
                }
                dfList.Save();
                DifferenceMaterials dm = new DifferenceMaterials();
                ///设置盘点差异表，盘点表字段值
                // dm.PARENTID = bc.ID;
                dm.MATERIALCODE = temp.MATERIALCODE;
                dm.NEWMCODE = temp.NEWMCODE;
                dm.PBATCHNO = temp.PBATCHNO;
                dm.PNUM = temp.PNUM;
                dm.PDEPOTWBS = temp.PDEPOTWBS;
                dm.PORTCODE = temp.PORTCODE;
                if (infoList.Rows.Count > 0)
                {
                    ///设置盘点差异表，库存表字段值
                    dm.KMATERIALCODE = infoList.Rows[0]["Code"].ToString();
                    dm.KMCODE = infoList.Rows[0]["MCODE"].ToString();
                    dm.KBATCHNO = infoList.Rows[0]["BATCHNO"].ToString();
                    dm.KPDEPOTWBS = Convert.ToInt32(infoList.Rows[0]["DEPOTWBS"]);
                    dm.KNUM = Convert.ToDouble(infoList.Rows[0]["NUM"]);
                }
                else
                {
                    dm.KMATERIALCODE = "";
                    dm.KMCODE = "";
                    dm.KBATCHNO = "";
                    dm.KPDEPOTWBS = 0;
                    dm.KNUM = 0;
                    dm.KPDEPOTWBS = 0;
                }
                ///设置差异和调整数量
                dm.CNUM = dm.PNUM - dm.KNUM;
                dif.Materials.Add(dm);
                dfList.Save();

            }
        }


        

            /// <summary>
        /// 根据盘点单提交日期查询此日期以后的所有产品的审核入库数量
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private DataTable GetInOrder(DateTime dt,CheckOrderMaterials cm)
        {
            //// --根据盘点单提交日期查询此日期以后的所有产品的审核入库数量
            string sql = "SELECT sim.MATERIALCODE,SUM(sim.NUM) AS num FROM dbo.Acc_WMS_InOrder " +
                         "si INNER join dbo.Acc_WMS_InOrderMaterials sim ON si.ID = sim.PARENTID" +
                         " WHERE si.submiteddate>'" + dt + "' and  sim.MATERIALCODE='" + cm.MATERIALCODE + "' and sim.DEPOTWBS='" + cm.PDEPOTWBS + "' " +
                         " and sim.BATCHNO='"+cm.PBATCHNO+"'" +
                         "GROUP BY sim.MATERIALCODE,sim.FMODEL,sim.MCODE,sim.DEPOTWBS,sim.BATCHNO";
            return this.model.GetDataAction().GetDataTable(sql);
        }
        ///// <summary>
        ///// 根据盘点单提交日期查询此日期以后的所有产品的审核入库数量
        ///// </summary>
        ///// <param name="dt"></param>
        ///// <returns></returns>
        //private DataTable GetInOrder(DateTime dt,CheckOrderMaterials cm)
        //{
        //    //// --根据盘点单提交日期查询此日期以后的所有产品的审核入库数量
        //    string sql = "SELECT sim.MATERIALCODE,SUM(sim.NUM) AS num FROM dbo.Acc_WMS_InOrder " +
        //                 "si INNER join dbo.Acc_WMS_InOrderMaterials sim ON si.ID = sim.PARENTID" +
        //                 " WHERE si.Revieweddate>'" + dt + "' and  sim.MATERIALCODE='" + cm.MATERIALCODE + "' and sim.DEPOTWBS='"+cm.PDEPOTWBS+"' " +
        //                 " and sim.BATCHNO='"+cm.PBATCHNO+"'" +
        //                 "GROUP BY sim.MATERIALCODE,sim.FMODEL,sim.MCODE,sim.DEPOTWBS,sim.BATCHNO";
        //    return this.model.GetDataAction().GetDataTable(sql);
        //}

        ///// <summary>
        ///// 根据盘点单提交日期查询此日期以后的所有产品的审核出库数量
        ///// </summary>
        ///// <param name="dt"></param>
        ///// <returns></returns>
        //private DataTable GetOutOrder(DateTime dt, CheckOrderMaterials cm)
        //{
            
        //   ////--根据盘点单提交日期查询此日期以后的所有产品的审核出库数量
        //    string sql = "SELECT sim.MATERIALCODE,SUM(sim.NUM) AS num FROM dbo.Acc_WMS_OutOrder " +
        //                 "si INNER join dbo.Acc_WMS_OutOrderMaterials sim ON si.ID = sim.PARENTID" +
        //                 " WHERE si.Revieweddate<'" + dt + "' and  sim.MATERIALCODE='" + cm.MATERIALCODE + "' and sim.DEPOTWBS='" + cm.PDEPOTWBS + "' " +
        //                 " and sim.BATCHNO='" + cm.PBATCHNO + "'" +
        //                 "GROUP BY sim.MATERIALCODE,sim.FMODEL,sim.MCODE,sim.DEPOTWBS,sim.BATCHNO";
        //    return this.model.GetDataAction().GetDataTable(sql);
        //}

        /// <summary>
        /// 根据盘点单提交日期查询此日期以后的所有产品的审核出库数量
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private DataTable GetOutOrder(DateTime dt, CheckOrderMaterials cm)
        {

            ////--根据盘点单提交日期查询此日期以后的所有产品的审核出库数量
            string sql = "SELECT sim.MATERIALCODE,SUM(sim.NUM) AS num FROM dbo.Acc_WMS_OutOrder " +
                         "si INNER join dbo.Acc_WMS_OutOrderMaterials sim ON si.ID = sim.PARENTID" +
                         " WHERE si.submiteddate<'" + dt + "' and  sim.MATERIALCODE='" + cm.MATERIALCODE + "' and sim.DEPOTWBS='" + cm.PDEPOTWBS + "' " +
                         " and sim.BATCHNO='" + cm.PBATCHNO + "'" +
                         "GROUP BY sim.MATERIALCODE,sim.FMODEL,sim.MCODE,sim.DEPOTWBS,sim.BATCHNO";
            return this.model.GetDataAction().GetDataTable(sql);
        }

        #endregion

        #endregion
    }
}
