using System;
using System.Collections.Generic;
using System.Linq;
using Acc.Business.Controllers;
using Acc.Business.WMS.Model;
using Acc.Contract.Center;
using Acc.Contract.Data.ControllerData;
using Acc.Contract.MVC;
using Way.EAP.DataAccess.Entity;
using Acc.Contract;
using Acc.Business.Model;
using Acc.Contract.Data;
using Acc.Business.WMS.Controllers;
using Acc.Business.WMS.XHY.Model;
using Way.EAP.DataAccess.Regulation;
using Way.EAP.DataAccess.Data;
using System.Data;

namespace Acc.Business.WMS.XHY.Controllers
{
    public class ProduceInOrderController : XHY_BusinessInOrderController
    {
        public ProduceInOrderController() : base(new XHY_StockInOrder()) { }
        /// <summary>
        /// 描述：新华杨成品入库控制器
        /// 作者：柳强
        /// 创建日期:2013-08-29
        /// </summary>
        #region 基础设置
    

        //显示在菜单
        protected override string OnControllerName()
        {
            return "成品入库";
        }
        //菜单中url路径
        protected override string OnGetPath()
        {
            return "Views/WMS/XHY/ProduceInOrder.htm";
        }
      
        //说明
        protected override string OnGetControllerDescription()
        {
            return "成品入库管理";
        }
        #endregion

        #region 初始化数据方法
        /// <summary>
        /// 初始化显示与否
        /// </summary>
        /// <param name="data"></param>
        /// <param name="item"></param>
        protected override void OnInitViewChildItem(Contract.Data.ModelData data, Contract.Data.ItemData item)
        {

            base.OnInitViewChildItem(data, item);
            if (data.name.EndsWith("StockInOrder"))
            {
                if (item.IsField("SOURCECODE"))
                {
                    if (item.foreign != null)
                    {
                        item.foreign.rowdisplay.Add("STAY5", "stay5");
                    }
                }
                data.title = "成品入库";
                switch (item.field.ToLower())
                {
                    case "code":
                        item.disabled = true;
                        item.title = "成品入库单号";
                        break;
                    case "issubmited":
                    case "isreviewed":
                        item.disabled = true;
                        break;
                    case "sourcecode":
                        item.title = "生产赋码单号";
                        break;
                    case "state":
                    case "sourcename":
                    case "sourceoutcode":
                    case "workerid":
                    case "bumen":
                    case "stocktype":
                    case "funitid":
                        item.visible = false;
                        break;
                    case "clientno":
                        item.title = "客户名称";
                        item.visible = false;
                        break;
                    case "issynchronous":
                        item.visible = true;
                        break;
                    case "stay5":
                        item.visible = true;
                        item.title = "车间";
                        item.index = 4;
                        item.required = true;
                        item.disabled = true;
                        break;
                    case "inouttime":
                        item.title = "生产日期";
                        item.visible = true;
                        item.disabled = true;
                        break;
                }
            }
            if (data.name.EndsWith("StockInOrderMaterials"))
            {
                data.title = "成品入库明细";
                switch (item.field.ToLower())
                {
                    case "num2":
                        item.visible = true;
                        item.title = "已操作数量";
                        break;
                    case "state":
                        item.visible = false;
                        break;
                    case "num":
                        item.title = "入库数量";
                        item.disabled = false;
                        break;
                    case "batchno":
                        item.disabled = true;
                        item.title = "生产批号";
                        break;

                }
                if (item.IsField("materialcode"))
                {
                    if (item.foreign != null)
                    {
                        item.foreign.rowdisplay.Add("STAY1".ToLower(),"num" );
                        item.foreign.rowdisplay.Add("STAY2".ToLower(), "batchno");
                    }
                }

            }
            if (data.name.EndsWith("StockInNoticeMaterials1"))
            {
                data.visible = true;
                switch (item.field.ToLower())
                {
                    case "finishnum":
                        item.title = "已操作数";
                        item.visible = true;
                        break;
                    case "staynum":
                        item.title = "待操作数";
                        item.visible = true;
                        break;
                    case "rowindex":
                    case "batchno":
                        item.visible = true;
                        break;
                    case "materialcode":
                        item.visible = true;
                        item.title = "物料名称";
                        break;
                    case "mcode":
                        item.visible = true;
                        item.title = "物料编码";
                        break;
                    case "fmodel":
                        item.visible = true;
                        item.title = "物料规格";
                        break;
                    case "num":
                        item.title = "入库数量";
                        item.disabled = false;
                        break;
                    case "id":
                        item.foreign.isfkey = true;
                        item.foreign.filedname = item.field;
                        item.foreign.objectname = data.name;
                        item.foreign.foreignobject = typeof(XHY_StockInOrder).FullName;
                        item.foreign.foreignfiled = ("SourceID").ToUpper();
                        item.foreign.tablename = new XHY_StockInOrder().ToString();
                        item.foreign.parenttablename = new StockInNoticeMaterials1().ToString();
                        break;
                    case "producetime":
                        item.visible = true;
                        break;
                    default:
                        item.visible = false;
                        break;
                }
            }
            if (data.name.EndsWith("StockInNoticeMaterials"))
            {
                data.visible = false;
            }
            if (data.name.EndsWith("LogData"))
            {
                switch (item.field.ToLower())
                {

                    case "logname":
                    case "sourcesystem":
                    case "sourcecode":
                    case "targetsystem":
                    case "targetcode":
                        item.visible = false;
                        break;
                }
            }
        }


