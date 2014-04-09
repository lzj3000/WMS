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

namespace Acc.Business.WMS.XHY.Controllers
{
    public class ProduceOutNoticeOrderController : XHY_OutNoticeOrderController
    {
        public ProduceOutNoticeOrderController() : base(new XHY_OutNoticeOrder()) { }
        #region 页面设置
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
            return "Views/WMS/XHY/ProduceOutOrderNotice.htm";
        }

        protected override string OnControllerName()
        {
            return "生产领用通知";
        }

        #endregion

        #region 初始化数据方法
        protected override void OnInitViewChildItem(Contract.Data.ModelData data, Contract.Data.ItemData item)
        {
            base.OnInitViewChildItem(data, item);
            if (data.name.EndsWith("XHY_OutNoticeOrder"))
            {
                data.title = "生产领用通知";
                switch (item.field.ToLower())
                {
                    case "code":
                        item.title = "生产领用单号";
                        item.visible = true;
                        item.disabled = true;
                        break;          
                    case "rowindex":
                        item.visible = true;
                        item.disabled = true;
                        break;                 
                    case "sourcename":
                    case "sourceoutcode":
                    case "workerid":
                    case "bumen":
                    case "stocktype":
                    case "finishtime":
                    case "isdisable":
                    case "clientno":
                    case "reviewedby":
                    case "revieweddate":
                    case "isreviewed":
                    case "state":
                    case "outrule":
                    case "sourcecode":
                    case "zdkh":
                    case "ywdw":
                    case "khlx":
                    case "jhfs":
                        item.visible = false;
                        break;
                    case "submiteddate":
                    case "submitedby":
                    case "createdby":
                    case "creationdate":
                    case "issubmited":
                        item.disabled = true;
                        break;
                    case "stay5":
                        item.visible = true;
                        item.title = "车间";
                        item.index = 2;
                        item.required = true;
                        break;


                }
            }
            if (data.name.EndsWith("StockOutNoticeMaterials"))
            {
                data.title = "生产领用通知明细";
                switch (item.field.ToLower())
                {
                    case "code":
                    case "sourcecode":
                    case "parentid":
                    case "state":////状态
                    case "createdby":
                    case "creationdate":
                        item.visible = false;
                        break;
                    case "finishnum":
                        item.title = "已操作数";
                        break;
                    case "staynum":
                        item.title = "待操作数";
                        break;
                    case "num":
                        item.title = "领用数量";
                        break;
                    case "batchno":
                        item.visible = true;
                        break;
                    case "mcode":
                        item.disabled = true;
                        break;
                    case "fmodel":
                        item.disabled = true;
                        break;
                   
                }
            }
        }
        #endregion

        #region 重写方法
        protected override void OnForeignLoading(IModel model, Contract.Data.ControllerData.loadItem item)
        {
            if (this.fdata.filedname == "MATERIALCODE")
            {
                //item.rowsql = "select * from (" + item.rowsql + ") a where a.CommodityType='0' or a.CommodityType='1'";
                item.rowsql = "select * from (" + item.rowsql + ") a where a.CommodityType='0' or a.CommodityType='1' or a.CommodityType='2'";
            }
            if (this.fdata.filedname == "STAY5")
            {
                item.rowsql = "select * from (" + item.rowsql + ") a where a.parentid in(select id from " + new Organization().ToString() + " where OrganizationName='生产中心')";
            }
            else
            {
                base.OnForeignLoading(model, item);
            }
        }
        protected override void OnAdding(ControllerBase.SaveEvent item)
        {
            XHY_OutNoticeOrder si = item.Item as XHY_OutNoticeOrder;
            if (string.IsNullOrEmpty(si.STAY5))
            {
                throw new Exception("异常：车间不能为空！");
            }
            base.OnAdding(item);
        }

