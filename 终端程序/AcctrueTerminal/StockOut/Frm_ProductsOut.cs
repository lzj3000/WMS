using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AcctrueTerminal.Common;
using AcctrueTerminal.Config;
using AcctrueTerminal.Model.IsOffModel;

namespace AcctrueTerminal.StockOut
{
    public partial class Frm_ProductsOut : Form
    {
        public Frm_ProductsOut()
        {
            InitializeComponent();
        }

        #region 初始化属性
        private SessionModel sm;
        private UrlTypeData ud;
        private string strSQL = string.Empty;
        private StockOutMaterials son;
        private string MingXiList;
        private string WHid;
        private string strSourceCode;
        private string strBatchno;
        private string strStockinfoID;
        //销售出库是否替换
        private string isTihuan = "";
        #endregion

        #region 页面加载
        private void Frm_ProductsOut_Load(object sender, EventArgs e)
        {
            sm = new SessionModel();
            if (PDASet.IsOff)
            {
                ;
            }
            else
            {
                DataTable dt = null;
                if (this.Owner.Text == BaseCommon.ProductsPicOut)
                    dt = BaseCommon.GetMobileSetInfo(BaseCommon.ProductsPicOut);
                if (this.Owner.Text == BaseCommon.ProductsSellOut)
                {
                    dt = BaseCommon.GetMobileSetInfo(BaseCommon.ProductsSellOut);
                    label9.Visible = false;
                    txt_StockNum.Visible = false;
                    txt_Num.Enabled = false;
                    txt_Num.BackColor = Color.Aquamarine;
                }
                if (this.Owner.Text == BaseCommon.ProductsElseOut)
                    dt = BaseCommon.GetMobileSetInfo(BaseCommon.ProductsElseOut);
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
        #endregion

        #region 选择出库单
        private void btn_CheckOrderQuery_Click(object sender, EventArgs e)
        {
            try
            {
                Frm_SerachOrder fso = null;
                if (this.Owner.Text == BaseCommon.ProductsPicOut)
                {
                    fso = new Frm_SerachOrder(BaseCommon.ProductsPicOut);
                }
                if (this.Owner.Text == BaseCommon.ProductsSellOut)
                    fso = new Frm_SerachOrder(BaseCommon.ProductsSellOut);
                if (this.Owner.Text == BaseCommon.ProductsElseOut)
                    fso = new Frm_SerachOrder(BaseCommon.ProductsElseOut);
                DialogResult ret = fso.ShowDialog();
                if (fso.OrderCode != null)
                {
                    for (int i = 0; i < SessionModel.DtOrderInfo.Rows.Count; i++)
                    {
                        if (fso.OrderCode == SessionModel.DtOrderInfo.Rows[i]["Code"].ToString())
                        {
                            strSourceCode = SessionModel.DtOrderInfo.Rows[i]["SOURCECODE"].ToString();
                        }
                    }

                    //先清除页面数据
                    Clear();
                    txt_OrderCode.Text = txt_OrderDesc.Text = txt_WhName.Text = "";


                    txt_OrderCode.Text = fso.OrderCode;
                    txt_OrderDesc.Text = fso.OrderDesc;
                    txt_WhName.Text = fso.WhName;
                    sm.OrderId = fso.OrderID;
                    sm.SourceCodeId = fso.SourceID;
                    WHid = fso.WhourseId.ToString();
                    //如果是生产出库则加载待操作明细
                    if (this.Owner.Text != BaseCommon.ProductsSellOut)
                        BindLv_OrderList(fso.DtStaylist);
                    BindLv_order();
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
        private void BindLv_order()
        {
            DataTable dtList = BaseCommon.GetOrderMaterialsInfo(sm.OrderId.ToString(), sm.ControllerName, BaseCommon.ProductsOut, BaseCommon.StockOutMaterials);
            SessionModel.DtOrderMaterialsInfo = dtList;
            if (dtList != null && dtList.Rows.Count > 0)
            {
                BindLv_RecordList(dtList);
            }
        }

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
                    list.SubItems.Add("KG");
                    list.SubItems.Add(dt.Rows[i]["FMODEL"].ToString());
                    //list.SubItems.Add(dt.Rows[i]["ID"].ToString());
                    list.SubItems.Add(dt.Rows[i]["MATERIALCODE"].ToString());
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
                    if (this.Owner.Text == BaseCommon.ProductsPicOut)
                        list.SubItems.Add("kg");
                    else
                        list.SubItems.Add("件");
                    list.SubItems.Add(dt.Rows[i]["FMODEL"].ToString());
                    list.SubItems.Add(dt.Rows[i]["ID"].ToString());
                    list.SubItems.Add(dt.Rows[i]["STAY4"].ToString());
                    list.SubItems.Add(dt.Rows[i]["IsCheckOk"].ToString());
                    lv_RecordList.Items.Add(list);
                }
            }
        }

        public void BindLv_shInfoList(DataTable dt)
        {
            lv_shInfo.Items.Clear();
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ListViewItem list = new ListViewItem((i + 1).ToString());
                    list.SubItems.Add(dt.Rows[i]["SEQUENCECODE"].ToString());
                    lv_shInfo.Items.Add(list);
                }
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

        public void IsOffSaveData()
        {
            if (UIHelper.QuestionMsg("确认保存？") == DialogResult.Yes)
            {
                son = new StockOutMaterials();
                if (!string.IsNullOrEmpty(txt_Code.Text.Trim()))
                {
                    son.PARENTID = sm.OrderId;
                    son.MATERIALCODE = sm.NameId;
                    son.MATERIALCODE_FNAME = txt_Desc.Text.Trim();
                    son.MCODE = txt_Code.Text.Trim();
                    son.NUM = Convert.ToDecimal(txt_Num.Text.Trim());
                    son.PORTNAME = sm.PortsId;
                    son.PORTNAME_PORTNO = txt_TrayCode.Text.Trim();
                    son.SourceRowID = sm.SourceRowId;
                    son.BATCHNO = txt_Batch.Text.Trim();
                    son.DEPOTWBS = sm.WhourseId;
                    son.DEPOTWBS_CODE = txt_Bin.Text.Trim();
                    son.FMODEL = txt_Spec.Text.Trim();
                    son.FUNITID = 0;
                    son.FUNITID_UNITNAME = "暂无";
                    if (BaseCommon.StockOutSaveData(son, BaseCommon.StockOutMaterials))
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
            }
        }

        public bool OneOrderIn()
        {
            ud = new UrlTypeData();
            ud.Type = (int)CheckEnum.Edit;
            ud.c = "Acc.Business.WMS.XHY.Controllers.ProduceInOrderController";
            ud.m = "AddInSequence";
            ud.LoadItem = "";
            SessionModel.DtStockInfo = BaseCommon.GetStockInfoMaterialsBywarehouse(sm.PortsId.ToString(), sm.NameId.ToString(), sm.WhourseId.ToString(), WHid, txt_Batch.Text);
            List<InSequenceList> ISL = new List<InSequenceList>();
            for (int i = 0; i < lv_shInfo.Items.Count; i++)
            {
                InSequenceList InSequences = new InSequenceList();
                InSequences.InOrderId = sm.OrderId;
                InSequences.OutOrderMATERIALID = Convert.ToInt32(MingXiList);
                InSequences.SEQUENCECODE = lv_shInfo.Items[i].SubItems[1].Text;
                InSequences.STOCKINFOMATERIALSID = Convert.ToInt32(strStockinfoID);
                ISL.Add(InSequences);
            }
            ud.LoadItem = UIHelper.InSequenceListConversion(ISL);
            if (ToJson.InSequenceInExecuteMethod(ud) != "Y")
                return false;
            return true;
        }

        public void WiFiSaveData()
        {
            SetValues();
            if (UIHelper.QuestionMsg("确认保存？") == DialogResult.Yes)
            {
                ud = new UrlTypeData();
                ud.Type = (int)CheckEnum.Edit;
                ud.c = sm.ControllerName;
                ud.m = BaseCommon.Edit;
                if (string.IsNullOrEmpty(txt_TrayCode.Text.Trim()))
                    sm.PortsId = 0;
                string strKCid = "0";
                //监管码
                string jianguanma = "";
                if (this.Owner.Text == BaseCommon.ProductsSellOut)
                {
                    jianguanma = ",'Acc.Business.WMS.Model.OutInSequence':[";
                    for (int i = 0; i < lv_shInfo.Items.Count; i++)
                    {
                        if (i == 0)
                            jianguanma += "{'SEQUENCECODE':'" + lv_shInfo.Items[i].SubItems[1].Text + "','StateBase':0}";
                        else
                            jianguanma += ",{'SequenceCode':'" + lv_shInfo.Items[i].SubItems[1].Text + "','StateBase':0}";
                    }
                    jianguanma += "]";
                }
                if (SessionModel.DtStockInfo.Rows.Count > 0)
                    strKCid = SessionModel.DtStockInfo.Rows[0]["CODE"].ToString();
                ud.LoadItem = "{'GetOldObject':{'ID':'" + sm.OrderId.ToString() + "','CODE':'" + txt_OrderCode.Text.Trim() + "','SourceCode':'" + strSourceCode + "','TOWHNO':'" + WHid + "'},'Acc.Business.WMS.Model.StockOutOrderMaterials':[{'PORTNAME':'" + sm.PortsId + "','MATERIALCODE':'" + sm.NameId + "','MCODE':'" + txt_Code.Text.Trim() + "','FMODEL':'" + txt_Spec.Text.Trim() + "','BATCHNO':'" + ToJson.UrlEncode(txt_Batch.Text.Trim()) + "','DEPOTWBS':'" + sm.WhourseId + "','NUM':'" + txt_Num.Text.Trim() + "','SourceRowID':" + GetSourceId(strKCid) + ",'StateBase':0}]}";
                if (this.Owner.Text == BaseCommon.ProductsSellOut || this.Owner.Text == BaseCommon.ProductsElseOut && BaseCommon.CheckMaterialSequenState(txt_Code.Text))
                {
                    if (this.Owner.Text == BaseCommon.ProductsElseOut)
                        ud.LoadItem = "{'GetOldObject':{'ID':'" + sm.OrderId.ToString() + "','CODE':'" + txt_OrderCode.Text.Trim() + "','SourceCode':'" + strSourceCode + "','TOWHNO':'" + WHid + "'},'Acc.Business.WMS.Model.StockOutOrderMaterials':[{'PORTNAME':'" + sm.PortsId + "','MATERIALCODE':'" + sm.NameId + "','MCODE':'" + txt_Code.Text.Trim() + "','FMODEL':'" + txt_Spec.Text.Trim() + "','BATCHNO':'" + ToJson.UrlEncode(txt_Batch.Text.Trim()) + "','DEPOTWBS':'" + sm.WhourseId + "','NUM':'" + txt_Num.Text.Trim() + "','SourceRowID':" + GetSourceId(strKCid) + ",'StateBase':0,'ID':0}]}";
                    if (this.Owner.Text == BaseCommon.ProductsSellOut)
                        ud.LoadItem = "{'GetOldObject':{'ID':'" + sm.OrderId.ToString() + "','CODE':'" + txt_OrderCode.Text.Trim() + "','SourceCode':'" + strSourceCode + "','TOWHNO':'" + WHid + "'},'Acc.Business.WMS.Model.StockOutOrderMaterials':[{'PORTNAME':'" + sm.PortsId + "','MATERIALCODE':'" + sm.NameId + "','MCODE':'" + txt_Code.Text.Trim() + "','FMODEL':'" + txt_Spec.Text.Trim() + "','BATCHNO':'" + ToJson.UrlEncode(txt_Batch.Text.Trim()) + "','DEPOTWBS':'" + sm.WhourseId + "','NUM':'" + txt_Num.Text.Trim() + "','SourceRowID':" + GetSourceId(strKCid) + ",'StateBase':3,'STAY4':'已确认','ID':'" + MingXiList + "'" + jianguanma + "}]}";
                    MingXiList = ToJson.OnOrderInExecuteMethod(ud);
                    sm.strResult = "Y";
                    //sm.strResult = ToJson.ExecuteMethod(ud);
                    //if (sm.strResult != "Y")
                    //{
                    //    UIHelper.PromptMsg(sm.strResult);
                    //    return;
                    //}
                    /*
                    if(BaseCommon.CheckMaterialSequenState(txt_Code.Text.Trim()))
                    {
                        if (OneOrderIn())
                            sm.strResult = "Y";
                    }
                     */
                }
                else
                {
                    sm.strResult = ToJson.ExecuteMethod(ud);
                }
                if (sm.strResult == "Y")
                {
                    UIHelper.PromptMsg(InfoMessage.SaveSuccess);
                    Clear();
                    IsEnabled(true, Color.White);
                    txt_Code.Focus();
                    BindLv_OrderList(BaseCommon.GetOrderNoticeInfo(sm.SourceCodeId.ToString(), sm.ControllerName, this.Owner.Text));
                    BindLv_order();
                    if (isTihuan == "1" && this.Owner.Text == BaseCommon.ProductsSellOut)
                    {
                        isTihuan = "";
                        txt_Num.Enabled = false;
                        txt_Num.BackColor = Color.Aquamarine;
                    }
                }
                else
                {
                    UIHelper.ErrorMsg(InfoMessage.SaveFailed);
                }
            }
        }
        #endregion

        #region 数据提交
        private void menuItemSubmmit_Click(object sender, EventArgs e)
        {
            try
            {
                //重新绑定记录
                BindLv_order();
                if (lv_RecordList.Items.Count <= 0 && this.Owner.Text != BaseCommon.ProductsSellOut)
                {
                    UIHelper.PromptMsg(InfoMessage.NotSubmmitData);
                    return;
                }
                if (UIHelper.QuestionMsg("确认提交？") == DialogResult.Yes)
                {
                    ud = new UrlTypeData();
                    ud.c = sm.ControllerName;
                    ud.m = BaseCommon.SubmmitData;
                    ud.LoadItem = "{'ID':'" + sm.OrderId.ToString() + "','TOWHNO':'" + WHid + "','StateBase':3}";
                    sm.strResult = ToJson.ExecuteMethod(ud);
                    if (sm.strResult == "Y")
                    {
                        UIHelper.PromptMsg(InfoMessage.SubmmitSuccess);
                        lv_RecordList.Items.Clear();
                        lv_OrderList.Items.Clear();
                        lv_shInfo.Items.Clear();
                        txt_OrderCode.Text = txt_OrderDesc.Text = txt_WhName.Text = "";
                    }
                    else
                    {
                        UIHelper.ErrorMsg(InfoMessage.SubmmitFailed);
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

        #region 散件出库
        /// <summary>
        /// 散件出库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_Code_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == 13)
                {
                    if (!string.IsNullOrEmpty(txt_Code.Text.Trim()))
                    {
                        if (PDASet.IsOff)
                        {
                            if (lv_OrderList.Items.Count >= 1 && !string.IsNullOrEmpty(txt_OrderCode.Text.Trim()))
                            {
                                for (int i = 0; i < lv_OrderList.Items.Count; i++)
                                {
                                    if (lv_OrderList.Items[i].SubItems[1].Text == txt_Code.Text.Trim() && txt_Batch.Text == lv_OrderList.Items[i].SubItems[7].Text)
                                    {
                                        txt_Desc.Text = lv_OrderList.Items[i].SubItems[2].Text;
                                        txt_StockNum.Text = lv_OrderList.Items[i].SubItems[3].Text;
                                        txt_Uom.Text = lv_OrderList.Items[i].SubItems[4].Text;
                                        txt_Spec.Text = lv_OrderList.Items[i].SubItems[5].Text;
                                        strBatchno = txt_Batch.Text = lv_OrderList.Items[i].SubItems[7].Text;
                                        sm.SourceRowId = Convert.ToInt32(lv_OrderList.Items[i].SubItems[6].Text);
                                        break;
                                    }
                                    else
                                    {
                                        if (i == (lv_OrderList.Items.Count - 1))
                                        {
                                            UIHelper.ErrorMsg(InfoMessage.CodeNotOrder);
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (this.Owner.Text != BaseCommon.ProductsSellOut && lv_OrderList.Items.Count >= 0 && !string.IsNullOrEmpty(txt_OrderCode.Text.Trim()))
                            {
                                for (int i = 0; i < lv_OrderList.Items.Count; i++)
                                {
                                    if (lv_OrderList.Items[i].SubItems[1].Text == txt_Code.Text.Trim() && txt_Batch.Text == lv_OrderList.Items[i].SubItems[7].Text)
                                    {
                                        txt_StockNum.Text = lv_OrderList.Items[i].SubItems[3].Text;
                                        sm.SourceRowId = Convert.ToInt32(lv_OrderList.Items[i].SubItems[6].Text);
                                        txt_Batch.Text = lv_OrderList.Items[i].SubItems[7].Text;
                                        break;
                                    }
                                    else
                                    {
                                        if (i == (lv_OrderList.Items.Count - 1))
                                        {
                                            UIHelper.ErrorMsg(InfoMessage.CodeNotOrder);
                                            return;
                                        }
                                    }
                                }
                                CheckTxtCode();
                            }
                        }
                    }
                    else
                    {
                        UIHelper.PromptMsg(InfoMessage.NotCode);
                    }
                    if (this.Owner.Text == BaseCommon.ProductsSellOut)
                    {
                        if (CheckMaterialsID())
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

        public bool CheckMaterials()
        {
            if (this.Owner.Text == BaseCommon.ProductsSellOut)
            {
                if (lv_RecordList.Items.Count >= 1 && !string.IsNullOrEmpty(txt_OrderCode.Text.Trim()))
                {
                    for (int i = 0; i < lv_RecordList.Items.Count; i++)
                    {
                        if (lv_RecordList.Items[i].SubItems[2].Text == txt_Code.Text.Trim())
                        {
                            return true;
                        }
                    }
                }
            }
            else if (this.Owner.Text == BaseCommon.ProductsElseOut)
                return true;
            else
            {
                if (lv_OrderList.Items.Count >= 1 && !string.IsNullOrEmpty(txt_OrderCode.Text.Trim()))
                {
                    for (int i = 0; i < lv_OrderList.Items.Count; i++)
                    {
                        if (lv_OrderList.Items[i].SubItems[1].Text == txt_Code.Text.Trim())
                        {
                            return true;
                        }
                    }
                }
            }
            UIHelper.ErrorMsg(InfoMessage.CodeNotOrder);
            return false;
        }

        private bool CheckMaterialsID()
        {
            SessionModel.DtMaterialsInfo = BaseCommon.GetMaterialsInfo(txt_Code.Text.Trim());
            if (SessionModel.DtMaterialsInfo != null && SessionModel.DtMaterialsInfo.Rows.Count > 0)
            {
                sm.NameId = Convert.ToInt32(SessionModel.DtMaterialsInfo.Rows[0]["ID"].ToString());
                txt_Desc.Text = SessionModel.DtMaterialsInfo.Rows[0]["FNAME"].ToString();
                txt_TrayCode.Focus();
                return true;
            }
            else
            {
                UIHelper.PromptMsg("产品编码无效,请正确输入!");
                txt_Code.Focus();
                txt_Code.SelectionStart = txt_Code.TextLength;
                return false;
            }
        }


        public void CheckTxtCode()
        {
            SessionModel.DtMaterialsInfo = BaseCommon.GetMaterialsInfo(txt_Code.Text.Trim());
            if (SessionModel.DtMaterialsInfo != null && SessionModel.DtMaterialsInfo.Rows.Count > 0)
            {
                sm.NameId = Convert.ToInt32(SessionModel.DtMaterialsInfo.Rows[0]["ID"].ToString());
                SessionModel.DtStockInfo = BaseCommon.GetStockInfoMaterials(null, sm.NameId.ToString(), null);
                if (SessionModel.DtStockInfo.Rows.Count > 0)
                {
                    DataRow[] dr = SessionModel.DtStockInfo.Select("", "LASTINTIME asc");
                    DataTable dtn = new DataTable();
                    dtn = SessionModel.DtStockInfo.Clone();//克隆A的结构
                    foreach (DataRow row in dr)
                    {
                        dtn.ImportRow(row);//复制行数据
                    }
                    SessionModel.DtStockInfo = dtn;
                    if (SessionModel.DtStockInfo != null && SessionModel.DtStockInfo.Rows.Count > 0)
                    {
                        #region 注释代码
                        //if (dtMaterialsInfo.Rows[0]["SEQUENCECODE"].ToString() == "True")
                        //{
                        //    txt_AssetsCode.Text = txt_Code.Text;
                        //    tabControlSet.SelectedIndex = 2;
                        //}
                        //if (dtStockInfo.Rows.Count > 1)
                        //{
                        //    if (dtStockInfo.Rows[0]["BATCHNO"].ToString())
                        //    {
                        //        txt_Batch.Enabled = false;
                        //        Btn_Select.Enabled = false;
                        //        txt_Batch.BackColor = Color.Aquamarine;
                        //    }
                        //}
                        #endregion

                        txt_TrayCode.Text = SessionModel.DtStockInfo.Rows[0]["PORTCODE_PORTNO"].ToString() == null ? "" : SessionModel.DtStockInfo.Rows[0]["PORTCODE_PORTNO"].ToString();
                        //txt_Batch.Text = SessionModel.DtStockInfo.Rows[0]["BATCHNO"].ToString();
                        txt_Desc.Text = SessionModel.DtStockInfo.Rows[0]["CODE_FNAME"].ToString();
                        if (this.Owner.Text == BaseCommon.ProductsSellOut)
                            txt_Uom.Text = "件";
                        else
                            txt_Uom.Text = "KG";
                        txt_Spec.Text = SessionModel.DtStockInfo.Rows[0]["FMODEL"].ToString();
                        txt_Bin.Text = SessionModel.DtStockInfo.Rows[0]["DEPOTWBS_CODE"].ToString();
                        txt_Bin.Focus();
                    }
                    else
                    {
                        UIHelper.PromptMsg(InfoMessage.CodeNotStockInfo);
                        txt_Code.SelectAll();
                    }
                }
                else
                {
                    UIHelper.PromptMsg("仓库不存在该产品！");
                    txt_Code.Focus();
                    txt_Code.SelectionStart = txt_Code.TextLength;
                }
            }
            else
            {
                UIHelper.PromptMsg(InfoMessage.NotFoundCode);
                txt_Code.SelectAll();
            }
        }
        #endregion

        #region 权限控制
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

        #region 按托盘出库
        private void txt_TrayCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == 13)
                {
                    BindLv_order();
                    string ControllerName = string.Empty;
                    if (this.Owner.Text == BaseCommon.ProductsSellOut)
                    {
                        for (int i = 0; i < lv_RecordList.Items.Count; i++)
                        {
                            if (txt_TrayCode.Text == lv_RecordList.Items[i].SubItems[1].Text)
                            {
                                if (lv_RecordList.Items[i].SubItems[10].Text == "已确认")
                                {
                                    MessageBox.Show("该托盘已经出库！");
                                    return;
                                }
                            }
                        }
                    }
                    SetValues();
                }
            }
            catch (Exception ex)
            {
                UIHelper.ErrorMsg(ex.Message);
                return;
            }
        }

        private void SetValues()
        {
            SessionModel.DtPortsInfo = BaseCommon.GetPortsInfo(txt_TrayCode.Text.Trim());
            if (SessionModel.DtPortsInfo != null && SessionModel.DtPortsInfo.Rows.Count > 0)
            {
                sm.PortsId = Convert.ToInt32(SessionModel.DtPortsInfo.Rows[0]["ID"].ToString());
                SessionModel.DtStockInfo = BaseCommon.GetStockInfoMaterials(sm.PortsId.ToString(), sm.NameId.ToString(), sm.WhourseId.ToString());
                if (SessionModel.DtStockInfo != null && SessionModel.DtStockInfo.Rows.Count > 0)
                {
                    //判断是否是销售出库
                    if (this.Owner.Text == BaseCommon.ProductsSellOut)
                    {
                        bool CheckB = true;
                        for (int i = 0; i < lv_RecordList.Items.Count; i++)
                        {
                            //循环销售出库记录，如果产品名称和托盘相同的话取出记录里面相应的数据
                            if (txt_TrayCode.Text == lv_RecordList.Items[i].SubItems[1].Text && txt_Code.Text.Trim() == lv_RecordList.Items[i].SubItems[2].Text)
                            {
                                txt_Code.Text = lv_RecordList.Items[i].SubItems[2].Text;
                                txt_Desc.Text = lv_RecordList.Items[i].SubItems[3].Text;
                                txt_Num.Text = lv_RecordList.Items[i].SubItems[4].Text;
                                txt_Bin.Text = lv_RecordList.Items[i].SubItems[5].Text;
                                txt_Batch.Text = lv_RecordList.Items[i].SubItems[6].Text;
                                txt_Uom.Text = lv_RecordList.Items[i].SubItems[7].Text;
                                txt_Spec.Text = lv_RecordList.Items[i].SubItems[8].Text;
                                MingXiList = lv_RecordList.Items[i].SubItems[9].Text;
                                CheckB = false;
                            }
                        }
                        if (CheckB == true)
                        {
                            if (string.IsNullOrEmpty(isTihuan))
                            {
                                UIHelper.ErrorMsg("该托盘不存在该单据中！");
                                return;
                            }
                        }
                    }
                    else
                    {
                        //SessionModel.DtStockInfo = BaseCommon.GetStockInfoMaterialsBywarehouse(SessionModel.DtPortsInfo.Rows[0]["ID"].ToString(), sm.NameId.ToString(), null, WHid, txt_Batch.Text);
                        if (SessionModel.DtStockInfo.Rows.Count > 0)
                        {
                            txt_Bin.Text = SessionModel.DtStockInfo.Rows[0]["DEPOTWBS_CODE"].ToString();
                        }
                        else
                            txt_Bin.Text = "请扫描货位！";

                        //IsEnabled(false, Color.Aquamarine);
                        txt_Code.Text = SessionModel.DtStockInfo.Rows[0]["MCODE"].ToString();
                        txt_Desc.Text = SessionModel.DtStockInfo.Rows[0]["CODE_FNAME"].ToString();
                        txt_Batch.Text = SessionModel.DtStockInfo.Rows[0]["BATCHNO"].ToString();
                        txt_Spec.Text = SessionModel.DtStockInfo.Rows[0]["FMODEL"].ToString();
                        //txt_Bin.Text = SessionModel.DtStockInfo.Rows[0]["DEPOTWBS_CODE"].ToString();
                        //txt_Num.Text = SessionModel.DtStockInfo.Rows[0]["NUM"].ToString();
                    }
                }
                else
                {
                    #region 4月1号后放开
                    /*
                    UIHelper.PromptMsg(InfoMessage.TrayCodeNotStock);
                    IsEnabled(true, Color.White);
                    txt_TrayCode.Focus();
                     */
                    #endregion
                }
            }
            else
            {
                UIHelper.ErrorMsg(InfoMessage.NotTrayCode);
                return;
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
            if (!UIHelper.CheckTextBox(txt_Code.Text.Trim()))
            {
                UIHelper.ErrorMsg(InfoMessage.InputOrScanningCode);
                txt_TrayCode.Focus();
                return false;
            }
            /*
            if (this.Owner.Text != BaseCommon.ProductsPicOut)
            {
                if (!UIHelper.CheckTextBox(txt_TrayCode.Text.Trim()))
                {
                    UIHelper.ErrorMsg(InfoMessage.InputOrScanningTrayCode);
                    txt_TrayCode.Focus();
                    return false;
                }
            }
             */
            if (!UIHelper.CheckTextBox(txt_Bin.Text.Trim()))
            {
                txt_Bin.Text = InfoMessage.InputOrScanningBinCode;
                txt_Bin.SelectAll();
                txt_Bin.Focus();
                return false;
            }
            if (this.Owner.Text == BaseCommon.ProductsSellOut)
            {
                if (CheckChecked())
                    return false;
            }
            if (!CheckMaterials())
            {
                return false;
            }
            if (!CheckMaterialsID())
                return false;
            if (!GetWhourseId(txt_Bin.Text.Trim()))
            {
                return false;
            }
            if (this.Owner.Text == BaseCommon.ProductsSellOut)
            {
                if (!CheckPorts())
                    return false;
            }
            else
            {
                if (!string.IsNullOrEmpty(txt_TrayCode.Text.Trim()))
                {
                    if (!CheckPorts())
                        return false;
                }
            }
            if (!UIHelper.CheckNum(txt_Num.Text.Trim()))
            {
                UIHelper.ErrorMsg(InfoMessage.Number);
                return false;
            }
            if (this.Owner.Text != BaseCommon.ProductsSellOut && this.Owner.Text != BaseCommon.ProductsElseOut)
            {
                if (Convert.ToDouble(txt_Num.Text.Trim()) > Convert.ToDouble(txt_StockNum.Text.Trim()))
                {
                    UIHelper.ErrorMsg("出库数量大于托盘存放量，操作终止！");
                    return false;
                }
            }
            if (this.Owner.Text == BaseCommon.ProductsSellOut || this.Owner.Text == BaseCommon.ProductsElseOut && BaseCommon.CheckMaterialSequenState(txt_Code.Text))
            {
                DataTable dt = BaseCommon.GetStockInfoMaterialsBywarehouse(sm.PortsId.ToString(), sm.NameId.ToString(), sm.WhourseId.ToString(), WHid, txt_Batch.Text);
                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToDouble(txt_Num.Text.Trim()) > Convert.ToDouble(dt.Rows[0]["NUM"].ToString()))
                    {
                        UIHelper.ErrorMsg("出库数量大于托盘存放量，操作终止！");
                        return false;
                    }
                    if (Convert.ToDouble(txt_Num.Text.Trim()) < Convert.ToDouble(dt.Rows[0]["NUM"].ToString()) && Convert.ToDouble(txt_Num.Text.Trim()) != Convert.ToDouble(lv_shInfo.Items.Count))
                    {
                        if (this.Owner.Text == BaseCommon.ProductsElseOut)
                            tabControlSet.SelectedIndex = 1;
                        else
                            tabControlSet.SelectedIndex = 2;
                        return false;
                    }
                    if (Convert.ToDouble(txt_Num.Text.Trim()) == Convert.ToDouble(dt.Rows[0]["NUM"].ToString()))
                    {
                        SessionModel.DtPortsInfo = BaseCommon.GetPortsInfo(txt_TrayCode.Text.Trim());
                        sm.PortsId = Convert.ToInt32(SessionModel.DtPortsInfo.Rows[0]["ID"].ToString());
                        SessionModel.DtWHourseInfo = BaseCommon.GetWhouseInfo(txt_Bin.Text.Trim(), WHid);
                        sm.WhourseId = Convert.ToInt32(SessionModel.DtWHourseInfo.Rows[0]["ID"].ToString());
                        SessionModel.DtMaterialsInfo = BaseCommon.GetMaterialsInfo(txt_Code.Text);
                        sm.NameId = Convert.ToInt32(SessionModel.DtMaterialsInfo.Rows[0]["ID"].ToString());
                        ud = new UrlTypeData();
                        ud.Type = 2;
                        ud.c = "Acc.Business.WMS.XHY.Controllers.ProduceInOrderController";
                        ud.m = "GetInfoSequenceList";
                        ud.LoadItem = "{}&MATERIALCODE=" + sm.NameId + "&batchno=" + txt_Batch.Text.Trim() + "&WAREHOUSEID=" + WHid + "&DEPOTWBS=" + sm.WhourseId + "&PORTCODE=" + sm.PortsId;
                        BindLv_shInfoList(ToJson.getData(ud));
                    }
                }
                if (Convert.ToDouble(txt_Num.Text.Trim()) != Convert.ToDouble(lv_shInfo.Items.Count))
                {
                    UIHelper.ErrorMsg("序列码件数与出库数量不匹配！");
                    return false;
                }
            }
            return true;
        }

        private bool CheckChecked()
        {
            if (lv_RecordList.Items.Count > 0)
            {
                for (int i = 0; i < lv_RecordList.Items.Count; i++)
                {
                    if (txt_TrayCode.Text.Trim() == lv_RecordList.Items[i].SubItems[1].Text && txt_Code.Text == lv_RecordList.Items[i].SubItems[2].Text && txt_Bin.Text.Trim() == lv_RecordList.Items[i].SubItems[5].Text && txt_Batch.Text == lv_RecordList.Items[i].SubItems[6].Text)
                    {
                        if (lv_RecordList.Items[i].SubItems[11].Text == "未质检")
                        {
                            UIHelper.ErrorMsg("该批次未质检，操作终止！");
                            return true;
                        }
                    }
                }
            }
            return false;
        }


        private bool CheckPorts()
        {
            SessionModel.DtPortsInfo = BaseCommon.GetPortsInfo(txt_TrayCode.Text.Trim());
            if (SessionModel.DtPortsInfo != null && SessionModel.DtPortsInfo.Rows.Count > 0)
            {
                sm.PortsId = Convert.ToInt32(SessionModel.DtPortsInfo.Rows[0]["ID"].ToString());
                SessionModel.DtStockInfo = BaseCommon.GetStockInfoMaterialsBywarehouse(sm.PortsId.ToString(), sm.NameId.ToString(), sm.WhourseId.ToString(), WHid, txt_Batch.Text);
                if (SessionModel.DtStockInfo != null && SessionModel.DtStockInfo.Rows.Count > 0)
                {
                    strStockinfoID = SessionModel.DtStockInfo.Rows[0]["ID"].ToString();
                    if (SessionModel.DtStockInfo.Rows[0]["Code"].ToString() != sm.NameId.ToString())
                    {
                        UIHelper.PromptMsg("该托盘上产品不等于扫描产品，操作终止！");
                        return false;
                    }
                }
                #region 4月1号后放开
                /*
                //else
                //{
                //    UIHelper.PromptMsg(InfoMessage.TrayCodeNotStock);
                //    return false;
                //}
                 */
                #endregion
            }
            else
            {
                UIHelper.ErrorMsg(InfoMessage.NotTrayCode);
                return false;
            }
            return true;
        }

        #endregion

        #region 单件序列码管理
        #endregion

        #region 属性控制
        public void IsEnabled(bool Falg, Color co)
        {
            txt_Code.Enabled = Falg;
            txt_Code.BackColor = co;
            txt_Batch.Enabled = Falg;
            txt_Batch.BackColor = co;
            txt_Bin.Enabled = Falg;
            txt_Bin.BackColor = co;
        }

        private void Clear()
        {
            txt_TrayCode.Text = txt_Code.Text = txt_Desc.Text = txt_Batch.Text = txt_Uom.Text = txt_Bin.Text = txt_Num.Text = txt_Spec.Text = txt_StockNum.Text = "";
            //txt_Batch.Enabled = true;
            //Btn_Select.Enabled = true;
            lv_RecordList.Items.Clear();
            lv_OrderList.Items.Clear();
            lv_shInfo.Items.Clear();
            SessionModel.Clear();
        }
        #endregion

        #region 数据删除
        /// <summary>
        /// 数据删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lv_RecordList_ItemActivate(object sender, EventArgs e)
        {
            if (lv_RecordList.Items.Count < 0)
            {
                UIHelper.ErrorMsg(InfoMessage.NotDeleteData);
                return;
            }
            if (UIHelper.QuestionMsg("确认删除？") == DialogResult.Yes)
            {
                int ID = Convert.ToInt32(lv_RecordList.Items[lv_RecordList.SelectedIndices[0]].SubItems[9].Text);
                if (this.lv_RecordList.Items.Count > 0 && this.lv_RecordList.SelectedIndices.Count > 0)
                {
                    ud = new UrlTypeData();
                    ud.Type = (int)CheckEnum.Edit;
                    ud.c = sm.ControllerName;
                    ud.m = BaseCommon.Edit;
                    ud.LoadItem = "{'GetOldObject':{'ID':'" + sm.OrderId.ToString() + "','CODE':'" + txt_OrderCode.Text.Trim() + "'},'Acc.Business.WMS.Model.StockOutOrderMaterials':[{'ID':" + ID + ",'StateBase':2}]}";
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
                else
                {
                    UIHelper.ErrorMsg(InfoMessage.NotDeleteData);
                    return;
                }
            }
            else
                lv_RecordList_ItemActivate_1(null, null);
        }
        #endregion

        #region KeyPress事件
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

        private void txt_Batch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txt_Bin.Focus();
            }
        }
        #endregion

        #region 获取存储位置信息
        private bool GetWhourseId(string BinCode)
        {
            DataTable dt = BaseCommon.GetWhouseInfo(BinCode, WHid);
            if (dt != null && dt.Rows.Count > 0)
            {
                sm.WhourseId = Convert.ToInt32(dt.Rows[0]["ID"].ToString());
                txt_Num.Focus();
                return true;
            }
            else
            {
                UIHelper.ErrorMsg("此货位不存在或者不属于当前仓库！");
                txt_Bin.SelectAll();
            }
            return false;
        }
        #endregion

        #region TabControl选项卡控制
        private void tabControlSet_SelectedIndexChanged(object sender, EventArgs e)
        {
            int result = tabControlSet.SelectedIndex;
            switch (tabControlSet.TabPages[tabControlSet.SelectedIndex].Text)
            {
                case "操作":
                    break;
                case "单件":
                    break;
                case "记录":
                    if (!string.IsNullOrEmpty(txt_OrderCode.Text.Trim()))
                    {
                        BindLv_order();
                    }
                    else
                    {
                        //UIHelper.ErrorMsg(InfoMessage.SelectOrder);
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
                UIHelper.ErrorMsg("请正确选择出库单号！");
                tabControlSet.SelectedIndex = 1;
                return false;
            }
            return true;
        }
        #endregion

        #region 退出界面
        private void Frm_ProductsOut_Closing(object sender, CancelEventArgs e)
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

        #region 单件出库回车事件
        private void txt_AssetsCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == 13 && txt_AssetsCode.Text.Trim() != "")
                {
                    if (txt_AssetsCode.Text.Trim().Length != 20)
                    {
                        UIHelper.ErrorMsg("请正确输入单件码！");
                        return;
                    }
                    for (int i = 0; i < lv_shInfo.Items.Count; i++)
                    {
                        if (txt_AssetsCode.Text == lv_shInfo.Items[i].SubItems[1].Text)
                        {
                            UIHelper.ErrorMsg("已经扫描该单件码！");
                            txt_AssetsCode.Text = "";
                            return;
                        }
                    }
                    ud = new UrlTypeData();
                    ud.Type = 2;
                    ud.c = "Acc.Business.WMS.XHY.Controllers.ProduceInOrderController";
                    ud.m = "GetInfoSequenceListById";
                    SessionModel.DtPortsInfo = BaseCommon.GetPortsInfo(txt_TrayCode.Text.Trim());
                    if (SessionModel.DtPortsInfo.Rows.Count > 0)
                    {
                        SessionModel.DtStockInfo = BaseCommon.GetStockInfoMaterials(SessionModel.DtPortsInfo.Rows[0]["ID"].ToString(), sm.NameId.ToString(), sm.WhourseId.ToString());
                        ud.LoadItem = "{}&CPorderid=" + SessionModel.DtStockInfo.Rows[0]["ID"].ToString() + "&SEcode=" + txt_AssetsCode.Text.Trim();
                        DataTable dt = ToJson.getData(ud);
                        if (dt != null)
                        {
                            if (dt.Rows.Count == 0)
                            {
                                UIHelper.ErrorMsg("该单件码不存在或者不存在该货位下！");
                                txt_AssetsCode.SelectAll();
                                txt_AssetsCode.Focus();
                                txt_AssetsCode.SelectionStart = txt_AssetsCode.TextLength;
                                return;
                            }
                        }
                        int i = lv_shInfo.Items.Count;
                        i = i + 1;
                        ListViewItem list = new ListViewItem();
                        list.Text = i.ToString();
                        list.SubItems.Add(txt_AssetsCode.Text.Trim().ToString());
                        lv_shInfo.Items.Add(list);
                        txt_AssetsCode.Text = "";
                    }
                    else
                        UIHelper.ErrorMsg("托盘码不存在！");
                }
            }
            catch (Exception ex)
            {
                UIHelper.ErrorMsg(ex.Message);
                return;
            }
        }
        #endregion

        private string GetSourceId(string strMaterial)
        {
            for (int i = 0; i < lv_OrderList.Items.Count; i++)
            {
                if (strMaterial == lv_OrderList.Items[i].SubItems[7].Text)
                {
                    return lv_OrderList.Items[i].SubItems[6].Text;
                }
            }
            return "0";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                SessionModel.DtOrderMaterialsInfo = BaseCommon.GetOrderMaterialsInfo(sm.OrderId.ToString(), sm.ControllerName, BaseCommon.ProductsOut, BaseCommon.StockOutMaterials);
                Frm_BestStore fs = null;
                if (this.Owner.Text != BaseCommon.ProductsSellOut)
                {
                    if (QueryData())
                    {
                        fs = new Frm_BestStore("");
                    }
                    else
                    {
                        UIHelper.ErrorMsg("库存中不存在该产品！");
                        return;
                    }

                }
                else
                {
                    fs = new Frm_BestStore("1");
                }
                DialogResult ret = fs.ShowDialog();
                if (fs.WhName != null)
                {
                    if (string.IsNullOrEmpty(strBatchno))
                        txt_Batch.Text = fs.StrBatch;
                    else
                    {
                        if (strBatchno != fs.StrBatch && this.Owner.Text != BaseCommon.ProductsSellOut)
                        {
                            UIHelper.ErrorMsg("选择数据的批次不正确，请重新选择！");
                            txt_TrayCode.Text = "";
                            txt_Bin.Text = "";
                            return;
                        }
                    }
                    txt_TrayCode.Text = fs.WhCode;
                    txt_Bin.Text = fs.WhName;
                    txt_Code.Text = fs.StrMaterialsCode;
                    txt_Desc.Text = fs.StrMaterialsNmae;
                    txt_Num.Text = fs.StrNum;
                    //SetValues();
                }
            }
            catch (Exception ex)
            {
                UIHelper.ErrorMsg(ex.Message);
                return;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                SessionModel.DtOrderMaterialsInfo = BaseCommon.GetOrderMaterialsInfo(sm.OrderId.ToString(), sm.ControllerName, BaseCommon.ProductsOut, BaseCommon.StockOutMaterials);
                Frm_BestStore fs = null;
                if (this.Owner.Text != BaseCommon.ProductsSellOut)
                {
                    if (QueryData())
                    {
                        fs = new Frm_BestStore("");
                    }
                    else
                    {
                        UIHelper.ErrorMsg("库存中不存在该产品！");
                        return;
                    }
                }
                else
                {
                    fs = new Frm_BestStore("1");
                }
                DialogResult ret = fs.ShowDialog();
                if (fs.WhName != null)
                {
                    if (string.IsNullOrEmpty(strBatchno))
                        txt_Batch.Text = fs.StrBatch;
                    else
                    {
                        if (strBatchno != fs.StrBatch && this.Owner.Text != BaseCommon.ProductsSellOut)
                        {
                            UIHelper.ErrorMsg("选择数据的批次不正确，请重新选择！");
                            txt_TrayCode.Text = "";
                            txt_Bin.Text = "";
                            return;
                        }
                    }
                    txt_TrayCode.Text = fs.WhCode;
                    txt_Bin.Text = fs.WhName;
                    txt_Code.Text = fs.StrMaterialsCode;
                    txt_Desc.Text = fs.StrMaterialsNmae;
                    txt_Num.Text = fs.StrNum;
                    //SetValues();
                }
            }
            catch (Exception ex)
            {
                UIHelper.ErrorMsg(ex.Message);
                return;
            }
        }

        private void lv_OrderList_ItemActivate(object sender, EventArgs e)
        {
            try
            {
                if (lv_OrderList.Items.Count > 0)
                {
                    txt_Code.Text = lv_OrderList.Items[lv_OrderList.SelectedIndices[0]].SubItems[1].Text;
                    txt_Desc.Text = lv_OrderList.Items[lv_OrderList.SelectedIndices[0]].SubItems[2].Text;
                    txt_StockNum.Text = lv_OrderList.Items[lv_OrderList.SelectedIndices[0]].SubItems[3].Text;
                    strBatchno = txt_Batch.Text = lv_OrderList.Items[lv_OrderList.SelectedIndices[0]].SubItems[7].Text;
                    txt_Uom.Text = "kg";
                    tabControlSet.SelectedIndex = 1;
                }
            }
            catch (Exception ex)
            {
                UIHelper.ErrorMsg(ex.Message);
                return;
            }
        }

        private void lv_RecordList_ItemActivate_1(object sender, EventArgs e)
        {
            try
            {
                if (lv_RecordList.Items.Count > 0 && this.Owner.Text == BaseCommon.ProductsSellOut)
                {
                    if (UIHelper.QuestionMsg("是否替换推荐信息？") == DialogResult.Yes)
                    {
                        isTihuan = "1";
                        txt_Num.Enabled = true;
                        txt_Num.BackColor = Color.White;
                        MingXiList = lv_RecordList.Items[lv_RecordList.SelectedIndices[0]].SubItems[9].Text;
                    }
                    txt_Code.Text = lv_RecordList.Items[lv_RecordList.SelectedIndices[0]].SubItems[2].Text;
                    txt_TrayCode.Text = lv_RecordList.Items[lv_RecordList.SelectedIndices[0]].SubItems[1].Text;
                    txt_Desc.Text = lv_RecordList.Items[lv_RecordList.SelectedIndices[0]].SubItems[3].Text;
                    txt_Batch.Text = lv_RecordList.Items[lv_RecordList.SelectedIndices[0]].SubItems[6].Text;
                    txt_Uom.Text = "件";
                    txt_Bin.Text = lv_RecordList.Items[lv_RecordList.SelectedIndices[0]].SubItems[5].Text;
                    txt_Num.Text = lv_RecordList.Items[lv_RecordList.SelectedIndices[0]].SubItems[4].Text;
                    tabControlSet.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                UIHelper.ErrorMsg(ex.Message);
                return;
            }
        }

        private bool QueryData()
        {
            SessionModel.DtMaterialsInfo = BaseCommon.GetMaterialsInfo(txt_Code.Text.Trim());
            if (SessionModel.DtMaterialsInfo != null && SessionModel.DtMaterialsInfo.Rows.Count > 0)
            {
                sm.NameId = Convert.ToInt32(SessionModel.DtMaterialsInfo.Rows[0]["ID"].ToString());
                SessionModel.DtStockInfo = BaseCommon.GetStockInfoMaterials(null, sm.NameId.ToString(), null);
                if (SessionModel.DtStockInfo.Rows.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }

        #region 删除序列码
        private void lv_shInfo_ItemActivate(object sender, EventArgs e)
        {
            try
            {
                if (UIHelper.QuestionMsg("确认删除此单件码？") == DialogResult.Yes)
                {
                    lv_shInfo.Items.RemoveAt(lv_shInfo.SelectedIndices[0]);
                    for (int i = 0; i < lv_shInfo.Items.Count; i++)
                    {
                        lv_shInfo.Items[i].SubItems[0].Text = (i + 1).ToString();
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
    }
}