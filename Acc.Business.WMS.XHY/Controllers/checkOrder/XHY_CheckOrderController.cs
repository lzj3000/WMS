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
using System.Data;
using Acc.Business.WMS.XHY.Model;
using Way.EAP.DataAccess.Regulation;
using Acc.Contract.Data;
using Acc.Business.Model;
using System.Threading;
using Way.EAP.DataAccess.Data;
namespace Acc.Business.WMS.XHY.Controllers
{
    public class XHY_CheckOrderController : BusinessController
    {
        public XHY_CheckOrderController() : base(new ProCheckMaterials()) { }
        public XHY_CheckOrderController(IModel model) : base(model) { }
        /// <summary>
        /// 描述：新华杨质检单基类控制器
        /// 作者：柳强
        /// 创建日期:2013-10-17
        /// </summary>

        #region 页面设置
        //是否启用审核--未启用因为审核人不可以为当前登陆人
        public override bool IsReviewedState
        {
            get
            {
                return false;
            }
        }
        //是否启用提交
        public override bool IsSubmit
        {
            get
            {
                return false;
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

        /// <summary>
        /// 是否启用下推
        /// </summary>
        public override bool IsPushDown
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
     
        //开发人
        protected override string OnGetAuthor()
        {
            return "柳强";
        }
       
        #endregion

        #region 初始化数据方法
        protected override void OnInitViewChildItem(Contract.Data.ModelData data, Contract.Data.ItemData item)
        {
            base.OnInitViewChildItem(data, item);
            //if (data.name.EndsWith("ProCheck"))
            //{
            //    switch (item.field.ToLower())
            //    {
            //        case "stay1":
            //        case "stay2":
            //        case "stay3":
            //        case "stay4":
            //        case "stay5":
            //            item.visible = false;
            //            break;
            //        case "sourcecode":
            //            item.visible = true;
            //            item.disabled = true;
            //            break;
            //        case "submiteddate":
            //        case "submitedby":
            //        case "createdby":
            //        case "creationdate":
            //        case "reviewedby":
            //        case "revieweddate":
            //        case "issubmited":
            //        case "isreviewed":
            //        case "code":
            //        case "rowindex":
            //            item.visible = true;
            //            item.disabled = true;
            //            break;

            //    }
            //}
            if (data.name.EndsWith("ProCheckMaterials"))
            {
                if (item.IsField("CHECKWARE"))
                    {
                        if (item.foreign != null)
                        {
                            item.foreign.rowdisplay.Add("FMODEL", "FMODEL");
                            item.foreign.rowdisplay.Add("CODE", "MCODE");
                        }
                    }
                switch (item.field.ToLower())
                {
                    case "stay1":
                    case "stay2":
                    case "stay3":
                    case "stay4":
                    case "stay5":
                    case "revieweddate":
                    case "reviewedby":
                    case "isreviewed":
                    case "modifiedby":
                    case "modifieddate":
                    case "isdisable":
                    case "isdelete":
                    case "checknum":
                        item.visible = false;
                        break;
                    case "submiteddate":
                        item.title = "质检日期";
                        item.visible = true;
                        item.disabled = true;
                        break;
                    case "submitedby":
                    item.title = "检验员";
                        item.visible = true;
                        item.disabled = true;
                        break;
                    case "issubmited":
                        item.title = "是否质检";
                        item.visible = true;
                        item.disabled = true;
                        break;
                    case "createdby":
                    case "creationdate":
                    case "code":
                    case "rowindex":
                        item.visible = true;
                        item.disabled = true;
                        break;
                    //default :
                    //    item.disabled = true;
                    //    break;
                        

                }
            }
        }
        #endregion

        #region 重写方法
        protected override void OnGetWhereing(IModel m, List<SQLWhere> where)
        {
            base.OnGetWhereing(m, where);
            if (m is ProCheckMaterials)
            {
                SQLWhere w = new SQLWhere();
                w.ColumnName = "CheckTypeName";
                w.Value = CheckTypeName();
                where.Add(w);
            }
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
                if (ac.command == "add")
                {
                    ac.visible = true;
                }
                if (ac.command == "remove")
                {
                    ac.visible = true;
                }
                if (ac.command == "addcopy")
                {
                    ac.visible = false;
                }
                //if (ac.command == "UnSubmitData")
                //{
                //    ac.title = "撤销质检";
                //}
                //if (ac.command == "SubmitData")
                //{
                //    ac.title = "质检";
                //}
            }
            return coms;
        }

        protected override void OnAdding(ControllerBase.SaveEvent item)
        {

            ProCheck pc = item.Item as ProCheck;
            pc.CheckTypeName = CheckTypeName();
              base.OnAdding(item);
        }


        protected override void OnEditing(ControllerBase.SaveEvent item)
        {
            base.OnEditing(item);
            ProCheck pc = new ProCheck();
            pc.CheckTypeName = CheckTypeName();
        }

        /// <summary>
        /// 添加到按钮（设置为合格）
        /// </summary>
        [ActionCommand(name = "设置为合格", title = "设置单据内所有物料为合格状态(除免检验)", index = 9, icon = "icon-ok", isalert = true)]
        public void TestData()
        {
            HG();
           
        }

        /// <summary>
        /// 合格
        /// </summary>
        public virtual void HG()
        {
            ProCheckMaterials pc = this.ActionItem as ProCheckMaterials;
            if (pc.IsOK == false)
            {
                string sql = "select * from " + pc.ToString() + " pc where CHECKWARE ='" + pc.CHECKWARE + "' and batchno='" + pc.BATCHNO + "' and IsOK=0";
                DataTable dt = this.model.GetDataAction().GetDataTable(sql);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    this.model.GetDataAction().Execute("update " + pc.ToString() + " set IsOK = 1,issubmited= 1,submitedby=" + this.user.ID + ",submiteddate= '" + DateTime.Now + "' where id='" + dt.Rows[i]["ID"] + "' ");
                }

                SetInfoCheckStatus(pc, "2");
            }
            else
            {
                throw new Exception("异常：质检单已质检合格不能重复质检！");
            }
        }

