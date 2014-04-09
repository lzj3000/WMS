using System;
using System.Collections.Generic;
using System.Linq;
using Acc.Business.Controllers;
using Acc.Business.WMS.Model;
using Acc.Contract;
using Acc.Contract.Center;
using Acc.Contract.Data.ControllerData;
using Acc.Contract.MVC;
using Way.EAP.DataAccess.Entity;
using Acc.Business.Model;
using Acc.Contract.Data;
using Acc.Business.WMS.Controllers;
using Way.EAP.DataAccess.Regulation;
using System.Data;
using Acc.Business.WMS.XHY.Model;
using Way.EAP.DataAccess.Data;

namespace Acc.Business.WMS.XHY.Controllers
{
    public class SellOutOrderController : XHY_BusinessOutOrderController
    {
        public SellOutOrderController() : base(new XHY_StockOutOrder()) { }
        /// <summary>
        /// 描述：新华杨销售出库控制器
        /// 作者：柳强
        /// 创建日期:2013-08-29
        /// </summary>

        #region 基础设置
        //显示在菜单
        protected override string OnControllerName()
        {
            return "销售出库";
        }
        //菜单中url路径
        protected override string OnGetPath()
        {
            return "Views/WMS/XHY/SellOutOrder.htm";
        }

        //说明
        protected override string OnGetControllerDescription()
        {
            return "销售出库管理";
        }


        public override bool IsPrint
        {
            get
            {
                return true;
            }
        }

#endregion

