using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acc.Contract.MVC;
using Acc.Business.Model;
using Way.EAP.DataAccess.Entity;
using Acc.Contract;
using Acc.Contract.Data;
using Way.EAP.DataAccess.Data;
using Acc.Contract.Data.ControllerData;
using Acc.Business.Model.Purview;
using System.IO;
using System.ComponentModel;
using System.Xml;
using System.Xml.XPath;
using Way.EAP;
using System.Data;
using Way.EAP.DataAccess.Regulation;

namespace Acc.Business.Controllers
{
    public class BusinessController : ControllerBase
    {
        public BusinessController() { }
        public BusinessController(IModel model) : base(model) { }
        
        protected override ModelData OnGetModelData()
        {
            // base.OnGetModelData();
            string name = this.GetType().FullName;
            ModelData data = base.OnGetModelData();
            LanguageController acc = new LanguageController();
            EntityList<SystemModel> list = new EntityList<SystemModel>(this.model.GetDataAction());
            list.GetData("ControllerName='" + name + "'");
            if (list.Count > 0)
            {
                SystemModel model = list[0];

                IDataAction action = this.model.GetDataAction();
                DataTable table = action.GetDataTable("select * from Acc_Bus_ModelData where ParentID=" + model.ID);
                if (table.Rows.Count < 1)
                {
                    return data;
                }
                //if (LanguageController.name != null)
                //{
                //    if (LanguageController.name == "btn")
                //    {
                //        string apth = "/XML/ModelDataToEnglish.xml";
                //        LanguageController nn = new LanguageController();

                //        table = acc.CXmlToDataTable(apth);
                //        table.Select("ParentID='" + model.ID + "'");

                //        setmodeldata(data, table);
                //    }
                //    else
                //    {
                //        setmodeldata(data, table);
                //    }
                //}
                //else
                //{
                    setmodeldata(data, table);
            //    }
            }
            return data;
        }
        private void setmodeldata(ModelData data, DataTable table)
        {
            DataRow[] rows = table.Select("ModelName='" + data.name + "'");
            
            if (rows.Length > 0)
            {
                foreach (DataRow r in rows)
                {
                    ItemData item = data.childitem.FirstOrDefault<ItemData>(delegate(ItemData d) { return d.field.Equals(r["field"].ToString()); });
                    if (item != null)
                    {
                        item.disabled = Convert.ToBoolean(r["disabled"]);
                        item.index = Convert.ToInt32(r["index"]);
                        item.isedit = Convert.ToBoolean(r["isedit"]);
                        item.issearch = Convert.ToBoolean(r["issearch"]);
                        item.length = Convert.ToInt32(r["length"]);
                        item.required = Convert.ToBoolean(r["required"]);
                        item.title = r["title"].ToString();
                        item.visible = Convert.ToBoolean(r["visible"]);
                    }
                }
            }
             foreach (ModelData mmd in data.childmodel)
            {
                setmodeldata(mmd, table);
            }
        }
        private void setmodeldata(ModelData data, SystemModel model)
        {
            foreach (ItemData d in data.childitem)
            {
                SystemModelData md = model.DataItems.Find(delegate(SystemModelData m) { return m.ModelName.Equals(data.name) && m.field.Equals(d.field); });
                if (md != null)
                {
                    d.disabled = md.disabled;
                    d.index = md.index;
                    d.isedit = md.isedit;
                    d.issearch = md.issearch;
                    d.length = md.length;
                    d.required = md.required;
                    d.title = md.title;
                    d.visible = md.visible;
                }
            }
            foreach (ModelData mmd in data.childmodel)
            {
                setmodeldata(mmd, model);
            }
        }
        
