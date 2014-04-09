using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Business.Controllers;
using Acc.Business.WMS.Model;
using Way.EAP.DataAccess.Data;
using System.Data;
using Way.EAP.DataAccess.Entity;
using Acc.Contract.Data;
using Acc.Contract.Data.ControllerData;
using Acc.Contract.MVC;

namespace Acc.Business.WMS.Controllers
{
    public class StockInfoMaterialsController : BusinessController
    {
        /// <summary>
        /// 库存产品管理控制器
        /// 创建人：柳强
        /// 创建时间：2012-01-09
        /// </summary>
        public StockInfoMaterialsController() : base(new StockInfoMaterials()) { }
        public StockInfoMaterialsController(IModel model) : base(model) { }

        public override bool IsReviewedState
        {
            get
            {
                return false;
            }
        }

  
        protected override string OnControllerName()
        {
            return "库存产品管理";
        }

        //菜单中url路径
        protected override string OnGetPath()
        {
            return "Views/WMS/Materials/StockInfoMaterials.htm";
        }   
        //开发人
        protected override string OnGetAuthor()
        {
            return "柳强";
        }
        //说明
        protected override string OnGetControllerDescription()
        {
            return "库存产品管理";
        }

        #region 初始化数据方法
        protected override void OnInitViewChildItem(Contract.Data.ModelData data, Contract.Data.ItemData item)
        {
            if (data.name.EndsWith("StockInfoMaterials"))
            {
                switch (item.field.ToLower())
                {
                    case "Code":
                        item.visible = true;
                        break;
                }
            }
            if (data.name.EndsWith("InfoInSequence"))
            {
                data.isadd = false;
                data.isedit = false;
                data.isremove = false;
                switch (item.field.ToLower())
                {
                    case "sequencecode":
                    case "rowindex":
                    item.visible = true;
                        break;
                    default:
                        item.visible = false;
                        break;
                }
            }
        }

        #endregion

        protected override void OnEditing(Contract.MVC.ControllerBase.SaveEvent item)
        {
            base.OnEditing(item);
            StockInfoMaterials info = item.Item as StockInfoMaterials;
            if (info.FREEZENUM >= 0 && info.FREEZENUM <= Convert.ToDouble(info.NUM))
            {
                //info.NUM -= info.FREEZENUM;
                //info.ISLOCK = true;
            }
            else
            {
                throw new Exception("请输入正确的冻结数量");
            }
        }

        protected override void OnReviewedData(Business.Model.BasicInfo info)
        {
            base.OnReviewedData(info);
        }

        protected override ActionCommand[] OnInitCommand(ActionCommand[] commands)
        {
            ActionCommand[] cmd = base.OnInitCommand(commands);
            //获取所有按钮集合
            foreach (ActionCommand ac in cmd)
            {
                if (ac.command == "add")
                {
                    ac.visible = false;
                }
                if (ac.command == "edit")
                {
                    ac.visible = false;
                }
                if (ac.command == "remove")
                {
                    ac.visible = false;
                }
                if (ac.command == "SubmitData")
                {
                    ac.visible = false;
                }
                if (ac.command == "OnSubmitData")
                {
                    ac.visible = false;
                }
                if (ac.command == "ReviewedData")
                {
                    ac.visible = false;
                }
            }
            return cmd;
        }


        /// <summary>
        /// 库存表code(sm.code也就是产品ID)
        /// </summary>
        [WhereParameter]
        public string SmCode { get; set; }
        /// 库存表depotwbs(sm.depotwbs也就是货位的ID)
        [WhereParameter]
        public string SmDepotwbs { get; set; }
        /// 库存表portcode(sm.portcode（也就是托盘的id)
        [WhereParameter]
        public string SmPortcode { get; set; }
        /// 库存表batchno(sm.batchno)
        [WhereParameter]
        public string SmBatchno { get; set; }
        /// 库存表senbatchno(sm.senbatchno)
        [WhereParameter]
        public string SmSenbatchno { get; set; }
        /// 库存表Num(sm.Num)
        [WhereParameter]
        public string SmNum { get; set; }
        /// controller(0:入库，1：出库)
        [WhereParameter]
        public string controller { get; set; }
        /// OrderId
        [WhereParameter]
        public string OrderId { get; set; }

