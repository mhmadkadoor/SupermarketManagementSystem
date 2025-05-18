using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace SupermarketManagementSystem
{
    public partial class LoginForm : DevExpress.XtraEditors.XtraForm
    {
        public LoginForm()
        {
            InitializeComponent();

            CenterPanel();
            _ = FormManager.PreloadFormsAsync();
        }
        private void CenterPanel()
        {
            if (this.Controls.ContainsKey("loginPnl"))
            {
                var loginPnl = this.Controls["loginPnl"];
                loginPnl.Left = (this.ClientSize.Width - loginPnl.Width) / 2;
                loginPnl.Top = (this.ClientSize.Height - loginPnl.Height) / 2;
            }
        }

        private void loginBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(usernameTxtF.Text) || string.IsNullOrEmpty(passwordTxtF.Text))
            {

                notifacationLbl.Text = "Please enter your username and password.";
                notifacationLbl.Visible = true;
                return;
            }
            else if (DatabaseManager.ValidateUser(usernameTxtF.Text, passwordTxtF.Text))
            {
                User user = new User(DatabaseManager.GetUserIdByUsername(usernameTxtF.Text));
                User.CurrentUser = user;
                user= null;

                // Hide the login form
                this.Hide();

                // Show the dashboard form
                FormManager.ShowForm(this, "DashboardForm");

                // Clear the text fields
                usernameTxtF.Text = string.Empty;
                passwordTxtF.Text = string.Empty;

                // Clear the notification label
                notifacationLbl.Text = string.Empty;
                notifacationLbl.Visible = false;


            }
            else
            {
                notifacationLbl.Text = "Invalid username or password.";
                notifacationLbl.Visible = true;
                // Clear the text fields
                usernameTxtF.Text = string.Empty;
                passwordTxtF.Text = string.Empty;
            }

        }

    }
}