        #endregion
        protected override void OnForeignIniting(IModel model, InitData data)
        {
            if (model is StockInOrder)
            {
                if (this.fdata.filedname == "SOURCECODE")
                {
                    foreach (ItemData d in data.modeldata.childitem)
                    {//Createdby Creationdate Submitedby  Submiteddate  IsSubmited
                        d.visible = false;
                        if (d.IsField("ROWINDEX"))
                        {
                            d.visible = true;
                            d.index = 1;
                        }
                        if (d.IsField("CREATEDBY"))
                        {
                            d.visible = true;
                        }
                        if (d.IsField("CREATIONDATE"))
                        {
                            d.visible = true;
                        }
                        if (d.IsField("SUBMITEDBY"))
                        {
                            d.visible = true;
                        }
                        if (d.IsField("SUBMITEDDATE"))
                        {
                            d.visible = true;
                        }
                        if (d.IsField("ISSSUBMITED"))
                        {
                            d.visible = true;
                        }
                        if (d.IsField("CODE"))
                        {
                            d.visible = true;
                            d.title = "生产附码单号";
                            d.index = 2;
                        }
                        if (d.IsField("MATERIALCODE"))
                        {
                            d.visible = true;
                            d.index = 3;
                        }
                        if (d.IsField("MCODE"))
                        {
                            d.visible = true;
                            d.index = 3;
                        }
                        if (d.IsField("STAY5"))
                        {
                            d.visible = true;
                            d.title = "车间";
                            d.index = 4;
                        }
                        if (d.IsField("FMODEL"))
                        {
                            d.visible = true;
                        }
                        if (d.IsField("NUM"))//生产数量
                        {
                            d.visible = true;
                        }
                        if (d.IsField("BATCHNO"))//生产批次
                        {
                            d.visible = true;
                        }
                    } return;
                }
                else
                {
                    base.OnForeignIniting(model, data);
                }
            }
            else {
                base.OnForeignIniting(model,data);
            }
        }

        #region 重写方法

        protected override void OnAdding(ControllerBase.SaveEvent item)
        {
            ValidateDZ(item);
            base.OnAdding(item);
           
        }

        protected override void OnEditing(ControllerBase.SaveEvent item)
        {
            ValidateDZ(item);
            base.OnEditing(item);
        }


        
        protected override void OnRemoveing(ControllerBase.SaveEvent item)
        {
           
            base.OnRemoveing(item);
            

        }

        public override void ValidateAddInNum(ControllerBase.SaveEvent item)
        {
            StockInOrder si = item.Item as StockInOrder;
            int d;
            for (int i = 0; i <si.Materials.Count ; i++)
            {
                string str = si.Materials[i].NUM.ToString();
                try
                {
                    d = Convert.ToInt32(str);
                }
                catch
                {
                    throw new Exception("序号:" + si.Materials[i].RowIndex + "  商品编码：" + si.Materials[i].MCODE + " 输入不是有效的数字或小数！");
                }
              
            }
            
            base.ValidateAddInNum(item);
        }