        /// <summary>
        /// 质检后修改库存对应状态
        /// </summary>
        /// <param name="pc"></param>
        /// <param name="status"></param>
        public virtual void SetInfoCheckStatus(ProCheckMaterials pc,string status)
        {
            StockInfoMaterials sim = new StockInfoMaterials();

            string where = "";

            if (!string.IsNullOrEmpty(pc.Depotwbs))
            {
                where += " and DEPOTWBS='" + pc.Depotwbs + "'";
            }
            if (!string.IsNullOrEmpty(pc.BATCHNO))
            {
                where += " and BATCHNO='" + pc.BATCHNO + "'";
            }
            if (!string.IsNullOrEmpty(pc.PortNo))
            {
                where += " and PORTCODE = '" + pc.PortNo + "'";
            }
            //if (!string.IsNullOrEmpty(pc.SourceCode))
            //{
            //    where += " and orderno = '" + pc.SourceCode + "'";
            //}
            string usql = "update " + sim.ToString() + " set STATUS='" + status + "' where code='" + pc.CHECKWARE + "'" + where + "";
            this.model.GetDataAction().Execute(usql);
        }
        /// <summary>
        /// 添加到按钮（设置为不合格）
        /// </summary>
        [ActionCommand(name = "设置为不合格", title = "设置单据内所有物料为不合格状态(除免检验)", index = 9, icon = "icon-remove", isalert = true)]
        public void TestDataNo()
        {
            BHG();
        }

        public virtual void BHG()
        {
            ProCheckMaterials pc = this.ActionItem as ProCheckMaterials;
            if (pc.IsSubmited == true)
            {
                throw new Exception("异常：该批次已质检合格不能设置为不合格");
            }
            this.model.GetDataAction().Execute("update " + pc.ToString() + " set IsOK = 0,issubmited=1,submitedby=" + this.user.ID + ",submiteddate= '" + DateTime.Now + "' where id='" + pc.ID + "' ");
            SetInfoCheckStatus(pc, "3");
        }
        #endregion

