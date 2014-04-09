using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using AcctrueTerminal.Common;
using AcctrueTerminal.Model.IsOffModel;
using System.Text.RegularExpressions;

namespace AcctrueTerminal.StockIn
{
    public partial class Frm_ProductsIn : Form
    {
        public Frm_ProductsIn()
        {
            InitializeComponent();
        }

        #region 初始化属性
        private SessionModel sm;
        private UrlTypeData ud;
        private string strSQL = string.Empty;
        private StockInMaterials sIn;
        private string strTempBatch;
        private string strSourceCode;
        private int whid;
        private string strMingXiID;
        #endregion

        #region 页面加载
        private void Frm_ProductsIn_Load(object sender, EventArgs e)
        {
            try
            {
                sm = new SessionModel();
                if (PDASet.IsOff)
                {
                    ;
                }
                else
                {
                    //获取权限
                    DataTable dt = null;
                    if (this.Owner.Text == BaseCommon.ProductsIn)
                        dt = BaseCommon.GetMobileSetInfo(BaseCommon.ProductsIn);
                    if (this.Owner.Text == BaseCommon.ProductsElseIn)
                        dt = BaseCommon.GetMobileSetInfo(BaseCommon.ProductsElseIn);
                    if (this.Owner.Text == BaseCommon.ProductsSemIn)
                    {
                        dt = BaseCommon.GetMobileSetInfo(BaseCommon.ProductsSemIn);
                        txt_Batch.Enabled = false;
                    }
                    if (this.Owner.Text == BaseCommon.ProductsProIn)
                        dt = BaseCommon.GetMobileSetInfo(BaseCommon.ProductsProIn);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        sm.ControllerName = dt.Rows[0]["ControllerName"].ToString();
                        sm.ModelId = Convert.ToInt32(dt.Rows[0]["ID"].ToString());
                        AccessControl(Convert.ToInt32(this.tabControlSet.TabPages.Count));
                    }
                    else
                    {
                        UIHelper.ErrorMsg(InfoMessage.NotControllerName);
                    }
                }
            }
            catch (Exception ex)
            {
                UIHelper.ErrorMsg(ex.Message);
                return;
            }
        }
        #endregion