        /// <summary>
        /// 验证成品入库产品是否有单重，如果没有不能保存和编辑
        /// </summary>
        /// <param name="item"></param>
        public void ValidateDZ(ControllerBase.SaveEvent item)
        {
            XHY_StockInOrder si = item.Item as XHY_StockInOrder;
            EntityList<XHY_Materials> ms = new EntityList<XHY_Materials>(this.model.GetDataAction());
            for (int i = 0; i < si.Materials.Count; i++)
            {
                ms.GetData("id='"+si.Materials[i].MATERIALCODE+"'");
                if (ms[0].WeightNUM == 0)
                {
                    throw new Exception("序号:" + si.Materials[i].RowIndex + " 商品名称:" + ms[0].FNAME + "  商品编码：" + ms[0].Code + " 单重属性值为0，请添加该产品单重后操作！");
                }
            }
        }

        protected override void OnSubmitData(BasicInfo info)
        {
            ValidateFMCode(info);
            base.OnSubmitData(info);
            ClearGroupTray(info);

        }

        /// <summary>
        /// 删除组盘记录表
        /// </summary>
        private void ClearGroupTray(BasicInfo info)
        {
            EntityList<GroupTray> listGT = new EntityList<GroupTray>(this.model.GetDataAction());
            StockInOrder sio = info as StockInOrder;
            GroupTray gt = new GroupTray();
            sio.Materials.DataAction = this.model.GetDataAction();
            sio.Materials.GetData();
            for (int i = 0; i < sio.Materials.Count; i++)
            {
                sio.Materials[i].InSequence.DataAction = this.model.GetDataAction();
                sio.Materials[i].InSequence.GetData();
                for (int j = 0; j < sio.Materials[i].InSequence.Count; j++)
                {
                    ////如果出库数量等于单件码
                    //if (sio.Materials[i].NUM == sio.Materials[i].InSequence.Count)
                    //{
                    //    string sql = "delete from " + gt.ToString() + " where traycode='" + sio.Materials[i].PORTNAME+ "'";
                    //    this.model.GetDataAction().Execute(sql);
                    //}
                    //listGT.GetData("FModel='" + sio.Materials[i].InSequence[j].SEQUENCECODE.ToString() + "'");
                    //listGT.RemoveAt(0);
                    //listGT.Save();
                    string sql = "delete from " + gt.ToString() + " where FModel='" + sio.Materials[i].InSequence[j].SEQUENCECODE.ToString() + "'";
                    this.model.GetDataAction().Execute(sql);
                }
            }
           
        }

        /// <summary>
        /// 验证当前三级表中录入的单件码数量是否和明细行num相符
        /// </summary>
        /// <param name="info"></param>
        private void ValidateFMCode(BasicInfo info)
        {
            XHY_StockInOrder sio = info as XHY_StockInOrder;
            sio.Materials.DataAction = this.model.GetDataAction();
            sio.Materials.GetData();
            for (int i = 0; i < sio.Materials.Count; i++)
            {
                sio.Materials[i].InSequence.DataAction = this.model.GetDataAction();
                sio.Materials[i].InSequence.GetData();
                 bool isdjm = sio.Materials[i].GetForeignObject<Materials>(this.model.GetDataAction()).SEQUENCECODE;
                 if (isdjm == true)
                 {
                     if (sio.Materials[i].InSequence.Count < sio.Materials[i].NUM)
                     {
                         throw new Exception("序号:" + sio.Materials[i].RowIndex + " 商品编码" + sio.Materials[i].MCODE + " 单件码录入不足" + sio.Materials[i].NUM + "请使用终端进行扫码");
                     }
                     if (sio.Materials[i].InSequence.Count > sio.Materials[i].NUM)
                     {
                         throw new Exception("序号:" + sio.Materials[i].RowIndex + " 商品编码" + sio.Materials[i].MCODE + " 单件码录入超过" + sio.Materials[i].NUM + "请减少单件码后保存");
                     }
                 }
            }
        }

