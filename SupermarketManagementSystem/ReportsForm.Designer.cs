namespace SupermarketManagementSystem
{
    partial class ReportsForm
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
            this.navPnl = new System.Windows.Forms.Panel();
            this.logoutNavLbl = new System.Windows.Forms.Label();
            this.profileNavLbl = new System.Windows.Forms.Label();
            this.reportsNavLbl = new System.Windows.Forms.Label();
            this.inventoryNavLbl = new System.Windows.Forms.Label();
            this.posNavLbl = new System.Windows.Forms.Label();
            this.dashboardNavLbl = new System.Windows.Forms.Label();
            this.mainLbl = new System.Windows.Forms.Label();
            this.navPnl.SuspendLayout();
            this.SuspendLayout();
            // 
            // navPnl
            // 
            this.navPnl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.navPnl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.navPnl.Controls.Add(this.logoutNavLbl);
            this.navPnl.Controls.Add(this.profileNavLbl);
            this.navPnl.Controls.Add(this.reportsNavLbl);
            this.navPnl.Controls.Add(this.inventoryNavLbl);
            this.navPnl.Controls.Add(this.posNavLbl);
            this.navPnl.Controls.Add(this.dashboardNavLbl);
            this.navPnl.Controls.Add(this.mainLbl);
            this.navPnl.Location = new System.Drawing.Point(0, 0);
            this.navPnl.Name = "navPnl";
            this.navPnl.Size = new System.Drawing.Size(1006, 88);
            this.navPnl.TabIndex = 1;
            // 
            // logoutNavLbl
            // 
            this.logoutNavLbl.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.logoutNavLbl.AutoSize = true;
            this.logoutNavLbl.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.logoutNavLbl.ForeColor = System.Drawing.Color.White;
            this.logoutNavLbl.Location = new System.Drawing.Point(654, 56);
            this.logoutNavLbl.Name = "logoutNavLbl";
            this.logoutNavLbl.Size = new System.Drawing.Size(58, 19);
            this.logoutNavLbl.TabIndex = 1;
            this.logoutNavLbl.Text = "Logout";
            this.logoutNavLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.logoutNavLbl.Click += new System.EventHandler(this.logoutNavLbl_Click);
            // 
            // profileNavLbl
            // 
            this.profileNavLbl.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.profileNavLbl.AutoSize = true;
            this.profileNavLbl.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.profileNavLbl.ForeColor = System.Drawing.Color.White;
            this.profileNavLbl.Location = new System.Drawing.Point(594, 56);
            this.profileNavLbl.Name = "profileNavLbl";
            this.profileNavLbl.Size = new System.Drawing.Size(54, 19);
            this.profileNavLbl.TabIndex = 1;
            this.profileNavLbl.Text = "Profile";
            this.profileNavLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.profileNavLbl.Click += new System.EventHandler(this.profileNavLbl_Click);
            // 
            // reportsNavLbl
            // 
            this.reportsNavLbl.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.reportsNavLbl.AutoSize = true;
            this.reportsNavLbl.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.reportsNavLbl.ForeColor = System.Drawing.Color.Brown;
            this.reportsNavLbl.Location = new System.Drawing.Point(525, 56);
            this.reportsNavLbl.Name = "reportsNavLbl";
            this.reportsNavLbl.Size = new System.Drawing.Size(63, 19);
            this.reportsNavLbl.TabIndex = 1;
            this.reportsNavLbl.Text = "Reports";
            this.reportsNavLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // inventoryNavLbl
            // 
            this.inventoryNavLbl.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.inventoryNavLbl.AutoSize = true;
            this.inventoryNavLbl.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.inventoryNavLbl.ForeColor = System.Drawing.Color.White;
            this.inventoryNavLbl.Location = new System.Drawing.Point(442, 56);
            this.inventoryNavLbl.Name = "inventoryNavLbl";
            this.inventoryNavLbl.Size = new System.Drawing.Size(77, 19);
            this.inventoryNavLbl.TabIndex = 1;
            this.inventoryNavLbl.Text = "Inventory";
            this.inventoryNavLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.inventoryNavLbl.Click += new System.EventHandler(this.inventoryNavLbl_Click);
            // 
            // posNavLbl
            // 
            this.posNavLbl.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.posNavLbl.AutoSize = true;
            this.posNavLbl.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.posNavLbl.ForeColor = System.Drawing.Color.White;
            this.posNavLbl.Location = new System.Drawing.Point(397, 56);
            this.posNavLbl.Name = "posNavLbl";
            this.posNavLbl.Size = new System.Drawing.Size(39, 19);
            this.posNavLbl.TabIndex = 1;
            this.posNavLbl.Text = "POS";
            this.posNavLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.posNavLbl.Click += new System.EventHandler(this.posNavLbl_Click);
            // 
            // dashboardNavLbl
            // 
            this.dashboardNavLbl.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.dashboardNavLbl.AutoSize = true;
            this.dashboardNavLbl.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dashboardNavLbl.ForeColor = System.Drawing.Color.White;
            this.dashboardNavLbl.Location = new System.Drawing.Point(306, 56);
            this.dashboardNavLbl.Name = "dashboardNavLbl";
            this.dashboardNavLbl.Size = new System.Drawing.Size(85, 19);
            this.dashboardNavLbl.TabIndex = 1;
            this.dashboardNavLbl.Text = "Dashboard";
            this.dashboardNavLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.dashboardNavLbl.Click += new System.EventHandler(this.dashboardNavLbl_Click);
            // 
            // mainLbl
            // 
            this.mainLbl.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.mainLbl.AutoSize = true;
            this.mainLbl.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mainLbl.ForeColor = System.Drawing.Color.White;
            this.mainLbl.Location = new System.Drawing.Point(326, 9);
            this.mainLbl.Name = "mainLbl";
            this.mainLbl.Size = new System.Drawing.Size(334, 25);
            this.mainLbl.TabIndex = 0;
            this.mainLbl.Text = "Supermarket Management System";
            // 
            // ReportsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayoutStore = System.Windows.Forms.ImageLayout.Stretch;
            this.BackgroundImageStore = global::SupermarketManagementSystem.Properties.Resources.background;
            this.ClientSize = new System.Drawing.Size(1005, 627);
            this.Controls.Add(this.navPnl);
            this.Name = "ReportsForm";
            this.Text = "Reports";
            this.navPnl.ResumeLayout(false);
            this.navPnl.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel navPnl;
        private System.Windows.Forms.Label logoutNavLbl;
        private System.Windows.Forms.Label profileNavLbl;
        private System.Windows.Forms.Label reportsNavLbl;
        private System.Windows.Forms.Label inventoryNavLbl;
        private System.Windows.Forms.Label posNavLbl;
        private System.Windows.Forms.Label dashboardNavLbl;
        private System.Windows.Forms.Label mainLbl;
    }
}