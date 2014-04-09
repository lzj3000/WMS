using System;
using System.Collections.Generic;
using System.Linq;
using Acc.Business.Controllers;
using Acc.Business.WMS.Model;
using Acc.Contract.Center;
using Acc.Contract.Data.ControllerData;
using Acc.Contract.MVC;
using Way.EAP.DataAccess.Entity;
using Acc.Business.WMS.Controllers;
using Acc.Contract.Data;
using Way.EAP.DataAccess.Regulation;
using Acc.Contract;
using Acc.Business.Model;
using System.Data;
using Acc.Business.WMS.XHY.Model;
namespace Acc.Business.WMS.XHY.Controllers
{
    public class PickOutOrderController : XHY_BusinessOutOrderController
    {
        public PickOutOrderController() : base(new XHY_StockOutOrder()) { }
        /// <summary>
        /// 描述：新华杨生产出库控制器
        /// 作者：柳强
        /// 创建日期:2013-10-09
        /// </summary>
        
        #region 基础设置
      
        //显示在菜单
        protected override string OnControllerName()
        {
            return "生产出库";
        }
        //菜单中url路径
        protected override string OnGetPath()
        {
            return "Views/WMS/XHY/PickOutOrder.htm";
        }
     
        //说明
        protected override string OnGetControllerDescription()
        {
            return "生产出库";
        }

        /// <summary>
        /// 此单据启用下推
        /// </summary>
        public override bool IsPushDown
        {
            get
            {
                return true;
            }
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
           
            if (data.name.EndsWith("StockOutOrder"))
            {
                if (item.IsField("SOURCECODE"))
                {
                    if (item.foreign != null)
                    {
                        item.foreign.rowdisplay.Add("STAY5", "STAY5");
                    }
                }
                data.title = "生产出库";
                switch (item.field.ToLower())
                {
                    case "code":
                        item.visible = true;
                        item.disabled = true;
                        item.title = "生产出库单号";
                        break;
                    case "state":
                    case "sourcename":
                    case "sourceoutcode":
                    case "workerid":
                    case "bumen":
                    case "stocktype":
                    case "clientno":
                    case "stay8":
                    case "stay9":
                    case "stay10":
                    case "logcode":
                    case "k3outordertype":
                    case "lxraddress":
                    case "lxrname":
                    case "lxrphone":
                    case "zdkh":
                    case "ywdw":
                    case "khlx":
                    case "jhfs":
                    case "yf":
                    case "zdlxr":
                    case "zdtel":
                    case "zdphone":
                    case "zdaddress":
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
                    case "sourcecode":
                        item.title = "生产领用通知单号";
                        break;
                    case "stay5":
                        item.visible = true;
                        item.title = "车间";
                        item.index = 3;
                        item.required = true;
                        item.disabled = true;
                        break;
                    


                }
            }
            if (data.name.EndsWith("StockOutOrderMaterials"))
            {
                data.title = "生产出库明细";
                switch (item.field.ToLower())
                {
                    
                    case "id":
                    case "code":
                    case "price":
                    case "parentid":
                    case "sourcecode":
                    case "state":
                    case "ischeckok":
                        item.visible = false;
                        item.isedit = false;
                        break;
                    case "batchno":
                        item.visible = true;
                        item.index = 4;
                        break;
                    case "depotwbs":
                        item.title = "货位编码";
                        item.required = true;
                        item.index = 5;
                        if (item.foreign.rowdisplay.ContainsKey("BATCHNO"))
                            item.foreign.rowdisplay.Remove("BATCHNO");
                        break;
                    case "portname":
                        item.title = "托盘编码";
                        item.index = 6;
                        item.required = false;
                       if (item.foreign.rowdisplay.ContainsKey("BATCHNO"))
                            item.foreign.rowdisplay.Remove("BATCHNO");
                        break;
                    case "num":
                        item.index = 7;
                        break;
                }
                if (item.IsField("materialcode"))
                {
                    if (item.foreign != null)
                    {
                        item.foreign.rowdisplay.Add("STAY2".ToLower(), "batchno");
                    }
                }
            }
            if (data.name.EndsWith("OutInSequence"))
            {
                data.visible = false;
            }
            if (data.name.EndsWith("StockOutNoticeMaterials"))
            {
                switch (item.field.ToLower())
                {
                    case "parentid":
                        item.foreign.isfkey = true;
                        item.foreign.filedname = item.field;
                        item.foreign.objectname = data.name;
                        item.foreign.foreignobject = typeof(XHY_StockOutOrder).FullName;
                        item.foreign.foreignfiled = ("SourceID").ToUpper();
                        item.foreign.tablename = new XHY_StockOutOrder().ToString();
                        item.foreign.parenttablename = new StockOutNoticeMaterials().ToString();
                        item.visible = false;

                        break;
                    case "staynum":
                        item.title = "待操作数";
                        break;
                    case "finishnum":
                        item.title = "已操作数";
                        break;
                    case "batchno":
                        item.visible = true;
                        break;
                }
            }
        }