        /// <summary>
        /// 单个入库
        /// </summary>
        /// <returns></returns>
        //[ActionCommand(name = "入库", title = "入库", index = 10, icon = "icon-ok", isalert = true)]
        public string OnePutin()
        {
            string returnStr = CheckPutInData(controller);
            if (returnStr != "Y")
                return returnStr;
            if (controller == "0")
                controller = "入库";
            if (controller == "1")
                controller = "出库";
            StockInfoMaterials stockinfo = new StockInfoMaterials();
            stockinfo.Code = SmCode;
            //stockinfo.BATCHNO = SmBatchno;
            //stockinfo.SENBATCHNO = SmSenbatchno;
            stockinfo.DEPOTWBS = SmDepotwbs.ToString();
            stockinfo.PORTCODE = SmPortcode;
            stockinfo.NUM = Convert.ToDouble(SmNum);
            EntityList<Materials> list1 = new EntityList<Materials>(this.model.GetDataAction());
            list1.GetData(" ID='" + SmCode + "'");
            putin(stockinfo,controller,this.model.GetDataAction());
            SmCode = "";
            SmDepotwbs = "";
            SmPortcode = "";
            SmBatchno = "";
            SmSenbatchno = "";
            SmNum = "";
            controller = "";
            return "Y";
        }
        protected Materials ValidateStockMaterial(StockInfoMaterials sm, IDataAction acion)
        {
            
            Materials mmm = sm.GetForeignObject<Materials>(acion);
            if (mmm.BATCH)
            {
                if (string.IsNullOrEmpty(sm.BATCHNO))
                    throw new Exception("异常：物料" + mmm.FNAME + "为批管，批次号不能为空！");
            }
            else
                sm.BATCHNO = "";
            if (mmm.SEQUENCECODE)
            {
                //StockInfoMaterials sim = sm;
                if (sm.STAY5 =="qt")
                {
                    //代表是其他出库
                    if (sm.StockSequence.Count != sm.NUM && sm.StockSequence.Count>0)
                        throw new Exception("异常：物料" + mmm.FNAME + "为序列码管理，序列码成员数量" + sm.StockSequence.Count + "与行数量" + sm.NUM + "不匹配！如要出库请添加匹配数量的单件码，或删除该物料"+sm.MCODE+"下对应的所有单件码");
                }
                else
                {
                    if (sm.StockSequence.Count <= 0)
                        throw new Exception("异常：物料" + mmm.FNAME + "为序列码管理，序列码不能为空！");
                   
                }
                
            }
            return mmm;
        }
        protected WareHouse ValidateWareHouse(StockInfoMaterials sm, IDataAction acion)
        {
            WareHouse wh = null;
            EntityList<WareHouse> whlist = new EntityList<WareHouse>(acion);
            whlist.GetData("ID=" + sm.WAREHOUSEID);
            if (whlist.Count == 0)
                throw new Exception("异常：指定的仓库不存在！");
            wh = whlist[0];
            //if (!string.IsNullOrEmpty(sm.DEPOTWBS))
            //{
            //    wh.ChildItems.GetData("ID='" + sm.DEPOTWBS + "'");
            //    if (wh.ChildItems.Count <= 0)
            //        throw new Exception("异常：指定的存储位置" + sm.DEPOTWBS + "在" + wh.WAREHOUSENAME + "中不存在！");
            //}
            return wh;
        }

