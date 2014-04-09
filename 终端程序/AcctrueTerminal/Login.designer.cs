namespace AcctrueTerminal
{
    partial class Login
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.lab_Version = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tabControlSet = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_UserPass = new System.Windows.Forms.TextBox();
            this.txt_UserName = new System.Windows.Forms.TextBox();
            this.lab_ErrorInfo = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.txt_Port = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.Btn_Ok = new System.Windows.Forms.Button();
            this.Btn_Set = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_Ip = new System.Windows.Forms.TextBox();
            this.checkUpdate = new System.Windows.Forms.CheckBox();
            this.cbIsVisible = new System.Windows.Forms.CheckBox();
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menuItemLogin = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tabControlSet.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lab_Version
            // 
            this.lab_Version.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular);
            this.lab_Version.Location = new System.Drawing.Point(51, 108);
            this.lab_Version.Name = "lab_Version";
            this.lab_Version.Size = new System.Drawing.Size(150, 20);
            this.lab_Version.Text = "版本号:V1.0.5678.98076";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.Lime;
            this.label3.Location = new System.Drawing.Point(29, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(172, 40);
            this.label3.Text = "仓库管理系统";
            // 
            // tabControlSet
            // 
            this.tabControlSet.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tabControlSet.Controls.Add(this.tabPage1);
            this.tabControlSet.Controls.Add(this.tabPage2);
            this.tabControlSet.Dock = System.Windows.Forms.DockStyle.None;
            this.tabControlSet.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular);
            this.tabControlSet.Location = new System.Drawing.Point(0, 134);
            this.tabControlSet.Name = "tabControlSet";
            this.tabControlSet.SelectedIndex = 0;
            this.tabControlSet.Size = new System.Drawing.Size(240, 160);
            this.tabControlSet.TabIndex = 45;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.White;
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.txt_UserPass);
            this.tabPage1.Controls.Add(this.txt_UserName);
            this.tabPage1.Controls.Add(this.lab_ErrorInfo);
            this.tabPage1.Location = new System.Drawing.Point(0, 0);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(240, 133);
            this.tabPage1.Text = "系统登录";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular);
            this.label2.Location = new System.Drawing.Point(39, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 20);
            this.label2.Text = "密   码：";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular);
            this.label1.Location = new System.Drawing.Point(37, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 20);
            this.label1.Text = "用户名：";
            // 
            // txt_UserPass
            // 
            this.txt_UserPass.Location = new System.Drawing.Point(102, 65);
            this.txt_UserPass.Name = "txt_UserPass";
            this.txt_UserPass.PasswordChar = '*';
            this.txt_UserPass.Size = new System.Drawing.Size(111, 21);
            this.txt_UserPass.TabIndex = 46;
            this.txt_UserPass.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_UserPass_KeyPress);
            // 
            // txt_UserName
            // 
            this.txt_UserName.Location = new System.Drawing.Point(102, 38);
            this.txt_UserName.Name = "txt_UserName";
            this.txt_UserName.Size = new System.Drawing.Size(111, 21);
            this.txt_UserName.TabIndex = 45;
            this.txt_UserName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_UserName_KeyPress);
            // 
            // lab_ErrorInfo
            // 
            this.lab_ErrorInfo.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lab_ErrorInfo.ForeColor = System.Drawing.Color.Red;
            this.lab_ErrorInfo.Location = new System.Drawing.Point(102, 99);
            this.lab_ErrorInfo.Name = "lab_ErrorInfo";
            this.lab_ErrorInfo.Size = new System.Drawing.Size(111, 20);
            this.lab_ErrorInfo.Text = "label6";
            this.lab_ErrorInfo.Visible = false;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.White;
            this.tabPage2.Controls.Add(this.txt_Port);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.Btn_Ok);
            this.tabPage2.Controls.Add(this.Btn_Set);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.txt_Ip);
            this.tabPage2.Location = new System.Drawing.Point(0, 0);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(232, 131);
            this.tabPage2.Text = "配置系统连接";
            // 
            // txt_Port
            // 
            this.txt_Port.Location = new System.Drawing.Point(93, 57);
            this.txt_Port.Name = "txt_Port";
            this.txt_Port.Size = new System.Drawing.Size(66, 21);
            this.txt_Port.TabIndex = 55;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular);
            this.label5.Location = new System.Drawing.Point(33, 61);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 20);
            this.label5.Text = "端口号：";
            // 
            // Btn_Ok
            // 
            this.Btn_Ok.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.Btn_Ok.Location = new System.Drawing.Point(119, 102);
            this.Btn_Ok.Name = "Btn_Ok";
            this.Btn_Ok.Size = new System.Drawing.Size(120, 31);
            this.Btn_Ok.TabIndex = 51;
            this.Btn_Ok.Text = "设置";
            this.Btn_Ok.Click += new System.EventHandler(this.Btn_Ok_Click);
            // 
            // Btn_Set
            // 
            this.Btn_Set.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.Btn_Set.Location = new System.Drawing.Point(0, 102);
            this.Btn_Set.Name = "Btn_Set";
            this.Btn_Set.Size = new System.Drawing.Size(120, 31);
            this.Btn_Set.TabIndex = 50;
            this.Btn_Set.Text = "获取当前配置";
            this.Btn_Set.Click += new System.EventHandler(this.Btn_Set_Click);
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular);
            this.label4.Location = new System.Drawing.Point(19, 32);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 20);
            this.label4.Text = "远程地址";
            // 
            // txt_Ip
            // 
            this.txt_Ip.Location = new System.Drawing.Point(93, 29);
            this.txt_Ip.Name = "txt_Ip";
            this.txt_Ip.Size = new System.Drawing.Size(129, 21);
            this.txt_Ip.TabIndex = 49;
            // 
            // checkUpdate
            // 
            this.checkUpdate.Location = new System.Drawing.Point(122, 132);
            this.checkUpdate.Name = "checkUpdate";
            this.checkUpdate.Size = new System.Drawing.Size(117, 20);
            this.checkUpdate.TabIndex = 46;
            this.checkUpdate.Text = "是否检查更新";
            this.checkUpdate.Visible = false;
            // 
            // cbIsVisible
            // 
            this.cbIsVisible.Enabled = false;
            this.cbIsVisible.Location = new System.Drawing.Point(9, 132);
            this.cbIsVisible.Name = "cbIsVisible";
            this.cbIsVisible.Size = new System.Drawing.Size(111, 20);
            this.cbIsVisible.TabIndex = 50;
            this.cbIsVisible.Text = "是否启用离线";
            this.cbIsVisible.Visible = false;
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menuItemLogin);
            this.mainMenu1.MenuItems.Add(this.menuItem2);
            // 
            // menuItemLogin
            // 
            this.menuItemLogin.Text = "登   录";
            this.menuItemLogin.Click += new System.EventHandler(this.Btn_Login_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Text = "退   出";
            this.menuItem2.Click += new System.EventHandler(this.Btn_Close_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(1, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(238, 42);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.ControlBox = false;
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.cbIsVisible);
            this.Controls.Add(this.checkUpdate);
            this.Controls.Add(this.tabControlSet);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lab_Version);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Location = new System.Drawing.Point(0, 0);
            this.Menu = this.mainMenu1;
            this.Name = "Login";
            this.Text = "登录";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Login_Load);
            this.tabControlSet.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lab_Version;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabControl tabControlSet;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_UserPass;
        private System.Windows.Forms.TextBox txt_UserName;
        private System.Windows.Forms.Label lab_ErrorInfo;
        private System.Windows.Forms.CheckBox checkUpdate;
        private System.Windows.Forms.Button Btn_Ok;
        private System.Windows.Forms.Button Btn_Set;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_Ip;
        private System.Windows.Forms.TextBox txt_Port;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox cbIsVisible;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menuItemLogin;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}