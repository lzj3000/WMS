using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Contract.Data;
using Acc.Business.Controllers;
using Acc.Business.WMS.Controllers;
using Way.EAP.DataAccess.Entity;
using Acc.Business.WMS.Model;
using Acc.Contract.MVC;
using Way.EAP.DataAccess.Data;
using System.Data;

namespace Acc.Business.WMS.XHY.Controllers
{
    public class XHY_StockInfoMaterialsController : StockInfoMaterialsController
    {
        public XHY_StockInfoMaterialsController() : base(new StockInfoMaterials()) { }
        //public class XHY_StockInfoMaterialsDR : ExcelDRController {   
        //    //public  override string Import()
        //    //{
        //    //   return base.Import();
        //    //}
        //}

        public override bool IsPrint
        {
            get
            {
                return false;
            }
        }

        public override bool IsPushDown
        {
            get
            {
                return true;
            }
        }

        //菜单中url路径
        protected override string OnGetPath()
        {
            return "Views/WMS/XHY/XHY_StockInfoMaterials.htm";
        }
        /// <summary>
        /// 禁用回收站
        /// </summary>
        public override bool IsClearAway
        {
            get
            {
                return false;
            }
        }
        [ActionCommand(name = "导入", title = "导入", index = 10, icon = "icon-down", isselectrow = false, onclick = "import1")]
        public void Import()
        {
            //ExcelDRController er = n;ew ExcelDRController();
            //er.bc = this;

        }