        public void putin(StockInfoMaterials sm, string controller, IDataAction acion, ControllerBase cbname)
        {
            UpdateInfoMaterials(sm, controller, acion, cbname);
        }
        /// <summary>
        /// 改变库存数量（重载）
        /// </summary>
        /// <param name="materialcode"></param>
        /// <param name="depot"></param>
        /// <param name="portno"></param>
        /// <param name="num"></param>
        public void putin(StockInfoMaterials sm, string controller,IDataAction acion)
        {
            UpdateInfoMaterials(sm, controller, acion,null);
            #region 不要
            ////string whereSql ="Code='" + sm.Code + "' and DEPOTWBS='" + sm.DEPOTWBS + "' and PORTCODE='"+sm.PORTCODE+" '";
            //string whereSql = "Code='" + sm.Code + "' and PORTCODE='" + sm.PORTCODE + " '";
            //whereSql += " and depotwbs in(select id from acc_wms_warehouse where type='" + sm.DEPOTWBS + "' OR id='" + sm.DEPOTWBS + "')";
            //if (sm.BATCHNO == null)
            //{
            //    whereSql += " and BATCHNO is null";
            //}
            //else if (sm.BATCHNO == "")
            //{
            //    whereSql += " and BATCHNO =''";
            //}
            //else
            //{
            //    whereSql += " and BATCHNO ='" + sm.BATCHNO + "'";
            //}
            //if (sm.SENBATCHNO == null)
            //{
            //    whereSql += " and SENBATCHNO is null";
            //}
            //else if (sm.SENBATCHNO == "")
            //{
            //    whereSql += " and SENBATCHNO =''";
            //}
            //else
            //{
            //    whereSql += " and SENBATCHNO ='" + sm.SENBATCHNO + "'";
            //}
            //EntityList<StockInfoMaterials> list1 = new EntityList<StockInfoMaterials>(this.model.GetDataAction());
            //list1.GetData(whereSql);
            //if (controller == "出库")
            //{
            //    ////添加验证，如果启用质检那么出库时将判断产品库存状态是否符合出库
            //    if (m.ISCHECKPRO == true)
            //    {
            //        whereSql += " and ([STATUS]!='3' or [STATUS]!='1')";
            //    }
            //    ///如果有库存记录
            //    if (list1.Count > 0)
            //    {
            //        foreach (StockInfoMaterials item in list1)
            //        {
            //            if (!m.NUMSTATE)
            //            {
            //                if (item.NUM - (-sm.NUM) < 0)
            //                {
            //                    throw new Exception("商品编码：" + m.Code + ";    商品名称：" + m.FNAME + ";    出库数量大于库存数量" + item.NUM + "且产品不允许负库存");
            //                }
            //            }
            //            item.NUM = item.NUM - (-sm.NUM);
            //        }
            //    }
            //    else
            //    {
            //        if (m.NUMSTATE == true)
            //        {
            //            StockInfoMaterials sim = NewStockInfoMaterials(sm, m);
            //            list1.Add(sim);
            //        }
            //        else
            //        {
            //            throw new Exception("商品编码：" + m.Code + "    商品名称：" + m.FNAME + "    出库数量大于库存数量且产品不允许负库存");
            //        }
            //    }
            //    //更改托盘和货位状态
            //    if (list1.Count > 0)
            //    {
            //        foreach (StockInfoMaterials item in list1)
            //        {
            //            UpdatePortState(item);
            //            UpdateWarehouseState(item);
            //        }
            //    }
            //    StockInfoMaterials sims = new StockInfoMaterials();
            //    ///如果此入库信息所属仓库不允许0库存记录，那么每次都执行删除记录
            //    EntityList<WareHouse> wh = new EntityList<WareHouse>(this.model.GetDataAction());
            //    wh.GetData("id='" + sm.DEPOTWBS + "'");
            //    if (wh.Count > 0)
            //    {
            //        if (wh[0].ISDELSTATUS == false)
            //        {
            //            foreach (var item in list1)
            //            {
            //                if (item.NUM == 0)
            //                {
            //                    string sql = "delete from " + sims.ToString() + " where id='" + item.ID + "'";
            //                    IDataAction action = this.model.GetDataAction();
            //                    action.Execute(sql);
            //                }
            //            }
            //        }
            //    }
            //}
            //else if (controller == "入库")
            //{
            //    ////入库时添加状态条件
            //    if (m.ISCHECKPRO == true)
            //    {
            //        whereSql += " and STATUS='" + sm.STATUS + "'";
            //    }
            //    ////库存信息有此库存在数量上累加
            //    if (list1.Count > 0)
            //    {
            //        foreach (StockInfoMaterials item in list1)
            //        {
            //            sm.ID = item.ID;
            //            item.NUM = item.NUM + sm.NUM;

            //        }
            //    }
            //    ////没有此库存创建一个新的记录
            //    else
            //    {
            //        StockInfoMaterials sim = NewStockInfoMaterials(sm, m);
            //        list1.Add(sim);
            //    }
            //    if (list1.Count > 0)
            //    {
            //        foreach (StockInfoMaterials item in list1)
            //        {
            //            UpdatePortState(item);
            //            UpdateWarehouseState(item);
            //        }
            //    }
            //}

            //list1.Save();

            #endregion
        }

