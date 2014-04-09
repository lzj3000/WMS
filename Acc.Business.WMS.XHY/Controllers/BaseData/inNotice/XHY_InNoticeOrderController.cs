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
using Acc.Business.Model;
using Acc.Contract.Data.ControllerData;

namespace Acc.Business.WMS.XHY.Controllers
{
    public class XHY_InNoticeOrderController : StockInNoticeController
    {
        public XHY_InNoticeOrderController() : base(new StockInNotice()) { }
        public XHY_InNoticeOrderController(IModel model) : base(model) { }
        #region 页面设置

        /// <summary>
        /// 启用下推
        /// </summary>
        public override bool IsPushDown
        {
            get { return true; }
        }
        /// <summary>
        /// 禁用审核
        /// </summary>
        public override bool IsReviewedState
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
        #endregion
        protected override void OnForeignIniting(IModel model, InitData data)
        {
            int aa = SetStockType();
            if(model is StockInNotice){
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
            if (model is StockInNoticeMaterials)
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
            }
        }
        #region 初始化方法
        /// <summary>
        /// 初始化显示与否
        /// </summary>
        /// <param name="data"></param>
        /// <param name="item"></param>
        protected override void OnInitViewChildItem(Contract.Data.ModelData data, Contract.Data.ItemData item)
        {

            base.OnInitViewChildItem(data, item);
            if (data.name.EndsWith("StockInNotice"))
            {
                switch (item.field.ToLower())
                {
                    case "rowindex":
                    case "code":
                        item.visible = true;
                        item.disabled = true;
                        break;
                    case "isdisable":
                    case "stay1":
                    case "stay2":
                    case "stay3":
                    case "stay4":
                    case "stay5":
                    case "issynchronous":
                        item.visible = false;
                        break;

                }
            }
            if (data.name.EndsWith("StockInNoticeMaterials"))
            {
                switch (item.field.ToLower())
                {
                    case "funitid":
                    case "isdisable":
                        item.visible = false;
                        break;
                    case "rowindex":
                        item.disabled = true;
                        item.visible = true;
                        break;
                    case "num":
                    case "finishnum":
                        item.visible = true;
                        item.title = "已操作数";
                        break;
                    case "staynum":
                    item.visible = true;
                    item.title = "待操作数";
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
                    default:
                        item.visible = false;
                        break;
                }
            }
            if (data.name.EndsWith("InSequence"))
            {
                data.visible = false;
            }
        }
        #endregion

        #region 重写方法
        /// <summary>
        /// 添加不同类型
        /// </summary>
        /// <param name="item"></param>
        /// <param name="stockType"></param>
        public virtual int SetStockType()
        {
            return 0;
        }

        /// <summary>
        /// 获取当前控制器名称
        /// </summary>
        /// <returns></returns>
        public virtual string GetThisController()
        {
            return "";
        }

        /// <summary>
        /// 指定下推对象
        /// </summary>
        /// <returns></returns>
        public virtual ControllerBase PushController()
        {
            return new XHY_InNoticeOrderController();
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
        /// 查询按钮，并添加查询条件
        /// </summary>
        /// <param name="m"></param>
        /// <param name="where"></param>
        protected override void OnGetWhereing(IModel m, List<SQLWhere> where)
        {
            base.OnGetWhereing(m, where);
            if (m is StockInNotice)
            {
                SQLWhere w = new SQLWhere();
                w.ColumnName = "STOCKTYPE";
                w.Value = SetStockType().ToString();
                where.Add(w);
            }
        }

        protected override void OnForeignLoading(IModel model, Contract.Data.ControllerData.loadItem item)
        {
            base.OnForeignLoading(model, item);
            ///选择客户不包括已禁用的
            if (model is StockInNotice)
            {
                if (this.fdata.filedname == "CLIENTNO")
                {
                    item.rowsql = "select * from (" + item.rowsql + ") a where a.isdisable =0";
                }
            }
            //if (model is StockInNoticeMaterials)
            //{
            //    if (this.fdata.filedname == "MATERIALCODE")
            //    {
            //        item.rowsql = "select id,code,FNAME,FFULLNAME,ClassID,FUNITID,BATCH,STATUS,ISOVERIN,IsDisable,ISCHECKPRO,Remark from " + new BusinessCommodity().ToString() + " a where a.CommodityType=" + SetStockType();
            //    }
            //}
        }

        protected override void OnAdding(ControllerBase.SaveEvent item)
        {
            base.OnAdding(item);
            StockInNotice on = item.Item as StockInNotice;
            if (on != null)
            {
                on.STOCKTYPE = SetStockType();
            }

        }
        protected override void OnEditing(ControllerBase.SaveEvent item)
        {
            EditValidateBatchNo(item);
            base.OnEditing(item);
            StockInNotice on = item.Item as StockInNotice;
            if (on != null)
            {
                on.STOCKTYPE = SetStockType();
            }
        }

        protected override void OnSubmitData(BasicInfo info)
        {
            StockInNotice sin = info as StockInNotice;
            sin.Materials.DataAction = this.model.GetDataAction();
            sin.Materials.GetData();
            if (sin.Materials.Count == 0)
            {
                throw new Exception("单据无明细数据不能提交！");
            }
            base.OnSubmitData(info);
        }

        private void ValidateBatchNo(ControllerBase.SaveEvent item)
        {
            StockInNotice on = item.Item as StockInNotice;
            Vali(on);

        }

        private void Vali(StockInNotice on)
        {
            EntityList<Materials> ms = new EntityList<Materials>(this.model.GetDataAction());

            for (int i = 0; i < on.Materials.Count; i++)
            {
                ms.GetData("id='" + on.Materials[i].MATERIALCODE + "'");
                if (ms[0].BATCH == false)
                {
                    if (on.Materials[i].BATCHNO != "")
                    {
                        throw new Exception("产品名称：" + ms[0].FNAME + "  产品编码：" + ms[0].Code + "不允许批次管理，不可录入批次");
                    }
                }
            }
        }
        private void EditValidateBatchNo(ControllerBase.SaveEvent item)
        {
            StockInNotice on = item.Item as StockInNotice;
            if (on.Materials.Count == 0)
            {
                on.Materials.DataAction = this.model.GetDataAction();
                on.Materials.GetData();
            }
            Vali(on);

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
                if (ac.command == "UnSubmitData")
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
            }
            return coms;
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
            StockInNotice cd = actionItem as StockInNotice;
            cd.Materials.DataAction = this.model.GetDataAction();
            cd.Materials.GetData();//获取子集的数据
            EntityBase eb = base.OnConvertItem(ca, actionItem);
            if (ca.cData.name.EndsWith(GetThisController()))
                eb = PushInOrder(eb, cd);//下推
            return eb;
        }