        protected override void OnInitViewChildItem(Contract.Data.ModelData data, Contract.Data.ItemData item)
        {
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
            if (data.name.EndsWith("StockInfoMaterials"))
            {
                data.disabled = true;
                switch (item.field.ToLower())
                {
                    case "code":
                    case "creationdate":
                        item.visible = true;
                        break;
                    //case "WAREHOUSEID":
                    //    item.title = "仓库编码";
                    //    break;
                    case "salearea":
                    case "issubmited":
                    case "submitedby":
                    case "reviewedby":
                    case "revieweddate":
                    case "freezenum":
                    case "isdisable":
                    case "createdby":
 
                    case "submiteddate":
                    case "isreviewed":
                    case "senbatchno":
                        item.visible = false;
                        break;
                    case "status":
                        item.title = "质检状态";
                        item.index = 7;
                        item.visible = true;
                        break;
                    case "status1":
                        item.title = "是否过期";
                        item.index = 12;
                        item.searchstate = false;
                        item.visible = true;
                        break;
                    case "islock":
                        item.visible = false;
                        break;
                    case "depotwbs":
                        item.index = 6;
                        break;
                    case "portcode":
                        item.index = 7;
                        break;

                }
            }
        }
        public string RowSql()
        {
           return "SELECT	Acc_WMS_InfoMaterials.[Code] ,Acc_WMS_InfoMaterials.[MCODE] ,Acc_WMS_InfoMaterials.[FMODEL] ,		Acc_WMS_InfoMaterials.[ISLOCK] ,		Acc_WMS_InfoMaterials.[BATCHNO] ,		Acc_WMS_InfoMaterials.[SENBATCHNO] ,		Acc_WMS_InfoMaterials.[WAREHOUSEID] ,		Acc_WMS_InfoMaterials.[DEPOTWBS] ,		Acc_WMS_InfoMaterials.[NUM] ,		Acc_WMS_InfoMaterials.[LASTINTIME] ,		Acc_WMS_InfoMaterials.[LASTOUTTIME] ,		Acc_WMS_InfoMaterials.[STATUS] ,		Acc_WMS_InfoMaterials.[SALEAREA] ,		Acc_WMS_InfoMaterials.[PORTCODE] ,		Acc_WMS_InfoMaterials.[ORDERNO] ,		Acc_WMS_InfoMaterials.[FREEZENUM] ,		Acc_WMS_InfoMaterials.[IsSubmited] ,		Acc_WMS_InfoMaterials.[Submiteddate] ,		Acc_WMS_InfoMaterials.[Submitedby] ,		Acc_WMS_InfoMaterials.[Createdby] ,		Acc_WMS_InfoMaterials.[Creationdate] ,		Acc_WMS_InfoMaterials.[Modifiedby] ,		Acc_WMS_InfoMaterials.[Modifieddate] ,		Acc_WMS_InfoMaterials.[Reviewedby] ,		Acc_WMS_InfoMaterials.[Revieweddate] ,		Acc_WMS_InfoMaterials.[IsReviewed] ,		Acc_WMS_InfoMaterials.[IsDisable] ,		Acc_WMS_InfoMaterials.[IsDelete] ,		Acc_WMS_InfoMaterials.[Remark] ,		Acc_WMS_InfoMaterials.[STAY1] ,		Acc_WMS_InfoMaterials.[STAY2] ,		Acc_WMS_InfoMaterials.[STAY3] ,		Acc_WMS_InfoMaterials.[STAY4] ,		Acc_WMS_InfoMaterials.[STAY5] ,		Acc_WMS_InfoMaterials.[ID] ,		f1.[FNAME] f1 ,		f2.[WAREHOUSENAME] f2 ,		f3.[Code] f3 ,		f4.[PORTNO] f4 ,		f5.[WorkName] f5 ,		f6.[WorkName] f6 ,		f7.[WorkName] f7 ,		f8.[WorkName] f8,		CASE WHEN CAST(DATEDIFF(dd, Acc_WMS_InfoMaterials.LASTINTIME,								GETDATE()) AS INTEGER) <= ISNULL(f1.shelflife,															  0) THEN '否'			 WHEN ISNULL(f1.shelflife,0) >0 AND CAST(DATEDIFF(dd, Acc_WMS_InfoMaterials.LASTINTIME,								GETDATE()) AS INTEGER) > ISNULL(f1.shelflife,															  0) THEN '是'															  ELSE '否' END		AS status1 FROM	Acc_WMS_InfoMaterials	LEFT JOIN Acc_Bus_BusinessCommodity f1 ON Acc_WMS_InfoMaterials.[Code] = f1.[ID]		LEFT JOIN Acc_WMS_WareHouse f2 ON Acc_WMS_InfoMaterials.[WAREHOUSEID] = f2.[ID]		LEFT JOIN Acc_WMS_WareHouse f3 ON Acc_WMS_InfoMaterials.[DEPOTWBS] = f3.[ID]		LEFT JOIN Acc_WMS_Ports f4 ON Acc_WMS_InfoMaterials.[PORTCODE] = f4.[ID]		LEFT JOIN Acc_Bus_OfficeWorker f5 ON Acc_WMS_InfoMaterials.[Submitedby] = f5.[ID]		LEFT JOIN Acc_Bus_OfficeWorker f6 ON Acc_WMS_InfoMaterials.[Createdby] = f6.[ID]		LEFT JOIN Acc_Bus_OfficeWorker f7 ON Acc_WMS_InfoMaterials.[Modifiedby] = f7.[ID]		LEFT JOIN Acc_Bus_OfficeWorker f8 ON Acc_WMS_InfoMaterials.[Reviewedby] = f8.[ID] ";
        }
        protected override Contract.Data.ControllerData.ReadTable OnSearchData(Contract.Data.ControllerData.loadItem item)
        {
            item.rowsql = "select a.*,CASE WHEN CAST(DATEDIFF(dd, a.LASTINTIME,GETDATE()) AS INTEGER) <= ISNULL(b.shelflife, 0) THEN '否'WHEN ISNULL(b.shelflife, 0) > 0 AND CAST(DATEDIFF(dd, a.LASTINTIME,GETDATE()) AS INTEGER) > ISNULL(b.shelflife,0) THEN '是' ELSE '否' END AS status1 from (" + item.rowsql + ") a INNER JOIN dbo.Acc_Bus_BusinessCommodity b ON a.Code= b.ID";
            return base.OnSearchData(item);
        }

        #region 下推

        public string GetThisController()
        {
            return new ReCheckOrderController().ToString();
        }
        /// <summary>
        /// 设置下推对象
        /// </summary>
        /// <returns></returns>
        public ControllerBase PushController()
        {
            return new ReCheckOrderController();
        }

