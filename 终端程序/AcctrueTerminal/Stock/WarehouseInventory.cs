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
    public partial class WarehouseInventory : Form
    {
        public WarehouseInventory()
        {
            sm = new SessionModel();
            InitializeComponent();
        }
        #region 初始化属性
        private SessionModel sm;
        private UrlTypeData ud;
        private string pdtype;
        private string whid;
        private string strCheckOrderID;
        #endregion
        private void WarehouseInventory_Closed(object sender, EventArgs e)
        {           
            this.mainMenu1.Dispose();
        }

        private void btn_CheckOrderQuery_Click(object sender, EventArgs e)
        {
            try
            {
                Frm_SerachOrder fso = null;
                fso = new Frm_SerachOrder(BaseCommon.WarehouseInventory);
                DialogResult ret = fso.ShowDialog();
                if (fso.OrderCode != null)
                {
                    txt_OrderCode.Text = fso.OrderCode;
                    txt_Store.Text = fso.OrderDesc;
                    pdtype = fso.SourceID.ToString();
                    whid = fso.WhourseId.ToString();
                    txt_WarehouseName.Text = fso.WhName;
                    DataTable dt = BaseCommon.GetCheckOrder(fso.OrderID.ToString());
                    if (dt.Rows[0]["MATERIALCODE_FNAME"].ToString() == "null")
                        txt_Type.Text = "盘点类型：整仓库盘点";
                    else
                        txt_Type.Text = "盘点产品：" + dt.Rows[0]["MATERIALCODE_FNAME"].ToString();
                    sm.OrderId = fso.OrderID;
                    BindLv_OrderList(fso.DtStaylist);
                }
            }
            catch (Exception ex)
            {
                UIHelper.ErrorMsg(ex.Message);
                return;
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
                    list.SubItems.Add(dt.Rows[i]["NEWMCODE"].ToString());
                    list.SubItems.Add(dt.Rows[i]["MATERIALCODE_FNAME"].ToString());
                    list.SubItems.Add(dt.Rows[i]["PNUM"].ToString());
                    //list.SubItems.Add(dt.Rows[i]["FUNITID_UNITNAME"].ToString());
                    list.SubItems.Add("暂无");
                    list.SubItems.Add(dt.Rows[i]["ID"].ToString());
                    lv_OrderList.Items.Add(list);
                }
            }
        }

        private void txt_Part_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                CheckPorts();
            }
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckData())
                {
                    if (UIHelper.QuestionMsg("确认保存？") == DialogResult.Yes)
                    {
                        ud = new UrlTypeData();
                        ud.Type = (int)CheckEnum.Edit;
                        ud.c = "Acc.Business.WMS.Controllers.CheckOrderController";
                        ud.m = BaseCommon.Edit;
                        ud.LoadItem = "{'GetOldObject':{'ID':'" + sm.OrderId.ToString() + "','WAREHOUSENAME':'" + whid + "','ORDERNAME':'" + txt_Store.Text + "','CheckOrderType':'" + pdtype + "','CODE':'" + txt_OrderCode.Text.Trim() + "'},'Acc.Business.WMS.Model.CheckOrderMaterials':[{'PORTCODE':'" + sm.PortsId + "','PDEPOTWBS':'" + sm.DepotwbsId + "','MATERIALCODE':'" + sm.NameId + "','NEWMCODE':'" + txt_Material.Text + "','PNUM':'" + txt_SsNum.Text.Trim() + "','PBATCHNO':'" + textBox1.Text + "','StateBase':0}]}";
                        sm.strResult = ToJson.ExecuteMethod(ud);
                        if (sm.strResult == "Y")
                        {
                            UIHelper.PromptMsg(InfoMessage.SaveSuccess);
                            Clear();
                            txt_Part.Focus();
                        }
                        else
                        {
                            UIHelper.ErrorMsg(InfoMessage.SaveFailed);
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

        private bool CheckData()
        {
            if (CheckOrder())
                return false;
            if (string.IsNullOrEmpty(txt_Part.Text.Trim()))
            {
                UIHelper.ErrorMsg("请扫描正确托盘码！");
                return false;
            }
            if (!CheckPorts())
                return false;
            if (!CheckMaterials())
                return false;
            if (!CheckDepots())
                return false;
            if (string.IsNullOrEmpty(txt_Desc.Text.Trim()) || string.IsNullOrEmpty(txt_Material.Text.Trim()))
            {
                UIHelper.ErrorMsg("请扫描正确产品码！");
                return false;
            }
            if (!UIHelper.CheckNum(txt_SsNum.Text.Trim()))
            {
                UIHelper.ErrorMsg(InfoMessage.Number);
                return false;
            }
            return true;
        }

        private bool CheckOrder()
        {
            if (string.IsNullOrEmpty(txt_OrderCode.Text) || string.IsNullOrEmpty(txt_WarehouseName.Text))
            {
                UIHelper.ErrorMsg("请正确选择盘点单号！");
                return true;
            }
            return false ;
        }

        private bool CheckPorts()
        {
            SessionModel.DtPortsInfo = BaseCommon.GetPortsInfo(txt_Part.Text.Trim());
            if (SessionModel.DtPortsInfo != null && SessionModel.DtPortsInfo.Rows.Count > 0)
            {
                sm.PortsId = Convert.ToInt32(SessionModel.DtPortsInfo.Rows[0]["ID"].ToString());
                txt_Material.Focus();
                return true;
            }
            else
            {
                UIHelper.PromptMsg(InfoMessage.TrayCodeNotStock);
                txt_Part.Focus();
                txt_Part.SelectionStart = txt_Part.TextLength;
                return false;
            }
        }

        private bool CheckMaterials()
        {
            SessionModel.DtMaterialsInfo = BaseCommon.GetMaterialsInfo(txt_Material.Text.Trim());
            if (SessionModel.DtMaterialsInfo != null && SessionModel.DtMaterialsInfo.Rows.Count > 0)
            {
                sm.NameId = Convert.ToInt32(SessionModel.DtMaterialsInfo.Rows[0]["ID"].ToString());
                txt_Desc.Text = SessionModel.DtMaterialsInfo.Rows[0]["FNAME"].ToString();
                textBox3.Focus();
                return true;
            }
            else
            {
                UIHelper.PromptMsg("产品编码无效,请正确输入!");
                txt_Material.Focus();
                txt_Material.SelectionStart = txt_Material.TextLength;
                return false;
            }
        }

        private bool CheckDepots()
        {
            if (string.IsNullOrEmpty(textBox3.Text.Trim()))
            {
                UIHelper.PromptMsg("请正确扫描货位编码!");
                return false;
            }
            SessionModel.DtWHourseInfo = BaseCommon.GetWhouseInfo(textBox3.Text.Trim(), whid);
            if (SessionModel.DtWHourseInfo != null && SessionModel.DtWHourseInfo.Rows.Count > 0)
            {
                sm.DepotwbsId = Convert.ToInt32(SessionModel.DtWHourseInfo.Rows[0]["ID"].ToString());
                //textBox1.Text = SessionModel.DtMaterialsInfo.Rows[0]["CODE"].ToString();
                txt_SsNum.Focus();
                return true;
            }
            else
            {
                UIHelper.PromptMsg("货位无效,请正确输入!");
                textBox3.Focus();
                textBox3.SelectionStart = textBox3.TextLength;
                return false;
            }
        }


        private void Clear()
        {
            txt_Part.Text = txt_Desc.Text = txt_Material.Text = textBox3.Text = txt_SsNum.Text = textBox1.Text= "";
        }

        private void tabControlSet_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControlSet.TabPages[tabControlSet.SelectedIndex].Text)
            {
                case "盘点记录":
                    if (SessionModel.DtPortsInfo.Rows.Count > 0)
                    {
                        SessionModel.DtTrayInfo = BaseCommon.GetCheckOrderInfo(sm.OrderId.ToString());
                        if (SessionModel.DtTrayInfo != null && SessionModel.DtTrayInfo.Rows.Count > 0)
                        {
                            BindLv_OrderList(SessionModel.DtTrayInfo);
                        }
                        else
                        {
                            //UIHelper.PromptMsg(InfoMessage.NotBindData);
                        }
                    }
                    break;
                case "盘点":
                    break;
                case "记录":
                    break;
                default:
                    break;

            }
        }

        private void lv_OrderList_ItemActivate(object sender, EventArgs e)
        {
            try
            {
                if (lv_OrderList.Items.Count == 0)
                {
                    UIHelper.ErrorMsg(InfoMessage.NotDeleteData);
                    return;
                }
                if (UIHelper.QuestionMsg("确认删除？") == DialogResult.Yes)
                {
                    int ID = Convert.ToInt32(lv_OrderList.Items[lv_OrderList.SelectedIndices[0]].SubItems[5].Text);
                    if (this.lv_OrderList.Items.Count > 0 && this.lv_OrderList.SelectedIndices.Count > 0)
                    {
                        ud = new UrlTypeData();
                        ud.c = "Acc.Business.WMS.Controllers.CheckOrderController";
                        ud.m = BaseCommon.Remove;
                        ud.LoadItem = "{'GetOldObject':{'ID':'" + sm.OrderId.ToString() + "','WAREHOUSENAME':'" + whid + "','ORDERNAME':'" + txt_Store.Text + "','CheckOrderType':'" + pdtype + "','CODE':'" + txt_OrderCode.Text.Trim() + "'},'Acc.Business.WMS.Model.CheckOrderMaterials':[{'ID':'" + ID + "','StateBase':2}]}";
                        sm.strResult = ToJson.ExecuteMethod(ud);
                        if (sm.strResult == "Y")
                        {
                            UIHelper.PromptMsg(InfoMessage.DeleteSuccess);
                            lv_OrderList.Items.RemoveAt(lv_OrderList.SelectedIndices[0]);
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
            }
            catch (Exception ex)
            {
                UIHelper.ErrorMsg(ex.Message);
                return;
            }
        }

        private void txt_OrderNum_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == 13)
                {
                    CheckMaterials();
                }
            }
            catch (Exception ex)
            {
                UIHelper.ErrorMsg(ex.Message);
                return;
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (CheckMaterials() && CheckDepots())
                {
                    SessionModel.DtStockInfo = BaseCommon.GetStockInfoMaterialsBywarehouse(null, sm.NameId.ToString(), sm.DepotwbsId.ToString(), whid, null);
                    if (SessionModel.DtStockInfo.Rows.Count > 0)
                    {
                        txt_Part.Text = SessionModel.DtStockInfo.Rows[0]["PORTCODE_PORTNO"].ToString();
                        textBox1.Text = SessionModel.DtStockInfo.Rows[0]["BATCHNO"].ToString();
                    }
                    else
                        UIHelper.ErrorMsg("该货位上不存在扫描产品的信息，请确认仓库是否选择正确！");
                }
            }
        }

        private void WarehouseInventory_Load(object sender, EventArgs e)
        {
            sm = new SessionModel();
            if (PDASet.IsOff)
            {

            }
            else
            {
                DataTable dt = BaseCommon.GetMobileSetInfo(BaseCommon.WarehouseInventory);
                if (dt != null && dt.Rows.Count > 0)
                {
                    sm.ControllerName = dt.Rows[0]["ControllerName"].ToString();
                    sm.ModelId = Convert.ToInt32(dt.Rows[0]["ID"].ToString());
                    AccessControl(Convert.ToInt32(this.tabControlSet.TabPages.Count));
                }
            }
        }

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
                    }
                    for (int i = 0; i < PDASet.List.Count; i++)
                    {

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
    }
}