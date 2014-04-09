using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AcctrueTerminal.Config;
using AcctrueTerminal.Common;

namespace AcctrueTerminal
{
    public partial class Frm_SerachOrder : Form
    {
        private string ControllerName = string.Empty;
        private string ModelName = string.Empty;               
        private SessionModel sm;
        private DataTable dtlist = null;
        public Frm_SerachOrder(string MName)
        {
            InitializeComponent();
            ModelName = MName;
        }
        private string fdata = string.Empty;
        private void Frm_SerachOrder_Closed(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            mainMenu1.Dispose();
        }

        #region 选择单号并且加载其待操作明细数据
        private void menuItemChecked_Click(object sender, EventArgs e)
        {
            try
            {
                if (lv_Order.Items.Count >= 1)
                {
                    if (this.lv_Order.SelectedIndices.Count > 0)
                    {
                        SourceID = Convert.ToInt32(this.lv_Order.Items[lv_Order.SelectedIndices[0]].SubItems[6].Text.ToString());
                        OrderID = Convert.ToInt32(this.lv_Order.Items[lv_Order.SelectedIndices[0]].SubItems[5].Text.ToString());
                        #region 注释代码
                        //fd = new ForeignData();
                        //ud = new UrlTypeData();

                        //fd.isfkey = true;            
                        //fd.filedname = "PARENTID";
                        //fd.foreignfiled = "SOURCECODE";                    
                        //fd.isassociate = true;
                        //fd.eventrow = "{}";
                        //if (ModelName == "入库")
                        //{
                        //    fd.objectname = "Acc.Business.WMS.Model.StockInNoticeMaterials";
                        //    fd.foreignobject = "Acc.Business.WMS.Model.StockInOrder";
                        //    fd.tablename = "Acc_WMS_StockInOrder";
                        //    fd.parenttablename = "Acc_WMS_StockInNoticeMaterials";
                        //    sw = new SQLWhere();
                        //    sw.ColumnName = "Acc_WMS_StockInNoticeMaterials.PARENTID";
                        //    sw.Value = SourceCodeID.ToString();
                        //    sw.Symbol = "=";
                        //    listsw = new List<SQLWhere>();
                        //    listsw.Add(sw);
                        //    li = new LoadItem();
                        //    li.selecttype = "Acc.Business.WMS.Model.StockInNoticeMaterials";
                        //    li.page = "1";
                        //    li.rows = "10";
                        //    li.columns = new string[] { "MCODE", "MATERIALCODE","NUM","FMODEL"};
                        //    li.whereList = listsw.ToArray();
                        //    ud.LoadItem = UIHelper.LoadItemConversion(li);
                        //    //ud.LoadItem = "{'selecttype':'Acc.Business.WMS.Model.StockInNoticeMaterials','page':1,'rows':10,'whereList':[{'ColumnName':'Acc_WMS_StockInNoticeMaterials.PARENTID','Value':'" + SourceCodeID + "'}]}";
                        //}
                        //if(ModelName=="出库")
                        //{
                        //    fd.objectname = "Acc.Business.WMS.Model.StockOutNoticeMaterials";
                        //    fd.foreignobject = "Acc.Business.WMS.Model.StockOutOrder";
                        //    fd.tablename = "Acc_WMS_StockOutOrder";
                        //    fd.parenttablename = "Acc_WMS_StockOutNoticeMaterials";
                        //    //ud.LoadItem = "{'selecttype':'Acc.Business.WMS.Model.StockOutNoticeMaterials','page':1,'rows':10,'whereList':[{'ColumnName':'Acc_WMS_StockOutNoticeMaterials.PARENTID','Value':'" + SourceCodeID + "'}]}";
                        //    sw = new SQLWhere();
                        //    sw.ColumnName = "Acc_WMS_StockOutNoticeMaterials.PARENTID";
                        //    sw.Value = SourceCodeID.ToString();
                        //    sw.Symbol = "=";
                        //    listsw = new List<SQLWhere>();
                        //    listsw.Add(sw);
                        //    li = new LoadItem();
                        //    li.selecttype = "Acc.Business.WMS.Model.StockOutNoticeMaterials";
                        //    li.page = "1";
                        //    li.rows = "10";
                        //    li.columns = new string[] { "MCODE", "MATERIALCODE", "NUM", "FMODEL" };
                        //    li.whereList = listsw.ToArray();
                        //    ud.LoadItem = UIHelper.LoadItemConversion(li);
                        //}

                        //fdata = UIHelper.FDataConversion(fd);

                        //ud.Type = (int)CheckEnum.Foreignkey;
                        //ud.m = BaseCommon.Foreignkey;
                        //ud.c = ControllerName;         
                        //ud.FDtata = fdata;
                        //DataTable dtlist = ToJson.getData(ud);
                        #endregion
                        if (PDASet.IsOff)
                        {
                            dtlist = BaseCommon.GetOrderNoticeInfoOff(OrderID.ToString(), ModelName);
                        }
                        else
                        {
                            if (ModelName == BaseCommon.GroupTray)
                                dtlist = BaseCommon.GetNoticeInfo(OrderID.ToString(), null);
                            else if (ModelName == BaseCommon.ProductsProIn)
                                dtlist = BaseCommon.GetNoticeInfo(SourceID.ToString(), null);
                            else if (ModelName == BaseCommon.WarehouseInventory)
                                dtlist = BaseCommon.GetCheckOrderInfo(OrderID.ToString());
                            else
                                dtlist = BaseCommon.GetOrderNoticeInfo(SourceID.ToString(), ControllerName, ModelName);
                        }
                        OrderCode = this.lv_Order.Items[lv_Order.SelectedIndices[0]].SubItems[0].Text.ToString();
                        OrderDesc = this.lv_Order.Items[lv_Order.SelectedIndices[0]].SubItems[1].Text.ToString();
                        if (ModelName == BaseCommon.WarehouseInventory)
                        {
                            string strWHid = this.lv_Order.Items[lv_Order.SelectedIndices[0]].SubItems[3].Text.ToString();
                            if (strWHid == "")
                            {
                                UIHelper.PromptMsg("仓库无效！");
                                return;
                            }
                            WhourseId = Convert.ToInt32(strWHid);
                            WhName = this.lv_Order.Items[lv_Order.SelectedIndices[0]].SubItems[4].Text.ToString();
                            if (dtlist != null && dtlist.Rows.Count > 0)
                            {
                                dtStaylist = dtlist;
                            }
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                            return;
                        }
                        OrderState = this.lv_Order.Items[lv_Order.SelectedIndices[0]].SubItems[2].Text.ToString();
                        OrderType = this.lv_Order.Items[lv_Order.SelectedIndices[0]].SubItems[3].Text.ToString();
                        WhName = this.lv_Order.Items[lv_Order.SelectedIndices[0]].SubItems[4].Text.ToString();
                        if (WhName == "" || WhName == null || WhName == "0" || WhName == "null")
                        {
                            if (ModelName != BaseCommon.GroupTray)
                            {
                                UIHelper.PromptMsg("仓库无效！");
                                SourceID = OrderID = 0;
                                OrderCode = OrderDesc = OrderState = OrderType = WhName = null;
                                dtlist = null;
                                return;
                            }
                        }
                        if (ModelName != BaseCommon.GroupTray)
                            WhourseId = Convert.ToInt32(this.lv_Order.Items[lv_Order.SelectedIndices[0]].SubItems[7].Text.ToString());
                        if (ModelName == BaseCommon.GroupTray)
                            MCODE = this.lv_Order.Items[lv_Order.SelectedIndices[0]].SubItems[8].Text.ToString();
                        if (dtlist != null && dtlist.Rows.Count > 0)
                        {
                            dtStaylist = dtlist;
                            this.DialogResult = DialogResult.OK;
                        }
                        else
                        {
                            if (ModelName == BaseCommon.ProductsProIn || ModelName == BaseCommon.GroupTray)
                                this.DialogResult = DialogResult.Yes;//this.DialogResult = DialogResult.OK;
                            else
                            {
                                if (ModelName != BaseCommon.ProductsElseIn && ModelName != BaseCommon.ProductsElseOut && ModelName != BaseCommon.ProductsSemIn)
                                    UIHelper.PromptMsg(InfoMessage.NotOrderList);
                                else
                                    this.DialogResult = DialogResult.OK;
                            }
                        }
                        this.Close();
                    }
                    else
                    {
                        UIHelper.PromptMsg(InfoMessage.SelectOrder);
                    }
                }
                else
                {
                    UIHelper.ErrorMsg(InfoMessage.NotOrder);
                }
            }
            catch (Exception ex)
            {
                UIHelper.ErrorMsg(ex.Message);
                return;
            }
        }
        #endregion