        /// <summary>
        /// 添加单据必备值：stocktype、sourceid、sourceController、SourceRowID、SourceTable
        /// </summary>
        /// <param name="item"></param>
        public override void AddDefault(ControllerBase.SaveEvent item)
        {
            XHY_StockInOrder ord = item.Item as XHY_StockInOrder;
            StockInNoticeMaterials1 sm = new StockInNoticeMaterials1();
            if (ord.SourceCode != null)
            {
                ///如果目标单据的sourceId =0 代表没有使用下推方法，则赋值SourceID和SourceController
                EntityList<StockInNoticeMaterials1> sin = new EntityList<StockInNoticeMaterials1>(this.model.GetDataAction());
                sin.GetData("code='" + ord.SourceCode + "'");
                if (sin.Count > 0)
                {
                    ord.SourceID = sin[0].ID;
                    ord.SourceController = InOrderController();
                    for (int j = 0; j < ord.Materials.Count; j++)
                    {
                        ord.Materials[j].SourceRowID = sin[0].ID;
                        ord.Materials[j].SourceTable = sm.ToString();
                    }
                }
            }
        }

        /// <summary>
        /// 制定源单控制器为生产附码单控制器
        /// </summary>
        /// <returns></returns>
        public override string InOrderController()
        {
            return new EndowedInNoticeOrderController().ToString();
        }

        /// <summary>
        /// 设置stockType= 1
        /// </summary>
        /// <returns></returns>
        public override int SetStockType()
        {
            return 1;
        }
        /// <summary>
        /// 加载待生产赋码单
        /// </summary>
        [WhereParameter]
        public string NoticeId1 { get; set; }
        public override string GetNoticeStayGrid()
        {
            EntityList<StockInNoticeMaterials1> List1 = new EntityList<StockInNoticeMaterials1>(this.model.GetDataAction());
            List1.GetData("code='" + NoticeId1 + "'");
            string str = JSON.Serializer(List1.ToArray());
            return str;
        }
        protected override void foreignInOrder(IModel model, loadItem item)
        {
            StockInNoticeMaterials1 sim = new StockInNoticeMaterials1();
            if (this.fdata.filedname == "SOURCECODE")
            {
                //item.rowsql = "select * from (" + item.rowsql + ") a where a.code in(select code from " + this.fdata.tablename + " where stocktype =  " + new PurchaseInNoticeOrderController().SetStockType() + " and IsSubmited=1)";
                item.rowsql = "select * FROM (" + item.rowsql + ") a where a.IsSubmited=1";
            }
            else
            {
                base.foreignInOrder(model, item);
            }
        }
        protected override void foreignInOrderMaterials(IModel model, loadItem item)
        {
            string W = "";
            //foreach (SQLWhere w in item.whereList)
            //    W += w.where("and   ");
            if (item.whereList.Length > 0)
            {
                string file = item.whereList[0].ColumnName;
                string symol = item.whereList[0].Symbol;
                string value = item.whereList[0].Value;
                file = file.Substring(file.IndexOf('.'));
                if (file.Equals(".STAY1"))
                {
                    symol = "=";
                    value = value.Substring(2, value.Length - 4);
                }
                switch (file)
                {
                    case ".FNAME": file = "c" + file; break;
                    case ".FMODEL": file = "c" + file; break;
                    case ".CODE": file = "c" + file; break;
                    case ".STAY1": file = "b." + "NUM"; break;
                    case ".STAY2": file = "b." + "BATCHNO"; break;
                }
                W = "and  " + file + "  " + symol + "   " + value;
            }
            StockInNoticeMaterials1 sim = new StockInNoticeMaterials1();
            if (this.ActionItem==null){
                base.foreignInOrderMaterials(model, item);
            }
            else  if (this.ActionItem["SourceCode"].ToString() == "")
            {
                throw new Exception("请先检查通知单是否选择");
            }
            ///选择可用的产品
            else if (this.fdata.filedname == "MATERIALCODE")
            {
                //item.rowsql = "select * from (" + item.rowsql + ") a where a.CommodityType=2 and a.id in(select MATERIALCODE from " + sim.ToString() + " sim where sim.code ='" + this.ActionItem["SourceCode"] + "')";
                //item.rowsql = "select * from (" + item.rowsql + ") a where a.CommodityType=2 and a.id in(select MATERIALCODE from " + sim.ToString() + " sim where sim.code ='" + this.ActionItem["SourceCode"] + "')";
                item.rowsql = string.Format("select c.ID,c.FNAME,c.FMODEL,c.Code,b.NUM STAY1,b.BATCHNO STAY2 from {0} b left join {1} c on b.MATERIALCODE=c.ID where b.Code='{2}' " + W + "", sim.ToString(), this.fdata.tablename, this.ActionItem["SourceCode"]);
                //item.rowsql = string.Format("select c.ID,c.FNAME,c.FMODEL,c.Code,b.NUM STAY1,b.BATCHNO STAY2 from {0} a left join {1} b on a.ID=b.PARENTID left join {2} c on b.MATERIALCODE=c.ID where a.Code='{3}'", new StockInNotice().ToString(), new StockInNoticeMaterials().ToString(), this.fdata.tablename, this.ActionItem["SourceCode"]);
            }
            else {
                base.foreignInOrderMaterials(model, item);
            }
            
        }
        //protected override void OnForeignLoading(IModel model, loadItem item)
        //{
        //    string s = this.fdata.filedname;
        //    StockInNoticeMaterials1 sim = model as StockInNoticeMaterials1;
        //    if (sim != null)
        //    {

