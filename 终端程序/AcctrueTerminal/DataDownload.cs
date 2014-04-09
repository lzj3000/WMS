using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AcctrueTerminal.Common;

namespace AcctrueTerminal
{
    public partial class DataDownload : Form
    {
        private SessionModel sm;
        private string ControlerName = string.Empty;
        public DataDownload()
        {
            InitializeComponent();
        }
        public void ShowText(string value)
        {
            try
            {
                txt_ShowMessage.Text = value;
                txt_ShowMessage.Refresh();
            }
            catch (Exception err)
            {
                throw new Exception(err.ToString());
            }
        }      

        #region 清除终端本地数据库数据
        /// <summary>
        /// 清除终端本地数据库数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItemClear_Click(object sender, EventArgs e)
        {
            if (UIHelper.QuestionMsg("确定要清除数据？") ==DialogResult.Yes)
            {
                if (cb_Materials.Checked)
                {
                    if (BaseCommon.DeletePDAData(BaseCommon.Materials,null))
                    {
                        ShowText(txt_ShowMessage.Text + InfoMessage.DeleteMaterialsEnd + "\r\n");
                    }
                }
                if (cb_WHourse.Checked)
                {
                    if (BaseCommon.DeletePDAData(BaseCommon.Whourse,null))
                    {
                        ShowText(txt_ShowMessage.Text + InfoMessage.DeleteMaterialsEnd + "\r\n");
                    }
                }
                if (cb_Ports.Checked)
                {
                    if (BaseCommon.DeletePDAData(BaseCommon.Ports,null))
                    {
                        ShowText(txt_ShowMessage.Text + InfoMessage.DeleteMaterialsEnd + "\r\n");
                    }
                }
                if (cb_StockIn.Checked)
                {
                    if (BaseCommon.DeletePDAData(BaseCommon.StockIn, BaseCommon.StockInNoticeMaterials))
                    {
                        ShowText(txt_ShowMessage.Text + InfoMessage.DeleteStockIn + "\r\n");
                    }
                }
                if (cb_StockOut.Checked)
                {
                    if (BaseCommon.DeletePDAData(BaseCommon.StockOut, BaseCommon.StockOutNoticeMaterials))
                    {
                        ShowText(txt_ShowMessage.Text + InfoMessage.DeleteStockOut + "\r\n");
                    }
                }
            }
        }
        #endregion