        public virtual BusinessController bc()
        {
            return new BusinessController();
        }
        /// <summary>
        /// 下推判断
        /// </summary>
        /// <param name="eb"></param>
        /// <param name="cd"></param>
        /// <returns></returns>
        protected virtual EntityBase PushInOrder(EntityBase eb, StockInNotice cd)
        {
            if (eb is StockInOrder)
            {
                StockInOrder ebin = eb as StockInOrder;
                string sourceCode = eb.GetAppendPropertyKey("SOURCECODE");
                ebin[sourceCode] = ebin.GetForeignObject<StockInNotice>(this.model.GetDataAction()).Code;
                //string clientNo = eb.GetAppendPropertyKey("CLIENTNO");
                //ebin[clientNo] = ebin.GetForeignObject<StockInNotice>(this.model.GetDataAction()).CLIENTNO;
                string cname = eb.GetAppendPropertyKey("CLIENTNO");
                string ccname = cd.GetAppendPropertyKey("CLIENTNO");
                ebin.CLIENTNO = cd.CLIENTNO;
                ebin[cname] = cd[ccname];
                ebin.STATE = 1;
                ebin.STOCKTYPE = SetStockType();
                ebin.IsSubmited = false;
                ebin.IsReviewed = false;
                ebin.SourceID = cd.ID;
                ebin.SourceController = this.ToString();
                ebin.Code = new BillNumberController().GetBillNo(bc());
                ebin[ebin.GetAppendPropertyKey("Createdby")] = "";
                ebin[ebin.GetAppendPropertyKey("Submitedby")] = "";
                ebin.Submiteddate = DateTime.MinValue;
                ebin.Creationdate = DateTime.MinValue;
                ebin.Reviewedby = "";
                ebin.Revieweddate = DateTime.MinValue;
                ebin.Modifiedby = "";
                ebin.Modifieddate = DateTime.MinValue;
                ebin.StayMaterials.DataAction = this.model.GetDataAction();
                int id = ebin.GetForeignObject<StockInNotice>(this.model.GetDataAction()).ID;
                ebin.StayMaterials.GetData("ParentId=" + id + "");
              
                //EntityList<Materials> materials = new EntityList<Materials>(this.model.GetDataAction());

                //for (int i = 0; i < ebin.StayMaterials.Count; i++)
                //{
                //    materials.GetData("id=" + ebin.StayMaterials[i].MATERIALCODE + "");
                //    ebin.StayMaterials[i].MCODE = materials[0].Code;
                //    ebin.StayMaterials[i].FMODEL = materials[0].FMODEL;
                //}
                ebin.Materials.RemoveAll(delegate(StockInOrderMaterials sim)
                {
                    return true;
                });
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

            //单据属性映射
            PropertyMap mapORDERID1 = new PropertyMap();
            ///单号ID --来源单号
            mapORDERID1.TargerProperty = "SourceCode";
            mapORDERID1.SourceProperty = "Code";
            b2.Convert.AddPropertyMap(mapORDERID1);

            //实体类型转换器（入库通知单-入库单）
            ConvertAssociate nTo = new ConvertAssociate();
            nTo.SourceType = typeof(StockInNoticeMaterials);//下推来源单据子集
            nTo.TargerType = typeof(StockInOrderMaterials);//下推目标单据子集
            YS(nTo);
            b2.AddConvert(nTo);
            list.Add(b2);
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
            map16.Value = new StockInNoticeMaterials().ToString();

            c.AddPropertyMap(map11);
            c.AddPropertyMap(map12);
            c.AddPropertyMap(map13);
            c.AddPropertyMap(map14);
            c.AddPropertyMap(map15);
            c.AddPropertyMap(map16);
        }
        //通知单下推，选择其他原单时改变待入库明细的数据
        [WhereParameter]
        public string NoticeId { get; set; }
        public virtual string GetNoticeStayGrid()
        {
            EntityList<StockInNotice> List = new EntityList<StockInNotice>(this.model.GetDataAction());
            List.GetData("Code='" + NoticeId + "'");
            EntityList<StockInNoticeMaterials> List1 = new EntityList<StockInNoticeMaterials>(this.model.GetDataAction());
            List1.GetData("PARENTID='" + List[0].ID + "'");
            string str = JSON.Serializer(List1.ToArray());
            return str;
        }
        #endregion
    }
}
