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
    public partial class DashboardForm : DevExpress.XtraEditors.XtraForm
    {
       
        
        public DashboardForm()
        {
            InitializeComponent();

            //// Subscribe to the event for tracking preloading
            //FormManager.OnFormPreloaded += (formName, loadedForms, totalForms) =>
            //{
            //    Console.WriteLine($"Loaded {loadedForms}/{totalForms}: {formName}");
            //};

            //Thread.Sleep(250);
            
            PanelManeger.InitializePanels(this, dashboardPnl, posPnl , inventoryPnl, reportsPnl, profilePnl);
            NavbarManager.InitializeLabels(dashboardNavLbl, posNavLbl, inventoryNavLbl,reportsNavLbl, profileNavLbl,  logoutNavLbl);
            UpdateComponents();
        }
        private void UpdateComponents()
        {
            if (User.CurrentUser != null)
            {
                // Set for Profile panel
                usernameTxtF.Text = User.CurrentUser.username;
                firstnameTxtF.Text = User.CurrentUser.firstname;
                lastnameTxtF.Text = User.CurrentUser.lastname;
            }
        }

        private void logoutNavLbl_Click(object sender, EventArgs e)
        {
            FormManager.ShowForm(this, "LoginForm");
            User.CurrentUser = null; // Clear the current user
            PanelManeger.ShowPanelByName("dashboard");

        }

        private void posNavLbl_Click(object sender, EventArgs e)
        {
            PanelManeger.ShowPanelByName("pos");

        }

        private void inventoryNavLbl_Click(object sender, EventArgs e)
        {
            PanelManeger.ShowPanelByName("inventory");
        }

        private void profileNavLbl_Click(object sender, EventArgs e)
        {

            PanelManeger.ShowPanelByName("profile");
            UpdateComponents();

        }
        private void reportsNavLbl_Click(object sender, EventArgs e)
        {
            PanelManeger.ShowPanelByName("report");

        }

        private void dashboardNavLbl_Click(object sender, EventArgs e)
        {
            PanelManeger.ShowPanelByName("dashboard");
        }

        private void updatenamesBtn_Click(object sender, EventArgs e)
        {
            string username= usernameTxtF.Text.Trim();
            firstnameTxtF.Text = firstnameTxtF.Text.Trim();
            lastnameTxtF.Text = lastnameTxtF.Text.Trim();

            if (string.IsNullOrEmpty(firstnameTxtF.Text) || string.IsNullOrEmpty(lastnameTxtF.Text) || string.IsNullOrEmpty(usernameTxtF.Text))
            {
                notifacationProfUULbl.Text = "Please enter your first name, last name,\n and username.";
                notifacationProfUULbl.ForeColor = Color.Red;
                updatenamesProfUUBtn.Location = new Point(updatenamesProfUUBtn.Location.X, notifacationProfUULbl.Bottom + 25);
                notifacationProfUULbl.Visible = true;
                return;
            }
            else if (firstnameTxtF.Text.Length < 4 || firstnameTxtF.Text.Length > 15 ||
                     lastnameTxtF.Text.Length < 4 || lastnameTxtF.Text.Length > 15 ||
                     username.Length < 4 || username.Length > 15)
            {
                notifacationProfUULbl.Text = "First name, last name,\nand username must be \nbetween 4 and 15 characters.";
                notifacationProfUULbl.ForeColor = Color.Red;
                updatenamesProfUUBtn.Location = new Point(updatenamesProfUUBtn.Location.X, notifacationProfUULbl.Bottom + 25);
                notifacationProfUULbl.Visible = true;
                return;
            }
            else if (username.Contains(" "))
            {
                notifacationProfUULbl.Text = "Username cannot contain spaces.";
                notifacationProfUULbl.ForeColor = Color.Red;
                updatenamesProfUUBtn.Location = new Point(updatenamesProfUUBtn.Location.X, notifacationProfUULbl.Bottom + 25);
                notifacationProfUULbl.Visible = true;
                return;
            }
            else if (!DatabaseManager.IsUsernameAveilable(username) && User.CurrentUser.username != username  )
            {
                notifacationProfUULbl.Text = "Username is already taken.";
                notifacationProfUULbl.ForeColor = Color.Red;
                updatenamesProfUUBtn.Location = new Point(updatenamesProfUUBtn.Location.X, notifacationProfUULbl.Bottom + 25);
                notifacationProfUULbl.Visible = true;
                return;
            }
            else if (User.CurrentUser.username == username || DatabaseManager.IsUsernameAveilable(username))//hbbhhjb
            {
                DatabaseManager.UpdateUserInfo(User.CurrentUser.id, username, firstnameTxtF.Text, lastnameTxtF.Text);
                notifacationProfUULbl.Text = "Profile updated successfully.";
                notifacationProfUULbl.ForeColor = Color.Green;
                updatenamesProfUUBtn.Location = new Point(updatenamesProfUUBtn.Location.X, notifacationProfUULbl.Bottom + 25);
                notifacationProfUULbl.Visible = true;
            }
            else
            {
                notifacationProfUULbl.Text = "Failed to update profile.";
                notifacationProfUULbl.ForeColor = Color.Red;
                updatenamesProfUUBtn.Location = new Point(updatenamesProfUUBtn.Location.X, notifacationProfUULbl.Bottom + 25);
                notifacationProfUULbl.Visible = true;
            }

        }

        private void updatepasswordProfUPBtn_Click(object sender, EventArgs e)
        {
            string currentpassword = currentpasswordProfUPTxtF.Text.Trim();
            string newpassword = newpasswordProfUPTxtF.Text.Trim();
            string confirmpassword = confirmpasswordProfUPTxtF.Text.Trim();
            if (string.IsNullOrEmpty(currentpassword) || string.IsNullOrEmpty(newpassword) || string.IsNullOrEmpty(confirmpassword))
            {
                notifacationProfUPLbl.Text = "Please enter your \ncurrent password, new password,\n and confirm password.";
                notifacationProfUPLbl.ForeColor = Color.Red;
                updatepasswordProfUPBtn.Location = new Point(updatepasswordProfUPBtn.Location.X, notifacationProfUPLbl.Bottom + 25);
                notifacationProfUPLbl.Visible = true;
                return;
            }
            else if (newpassword.Length < 8 || newpassword.Length > 15)
            {
                notifacationProfUPLbl.Text = "New password must be \nbetween 8 and 15 characters.";
                notifacationProfUPLbl.ForeColor = Color.Red;
                updatepasswordProfUPBtn.Location = new Point(updatepasswordProfUPBtn.Location.X, notifacationProfUPLbl.Bottom + 25);
                notifacationProfUPLbl.Visible = true;
                return;
            }
            else if (newpassword != confirmpassword)
            {
                notifacationProfUPLbl.Text = "New password and confirm \npassword do not match.";
                notifacationProfUPLbl.ForeColor = Color.Red;
                updatepasswordProfUPBtn.Location = new Point(updatepasswordProfUPBtn.Location.X, notifacationProfUPLbl.Bottom + 25);
                notifacationProfUPLbl.Visible = true;
                return;
            }
            else if (DatabaseManager.ValidateUser(User.CurrentUser.username, currentpassword))
            {
                DatabaseManager.ChangeUserPassword(User.CurrentUser.username, newpassword);
                notifacationProfUPLbl.Text = "Password updated successfully.";
                notifacationProfUPLbl.ForeColor = Color.Green;
                updatepasswordProfUPBtn.Location = new Point(updatepasswordProfUPBtn.Location.X, notifacationProfUPLbl.Bottom + 25);
                notifacationProfUPLbl.Visible = true;
            }
            else
            {
                notifacationProfUPLbl.Text = "Current password is incorrect.";
                notifacationProfUPLbl.ForeColor = Color.Red;
                updatepasswordProfUPBtn.Location = new Point(updatepasswordProfUPBtn.Location.X, notifacationProfUPLbl.Bottom + 25);
                notifacationProfUPLbl.Visible = true;
            }
        }
    }
}