        public void IsPush(ControllerBase sin)
        {
            IDataAction action = this.model.GetDataAction();
            StockInfoMaterials info = base.getinfo(this.ActionItem["ID"].ToString()) as StockInfoMaterials;
            string sql = RowSql() + " where Acc_WMS_InfoMaterials.id='" + info.ID + "' ";
            if (action.GetDataTable(sql).Rows[0]["status1"].ToString() == "否")
            {
                throw new Exception("异常：未过期产品不能进行下推");
            }
            ProCheckMaterials pc = new ProCheckMaterials();
            string prochecksql = "select id from "+pc.ToString()+" where sourceid='"+info.ID+"' and sourcecontroller='"+this.ToString()+"'";
            DataTable dt = action.GetDataTable(prochecksql);
            if(dt.Rows.Count>0)
            {
                throw new Exception("异常：该库存记录已下推到复检单不能重复下推");
            }
        }
        #endregion

        /// <summary>
        /// 单据转换
        /// </summary>
        /// <param name="ca"></param>
        /// <param name="actionItem"></param>
        /// <returns></returns>
        protected override EntityBase OnConvertItem(ControllerAssociate ca, EntityBase actionItem)
        {
            //下推前判断
            IsPush(this);
            StockInfoMaterials cd = actionItem as StockInfoMaterials;
            EntityBase eb = base.OnConvertItem(ca, actionItem);
            if (ca.cData.name.EndsWith(GetThisController()))
                eb = PushInOrder(eb, cd);//下推
           
            return eb;
        }
        /// <summary>
        /// 下推判断
        /// </summary>
        /// <param name="eb">转换为的</param>
        /// <param name="cd">要转换的</param>
        /// <returns></returns>
        protected EntityBase PushInOrder(EntityBase eb, StockInfoMaterials cd)
        {
            if (eb is ProCheckMaterials)
            {
                ProCheckMaterials ebin = eb as ProCheckMaterials;
               
                ebin.CheckTypeName = "3";
                ebin.IsSubmited = false;
                ebin.IsReviewed = false;
                ebin.SourceID = cd.ID;
                ebin.SourceController = this.ToString();
                ebin.Code = new BillNumberController().GetBillNo(new ReCheckOrderController());
                string MATERIALCODE = cd.GetAppendPropertyKey("Code");
                ebin[MATERIALCODE] = cd.GetForeignObject<Materials>(this.model.GetDataAction()).FNAME;
                ebin.CHECKWARE = cd.Code;
                ebin.MCODE = cd.MCODE;
                ebin.FMODEL = cd.FMODEL;
                ebin.BATCHNO = cd.BATCHNO;
                ebin.PortNo = cd.PORTCODE;
                ebin.Depotwbs = cd.DEPOTWBS;
                ebin.Remark = "最后操作：库存管理下推";
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

            ControllerAssociate b2 = new ControllerAssociate(this, PushController());
        
            //单据属性映射
            PropertyMap mapORDERID1 = new PropertyMap();
            ///单号ID --来源单号
            mapORDERID1.TargerProperty = "SourceCode";
            mapORDERID1.SourceProperty = "Code";
            b2.Convert.AddPropertyMap(mapORDERID1);

            //实体类型转换器（出库通知单-出库单）
            ConvertAssociate nTo = new ConvertAssociate();
            nTo.SourceType = typeof(StockOutNoticeMaterials);//下推来源单据子集
            nTo.TargerType = typeof(StockOutOrderMaterials);//下推目标单据子集
            b2.AddConvert(nTo);
            list.Add(b2);
        }

        protected override void OnForeignLoading(IModel model, Contract.Data.ControllerData.loadItem item)
        {
            base.OnForeignLoading(model, item);
            if (this.fdata.filedname == "WAREHOUSEID")
            {
                item.rowsql = "select * from (" + item.rowsql + ") a where exists(select id from acc_wms_warehouse where a.WAREHOUSENAME=WAREHOUSENAME and whtype=0)";
            }
            if (this.fdata.filedname == "DEPOTWBS")
            {
                item.rowsql = "select * from (" + item.rowsql + ") a where exists(select id from acc_wms_warehouse where a.WAREHOUSENAME=WAREHOUSENAME and whtype=2)";
            }
        }
     
    }
}