        #region 初始化设置
        /// <summary>
        /// 初始化显示与否
        /// </summary>
        /// <param name="data"></param>
        /// <param name="item"></param>
        protected override void OnInitViewChildItem(Contract.Data.ModelData data, Contract.Data.ItemData item)
        {
            base.OnInitViewChildItem(data, item);
            data.isadd = false;

            if (data.name.EndsWith("XHY_StockOutOrder"))
            {
                switch (item.field.ToLower())
                {
                    case "lxrname":
                    case "lxrphone":
                    case "lxraddress":
                    case "zdkh":
                    case "khlx":
                    case "jhfs":
                    case "ywdw":
                        item.visible = true;
                        item.disabled = true;
                        break;

                }
            }
            if (data.name.EndsWith("StockOutOrder"))
            {
                //根据销售人员带出所在部门
                if (item.field == "SALEUSER")
                {
                    if (item.foreign != null)
                    {
                        item.foreign.rowdisplay.Add("OrganizationID".ToUpper(), "BUMEN");
                    }
                }
                if (item.IsField("SOURCECODE"))
                {
                    if (item.foreign != null)
                    {
                        item.foreign.rowdisplay.Add("CLIENTNO", "CLIENTNO");
                    }
                }
                data.title = "销售出库";
                switch (item.field.ToLower())
                {
                    case "code":
                        item.title = "销售出库单号";
                        item.visible = true;
                        item.disabled = true;
                        break;
                    case "clientno":
                        item.title = "客户编码";
                        item.disabled = true;
                        break;
                    case "carphone":
                    case "state":
                    case "stocktype":
                    case "sourceoutcode":
                    case "sourcename":
                    case "workerid":
                    case "bumen":
                    case "k3outordertype":
                        item.visible = false;
                        break;
                    case "sourcecode":
                        item.visible = true;
                        item.title = "销售出库通知单号";
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
                    
                }
            }
            if (data.name.EndsWith("StockOutOrderMaterials"))
            {
                data.isadd = true;
                data.title = "销售出库明细";
                switch (item.field.ToLower())
                {
                    case "rowindex":
                        item.visible = true;
                        item.disabled = true;
                        break;
                    case "code":
                    case "price":
                    case "parentid":
                    case "sourcecode":
                    case "state":
                    case "reviewedby":
                    case "isreviewed":
                    case "finishtime":
                    case "inouttime":
                    case "isdisable":
                    case "creationdate":
                        item.visible = false;
                        break;
                    case "batchno":
                    case "mcode":
                    case "fmodel":
                        item.visible = true;
                        item.disabled = true;
                        break;
                    case "num":
                        item.title = "出库数量（件数）";
                        item.visible = true;
                        item.disabled = false;
                        break;
                    case "stay6":
                        item.visible = false;
                        item.title = "出库数量";
                        item.required = true;
                        break;
                    case "stay4":
                        item.title = "状态";
                        item.visible = true;
                        item.disabled = true;
                        item.index = 14;
                        break;
                    case "modifieddate":
                        item.title = "确认时间";
                        item.visible = true;
                        item.disabled = true;
                        break;
                    case "revieweddate":
                        item.title = "生产时间";
                        item.visible = true;
                        item.disabled = true;
                        break;
                    case "stay1":
                        item.visible = false;
                        break;
                    case "ischeckok":
                        item.disabled = true;
                        break;
                    case "materialcode":
                        item.required = true;
                        item.title = "物料名称";
                        break;
                    case "depotwbs":
                        item.title = "货位编码";
                        item.required = false;
                        if (item.foreign.rowdisplay.ContainsKey("BATCHNO"))
                            item.foreign.rowdisplay.Remove("BATCHNO");
                        break;
                    case "portname":
                        item.title = "托盘编码";
                        item.required = false;
                        if (item.foreign.rowdisplay.ContainsKey("BATCHNO"))
                            item.foreign.rowdisplay.Remove("BATCHNO");
                        break;
                    case "stay5":
                        item.visible = false;
                        item.disabled = true;
                        item.title = "包装";
                        break;

                }
                if (item.IsField("materialcode"))
                {
                    if (item.foreign != null)
                    {
                       // item.foreign.rowdisplay.Add("STAY2".ToLower(), "batchno");
                    }
                }
            }
            if (data.name.EndsWith("StockOutNoticeMaterials"))
            {
                switch (item.field.ToLower())
                {
                    case "num":
                        item.title = "销售件数";
                        break;
                    case "stay6":
                        item.visible = true;
                        item.title = "销售数量(公斤)";
                        item.disabled = true;
                        break;
                    case "staynum":
                        item.title = "待操作数(件数)";
                        item.disabled = true;
                        break;
                    case "finishnum":
                       item.title = "已操作数(件数)";
                        item.disabled = true;
                        break;
                    case "parentid":
                        item.foreign.isfkey = true;
                        item.foreign.filedname = item.field;
                        item.foreign.objectname = data.name;
                        item.foreign.foreignobject = typeof(XHY_StockOutOrder).FullName;
                        item.foreign.foreignfiled = ("SourceID").ToUpper();
                        item.foreign.tablename = new XHY_StockOutOrder().ToString();
                        item.foreign.parenttablename = new StockOutNoticeMaterials().ToString();
                        break;
                }
            }
            if (data.name.EndsWith("OutInSequence"))
            {
                data.visible = true;
                data.isadd = false;
                data.isedit = false;
                data.isremove = true;
            }
        }

        #endregion

        #region 重写方法

        protected override void OnForeignIniting(IModel model, InitData data)
        {
            if (this.fdata.filedname == "SOURCECODE")
            {
                SellOutNoticeOrderController pnoc = new SellOutNoticeOrderController();
                data.modeldata = pnoc.Idata.modeldata;
            }
            else
            {
                base.OnForeignIniting(model, data);
            }
        }

        protected override void OnAdding(ControllerBase.SaveEvent item)
        {
            //GetOutOrderOfInfoMaterials(item);
           
            StockOutOrder so = item.Item as StockOutOrder;
           
            //infoMaterial(so);
            for (int i = 0; i < so.Materials.Count; i++)
            {
                so.Materials[i].STAY4 = "未确认";
                //so.Materials[i].Creationdate = DateTime.Now;
            }
            //base.ValidataCheckOrder(so);
            base.OnAdding(item);
            //GetOutOrderOfInfoMaterials(item);
            infoMaterial(so);
            if (!string.IsNullOrEmpty(so.SourceCode) && !string.IsNullOrEmpty(so.SourceID.ToString()))
            {
                string sql = "UPDATE dbo.Acc_WMS_OutNotice SET STAY3 = '是' WHERE code='" + so.SourceCode + "' AND id='" + so.SourceID + "'";
                this.model.GetDataAction().Execute(sql);
            }
        }