        #region 封装属性
        public string OrderCode { get; set; }
        public string OrderDesc { get; set; }
        public string OrderState { get; set; }
        public string OrderType { get; set; }
        public string WhName { get; set; }
        public int OrderID { get; set; }
        public int SourceID { get; set; }
        public int WhourseId { get; set; }
        public string MCODE { get; set; }
        public DataTable dtStaylist;

        public DataTable DtStaylist
        {
            get 
            {
                if (dtStaylist == null)
                {
                    dtStaylist = new DataTable();
                }
                return dtStaylist;
            }
            set { dtStaylist = value; }
        }      
        
        #endregion

        private void Frm_SerachOrder_Load(object sender, EventArgs e)
        {
            try
            {
                sm = new SessionModel();
                if (PDASet.IsOff)
                {
                    menuItemDownLoad.Enabled = false;
                }
                else
                {
                    if (ModelName == BaseCommon.GroupTray)
                        lv_Order.Columns[4].Width = 0;
                    menuItemDownLoad.Enabled = true;
                    DataTable dt = BaseCommon.GetMobileSetInfo(ModelName);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        ControllerName = dt.Rows[0]["ControllerName"].ToString();
                        if (ModelName == BaseCommon.GroupTray)
                            ControllerName = "Acc.Business.WMS.Controllers.StockInOrderController";
                    }
                    else
                    {
                        UIHelper.ErrorMsg(InfoMessage.NotControllerName);
                    }
                }
                if (ModelName == BaseCommon.GroupTray)
                    SessionModel.DtOrderInfo = BaseCommon.GetNoticeInfo(null, null);
                else if (ModelName == BaseCommon.WarehouseInventory)
                    SessionModel.DtOrderInfo = BaseCommon.GetCheckOrder(null);
                else
                    SessionModel.DtOrderInfo = BaseCommon.GetOrderInfo(ControllerName, null, ModelName);
                QueryData();
            }
            catch (Exception ex)
            {
                UIHelper.ErrorMsg(ex.Message);
                return;
            }
        }