        #endregion

        #region 重写方法 
        protected override void OnAdding(ControllerBase.SaveEvent item)
        {
            XHY_StockOutOrder sio = item.Item as XHY_StockOutOrder;
            //base.ValidataCheckOrder(sio);
            base.OnAdding(item);
         
        }

        /// <summary>
        /// 保存和编辑是不验证是否已经质检合格
        /// </summary>
        /// <param name="so"></param>
        public override void ValidataCheckOrder(StockOutOrder so)
        {
            
        }
        protected override void OnEditing(ControllerBase.SaveEvent item)
        {
            XHY_StockOutOrder sio = item.Item as XHY_StockOutOrder;
           // base.ValidataCheckOrder(sio);
            base.OnEditing(item);
           
        }
        /// <summary>
        /// 验证出库产品是否通过质检
        /// </summary>
        /// <param name="so"></param>
        private void ValidateCheckOrderMaterials()
        {
            ProCheckMaterials pm = new ProCheckMaterials();
            DataTable dt = null;
            StockOutOrder so = base.getinfo(this.ActionItem["ID"].ToString()) as StockOutOrder;
            so.Materials.DataAction = this.model.GetDataAction();
            so.Materials.GetData();
            for (int i = 0; i < so.Materials.Count; i++)
            {
                
                ///验证出库产品是否需要质检
                if (so.Materials[i].PORTNAME != "0" && !string.IsNullOrEmpty(so.Materials[i].PORTNAME) )
                {
                    Materials ms = so.Materials[i].GetForeignObject<Materials>(this.model.GetDataAction());
                    if (ms.ISCHECKPRO == true)
                    {
                        string msql = "select * from " + pm.ToString() + " pm where pm.BATCHNO ='" + so.Materials[i].BATCHNO + "' and pm.CHECKWARE = '" + so.Materials[i].MATERIALCODE + "'";
                        dt = this.model.GetDataAction().GetDataTable(msql);
                        if (dt.Rows.Count == 0)
                        {
                            throw new Exception("异常：序号：" + so.Materials[i].RowIndex + " 出库产品编码" + so.Materials[i].MCODE + " 批次不存在质检单中");
                        }
                        if (Convert.ToInt32(dt.Rows[0]["IsOk"]) == 0)
                        {
                            throw new Exception("异常：序号" + so.Materials[i].RowIndex + " 产品编码" + so.Materials[i].MCODE + " 出库批次未质检通过");
                        }
                    }
                }
            }
        }

        protected override void OnSubmitData(BasicInfo info)
        {
            ValidateCheckOrderMaterials();
            base.OnSubmitData(info);
        }

        protected override void OnForeignIniting(IModel model, InitData data)
        {

            if (this.fdata.filedname == "SOURCECODE")
            {
                ProduceOutNoticeOrderController pnoc = new ProduceOutNoticeOrderController();
                data.modeldata = pnoc.Idata.modeldata;
            }
            else {
                base.OnForeignIniting(model, data);
            }
        }
        /// <summary>
   /// 设置源单为生产领用通知单
   /// </summary>
   /// <returns></returns>
        public override string OutOrderController()
        {
            return new ProduceOutNoticeOrderController().ToString();
        }

        /// <summary>
        /// 设置stockType=2
        /// </summary>
        /// <returns></returns>
        public override int SetStockType()
        {
            return 2;
        }
        protected override void foreignOutOrder(IModel model, loadItem item)
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
                file="a"+file;
               