        //    }
        //    else
        //        base.OnForeignLoading(model, item);
        //}
        #endregion

        ///// <summary>
        ///// 添加按钮（同步到K3）
        ///// </summary>
        //[ActionCommand(name = "同步到K3", title = "将该单据数据同步到K3系统", index = 9, icon = "icon-ok", isalert = true)]
        //public void TransferData()
        //{
        //    StockInOrder mi = this.ActionItem as StockInOrder;
        //    if (!mi.IsSubmited)
        //    {
        //        throw new Exception("单据未提交，不符合同步条件！");
        //    }
        //}


        /// <summary>
        /// 加载待入库明细
        /// </summary>
        [WhereParameter]
        public string NoticeId { get; set; }
        public virtual string GetNoticeStayGrid1()
        {
            EntityList<StockInNoticeMaterials1> list = new EntityList<StockInNoticeMaterials1>(this.model.GetDataAction());
            list.GetData("code='" + NoticeId + "'");
            List<StockInNoticeMaterials1> list1 = new List<StockInNoticeMaterials1>();
            list1 = list;
            string str = JSON.Serializer(list.ToArray());
            return str;
        }


        #region 终端使用的方法
        [WhereParameter]
        public List<InSequenceList> InSequences { get; set; }
        public string AddInSequence()
        {
            EntityList<FMInSequence> list = new EntityList<FMInSequence>(this.model.GetDataAction());
            EntityList<InSequence> listins = new EntityList<InSequence>(this.model.GetDataAction());
            EntityList<InfoInSequence> listinfo = new EntityList<InfoInSequence>(this.model.GetDataAction());
            EntityList<OutInSequence> listoutin = new EntityList<OutInSequence>(this.model.GetDataAction());
            EntityList<StockInOrder> listinorder = new EntityList<StockInOrder>(this.model.GetDataAction());
            EntityList<StockOutOrderMaterials> listOutM = new EntityList<StockOutOrderMaterials>(this.model.GetDataAction());
            for (int i = 0; i < InSequences.Count; i++)
            {
                //入库
                if (InSequences[i].OutOrderMATERIALID == 0 && InSequences[i].InOrderMATERIALID!=0)
                {
                    list.GetData(" SEQUENCECODE='" + InSequences[i].SEQUENCECODE.ToString() + "' ");
                    if (list.Count == 0)
                    {
                        IDataAction action = this.model.GetDataAction();
                        action.Execute(" delete  from Acc_WMS_InOrderMaterials where ID=" + InSequences[i].InOrderMATERIALID);
                        throw new Exception(InSequences[i].SEQUENCECODE.ToString() + " 序列码不存在，操作失败！");
                    }
                    else
                    {
                        listinorder.GetData("ID=" + InSequences[i].InOrderId.ToString());
                        if (list[0]["InNoticeMaterialsSourceID"].ToString() != listinorder[0].SourceID.ToString() && listinorder[0].STOCKTYPE != 4)
                        {
                            IDataAction action = this.model.GetDataAction();
                            action.Execute(" delete  from Acc_WMS_InOrderMaterials where ID=" + InSequences[i].InOrderMATERIALID);
                            throw new Exception(InSequences[i].SEQUENCECODE.ToString() + " 序列码不存在该单据中，操作失败！");
                        }
                    }
                }
                //出库
                if (InSequences[i].InOrderMATERIALID == 0)
                {
                    listinfo.GetData(" SEQUENCECODE='" + InSequences[i].SEQUENCECODE.ToString() + "'");
                    if (listinfo.Count == 0)
                    {
                        IDataAction action = this.model.GetDataAction();
                        action.Execute(" delete  from Acc_WMS_OutOrderMaterials where ID=" + InSequences[i].InOrderMATERIALID);
                        throw new Exception(InSequences[i].SEQUENCECODE.ToString() + " 序列码不存在，操作失败！");
                    }
                    else
                    {
                        if (listinfo[0]["STOCKINFOMATERIALSID"].ToString() != InSequences[i].STOCKINFOMATERIALSID.ToString())
                        {
                            IDataAction action = this.model.GetDataAction();
                            action.Execute(" delete  from Acc_WMS_OutOrderMaterials where ID=" + InSequences[i].InOrderMATERIALID);
                            throw new Exception(InSequences[i].SEQUENCECODE.ToString() + " 序列码不属于该产品，操作失败！");
                        }
                    }
                }
            }
            for (int i = 0; i < InSequences.Count; i++)
            {
                InSequence InS = new InSequence();
                OutInSequence OutS = new OutInSequence();
                //入库
                if (InSequences[i].OutOrderMATERIALID == 0)
                {
                    InS.InOrderMATERIALID = InSequences[i].InOrderMATERIALID;
                    InS.SEQUENCECODE = InSequences[i].SEQUENCECODE.ToString();
                    listins.Add(InS);
                }
                //出库
                if (InSequences[i].InOrderMATERIALID == 0)
                {
                    OutS.OutOrderMATERIALID = InSequences[i].OutOrderMATERIALID;
                    OutS.SEQUENCECODE = InSequences[i].SEQUENCECODE.ToString();
                    listoutin.Add(OutS);
                    if (InSequences[i].OutOrderMATERIALID != 0)//判断是不是其他出库
                    {
                        listOutM.GetData("ID=" + InSequences[i].OutOrderMATERIALID);
                        listOutM[0].STAY4 = "已确认";
                        listOutM.Save();
                    }
                }
            }
            listins.Save();
            listoutin.Save();
            throw new Exception("Y");
        }

