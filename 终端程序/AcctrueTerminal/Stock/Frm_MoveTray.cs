using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AcctrueTerminal.Common;

namespace AcctrueTerminal.Stock
{
    public partial class Frm_MoveTray : Form
    {
        private UrlTypeData ud;
        private SessionModel sm;
        string WHid;
        private string materials;
        private string materialscode;
        private string ytp;
        private string mtp;
        private string yhw;
        private string mhw;
        private string infoId;
        private string strbatchno;
        private string strInfoSequence;

        public Frm_MoveTray()
        {
            InitializeComponent();
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckData())
                {
                    ud = new UrlTypeData();
                    ud.Type = (int)CheckEnum.Edit;
                    ud.c = "Acc.Business.WMS.Controllers.MoveOrderController";
                    ud.m = BaseCommon.Edit;
                    ud.LoadItem = "{'GetOldObject':{'ID':'" + sm.MoveOrderId + "','DEPOTWBS':'" + WHid + "'}";
                    if (sm.MoveOrderId == 0)
                    {
                        ud.m = BaseCommon.Add;
                        ud.LoadItem = "{'DEPOTWBS':'" + WHid + "','StateBase':0";
                    }
                    DataTable dt = BaseCommon.GetMaterialsInfo(materialscode);
                    ud.LoadItem += ",'Acc.Business.WMS.Model.MoveOrderMaterials':[{'FROMPORT':'" + ytp + "','FROMDEPOT':'" + yhw + "','TOPORT':'" + mtp + "','TODEPOT':'" + mhw + "','MCODE':'" + dt.Rows[0]["CODE"].ToString() + "','FMODEL':'" + dt.Rows[0]["FMODEL"].ToString() + "','MATERIALCODE':'" + materials + "','NUM':'" + textBox2.Text.Trim() + "','BATCHNO':'" + strbatchno + "','StateBase':0}]}&InfoSequences=" + strInfoSequence;
                    if (sm.MoveOrderId == 0)
                    {
                        sm.MoveOrderId = Convert.ToInt32(ToJson.ExecuteMethodGetFirstID(ud));
                        sm.strResult = "Y";
                    }
                    else
                        sm.strResult = ToJson.ExecuteMethod(ud);
                    if (sm.strResult == "Y")
                    {
                        UIHelper.PromptMsg(InfoMessage.MoveSuccess);
                        txt_Gname.Text = textBox1.Text = textBox2.Text = textBox5.Text = textBox3.Text = "";
                        txt_Gname.Focus();
                        lv_OrderList.Items.Clear();
                        lv_shInfo.Items.Clear();
                        strInfoSequence = null;
                    }
                    else
                    {
                        UIHelper.ErrorMsg(InfoMessage.MoveFailed);
                    }
                }
            }
            catch (Exception ex)
            {
                UIHelper.ErrorMsg(ex.Message);
                return;
            }
        }

        private void txt_Gname_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == 13)
                {
                    if (!string.IsNullOrEmpty(txt_Gname.Text.Trim()))
                    {
                        SessionModel.DtWHourseInfo = BaseCommon.GetPortsInfo(txt_Gname.Text.Trim());
                        if (SessionModel.DtWHourseInfo != null && SessionModel.DtWHourseInfo.Rows.Count > 0)
                        {
                            ytp = SessionModel.DtWHourseInfo.Rows[0]["ID"].ToString();
                            SessionModel.DtStockInfo = BaseCommon.GetStockInfoMaterials(ytp, null, null);
                            if (SessionModel.DtStockInfo != null && SessionModel.DtStockInfo.Rows.Count > 0)
                            {
                                if (SessionModel.DtStockInfo.Rows[0]["WAREHOUSEID"].ToString() != WHid)
                                {
                                    UIHelper.ErrorMsg("该托盘不属于当前选择仓库！");
                                    return;
                                }
                                //textBox1.Text = SessionModel.DtStockInfo.Rows[0]["DEPOTWBS_Code"].ToString();
                                SessionModel.DtWHourseInfo = BaseCommon.GetWhouseInfo(textBox1.Text, WHid);
                                if (SessionModel.DtWHourseInfo.Rows.Count > 0)
                                    yhw = SessionModel.DtWHourseInfo.Rows[0]["ID"].ToString();
                                Bindlv_OrderList(SessionModel.DtStockInfo);
                                textBox2.Focus();
                            }
                            else
                            {
                                UIHelper.ErrorMsg(InfoMessage.NotTrayCode);
                                listView1.Items.Clear();
                                txt_Gname.Focus();
                            }
                        }
                        else
                        {
                            UIHelper.ErrorMsg(InfoMessage.NotTrayCode);
                        }
                    }
                    else
                    {
                        UIHelper.ErrorMsg(InfoMessage.InputOrScanningBin);
                    }
                }
            }
            catch (Exception ex)
            {
                UIHelper.ErrorMsg(ex.Message);
                return;
            }
        }

        #region 数据绑定
        private void Bindlv_OrderList(DataTable dt)
        {
            lv_OrderList.Items.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ListViewItem list = new ListViewItem((i + 1).ToString());
                list.SubItems.Add(dt.Rows[i]["MCODE"].ToString());
                list.SubItems.Add(dt.Rows[i]["CODE_FNAME"].ToString());
                list.SubItems.Add(dt.Rows[i]["NUM"].ToString());
                list.SubItems.Add("kg");
                list.SubItems.Add(dt.Rows[i]["CODE"].ToString());
                list.SubItems.Add(dt.Rows[i]["ID"].ToString());
                list.SubItems.Add(dt.Rows[i]["DEPOTWBS_Code"].ToString());
                list.SubItems.Add(dt.Rows[i]["BATCHNO"].ToString());
                lv_OrderList.Items.Add(list);
            }
        }

        private void lv_OrderList_ItemActivate(object sender, EventArgs e)
        {
            try
            {
                textBox2.Text = lv_OrderList.Items[lv_OrderList.SelectedIndices[0]].SubItems[3].Text;
                infoId = lv_OrderList.Items[lv_OrderList.SelectedIndices[0]].SubItems[6].Text;
                textBox1.Text = lv_OrderList.Items[lv_OrderList.SelectedIndices[0]].SubItems[7].Text;
                if (BaseCommon.GetWhouseInfo(textBox1.Text, WHid).Rows.Count > 0)
                    yhw = BaseCommon.GetWhouseInfo(textBox1.Text, WHid).Rows[0]["ID"].ToString();
                else
                    yhw = null;
                materialscode = lv_OrderList.Items[lv_OrderList.SelectedIndices[0]].SubItems[1].Text;
                CheckMateris(materialscode);
                strbatchno = lv_OrderList.Items[lv_OrderList.SelectedIndices[0]].SubItems[8].Text;
            }
            catch (Exception ex)
            {
                UIHelper.ErrorMsg(ex.Message);
                return;
            }
        }
        #endregion

        #region 数据验证
        /// <summary>
        /// 数据验证
        /// </summary>
        /// <returns></returns>
        private bool CheckData()
        {
            if (!UIHelper.CheckTextBox(txt_OrderCode.Text.Trim()))
            {
                UIHelper.ErrorMsg("请选择仓库！");
                return false;
            }
            if (lv_OrderList.SelectedIndices.Count <= 0)
            {
                UIHelper.ErrorMsg(InfoMessage.SelectData);
                return false;
            }
            if (string.IsNullOrEmpty(txt_Gname.Text.Trim()))
            {
                txt_Gname.Text = InfoMessage.InputOrScanningBin;
                txt_Gname.SelectAll();
                txt_Gname.Focus();
                return false;
            }
            if (lv_OrderList.Items.Count <= 0)
            {
                UIHelper.PromptMsg("此托盘不存在仓库中！");
                return false;
            }
            if (!UIHelper.CheckNum(textBox2.Text.Trim()))
            {
                UIHelper.ErrorMsg(InfoMessage.Number);
                return false;
            }
            if (Convert.ToSingle(textBox2.Text.Trim()) > Convert.ToSingle(lv_OrderList.Items[lv_OrderList.SelectedIndices[0]].SubItems[3].Text))
            {
                UIHelper.ErrorMsg(InfoMessage.MoveNumberMess);
                return false;
            }
            if (string.IsNullOrEmpty(textBox5.Text) || string.IsNullOrEmpty(mtp))
            {
                UIHelper.ErrorMsg("请扫描目标托盘并回车确认！");
                return false;
            }
            if (string.IsNullOrEmpty(textBox3.Text) || string.IsNullOrEmpty(mhw))
            {
                UIHelper.ErrorMsg("请扫描目标货位并回车确认！");
                return false;
            }
            if (txt_Gname.Text.Trim() == textBox5.Text.Trim())
            {
                UIHelper.ErrorMsg("起始托盘不能等于目标托盘！");
                return false;
            }
            if (string.IsNullOrEmpty(textBox5.Text) && !string.IsNullOrEmpty(textBox3.Text))
            {
                UIHelper.ErrorMsg("输入目标货位后目标托盘不能为空！");
                return false;
            }
            //if (textBox1.Text.Trim() == textBox3.Text.Trim())
            //{
            //    UIHelper.ErrorMsg("源货位不能等于目标货位！");
            //    return false;
            //}
            if (BaseCommon.CheckMaterialSequenState(materialscode))
            {
                ud = new UrlTypeData();
                ud.Type = 2;
                ud.c = "Acc.Business.WMS.XHY.Controllers.ProduceInOrderController";
                ud.m = "GetInfoSequenceList";
                ud.LoadItem = "{}&MATERIALCODE=" + materials + "&batchno=" + strbatchno + "&WAREHOUSEID=" + WHid + "&DEPOTWBS=" + yhw + "&PORTCODE=" + ytp;
                DataTable dt=ToJson.getData(ud);
                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToDouble(textBox2.Text.Trim()) > Convert.ToDouble(lv_shInfo.Items.Count))
                    {
                        UIHelper.ErrorMsg("选择移位的产品为序列码管理，并且序列码数小于移位的数量！");
                        tabControlSet.SelectedIndex = 2;
                        return false;
                    }
                    if (Convert.ToDouble(textBox2.Text.Trim()) < Convert.ToDouble(lv_shInfo.Items.Count))
                    {
                        UIHelper.ErrorMsg("选择移位的产品为序列码管理，并且序列码数大于移位的数量！");
                        tabControlSet.SelectedIndex = 2;
                        return false;
                    }
                    List<InfoInSequence> infoSelists = new List<InfoInSequence>();
                    for (int i = 0; i < lv_shInfo.Items.Count; i++)
                    {
                        InfoInSequence infoSelist = new InfoInSequence();
                        infoSelist.STOCKINFOMATERIALSID =Convert.ToInt32(infoId);
                        infoSelist.SEQUENCECODE = lv_shInfo.Items[i].SubItems[1].Text;
                        infoSelists.Add(infoSelist);
                    }
                    strInfoSequence = UIHelper.InFoSequenceListConversion(infoSelists);
                }
            }
            return true;
        }

        private bool CheckMateris(string strMCode)
        {
            DataTable dt = BaseCommon.GetMaterialsInfo(strMCode);
            if (dt != null && dt.Rows.Count > 0)
            {
                materials = dt.Rows[0]["ID"].ToString();
                return true;
            }
            else
                return false;
        }

        private bool btn_CheckPort(string strPortCode)
        {
            DataTable dt = BaseCommon.GetPortsInfo(strPortCode);
            if (dt != null && dt.Rows.Count > 0)
                return true;
            else
            {
                UIHelper.ErrorMsg("请扫描正确托盘编码！");
                return false;
            }
        }
        private bool btn_Warehouse(string strWarehouseCode)
        {
            DataTable dt = BaseCommon.GetWhouseInfo(strWarehouseCode,WHid);
            if (dt != null && dt.Rows.Count > 0)
                return true;
            else
            {
                UIHelper.ErrorMsg("请扫描正货位编码！");
                return false;
            }
        }
        #endregion

        private void btn_CheckOrderQuery_Click(object sender, EventArgs e)
        {
            try
            {
                Frm_Store fs = new Frm_Store("0");
                DialogResult ret = fs.ShowDialog();
                if (fs.WhName != null)
                {
                    txt_OrderCode.Text = fs.WhCode;
                    tb_OrderCheckDesc.Text = fs.WhName;
                    WHid = fs.WHID;
                    sm.WhourseId = Convert.ToInt32(fs.WHID);
                }
            }
            catch (Exception ex)
            {
                UIHelper.ErrorMsg(ex.Message);
                return;
            }
        }

        private void Frm_MoveTray_Load(object sender, EventArgs e)
        {
            sm = new SessionModel();
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == 13)
                {
                    if (!UIHelper.CheckTextBox(txt_OrderCode.Text.Trim()))
                    {
                        UIHelper.ErrorMsg("请选择仓库！");
                        return;
                    }
                    if (!GetWhourseId(textBox3.Text.Trim()))
                    {
                        UIHelper.ErrorMsg("该货位不存在或者不存在该仓库中！");
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

        #region 获取存储位置信息
        private bool GetWhourseId(string BinCode)
        {
            DataTable dt = BaseCommon.GetWhouseInfo(BinCode, WHid);
            if (dt != null && dt.Rows.Count > 0)
            {
                mhw = dt.Rows[0]["ID"].ToString();
                return true;
            }
            else
            {
                UIHelper.ErrorMsg(InfoMessage.BinCodeNotFound);
            }
            return false;
        }
        #endregion

        private void tabControlSet_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControlSet.TabPages[tabControlSet.SelectedIndex].Text)
            {
                case "记录":
                    BindlistView1List(BaseCommon.GetMoveOrderInfo(sm.MoveOrderId.ToString()));
                    break;
                default:
                    break;

            }
        }

        public void BindlistView1List(DataTable dt)
        {
            listView1.Items.Clear();
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ListViewItem list = new ListViewItem((i + 1).ToString());
                    list.SubItems.Add(dt.Rows[i]["MATERIALCODE_FNAME"].ToString());
                    list.SubItems.Add(dt.Rows[i]["NUM"].ToString());
                    list.SubItems.Add(dt.Rows[i]["FROMDEPOT_CODE"].ToString());
                    list.SubItems.Add(dt.Rows[i]["TODEPOT_WAREHOUSENAME"].ToString());
                    list.SubItems.Add(dt.Rows[i]["FROMPORT_PORTNO"].ToString());
                    list.SubItems.Add(dt.Rows[i]["TOPORT_PORTNO"].ToString());
                    listView1.Items.Add(list);
                }
            }
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == 13)
                {
                    if (!string.IsNullOrEmpty(textBox5.Text.Trim()))
                    {
                        SessionModel.DtPortsInfo = BaseCommon.GetPortsInfo(textBox5.Text.Trim());
                        if (SessionModel.DtPortsInfo.Rows.Count > 0)
                        {
                            mtp = SessionModel.DtPortsInfo.Rows[0]["ID"].ToString();
                        }
                        else
                        {
                            UIHelper.ErrorMsg(InfoMessage.NotTrayCode);
                            return;
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

        private void txt_AssetsCode_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void lv_shInfo_ItemActivate(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 序列码验证
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_AssetsCode_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == 13 && txt_AssetsCode.Text.Trim() != "")
                {
                    if (!UIHelper.CheckTextBox(txt_OrderCode.Text.Trim()))
                    {
                        UIHelper.ErrorMsg("请选择仓库！");
                        return;
                    }
                    if (string.IsNullOrEmpty(txt_Gname.Text))
                    {
                        UIHelper.ErrorMsg("请扫描托盘！");
                        return;
                    }
                    if (!UIHelper.CheckNum(textBox2.Text.Trim()))
                    {
                        UIHelper.ErrorMsg(InfoMessage.Number);
                        return;
                    }
                    if (lv_OrderList.SelectedIndices.Count <= 0)
                    {
                        UIHelper.ErrorMsg(InfoMessage.SelectData);
                        return;
                    }
                    if (!BaseCommon.CheckMaterialSequenState(materialscode))
                    {
                        UIHelper.ErrorMsg("选择产品不是单件码管理，不需要输入单件码！");
                        txt_AssetsCode.Text = "";
                        return;
                    }
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
                    ud.LoadItem = "{}&CPorderid=" + infoId + "&SEcode=" + txt_AssetsCode.Text.Trim();
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
                    int l = lv_shInfo.Items.Count;
                    l = l + 1;
                    ListViewItem list = new ListViewItem();
                    list.Text = l.ToString();
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

        private void lv_shInfo_ItemActivate_1(object sender, EventArgs e)
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

        private void Frm_MoveTray_Closing(object sender, CancelEventArgs e)
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

        private void menuItem2_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                UIHelper.ErrorMsg(ex.Message);
                return;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

    }
}