using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AcctrueTerminal.Common;

namespace AcctrueTerminal.StockOut
{
    public partial class Frm_OffShelf : Form
    {
        //private DataTable dtTray = null;
        //private DataTable dtPorts = null;
        //private DataTable dtStockIn = null;
        //private int txtBinID;
        private SessionModel sm;
        private UrlTypeData ud;
        public Frm_OffShelf()
        {
            InitializeComponent();
        }
       
        private void txt_Bin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (string.IsNullOrEmpty(txt_Bin.Text.Trim()) || txt_Bin.Text.Trim() == InfoMessage.InputOrScanningBinCode)
                {
                    //UIHelper.PromptMsg(InfoMessage.InputOrScanningBinCode);
                    txt_Bin.Text = InfoMessage.InputOrScanningBinCode;
                    txt_Bin.SelectAll();
                    txt_Bin.Focus();
                }
                else
                {
                    DataTable dt = BaseCommon.GetWhouseInfo(txt_Bin.Text.Trim(),null);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        sm.WhourseId = Convert.ToInt32(dt.Rows[0]["ID"].ToString());
                    }
                    else
                    {
                        UIHelper.ErrorMsg(InfoMessage.BinCodeNotFound);
                        txt_Bin.SelectAll();
                    }
                }
            }
        }

        private void menuItemOffShelf_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_TrayCode.Text.Trim()) && lv_OffShelfData.Items.Count > 0)
            {
                if (!string.IsNullOrEmpty(txt_Bin.Text.Trim()))
                {
                    if (UIHelper.QuestionMsg("确认下架？") == DialogResult.Yes)
                    {
                        string LoadItem = "{'ID':'" + sm.PortsId + "','PORTNO':'" + txt_TrayCode.Text.Trim() + "','PORTNO':'" + sm.PortsName + "','STATUS',:'0','StateBase':3}";
                        if (BaseCommon.EditPortsState(LoadItem))
                        {
                            UIHelper.PromptMsg(InfoMessage.SaveSuccess);
                            Clear();
                        }
                        else
                        {
                            UIHelper.PromptMsg(InfoMessage.SaveSuccess);
                        }

                        #region 注释代码
                        //ud = new UrlTypeData();
                        //ud.c = "Acc.Business.WMS.Controllers.PortsController";
                        //ud.m = BaseCommon.Edit;
                        //ud.LoadItem = "{'GetOldObject':{'ID':'" + sm.StockId + "','DEPOTWBS',:'" + sm.DepotwbsId + "','StateBase':3}}";
                        //ud.LoadItem = "{'GetOldObject':{'ID':'" + sm.PortsId + "','PORTNO':'" + txt_TrayCode.Text.Trim() + "','STATUS',:'0','StateBase':3}}";
                        //sm.strResult = ToJson.ExecuteMethod(ud);
                        //if (sm.strResult == "Y")
                        //{
                        //    UIHelper.PromptMsg(InfoMessage.SaveSuccess);
                        //    Clear();
                        //}
                        //else
                        //{
                        //    UIHelper.PromptMsg(InfoMessage.SaveSuccess);
                        //}
                        #endregion
                    }                    
                }
                else
                {
                    txt_Bin.Text = InfoMessage.InputOrScanningBinCode;
                    txt_Bin.SelectAll();
                    txt_Bin.Focus();
                }
            }
            else
            {
                UIHelper.ErrorMsg(InfoMessage.NotGroupTray);
            }
        }
        public void BindLv_OffShelfData(DataTable dt)
        {
            lv_OffShelfData.Items.Clear();
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ListViewItem list = new ListViewItem((i + 1).ToString());
                    list.SubItems.Add(dt.Rows[i]["PORTCODE_PORTNO"].ToString());
                    list.SubItems.Add(dt.Rows[i]["MCODE"].ToString());
                    list.SubItems.Add(dt.Rows[i]["CODE_FNAME"].ToString());
                    list.SubItems.Add(dt.Rows[i]["NUM"].ToString());
                    list.SubItems.Add(dt.Rows[i]["BATCHNO"].ToString());
                    list.SubItems.Add("暂无");
                    list.SubItems.Add(dt.Rows[i]["FMODEL"].ToString());
                    list.SubItems.Add(dt.Rows[i]["ID"].ToString());
                    lv_OffShelfData.Items.Add(list);
                }
            }
        }

        private void txt_TrayCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (string.IsNullOrEmpty(txt_TrayCode.Text.Trim()) || txt_TrayCode.Text.Trim() == InfoMessage.InputOrScanningTrayCode)
                {
                    txt_TrayCode.Text = InfoMessage.InputOrScanningTrayCode;
                    txt_TrayCode.SelectAll();
                    txt_TrayCode.Focus();
                }
                else
                {
                    SessionModel.DtPortsInfo = BaseCommon.GetPortsInfo(txt_TrayCode.Text.Trim());
                    if (SessionModel.DtPortsInfo != null && SessionModel.DtPortsInfo.Rows.Count > 0)
                    {
                        sm.PortsId =Convert.ToInt32( SessionModel.DtPortsInfo.Rows[0]["ID"].ToString());
                        sm.PortsName = SessionModel.DtPortsInfo.Rows[0]["PORTNAME"].ToString();
                        SessionModel.DtStockInfo = BaseCommon.GetStockInfoMaterials(sm.PortsId.ToString(), null, null);
                        if (SessionModel.DtStockInfo != null && SessionModel.DtStockInfo.Rows.Count > 0)
                        {
                            BindLv_OffShelfData(SessionModel.DtStockInfo);
                            txt_Bin.Text = SessionModel.DtStockInfo.Rows[0]["DEPOTWBS_CODE"].ToString();
                        }
                        else
                        {
                            UIHelper.PromptMsg(InfoMessage.PortsNotStockInfo);
                        }
                    }
                    else
                    {
                        UIHelper.PromptMsg(InfoMessage.TrayCodeNotFound);
                    }
                }
            }
        }

        private void Clear()
        {
            SessionModel.Clear();
        }

        private void Frm_OffShelf_Load(object sender, EventArgs e)
        {
            sm = new SessionModel();
        }

        private void Frm_OffShelf_Closing(object sender, CancelEventArgs e)
        {
            DialogResult result;
            result = UIHelper.QuestionMsg("是否退出");
            if (result == DialogResult.Yes)
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }
    }
}