        #region 虚方法 ，供子类重写满足不同需求
        /// <summary>
        /// 子类重写，设置不同的checkTypeName
        /// </summary>
        /// <returns></returns>
        public virtual string CheckTypeName()
        {
            return "";
        }

        public virtual string CreateBy()
        {
            return "";
        }

        /// <summary>
        /// 设置备注
        /// </summary>
        /// <returns></returns>
        public virtual string SetRemark()
        {
            return "";
        }
        /// <summary>
        /// 采购入库质检
        /// </summary>
        /// <param name="item"></param>
        protected void ccg(EventBase item)
        {
            if (item.Controller is PurchaseInOrderController)
            {
                IDataAction acion=this.model.GetDataAction();
                PurchaseInOrderController pic = item.Controller as PurchaseInOrderController;
                StockInOrder sio = pic.ActionItem as StockInOrder;
                sio.Materials.DataAction = acion;
                sio.Materials.GetData();
                EntityList<ProCheckMaterials> list = new EntityList<ProCheckMaterials>(acion);
                ProCheckMaterials pcm;
                BillNumberController bnc = new BillNumberController();
                StockInOrderMaterials sim;
                var result = from o in sio.Materials group o by new {o.MATERIALCODE, o.BATCHNO } into g select new { g.Key, Totle = g.Sum(p => p.NUM), Items = g.ToList<StockInOrderMaterials>() };
                foreach (var r in result)
                {
                    pcm = new ProCheckMaterials();
                    string strSql = string.Format("select * from " + pcm.ToString() + " where CHECKWARE='{0}' and BATCHNO ='{1}'", r.Key.MATERIALCODE, r.Key.BATCHNO);
                    object obj = acion.GetValue(strSql);
                    if (obj == null || string.IsNullOrEmpty(obj.ToString()))
                    {
                        sim = r.Items[0];
                        pcm.BATCHNO = sim.BATCHNO;
                        pcm.CHECKWARE = sim.MATERIALCODE;
                        pcm.STAY1 = sio.SourceCode;
                        pcm.MCODE = sim.MCODE;
                        pcm.FMODEL = sim.FMODEL;
                        pcm.Createdby = sio.Createdby;
                        pcm.Creationdate = DateTime.Now;
                        pcm.IsOK = false;
                        pcm.SourceID = sio.ID;
                        pcm.SourceCode = sio.Code;
                        pcm.SourceController = pic.GetType().FullName;
                        pcm.CheckTypeName = CheckTypeName();
                        pcm.Code = bnc.GetBillNo(pic);
                        list.Add(pcm);
                    }
                }
                list.Save();
            }
            
        }
        /// <summary>
        /// 添加质检单
        /// </summary>
        /// <param name="item"></param>
        /// <param name="name"></param>
        /// <param name="so"></param>
        public virtual void AddCheckMaterialsItem(EventBase item, string name, Acc.Business.WMS.Model.ProCheckMaterials so)
        {
            string title = ((IController)item.Controller).ControllerName();
            if (item.MethodName == "SubmitData" && item.EventType == CustomeEventType.Complete)
            {
                ccg(item);
                return;
                //StockInOrder si = new StockInOrder();
                //StockInOrderMaterials sim = new StockInOrderMaterials();
                //BusinessCommodity bc = new BusinessCommodity();
                //BillNumberController bnc = null;
                //ProCheckMaterials pm = new ProCheckMaterials();
                /////得到入库产品信息，得到不同批次的信息
                //string sql = "select io.code,io.SourceCode,io.id,sm.MATERIALCODE,sm.MCODE,sm.FMODEL,sm.BATCHNO,sm.NUM,sm.ID as smid from " + si.ToString() + " io inner join " + sim.ToString() + " sm on sm.PARENTID = io.ID inner join " + bc.ToString() + " bm on sm.materialcode=bm.id where io.id = " + item.CustomeData["ID"] + " group by io.code,io.SourceCode,io.id,sm.MATERIALCODE,sm.MCODE,sm.FMODEL,sm.BATCHNO,sm.NUM,sm.ID";
                //DataTable dtCount = this.model.GetDataAction().GetDataTable(sql);
                //string strSql = "";
                //DataTable newDt = null;
                //DataTable resultDt = new DataTable();
                //EntityList<ProCheckMaterials> pcmList = new EntityList<ProCheckMaterials>(this.model.GetDataAction());
                //ProCheckMaterials pmNew = null;
                //for (int i = 0; i < dtCount.Rows.Count; i++)
                //{
                //    strSql = string.Format("select * from " + pm.ToString() + " where sourcecode ='{0}' and CHECKWARE='{1}' and BATCHNO ='{2}'", dtCount.Rows[i]["SourceCode"], dtCount.Rows[i]["MATERIALCODE"], dtCount.Rows[i]["BATCHNO"]);
                //    newDt = this.model.GetDataAction().GetDataTable(strSql);
                //    if (newDt.Rows.Count == 0)
                //    {
                //        pmNew = new ProCheckMaterials();
                //        pmNew.STAY1 = dtCount.Rows[i]["SourceCode"].ToString();
                //        pmNew.Createdby = item.user.ID;
                //        pmNew.SourceID = Convert.ToInt32(dtCount.Rows[i]["id"]);
                //        pmNew.SourceCode = dtCount.Rows[i]["Code"].ToString();
                //        pmNew.CHECKWARE = dtCount.Rows[i]["MATERIALCODE"].ToString();
                //        pmNew.FMODEL = dtCount.Rows[i]["FMODEL"].ToString();
                //        pmNew.MCODE = dtCount.Rows[i]["MCODE"].ToString();
                //        pmNew.CHECKNUM = Convert.ToDouble(dtCount.Rows[i]["NUM"]);
                //        pmNew.BATCHNO = dtCount.Rows[i]["BATCHNO"].ToString();
                //        pmNew.Code = dtCount.Rows[i]["Code"].ToString();
                //        pcmList.Add(pmNew);
                //    }
                //}
                //List<ProCheckMaterials> resultList = MergerProCheckMaterials(pcmList);

                //for (int i = 0; i < resultList.Count; i++)
                //{
                //    bnc = new BillNumberController();
                //    so.Code = bnc.GetBillNo(this);
                //    so.STAY1 = resultList[i]["SourceCode"].ToString();
                //    so.Createdby = item.user.ID;
                //    so.Creationdate = DateTime.Now;
                //    so.SourceController = name;
                //    so.SourceID = Convert.ToInt32(resultList[i]["id"]);
                //    so.SourceCode = resultList[i]["Code"].ToString();
                //    so.Remark = SetRemark();
                //    so.CheckTypeName = CheckTypeName();
                //    so.CHECKWARE = resultList[i]["MATERIALCODE"].ToString();
                //    so.FMODEL = resultList[i]["FMODEL"].ToString();
                //    so.MCODE = resultList[i]["MCODE"].ToString();
                //    so.CHECKNUM = Convert.ToDouble(resultList[i]["NUM"]);
                //    so.BATCHNO = resultList[i]["BATCHNO"].ToString();
                //    so.IsOK = false;
                //    string insertSql = string.Format("insert into " + pm.ToString() + "(SourceID,SourceController,SourceCode,SourceRowID,SourceTable,CheckTypeName,CHECKWARE,MCODE,FMODEL,BATCHNO,CHECKNUM,IsOK,Createdby,Creationdate,code,stay1) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}')", so.SourceID, so.SourceController, so.SourceCode, Convert.ToInt32(resultList[i]["smid"]), null, so.CheckTypeName, so.CHECKWARE, so.MCODE, so.FMODEL, so.BATCHNO, so.CHECKNUM, so.IsOK, so.Createdby, so.Creationdate, so.Code, so.STAY1);
                //    this.model.GetDataAction().Execute(insertSql);
                }
        }


      
        #endregion

      
    }

   
}
