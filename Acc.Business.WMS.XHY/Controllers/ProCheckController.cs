using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Business.Controllers;
using Acc.Business.WMS.XHY.Model;
using Acc.Contract.MVC;
using Acc.Contract.Center;
using Acc.Contract.Data.ControllerData;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model;
using Acc.Contract.Data;
using Acc.Business.WMS.Model;
using System.Data;

namespace Acc.Business.WMS.XHY.Controllers
{
    public class ProCheckController : BusinessController
    {
        public ProCheckController() : base(new ProCheck()) { }
        public ProCheckController(IModel model) : base(model) { }
        #region 初始化数据方法


        #region 基础设置

        /// <summary>
        /// 禁用审核
        /// </summary>
        public override bool IsReviewedState
        {
            get
            {
                return true;
            }
        }

        public override bool IsPushDown
        {
            get
            {
                return false ;
            }
        }
        //菜单中url路径
        protected override string OnGetPath()
        {
            return "Views/THDB/ProCheck.htm";
        }
        //开发人
        protected override string OnGetAuthor()
        {
            return "柳强";
        }
        //说明
        protected override string OnGetControllerDescription()
        {
            return "质检管理";
        }
        #endregion

        #region 新增 编辑
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="item"></param>
        protected override void OnAdding(ControllerBase.SaveEvent item)
        {
            base.OnAdding(item);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="item"></param>
        protected override void OnEditing(ControllerBase.SaveEvent item)
        {
            base.OnEditing(item);
        }

        protected override void OnSubmitData(BasicInfo info)
        {
            //Validate(info);
            base.OnSubmitData(info);

        }

        ///// <summary>
        ///// 验证数据
        ///// </summary>
        ///// <param name="item"></param>
        //private void Validate(BasicInfo info)
        //{
        //    ProCheck p = info as ProCheck;
        //    if (p.CheckType == null)
        //    {
        //        // throw new Exception("质检类型不能为空");
        //    }
        //    p.Materials.DataAction = this.model.GetDataAction();
        //    p.Materials.GetData();
        //    foreach (var item in p.Materials)
        //    {
        //        EntityList<Materials> ms = new EntityList<Materials>(this.model.GetDataAction());
        //        ms.GetData("id='" + item.CHECKWARE + "'");
        //        if (item.QUANUM < 0)
        //        {
        //            throw new Exception("异常：“质检产品：" + ms[0].FNAME + "  产品编码：" + ms[0].Code + "” 合格数量小于0，请输入正确数量");
        //        }
        //        if (item.QUANUM > item.CHECKNUM)
        //        {

        //            throw new Exception("异常：“质检产品：" + ms[0].FNAME + "  产品编码：" + ms[0].Code + "” 合格数量大于质检数量，请输入正确数量");
        //        }
        //    }
        //}

        protected override void OnReviewedData(BasicInfo info)
        {

            base.OnReviewedData(info);    
            //UpdateInOrderMaterialsState(info);
            
        }
        ///// <summary>
        ///// 审核完成修改入库单明细行单据质检状态
        ///// </summary>
        ///// <param name="info"></param>
        //private void UpdateInOrderMaterialsState(BasicInfo info)
        //{
        //    #region 获取质检单内容及质检明细数据
        //    ProCheck pc = info as ProCheck;
        //    pc.Materials.GetData("parentid='" + pc.ID + "'");
        //    #endregion

        //    #region 根据质检单源单id查询此质检单来源单据信息
        //    EntityList<StockInOrder> inorder = new EntityList<StockInOrder>(this.model.GetDataAction());
        //    inorder.GetData("code='" + pc.SourceCode + "'");
        //    if (inorder.Count > 0)
        //    {
        //        #region 根据入库单得到入库的库存项信息并判断每次质检数量等信息生成移位单等
        //        ///得到已经添加完的库存项，未检验的
        //        EntityList<StockInfo_Materials> simList = new EntityList<StockInfo_Materials>(this.model.GetDataAction());
        //        simList.GetData("ORDERNO='" + inorder[0].ID + "'");
        //        //得到多条入库后的库存项
        //        //如果质检单的合格数量和单据数量不匹配那么就将此库存项拆分成两条数据。有一个数据是未合格的
        //        EntityList<StockInfo_Materials> temp = new EntityList<StockInfo_Materials>(this.model.GetDataAction());
        //        if (simList.Count > 0)
        //        {
        //            /////定义以为单主表数据，如果质检中有不合格产品将保存此移位单
        //            //EntityList<MoveOrder> list = new EntityList<MoveOrder>(this.model.GetDataAction());
        //            //EntityList<MoveOrderMaterials> list1 = new EntityList<MoveOrderMaterials>(this.model.GetDataAction());
        //            //MoveOrder mo = new MoveOrder();
        //            //mo.DEPOTWBS = Convert.ToInt32(simList[0].DEPOTWBS);
        //            //mo.SourceCode = pc.Code;
        //            //mo.SourceController = this.ToString();
        //            //mo.SourceID = pc.ID;
        //            //BillNumberController bnc = new BillNumberController();
        //            //mo.Code = bnc.GetBillNo(bnc);
        //            //list.Add(mo);
        //            ///如果质检不合格生成新库存项*（不合格）
        //            EntityList<StockInfo_Materials> newInfoList = new EntityList<StockInfo_Materials>(this.model.GetDataAction());


        //            for (int i = 0; i < simList.Count; i++)
        //            {
        //                for (int j = 0; j < pc.Materials.Count; j++)
        //                {
        //                    if (simList[i].Code.Equals(pc.Materials[j].CHECKWARE))
        //                    {
        //                        temp.GetData("status='2' and code='" + pc.Materials[j].CHECKWARE + "' and depotwbs='" + simList[i].DEPOTWBS + "' and batchno='" + simList[i].BATCHNO + "'");
        //                        if (temp.Count > 0)
        //                        {
        //                            string sql =
        //                            string.Format("update Acc_wms_StockInfo_Materials set num='{0}',status=2" +
        //                            " where ORDERNO='{1}' and code='{2}'",
        //                            pc.Materials[j].QUANUM + temp[0].NUM, inorder[0].ID, pc.Materials[j].CHECKWARE);
        //                            this.model.GetDataAction().Execute(sql);
        //                            string sql1 = "delete from Acc_wms_StockInfo_Materials where id=" + temp[0].ID;
        //                            this.model.GetDataAction().Execute(sql1);
        //                            //if (temp[0].NUM > pc.Materials[j].QUANUM)
        //                            //{
        //                            //    list.Save();
        //                            //    MoveOrderMaterials som = new MoveOrderMaterials();
        //                            //    som.PID = list[0].ID;
        //                            //    som.SourceCode = pc.Code;
        //                            //    som.SourceID = pc.ID;
        //                            //    som.SourceController = this.ToString();
        //                            //    som.NEWMCODE = temp[0].MCODE; ///物料编码
        //                            //    som.NEWCODE = temp[0].ID.ToString(); ///库存项
        //                            //    som.NEWMNAME = pc.Materials[j].CHECKWARE.ToString(); ///质检单的产品名称
        //                            //    som.NEWFMODEL = temp[0].FMODEL; ///规格型号
        //                            //    som.BATCHNO = temp[0].BATCHNO; ///库存批次
        //                            //    som.NUM = Convert.ToDouble(temp[0].NUM - pc.Materials[j].QUANUM); //库存数量
        //                            //    som.DEPOTWBS = temp[0].DEPOTWBS; ///存储位置
        //                            //    mo.Materials.Add(som);
        //                            //    list.Save();
        //                            //}
        //                        }
        //                        else
        //                        {
        //                            string sql =
        //                            string.Format("update Acc_wms_StockInfo_Materials set status='{0}', num='{1}'" +
        //                            " where ORDERNOID='{2}' and code='{3}'",
        //                              2, pc.Materials[j].QUANUM, inorder[0].ID, pc.Materials[j].CHECKWARE);
        //                            this.model.GetDataAction().Execute(sql);
        //                        }

        //                        #region 不合格产品添加新库存
        //                        if (pc.Materials[j].CHECKNUM > pc.Materials[j].QUANUM)
        //                        {
        //                            StockInfo_Materials sim = new StockInfo_Materials();
        //                            sim.Code = pc.Materials[j].CHECKWARE;
        //                            sim.MCODE = pc.Materials[j].MCODE;
        //                            sim.FMODEL = pc.Materials[j].FMODEL;
        //                            sim.PORTCODE = simList[i].PORTCODE;
        //                            sim.ISLOCK = false;
        //                            sim.ORDERNO = inorder[0].ID.ToString();
        //                            sim.STATUS = "3";
        //                            sim.BATCHNO = pc.Materials[j].BATCHNO;
        //                            sim.DEPOTWBS = simList[i].DEPOTWBS;
        //                            sim.NUM = pc.Materials[j].CHECKNUM - pc.Materials[j].QUANUM;

        //                            sim.LASTINTIME = DateTime.Now;
        //                            newInfoList.Add(sim);
        //                            newInfoList.Save();
        //                            #region 添加移位单明细（不合格产品）
        //                            if (pc.Materials[j].CHECKNUM > pc.Materials[j].QUANUM)
        //                            {
        //                                //list.Save();
        //                                //MoveOrderMaterials som = new MoveOrderMaterials();
        //                                //som.PID = list[0].ID;
        //                                //som.SourceCode = pc.Code;
        //                                //som.SourceID = pc.ID;
        //                                //som.SourceController = this.ToString();
        //                                //som.NEWMCODE = sim.MCODE; ///物料编码
        //                                //som.NEWCODE = sim.ID.ToString(); ///库存项
        //                                //som.NEWMNAME = pc.Materials[j].CHECKWARE.ToString(); ///质检单的产品名称
        //                                //som.NEWFMODEL = sim.FMODEL; ///规格型号
        //                                //som.BATCHNO = sim.BATCHNO; ///库存批次
        //                                //som.NUM = Convert.ToDouble(pc.Materials[j].CHECKNUM - pc.Materials[j].QUANUM); //库存数量
        //                                //som.DEPOTWBS = sim.DEPOTWBS; ///存储位置
        //                                //som.LASTINTIME = sim.Creationdate;
        //                                //mo.Materials.Add(som);
        //                                //list.Save();
        //                            }
        //                            #endregion
        //                        #endregion

        //                        }
        //                    }
        //                }

        //            }

        //        }
        //        #endregion
        //    }
        //    #endregion
        //}

        /// <summary>
        /// 添加到按钮（设置为合格）
        /// </summary>
        [ActionCommand(name = "设置为合格", title = "设置单据内所有物料为合格状态(除免检验)", index = 9, icon = "icon-search", isalert = true)]
        public void TestData()
        {
            ProCheck pc = this.ActionItem as ProCheck;
            if (pc.IsSubmited == false && pc.IsReviewed == false)
            {
                this.model.GetDataAction().Execute("update Acc_WMS_ProCheck set isok = 1 where id='" + pc.ID + "'");

            }
        }

        /// <summary>
        /// 添加到按钮（设置为不合格）
        /// </summary>
        [ActionCommand(name = "设置为不合格", title = "设置单据内所有物料(除免检验)为不合格状态", index = 10, icon = "icon-search", isalert = true)]
        public void TestData1()
        {
            ProCheck pc = this.ActionItem as ProCheck;
            if (pc.IsSubmited == false && pc.IsReviewed == false)
            {
                    this.model.GetDataAction().Execute("update Acc_WMS_ProCheck set isok = 0 where id='" + pc.ID + "'");
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="item"></param>
        protected override void OnInitViewChildItem(Contract.Data.ModelData data, Contract.Data.ItemData item)
        {
            base.OnInitViewChildItem(data, item);
            if (data.name.EndsWith("ProCheck"))
            {

                switch (item.field.ToLower())
                {
                    case "code":
                        item.title = "编码";
                        item.disabled = true;
                        break;
                    case "clientno":
                    case "onrevieweddata":
                        item.visible = false;
                        break;
                    case "sourcecode":
                        item.visible = true;
                        item.disabled = true;
                        break;
                    case "modifiedby":
                    case "modifieddate":
                    case "isdisable":
                    case "isdelete":
                        item.visible = false;
                        break;
                    case "reviewedby":
                    case "revieweddate":
                    case "issubmited":
                    case "isreviewed":
                    case "submiteddate":
                    case "submitedby":
                    case "createdby":
                    case "creationdate":
                        item.disabled = true;
                        break;

                }
            }
            if (data.name.EndsWith("ProCheckMaterials"))
            {
                switch (item.field.ToLower())
                {
                    case "sourcecode":
                        item.visible = false;
                        item.disabled = true;
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
                    case "isdisable":
                    case "isdelete":
                    case "parentid":
                    case "code":
                        item.disabled = false;
                        item.visible = false;
                        break;

                }
            }
        }
        #endregion


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
                if (ac.command == "UnReviewedData")
                {
                    ac.visible = false;
                }
            }
            return coms;
        }

        ///// <summary>
        ///// 定义监控的类和方法（观察者模式）
        ///// </summary>
        ///// <returns></returns>
        //protected override ObserverClient[] OnGetObserverClient()
        //{
        //    List<ObserverClient> list = new List<ObserverClient>();
        //    ObserverClient o = new ObserverClient();
        //    o.ControllerName = "Acc.Business.WMS.XHY.Controllers.OtherInOrderController";
        //    o.MethodName = "ReviewedData";
        //    o.ObserverType = CustomeEventType.Complete;
        //    o.IsAsynchronous = true;
        //    o.SourceController = this;

        //    ObserverClient o1 = new ObserverClient();
        //    o1.ControllerName = "Acc.Business.WMS.XHY.Controllers.ProduceInOrderController";
        //    o1.MethodName = "ReviewedData";
        //    o1.ObserverType = CustomeEventType.Complete;
        //    o1.IsAsynchronous = true;
        //    o1.SourceController = this;

        //    ObserverClient o2 = new ObserverClient();
        //    o2.ControllerName = "Acc.Business.WMS.XHY.Controllers.PurchaseInOrderController";
        //    o2.MethodName = "ReviewedData";
        //    o2.ObserverType = CustomeEventType.Complete;
        //    o2.IsAsynchronous = true;
        //    o2.SourceController = this;

        //    ObserverClient o3 = new ObserverClient();
        //    o3.ControllerName = "Acc.Business.WMS.XHY.Controllers.ReturnInOrderController";
        //    o3.MethodName = "ReviewedData";
        //    o3.ObserverType = CustomeEventType.Complete;
        //    o3.IsAsynchronous = true;
        //    o3.SourceController = this;
        //    list.Add(o);
        //    list.Add(o1);
        //    list.Add(o2);
        //    list.Add(o3);
        //    return list.ToArray();
        //}

        //protected override void InceptNotify(EventBase item)
        //{
        //    OnObserverProduction(item);
        //}

        ///// <summary>
        ///// 观察入库单(根据原单明细行物料大类分别创建质检单)
        ///// </summary>
        //protected virtual void OnObserverProduction(EventBase item)
        //{

        //    string name = item.Controller.GetType().FullName;
        //    ///入库单
        //    if (name != null)
        //    {
        //        string title = ((IController)item.Controller).ControllerName();
        //        if (item.MethodName == "ReviewedData" && item.EventType == CustomeEventType.Complete)
        //        {
        //            DataTable dtCount = this.model.GetDataAction().GetDataTable("select count(bm.classid),bm.classid from Acc_WMS_InOrderMaterials sm inner join acc_bus_businesscommodity bm on sm.materialcode=bm.id  where sm.parentid = '" + item.CustomeData["id"] + "' group by bm.classid");
        //            for (int i = 0; i < dtCount.Rows.Count; i++)
        //            {
        //                EntityList<ProCheck> list = new EntityList<ProCheck>(this.model.GetDataAction());
        //                ProCheck so = new ProCheck();
        //                BillNumberController bnc = new BillNumberController();
        //                ProCheckController bc = new ProCheckController();
        //                so.Code = bnc.GetBillNo(bc);
        //                so.Createdby = item.user.ID;
        //                so.Creationdate = DateTime.Now;
        //                so.SourceController = name;
        //                so.SourceID = Convert.ToInt32(item.CustomeData["id"]);
        //                so.SourceCode = item.CustomeData["Code"].ToString();
        //                so.Remark = "来源入库单";
        //                list.Add(so);
        //                list.Save();
        //                ///添加质检单子集
        //                DataTable dt = this.model.GetDataAction().GetDataTable("select * from Acc_WMS_InOrderMaterials sm inner join acc_bus_businesscommodity ab on ab.id= sm.materialcode where sm.parentid='" + item.CustomeData["id"] + "' and ab.classid='" + dtCount.Rows[i]["classId"] + "'");
        //                foreach (DataRow e in dt.Rows)
        //                {
        //                    ProCheckMaterials sm = new ProCheckMaterials();
        //                    sm.SourceCode = so.Code;
        //                    sm.SourceController = this.ToString();
        //                    sm.SourceID = so.ID;
        //                    sm.SourceRowID = Convert.ToInt32(e["ID"]);
        //                    sm.SourceTable = e.ToString();
        //                    sm.CHECKWARE = e["MATERIALCODE"].ToString();
        //                    sm.FMODEL = e["FMODEL"].ToString();
        //                    sm.MCODE = e["MCODE"].ToString();
        //                    sm.BATCHNO = e["BATCHNO"].ToString();
        //                    sm.CHECKNUM = Convert.ToDouble(e["NUM"]);
        //                    sm.CHECKTIME = DateTime.Now;
        //                    EntityList<Materials> ms = new EntityList<Materials>(this.model.GetDataAction());
        //                    ms.GetData("id='" + sm.CHECKWARE + "'");

        //                    so.Materials.Add(sm);
        //                    list.Save();
        //                }
        //                ///清空list重新添加
        //                list.Clear();

        //            }

        //        }

        //    }
        //    //if (list.Count > 0)
        //    //{
        //    //    //base.OnSubmitData(so);
        //    //    //base.OnReviewedData(so);
        //    //}
        //}

        #endregion


    }
}