        protected override void OnRemoveing(ControllerBase.SaveEvent item)
        {
            StockOutOrder so = item.Item as StockOutOrder;
            string sql = "select * from Acc_WMS_OutOrder where SourceCode='" + so.SourceCode + "' and SourceID= '" + so.SourceID + "'";
            if (this.model.GetDataAction().GetDataTable(sql).Rows.Count == 1)
            {
                string usql = "UPDATE dbo.Acc_WMS_OutNotice SET STAY3 = '否' WHERE code='" + so.SourceCode + "' AND id='" + so.SourceID + "'";
                this.model.GetDataAction().Execute(usql);
            }

            base.OnRemoveing(item);
        }

        public override void ValidateBatchno(ControllerBase.SaveEvent item)
        {
           
        }

        public override void ValidateDepotwbs(ControllerBase.SaveEvent item)
        {
           
        }

        public override void ValidatePort(ControllerBase.SaveEvent item)
        {
           
        }

        public override void ValidateAddInNum(ControllerBase.SaveEvent item)
        {
            StockOutOrder si = item.Item as StockOutOrder;
            Decimal d;
            for (int i = 0; i < si.Materials.Count; i++)
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

        protected override ReadTable OnSearchData(loadItem item)
        {
            return base.OnSearchData(item);
        }

        public void infoMaterial(StockOutOrder so)
        {
            var result = from o in so.Materials group o by new { o.SourceRowID,o.MATERIALCODE } into g select new { g.Key, Totle = g.Sum(p => p.NUM),Items = g.ToList<StockOutOrderMaterials>() };
            StockInfoMaterials sim = new StockInfoMaterials();
            string simtn = sim.ToString();
            StockOutOrderMaterials mat = new StockOutOrderMaterials();
            string mtn = mat.ToString();
            string orderby = so.STAY3 == "0" ? " order by batchno asc" : "order by batchno desc";
            string sql = "select a.ID,a.DEPOTWBS,a.BATCHNO,a.PORTCODE,(a.NUM-ISNULL(b.stay6,0)) NUM,a.lastintime,a.CREATIONDATE from " + simtn 
            +" a left join (select STAY3, STAY6 from " + mtn + " where SourceRowID={2}) b on a.id=b.stay3"
            +" where a.code='{0}' and a.WAREHOUSEID={1} and (b.stay6 is null or b.stay6>0) "+ orderby;
            IDataAction action=this.model.GetDataAction();
            DataTable stt=null;
            EntityList<StockOutOrderMaterials> soomList = new EntityList<StockOutOrderMaterials>(action);
            EntityList<XHY_Materials> materials = new EntityList<XHY_Materials>(this.model.GetDataAction());
            double snum = 0, cnsum = 0; ;
            StockOutOrderMaterials soom = null;
            StockOutOrderMaterials oldm = null;
            
            foreach (var item in result)
            {
                oldm = item.Items[0];
                stt = action.GetDataTable(string.Format(sql, item.Key.MATERIALCODE, so.TOWHNO, oldm.SourceRowID));
                if (stt != null && stt.Rows.Count > 0)
                {
                    foreach (DataRow r in stt.Rows)
                        snum += double.Parse(r["NUM"].ToString());
                    if (snum >= item.Totle)
                    {
                        cnsum = item.Totle;
                        foreach (DataRow r in stt.Rows)
                        {
                            soom = new StockOutOrderMaterials();
                            materials.GetData("id='" + item.Items[0].MATERIALCODE + "'");
                            soom.SourceTable = oldm.SourceTable;
                            soom.SourceID = oldm.SourceID;
                            soom.SourceController = oldm.SourceController;
                            soom.SourceCode = oldm.SourceCode;
                            soom.SourceRowID=item.Key.SourceRowID;
                            soom.MATERIALCODE = item.Items[0].MATERIALCODE;
                            soom.FMODEL = item.Items[0].FMODEL.ToString();
                            soom.MCODE = item.Items[0].MCODE.ToString();
                            soom.DEPOTWBS = r["DEPOTWBS"].ToString();
                            soom.PORTNAME = r["PORTCODE"].ToString();
                            soom.BATCHNO = r["BATCHNO"].ToString();
                            soom.NUM = double.Parse(r["NUM"].ToString());
                            soom.STAY7 = soom.NUM * materials[0].WeightNUM;
                            soom.STAY5 = materials[0].WeightNUM.ToString();
                            soom.STAY3 = r["ID"].ToString();
                            soom.STAY4 = "未确认";
                            DataTable dt1 = this.model.GetDataAction().GetDataTable("select lastintime from acc_wms_infomaterials where depotwbs= '" + soom.DEPOTWBS + "' and PORTCODE='" + soom.PORTNAME + "' and mcode='" + soom.MCODE + "'");
                            if (dt1.Rows.Count > 0)
                            {
                                soom.Revieweddate = Convert.ToDateTime(dt1.Rows[0]["lastintime"]);
                            }
                            else
                            {
                                soom.Revieweddate = Convert.ToDateTime(r["lastintime"]);
                            }
                            cnsum = cnsum - soom.NUM;
                            if (cnsum <= 0)
                            {
                                soom.STAY6 = System.Math.Abs(cnsum);
                                soom.NUM = soom.NUM - soom.STAY6;
                                soom.STAY7 = soom.NUM * materials[0].WeightNUM;
                                soom.STAY5 = materials[0].WeightNUM.ToString();
                                soomList.Add(soom);
                                break;
                            }
                            soomList.Add(soom);
                        }
                    }
                    else
                    {

                        throw new Exception("异常：产品编码" + oldm.MCODE + " 出库数量" + item.Totle+ "大于库存数量，库存数量为" + snum + "！");
                    }
                }
                else
                {
                  
                    throw new Exception("异常：产品编码" + oldm.MCODE + " 无库存不能保存！");
                }
            }
            so.Materials.Clear();
            so.Materials.AddRange(soomList);
        }

        /// <summary>
        /// 销售出库几种情况
        /// 1.出库产品和库存产品一一对应
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void GetOutOrderOfInfoMaterials(ControllerBase.SaveEvent item)
        {
            StockOutOrder so = item.Item as StockOutOrder;
            StockInfoMaterials sim = new StockInfoMaterials();
            StockOutOrderMaterials soom = null;
            EntityList<StockInfoMaterials> oneList = new EntityList<StockInfoMaterials>(this.model.GetDataAction());
            EntityList<StockInfoMaterials> twoList = new EntityList<StockInfoMaterials>(this.model.GetDataAction());
            EntityList<StockOutOrderMaterials> soomList = new EntityList<StockOutOrderMaterials>(this.model.GetDataAction());
            EntityList<XHY_Materials> xhy_materials = new EntityList<XHY_Materials>(this.model.GetDataAction());
            ///根据销售出库通知单出库规则生成orderby条件
            string orderby = so.STAY3 == "0" ? " order by batchno asc" : "order by batchno desc";
            ///记录每个相同产品的数量
            double tempNum = 0;
            ///循环销售出库明细
            for (int i = 0; i < so.Materials.Count; i++)
            {
                ///每次循环新产品时清除之前的产品记录数量
                tempNum = 0;
                ///如果销售出库明细行数量输入为0则直接报错
                if (so.Materials[i].NUM <= 0)
                {
                    throw new Exception("异常：序号" + so.Materials[i].RowIndex + " 产品编码" + so.Materials[i].MCODE + " 数量输入错误！");
                }
                ///得到库存数据（根据当前产品和主单仓库并且数量大于0 的数据）
                oneList.GetData("code='" + so.Materials[i].MATERIALCODE + "' and WAREHOUSEID ='" + so.TOWHNO + "' and num >0 " + orderby + "");

                ///库存无数据（如果库存无数据则直接报错）
                if (oneList.Count == 0)
                {
                    throw new Exception("异常：序号" + so.Materials[i].RowIndex + " 产品编码" + so.Materials[i].MCODE + " 无库存不能保存！");
                }

                ///库存只有一条库存记录
                if (oneList.Count == 1)
                {
                    ///如果库存只有一条记录，且当前数量大于库存数量则报错
                    if (oneList[0].NUM < so.Materials[i].NUM)
                    {
                        throw new Exception("异常：商品编码" + so.Materials[i].MCODE + " 出库数量大于库存数量,库存数量为" + oneList[0].NUM + " ！");
                    }
                }

                ///库存有多条记录
                for (int z = 0; z < oneList.Count; z++)
                {
                    
                    ///如果多条记录中的第一行就结果就大于出库明细行数量，则将此库存记录记录
                    if (so.Materials[i].NUM <= oneList[z].NUM)
                    {
                        //如果明细行数量小于等于库存数量，则当前明细行数量为库存查询记录的数量
                        oneList[z].NUM = so.Materials[i].NUM;
                        ///将此行插入到一个新的list中
                        twoList.Add(oneList[z]);
                        ///同时记录此数量
                        tempNum = so.Materials[i].NUM;
                        continue;
                    }
                    else
                    {
                        ///将此条记录的库存数量保存到临时数据
                        tempNum += oneList[z].NUM;
                        if (tempNum <= so.Materials[i].NUM)
                        {
                            twoList.Add(oneList[z]);
                        }
                        else
                        {
                            //得到第二条记录的库存记录，插入到出库明细中应该将此数量更改
                            oneList[z].NUM = oneList[z].NUM- (tempNum - so.Materials[i].NUM);
                            twoList.Add(oneList[z]);
                        }
                    }

                    ///如果出库数量和库存行数量完全相同，加载此库存数据的单件码信息到出库明细行三级表
                }
                //如果明细行数量大于库存记录数量，提示错误
                if (so.Materials[i].NUM > tempNum)
                {
                    throw new Exception("异常：商品编码" + so.Materials[i].MCODE + " 出库数量大于库存数量,库存数量为" + tempNum + "！");
                }
                
            }
            ///添加新明细行数据

            for (int j = 0; j < twoList.Count; j++)
            {
                soom = new StockOutOrderMaterials();
                soom.MATERIALCODE = twoList[j].Code.ToString();
                soom.BATCHNO = twoList[j].BATCHNO.ToString();
                soom.FMODEL = twoList[j].FMODEL.ToString();
                soom.MCODE = twoList[j].MCODE.ToString();
                soom.DEPOTWBS = twoList[j].DEPOTWBS.ToString();
                soom.PORTNAME = twoList[j].PORTCODE.ToString();
                soom.NUM = twoList[j].NUM;//此处需要特殊处理
                soomList.Add(soom);
            }
            ///删除原出库明细行
            so.Materials.RemoveAll(delegate(StockOutOrderMaterials som)
            {
                return true;
            });
            ///添加新出库明细行
            for (int i = 0; i < soomList.Count; i++)
            {
                so.Materials.Add(soomList[i]);
            }
        }
     
        protected override void OnEditing(ControllerBase.SaveEvent item)
        {
            XHY_StockOutOrder so = item.Item as XHY_StockOutOrder;
            EntityList<StockOutOrderMaterials> soomList = new EntityList<StockOutOrderMaterials>(this.model.GetDataAction());
            EntityList<XHY_Materials> materials = new EntityList<XHY_Materials>(this.model.GetDataAction());
            for (int i = 0; i < so.Materials.Count; i++)
            {               
                materials.GetData("id='" + so.Materials[i].MATERIALCODE + "'");
                if (((IEntityBase)so.Materials[i]).StateBase == EntityState.Delete)
                {
                    soomList.GetData("Id='" + so.Materials[i].ID + "'");

                    so.Materials[i].MCODE = soomList[0].MCODE;
                    so.Materials[i].FMODEL = soomList[0].FMODEL;
                    so.Materials[i].STAY7 = so.Materials[i].NUM * materials[0].WeightNUM;
                    so.Materials[i].STAY5 = materials[0].WeightNUM.ToString();
                    if (soomList[0].STAY4 == "已确认")
                    {
                        throw new Exception("异常：商品编码" + so.Materials[i].MCODE + " 出库产品已经过终端确认不能删除！");
                    }
                }
                if (((IEntityBase)so.Materials[i]).StateBase == EntityState.Insert)
                {
                    so.Materials[i].STAY4 = "未确认";
                    so.Materials[i].STAY7 = so.Materials[i].NUM * materials[0].WeightNUM;
                    so.Materials[i].STAY5 = materials[0].WeightNUM.ToString();
                }
            }
            //base.ValidataCheckOrder(so);
            base.OnEditing(item);
        }

        protected override void OnSubmitData(BasicInfo info)
        {
             StockOutOrder so = info as StockOutOrder;
             so.Materials.DataAction = this.model.GetDataAction();
             so.Materials.GetData();
             base.ValidataCheckOrder(so);
            SellOutSubmit(info);
            base.OnSubmitData(info);
            //DelInSequence(so);
        }

        /// <summary>
        /// 销售出库提交方法
        /// </summary>
        /// <param name="info"></param>
        private void SellOutSubmit(BasicInfo info)
        {
            StockOutOrder soo = info as StockOutOrder;
            soo.Materials.DataAction = this.model.GetDataAction();
            soo.Materials.GetData();
            
            base.ValidataCheckOrder(soo);
            //EntityList<InfoInSequence> infoInsList = new EntityList<InfoInSequence>(this.model.GetDataAction());
            soo.Materials.DataAction = this.model.GetDataAction();
            soo.Materials.GetData();
            EntityList<StockInfoMaterials> simList = new EntityList<StockInfoMaterials>(this.model.GetDataAction());
            for (int i = 0; i < soo.Materials.Count; i++)
            {
                if (soo.Materials[i].STAY4 == "未确认")
                {
                    throw new Exception("异常：序号" + soo.Materials[i].RowIndex + "   商品编码" + soo.Materials[i].MCODE + " 出库产品未经过终端确认不能提交！");
                }
                if (soo.Materials[i].DEPOTWBS == "")
                {
                    throw new Exception("异常：序号" + soo.Materials[i].RowIndex + "   商品编码" + soo.Materials[i].MCODE + " 未添加货位不能出库！");
                }
                if (soo.Materials[i].PORTNAME == "")
                {
                    throw new Exception("异常：序号" + soo.Materials[i].RowIndex + "   商品编码" + soo.Materials[i].MCODE + " 未添加托盘不能出库！");
                }
            }
            ValidateFMCode(info);
        }

        /// <summary>
        /// 删除单件码
        /// </summary>
        /// <param name="soo"></param>
        private void DelInSequence(StockOutOrder soo)
        {
            InfoInSequence iis = new InfoInSequence();
            for (int i = 0; i < soo.Materials.Count; i++)
            {
                soo.Materials[i].OutInSequence.DataAction = this.model.GetDataAction();
                soo.Materials[i].OutInSequence.GetData();
                var ss = soo.Materials[i].OutInSequence;
                for (int j = 0; j < ss.Count; j++)
                {
                    this.model.GetDataAction().Execute("delete from " + iis.ToString() + " iis where SEQUENCECODE ='" + ss[j].SEQUENCECODE + "'");
                }
            }
        }

        /// <summary>
        /// 验证当前三级表中录入的单件码数量是否和明细行num相符
        /// </summary>
        /// <param name="info"></param>
        private void ValidateFMCode(BasicInfo info)
        {
            XHY_StockOutOrder sio = info as XHY_StockOutOrder;
            sio.Materials.DataAction = this.model.GetDataAction();
            sio.Materials.GetData();
            for (int i = 0; i < sio.Materials.Count; i++)
            {
                sio.Materials[i].OutInSequence.DataAction = this.model.GetDataAction();
                sio.Materials[i].OutInSequence.GetData();
                bool isdjm = sio.Materials[i].GetForeignObject<Materials>(this.model.GetDataAction()).SEQUENCECODE;
                if (isdjm == true)
                {
                    if (sio.Materials[i].OutInSequence.Count < sio.Materials[i].NUM)
                    {
                        throw new Exception("序号:" + sio.Materials[i].RowIndex + " 商品编码" + sio.Materials[i].MCODE + " 单件码录入不足" + sio.Materials[i].NUM + "请使用终端进行扫码");
                    }
                    if (sio.Materials[i].OutInSequence.Count > sio.Materials[i].NUM)
                    {
                        throw new Exception("序号:" + sio.Materials[i].RowIndex + " 商品编码" + sio.Materials[i].MCODE + " 单件码录入超过" + sio.Materials[i].NUM + "请减少单件码后保存");
                    }
                }
            }
        }

        /// <summary>
        /// 指定源单控制器销售出库通知单
        /// </summary>
        /// <returns></returns>
        public override string OutOrderController()
        {
            return new SellOutNoticeOrderController().ToString();
        }

        /// <summary>
        /// 设置stockType=1
        /// </summary>
        /// <returns></returns>
        public override int SetStockType()
        {
            return 1;
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
                file = "a" + file;

                W = "and  " + file + "  " + symol + "   " + value;
            }
            if (this.fdata.filedname == "SOURCECODE")
            {
                //StockOutNotice son = new StockOutNotice();
                //item.rowsql = "select * from (" + item.rowsql + ") a where a.Code in(select Code from " + son.ToString() + " where stocktype = " + new SellOutNoticeOrderController().SetStockType() + " and IsSubmited=1)";
                SellOutNoticeOrderController pnoc = new SellOutNoticeOrderController();
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
            ///选择可用的产品
            if (this.fdata.filedname == "MATERIALCODE" && this.ActionItem["SourceCode"] != null)
            {
                item.rowsql = "select * from (" + item.rowsql + ") a where a.CommodityType=2 and a.isdisable=0 and a.id in(select MATERIALCODE from " + new StockOutNoticeMaterials().ToString() + " sim INNER JOIN " + new StockOutNotice().ToString() + " si on si.ID= sim.PARENTID  where si.code ='" + this.ActionItem["SourceCode"] + "')";
            }
            else
            {
                base.foreignOutOrderMaterials(model, item);
            }
        }

        public override string GetNoticeStayGrid()
        {
            return base.GetNoticeStayGrid();
        }
        [WhereParameter]
        public string NoticeId1 { get; set; }
        public string GetNoticeMaterials()
        {
            StockOutNoticeMaterials oom = new StockOutNoticeMaterials();
            StockOutNotice oo = new StockOutNotice();
            Materials ms = new Materials();
            string sql = "SELECT oom.MATERIALCODE,bc.FNAME AS F4 ,oom.MCODE,oom.FMODEL,oom.NUM,oom.BATCHNO FROM " + oom.ToString() + " oom INNER JOIN " + oo.ToString() + " oo ON oo.ID = oom.PARENTID inner join " + ms.ToString() + " bc on bc.id = oom.MATERIALCODE WHERE oo.code='" + NoticeId + "'";
            DataTable dt = this.model.GetDataAction().GetDataTable(sql);
            string str = JSON.Serializer(dt);
            return str;
        }
        #endregion
        [ActionCommand(name = "打印单据", title = "", index = 9, icon = "icon-ok",isselectrow = true, onclick = "OpenWin")]
        public void op()
        { 
        
        }
       
    }
}