                W = "and  " + file + "  " + symol + "   " + value;
            }
            if (this.fdata.filedname == "SOURCECODE")
            {
                
                //StockOutNotice son = new StockOutNotice();
                //item.rowsql = "select * from (" + item.rowsql + ") a where a.Code in(select Code from " + son.ToString() + " where stocktype = " + new ProduceOutNoticeOrderController().SetStockType() + " and IsSubmited=1)";
                ProduceOutNoticeOrderController pnoc = new ProduceOutNoticeOrderController();
                string sql = pnoc.model.GetSearchSQL();
                item.rowsql = "select * from  (" + sql + ")  a where  IsSubmited=1 and a.STOCKTYPE=" + pnoc.SetStockType() + "   " + W;
            }
            else
            {
                base.foreignOutOrder(model, item);
            }
        }
        protected override void foreignOutOrderMaterials(IModel model, loadItem item)
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
            if (this.ActionItem["SourceCode"].ToString()== "") {
                throw new Exception("生产领用通知单号不可以为空");
            }
            ///选择可用的产品
            if (this.fdata.filedname == "MATERIALCODE" && this.ActionItem["SourceCode"] != null)
            {
                //item.rowsql = "select * from (" + item.rowsql + ") a where a.CommodityType!=2 and a.isdisable=0 and a.id in(select MATERIALCODE from " + new StockOutNoticeMaterials().ToString() + " sim INNER JOIN " + new StockOutNotice().ToString() + " si on si.ID= sim.PARENTID  where si.code ='" + this.ActionItem["SourceCode"] + "')";
                item.rowsql = string.Format("select c.ID,c.FNAME,c.FMODEL,c.Code,b.BATCHNO STAY2 from {0} a left join {1} b on a.ID=b.PARENTID left join {2} c on b.MATERIALCODE=c.ID where a.Code='{3}' " + W + "", new StockOutNotice().ToString(), new StockOutNoticeMaterials().ToString(), this.fdata.tablename, this.ActionItem["SourceCode"]);
            }
            else {
                base.foreignOutOrderMaterials(model, item);
            }
        }

        public override string GetNoticeStayGrid()
        {
            return base.GetNoticeStayGrid();
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
            XHY_StockOutOrder cd = actionItem as XHY_StockOutOrder;
            EntityBase eb = base.OnConvertItem(ca, actionItem);
            if (ca.cData.name.EndsWith(new ReturnInOrderController().ToString()))
                eb = PushInOrder(eb, cd);//下推
            return eb;
        }

        /// <summary>
        /// 下推判断
        /// </summary>
        /// <param name="eb">转换为的</param>
        /// <param name="cd">要转换的</param>
        /// <returns></returns>
        protected EntityBase PushInOrder(EntityBase eb, StockOutOrder cd)
        {
            if (eb is StockInOrder)
            {
                StockInOrder ebin = eb as StockInOrder;
                ebin.SourceCode = cd.Code;
                string sourceCode = eb.GetAppendPropertyKey("SOURCECODE");
                ebin[sourceCode] =cd.Code;
                ebin.STAY5 = cd.STAY5;
                string STAY5 = eb.GetAppendPropertyKey("STAY5");
                ebin[STAY5] = ebin.GetForeignObject<Organization>(this.model.GetDataAction()).OrganizationName;
                ebin.STOCKTYPE = 3;
                ebin.IsSubmited = false;
                ebin.IsReviewed = false;
                ebin.SourceID = cd.ID;
                ebin.SourceController = this.ToString();
                ebin.Code = new BillNumberController().GetBillNo(new ReturnInOrderController());
                ebin[ebin.GetAppendPropertyKey("Createdby")] = "";
                ebin[ebin.GetAppendPropertyKey("Submitedby")] = "";
                ebin[ebin.GetAppendPropertyKey("TOWHNO")] = "";
                ebin.Submiteddate = DateTime.MinValue;
                ebin.Creationdate = DateTime.MinValue;
                ebin.Reviewedby = "";
                ebin.Revieweddate = DateTime.MinValue;
                ebin.Modifiedby = "";
                ebin.Modifieddate = DateTime.MinValue;
                ebin.Materials.RemoveAll(delegate(StockInOrderMaterials m)
                {
                    return true;
                });
                cd.Materials.DataAction = this.model.GetDataAction();
                cd.Materials.GetData();
                for (int i = 0; i < cd.Materials.Count; i++)
                {
                    StockInOrderMaterials aa = new StockInOrderMaterials();
                    aa.BATCHNO = cd.Materials[i].BATCHNO;
                    string aname = aa.GetAppendPropertyKey("MATERIALCODE");
                    string cname = cd.Materials[i].GetAppendPropertyKey("MATERIALCODE");
                    aa.MATERIALCODE = cd.Materials[i].MATERIALCODE;
                    aa[aname] = cd.Materials[i][cname];
                    aa.NUM = cd.Materials[i].NUM;
                    aa.MCODE = cd.Materials[i].MCODE;
                    aa.FMODEL = cd.Materials[i].FMODEL;
                    //aa.SourceController = this.ToString();
                    //aa.SourceID = cd.ID;
                    aa.SourceRowID = cd.Materials[i].ID;
                    aa.SourceTable = "Acc_WMS_OutOrderMaterials";
                    ebin.Materials.Add(aa);
                }
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
        public void RelationOrder(List<ControllerAssociate> list)
        {
            //控制器转换器入库通知-入库单

            ControllerAssociate b2 = new ControllerAssociate(this, new ReturnInOrderController());

            //单据属性映射
            PropertyMap mapORDERID1 = new PropertyMap();
            ///单号ID --来源单号
            mapORDERID1.TargerProperty = "SourceCode";
            mapORDERID1.SourceProperty = "Code";
            b2.Convert.AddPropertyMap(mapORDERID1);

            //实体类型转换器（出库通知单-出库单）
            ConvertAssociate nTo = new ConvertAssociate();
            nTo.SourceType = typeof(StockOutOrderMaterials);//下推来源单据子集
            nTo.TargerType = typeof(StockInOrderMaterials);//下推目标单据子集

            //YS(nTo);
            //b2.AddConvert(nTo);
            list.Add(b2);
        }

        /// <summary>
        /// 映射
        /// </summary>
        /// <param name="c"></param>
        public void YS(ConvertAssociate c)
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
            map16.Value = new StockOutOrderMaterials().ToString();

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