        protected override void OnEditing(ControllerBase.SaveEvent item)
        {
            StockOutNotice on = item.Item as StockOutNotice;
            if (on.IsSubmited == true)
            {
                throw new Exception("异常：单据已提交不能编辑！");
            }
            base.OnEditing(item);
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


        /// <summary>
        /// 设置stocktype =1
        /// </summary>
        /// <returns></returns>
        public override int SetStockType()
        {
            return 1;
        }
        /// <summary>
        /// 设置下推对象
        /// </summary>
        /// <returns></returns>
        public override ControllerBase PushController()
        {
            return new PickOutOrderController();
        }
        #endregion

        #region 下推

        public override string GetThisController()
        {
            return new PickOutOrderController().ToString();
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
            WMShelp.IsPush(this);
            XHY_OutNoticeOrder cd = actionItem as XHY_OutNoticeOrder;
            EntityBase eb = base.OnConvertItem(ca, actionItem);
            if (ca.cData.name.EndsWith(GetThisController()))
                eb = PushInOrder(eb, cd);//下推
            //if (ca.cData.name.EndsWith(new ReturnInNoticeOrderCotroller().ToString()))
            //    eb = PushInOrder(eb, cd);//下推
            return eb;
        }
        /// <summary>
        /// 下推判断
        /// </summary>
        /// <param name="eb">转换为的</param>
        /// <param name="cd">要转换的</param>
        /// <returns></returns>
        protected override EntityBase PushInOrder(EntityBase eb, XHY_OutNoticeOrder cd)
        {
            if (eb is XHY_StockOutOrder)
            {
                XHY_StockOutOrder ebin = eb as XHY_StockOutOrder;
                string sourceCode = eb.GetAppendPropertyKey("SOURCECODE");
                ebin[sourceCode] = cd.Code;
                //string clientNo = eb.GetAppendPropertyKey("CLIENTNO");
                //ebin[clientNo] = ebin.GetForeignObject<StockOutNotice>(this.model.GetDataAction()).CLIENTNO;
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
                ebin.Code = new BillNumberController().GetBillNo(new PickOutOrderController()) ;
                ebin.Submitedby = "";
                ebin.Submiteddate = DateTime.MinValue;
                ebin.Reviewedby = "";
                ebin.Revieweddate = DateTime.MinValue;
                ebin.Modifiedby = "";
                ebin.Modifieddate = DateTime.MinValue;
                ebin.STAY3 = cd.OutRule;///将出库规则赋值到stay3上
                ebin.StayMaterials.DataAction = this.model.GetDataAction();
                int id = cd.ID;
                ebin.StayMaterials.GetData("ParentId=" + id + "");
                ///销售出库通知单重写此方法
                for (int m = 0; m < ebin.Materials.Count; m++)
                {
                    ebin.Materials.Remove(ebin.Materials[m]);
                    m--;
                }
                SellOutOrder(ebin,null);
                //ebin.Materials.RemoveAll(delegate(StockOutOrderMaterials sim)
                //{
                //    return true;
                //});
                eb = ebin;
            }
            //if (eb is StockInNotice)
            //{

            //    StockInNotice ebin = eb as StockInNotice;
            //    ebin.SourceCode = cd.Code;
            //    string sourceCode = eb.GetAppendPropertyKey("SOURCECODE");
            //    ebin[sourceCode] = ebin.GetForeignObject<StockOutNotice>(this.model.GetDataAction()).Code;
            //    ebin.STAY5 = cd.STAY5;
            //    string STAY5 = eb.GetAppendPropertyKey("STAY5");
            //    ebin[STAY5] = ebin.GetForeignObject<Organization>(this.model.GetDataAction()).OrganizationName;
            //    ebin.STOCKTYPE = 3;
            //    ebin.IsSubmited = false;
            //    ebin.IsReviewed = false;
            //    ebin.SourceID = cd.ID;
            //    ebin.SourceController = this.ToString();
            //    ebin.Code = new BillNumberController().GetBillNo(new ReturnInNoticeOrderCotroller());
            //    ebin.Submitedby = "";
            //    ebin.Submiteddate = DateTime.MinValue;
            //    ebin.Reviewedby = "";
            //    ebin.Revieweddate = DateTime.MinValue;
            //    ebin.Modifiedby = "";
            //    ebin.Modifieddate = DateTime.MinValue;
            //    ebin.Materials.RemoveAll(delegate(StockInNoticeMaterials m)
            //    {
            //        return m.NUM <= 0;
            //    });
            //    for(int  m=0;m<ebin.Materials.Count;m++)
            //    {
            //        ebin.Materials.Remove(ebin.Materials[m]);
            //        m--;
            //    }
            //    for (int  i=0;i<cd.Materials.Count;i++)
            //    {
            //        StockInNoticeMaterials aa = new StockInNoticeMaterials();
            //        aa.BATCHNO = cd.Materials[i].BATCHNO;
            //        aa.MATERIALCODE = cd.Materials[i].MATERIALCODE;
            //        string MATERIALCODE = cd.Materials[i].GetAppendPropertyKey("MATERIALCODE");
            //        aa[MATERIALCODE] = cd.Materials[i].GetForeignObject<Materials>(this.model.GetDataAction()).FNAME;
            //        aa.NUM = cd.Materials[i].NUM - aa.GetSourceNum(new XHY_OutNoticeOrder().ToString(), 0, cd.Materials[i].ID, aa.MATERIALCODE);
            //        aa.MCODE = cd.Materials[i].MCODE;
            //        aa.FMODEL = cd.Materials[i].FMODEL;
            //        aa.SourceController = this.ToString();
            //        aa.SourceID = cd.ID;
            //        aa.SourceRowID = cd.Materials[i].ID;
            //        aa.SourceTable = new XHY_OutNoticeOrder().ToString();
            //        ebin.Materials.Add(aa);
            //    }
            //    for (int m = 0; m < ebin.Materials.Count; m++)
            //    {
            //        if (ebin.Materials[m].NUM<=0)
            //        {
            //            ebin.Materials.Remove(ebin.Materials[m]);
            //            m--;
            //        }
            //    }
            //    if (ebin.Materials.Count==0)
            //        throw new Exception("下推完成，不能重复下推！");
            //    eb = ebin;

            //}
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
        public override void RelationOrder(List<ControllerAssociate> list)
        {
            //控制器转换器入库通知-入库单

            ControllerAssociate b2 = new ControllerAssociate(this, PushController());
            //ControllerAssociate b3 = new ControllerAssociate(this, new ReturnInNoticeOrderCotroller());

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

            //实体类型转换器（入库通知单-入库单）
           // ConvertAssociate nTo1 = new ConvertAssociate();
           // nTo1.SourceType = typeof(StockOutNoticeMaterials);//下推来源单据子集
           // nTo1.TargerType = typeof(StockInNoticeMaterials);//下推目标单据子集
            //YS(nTo);
            b2.AddConvert(nTo);
           // b3.AddConvert(nTo1);
            list.Add(b2);
           // list.Add(b3);
        }
    }
}
