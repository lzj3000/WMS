﻿namespace AcctrueTerminal.Stock
{
    partial class Frm_MoveBin
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenu1;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menuItemSave = new System.Windows.Forms.MenuItem();
            this.menuItemFinish = new System.Windows.Forms.MenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txt_EndBin = new System.Windows.Forms.TextBox();
            this.txt_StartBin = new System.Windows.Forms.TextBox();
            this.checkBoxSelect = new System.Windows.Forms.CheckBox();
            this.txt_Num = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_SelectWh = new System.Windows.Forms.Button();
            this.txt_WhCode = new System.Windows.Forms.TextBox();
            this.txt_WhName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.label14 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.lv_MoveData = new System.Windows.Forms.ListView();
            this.columnHeader17 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader18 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader19 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader20 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader21 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader22 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lv_StockInfoList = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
            this.tabControlSet = new System.Windows.Forms.TabControl();
            this.panel1.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabControlSet.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menuItemSave);
            this.mainMenu1.MenuItems.Add(this.menuItemFinish);
            // 
            // menuItemSave
            // 
            this.menuItemSave.Text = "转移";
            this.menuItemSave.Click += new System.EventHandler(this.menuItemSave_Click);
            // 
            // menuItemFinish
            // 
            this.menuItemFinish.Text = "完成";
            this.menuItemFinish.Click += new System.EventHandler(this.menuItemFinish_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.panel1.Controls.Add(this.txt_EndBin);
            this.panel1.Controls.Add(this.txt_StartBin);
            this.panel1.Controls.Add(this.checkBoxSelect);
            this.panel1.Controls.Add(this.txt_Num);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.btn_SelectWh);
            this.panel1.Controls.Add(this.txt_WhCode);
            this.panel1.Controls.Add(this.txt_WhName);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Location = new System.Drawing.Point(2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(235, 87);
            // 
            // txt_EndBin
            // 
            this.txt_EndBin.BackColor = System.Drawing.Color.White;
            this.txt_EndBin.Location = new System.Drawing.Point(150, 30);
            this.txt_EndBin.Name = "txt_EndBin";
            this.txt_EndBin.Size = new System.Drawing.Size(82, 21);
            this.txt_EndBin.TabIndex = 35;
            this.txt_EndBin.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_EndBin_KeyPress);
            // 
            // txt_StartBin
            // 
            this.txt_StartBin.BackColor = System.Drawing.Color.White;
            this.txt_StartBin.Location = new System.Drawing.Point(44, 30);
            this.txt_StartBin.Name = "txt_StartBin";
            this.txt_StartBin.Size = new System.Drawing.Size(82, 21);
            this.txt_StartBin.TabIndex = 28;
            this.txt_StartBin.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_StartBin_KeyPress);
            // 
            // checkBoxSelect
            // 
            this.checkBoxSelect.Location = new System.Drawing.Point(133, 54);
            this.checkBoxSelect.Name = "checkBoxSelect";
            this.checkBoxSelect.Size = new System.Drawing.Size(70, 20);
            this.checkBoxSelect.TabIndex = 21;
            this.checkBoxSelect.Text = "全选";
            this.checkBoxSelect.CheckStateChanged += new System.EventHandler(this.checkBoxSelect_CheckStateChanged);
            // 
            // txt_Num
            // 
            this.txt_Num.BackColor = System.Drawing.Color.White;
            this.txt_Num.Location = new System.Drawing.Point(44, 54);
            this.txt_Num.Name = "txt_Num";
            this.txt_Num.Size = new System.Drawing.Size(82, 21);
            this.txt_Num.TabIndex = 19;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(2, 58);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 14);
            this.label4.Text = "数量";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(130, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(19, 16);
            this.label1.Text = "至";
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular);
            this.label9.ForeColor = System.Drawing.Color.IndianRed;
            this.label9.Location = new System.Drawing.Point(-1, 77);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(235, 6);
            this.label9.Text = "－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－";
            this.label9.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(2, 34);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(37, 14);
            this.label8.Text = "货位";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(2, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 14);
            this.label2.Text = "仓库";
            // 
            // btn_SelectWh
            // 
            this.btn_SelectWh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btn_SelectWh.Font = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Bold);
            this.btn_SelectWh.Location = new System.Drawing.Point(125, 6);
            this.btn_SelectWh.Name = "btn_SelectWh";
            this.btn_SelectWh.Size = new System.Drawing.Size(32, 21);
            this.btn_SelectWh.TabIndex = 6;
            this.btn_SelectWh.Text = "...";
            this.btn_SelectWh.Click += new System.EventHandler(this.btn_SelectWh_Click);
            // 
            // txt_WhCode
            // 
            this.txt_WhCode.Enabled = false;
            this.txt_WhCode.Location = new System.Drawing.Point(44, 6);
            this.txt_WhCode.Name = "txt_WhCode";
            this.txt_WhCode.Size = new System.Drawing.Size(83, 21);
            this.txt_WhCode.TabIndex = 7;
            // 
            // txt_WhName
            // 
            this.txt_WhName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txt_WhName.Enabled = false;
            this.txt_WhName.Location = new System.Drawing.Point(154, 6);
            this.txt_WhName.Name = "txt_WhName";
            this.txt_WhName.ReadOnly = true;
            this.txt_WhName.Size = new System.Drawing.Size(78, 21);
            this.txt_WhName.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular);
            this.label3.ForeColor = System.Drawing.Color.IndianRed;
            this.label3.Location = new System.Drawing.Point(-2, 101);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(246, 6);
            this.label3.Text = "－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // tabPage6
            // 
            this.tabPage6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tabPage6.Controls.Add(this.label14);
            this.tabPage6.Controls.Add(this.textBox4);
            this.tabPage6.Controls.Add(this.lv_MoveData);
            this.tabPage6.Location = new System.Drawing.Point(0, 0);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Size = new System.Drawing.Size(232, 150);
            this.tabPage6.Text = "记录";
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(6, 4);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(57, 20);
            this.label14.Text = "编   码";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(67, 1);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(164, 21);
            this.textBox4.TabIndex = 9;
            // 
            // lv_MoveData
            // 
            this.lv_MoveData.Activation = System.Windows.Forms.ItemActivation.TwoClick;
            this.lv_MoveData.Columns.Add(this.columnHeader17);
            this.lv_MoveData.Columns.Add(this.columnHeader18);
            this.lv_MoveData.Columns.Add(this.columnHeader19);
            this.lv_MoveData.Columns.Add(this.columnHeader20);
            this.lv_MoveData.Columns.Add(this.columnHeader21);
            this.lv_MoveData.Columns.Add(this.columnHeader22);
            this.lv_MoveData.Columns.Add(this.columnHeader6);
            this.lv_MoveData.FullRowSelect = true;
            this.lv_MoveData.Location = new System.Drawing.Point(1, 27);
            this.lv_MoveData.Name = "lv_MoveData";
            this.lv_MoveData.Size = new System.Drawing.Size(237, 126);
            this.lv_MoveData.TabIndex = 2;
            this.lv_MoveData.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader17
            // 
            this.columnHeader17.Text = "行";
            this.columnHeader17.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader17.Width = 40;
            // 
            // columnHeader18
            // 
            this.columnHeader18.Text = "编码";
            this.columnHeader18.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader18.Width = 60;
            // 
            // columnHeader19
            // 
            this.columnHeader19.Text = "描述";
            this.columnHeader19.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader19.Width = 60;
            // 
            // columnHeader20
            // 
            this.columnHeader20.Text = "数量";
            this.columnHeader20.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader20.Width = 36;
            // 
            // columnHeader21
            // 
            this.columnHeader21.Text = "单位";
            this.columnHeader21.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader21.Width = 39;
            // 
            // columnHeader22
            // 
            this.columnHeader22.Text = "起始货位";
            this.columnHeader22.Width = 60;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "终止货位";
            this.columnHeader6.Width = 60;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tabPage1.Controls.Add(this.lv_StockInfoList);
            this.tabPage1.Location = new System.Drawing.Point(0, 0);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(240, 153);
            this.tabPage1.Text = "操作";
            // 
            // lv_StockInfoList
            // 
            this.lv_StockInfoList.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.lv_StockInfoList.Columns.Add(this.columnHeader1);
            this.lv_StockInfoList.Columns.Add(this.columnHeader2);
            this.lv_StockInfoList.Columns.Add(this.columnHeader3);
            this.lv_StockInfoList.Columns.Add(this.columnHeader5);
            this.lv_StockInfoList.Columns.Add(this.columnHeader4);
            this.lv_StockInfoList.Columns.Add(this.columnHeader7);
            this.lv_StockInfoList.FullRowSelect = true;
            this.lv_StockInfoList.Location = new System.Drawing.Point(2, 0);
            this.lv_StockInfoList.Name = "lv_StockInfoList";
            this.lv_StockInfoList.Size = new System.Drawing.Size(238, 153);
            this.lv_StockInfoList.TabIndex = 1;
            this.lv_StockInfoList.View = System.Windows.Forms.View.Details;
            this.lv_StockInfoList.ItemActivate += new System.EventHandler(this.lv_StockInfoList_ItemActivate);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "序号";
            this.columnHeader1.Width = 40;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "编码";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader2.Width = 60;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "描述";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader3.Width = 60;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "数量";
            this.columnHeader5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader5.Width = 36;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "单位";
            this.columnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader4.Width = 39;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "产品ID";
            this.columnHeader7.Width = 0;
            // 
            // tabControlSet
            // 
            this.tabControlSet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tabControlSet.Controls.Add(this.tabPage1);
            this.tabControlSet.Controls.Add(this.tabPage6);
            this.tabControlSet.Dock = System.Windows.Forms.DockStyle.None;
            this.tabControlSet.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular);
            this.tabControlSet.Location = new System.Drawing.Point(-1, 88);
            this.tabControlSet.Name = "tabControlSet";
            this.tabControlSet.SelectedIndex = 0;
            this.tabControlSet.Size = new System.Drawing.Size(240, 181);
            this.tabControlSet.TabIndex = 8;
            // 
            // Frm_MoveBin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(239, 269);
            this.Controls.Add(this.tabControlSet);
            this.Controls.Add(this.panel1);
            this.Menu = this.mainMenu1;
            this.Name = "Frm_MoveBin";
            this.Text = "货位转移";
            this.Load += new System.EventHandler(this.Frm_MoveBin_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.Frm_MoveBin_Closing);
            this.panel1.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabControlSet.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem menuItemSave;
        private System.Windows.Forms.MenuItem menuItemFinish;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_SelectWh;
        private System.Windows.Forms.TextBox txt_WhCode;
        private System.Windows.Forms.TextBox txt_WhName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_Num;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox checkBoxSelect;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.ListView lv_MoveData;
        private System.Windows.Forms.ColumnHeader columnHeader17;
        private System.Windows.Forms.ColumnHeader columnHeader18;
        private System.Windows.Forms.ColumnHeader columnHeader19;
        private System.Windows.Forms.ColumnHeader columnHeader20;
        private System.Windows.Forms.ColumnHeader columnHeader21;
        private System.Windows.Forms.ColumnHeader columnHeader22;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ListView lv_StockInfoList;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.TabControl tabControlSet;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.TextBox txt_StartBin;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.TextBox txt_EndBin;
        private System.Windows.Forms.ColumnHeader columnHeader7;
    }
}