        private void UpdateInfoMaterials(StockInfoMaterials sm, string controller, IDataAction acion, ControllerBase cbname)
        {
            string stay5 = sm.STAY5;
            if (sm == null)
                throw new Exception("异常：库存实体不能为空！");
            if (string.IsNullOrEmpty(sm.Code))
                throw new Exception("异常：物料ID不能为空！");
            if (sm.WAREHOUSEID <= 0)
                throw new Exception("异常：仓库ID不能为空！");
            Materials mmm = ValidateStockMaterial(sm, acion);
            WareHouse stock = ValidateWareHouse(sm, acion);
            string w = "code='" + sm.Code + "' and WAREHOUSEID='" + sm.WAREHOUSEID + "'";
            if (!string.IsNullOrEmpty(sm.BATCHNO))
                w += " and BATCHNO='" + sm.BATCHNO + "'";
            if (!string.IsNullOrEmpty(sm.DEPOTWBS))
                w += " and DEPOTWBS='" + sm.DEPOTWBS + "'";
            if (!string.IsNullOrEmpty(sm.PORTCODE) && sm.PORTCODE != "0")
                w += " and PORTCODE='" + sm.PORTCODE + "'";
            if (!string.IsNullOrEmpty(sm.SENBATCHNO))
                w += " and SENBATCHNO='" + sm.SENBATCHNO + "'";
            EntityList<StockInfoMaterials> list = new EntityList<StockInfoMaterials>(acion);
            list.GetData(w);
            InfoInSequence seq = new InfoInSequence();
            string seqtn = seq.ToString();
            string hw = "";
            if (!string.IsNullOrEmpty(sm.DEPOTWBS))
                hw = "货位：" + sm[sm.GetAppendPropertyKey("DEPOTWBS")].ToString();
            string tp = "";
            if (!string.IsNullOrEmpty(sm.PORTCODE) && sm.PORTCODE != "0")
                tp = "托盘：" + sm[sm.GetAppendPropertyKey("PORTCODE")].ToString();
            string pc = "";
            if (!string.IsNullOrEmpty(sm.BATCHNO))
                pc = "批次：" + sm.BATCHNO;
            string msg = "物料" + mmm.FNAME + "(编码：" + mmm.Code + "" + pc + ")在" + stock.WAREHOUSENAME + "的指定位置(" + hw + tp + ")";
            if (controller == "入库")
            {
                if (list.Count > 0)
                {
                    StockInfoMaterials sm1 = list[0];
                    sm1.NUM = sm1.NUM + sm.NUM;
                    if (mmm.SEQUENCECODE)
                    {
                        string sql = "select SEQUENCECODE from " + seqtn + " where SEQUENCECODE='{0}'";
                        object obj = null;
                        foreach (InfoInSequence iis in sm.StockSequence)
                        {
                            obj = acion.GetValue(string.Format(sql, iis.SEQUENCECODE));
                            if (obj == null || string.IsNullOrEmpty(obj.ToString()))
                                sm1.StockSequence.Add(iis);
                            else
                            {
                                throw new Exception("异常：序列码" + iis.SEQUENCECODE + "在库存中已存在，不能入库！");
                            }
                        }
                    }
                }
                else
                {
                    if (!mmm.SEQUENCECODE)
                        sm.StockSequence.Clear();
                    list.Add(sm);
                }
                ((IEntityCommand)list).Save(acion);
                if (!string.IsNullOrEmpty(sm.PORTCODE))
                {
                    string portno = " id BETWEEN 1212 AND 1226";
                    string upprot = "update " + new Ports().ToString() + " set STATUS=1 where " + portno + " and ID=" + sm.PORTCODE;
                    acion.Execute(upprot);
                }
            }
            if (controller == "出库")
            {
                if (list.Count > 0)
                {
                    StockInfoMaterials sm1 = list[0];
                    if (stay5 != "qt")
                    {
                        string status1 = "SELECT a.ID, CASE WHEN CAST(DATEDIFF(dd, a.LASTINTIME,GETDATE()) AS INTEGER) <= ISNULL(b.shelflife, 0) THEN '否'WHEN ISNULL(b.shelflife, 0) > 0 AND CAST(DATEDIFF(dd, a.LASTINTIME,GETDATE()) AS INTEGER) > ISNULL(b.shelflife,0) THEN '是' ELSE '否' END AS status1 FROM dbo.Acc_WMS_InfoMaterials a INNER JOIN dbo.Acc_Bus_BusinessCommodity b ON a.Code= b.ID where a.id='" + sm1.ID + "'";
                        DataTable dtStatus = acion.GetDataTable(status1);
                        if (dtStatus.Rows[0]["status1"].ToString() == "是")
                        {
                            throw new Exception("异常：" + msg + "指定出库" + sm.NUM + "库存数" + (sm1.NUM + sm.NUM) + "的产品已过有效期请复检合格后出库！（或使用其他出库进行出库）");
                        }
                    }
                    sm1.NUM = sm1.NUM - sm.NUM;
                    if (sm1.NUM < 0)
                    {
                        if (!mmm.NUMSTATE)
                            throw new Exception("异常：" + msg + "指定出库" + sm.NUM + "库存数" + (sm1.NUM + sm.NUM) + "库存数量不足不能出库！");
                    }
                    if (sm1.NUM == 0)
                        list.Remove(sm1);

                    if (mmm.SEQUENCECODE)
                    {
                        string del = "delete from " + seqtn + " where SEQUENCECODE='{0}'";
                        string sql = "select SEQUENCECODE from " + seqtn + " where SEQUENCECODE='{0}'";
                        object obj = null;
                        foreach (InfoInSequence iis in sm.StockSequence)
                        {
                            obj = acion.GetValue(string.Format(sql, iis.SEQUENCECODE));
                            if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
                                acion.Execute(string.Format(del, iis.SEQUENCECODE));
                            else
                                throw new Exception("异常：" + msg + "序列码" + iis.SEQUENCECODE + "不存在,不能出库！");
                        }
                    }
                    //}
                    ((IEntityCommand)list).Save(acion);
                    if (!string.IsNullOrEmpty(sm.PORTCODE))
                    {
                        string upprot = "update " + new Ports().ToString() + " set STATUS=0 where ID=" + sm.PORTCODE;
                        acion.Execute(upprot);
                    }
                }
                else
                {

                    throw new Exception("异常：" + msg + "不存在不能出库!");
                }
            }
            return;
        }