        [WhereParameter]
        public string webSql { get; set; }
        public string webExecSql()
        {
            IDataAction action = this.model.GetDataAction();
            action.Execute(webSql);
            throw new Exception("Y");
        }

        [WhereParameter]
        public string CPorderid { get; set; }
        [WhereParameter]
        public string SEcode { get; set; }
        public string GetSequencesList()
        {
            EntityList<FMInSequence> list = new EntityList<FMInSequence>(this.model.GetDataAction());
            if (SEcode == null)
                list.GetData("InNoticeMaterialsSourceID=" + CPorderid);
            else
                list.GetData("InNoticeMaterialsSourceID=" + CPorderid + " and SEQUENCECODE='" + SEcode + "'");
            string str = JSON.Serializer(list.ToArray());
            return str;
        }

        [WhereParameter]
        public string MATERIALCODE { get; set; }
        [WhereParameter]
        public string batchno { get; set; }
        [WhereParameter]
        public string WAREHOUSEID { get; set; }
        [WhereParameter]
        public string DEPOTWBS { get; set; }
        [WhereParameter]
        public string PORTCODE { get; set; }

        public string GetInfoSequenceList()
        {
            EntityList<StockInfoMaterials> simList = new EntityList<StockInfoMaterials>(this.model.GetDataAction());
            EntityList<InfoInSequence> infoInsList = new EntityList<InfoInSequence>(this.model.GetDataAction());
            simList.GetData("code='" + MATERIALCODE + "' and batchno='" + batchno + "' and WAREHOUSEID='" + WAREHOUSEID + "' and DEPOTWBS ='" + DEPOTWBS + "' and PORTCODE='" + PORTCODE + "'");
            if (simList.Count == 1)
            {
                infoInsList.GetData("STOCKINFOMATERIALSID='" + simList[0].ID + "'");
            }
            string str = EntityHelper.ToJSON(infoInsList.ToArray());
            return str;
        }

