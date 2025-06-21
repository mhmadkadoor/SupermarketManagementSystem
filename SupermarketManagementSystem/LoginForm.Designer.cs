namespace SupermarketManagementSystem
{
    partial class LoginForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.loginPnl = new System.Windows.Forms.Panel();
            this.exitLbl = new System.Windows.Forms.Label();
            this.notifacationLbl = new System.Windows.Forms.Label();
            this.usernameLbl = new System.Windows.Forms.Label();
            this.usernameTxtF = new DevExpress.XtraEditors.TextEdit();
            this.passwordLbl = new System.Windows.Forms.Label();
            this.passwordTxtF = new DevExpress.XtraEditors.TextEdit();
            this.loginBtn = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.loginPnl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.usernameTxtF.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.passwordTxtF.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // loginPnl
            // 
            resources.ApplyResources(this.loginPnl, "loginPnl");
            this.loginPnl.Controls.Add(this.simpleButton1);
            this.loginPnl.Controls.Add(this.exitLbl);
            this.loginPnl.Controls.Add(this.notifacationLbl);
            this.loginPnl.Controls.Add(this.usernameLbl);
            this.loginPnl.Controls.Add(this.usernameTxtF);
            this.loginPnl.Controls.Add(this.passwordLbl);
            this.loginPnl.Controls.Add(this.passwordTxtF);
            this.loginPnl.Controls.Add(this.loginBtn);
            this.loginPnl.Name = "loginPnl";
            // 
            // exitLbl
            // 
            resources.ApplyResources(this.exitLbl, "exitLbl");
            this.exitLbl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.exitLbl.Name = "exitLbl";
            this.exitLbl.Click += new System.EventHandler(this.exitLbl_Click);
            // 
            // notifacationLbl
            // 
            resources.ApplyResources(this.notifacationLbl, "notifacationLbl");
            this.notifacationLbl.ForeColor = System.Drawing.Color.Red;
            this.notifacationLbl.Name = "notifacationLbl";
            // 
            // usernameLbl
            // 
            resources.ApplyResources(this.usernameLbl, "usernameLbl");
            this.usernameLbl.Name = "usernameLbl";
            // 
            // usernameTxtF
            // 
            resources.ApplyResources(this.usernameTxtF, "usernameTxtF");
            this.usernameTxtF.Name = "usernameTxtF";
            this.usernameTxtF.Properties.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("usernameTxtF.Properties.Appearance.Font")));
            this.usernameTxtF.Properties.Appearance.Options.UseFont = true;
            // 
            // passwordLbl
            // 
            resources.ApplyResources(this.passwordLbl, "passwordLbl");
            this.passwordLbl.Name = "passwordLbl";
            // 
            // passwordTxtF
            // 
            resources.ApplyResources(this.passwordTxtF, "passwordTxtF");
            this.passwordTxtF.Name = "passwordTxtF";
            this.passwordTxtF.Properties.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("passwordTxtF.Properties.Appearance.Font")));
            this.passwordTxtF.Properties.Appearance.Options.UseFont = true;
            this.passwordTxtF.Properties.UseSystemPasswordChar = true;
            // 
            // loginBtn
            // 
            resources.ApplyResources(this.loginBtn, "loginBtn");
            this.loginBtn.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("loginBtn.Appearance.Font")));
            this.loginBtn.Appearance.Options.UseFont = true;
            this.loginBtn.Name = "loginBtn";
            this.loginBtn.Click += new System.EventHandler(this.loginBtn_Click);
            // 
            // simpleButton1
            // 
            resources.ApplyResources(this.simpleButton1, "simpleButton1");
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // LoginForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayoutStore = System.Windows.Forms.ImageLayout.Stretch;
            this.BackgroundImageStore = global::SupermarketManagementSystem.Properties.Resources.background;
            this.Controls.Add(this.loginPnl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "LoginForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.loginPnl.ResumeLayout(false);
            this.loginPnl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.usernameTxtF.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.passwordTxtF.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel loginPnl;
        private DevExpress.XtraEditors.SimpleButton loginBtn;
        private DevExpress.XtraEditors.TextEdit passwordTxtF;
        private System.Windows.Forms.Label passwordLbl;
        private System.Windows.Forms.Label usernameLbl;
        private DevExpress.XtraEditors.TextEdit usernameTxtF;
        private System.Windows.Forms.Label notifacationLbl;
        private System.Windows.Forms.Label exitLbl;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
    }
}