        #region 下载数据到终端本地数据库
        /// <summary>
        /// 下载数据到终端本地数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItemDownLoad_Click(object sender, EventArgs e)
        {
            if (!PDASet.IsOff)
            {
                #region 基础信息下载
                if (cb_Materials.Checked)//物料信息下载
                {
                    SessionModel.DtMaterialsInfo = BaseCommon.GetMaterialsInfo(null);
                    if (SessionModel.DtMaterialsInfo != null && SessionModel.DtMaterialsInfo.Rows.Count > 0)
                    {
                        if (BaseCommon.DownLoadMaterials(SessionModel.DtMaterialsInfo))
                        {
                            ShowText(txt_ShowMessage.Text + InfoMessage.DownLoadMaterialsEnd + "\r\n");
                            SessionModel.DtMaterialsInfo.Clear();
                            cb_Materials.Checked = false;
                        }
                    }
                    else
                    {
                        UIHelper.ErrorMsg(InfoMessage.NotDownLoadData);
                    }
                }
                if (cb_WHourse.Checked)//仓库信息下载
                {
                    SessionModel.DtWHourseInfo = BaseCommon.GetWhouseInfo(null,null);
                    if (SessionModel.DtWHourseInfo != null && SessionModel.DtWHourseInfo.Rows.Count > 0)
                    {
                        if (BaseCommon.DownLoadWhourse(SessionModel.DtWHourseInfo))
                        {
                            ShowText(txt_ShowMessage.Text + InfoMessage.DownLoadWhourseEnd + "\r\n");
                            SessionModel.DtWHourseInfo.Clear();
                            cb_WHourse.Checked = false;
                        }
                    }
                    else
                    {
                        UIHelper.ErrorMsg(InfoMessage.NotDownLoadData);
                    }
                }
                if (cb_Ports.Checked)//托盘信息下载
                {
                    SessionModel.DtPortsInfo = BaseCommon.GetPortsInfo(null);
                    if (SessionModel.DtPortsInfo != null && SessionModel.DtPortsInfo.Rows.Count > 0)
                    {
                        if (BaseCommon.DownLoadPorts(SessionModel.DtPortsInfo))
                        {
                            ShowText(txt_ShowMessage.Text + InfoMessage.DownLoadPorts + "\r\n");
                            SessionModel.DtPortsInfo.Clear();
                            cb_Ports.Checked = false;
                        }
                    }
                    else
                    {
                        UIHelper.ErrorMsg(InfoMessage.NotDownLoadData);
                    }
                }
                #endregion

                #region 业务数据下载
                if (cb_StockIn.Checked)
                {
                    UIHelper.PromptMsg(InfoMessage.DownLoadInfo);
                    #region 全部下载
                    //IsEnabled(false);
                    //if (string.IsNullOrEmpty(ControlerName))
                    //{
                    //    ControlerName=GetControllerName(BaseCommon.ProductsIn);                       
                    //}
                    //SessionModel.DtOrderInfo = BaseCommon.GetOrderInfo(ControlerName);
                    //if (SessionModel.DtOrderInfo != null && SessionModel.DtOrderInfo.Rows.Count > 0)
                    //{
                    //    if (BaseCommon.DownLoadStockInOrderInfo(SessionModel.DtOrderInfo))
                    //    {
                    //        SessionModel.DtOrderNoticeMaterialsInfo = BaseCommon.GetOrderNoticeInfo(null, ControlerName, BaseCommon.ProductsIn);
                    //        if (SessionModel.DtOrderNoticeMaterialsInfo != null && SessionModel.DtOrderNoticeMaterialsInfo.Rows.Count > 0)
                    //        {
                    //            if (BaseCommon.DownLoadStockInNoticeMaterialsInfo(SessionModel.DtOrderNoticeMaterialsInfo))
                    //            {
                    //                ShowText(txt_ShowMessage.Text + InfoMessage.DownLoadStockIn + "\r\n");
                    //                SessionModel.DtOrderInfo.Clear();
                    //                SessionModel.DtOrderNoticeMaterialsInfo.Clear();
                    //                cb_StockIn.Checked = false;
                    //                IsEnabled(true);
                    //            }
                    //        }
                    //    }
                    //}
                    #endregion
                }
                if (cb_StockOut.Checked)
                {
                    UIHelper.PromptMsg(InfoMessage.DownLoadInfo);
                    #region 全部下载
                    //if (string.IsNullOrEmpty(ControlerName))
                    //{
                    //    GetControllerName(BaseCommon.ProductsOut);
                    //}
                    //SessionModel.DtOrderInfo = BaseCommon.GetOrderInfo(ControlerName);
                    //if (SessionModel.DtOrderInfo != null && SessionModel.DtOrderInfo.Rows.Count > 0)
                    //{
                    //    if (BaseCommon.DownLoadStockOutOrderInfo(SessionModel.DtOrderInfo))
                    //    {
                    //        SessionModel.DtOrderNoticeMaterialsInfo = BaseCommon.GetOrderNoticeInfo(null, ControlerName, BaseCommon.ProductsOut);
                    //        if (SessionModel.DtOrderNoticeMaterialsInfo != null && SessionModel.DtOrderNoticeMaterialsInfo.Rows.Count > 0)
                    //        {
                    //            if (BaseCommon.DownLoadStockOutNoticeMaterialsInfo(SessionModel.DtOrderNoticeMaterialsInfo))
                    //            {
                    //                ShowText(txt_ShowMessage.Text + InfoMessage.DownLoadStockOut + "\r\n");
                    //                SessionModel.DtOrderInfo.Clear();
                    //                SessionModel.DtOrderNoticeMaterialsInfo.Clear();
                    //                cb_StockOut.Checked = false;
                    //            }
                    //        }
                    //    }
                    //}
                    #endregion
                }
                #endregion
            }
            else
            {
                UIHelper.ErrorMsg(InfoMessage.IsOffInfo);
            }
        }
        #endregion

        private void DataDownload_Load(object sender, EventArgs e)
        {
            sm = new SessionModel();
            if (PDASet.IsOff)
            {
                menuItemDownLoad.Enabled = false;
            }
            else
            {
                menuItemDownLoad.Enabled = true;
            }
        }

        private void DataDownload_Closed(object sender, EventArgs e)
        {
            mainMenu1.Dispose();

        }

        public string GetControllerName(string ConName)
        {
            DataTable dt = BaseCommon.GetMobileSetInfo(ConName);
            if (dt != null && dt.Rows.Count > 0)
            {
                ControlerName = dt.Rows[0]["ControllerName"].ToString();
                return ControlerName;
            }
            return "";
        }

        public void IsEnabled(bool Flag)
        {
            menuItemClear.Enabled = Flag;
            menuItemDownLoad.Enabled = Flag;
        }
    }
}