        public string GetInfoSequenceListById()
        {
            EntityList<InfoInSequence> simList = new EntityList<InfoInSequence>(this.model.GetDataAction());
            simList.GetData("STOCKINFOMATERIALSID='" + CPorderid + "' and SEQUENCECODE='" + SEcode + "'");
            string str = EntityHelper.ToJSON(simList.ToArray());
            return str;
        }

        public string GetZPSUM()
        {
            EntityList<ZPSUM> List = new EntityList<ZPSUM>(this.model.GetDataAction());
            List.GetData("OrderCode='" + CPorderid + "'");
            string str = EntityHelper.ToJSON(List.ToArray());
            return str;
        }

        public string UpdateSCFM()
        {
            EntityList<StockInNoticeMaterials1> simList = new EntityList<StockInNoticeMaterials1>(this.model.GetDataAction());
            simList.GetData("ID=" + CPorderid);
            simList[0].STATE = 1;
            simList.Save();
            throw new Exception("Y");
        }


        public string GetServerData()
        {
            IDataAction action = this.model.GetDataAction();
            DataTable dt = action.GetDataTable("Select CONVERT(varchar(100), GETDATE(), 12) as Data");
            throw new Exception(dt.Rows[0]["Data"].ToString());
        }
        #endregion



    }



    public class InSequenceList
    {
        /// <summary>
        /// 单据ID
        /// </summary>
        public int InOrderId { get; set; }
        /// <summary>
        /// 入库明细ID
        /// </summary>
        public int InOrderMATERIALID { get; set; }
        /// <summary>
        /// 出库明细ID
        /// </summary>
        public int OutOrderMATERIALID { get; set; }
        /// <summary>
        /// 库存ID
        /// </summary>
        public int STOCKINFOMATERIALSID { get; set; }
        /// <summary>
        /// 序列码
        /// </summary>
        public string SEQUENCECODE { get; set; }
    }


    public class ZPSUM : BasicInfo
    {
        /// <summary>
        /// 单号
        /// </summary>
        public string OrderCode { get; set; }
        /// <summary>
        /// 托盘
        /// </summary>
        public string TrayCode_PORTNO { get; set; }
        /// <summary>
        /// 产品编码
        /// </summary>
        public string GCODE { get; set; }
        /// <summary>
        /// 产品
        /// </summary>
        public string GNAME_FNAME { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public double NUM { get; set; }

        protected override string GetSearchSQL()
        {
            string strSql = " select E.OrderCode,E.TrayCode_PORTNO,E.GCODE,E.GNAME_FNAME,COUNT(*) as NUM from (";
            strSql = strSql + "select A.*,B.WAREHOUSENAME as WHNAME_WAREHOUSENAME,C.PORTNO as TrayCode_PORTNO,D.FNAME as GNAME_FNAME ";
            strSql = strSql + " from Acc_WMS_GroupTray A left join Acc_WMS_WareHouse B on A.WHNAME=B.ID left join Acc_WMS_Ports C on A.TrayCode=C.ID left join Acc_Bus_BusinessCommodity D on A.GNAME=D.ID ";
            strSql = strSql + ") E  group by E.OrderCode,E.TrayCode_PORTNO,E.GCODE,E.GNAME_FNAME";
            return strSql;
        }
    }
}