        /// <summary>
        /// 更改托盘状态
        /// </summary>
        /// <param name="sim"></param>
        public void UpdatePortState(StockInfoMaterials sim)
        {
            if (sim.NUM == 0.00)
            {
                EntityList<StockInfoMaterials> listinfo = new EntityList<StockInfoMaterials>(this.model.GetDataAction());
                listinfo.GetData("PORTCODE='"+sim.PORTCODE+"' and ID<>"+sim.ID);
                if (listinfo.Count == 0)
                {
                    EntityList<Ports> listPort = new EntityList<Ports>(this.model.GetDataAction());
                    listPort.GetData("ID=" + sim.PORTCODE);
                    if(listPort[0].STATUS == "1")
                        listPort[0].STATUS = "0";
                    else
                        listPort[0].STATUS = "1";
                    listPort.Save();
                }
            }
        }

        /// <summary>
        /// 更改货位状态
        /// </summary>
        /// <param name="sim"></param>
        public void UpdateWarehouseState(StockInfoMaterials sim)
        {
            if (sim.NUM == 0.00)
            {
                EntityList<StockInfoMaterials> listinfo = new EntityList<StockInfoMaterials>(this.model.GetDataAction());
                listinfo.GetData("DEPOTWBS='" + sim.DEPOTWBS + "' and ID<>" + sim.ID);
                if (listinfo.Count == 0)
                {
                    EntityList<WareHouse> listWarehouse = new EntityList<WareHouse>(this.model.GetDataAction());
                    listWarehouse.GetData("ID=" + sim.DEPOTWBS);
                    if (listWarehouse[0].STATUS == "1")
                        listWarehouse[0].STATUS = "0";
                    else
                        listWarehouse[0].STATUS = "1";
                    listWarehouse.Save();
                }
            }
        }

        /// <summary>
        /// 没有库存记录，出入库都需要创建个新库存记录
        /// </summary>
        /// <param name="sm"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        private static StockInfoMaterials NewStockInfoMaterials(StockInfoMaterials sm, Materials m)
        {
            StockInfoMaterials sim = new StockInfoMaterials();
            sim = sm;
            sim.ISLOCK = false;
            sim.LASTINTIME = DateTime.Now;
            //if (m.ISCHECKPRO == true)
            //{
            //    sim.STATUS = "1";////待检验
            //}
            //else
            //{
            //    sim.STATUS = "4";////免检验
            //}
            return sim;
        }
       
        private string CheckPutInData(string strcontroller)
        {

            EntityList<Ports> list1 = new EntityList<Ports>(this.model.GetDataAction());
            list1.GetData(" ID='" + SmPortcode + "'");
            if (list1.Count == 0)
                return "系统无此托盘！";
            else
            {
                Ports ps = list1[0];
                if (strcontroller == "0")
                {
                    if (ps.STATUS == "1")
                        return "此托盘处于占用状态，不能进行扫描！";
                }
            }
            if (SmCode == null || SmCode == "")
                return "请选择物料！";
            return "Y";
        }
    }
}
