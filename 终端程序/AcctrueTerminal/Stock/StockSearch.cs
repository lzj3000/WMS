﻿using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AcctrueTerminal.Common;

namespace AcctrueTerminal.Stock
{
    public partial class StockSearch : Form
    {
        private DataTable dtStockInfo = null;
        private SessionModel sm;
        public StockSearch()
        {
            InitializeComponent();
        }

        private void Btn_Query(object sender, EventArgs e)
        {
            try
            {
                string strWhourseId = null;
                string strCode = null;
                string strDepotId = null;
                if (string.IsNullOrEmpty(txt_WhCode.Text.Trim()) && string.IsNullOrEmpty(txt_Bin.Text.Trim()) && string.IsNullOrEmpty(txt_Code.Text.Trim()))
                {
                    UIHelper.PromptMsg(InfoMessage.SelectQueryConditions);
                    return;
                }
                if (!string.IsNullOrEmpty(txt_Bin.Text.Trim()))
                {
                    SessionModel.DtWHourseInfo = BaseCommon.GetWhouseInfo(txt_Bin.Text.Trim(), null);
                    if (SessionModel.DtWHourseInfo.Rows.Count > 0)
                        strDepotId = SessionModel.DtWHourseInfo.Rows[0]["ID"].ToString();
                    else
                    {
                        UIHelper.ErrorMsg("请扫描正确货位！");
                        return;
                    }
                }
                if (!string.IsNullOrEmpty(txt_Code.Text.Trim()))
                {
                    SessionModel.DtMaterialsInfo = BaseCommon.GetMaterialsInfo(txt_Code.Text.Trim());
                    if (SessionModel.DtMaterialsInfo.Rows.Count > 0)
                        strCode = SessionModel.DtMaterialsInfo.Rows[0]["ID"].ToString();
                    else
                    {
                        UIHelper.ErrorMsg("请扫描正确产品编码！");
                        return;
                    }
                }
                if (!string.IsNullOrEmpty(txt_WhCode.Text.Trim()))
                {
                    SessionModel.DtWHourseInfo = BaseCommon.GetWhouseInfo(txt_WhCode.Text.Trim(), null);
                    if (SessionModel.DtWHourseInfo.Rows.Count > 0)
                        strWhourseId = SessionModel.DtWHourseInfo.Rows[0]["ID"].ToString();
                }
                //dtStockInfo = BaseCommon.GetStockInfoMaterials(null, strCode, strWhourseId);
                dtStockInfo = BaseCommon.GetStockInfoMaterialsBywarehouseAndOr(null, strCode, strDepotId, strWhourseId, null);
                if (dtStockInfo != null && dtStockInfo.Rows.Count > 0)
                {
                    BindLv_QueryData(dtStockInfo);
                }
                else
                {
                    UIHelper.PromptMsg(InfoMessage.NotBindData);
                    lv_QueryData.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                UIHelper.ErrorMsg(ex.Message);
                return;
            }
        }

        private void BindLv_QueryData(DataTable dt)
        {
            lv_QueryData.Items.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {                
                ListViewItem list = new ListViewItem((i + 1).ToString());
                list.SubItems.Add(dt.Rows[i]["WAREHOUSEID_WAREHOUSENAME"].ToString());
                list.SubItems.Add(dt.Rows[i]["DEPOTWBS_CODE"].ToString());
                list.SubItems.Add(dt.Rows[i]["PORTCODE_PORTNO"].ToString());
                list.SubItems.Add(dt.Rows[i]["MCODE"].ToString());
                list.SubItems.Add(dt.Rows[i]["CODE_FNAME"].ToString());
                list.SubItems.Add(dt.Rows[i]["NUM"].ToString());
                list.SubItems.Add(dt.Rows[i]["BATCHNO"].ToString());
                list.SubItems.Add(dt.Rows[i]["FMODEL"].ToString());
                list.SubItems.Add("暂无");      
                lv_QueryData.Items.Add(list);
            }
        }

        private void btn_SelectWh_Click(object sender, EventArgs e)
        {
            try
            {
                Frm_Store fs = new Frm_Store("0");
                DialogResult ret = fs.ShowDialog();
                if (fs.WhName != null)
                {
                    txt_WhCode.Text = fs.WhCode;
                    txt_WhName.Text = fs.WhName;
                }
            }
            catch (Exception ex)
            {
                UIHelper.ErrorMsg(ex.Message);
                return;
            }
        }

        private void StockSearch_Load(object sender, EventArgs e)
        {
            sm = new SessionModel();
        }

        private void StockSearch_Closing(object sender, CancelEventArgs e)
        {
            if (UIHelper.QuestionMsg("是否退出") == DialogResult.Yes)
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

        private void menuItem2_Click(object sender, EventArgs e)
        {
            this.Close();
        } 
    }
}