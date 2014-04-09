namespace AcctrueTerminal
{
    partial class Frm_Batch
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
            this.lv_QueryData = new System.Windows.Forms.ListView();
            this.columnHeader9 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader8 = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // lv_QueryData
            // 
            this.lv_QueryData.Columns.Add(this.columnHeader9);
            this.lv_QueryData.Columns.Add(this.columnHeader3);
            this.lv_QueryData.Columns.Add(this.columnHeader4);
            this.lv_QueryData.Columns.Add(this.columnHeader5);
            this.lv_QueryData.Columns.Add(this.columnHeader6);
            this.lv_QueryData.Columns.Add(this.columnHeader7);
            this.lv_QueryData.Columns.Add(this.columnHeader8);
            this.lv_QueryData.FullRowSelect = true;
            this.lv_QueryData.Location = new System.Drawing.Point(0, 1);
            this.lv_QueryData.Name = "lv_QueryData";
            this.lv_QueryData.Size = new System.Drawing.Size(240, 290);
            this.lv_QueryData.TabIndex = 5;
            this.lv_QueryData.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "序号";
            this.columnHeader9.Width = 40;
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
            // Frm_Batch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.Controls.Add(this.lv_QueryData);
            this.Name = "Frm_Batch";
            this.Text = "选择批次";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lv_QueryData;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
    }
}