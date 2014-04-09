namespace AcctrueTerminal.Stock
{
    partial class StockSearch
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.btn_QueryData = new System.Windows.Forms.Button();
            this.btn_SelectWh = new System.Windows.Forms.Button();
            this.txt_Code = new System.Windows.Forms.TextBox();
            this.txt_WhCode = new System.Windows.Forms.TextBox();
            this.txt_WhName = new System.Windows.Forms.TextBox();
            this.txt_Bin = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lv_QueryData = new System.Windows.Forms.ListView();
            this.columnHeader9 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader10 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader8 = new System.Windows.Forms.ColumnHeader();
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.btn_QueryData);
            this.panel1.Controls.Add(this.btn_SelectWh);
            this.panel1.Controls.Add(this.txt_Code);
            this.panel1.Controls.Add(this.txt_WhCode);
            this.panel1.Controls.Add(this.txt_WhName);
            this.panel1.Controls.Add(this.txt_Bin);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Location = new System.Drawing.Point(0, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(240, 83);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(4, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 14);
            this.label4.Text = "仓库";
            // 
            // btn_QueryData
            // 
            this.btn_QueryData.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btn_QueryData.Location = new System.Drawing.Point(196, 28);
            this.btn_QueryData.Name = "btn_QueryData";
            this.btn_QueryData.Size = new System.Drawing.Size(38, 45);
            this.btn_QueryData.TabIndex = 24;
            this.btn_QueryData.Text = "查询";
            this.btn_QueryData.Click += new System.EventHandler(this.Btn_Query);
            // 
            // btn_SelectWh
            // 
            this.btn_SelectWh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btn_SelectWh.Font = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Bold);
            this.btn_SelectWh.Location = new System.Drawing.Point(127, 4);
            this.btn_SelectWh.Name = "btn_SelectWh";
            this.btn_SelectWh.Size = new System.Drawing.Size(32, 21);
            this.btn_SelectWh.TabIndex = 9;
            this.btn_SelectWh.Text = "...";
            this.btn_SelectWh.Click += new System.EventHandler(this.btn_SelectWh_Click);
            // 
            // txt_Code
            // 
            this.txt_Code.BackColor = System.Drawing.Color.White;
            this.txt_Code.Location = new System.Drawing.Point(46, 52);
            this.txt_Code.Name = "txt_Code";
            this.txt_Code.Size = new System.Drawing.Size(147, 21);
            this.txt_Code.TabIndex = 23;
            // 
            // txt_WhCode
            // 
            this.txt_WhCode.Location = new System.Drawing.Point(46, 4);
            this.txt_WhCode.Name = "txt_WhCode";
            this.txt_WhCode.Size = new System.Drawing.Size(83, 21);
            this.txt_WhCode.TabIndex = 11;
            // 
            // txt_WhName
            // 
            this.txt_WhName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txt_WhName.Enabled = false;
            this.txt_WhName.Location = new System.Drawing.Point(156, 4);
            this.txt_WhName.Name = "txt_WhName";
            this.txt_WhName.ReadOnly = true;
            this.txt_WhName.Size = new System.Drawing.Size(78, 21);
            this.txt_WhName.TabIndex = 12;
            // 
            // txt_Bin
            // 
            this.txt_Bin.BackColor = System.Drawing.Color.White;
            this.txt_Bin.Location = new System.Drawing.Point(46, 28);
            this.txt_Bin.Name = "txt_Bin";
            this.txt_Bin.Size = new System.Drawing.Size(147, 21);
            this.txt_Bin.TabIndex = 17;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(5, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 14);
            this.label1.Text = "位置";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(5, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 18);
            this.label2.Text = "编码";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular);
            this.label3.ForeColor = System.Drawing.Color.IndianRed;
            this.label3.Location = new System.Drawing.Point(-2, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(246, 6);
            this.label3.Text = "－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lv_QueryData
            // 
            this.lv_QueryData.Columns.Add(this.columnHeader9);
            this.lv_QueryData.Columns.Add(this.columnHeader1);
            this.lv_QueryData.Columns.Add(this.columnHeader2);
            this.lv_QueryData.Columns.Add(this.columnHeader10);
            this.lv_QueryData.Columns.Add(this.columnHeader3);
            this.lv_QueryData.Columns.Add(this.columnHeader4);
            this.lv_QueryData.Columns.Add(this.columnHeader5);
            this.lv_QueryData.Columns.Add(this.columnHeader6);
            this.lv_QueryData.Columns.Add(this.columnHeader7);
            this.lv_QueryData.Columns.Add(this.columnHeader8);
            this.lv_QueryData.FullRowSelect = true;
            this.lv_QueryData.Location = new System.Drawing.Point(1, 92);
            this.lv_QueryData.Name = "lv_QueryData";
            this.lv_QueryData.Size = new System.Drawing.Size(238, 176);
            this.lv_QueryData.TabIndex = 4;
            this.lv_QueryData.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "序号";
            this.columnHeader9.Width = 40;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "仓库";
            this.columnHeader1.Width = 60;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "位置";
            this.columnHeader2.Width = 60;
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "托盘";
            this.columnHeader10.Width = 60;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "编码";
            this.columnHeader3.Width = 60;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "名称";
            this.columnHeader4.Width = 60;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "数量";
            this.columnHeader5.Width = 60;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "批次";
            this.columnHeader6.Width = 60;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "规格";
            this.columnHeader7.Width = 60;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "单位";
            this.columnHeader8.Width = 60;
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menuItem1);
            this.mainMenu1.MenuItems.Add(this.menuItem2);
            // 
            // menuItem1
            // 
            this.menuItem1.Text = "查    询";
            this.menuItem1.Click += new System.EventHandler(this.Btn_Query);
            // 
            // menuItem2
            // 
            this.menuItem2.Text = "返    回";
            this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
            // 
            // StockSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this.lv_QueryData);
            this.Controls.Add(this.panel1);
            this.Menu = this.mainMenu1;
            this.Name = "StockSearch";
            this.Text = "库存查询";
            this.Load += new System.EventHandler(this.StockSearch_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.StockSearch_Closing);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_Bin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView lv_QueryData;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.TextBox txt_Code;
        private System.Windows.Forms.Button btn_QueryData;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btn_SelectWh;
        private System.Windows.Forms.TextBox txt_WhCode;
        private System.Windows.Forms.TextBox txt_WhName;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader10;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem2;
    }
}