        #region 获取单据
        /// <summary>
        /// 获取单据
        /// </summary>
        private void QueryData()
        {     
            lv_Order.Items.Clear();    
            //SessionModel.DtOrderInfo = BaseCommon.GetOrderInfo(ControllerName);
            if (SessionModel.DtOrderInfo != null && SessionModel.DtOrderInfo.Rows.Count > 0)
            {
                for (int i = 0; i < SessionModel.DtOrderInfo.Rows.Count; i++)
                {
                    ListViewItem list = new ListViewItem();
                    if (ModelName == BaseCommon.WarehouseInventory)
                    {
                        list.Text = SessionModel.DtOrderInfo.Rows[i]["Code"].ToString();
                        list.SubItems.Add(SessionModel.DtOrderInfo.Rows[i]["CREATEDBY_WORKNAME"].ToString());
                        //list.SubItems.Add(SessionModel.DtOrderInfo.Rows[i]["ORDERNAME"].ToString());
                        list.SubItems.Add(SessionModel.DtOrderInfo.Rows[i]["REMARK"].ToString());
                        list.SubItems.Add(SessionModel.DtOrderInfo.Rows[i]["WAREHOUSENAME"].ToString());
                        list.SubItems.Add(SessionModel.DtOrderInfo.Rows[i]["WAREHOUSENAME_WAREHOUSENAME"].ToString());
                        list.SubItems.Add(SessionModel.DtOrderInfo.Rows[i]["ID"].ToString());
                        list.SubItems.Add(SessionModel.DtOrderInfo.Rows[i]["CHECKORDERTYPE"].ToString());
                    }
                    else
                    {
                        list.Text = SessionModel.DtOrderInfo.Rows[i]["code"].ToString();
                        list.SubItems.Add(SessionModel.DtOrderInfo.Rows[i]["CREATEDBY_WORKNAME"].ToString());
                        //list.SubItems.Add(SessionModel.DtOrderInfo.Rows[i]["REMARK"].ToString());
                        list.SubItems.Add(SessionModel.DtOrderInfo.Rows[i]["STATE"].ToString());
                        list.SubItems.Add(SessionModel.DtOrderInfo.Rows[i]["STOCKTYPE"].ToString());
                        if (ModelName != BaseCommon.GroupTray)
                            list.SubItems.Add(SessionModel.DtOrderInfo.Rows[i]["TOWHNO_WAREHOUSENAME"].ToString());
                        else
                            list.SubItems.Add(SessionModel.DtOrderInfo.Rows[i]["DEPOTWBS"].ToString());
                        list.SubItems.Add(SessionModel.DtOrderInfo.Rows[i]["ID"].ToString());
                        list.SubItems.Add(SessionModel.DtOrderInfo.Rows[i]["SOURCEID"].ToString());
                        if (ModelName == BaseCommon.GroupTray)
                            list.SubItems.Add(SessionModel.DtOrderInfo.Rows[i]["DEPOTWBS"].ToString());
                        else
                            list.SubItems.Add(SessionModel.DtOrderInfo.Rows[i]["TOWHNO"].ToString());
                        if (ModelName == BaseCommon.GroupTray)
                            list.SubItems.Add(SessionModel.DtOrderInfo.Rows[i]["MCODE"].ToString());
                    }
                    this.lv_Order.Items.Add(list);                   
                }
            }
            else
            {
                UIHelper.PromptMsg(InfoMessage.NotOrder);
                this.Close();
            }
        }
        #endregion

        #region 下载单据
        /// <summary>
        /// 下载单据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItemDownLoad_Click(object sender, EventArgs e)
        {

        }
        #endregion
    }
}