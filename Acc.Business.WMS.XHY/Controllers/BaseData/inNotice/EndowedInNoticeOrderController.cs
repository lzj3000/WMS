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
    public class EndowedInNoticeOrderController : BusinessController
    {
        public EndowedInNoticeOrderController()
            : base(new StockInNoticeMaterials1())
        {
            // IEntityBase ieb = (IEntityBase)this.model;
            //ieb.ForeignKey += new ForeignKeyHandler(StockoutForeignKeyHandler);

            //IHierarchicalEntityView[] view = ieb.GetChildEntityList();
            //for (int i = 0; i < view.Length; i++)
            //{
            //    if (view[i].ChildEntity.ToString().Equals("Acc_WMS_StockInNoticeMaterials", StringComparison.OrdinalIgnoreCase))
            //    {
            //        ((IEntityBase)view[i].ChildEntity).ForeignKey += new ForeignKeyHandler(SendListForeignKeyHandler);
            //    }
            //}
        }
        #region 页面设置

        //菜单中url路径
        protected override string OnGetPath()
        {
            return "Views/WMS/XHY/EndowedInNoticeOrder.htm";
        }

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
        /// <summary>
        /// 动态绑定外键
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="target"></param>
        //void StockoutForeignKeyHandler(EntityBase entity, Dictionary<string, EntityForeignKeyAttribute> target)
        //{
        //    if (target.ContainsKey("Stay5"))
        //    {
        //        target.Remove("Stay5");
        //    }
        //    target.Add("Stay5", new EntityForeignKeyAttribute(typeof(Organization), "ID", "OrganizationName"));
        //}

        /// <summary>
        /// 显示名
        /// </summary>
        /// <returns></returns>
        protected override string OnControllerName()
        {
            return "生产赋码单";
        }
        protected override void OnForeignIniting(IModel model, InitData data)
        {
            if (model is StockInNoticeMaterials1)
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
                //序号 车间编码CODE、名称OrganizationName、上级机构ParentID、是否禁用IsDisable、、备注Remark
                if (this.fdata.filedname == "STAY5")
                {
                    foreach (ItemData d in data.modeldata.childitem)
                    {
                        d.visible = false;
                        if (d.IsField("ROWINDEX"))
                        {
                            d.visible = true;
                        }
                        if (d.IsField("Code"))
                        {
                            d.visible = true;
                            d.title = "车间编码";
                        }
                        if (d.IsField("OrganizationName"))
                            d.visible = true;
                        if (d.IsField("ParentID"))
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

            if (data.name.EndsWith("StockInNoticeMaterials1"))
            {
                data.title = "生产赋码明细";
                if (item.IsField("materialcode"))
                {
                    if (item.foreign != null)
                    {
                        item.foreign.rowdisplay.Add("FMODEL", "FMODEL");
                        item.foreign.rowdisplay.Add("CODE", "MCODE");
                    }
                }

                switch (item.field.ToLower())
                {
                    case "stay5":
                        item.visible = true;
                        item.title = "车间";
                        item.index = 4;
                        //Organization order = new Organization();
                        //item.foreign.isfkey = true;
                        //item.foreign.foreignfiled = "ID";
                        //item.foreign.filedname = item.field;
                        //item.foreign.displayfield = ("OrganizationName").ToUpper();
                        //item.foreign.objectname = data.name;
                        //item.foreign.foreignobject = order.GetType().FullName;
                        //item.foreign.tablename = order.ToString();
                        item.required = true;
                        break;
                    case "sourcecode":
                    case "parentid":
                        item.visible = false;
                        break;
                    case "num":
                        item.title = "生产数量";
                        item.required = true;
                        break;
                    case "batchno":
                        item.visible = true;
                        item.title = "生产批号";
                        item.index = 5;
                        item.required = true;
                        break;
                    //case "stay1":
                    //            item.visible = true;
                    //            item.index = 6;
                    //            item.title = "是否已生成单件码";
                    //            item.disabled = true;
                    //            break;
                    case "finishnum":
                    case "staynum":
                        item.disabled = true;
                        item.visible = false;
                        break;
                    case "sourcename":
                    case "sourceoutcode":
                    case "workerid":
                    case "bumen":
                    case "stocktype":
                    case "finishtime":
                    case "isdisable":
                    case "funitid":
                        item.visible = false;
                        break;
                    case "code":
                        item.visible = true;
                        item.title = "生产赋码单号";
                        item.disabled = true;
                        break;
                    case "mcode":
                    case "fmodel":
                    case "rowindex":

                        item.visible = true;
                        item.disabled = true;
                        break;
                    case "materialcode":
                        item.required = true;
                        break;
                    case "submiteddate":
                    case "submitedby":
                    case "issubmited":
                    case "createdby":
                    case "creationdate":
                        item.visible = true;
                        item.disabled = true;
                        break;
                    case "stay3":
                        item.disabled = true;
                        item.index = 8;
                        item.visible = true;
                        item.title = "已下推质检";
                        break;
                    case "producetime":
                        item.visible = true;
                        break;
                }
            }
            if (data.name.EndsWith("FMInSequence"))
            {
                data.visible = true;
                data.disabled = true;
                data.title = "单件码";
                switch (item.field.ToLower())
                {
                    case "sequencecode":
                        item.visible = true;
                        break;
                    default:
                        item.visible = false;
                        break;
                }
            }
        }
        #endregion
        #region 重写方法
        protected override void OnForeignLoading(IModel model, loadItem item)
        {
            //////选择成品项
            if (this.fdata.filedname == "MATERIALCODE")
            {
                item.rowsql = "select * from (" + item.rowsql + ") a where a.CommodityType=2";
                //item.rowsql = "select id,code,FNAME,FFULLNAME,ClassID,FUNITID,BATCH,STATUS,ISOVERIN,IsDisable,ISCHECKPRO,Remark,IsDelete from " + new BusinessCommodity().ToString() + " a where a.CommodityType=2";
            }            
            if (this.fdata.filedname == "STAY5")
            {
                item.rowsql = "select * from (" + item.rowsql + ") a where a.parentid in(select id from " + new Organization().ToString() + " where OrganizationName='生产中心')";
            } //item.rowsql = "select id,CODE,OrganizationName,ParentID,IsDisable,Remark from  "+new Organization().ToString();
            base.OnForeignLoading(model, item);
        }
        /// <summary>
        /// 通知单下推，选择其他原单时改变待入库明细的数据
        /// 加载待生产赋码单
        /// </summary>
        [WhereParameter]
        public string NoticeId1 { get; set; }
        public virtual string GetNoticeStayGrid()
        {
            EntityList<StockInNoticeMaterials1> List1 = new EntityList<StockInNoticeMaterials1>(this.model.GetDataAction());
            List1.GetData("code='" + NoticeId1 + "'");
            string str = JSON.Serializer(List1.ToArray());
            return str;
        }
        protected override void OnAdding(ControllerBase.SaveEvent item)
        {
            ValiItem(item);
            base.OnAdding(item);
        }


        protected override void OnEditing(ControllerBase.SaveEvent item)
        {
            StockInNoticeMaterials1 sin = item.Item as StockInNoticeMaterials1;
            if (sin.IsSubmited == true)
            {
                throw new Exception("只有新建状态下的单据可以编辑!");
            }
            if (sin.NUM < sin.STAY6)
            {
                throw new Exception("生产数量不能小于原来数量!");
            }
            base.OnEditing(item);
            sin.STAY6 = sin.NUM;

        }


        protected override ReadTable OnSearchData(loadItem item)
        {
            //item.rowsql="select distinct ID,STAY5,TempTime,SourceID,SourceCode,SourceController,SourceRowID,SourceName	,SourceTable,MATERIALCODE,NUM,FUNITID,PRICE,STATE	,BATCHNO,STAY6,STAY7	,IsSubmited,	Submiteddate,Submitedby,Createdby,Creationdate,Modifiedby,Modifieddate,Reviewedby,	Revieweddate,IsReviewed,Code,IsDisable,IsDelete,Remark,STAY1,STAY2,STAY3,STAY4,FMODEL,MCODE	,FINISHNUM	,STAYNUM from ("+item.rowsql+") as o";
            return base.OnSearchData(item);
        }


        protected override void OnSubmitData(BasicInfo info)
        {
            StockInNoticeMaterials1 sim = info as StockInNoticeMaterials1;
            if (sim.STAY2 == null)
            {
                throw new Exception("异常：请先生成单件码后提交！");
            }
            if (sim.IsSubmited == true)
            {
                throw new Exception("异常：单据已提交不能重复提交！");
            }
            base.OnSubmitData(info);
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

        protected override ReadTable OnForeignLoad(IModel model, loadItem item)
        {
            return base.OnForeignLoad(model, item);
        }
        ///// <summary>
        ///// 设置stockType=3
        ///// </summary>
        ///// <returns></returns>
        //public override int SetStockType()
        //{
        //    return 3;
        //}
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
                if (ac.command == "setCode")
                {
                    ac.index = 4;
                }
                if (ac.command == "SubmitData")
                {
                    ac.index = 5;
                }
            }
            return coms;
        }

        /// <summary>
        /// 出库通知单删除时验证此方法
        /// </summary>
        protected override void Remove()
        {
            if (this.ActionItem != null)
            {
                BasicInfo info = getinfo(this.ActionItem["id"].ToString());
                if (info != null)
                {
                    EntityList<StockInOrder> sio = new EntityList<StockInOrder>(this.model.GetDataAction());
                    sio.GetData("sourcecode='" + info.Code + "'");
                    if (sio.Count == 0)
                    {
                        try
                        {
                            this.modelList.Add(this.ActionItem);
                            this.modelList.Remove(this.ActionItem);
                            this.modelList.Save();
                        }
                        finally
                        {
                            this.modelList.Clear();
                        }
                    }
                    else
                    {
                        throw new Exception("单据不能删除，该单据已在下游单据中存在！");
                    }
                }

            }
        }


        /// <summary>
        /// 设置下推对象
        /// </summary>
        /// <returns></returns>
        public ControllerBase PushController()
        {
            return new ProduceInOrderController();
        }
        #endregion

        #region 生成单件码
        [ActionCommand(name = "生成单件码", title = "生成单件码", index = 6, icon = "icon-ok", isselectrow = true, isalert = true)]
        public void setCode()
        {
           
            if (Convert.ToInt32(this.ActionItem["IsSubmited"]) == 1)
            {
                throw new Exception("单据已提交不能生成单件码!");
            }
            FMInSequence isq = new FMInSequence();
            StockInNoticeMaterials1 simStr = new StockInNoticeMaterials1();
            Organization orz = new Organization();
            int id_key = Convert.ToInt32(this.ActionItem["ID"]);
            DataTable dtIsq = this.model.GetDataAction().GetDataTable("select id from " + isq.ToString() + " where InNoticeMaterialsSourceID =" + id_key + "");
            if (this.ActionItem["STAY2"] != null && Convert.ToInt32(this.ActionItem["NUM"]) == Convert.ToInt32(dtIsq.Rows.Count))
            {
                throw new Exception("不能重复生成单件码!");
            }
            if (this.ActionItem["STAY5"] == "")
            {
                throw new Exception("选择车间数据错误!");
            }
            string tempchejian = this.model.GetDataAction().GetDataTable("select code from " + orz.ToString() + " where id = '" + this.ActionItem["STAY5"].ToString() + "'").Rows[0]["Code"].ToString();
            if (tempchejian.Length == 0 || tempchejian.Length < 2)
            {
                throw new Exception("选择车间数据无Code数据!");
            }
            if (tempchejian.Length == 0 || tempchejian.Length < 2)
            {
                throw new Exception("选择车间数据无Code数据!");
            }
            string chejianno =tempchejian.Substring(tempchejian.Length-2,2);
            string materialno = this.ActionItem["MATERIALCODE"].ToString().PadLeft(4, '0');
            string time = DateTime.Now.ToString("yyMMdd");
            int startNum = 1;
            int endNum = 0;
            Random rad = null;
            string sequencecode = string.Empty;
            int innoticesourceid = id_key;
            string sqlStr = "select * from " + simStr.ToString() + " where stay5='" + this.ActionItem["STAY5"] + "' and materialcode='" + this.ActionItem["MATERIALCODE"] + "' and temptime='" + DateTime.Now.ToString("yyyyMMdd") + "' and issubmited=1";
            DataTable dt = this.model.GetDataAction().GetDataTable(sqlStr);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    startNum += Convert.ToInt32(dt.Rows[i]["NUM"]);
                }
                endNum = startNum + Convert.ToInt32(this.ActionItem["NUM"]) - 1;
                rad = new Random();
                for (int i = startNum; i <= endNum; i++)
                {
                    int value1 = rad.Next(1, 10);
                    int value2 = rad.Next(1, 10);
                    int value3 = rad.Next(1, 10);
                    int value4 = rad.Next(1, 10);
                    string value = value1.ToString() + value2.ToString() + value3.ToString() + value4.ToString();
                    sequencecode = chejianno + materialno + time + i.ToString().PadLeft(4, '0') + value;
                    string sql = string.Format("insert into " + isq.ToString() + "(innoticematerialssourceid,sequencecode) values('{0}','{1}')", innoticesourceid, sequencecode);
                    this.model.GetDataAction().Execute(sql);
                }

            }
            else
            {

                if (DateTime.Now.ToString("yyyyMMdd").Equals(this.ActionItem["TempTime"]))
                {
                    endNum = Convert.ToInt32(this.ActionItem["NUM"]) - dtIsq.Rows.Count;
                    if (this.ActionItem["STAY2"] != null)
                    {
                        startNum = dtIsq.Rows.Count + 1;
                        endNum = startNum + (Convert.ToInt32(this.ActionItem["NUM"]) - dtIsq.Rows.Count) - 1;
                    }
                    rad = new Random();
                    for (int i = startNum; i <= endNum; i++)
                    {
                        int value1 = rad.Next(1, 10);
                        int value2 = rad.Next(1, 10);
                        int value3 = rad.Next(1, 10);
                        int value4 = rad.Next(1, 10);
                        string value = value1.ToString() + value2.ToString() + value3.ToString() + value4.ToString();
                        sequencecode = chejianno + materialno + time + i.ToString().PadLeft(4, '0') + value;
                        string sql = string.Format("insert into " + isq.ToString() + "(innoticematerialssourceid,sequencecode) values('{0}','{1}')", innoticesourceid, sequencecode);
                        this.model.GetDataAction().Execute(sql);
                    }
                }
                else
                {
                    startNum += dtIsq.Rows.Count;
                    endNum = Convert.ToInt32(this.ActionItem["NUM"]);
                    rad = new Random();
                    for (int i = startNum; i <= endNum; i++)
                    {

                        int value1 = rad.Next(1, 10);
                        int value2 = rad.Next(1, 10);
                        int value3 = rad.Next(1, 10);
                        int value4 = rad.Next(1, 10);
                        string value = value1.ToString() + value2.ToString() + value3.ToString() + value4.ToString();
                        sequencecode = chejianno + materialno + time + i.ToString().PadLeft(4, '0') + value;
                        string sql = string.Format("insert into " + isq.ToString() + "(innoticematerialssourceid,sequencecode) values('{0}','{1}')", innoticesourceid, sequencecode);
                        this.model.GetDataAction().Execute(sql);
                    }
                }
            }
            this.model.GetDataAction().Execute("update " + simStr.ToString() + " set stay2='1' where id=" + id_key + "");
        }


        private void ValiItem(ControllerBase.SaveEvent item)
        {
            StockInNoticeMaterials1 sim = item.Item as StockInNoticeMaterials1;
            if (sim.ProduceTime == DateTime.MinValue)
            {
                throw new Exception("请填写生产日期!");
            }
            if (sim.STAY5 == "")
            {
                throw new Exception("车间不能为空!");
            }
            if (sim.MATERIALCODE == "")
            {
                throw new Exception("物料不能为空!");
            }
            if (sim.NUM == 0)
            {
                throw new Exception("数量不能为空!");
            }
            if (sim.NUM > 9999)
            {
                throw new Exception("数量不能大于9999!");
            }
            if (sim.BATCHNO == "")
            {
                throw new Exception("批次不能为空!");
            }
            string time = DateTime.Now.ToString("yyyyMMdd");
            string sqlStr = "select * from " + sim.ToString() + " where stay5='" + sim.STAY5 + "' and materialcode='" + sim.MATERIALCODE + "' and issubmited = 0 and TempTime ='"+time+"'";
            DataTable dt = this.model.GetDataAction().GetDataTable(sqlStr);
            if (dt.Rows.Count > 0)
            {
                throw new Exception("单号"+dt.Rows[0]["Code"]+"已存在本次添加数据的相同信息，请在该单据中添加生产数量或提交该单据后操作！");
            }
            sim.STAY6 = sim.NUM;
            sim.TempTime = DateTime.Now.ToString("yyyyMMdd");

        }
        #endregion


        #region 添加打印按钮和模板设置
        [ActionCommand(name = "打印单件条码", title = "打印此单据下的单件码", index = 7, icon = "icon-ok", onclick = "openFMCode", isselectrow = true)]
        public void printCode()
        {
            //生成界面方法按钮用于权限控制，本方法无代码
        }
        //[ActionCommand(name = "设置打印模板", title = "设置打印模板", index = 8, icon = "icon-ok", onclick = "SetPrintModel", isselectrow = false)]
        //public void SetPrintModel()
        //{
        //    //生成界面方法按钮用于权限控制，本方法无代码
        //}

        [WhereParameter]
        public string inOrderCode { get; set; }//入库单ID
        public string GetInsCode()
        {
            FMInSequence isq = new FMInSequence();

            StockInNoticeMaterials1 sim = new StockInNoticeMaterials1();
            IDataAction action = this.model.GetDataAction();
            string sql = "select isq.ID,isq.SEQUENCECODE from " + isq.ToString() + " isq inner join " + sim.ToString() + " sim on sim.id=isq.InNoticeMaterialsSourceID where sim.id='" + inOrderCode + "'";
            DataTable dt = action.GetDataTable(sql);
            return Acc.Contract.JSON.Serializer(dt);
        }

        [WhereParameter]
        public string DJM { get; set; }//入库单ID
        public string GetDJM()
        {
            FMInSequence isq = new FMInSequence();
            IDataAction action = this.model.GetDataAction();
            string sql = "select isq.ID,isq.SEQUENCECODE from " + isq.ToString() + " isq where SEQUENCECODE='" + DJM + "'";
            DataTable dt = action.GetDataTable(sql);
            return Acc.Contract.JSON.Serializer(dt);
        }
        #endregion

        #region 下推功能
        public string GetThisController()
        {
            return new ProduceInOrderController().ToString();
        }
        /// <summary>
        /// 单据转换
        /// </summary>
        /// <param name="ca"></param>
        /// <param name="actionItem"></param>
        /// <returns></returns>
        protected override EntityBase OnConvertItem(ControllerAssociate ca, EntityBase actionItem)
        {
            //下推前判断
            WMShelp.IsPush(this);
            StockInNoticeMaterials1 cd = actionItem as StockInNoticeMaterials1;
            EntityBase eb = base.OnConvertItem(ca, actionItem);
            if (ca.cData.name.EndsWith(GetThisController()))
                eb = PushInOrder(eb, cd);//下推
            if (ca.cData.name.EndsWith(new ProductCheckOrderController().ToString()))
                eb = PushInOrder(eb, cd);//下推
            return eb;
        }

        /// <summary>
        /// 下推判断
        /// </summary>
        /// <param name="eb"></param>
        /// <param name="cd"></param>
        /// <returns></returns>
        protected virtual EntityBase PushInOrder(EntityBase eb, StockInNoticeMaterials1 cd)
        {
            if (eb is StockInOrder)
            {
                XHY_StockInOrder ebin = eb as XHY_StockInOrder;
                string sourceCode = eb.GetAppendPropertyKey("SOURCECODE");
                ebin[sourceCode] = cd.Code;
                //string clientNo = eb.GetAppendPropertyKey("CLIENTNO");
                //ebin[clientNo] = ebin.GetForeignObject<StockInNotice>(this.model.GetDataAction()).CLIENTNO;
                ebin.STATE = 1;
                ebin.STOCKTYPE = 1;
                ebin.IsSubmited = false;
                ebin.IsReviewed = false;
                ebin.SourceID = cd.ID;
                ebin.SourceController = this.ToString();
                ebin.Code = new BillNumberController().GetBillNo(new ProduceInOrderController());
                ebin[ebin.GetAppendPropertyKey("Createdby")] = "";
                ebin[ebin.GetAppendPropertyKey("Submitedby")] = "";
                ebin.Submiteddate = DateTime.MinValue;
                ebin.Creationdate = DateTime.MinValue;
                ebin.Reviewedby = "";
                ebin.INOUTTIME = cd.ProduceTime;
                ebin.Modifiedby = "";
                ebin.Modifieddate = DateTime.MinValue;
                ebin.StayMaterials1.DataAction = this.model.GetDataAction();
                int id = cd.ID;
                ebin.StayMaterials1.GetData("id=" + id + "");
                eb = ebin;

            }
            if (eb is ProCheckMaterials)
            {
                ProCheckMaterials ebin = eb as ProCheckMaterials;
                EntityList<ProCheckMaterials> pcmList = new EntityList<ProCheckMaterials>(this.model.GetDataAction());
                pcmList.GetData("sourcecode='" + cd.Code + "'");
                if (pcmList.Count > 0)
                {
                    throw new Exception("下推完成不能重复下推");
                }
                ebin.SourceCode = cd.Code;
                string cname = eb.GetAppendPropertyKey("CHECKWARE");
                string ccname = cd.GetAppendPropertyKey("MATERIALCODE");
                ebin.CHECKWARE = cd.MATERIALCODE;
                ebin[cname] = cd[ccname];
                ebin.CheckTypeName = "2";
                ebin.IsSubmited = false;
                ebin.SourceID = cd.ID;
                ebin.SourceController = this.ToString();
                ebin.Code = new BillNumberController().GetBillNo(new ProductCheckOrderController());
                ebin[ebin.GetAppendPropertyKey("Createdby")] = "";
                ebin[ebin.GetAppendPropertyKey("Submitedby")] = "";
                ebin.Submiteddate = DateTime.MinValue;
                ebin.Creationdate = DateTime.MinValue;
                ebin.Modifiedby = "";
                ebin.Modifieddate = DateTime.MinValue;
                ebin.ProduceTime = cd.ProduceTime;
                ebin.BATCHNO = cd.BATCHNO;
                ebin.FMODEL = cd.FMODEL;
                ebin.IsOK = false;

                eb = ebin;

            }
            return eb;
        }


        /// <summary>
        /// 单据转换
        /// </summary>
        /// <returns></returns>
        protected override ControllerAssociate[] DownAssociate()
        {
            List<ControllerAssociate> list = new List<ControllerAssociate>();
            //入库通知下推入库单
            RelationOrder(list);
            return list.ToArray();
        }

        /// <summary>
        /// 入库通知-入库单(下推关联)
        /// </summary>
        /// <param name="list"></param>
        public virtual void RelationOrder(List<ControllerAssociate> list)
        {
            //控制器转换器入库通知-入库单

            ControllerAssociate b2 = new ControllerAssociate(this, PushController());
            ControllerAssociate b3 = new ControllerAssociate(this, new ProductCheckOrderController());

            //单据属性映射
            PropertyMap mapORDERID1 = new PropertyMap();
            ///单号ID --来源单号
            mapORDERID1.TargerProperty = "SourceCode";
            mapORDERID1.SourceProperty = "Code";
            b2.Convert.AddPropertyMap(mapORDERID1);

            //实体类型转换器（入库通知单-入库单）
            ConvertAssociate nTo = new ConvertAssociate();
            nTo.SourceType = typeof(StockInNoticeMaterials1);//下推来源单据子集
            nTo.TargerType = typeof(StockInOrderMaterials);//下推目标单据子集

            //实体类型转换器（入库通知单-入库单）
            ConvertAssociate nTo1 = new ConvertAssociate();
            nTo1.SourceType = typeof(StockInNoticeMaterials1);//下推来源单据子集
            nTo1.TargerType = typeof(ProCheckMaterials);//下推目标单据子集
            //YS(nTo);
            b2.AddConvert(nTo);
            b3.AddConvert(nTo1);
            list.Add(b2);
            list.Add(b3);
        }

        /// <summary>
        /// 映射
        /// </summary>
        /// <param name="c"></param>
        public virtual void YS(ConvertAssociate c)
        {
            PropertyMap mapMATERIALSID = new PropertyMap();//ID
            mapMATERIALSID.SourceProperty = "CODE";
            mapMATERIALSID.TargerProperty = "SourceCode";
            c.AddPropertyMap(mapMATERIALSID);

            PropertyMap map11 = new PropertyMap();//ID
            map11.SourceProperty = "p.ID";
            map11.TargerProperty = "SourceID";
            PropertyMap map12 = new PropertyMap();//ID
            map12.TargerProperty = "SourceController";
            map12.IsValue = true;
            map12.Value = this.ToString();

            PropertyMap map13 = new PropertyMap();//ID
            map13.SourceProperty = "ID";
            map13.TargerProperty = "SourceRowID";

            PropertyMap map14 = new PropertyMap();//ID
            map14.SourceProperty = "p.Code";
            map14.TargerProperty = "SourceCode";

            PropertyMap map15 = new PropertyMap();//ID
            map15.TargerProperty = "SourceName";
            map15.IsValue = true;
            map15.Value = ((IController)this).ControllerName();

            PropertyMap map16 = new PropertyMap();//ID
            map16.TargerProperty = "SourceTable";
            map16.IsValue = true;
            map16.Value = new StockInNoticeMaterials1().ToString();

            c.AddPropertyMap(map11);
            c.AddPropertyMap(map12);
            c.AddPropertyMap(map13);
            c.AddPropertyMap(map14);
            c.AddPropertyMap(map15);
            c.AddPropertyMap(map16);
        }


        #endregion
    }
}
