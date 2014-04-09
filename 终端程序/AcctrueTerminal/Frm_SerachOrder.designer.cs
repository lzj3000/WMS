namespace AcctrueTerminal
{
    partial class Frm_SerachOrder
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
            this.menuItemChecked = new System.Windows.Forms.MenuItem();
            this.menuItemDownLoad = new System.Windows.Forms.MenuItem();
            this.lv_Order = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader8 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader9 = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menuItemChecked);
            this.mainMenu1.MenuItems.Add(this.menuItemDownLoad);
            // 
            // menuItemChecked
            // 
            this.menuItemChecked.Text = "选择";
            this.menuItemChecked.Click += new System.EventHandler(this.menuItemChecked_Click);
            // 
            // menuItemDownLoad
            // 
            this.menuItemDownLoad.Enabled = false;
            this.menuItemDownLoad.Text = "";
            // 
            // lv_Order
            // 
            this.lv_Order.Activation = System.Windows.Forms.ItemActivation.TwoClick;
            this.lv_Order.Columns.Add(this.columnHeader1);
            this.lv_Order.Columns.Add(this.columnHeader2);
            this.lv_Order.Columns.Add(this.columnHeader4);
            this.lv_Order.Columns.Add(this.columnHeader5);
            this.lv_Order.Columns.Add(this.columnHeader3);
            this.lv_Order.Columns.Add(this.columnHeader6);
            this.lv_Order.Columns.Add(this.columnHeader7);
            this.lv_Order.Columns.Add(this.columnHeader8);
            this.lv_Order.Columns.Add(this.columnHeader9);
            this.lv_Order.FullRowSelect = true;
            this.lv_Order.Location = new System.Drawing.Point(2, 3);
            this.lv_Order.Name = "lv_Order";
            this.lv_Order.Size = new System.Drawing.Size(236, 264);
            this.lv_Order.TabIndex = 22;
            this.lv_Order.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "单号";
            this.columnHeader1.Width = 100;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "创建人";
            this.columnHeader2.Width = 100;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "状态";
            this.columnHeader4.Width = 0;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "类型";
            this.columnHeader5.Width = 0;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "仓库";
            this.columnHeader3.Width = 60;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "OrderID";
            this.columnHeader6.Width = 0;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "SOURCEID";
            this.columnHeader7.Width = 0;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "WhourseID";
            this.columnHeader8.Width = 0;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "MCODE";
            this.columnHeader9.Width = 0;
            // 
            // Frm_SerachOrder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(240, 270);
            this.Controls.Add(this.lv_Order);
            this.Menu = this.mainMenu1;
            this.Name = "Frm_SerachOrder";
            this.Text = "选择单号";
            this.Load += new System.EventHandler(this.Frm_SerachOrder_Load);
            this.Closed += new System.EventHandler(this.Frm_SerachOrder_Closed);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem menuItemChecked;
        private System.Windows.Forms.MenuItem menuItemDownLoad;
        private System.Windows.Forms.ListView lv_Order;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader9;
    }
}