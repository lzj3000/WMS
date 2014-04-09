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
    public partial class Frm_Store : Form
    {
        #region 封装属性
        public string WhName;       
        public string WhCode;
        public string WHID;
        public string StrWHTYPE;
        #endregion

        public Frm_Store(string WHTYPE)
        {
            StrWHTYPE = WHTYPE;
            InitializeComponent();
        }

        private void Frm_Store_Load(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = BaseCommon.GetWhouseInfo(null, null);
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (StrWHTYPE == "0")
                    {
                        DataRow[] dr = dt.Select("WHTYPE='0'");
                        DataTable dtn = new DataTable();
                        dtn = dt.Clone();//克隆A的结构
                        foreach (DataRow row in dr)
                        {
                            dtn.ImportRow(row);//复制行数据
                        }
                        dt = dtn;
                    }

                    BindLv_StoreList(dt);
                }
                else
                {
                    UIHelper.PromptMsg(InfoMessage.NotBindData);
                }
            }
            catch (Exception ex)
            {
                UIHelper.ErrorMsg(ex.Message);
                return;
            }
        }

        private void BindLv_StoreList(DataTable dt)
        {
            lv_StoreList.Items.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ListViewItem list = new ListViewItem((i + 1).ToString());
                list.SubItems.Add(dt.Rows[i]["CODE"].ToString());
                list.SubItems.Add(dt.Rows[i]["WAREHOUSENAME"].ToString());
                list.SubItems.Add(dt.Rows[i]["ID"].ToString());
                lv_StoreList.Items.Add(list);
            }
        }

        private void menuItemSelect_Click(object sender, EventArgs e)
        {
            try
            {
                if (lv_StoreList.Items.Count > 0)
                {
                    ListView.SelectedIndexCollection c = lv_StoreList.SelectedIndices;
                    if (c.Count > 0)
                    {
                        WhCode = this.lv_StoreList.Items[lv_StoreList.SelectedIndices[0]].SubItems[1].Text.ToString();
                        WhName = this.lv_StoreList.Items[lv_StoreList.SelectedIndices[0]].SubItems[2].Text.ToString();
                        WHID = this.lv_StoreList.Items[lv_StoreList.SelectedIndices[0]].SubItems[3].Text.ToString();
                        this.DialogResult = DialogResult.OK;
                    }
                    else
                        UIHelper.PromptMsg(InfoMessage.SelectData);
                }
                else
                {
                    UIHelper.PromptMsg(InfoMessage.SelectData);
                }
            }
            catch (Exception ex)
            {
                UIHelper.ErrorMsg(ex.Message);
                return;
            }
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {

        }
    }
}