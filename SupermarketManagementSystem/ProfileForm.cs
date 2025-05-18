using DevExpress.XtraEditors;
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


namespace SupermarketManagementSystem
{
    

    public partial class ProfileForm : DevExpress.XtraEditors.XtraForm
    {
        public ProfileForm()
        {
            InitializeComponent();
            CenterPanel();
            SetComponents();
        }
        private void CenterPanel()
        {
            if (this.Controls.ContainsKey("profilePnl"))
            {
                var profilePnl = this.Controls["profilePnl"];
                profilePnl.Left = (this.ClientSize.Width - profilePnl.Width) / 2;
                //profilePnl.Top = (this.ClientSize.Height - profilePnl.Height) / 2;
            }
        }
        private void SetComponents()
        {
            // Set the text of the labels to the current user's information
            usernameTxtF.Text = User.CurrentUser.username;
            firstnameTxtF.Text = User.CurrentUser.firstname;
            lastnameTxtF.Text = User.CurrentUser.lastname;
        }
        private void reportsNavLbl_Click(object sender, EventArgs e)
        {
            Form ReportsForm = new ReportsForm();
            FormManager.ShowForm(this, "ReportsForm");

        }

        private void dashboardNavLbl_Click(object sender, EventArgs e)
        {
            FormManager.ShowForm(this, "DashboardForm");
        }


        private void logoutNavLbl_Click(object sender, EventArgs e)
        {

            Form LoginForm = new LoginForm();
            FormManager.ShowForm(this, "LoginForm");
            this.Hide();

        }

        private void posNavLbl_Click(object sender, EventArgs e)
        {
            FormManager.ShowForm(this, "PosForm");

        }

        private void inventoryNavLbl_Click(object sender, EventArgs e)
        {
            FormManager.ShowForm(this, "InventoryForm");
        }

        private void updatenamesBtn_Click(object sender, EventArgs e)
        {
            usernameTxtF.Text = usernameTxtF.Text.Trim();
            firstnameTxtF.Text = firstnameTxtF.Text.Trim();
            lastnameTxtF.Text = lastnameTxtF.Text.Trim();

            if (string.IsNullOrEmpty(firstnameTxtF.Text) || string.IsNullOrEmpty(lastnameTxtF.Text) || string.IsNullOrEmpty(usernameTxtF.Text))
            {
                notifacationLbl.Text = "Please enter your first name, last name, and username.";
                notifacationLbl.ForeColor = Color.Red;
                notifacationLbl.Visible = true;
                return;
            }
            else if (firstnameTxtF.Text.Length < 4 || firstnameTxtF.Text.Length > 15 ||
                     lastnameTxtF.Text.Length < 4 || lastnameTxtF.Text.Length > 15 ||
                     usernameTxtF.Text.Length < 4 || usernameTxtF.Text.Length > 15)
            {
                notifacationLbl.Text = "First name, last name, and username must be between 4 and 15 characters.";
                notifacationLbl.ForeColor = Color.Red;
                notifacationLbl.Visible = true;
                return;
            }
            else if (usernameTxtF.Text.Contains(" "))
            {
                notifacationLbl.Text = "Username cannot contain spaces.";
                notifacationLbl.ForeColor = Color.Red;
                notifacationLbl.Visible = true;
                return;
            }
            else if (!DatabaseManager.IsUsernameAveilable(usernameTxtF.Text))
            {
                notifacationLbl.Text = "Username is already taken.";
                notifacationLbl.ForeColor = Color.Red;
                notifacationLbl.Visible = true;
                return;
            }
            else if (DatabaseManager.IsUsernameAveilable(usernameTxtF.Text))
            {
                DatabaseManager.UpdateUserInfo(User.CurrentUser.id, usernameTxtF.Text, firstnameTxtF.Text, lastnameTxtF.Text);
                notifacationLbl.Text = "Profile updated successfully.";
                notifacationLbl.ForeColor = Color.Green;
                notifacationLbl.Visible = true;
            }
            else
            {
                notifacationLbl.Text = "Failed to update profile.";
                notifacationLbl.ForeColor = Color.Red;
                notifacationLbl.Visible = true;
            }

        }
    }
}