        public override ReadTable SearchData(loadItem item)
        {
            
            return base.SearchData(item);
        }
        public override ReadTable SearchTreeData(loadItem item)
        {
            return base.SearchTreeData(item);
        }
        protected override InitData OnInitData()
        {
            InitData datga = base.OnInitData();
            string name = this.GetType().FullName;
            ModelData data = base.OnGetModelData();
            #region 先不要语言
            //LanguageController acc = new LanguageController();
            //EntityList<SystemModel> list = new EntityList<SystemModel>(this.model.GetDataAction());
            //list.GetData("ControllerName='" + name + "'");
            //if (list.Count > 0)
            //{
            //    SystemModel model = list[0];
            //    string MLID = model.ID.ToString();
            //    string isturn = null;
            //    string filePath = "C:\\infor.txt";
            //    if (!File.Exists(filePath))
            //    {
            //        Console.WriteLine("没有此文件");
            //    }
            //    else
            //    {
            //        FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            //        StreamReader srz = new StreamReader(file);
            //        isturn = srz.ReadLine();


            //        srz.Close();
            //    }
            //    if (isturn != null)
            //    {
            //        if (isturn == "btn")
            //        {
            //            string ApthModelData = "/XML/ModelDataToEnglish.xml";
            //            string ApthSystemModel = "/XML/apthSystemModel.xml";
            //            string ApthTitleName = "/XML/titleName.xml";
            //            biti(datga,acc, ApthModelData);
            //            datga=danzi(datga, acc, ApthSystemModel,model);
            //            datga=mulu(datga, acc, ApthTitleName,model);
                        
                        
            //        }
                   
            //    }

            //}
            #endregion

            return datga;
        }
        /// <summary>
        /// 改变datga.title
        /// </summary>
        /// <param name="datga"></param>
        /// <param name="acc"></param>
        /// <param name="ApthModelData"></param>
        /// <returns></returns>
        private void biti(InitData datga, LanguageController acc, string ApthModelData)
        {
            DataTable tableModelData = acc.CXmlToDataTable(ApthModelData);
             setmodeldata(datga.modeldata, tableModelData);
            

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="datga"></param>
        /// <param name="acc"></param>
        /// <param name="ApthSystemModel"></param>
        /// <returns></returns>
        private InitData danzi(InitData datga, LanguageController acc, string ApthSystemModel,SystemModel model)
        {
            DataTable tableModelData = acc.CXmlToDataTable(ApthSystemModel);            
            foreach (ActionCommand cmd in datga.commands)
            {
                DataRow[] dr = tableModelData.Select("ParentID='" + model.ID + "'and command='"+cmd.command+"'");
                if (dr.Count()==0)
                {
                    return datga;
                }
                cmd.name = dr[0]["command"].ToString();
                cmd.title = dr[0]["command"].ToString();
            }
            return datga;
        }
        private InitData mulu(InitData datga, LanguageController acc, string ApthTitleName,SystemModel model)
        {
            DataTable tableModelData = acc.CXmlToDataTable(ApthTitleName);
            for (int i = 0; i < tableModelData.Rows.Count; i++)
            {
                if (tableModelData.Rows[i]["parentID"].ToString() == model.ID.ToString())
                {
                    datga.title = tableModelData.Rows[i]["ModelTableName"].ToString();
                    datga.modeldata.title = tableModelData.Rows[i]["ModelTableName"].ToString();
                }

            }
            return datga;
        }
        private void ParentValid(IEntityBase eb)
        {
            if (eb.IsHaveProperty("ParentID"))
            {
                if (eb["ParentID"] != null && !string.IsNullOrEmpty(eb["ParentID"].ToString()))
                {
                    if (eb["ParentID"].ToString() != "0" && eb["ID"].ToString() != "0")
                    {
                        if (eb["ParentID"].ToString() == eb["ID"].ToString())
                        {
                            string name = "父属性";
                            Dictionary<string, EntityControlAttribute> eca = eb.GetPropertyExpand();
                            if (eca.ContainsKey("ParentID"))
                            {
                                EntityControlAttribute ec = eca["ParentID"];
                                if (ec != null)
                                {
                                    name = ec.HeaderText;
                                }
                            }
                            throw new Exception("异常：" + name + "不能设置为自己！");
                        }
                    }
                }
            }
        }

        protected override ReadTable OutSearchData(loadItem item)
        {
            if (this.model is BasicInfo)
            {
                List<SQLWhere> ws = new List<SQLWhere>();
                ws.AddRange(item.whereList);
                SQLWhere s = new SQLWhere();
                s.ColumnName = "IsDisable";
                s.Value = "0";
                ws.Add(s);
                item.whereList = ws.ToArray();
            }
            return base.OutSearchData(item);
        }
        protected override ReadTable OnSearchData(loadItem item)
        {
            return base.OnSearchData(item);
        }
        protected override void OnAdding(ControllerBase.SaveEvent item)
        {
            ParentValid(item.Item);
            BasicInfo info = item.Item as BasicInfo;
            if (info != null)
            {
                if (string.IsNullOrEmpty(info.Code))
                {
                    //自动生成单据编号
                    BillNumberController bnc = new BillNumberController();
                    info.Code = bnc.GetBillNo(this);
                }
                info.Createdby = user.ID;
                info.Creationdate = DateTime.Now;
                info.Modifieddate = DateTime.Now;
                info.Modifiedby = user.ID;
                info.IsSubmited = false;
                info.Submitedby = "";
                info.Submiteddate = DateTime.MinValue;
                info.Reviewedby = "";
                info.Revieweddate = DateTime.MinValue;
                info.IsReviewed = false;
                info.IsDisable = false;
            }
        }
        protected override void OnEditing(ControllerBase.SaveEvent item)
        {
            ParentValid(item.Item);
            //BasicInfo info = getinfo(item.Item["ID"].ToString());
            //if (info.IsSubmited)
            //{
            //    throw new Exception("异常：单据已提交不能修改!");
            //}
            BasicInfo info = item.Item as BasicInfo;
            if (info != null)
            {
                info.Modifiedby = user.ID;
                info.Modifieddate = DateTime.Now;
            }
        }
        //protected override void OnEdited(EntityBase item)
        //{
        //    this.ActionItem = getinfo(item["ID"].ToString());
        //}
        private void checkInfo(BasicInfo info)
        {
            if (info.IsDelete)
                throw new Exception("异常：单据已删除!");
            if (info.IsDisable)
                throw new Exception("异常：单据已禁用!");
           
        }
        
        public BasicInfo getinfo(string id)
        {
            BasicInfo info = null;
            string where = "ID=" + id;
            this.modelList.GetData(where);
            if (this.modelList.Count > 0)
            {
                info = this.modelList[0] as BasicInfo;
                this.modelList.Clear();
            }
            return info;
        }
        protected virtual void Save(EntityBase item)
        {
            try
            {
                this.modelList.Add(item);
                this.modelList.Save();
                this.ActionItem = item;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally {
                this.modelList.Clear();
            }
        }
        private ProcessState getstate(BasicInfo info,string cname)
        {
            ActionCommand cmd = null;
            foreach (ActionCommand c in this.Idata.commands)
            {
                if (c.command == cname)
                {
                    cmd = c;
                    break;
                }
            }
            ProcessState state = new ProcessState();
            if (cmd != null)
            {
                state.NodeCode = cmd.command;
                state.NodeName = cmd.name;
                state.ArrivalTime = DateTime.Now;
                state.TableName = info.ToString();
                state.TaskID = info.ID;
                state.SubmitUserID = this.user.ID;
                state.SubmitUser = this.user.name;
                state.ControllerName = this.Idata.name;
                state.NodeDesc = this.Idata.modeldata.title;
            }
            return state;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="state"></param>
        /// <param name="issender"></param>
        protected void SendMessage(ProcessState state,bool issender)
        {
            UserMessgae um = new UserMessgae();
            um.ID = this.ActionItem["ID"].ToString();
            um.c = state.ControllerName;
            um.MsgID = state.ID.ToString();
            um.MsgTitle = state.NodeName + this.Idata.modeldata.title;
            if (issender)
            {
                um.RecipientID = state.WorkUserID;
                um.RecipientName = state.WorkUser;
                um.SenderID = state.SubmitUserID;
                um.SenderName = state.SubmitUser;
                um.IsTask = true;
                um.MsgDesc = um.SenderName + um.MsgTitle + "等待处理。";
            }
            else
            {
                um.RecipientID = state.SubmitUserID;
                um.RecipientName = state.SubmitUser;
                um.SenderID = state.WorkUserID;
                um.SenderName = state.WorkUser;
                um.MsgTitle = state.WorkState + this.Idata.modeldata.title;
                um.IsTask = false;
                um.MsgDesc = um.SenderName + um.MsgTitle + "处理完毕。";
            }
           
            ControllerCenter.GetCenter.SendMessage(um);
        }

        
        protected override ActionCommand[] OnInitCommand(ActionCommand[] commands)
        {
            List<ActionCommand> list = new List<ActionCommand>();
            foreach (ActionCommand ac in commands)
            {
                if (ac.command == "SubmitData" || ac.command == "UnSubmitData")
                {
                    if (IsSubmit)//处理是否显示提交按钮
                        list.Add(ac);
                }
                else if (ac.command == "ReviewedData" || ac.command == "UnReviewedData")
                {
                    if (IsReviewedState)//处理是否显示审核按钮
                        list.Add(ac);
                }
                else
                    list.Add(ac);
            }
            AssociateCommands(list);
            return list.ToArray();
        }

        protected override void OnInitView(ModelData data)
        {
            if ((data.parent != null && data.parent.parent == null) && data.name == "Acc.Business.Model.ProcessState")
            {
                data.visible = IsReviewedState;
            }
        }

        #region 提交功能

        public override bool IsSubmit
        {
            get
            {
                return true;
            }
        }
        protected virtual void OnSubmitUser(BasicInfo info, ProcessState state)
        {
            if (this.user.ManagerID > 0)
            {
                state.WorkUser = this.user.ManagerName;
                state.WorkUserID = this.user.ManagerID.ToString();
            }
            else
            {
                state.WorkUser = this.user.name;
                state.WorkUserID = this.user.ID;
            }
        }
        protected virtual void OnSubmitData(BasicInfo info)
        {
            BasicInfo iv = getinfo(info["id"].ToString());
            if (iv != null)
            {
                if (iv.IsSubmited)
                    throw new Exception("异常：单据已提交，不能再次提交！");
                if (iv.IsReviewed)
                    throw new Exception("异常：单据已审核，不能再次提交！");
                checkInfo(iv);
            }
            else
            {
                this.add();
            }
            info.IsSubmited = true;
            info.Submitedby = this.user.ID;
            info.Submiteddate = DateTime.Now;
            //爱创业务暂时屏蔽
            if (this.IsReviewedState)
            {
                ProcessState state = getstate(info, "SubmitData");
                OnSubmitUser(info, state);
                info.ProcessStateItems.Add(state);
                info.Reviewedby = state.WorkUserID;
                SendMessage(state, true);
            }
            
        }
        
        protected virtual void OnUnSubmitData(BasicInfo info)
        {
            if (!info.IsSubmited)
                throw new Exception("异常：单据未提交，不能撤消提交！");
            if (info.IsReviewed)
                throw new Exception("异常：单据已审核，不能撤消提交！");
            checkInfo(info);
            info.IsSubmited = false;
            info.Submitedby = this.user.ID;
            info.Submiteddate = DateTime.Now;
            info.Reviewedby = "";
            if (this.IsReviewedState)
            {
                info.ProcessStateItems.GetData("TaskID='" + info.ID + "' and NodeCode='SubmitData' and WorkState is null");
                if (info.ProcessStateItems.Count > 0)
                {
                    info.ProcessStateItems[0].WorkState = "撤消提交";
                    info.ProcessStateItems[0].WorkUser = this.user.name;
                    info.ProcessStateItems[0].WorkUserID = this.user.ID;
                    info.ProcessStateItems[0].ProcessingTime = DateTime.Now;
                }
            }
            Save(info);
        }

        [ActionCommand(name = "提交", title = "提交给上级领导审批", index = 4, icon = "icon-ok", isalert = true,editshow=true)]
        public void SubmitData()
        {
            //BasicInfo info = getinfo(this.ActionItem["id"].ToString());
            //if (info != null)
            //{
            OnSubmitData(this.ActionItem as BasicInfo);
            BasicInfo info = this.ActionItem as BasicInfo;
            //}
            Save(info);
        }
        [ActionCommand(name = "撤消提交", title = "提交给上级领导审批", index = 5, icon = "icon-undo", isalert = true, issplit = true, splitname = "SubmitData")]
        public void UnSubmitData()
        {
            BasicInfo info = getinfo(this.ActionItem["id"].ToString());
            if (info != null)
            {
                OnUnSubmitData(info);
            }
        }

        #endregion
        protected override void OnInitViewChildItem(ModelData data, ItemData item)
        {
            if (!IsSubmit)
            {
                if (item.IsField("IsSubmited"))
                    item.visible = false;
                if (item.IsField("Submiteddate"))
                    item.visible = false;
                if (item.IsField("Submitedby"))
                    item.visible = false;
            }
            else
            { 
               
            }
            if (!IsReviewedState)
            {
                if (item.IsField("Reviewedby"))
                    item.visible = false;
                if (item.IsField("Revieweddate"))
                    item.visible = false;
                if (item.IsField("IsReviewed"))
                    item.visible = false;
            }
            else
            {
                //this.getCommand();
            }
        }

        #region 审核功能

        protected virtual void OnReviewedData(BasicInfo info)
        {
            string name = this.getCommandName("ReviewedData");
            //if (info.Reviewedby != this.user.ID)
            //    throw new Exception("异常：单据审核人不匹配，不能审核单据！");
            if (!info.IsSubmited)
                throw new Exception("异常：单据未提交，不能" + name + "！");
            if (info.IsReviewed)
                throw new Exception("异常：单据已审核，不能再次" + name + "！");
            checkInfo(info);

            ///爱创业务暂时屏蔽
            info.ProcessStateItems.GetData("TaskID='" + info.ID + "' and NodeCode='SubmitData' and WorkState is null");
            if (info.ProcessStateItems.Count > 0)
            {
                info.ProcessStateItems[0].WorkState = name;
                info.ProcessStateItems[0].WorkUser = this.user.name;
                if (info.ProcessStateItems[0].WorkUserID != this.user.ID)
                {
                    throw new Exception("异常：单据" + name + "人不匹配，不能" + name + "单据！");
                }
                info.ProcessStateItems[0].ProcessingTime = DateTime.Now;
                SendMessage(info.ProcessStateItems[0], false);
            }
            info.IsReviewed = true;
            info.Revieweddate = DateTime.Now;
            if (this.user==null)
            {
                AcctrueUser aa = new AcctrueUser();
                aa.ID = "1";
                user = aa;
            }
            info.Reviewedby = this.user.ID;
            Save(info);
            ValidateEmail();
        }

        /// <summary>
        /// 验证邮件是否可以发送！
        /// </summary>
        private void ValidateEmail()
        {
            EntityList<SystemConfigItems> sci = new EntityList<SystemConfigItems>(this.model.GetDataAction());
            sci.GetData("ISDEFAULT=1 and IsDisable=0");
            if (sci.Count > 0)
            {
                if (sci[0].IsEnableEmail == true)
                {
                    SystemEmailController sec = new SystemEmailController();
                    //获取上级的信息
                    EntityList<OfficeWorker> ofList = new EntityList<OfficeWorker>(this.model.GetDataAction());
                    ofList.GetData("id= " + this.user.ManagerID + "");
                    if (ofList.Count > 0)
                    {
                        new SystemEmailController().Send(this.user.name, this.OnControllerName(), "mr_liuqiang@163.com", null);
                    }
                }
            }
        }

        [ActionCommand(name = "审核", title = "审核认同单据", index = 5, icon = "icon-ok", isalert = true, editshow = true)]
        public virtual void ReviewedData()
        {
            BasicInfo info = getinfo(this.ActionItem["id"].ToString());
            if (info != null)
            {
                OnReviewedData(info);
            }
        }

        protected virtual void OnUnReviewedData(BasicInfo info)
        {
            string name = this.getCommandName("UnReviewedData");
            if (info.Reviewedby != this.user.ID)
                throw new Exception("异常：单据" + name + "不匹配，不能" + name + "单据！");
            if (!info.IsSubmited)
                throw new Exception("异常：单据未提交，不能" + name + "！");
            //if (!info.IsReviewed)
            //    throw new Exception("异常：单据未审核，不能驳回审核！");
            checkInfo(info);
            //info.ProcessStateItems.GetData("TaskID='" + info.ID + "' and NodeCode='SubmitData'");
            //if (info.ProcessStateItems.Count > 0)
            //{
            //    info.ProcessStateItems[0].WorkState = name;
            //    info.ProcessStateItems[0].WorkUser = this.user.name;
            //    if (info.ProcessStateItems[0].WorkUserID != this.user.ID)
            //    {
            //        throw new Exception("异常：单据" + name + "人不匹配，不能" + name + "单据！");
            //    }
            //    info.ProcessStateItems[0].ProcessingTime = DateTime.Now;
            //    SendMessage(info.ProcessStateItems[0], false);
            //}
            info.IsReviewed = false;
            info.Revieweddate = DateTime.MinValue;
            info.IsSubmited = false;
            info.Submiteddate = DateTime.MinValue;
            info.Reviewedby = "";
            Save(info);
        }
        protected string getCommandName(string mod)
        {
            foreach (ActionCommand ac in this.Idata.commands)
            {
                if (mod == ac.command)
                    return ac.name;
            }
            return mod;
        }
        [ActionCommand(name = "驳回审核", title = "驳回单据到提交人", index = 6, icon = "icon-undo", isalert = true, issplit = true, splitname = "ReviewedData")]
        public void UnReviewedData()
        {
            BasicInfo info = getinfo(this.ActionItem["id"].ToString());
            if (info != null)
            {
                OnUnReviewedData(info);
            }
        }

        #endregion

        #region 回收站功能处理

        public override bool IsClearAway
        {
            get
            {
                return true;
            }
        }
        protected override void OnGetWhereing(IModel m, List<SQLWhere> where)
        {
            if (this.IsClearAway)
            {
                if (!where.Exists(delegate(SQLWhere w) { return w.ColumnName.Equals("IsDelete", StringComparison.CurrentCultureIgnoreCase); }) && m is BasicInfo)
                {
                    SQLWhere w = new SQLWhere();
                    w.ColumnName = "IsDelete";
                    w.Value = "0";
                    where.Add(w);
                }
            }
        }
        protected override void Remove()
        {
            if (this.ActionItem != null)
            {
                BasicInfo info = getinfo(this.ActionItem["id"].ToString());
                if (info != null)
                {
                    checkInfo(info);
                    if (info.IsSubmited)
                        throw new Exception("异常：单据已提交，不能删除！");
                }
                else
                {
                    base.Remove();
                    return;
                }
                if (IsClearAway)
                {
                    info.IsDelete = true;
                    this.Save(info);
                }
                else
                {
                    base.Remove();
                }
            }
        }
        /// <summary>
        /// 还原或清除的ID
        /// </summary>
        [WhereParameter]
        public string reductionid { get; set; }
        /// <summary>
        /// 还原
        /// </summary>
        public void Reduction()
        {
            if (!string.IsNullOrEmpty(this.reductionid))
            {
                IDataAction action = this.model.GetDataAction();
                action.Execute("update " + this.model.ToString() + " set IsDelete=0 where id in (" + this.reductionid + ")");
            }
        }
        /// <summary>
        /// 清除
        /// </summary>
        public void ClearAway()
        {
            string sql = "id in (" + this.reductionid + ")";
            this.modelList.DataAction = this.model.GetDataAction();
            this.modelList.GetData(sql);
            this.modelList.RemoveAll();
            this.modelList.Save();
        }
        /// <summary>
        /// 自动清除数据
        /// </summary>
        public virtual void AutoRemove()
        {
            string sql = "update " + this.model.ToString() + " set IsDelete=1,Modifieddate=GETDATE() where issubmit=0 and isdelete=0 and DATEDIFF(DAY,Modifieddate,GETDATE())>=5";
            string sql1 = "delete from " + this.model.ToString() + " where isdelete=1 and DATEDIFF(DAY,Modifieddate,GETDATE())>=10";
            IDataAction action = this.model.GetDataAction();
            action.Execute(sql1);
            action.Execute(sql);
        }
        #endregion 

        #region 上拉/下推功能
        /// <summary>
        /// 是否支持上拉功能
        /// </summary>
        public virtual bool IsPullUp { get { return false; } }
        /// <summary>
        /// 是否支持下推功能
        /// </summary>
        public virtual bool IsPushDown { get { return false; } }
        /// <summary>
        /// 是否支持导入功能
        /// </summary>
        public virtual bool IsImportIn { get { return false; } }


        public void pullup()
        {

        }

        [WhereParameter]
        public string Tag { get; set; }

        public string pushdown()
        {
            ControllerAssociate[] da = this.DownAssociate();
            if (!string.IsNullOrEmpty(this.Tag))
            {
                ControllerAssociate ca = null;
                foreach (ControllerAssociate c in da)
                {
                    if (c.cData.name == Tag)
                    {
                        ca = c;
                        break;
                    }
                }
                if (ca != null)
                {
                    EntityBase eb = OnConvertItem(ca, this.ActionItem);
                    return EntityHelper.ToJSON(eb);  
                }
            }
            return "";
        }
        /// <summary>
        /// 转换成员方法
        /// </summary>
        /// <param name="ca">控制器关系</param>
        /// <param name="actionItem">待转换成员</param>
        /// <returns></returns>
        protected virtual EntityBase OnConvertItem(ControllerAssociate ca,EntityBase actionItem)
        {
            ca.parent.ActionItem = actionItem;
            return ca.GetItem();
        }

        /// <summary>
        /// 创建控制器关联
        /// </summary>
        protected virtual ControllerAssociate[] DownAssociate()
        {
            return null;
        }


        protected virtual void AssociateCommands(List<ActionCommand> coms)
        {
            if (IsPushDown)
            {
                ActionCommand xt = new ActionCommand();
                xt.index = coms.Count;
                xt.command = "down";
                xt.icon = "icon-pushedDown";
                xt.name = "下推";
                xt.title = "下推新单据";
                coms.Add(xt);
                ControllerAssociate[] da = this.DownAssociate();
                if (da != null)
                {
                    for (int i = 0; i < da.Length; i++)
                    {
                        ControllerAssociate ca = da[i];
                        ActionCommand ac = new ActionCommand();
                        ac.command = "pushdown";
                        ac.name = ca.title;
                        ac.Tag = ca.child.GetType().FullName;
                        ac.isselectrow = true;
                        ac.issplit = true;
                        ac.splitname = "down";
                        coms.Add(ac);
                    }
                }
            }
        
        }

        #endregion 


        protected override void OnForeignIniting(IModel model, InitData data)
        {
            base.OnForeignIniting(model, data);
        }

        public virtual void addcopy()
        {
            #region 不要
            //IEntityBase ieb = this.ActionItem as IEntityBase;
            //if (ieb != null)
            //{
            //    IHierarchicalEntityView[] hev = ieb.GetChildEntityList();
            //    foreach (IHierarchicalEntityView v in hev)
            //    {
            //        if (!(v.ChildEntity is ProcessState))
            //        {
            //            IEntityList list = v.GetEntityList();
            //            list.DataAction = this.model.GetDataAction();
            //            list.GetData();
            //            foreach (IEntityBase e in list)
            //            {
            //                e.StateBase = EntityState.Insert;
            //            }
            //        }
            //    }
            //    ieb.StateBase = EntityState.Insert;
            //    SaveEvent se = new SaveEvent();
            //    se.Item = ieb as EntityBase;
            //    this.OnAdding(se);
            //    try
            //    {
            //        this.modelList.Clear();
            //        this.modelList.Add(ieb);
            //        this.modelList.Save();
            //    }
            //    finally
            //    {
            //        this.modelList.Clear();
            //    }
            //}
            #endregion

            if (this.ActionItem != null)
            { 
               BasicInfo info= this.ActionItem as BasicInfo;
               //BillNumberController bnc = new BillNumberController();
               //info.Code = bnc.GetBillNo(this);
               info.Createdby = this.user.ID;
               info.Creationdate = DateTime.Now;
               info.IsDelete = false;
               info.IsDisable = false;
               info.IsReviewed = false;
               info.IsSubmited = false;
               info.Modifiedby = "";
               info.Modifieddate = DateTime.MinValue;
               info.Reviewedby = "";
               info.Revieweddate = DateTime.MinValue;
               info.Submitedby = "";
               info.Submiteddate = DateTime.MinValue;
               if (info != null)
               {
                   int id = info.ID;
                   object oid = null;
                   IDataAction action = this.model.GetDataAction();
                   IEntityBase ieb = (IEntityBase)info;
                   if (id > 0)
                   {
                       Dictionary<string, EntityFieldAttribute> fields = ieb.GetField();
                       string auto = "";
                       foreach (KeyValuePair<string, EntityFieldAttribute> kv in fields)
                       {
                           if (kv.Value != null && kv.Value.IsIdentity)
                               auto = kv.Key;
                       }
                       string ins = info.Regulation.Insert(info, null);
                       action.Execute(ins, out oid);
                       if (oid != null)
                       {
                           IHierarchicalEntityView[] hev = ieb.GetChildEntityList();
                           foreach (IHierarchicalEntityView v in hev)
                           {
                               if (!(v.ChildEntity is ProcessState) && v.GetEntityList().Count > 0)
                               {

                                   EntityBase eb = v.ChildEntity;
                                   Dictionary<string, EntityForeignKeyAttribute> items = ((IEntityBase)eb).GetForeignKey();
                                   string rel = "";
                                   foreach (KeyValuePair<string, EntityForeignKeyAttribute> kv in items)
                                   {
                                       if (kv.Value != null)
                                       {
                                           if (kv.Value.FieldType.Equals(v.ParentEntity.GetType()) && kv.Value.IsChildAssociate)
                                           {
                                               if (kv.Value.FieldName.Equals(auto, StringComparison.CurrentCultureIgnoreCase))
                                               {
                                                   rel =  v.ChildEntity.Regulation.GetDataBaseColumnName(kv.Key);
                                                   break;
                                               }
                                           }
                                       }
                                   }
                                   string csql="";
                                   Dictionary<string, EntityFieldAttribute> cfields = ((IEntityBase)eb).GetField();
                                   foreach (KeyValuePair<string, EntityFieldAttribute> kv in cfields)
                                   { 
                                       if(kv.Value!=null&&kv.Value.IsIdentity==false)
                                       {
                                           if (string.IsNullOrEmpty(csql))
                                               csql = eb.Regulation.GetDataBaseColumnName(kv.Key);
                                           else
                                               csql += "," + eb.Regulation.GetDataBaseColumnName(kv.Key);
                                      }

                                   }
                                   if (!string.IsNullOrEmpty(rel))
                                   {
                                       string sql = "select " + csql.Replace(rel, oid.ToString()) + " from " + eb.ToString() + " where " + rel + "=" + id;
                                       ins = "insert " + eb.ToString() + "(" + csql + ") " + sql;
                                       action.Execute(ins);
                                   }
                               }
                           }
                       }
                   }
               }
            }
        }
    }

}