        #region 选择入库单
        private void btn_CheckOrderQuery_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtList = null;
                Frm_SerachOrder fso = null;
                if (this.Owner.Text == BaseCommon.ProductsIn)
                {
                    fso = new Frm_SerachOrder(BaseCommon.ProductsIn);
                }
                if (this.Owner.Text == BaseCommon.ProductsElseIn)
                {
                    fso = new Frm_SerachOrder(BaseCommon.ProductsElseIn);
                }
                if (this.Owner.Text == BaseCommon.ProductsSemIn)
                {
                    fso = new Frm_SerachOrder(BaseCommon.ProductsSemIn);
                }
                if (this.Owner.Text == BaseCommon.ProductsProIn)
                {
                    fso = new Frm_SerachOrder(BaseCommon.ProductsProIn);
                }
                DialogResult ret = fso.ShowDialog();
                if (fso.OrderCode != null)
                {
                    //先清除页面数据
                    txt_OrderCode.Text = txt_OrderDesc.Text = txt_WhName.Text = txt_TrayCode.Text = txt_Code.Text = txt_Desc.Text = txt_Batch.Text = txt_Uom.Text = txt_Bin.Text = txt_Num.Text = txt_Spec.Text = txt_YNum.Text = ""; ;
                    lv_shInfo.Items.Clear();
                    lv_RecordList.Items.Clear();
                    lv_GroupTrayData.Items.Clear();


                    txt_OrderCode.Text = fso.OrderCode;
                    txt_OrderDesc.Text = fso.OrderDesc;
                    txt_WhName.Text = fso.WhName;
                    sm.OrderId = fso.OrderID;
                    sm.SourceCodeId = fso.SourceID;
                    this.strSourceCode = fso.dtStaylist.Rows[0]["CODE"].ToString();
                    whid = sm.WhourseId = fso.WhourseId;
                    //sm.SourceCodeId = fso.SourceCodeID;
                    for (int i = 0; i < SessionModel.DtOrderInfo.Rows.Count; i++)
                    {
                        if (txt_OrderCode.Text == SessionModel.DtOrderInfo.Rows[i]["Code"].ToString())
                        {
                            strSourceCode = SessionModel.DtOrderInfo.Rows[i]["SOURCECODE"].ToString();
                        }
                    }
                    BindLv_OrderList(fso.DtStaylist);
                    if (this.Owner.Text == BaseCommon.ProductsProIn && fso.DtStaylist.Rows.Count > 0)
                    {
                        txt_Batch.Text = lv_OrderList.Items[0].SubItems[7].Text.ToString();
                        txt_Batch.Enabled = false;
                    }
                    dtList = BaseCommon.GetOrderMaterialsInfo(sm.OrderId.ToString(), sm.ControllerName, BaseCommon.ProductsIn, BaseCommon.StockInMaterials);
                    lv_RecordList.Items.Clear();
                    if (dtList != null && dtList.Rows.Count > 0)
                    {
                        if (UIHelper.QuestionMsg(InfoMessage.FindNotSubmmitData) == DialogResult.Yes)
                        {
                            BindLv_RecordList(dtList);
                            tabControlSet.SelectedIndex = 3;
                        }
                        else
                        {
                            tabControlSet.SelectedIndex = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                UIHelper.ErrorMsg(ex.Message);
                return;
            }
        }
        #endregion

        #region ListView 数据绑定
        private void BindLv_OrderList(DataTable dt)
        {
            lv_OrderList.Items.Clear();
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ListViewItem list = new ListViewItem((i + 1).ToString());
                    list.SubItems.Add(dt.Rows[i]["MCODE"].ToString());
                    list.SubItems.Add(dt.Rows[i]["MATERIALCODE_FNAME"].ToString());
                    list.SubItems.Add(dt.Rows[i]["STAYNUM"].ToString());
                    //list.SubItems.Add(dt.Rows[i]["FUNITID_UNITNAME"].ToString());
                    if (this.Owner.Text == BaseCommon.ProductsProIn)
                        list.SubItems.Add("件");
                    else
                        list.SubItems.Add("kg");
                    list.SubItems.Add(dt.Rows[i]["FMODEL"].ToString());
                    list.SubItems.Add(dt.Rows[i]["ID"].ToString());
                    list.SubItems.Add(dt.Rows[i]["BATCHNO"].ToString());
                    lv_OrderList.Items.Add(list);
                }
            }
        }
        public void BindLv_RecordList(DataTable dt)
        {
            lv_RecordList.Items.Clear();
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ListViewItem list = new ListViewItem((i + 1).ToString());
                    list.SubItems.Add(dt.Rows[i]["PORTNAME_PORTNO"].ToString());
                    list.SubItems.Add(dt.Rows[i]["MCODE"].ToString());
                    list.SubItems.Add(dt.Rows[i]["MATERIALCODE_FNAME"].ToString());
                    list.SubItems.Add(dt.Rows[i]["NUM"].ToString());
                    list.SubItems.Add(dt.Rows[i]["DEPOTWBS_CODE"].ToString());
                    list.SubItems.Add(dt.Rows[i]["BATCHNO"].ToString());
                    //list.SubItems.Add(dt.Rows[i]["FUNITID_UNITNAME"].ToString());
                    if (this.Owner.Text == BaseCommon.ProductsProIn)
                        list.SubItems.Add("件");
                    else
                        list.SubItems.Add("kg");
                    list.SubItems.Add(dt.Rows[i]["FMODEL"].ToString());
                    list.SubItems.Add(dt.Rows[i]["ID"].ToString());
                    lv_RecordList.Items.Add(list);
                }
            }
        }

        public void BindLv_GroupTrayData(DataTable dt)
        {
            Cursor.Current = Cursors.WaitCursor;
            lv_GroupTrayData.Items.Clear();
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ListViewItem list = new ListViewItem((i + 1).ToString());
                    list.SubItems.Add(dt.Rows[i]["TRAYCODE_PORTNO"].ToString());
                    list.SubItems.Add(dt.Rows[i]["GCODE"].ToString());
                    txt_Code.Text = dt.Rows[i]["GCODE"].ToString();
                    CheckTxtCode();
                    /*
                     if (this.Owner.Text == BaseCommon.ProductsProIn || this.Owner.Text == BaseCommon.ProductsElseIn)
        {
            txt_Code.Text = dt.Rows[i]["GCODE"].ToString();
            CheckTxtCode();
            ListViewItem listl = new ListViewItem();
            listl.SubItems.Add(dt.Rows[i]["FMODEL"].ToString());
            lv_shInfo.Items.Add(listl);
            txt_AssetsCode.Text = "";
        }
                     */
                    list.SubItems.Add(dt.Rows[i]["GNAME_FNAME"].ToString());
                    list.SubItems.Add(dt.Rows[i]["TRAYNUM"].ToString());
                    list.SubItems.Add(dt.Rows[i]["BATCHNO"].ToString());
                    //list.SubItems.Add(dt.Rows[i]["FUNITID_UNITNAME"].ToString());
                    list.SubItems.Add("件");
                    list.SubItems.Add(dt.Rows[i]["FMODEL"].ToString());
                    list.SubItems.Add(dt.Rows[i]["ID"].ToString());
                    lv_GroupTrayData.Items.Add(list);
                }
                txt_Num.Text = lv_GroupTrayData.Items.Count.ToString();
                txt_Batch.Enabled = txt_Code.Enabled = txt_Num.Enabled = false;
                txt_Batch.BackColor = Color.White;
            }
            Cursor.Current = Cursors.Default;
        }
        #endregion

        #region 数据提交
        private void menuItemSubmmit_Click(object sender, EventArgs e)
        {
            try
            {
                //重新绑定明细
                DataTable dt = BaseCommon.GetOrderMaterialsInfo(sm.OrderId.ToString(), sm.ControllerName, BaseCommon.ProductsIn, BaseCommon.StockInMaterials);
                if (dt != null && dt.Rows.Count > 0)
                {
                    BindLv_RecordList(dt);
                }
                if (lv_RecordList.Items.Count <= 0)
                {
                    UIHelper.PromptMsg(InfoMessage.NotSubmmitData);
                    return;
                }
                if (this.Owner.Text == BaseCommon.ProductsProIn || this.Owner.Text == BaseCommon.ProductsSemIn)
                {
                    for (int i = 0; i < lv_RecordList.Items.Count; i++)
                    {
                        if (string.IsNullOrEmpty(lv_RecordList.Items[i].SubItems[5].Text) || lv_RecordList.Items[i].SubItems[5].Text == "0" || lv_RecordList.Items[i].SubItems[5].Text == "null")
                        {
                            i = i + 1;
                            UIHelper.ErrorMsg("第" + i + "行存放位置未确认，不能提交！");
                            return;
                        }
                    }
                }
                if (UIHelper.QuestionMsg("确认提交？") == DialogResult.Yes)
                {
                    ud = new UrlTypeData();
                    ud.c = sm.ControllerName;
                    ud.m = BaseCommon.SubmmitData;
                    ud.LoadItem = "{'ID':'" + sm.OrderId + "','TOWHNO':'" + whid + "','StateBase':3}";
                    sm.strResult = ToJson.ExecuteMethod(ud);
                    if (sm.strResult == "Y")
                    {
                        UIHelper.PromptMsg(InfoMessage.SubmmitSuccess);
                        lv_OrderList.Items.Clear();
                        lv_RecordList.Items.Clear();
                        lv_RecordList.Items.Clear();
                        txt_OrderCode.Text = txt_OrderDesc.Text = txt_WhName.Text = "";
                    }
                    else
                    {
                        UIHelper.ErrorMsg(InfoMessage.SubmmitFailed + "：" + sm.strResult);
                    }
                }
            }
            catch (Exception ex)
            {
                UIHelper.ErrorMsg(ex.Message);
                return;
            }
        }
        #endregion

        #region 数据保存
        private void menuItemSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckData())
                {
                    if (PDASet.IsOff)
                    {
                        IsOffSaveData();
                    }
                    else
                    {
                        WiFiSaveData();
                    }
                }
            }
            catch (Exception ex)
            {
                UIHelper.ErrorMsg(ex.Message);
                return;
            }
        }

        public bool OneOrderIn(string ListId)
        {
            if (!BaseCommon.CheckMaterialSequenState(txt_Code.Text))
            {
                return true;
            }
            if (ListId.Length > 10)
            {
                return false;
            }
            ud = new UrlTypeData();
            ud.Type = (int)CheckEnum.Edit;
            ud.c = "Acc.Business.WMS.XHY.Controllers.ProduceInOrderController";
            ud.m = "AddInSequence";
            ud.LoadItem = "";
            List<InSequenceList> ISL = new List<InSequenceList>();
            for (int i = 0; i < lv_GroupTrayData.Items.Count; i++)
            {
                InSequenceList InSequences = new InSequenceList();
                InSequences.InOrderId = sm.OrderId;// Convert.ToInt32(lv_OrderList.Items[0].SubItems[6].Text.ToString());
                InSequences.InOrderMATERIALID = Convert.ToInt32(ListId);
                InSequences.SEQUENCECODE = lv_GroupTrayData.Items[i].SubItems[7].Text;
                ISL.Add(InSequences);
            }
            ud.LoadItem = UIHelper.InSequenceListConversion(ISL);
            if (ToJson.InSequenceInExecuteMethod(ud) != "Y")
                return false;
            return true;
        }

        #region 无线网络操作
        public void WiFiSaveData()
        {
            if (UIHelper.QuestionMsg("确认保存？") == DialogResult.Yes)
            {
                ud = new UrlTypeData();
                ud.Type = (int)CheckEnum.Edit;
                ud.c = sm.ControllerName;
                ud.m = BaseCommon.Edit;
                string strStateBase = "0";
                #region 4月1号之后放开
                if (this.Owner.Text==BaseCommon.ProductsSemIn)
                {
                    if (!string.IsNullOrEmpty(txt_Bin.Text.Trim()) && string.IsNullOrEmpty(strMingXiID))
                    {
                        UIHelper.ErrorMsg("半成品入库第一次操作不能入库货位！");
                        return;
                    }
                    if (string.IsNullOrEmpty(txt_Bin.Text.Trim()))
                        sm.WhourseId = 0;
                    else
                        strStateBase = "3,'ID':" + strMingXiID;
                }
                #endregion
                if (string.IsNullOrEmpty(txt_TrayCode.Text.Trim()))
                    sm.PortsId = 0;
                else
                    sm.PortsId = Convert.ToInt32(SessionModel.DtPortsInfo.Rows[0]["ID"].ToString());
                DataTable dt = BaseCommon.GetOrderInfo("Acc.Business.WMS.Controllers.StockInOrderController", txt_OrderCode.Text.Trim(), BaseCommon.ProductsProIn);
                if (string.IsNullOrEmpty(txt_Bin.Text.Trim()))
                    sm.WhourseId = 0;
                ud.LoadItem = "{'GetOldObject':{'ID':'" + sm.OrderId.ToString() + "','CODE':'" + txt_OrderCode.Text.Trim() + "','SourceCode':'" + strSourceCode + "','CLIENTNO':'" + dt.Rows[0]["CLIENTNO"].ToString() + "','TOWHNO':'" + whid + "'},'Acc.Business.WMS.Model.StockInOrderMaterials':[{'PARENTID':'" + sm.OrderId + "','PORTNAME':'" + sm.PortsId + "','MATERIALCODE':'" + SessionModel.DtMaterialsInfo.Rows[0]["ID"].ToString() + "','MCODE':'" + txt_Code.Text.Trim() + "','FMODEL':'" + SessionModel.DtMaterialsInfo.Rows[0]["FMODEL"].ToString() + "','BATCHNO':'" +ToJson.UrlEncode(txt_Batch.Text.Trim()) + "','DEPOTWBS':'" + sm.WhourseId + "','NUM':'" + txt_Num.Text.Trim() + "','SourceRowID':" + sm.SourceRowId + ",'StateBase':" + strStateBase + "}]}";
                if (this.Owner.Text == BaseCommon.ProductsProIn || this.Owner.Text == BaseCommon.ProductsElseIn)
                {
                    if (OneOrderIn(ToJson.OnOrderInExecuteMethod(ud)))
                        sm.strResult = "Y";
                }
                else
                    sm.strResult = ToJson.ExecuteMethod(ud);
                if (sm.strResult == "Y")
                {
                    UIHelper.PromptMsg(InfoMessage.SaveSuccess);
                    strMingXiID = "";
                    //重新绑定待操作明细
                    if (this.Owner.Text == BaseCommon.ProductsProIn)
                        BindLv_OrderList(BaseCommon.GetNoticeInfo(sm.SourceCodeId.ToString(), null));
                    else
                        BindLv_OrderList(BaseCommon.GetOrderNoticeInfo(sm.SourceCodeId.ToString(), sm.ControllerName, this.Owner.Text));
                    //重新绑定明细
                    DataTable dts = BaseCommon.GetOrderMaterialsInfo(sm.OrderId.ToString(), sm.ControllerName, BaseCommon.ProductsIn, BaseCommon.StockInMaterials);
                    if (dts != null && dts.Rows.Count > 0)
                    {
                        BindLv_RecordList(dts);
                    }
                    Clear();
                    IsEnabled(true, Color.White);
                    txt_Code.Focus();
                }
                else
                {
                    UIHelper.ErrorMsg(InfoMessage.SaveFailed + "：" + sm.strResult);
                }
            }
        }
        #endregion

        #region 离线状态操作
        private void IsOffSaveData()
        {
            if (UIHelper.QuestionMsg("确认保存？") == DialogResult.Yes)
            {
                sIn = new StockInMaterials();
                if (!string.IsNullOrEmpty(txt_Code.Text.Trim()))
                {
                    sIn.PARENTID = sm.OrderId;
                    sIn.MATERIALCODE = sm.NameId;
                    sIn.MATERIALCODE_FNAME = txt_Desc.Text.Trim();
                    sIn.MCODE = txt_Code.Text.Trim();
                    sIn.NUM = Convert.ToDecimal(txt_Num.Text.Trim());
                    sIn.PORTNAME = sm.PortsId;
                    sIn.PORTNAME_PORTNO = txt_TrayCode.Text.Trim();
                    sIn.SourceRowID = sm.SourceRowId;
                    sIn.BATCHNO = txt_Batch.Text.Trim();
                    sIn.DEPOTWBS = sm.WhourseId;
                    sIn.DEPOTWBS_CODE = txt_Bin.Text.Trim();
                    sIn.FMODEL = txt_Spec.Text.Trim();
                    sIn.FUNITID = 0;
                    sIn.FUNITID_UNITNAME = "暂无";
                    if (BaseCommon.StockInSaveData(sIn, BaseCommon.StockInMaterials))
                    {
                        UIHelper.PromptMsg(InfoMessage.SaveSuccess);
                        Clear();
                        IsEnabled(true, Color.White);
                        txt_Code.Focus();
                    }
                    else
                    {
                        UIHelper.ErrorMsg(InfoMessage.SaveFailed + "：" + sm.strResult);
                    }
                }
                //按托盘入库
                if (lv_GroupTrayData.Items.Count > 0 && string.IsNullOrEmpty(txt_Code.Text.Trim()))
                {
                    #region 暂时不可用
                    //for (int i = 0; i < lv_GroupTrayData.Items.Count; i++)
                    //{
                    //    sIn.PARENTID = sm.OrderId;
                    //    sIn.MATERIALCODE = sm.NameId;
                    //    sIn.MATERIALCODE_FNAME = txt_Desc.Text.Trim();
                    //    sIn.MCODE = txt_Code.Text.Trim();
                    //    sIn.NUM = Convert.ToDecimal(txt_Num.Text.Trim());
                    //    sIn.PORTNAME = sm.PortsId;
                    //    sIn.PORTNAME_PORTNO = txt_TrayCode.Text.Trim();
                    //    sIn.SourceRowID = sm.SourceRowId;
                    //    sIn.BATCHNO = txt_Batch.Text.Trim();
                    //    sIn.DEPOTWBS = sm.WhourseId;
                    //    sIn.DEPOTWBS_CODE = txt_Bin.Text.Trim();
                    //    sIn.FMODEL = txt_Spec.Text.Trim();
                    //    sIn.FUNITID = 0;
                    //    sIn.FUNITID_UNITNAME = "暂无";
                    //    //strSQL = "ID='" + sm.OrderId.ToString() + "',PORTNAME='" + SessionModel.DtTrayInfo.Rows[i]["TRAYCODE"].ToString() + "',MATERIALCODE='" + SessionModel.DtTrayInfo.Rows[i]["GNAME"].ToString() + "',MCODE='" + SessionModel.DtTrayInfo.Rows[i]["GCODE"].ToString() + "',FMODEL='" + SessionModel.DtTrayInfo.Rows[i]["FMODEL"].ToString() + "',BATCH='" + SessionModel.DtTrayInfo.Rows[i]["BATCHNO"].ToString() + "',DEPOTWBS='" + sm.WhourseId + "',NUM='" + SessionModel.DtTrayInfo.Rows[i]["TRAYNUM"].ToString() + "'";
                    //    Flag = BaseCommon.SaveData(strSQL, BaseCommon.StockInMaterials);
                    //}
                    //if (Flag)
                    //{
                    //    UIHelper.PromptMsg(InfoMessage.SaveSuccess);
                    //    ClearGroupTrayData();
                    //    Clear();
                    //    IsEnabled(true, Color.White);
                    //    txt_Code.Focus();
                    //}
                    //else
                    //{
                    //    UIHelper.ErrorMsg(InfoMessage.SaveFailed + "：" + sm.strResult);
                    //}
                    #endregion
                }
                else
                {
                    UIHelper.ErrorMsg(InfoMessage.NotBindData);
                }
            }
        }
        #endregion
        #endregion

        #region 散件入库或入库组盘
        /// <summary>
        /// 扫描编码自动带出描述、规格、单位等信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_Code_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == 13)
                {
                    //判断此编码是否属于该入库单
                    if (!string.IsNullOrEmpty(txt_Code.Text.Trim()))
                    {
                        if (lv_OrderList.Items.Count >= 1 && !string.IsNullOrEmpty(txt_OrderCode.Text.Trim()) || this.Owner.Text == BaseCommon.ProductsElseIn)
                        {
                            SetYnum();
                            CheckTxtCode();
                            if (this.Owner.Text == BaseCommon.ProductsIn)
                            {
                                //txt_Batch.Text = GetCreateBatchNo();
                                txt_Batch.Focus();
                                txt_Batch.SelectionStart = txt_Batch.TextLength;
                            }
                            else if (this.Owner.Text == BaseCommon.ProductsSemIn)
                            {
                                /*
                                for (int i = 0; i < lv_OrderList.Items.Count; i++)
                                {
                                    if (txt_Code.Text == lv_OrderList.Items[i].SubItems[1].Text.ToString())
                                        txt_Batch.Text = lv_OrderList.Items[i].SubItems[7].Text.ToString();
                                }
                                 */ 
                            }
                            else if (this.Owner.Text == BaseCommon.ProductsProIn)
                            {
                                txt_Batch.Enabled = false;
                            }
                            else
                                txt_Batch.Text = "";
                        }
                    }
                    else
                    {
                        UIHelper.PromptMsg(InfoMessage.NotCode);
                    }
                }
            }
            catch (Exception ex)
            {
                UIHelper.ErrorMsg(ex.Message);
                return;
            }
        }

        /// <summary>
        /// 设置待操作数量
        /// </summary>
        public void SetYnum()
        {
            for (int i = 0; i < lv_OrderList.Items.Count; i++)
            {
                if (this.Owner.Text != BaseCommon.ProductsIn)
                {
                    if (lv_OrderList.Items[i].SubItems[1].Text == txt_Code.Text.Trim() && lv_OrderList.Items[i].SubItems[7].Text == txt_Batch.Text.Trim())
                    {
                        txt_YNum.Text = lv_OrderList.Items[i].SubItems[3].Text;
                        sm.SourceRowId = Convert.ToInt32(lv_OrderList.Items[i].SubItems[6].Text);
                        break;
                    }
                    else
                    {
                        if (i == (lv_OrderList.Items.Count - 1))
                        {
                            if (this.Owner.Text != BaseCommon.ProductsElseIn && this.Owner.Text != BaseCommon.ProductsElseOut && this.Owner.Text != BaseCommon.ProductsElseIn)
                            {
                                UIHelper.ErrorMsg(InfoMessage.CodeNotOrder);
                                return;
                            }
                        }
                    }
                }
                else
                {
                    if (lv_OrderList.Items[i].SubItems[1].Text == txt_Code.Text.Trim())
                    {
                        txt_YNum.Text = lv_OrderList.Items[i].SubItems[3].Text;
                        sm.SourceRowId = Convert.ToInt32(lv_OrderList.Items[i].SubItems[6].Text);
                        break;
                    }
                    else
                    {
                        if (i == (lv_OrderList.Items.Count - 1))
                        {
                            if (this.Owner.Text != BaseCommon.ProductsElseIn && this.Owner.Text != BaseCommon.ProductsElseOut && this.Owner.Text != BaseCommon.ProductsElseIn)
                            {
                                UIHelper.ErrorMsg(InfoMessage.CodeNotOrder);
                                return;
                            }
                        }
                    }
                }
            }
        }


        public bool CheckTxtCode()
        {
            SessionModel.DtMaterialsInfo = BaseCommon.GetMaterialsInfo(txt_Code.Text.Trim());
            if (SessionModel.DtMaterialsInfo != null && SessionModel.DtMaterialsInfo.Rows.Count > 0)
            {
                sm.NameId = Convert.ToInt32(SessionModel.DtMaterialsInfo.Rows[0]["ID"].ToString());
                /*
                if (SessionModel.DtMaterialsInfo.Rows[0]["SEQUENCECODE"].ToString() == "True")
                {
                    txt_AssetsCode.Text = txt_Code.Text;
                    tabControlSet.SelectedIndex = 2;
                }
                if (SessionModel.DtMaterialsInfo.Rows[0]["BATCH"].ToString() == "False")
                {
                    txt_Batch.Enabled = false;
                    //Btn_Select.Enabled = false;
                    txt_Batch.BackColor = Color.Aquamarine;
                }
                 */ 
                txt_Desc.Text = SessionModel.DtMaterialsInfo.Rows[0]["FNAME"].ToString();
                if (this.Owner.Text == BaseCommon.ProductsProIn)
                    txt_Uom.Text = "件";
                else
                    txt_Uom.Text = "KG";
                txt_Spec.Text = SessionModel.DtMaterialsInfo.Rows[0]["FMODEL"].ToString();
                if (txt_Batch.Enabled != false)
                {
                    txt_Batch.Focus();
                }
                else
                {
                    txt_Bin.Focus();
                }
                return true;
            }
            else
            {
                UIHelper.PromptMsg(InfoMessage.NotFoundCode);
                txt_Code.SelectAll();
                return false;
            }
        }
        #endregion

        #region 权限控制
        /// <summary>
        /// 权限控制
        /// </summary>
        public void AccessControl(int TabControlCount)
        {
            ud = BaseCommon.ModelControlFdata(sm.ModelId);
            DataTable dt = ToJson.getData(ud);
            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < tabControlSet.TabPages.Count; i++)
                    {
                        if (dt.Select("ModelListName='" + tabControlSet.TabPages[i].Text + "'").Length > 0)
                        {
                            PDASet.List.Add(i);
                        }
                        if (dt.Select("ModelListName='托盘记录'").Length <= 0)
                        {
                            //txt_TrayCode.Enabled = false;
                            //txt_TrayCode.BackColor = Color.Aquamarine;
                        }
                    }
                    for (int i = 0; i < PDASet.List.Count; i++)
                    {
                        #region
                        //if (tabControlSet.TabPages.Count < 6)
                        //{
                        //    a = 6 - tabControlSet.TabPages.Count;
                        //    tabControlSet.TabPages.RemoveAt(Convert.ToInt32(list[i])-a);
                        //}
                        //else
                        //{
                        //    tabControlSet.TabPages.RemoveAt(Convert.ToInt32(list[i]));
                        //}
                        #endregion

                        PDASet.InitTabPageCount = TabControlCount - tabControlSet.TabPages.Count;
                        tabControlSet.TabPages.RemoveAt(Convert.ToInt32(PDASet.List[i]) - PDASet.InitTabPageCount);
                    }
                }
                PDASet.List.Clear();
                tabControlSet.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region 按托盘入库
        private void txt_TrayCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == 13)
                {
                    if (string.IsNullOrEmpty(txt_TrayCode.Text.Trim()))
                    {
                        UIHelper.ErrorMsg(InfoMessage.InputOrScanningTrayCode);
                        return;
                    }
                    if (CheckTxtTrayCode())
                        txt_Code.Focus();
                    else
                    {
                        txt_TrayCode.SelectAll();
                        txt_TrayCode.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                UIHelper.ErrorMsg(ex.Message);
                return;
            }
        }

        public bool CheckTxtTrayCode()
        {
            SessionModel.DtPortsInfo = BaseCommon.GetPortsInfo(txt_TrayCode.Text.Trim());
            if (SessionModel.DtPortsInfo != null && SessionModel.DtPortsInfo.Rows.Count > 0)
            {
                if (SessionModel.DtPortsInfo.Rows[0]["STATUS"].ToString() == "1" && this.Owner.Text!=BaseCommon.ProductsProIn)
                {
                    UIHelper.ErrorMsg("托盘占用！");
                    return false;
                }
                sm.PortsId = Convert.ToInt32(SessionModel.DtPortsInfo.Rows[0]["ID"].ToString());
                SessionModel.DtTrayInfo = BaseCommon.GetTrayInfo(this.strSourceCode.Trim(), sm.PortsId.ToString(), null);
                if (this.Owner.Text == BaseCommon.ProductsProIn && SessionModel.DtTrayInfo.Rows.Count == 0)
                {
                    //判断是否组盘和序列码管理，如果未组盘，并且是序列码管理则终止操作
                    txt_Code.Text = lv_OrderList.Items[0].SubItems[1].Text;
                    CheckTxtCode();
                    if (BaseCommon.CheckMaterialSequenState(txt_Code.Text))
                    {
                        UIHelper.PromptMsg("托盘未组盘，并且当前操作单据的产品为序列码管理，操作终止！");
                        return false;
                    }
                    UIHelper.PromptMsg("托盘未组盘！");
                }
                if (SessionModel.DtTrayInfo != null && SessionModel.DtTrayInfo.Rows.Count > 0)
                {
                    if (this.Owner.Text != BaseCommon.ProductsProIn && this.Owner.Text != BaseCommon.ProductsElseIn)
                    {
                        UIHelper.ErrorMsg("托盘已经组盘，不能操作！");
                        txt_TrayCode.Focus();
                        txt_TrayCode.SelectionStart = txt_TrayCode.TextLength;
                        return false;
                    }
                    IsEnabled(false, Color.Aquamarine);
                    BindLv_GroupTrayData(SessionModel.DtTrayInfo);
                    txt_Batch.Text = lv_GroupTrayData.Items[0].SubItems[5].Text.ToString();
                    //for (int i = 0; i < lv_GroupTrayData.Items.Count;i++ )
                    //{
                    //    txt_Batch.Text=lv_GroupTrayData.Items[i].SubItems[5].Text.ToString();
                    //}
                    if (this.Owner.Text != BaseCommon.ProductsElseIn)
                        txt_YNum.Text = lv_OrderList.Items[0].SubItems[3].Text;
                }
                else
                {
                    IsEnabled(true, Color.White);
                    txt_Code.Enabled = true;
                    //txt_TrayCode.Focus();
                }
                return true;
            }
            else
            {
                UIHelper.ErrorMsg(InfoMessage.NotTrayCode);
                txt_TrayCode.SelectAll();
                return false;
            }
        }
        #endregion

        #region 数据项验证
        private bool CheckData()
        {
            if (!UIHelper.CheckTextBox(txt_OrderCode.Text.Trim()))
            {
                UIHelper.ErrorMsg(InfoMessage.SelectOrder);
                return false;
            }
            if (!UIHelper.CheckTextBox(txt_Code.Text.Trim()) && !UIHelper.CheckTextBox(txt_TrayCode.Text.Trim()))
            {
                UIHelper.ErrorMsg(InfoMessage.InputOrScanningCode + "或者" + InfoMessage.InputOrScanningTrayCode);
                txt_TrayCode.Focus();
                return false;
            }
            /*
            if (this.Owner.Text != BaseCommon.ProductsSemIn || this.Owner.Text != BaseCommon.ProductsIn)
            {
                if (string.IsNullOrEmpty(txt_TrayCode.Text.Trim()))
                {
                    UIHelper.ErrorMsg(InfoMessage.InputOrScanningTrayCode);
                    txt_TrayCode.Focus();
                    return false;
                }
            }
             */
            if (this.Owner.Text == BaseCommon.ProductsProIn)
            {
                if (!CheckTxtTrayCode())
                    return false;
            }
            else
            {
                if (!string.IsNullOrEmpty(txt_TrayCode.Text.Trim()))
                {
                    if (!CheckTxtTrayCode())
                        return false;
                }
            }
            if (!CheckTxtCode())
            {
                return false;
            }
            SetYnum();
            if (this.Owner.Text == BaseCommon.ProductsSemIn)
            {
                if (!string.IsNullOrEmpty(txt_Bin.Text.Trim()))
                {
                    if (!GetWhourseId(txt_Bin.Text.Trim()))
                    {
                        return false;
                    }
                }
            }
            else
            {
                if (!UIHelper.CheckTextBox(txt_Bin.Text.Trim()))
                {
                    txt_Bin.Text = InfoMessage.InputOrScanningBinCode;
                    txt_Bin.SelectAll();
                    txt_Bin.Focus();
                    UIHelper.ErrorMsg("请扫描货位！");
                    return false;
                }
                if (!GetWhourseId(txt_Bin.Text.Trim()))
                {
                    return false;
                }
            }
            /*
            if (this.Owner.Text == BaseCommon.ProductsIn && txt_Batch.Text.Trim().Length<6)
            {
                UIHelper.ErrorMsg("批次修改错误！");
                txt_Batch.Text = strTempBatch;
                txt_Batch.Focus();
                txt_Batch.SelectionStart = txt_Batch.TextLength;
                return false;
            }
            if (this.Owner.Text == BaseCommon.ProductsIn)
            {
                Regex r = new Regex(strTempBatch); // 定义一个Regex对象实例
                Match m = r.Match(txt_Batch.Text.Trim()); // 在字符串中匹配
                if (!m.Success) 
                {
                    UIHelper.ErrorMsg("批次修改错误！");
                    txt_Batch.Text = strTempBatch;
                    txt_Batch.Focus();
                    txt_Batch.SelectionStart = txt_Batch.TextLength;
                    return false;
                }
            }
             */
            if (string.IsNullOrEmpty(txt_Batch.Text.Trim()))
            {
                UIHelper.ErrorMsg("请输入批次！");
                txt_Batch.Focus();
                return false;
            }
            if (!UIHelper.CheckNum(txt_Num.Text.Trim()))
            {
                UIHelper.ErrorMsg(InfoMessage.Number);
                return false;
            }
            if (this.Owner.Text != BaseCommon.ProductsElseIn)
            {
                if (!UIHelper.CheckNum(txt_YNum.Text.Trim()))
                {
                    UIHelper.ErrorMsg(InfoMessage.Number);
                    return false;
                }
                if (Convert.ToDouble(txt_Num.Text.Trim()) > Convert.ToDouble(txt_YNum.Text.Trim()))
                {
                    UIHelper.ErrorMsg("入库数量大于源单数量！");
                    return false;
                }
            }
            //if (GetWhourseId(txt_Bin.Text.Trim()) == false)
            //{
            //    return false;
            //}

            return true;
        }
        #endregion

        #region 单件序列码管理
        #endregion

        #region 数据删除
        /// <summary>
        /// 数据删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lv_RecordList_ItemActivate(object sender, EventArgs e)
        {
            try
            {
                if (lv_RecordList.Items.Count < 0)
                {
                    UIHelper.ErrorMsg(InfoMessage.NotDeleteData);
                    return;
                }
                if (this.Owner.Text == BaseCommon.ProductsSemIn)
                {
                    if (UIHelper.QuestionMsg("是否进行入库操作？") == DialogResult.Yes)
                    {
                        txt_TrayCode.Text = lv_RecordList.Items[lv_RecordList.SelectedIndices[0]].SubItems[1].Text;
                        txt_Code.Text = lv_RecordList.Items[lv_RecordList.SelectedIndices[0]].SubItems[2].Text;
                        txt_Desc.Text = lv_RecordList.Items[lv_RecordList.SelectedIndices[0]].SubItems[3].Text;
                        txt_YNum.Text = txt_Num.Text = lv_RecordList.Items[lv_RecordList.SelectedIndices[0]].SubItems[4].Text;
                        //txt_Bin.Text = lv_RecordList.Items[lv_RecordList.SelectedIndices[0]].SubItems[5].Text;
                        txt_Bin.Text = "";
                        txt_Batch.Text = lv_RecordList.Items[lv_RecordList.SelectedIndices[0]].SubItems[6].Text;
                        txt_Uom.Text = lv_RecordList.Items[lv_RecordList.SelectedIndices[0]].SubItems[7].Text;
                        txt_Spec.Text = lv_RecordList.Items[lv_RecordList.SelectedIndices[0]].SubItems[8].Text;
                        strMingXiID = lv_RecordList.Items[lv_RecordList.SelectedIndices[0]].SubItems[9].Text;
                        tabControlSet.SelectedIndex = 1;
                        return;
                    }
                }
                if (UIHelper.QuestionMsg("确认删除？") == DialogResult.Yes)
                {
                    int ID = Convert.ToInt32(lv_RecordList.Items[lv_RecordList.SelectedIndices[0]].SubItems[9].Text);
                    if (this.lv_RecordList.Items.Count > 0 && this.lv_RecordList.SelectedIndices.Count > 0)
                    {
                        if (PDASet.IsOff)
                        {
                            strSQL = " where ID=" + ID + "";
                            if (BaseCommon.DeleteData(BaseCommon.StockInMaterials, strSQL))
                            {
                                UIHelper.PromptMsg(InfoMessage.DeleteSuccess);
                                lv_RecordList.Items.RemoveAt(lv_RecordList.SelectedIndices[0]);
                            }
                            else
                            {
                                UIHelper.ErrorMsg(InfoMessage.DeleteFailed);
                            }
                        }
                        else
                        {
                            ud = new UrlTypeData();
                            ud.Type = (int)CheckEnum.Edit;
                            ud.c = sm.ControllerName;
                            ud.m = BaseCommon.Edit;
                            ud.LoadItem = "{'GetOldObject':{'ID':'" + sm.OrderId + "','CODE':'" + txt_OrderCode.Text.Trim() + "','SourceCode':'" + strSourceCode + "','TOWHNO':'" + whid + "'},'Acc.Business.WMS.Model.StockInOrderMaterials':[{'ID':" + ID + ",'StateBase':2}]}";
                            sm.strResult = ToJson.ExecuteMethod(ud);
                            if (sm.strResult == "Y")
                            {
                                UIHelper.PromptMsg(InfoMessage.DeleteSuccess);
                                lv_RecordList.Items.RemoveAt(lv_RecordList.SelectedIndices[0]);
                            }
                            else
                            {
                                UIHelper.ErrorMsg(InfoMessage.DeleteFailed);
                            }
                        }
                    }
                    else
                    {
                        UIHelper.ErrorMsg(InfoMessage.NotDeleteData);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                UIHelper.ErrorMsg(ex.Message);
                return;
            }
        }
        #endregion

        #region 属性控制
        public void IsEnabled(bool Falg, Color co)
        {
            //txt_Code.Enabled = Falg;
            //txt_Code.BackColor = co;
            txt_Batch.Enabled = Falg;
            txt_Batch.BackColor = co;
            txt_Num.Enabled = Falg;
            txt_Num.BackColor = co;
        }

        private void Clear()
        {
            txt_TrayCode.Text = txt_Code.Text = txt_Desc.Text = txt_Batch.Text = txt_Uom.Text = txt_Bin.Text = txt_Num.Text = txt_Spec.Text = txt_YNum.Text = ""; ;
            txt_Batch.Enabled = true;
            lv_shInfo.Items.Clear();
            lv_RecordList.Items.Clear();
            lv_GroupTrayData.Items.Clear();
            SessionModel.Clear();
        }
        #endregion

        #region Tab页面控制
        private void tabControlSet_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dtList = null;
            switch (tabControlSet.TabPages[tabControlSet.SelectedIndex].Text)
            {
                case "明细":
                    if (txt_Batch.Enabled == false)
                    {
                        txt_Bin.Focus();
                        txt_Bin.SelectAll();
                    }
                    else
                    {
                        txt_TrayCode.Focus();
                        txt_TrayCode.SelectAll();
                    }
                    break;
                case "操作":
                    txt_AssetsCode.Focus();
                    break;
                case "记录":
                    if (string.IsNullOrEmpty(txt_OrderCode.Text.Trim()) || txt_OrderCode.Text.Trim() == InfoMessage.SelectOrder)
                    {
                        txt_OrderCode.Text = InfoMessage.SelectOrder;
                        txt_OrderCode.SelectAll();
                        txt_OrderCode.Focus();
                    }
                    else
                    {
                        dtList = BaseCommon.GetOrderMaterialsInfo(sm.OrderId.ToString(), sm.ControllerName, BaseCommon.ProductsIn, BaseCommon.StockInMaterials);
                        if (dtList != null && dtList.Rows.Count > 0)
                        {
                            BindLv_RecordList(dtList);
                        }
                    }
                    break;
                default:
                    break;

            }
        }

        private bool CheckOrder()
        {
            if (string.IsNullOrEmpty(txt_OrderCode.Text) || string.IsNullOrEmpty(txt_OrderDesc.Text) || string.IsNullOrEmpty(txt_WhName.Text))
            {
                UIHelper.ErrorMsg("请正确选择入库单号！");
                tabControlSet.SelectedIndex = 1;
                return false;
            }
            return true;
        }
        #endregion

        #region KeyPress事件
        private void txt_Batch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txt_Bin.Focus();
            }
        }

        #region 验证存放位置
        /// <summary>
        /// 验证存放位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_Bin_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == 13)
                {
                    if (!string.IsNullOrEmpty(txt_Bin.Text.Trim()))
                    {
                        GetWhourseId(txt_Bin.Text.Trim());
                    }
                    else
                    {
                        UIHelper.PromptMsg(InfoMessage.InputOrScanningBinCode);
                    }
                }
            }
            catch (Exception ex)
            {
                UIHelper.ErrorMsg(ex.Message);
                return;
            }
        }
        #endregion

        private void txt_Num_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == 13)
                {
                    if (UIHelper.CheckNum(txt_Num.Text.Trim()))
                    {
                        menuItemSave_Click(sender, e);
                    }
                }
            }
            catch (Exception ex)
            {
                UIHelper.ErrorMsg(ex.Message);
                return;
            }
        }
        #endregion

        #region 获取存储位置信息
        private bool GetWhourseId(string BinCode)
        {
            DataTable dt = null;
            dt = BaseCommon.GetWhouseInfo(BinCode, whid.ToString());
            if (dt != null && dt.Rows.Count > 0)
            {
                sm.WhourseId = Convert.ToInt32(dt.Rows[0]["ID"].ToString());
                txt_Num.Focus();
                return true;
            }
            else
            {
                UIHelper.ErrorMsg(InfoMessage.BinCodeNotFound);
                txt_Bin.SelectAll();
            }
            return false;
        }
        #endregion

        #region 删除组盘数据
        /// <summary>
        /// 删除组盘数据
        /// </summary>
        private void ClearGroupTrayData()
        {
            DataTable dtTrayCName = BaseCommon.GetMobileSetInfo(BaseCommon.GroupTray);
            ud = new UrlTypeData();
            ud.c = dtTrayCName.Rows[0]["ControllerName"].ToString();
            ud.m = BaseCommon.Remove;

            for (int i = 0; i < SessionModel.DtTrayInfo.Rows.Count; i++)
            {
                ud.LoadItem = "{'ID':'" + SessionModel.DtTrayInfo.Rows[i]["ID"].ToString() + "','StateBase':3}";
                sm.strResult = ToJson.ExecuteMethod(ud);
                if (sm.strResult != "Y")
                {
                    UIHelper.ErrorMsg(sm.strResult);
                }
            }
            if (sm.strResult == "Y")
            {
                //UIHelper.PromptMsg(InfoMessage.DeleteSuccess);                
                lv_GroupTrayData.Items.Clear();
            }
            else
            {
                UIHelper.ErrorMsg(InfoMessage.DeleteFailed);
            }
        }
        #endregion

        #region ListViewTODataTable
        public DataTable ListViewTODataTable(ListView lv)
        {
            DataRow dr;
            DataTable dt = null;
            for (int i = 0; i < lv.Columns.Count; i++)
            {
                lv.Columns.Add(lv.Columns[i].Text.Trim(), 60, HorizontalAlignment.Center);
            }

            for (int i = 0; i < lv.Items.Count; i++)
            {
                dr = dt.NewRow();
                for (int j = 0; j < lv.Columns.Count; j++)
                {
                    dr[j] = lv.Items[i].SubItems[j].Text.Trim();
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
        #endregion

        #region 退出界面
        private void Frm_ProductsIn_Closing(object sender, CancelEventArgs e)
        {
            DialogResult result;
            result = UIHelper.QuestionMsg("是否退出");
            if (result == DialogResult.Yes)
            {
                e.Cancel = false;
                SessionModel.Clear();
                mainMenu1.Dispose();
            }
            else
            {
                e.Cancel = true;
            }
        }
        #endregion

        #region 单件入库回车事件
        private void txt_AssetsCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == 13 && txt_AssetsCode.Text.Trim() != "")
                {
                    int i = lv_shInfo.Items.Count;
                    i = i + 1;
                    ListViewItem list = new ListViewItem();
                    list.SubItems.Add(txt_AssetsCode.Text.Trim().ToString());
                    lv_shInfo.Items.Add(list);
                    txt_AssetsCode.Text = "";
                }
            }
            catch (Exception ex)
            {
                UIHelper.ErrorMsg(ex.Message);
                return;
            }
        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection c = lv_shInfo.SelectedIndices;
            if (c.Count > 0)
            {
                lv_shInfo.Items.RemoveAt(c[0]);
                lv_shInfo.Refresh();

            }
        }

        private void lv_OrderList_ItemActivate(object sender, EventArgs e)
        {
            try
            {
                if (lv_OrderList.Items.Count > 0 && this.Owner.Text != BaseCommon.ProductsProIn)
                {
                    txt_Code.Text = lv_OrderList.Items[lv_OrderList.SelectedIndices[0]].SubItems[1].Text;
                    txt_Desc.Text = lv_OrderList.Items[lv_OrderList.SelectedIndices[0]].SubItems[2].Text;
                    if (this.Owner.Text == BaseCommon.ProductsIn)
                    {
                        //txt_Batch.Text = GetCreateBatchNo();
                    }
                    else
                    {
                        txt_Batch.Text = lv_OrderList.Items[lv_OrderList.SelectedIndices[0]].SubItems[7].Text;
                    }
                    if (this.Owner.Text == BaseCommon.ProductsProIn)
                        txt_Uom.Text = "件";
                    else
                        txt_Uom.Text = "KG";
                    txt_Spec.Text = lv_OrderList.Items[lv_OrderList.SelectedIndices[0]].SubItems[5].Text;
                    txt_YNum.Text = lv_OrderList.Items[lv_OrderList.SelectedIndices[0]].SubItems[3].Text;
                    tabControlSet.SelectedIndex = 1;
                }
            }
            catch (Exception ex)
            {
                UIHelper.ErrorMsg(ex.Message);
                return;
            }
        }

        private string GetCreateBatchNo()
        {
            ud = new UrlTypeData();
            ud.Type = (int)CheckEnum.Edit;
            ud.c = "Acc.Business.WMS.XHY.Controllers.ProduceInOrderController";
            ud.m = "GetServerData";
            ud.LoadItem = "{}";
            strTempBatch = ToJson.GetServerData(ud);
            return strTempBatch;
        }
    }
}