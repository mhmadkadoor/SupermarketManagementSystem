using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SupermarketManagementSystem
{
    public partial class PosForm : DevExpress.XtraEditors.XtraForm
    {
        
        
        public PosForm()
        {
            InitializeComponent();
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

        private void inventoryNavLbl_Click(object sender, EventArgs e)
        {
            FormManager.ShowForm(this, "InventoryForm");
        }

        private void profileNavLbl_Click(object sender, EventArgs e)
        {
            FormManager.ShowForm(this, "ProfileForm");
        }
    }
}