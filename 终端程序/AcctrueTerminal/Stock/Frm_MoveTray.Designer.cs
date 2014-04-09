namespace AcctrueTerminal.Stock
{
    partial class Frm_MoveTray
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
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.lv_OrderList = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader12 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader15 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader8 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader9 = new System.Windows.Forms.ColumnHeader();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txt_Gname = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tabControlSet = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader17 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader19 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader20 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader14 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader16 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.txt_AssetsCode = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.lv_shInfo = new System.Windows.Forms.ListView();
            this.columnHeader10 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader11 = new System.Windows.Forms.ColumnHeader();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_CheckOrderQuery = new System.Windows.Forms.Button();
            this.txt_OrderCode = new System.Windows.Forms.TextBox();
            this.tb_OrderCheckDesc = new System.Windows.Forms.TextBox();
            this.tabControlSet.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menuItem1);
            this.mainMenu1.MenuItems.Add(this.menuItem2);
            // 
            // menuItem1
            // 
            this.menuItem1.Text = "转    移";
            this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Text = "完    成";
            this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.Color.White;
            this.textBox2.Location = new System.Drawing.Point(37, 55);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(83, 21);
            this.textBox2.TabIndex = 33;
            // 
            // lv_OrderList
            // 
            this.lv_OrderList.Activation = System.Windows.Forms.ItemActivation.TwoClick;
            this.lv_OrderList.Columns.Add(this.columnHeader1);
            this.lv_OrderList.Columns.Add(this.columnHeader2);
            this.lv_OrderList.Columns.Add(this.columnHeader3);
            this.lv_OrderList.Columns.Add(this.columnHeader5);
            this.lv_OrderList.Columns.Add(this.columnHeader4);
            this.lv_OrderList.Columns.Add(this.columnHeader12);
            this.lv_OrderList.Columns.Add(this.columnHeader15);
            this.lv_OrderList.Columns.Add(this.columnHeader8);
            this.lv_OrderList.Columns.Add(this.columnHeader9);
            this.lv_OrderList.FullRowSelect = true;
            this.lv_OrderList.Location = new System.Drawing.Point(1, 7);
            this.lv_OrderList.Name = "lv_OrderList";
            this.lv_OrderList.Size = new System.Drawing.Size(238, 98);
            this.lv_OrderList.TabIndex = 1;
            this.lv_OrderList.View = System.Windows.Forms.View.Details;
            this.lv_OrderList.ItemActivate += new System.EventHandler(this.lv_OrderList_ItemActivate);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "行";
            this.columnHeader1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
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
            // columnHeader12
            // 
            this.columnHeader12.Text = "批次";
            this.columnHeader12.Width = 0;
            // 
            // columnHeader15
            // 
            this.columnHeader15.Text = "仓库";
            this.columnHeader15.Width = 0;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "货位";
            this.columnHeader8.Width = 0;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "批次";
            this.columnHeader9.Width = 40;
            // 
            // checkBox1
            // 
            this.checkBox1.Location = new System.Drawing.Point(148, 54);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(70, 20);
            this.checkBox1.TabIndex = 34;
            this.checkBox1.Text = "全选";
            this.checkBox1.Visible = false;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(3, 60);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 14);
            this.label4.Text = "数量";
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.White;
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(148, 31);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(87, 21);
            this.textBox1.TabIndex = 32;
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular);
            this.label9.ForeColor = System.Drawing.Color.IndianRed;
            this.label9.Location = new System.Drawing.Point(1, 74);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(235, 10);
            this.label9.Text = "－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－";
            this.label9.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txt_Gname
            // 
            this.txt_Gname.BackColor = System.Drawing.Color.White;
            this.txt_Gname.Location = new System.Drawing.Point(37, 31);
            this.txt_Gname.Name = "txt_Gname";
            this.txt_Gname.Size = new System.Drawing.Size(83, 21);
            this.txt_Gname.TabIndex = 31;
            this.txt_Gname.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_Gname_KeyPress);
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(3, 36);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(37, 14);
            this.label8.Text = "托盘";
            // 
            // tabControlSet
            // 
            this.tabControlSet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tabControlSet.Controls.Add(this.tabPage1);
            this.tabControlSet.Controls.Add(this.tabPage6);
            this.tabControlSet.Controls.Add(this.tabPage2);
            this.tabControlSet.Dock = System.Windows.Forms.DockStyle.None;
            this.tabControlSet.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular);
            this.tabControlSet.Location = new System.Drawing.Point(0, 80);
            this.tabControlSet.Name = "tabControlSet";
            this.tabControlSet.SelectedIndex = 0;
            this.tabControlSet.Size = new System.Drawing.Size(240, 188);
            this.tabControlSet.TabIndex = 29;
            this.tabControlSet.SelectedIndexChanged += new System.EventHandler(this.tabControlSet_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tabPage1.Controls.Add(this.textBox3);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.textBox5);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.lv_OrderList);
            this.tabPage1.Location = new System.Drawing.Point(0, 0);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(240, 160);
            this.tabPage1.Text = "托盘";
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.Color.White;
            this.textBox3.Location = new System.Drawing.Point(60, 135);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(173, 21);
            this.textBox3.TabIndex = 46;
            this.textBox3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox3_KeyPress);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(3, 138);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 14);
            this.label3.Text = "目标货位";
            // 
            // textBox5
            // 
            this.textBox5.BackColor = System.Drawing.Color.White;
            this.textBox5.Location = new System.Drawing.Point(60, 109);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(173, 21);
            this.textBox5.TabIndex = 42;
            this.textBox5.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox5_KeyPress);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 112);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 14);
            this.label1.Text = "目标托盘";
            // 
            // tabPage6
            // 
            this.tabPage6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tabPage6.Controls.Add(this.listView1);
            this.tabPage6.Location = new System.Drawing.Point(0, 0);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Size = new System.Drawing.Size(240, 160);
            this.tabPage6.Text = "记录";
            // 
            // listView1
            // 
            this.listView1.Activation = System.Windows.Forms.ItemActivation.TwoClick;
            this.listView1.Columns.Add(this.columnHeader17);
            this.listView1.Columns.Add(this.columnHeader19);
            this.listView1.Columns.Add(this.columnHeader20);
            this.listView1.Columns.Add(this.columnHeader14);
            this.listView1.Columns.Add(this.columnHeader16);
            this.listView1.Columns.Add(this.columnHeader6);
            this.listView1.Columns.Add(this.columnHeader7);
            this.listView1.FullRowSelect = true;
            this.listView1.Location = new System.Drawing.Point(1, 5);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(238, 154);
            this.listView1.TabIndex = 2;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader17
            // 
            this.columnHeader17.Text = "行";
            this.columnHeader17.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader17.Width = 40;
            // 
            // columnHeader19
            // 
            this.columnHeader19.Text = "产品名称";
            this.columnHeader19.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader19.Width = 60;
            // 
            // columnHeader20
            // 
            this.columnHeader20.Text = "数量";
            this.columnHeader20.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader20.Width = 36;
            // 
            // columnHeader14
            // 
            this.columnHeader14.Text = "起始位置";
            this.columnHeader14.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader14.Width = 60;
            // 
            // columnHeader16
            // 
            this.columnHeader16.Text = "终止位置";
            this.columnHeader16.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader16.Width = 60;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "起始托盘";
            this.columnHeader6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader6.Width = 60;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "目标托盘";
            this.columnHeader7.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader7.Width = 60;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tabPage2.Controls.Add(this.txt_AssetsCode);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.lv_shInfo);
            this.tabPage2.Location = new System.Drawing.Point(0, 0);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(240, 160);
            this.tabPage2.Text = "单件";
            // 
            // txt_AssetsCode
            // 
            this.txt_AssetsCode.Location = new System.Drawing.Point(53, 9);
            this.txt_AssetsCode.Name = "txt_AssetsCode";
            this.txt_AssetsCode.Size = new System.Drawing.Size(184, 21);
            this.txt_AssetsCode.TabIndex = 7;
            this.txt_AssetsCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_AssetsCode_KeyPress_1);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(4, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 20);
            this.label5.Text = "单件码：";
            // 
            // lv_shInfo
            // 
            this.lv_shInfo.BackColor = System.Drawing.Color.White;
            this.lv_shInfo.Columns.Add(this.columnHeader10);
            this.lv_shInfo.Columns.Add(this.columnHeader11);
            this.lv_shInfo.FullRowSelect = true;
            this.lv_shInfo.Location = new System.Drawing.Point(2, 36);
            this.lv_shInfo.Name = "lv_shInfo";
            this.lv_shInfo.Size = new System.Drawing.Size(238, 123);
            this.lv_shInfo.TabIndex = 6;
            this.lv_shInfo.View = System.Windows.Forms.View.Details;
            this.lv_shInfo.ItemActivate += new System.EventHandler(this.lv_shInfo_ItemActivate_1);
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "行号";
            this.columnHeader10.Width = 40;
            // 
            // columnHeader11
            // 
            this.columnHeader11.Text = "单件编码";
            this.columnHeader11.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader11.Width = 120;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.button1.Font = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Bold);
            this.button1.Location = new System.Drawing.Point(119, 31);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(32, 21);
            this.button1.TabIndex = 40;
            this.button1.Text = "...";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(3, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 14);
            this.label2.Text = "仓库";
            // 
            // btn_CheckOrderQuery
            // 
            this.btn_CheckOrderQuery.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btn_CheckOrderQuery.Font = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Bold);
            this.btn_CheckOrderQuery.Location = new System.Drawing.Point(119, 6);
            this.btn_CheckOrderQuery.Name = "btn_CheckOrderQuery";
            this.btn_CheckOrderQuery.Size = new System.Drawing.Size(32, 21);
            this.btn_CheckOrderQuery.TabIndex = 45;
            this.btn_CheckOrderQuery.Text = "...";
            this.btn_CheckOrderQuery.Click += new System.EventHandler(this.btn_CheckOrderQuery_Click);
            // 
            // txt_OrderCode
            // 
            this.txt_OrderCode.Location = new System.Drawing.Point(37, 6);
            this.txt_OrderCode.Name = "txt_OrderCode";
            this.txt_OrderCode.Size = new System.Drawing.Size(83, 21);
            this.txt_OrderCode.TabIndex = 46;
            // 
            // tb_OrderCheckDesc
            // 
            this.tb_OrderCheckDesc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tb_OrderCheckDesc.Enabled = false;
            this.tb_OrderCheckDesc.Location = new System.Drawing.Point(149, 6);
            this.tb_OrderCheckDesc.Name = "tb_OrderCheckDesc";
            this.tb_OrderCheckDesc.ReadOnly = true;
            this.tb_OrderCheckDesc.Size = new System.Drawing.Size(86, 21);
            this.tb_OrderCheckDesc.TabIndex = 47;
            // 
            // Frm_MoveTray
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btn_CheckOrderQuery);
            this.Controls.Add(this.txt_OrderCode);
            this.Controls.Add(this.tb_OrderCheckDesc);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txt_Gname);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.tabControlSet);
            this.Menu = this.mainMenu1;
            this.Name = "Frm_MoveTray";
            this.Text = "托盘转移";
            this.Load += new System.EventHandler(this.Frm_MoveTray_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.Frm_MoveTray_Closing);
            this.tabControlSet.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.ListView lv_OrderList;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader12;
        private System.Windows.Forms.ColumnHeader columnHeader15;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txt_Gname;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TabControl tabControlSet;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader17;
        private System.Windows.Forms.ColumnHeader columnHeader19;
        private System.Windows.Forms.ColumnHeader columnHeader20;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ColumnHeader columnHeader14;
        private System.Windows.Forms.ColumnHeader columnHeader16;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_CheckOrderQuery;
        private System.Windows.Forms.TextBox txt_OrderCode;
        private System.Windows.Forms.TextBox tb_OrderCheckDesc;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox txt_AssetsCode;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ListView lv_shInfo;
        private System.Windows.Forms.ColumnHeader columnHeader10;
        private System.Windows.Forms.ColumnHeader columnHeader11;
    }
}