using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SupermarketManagementSystem
{
    internal class PanelManeger
    {
        private static Dictionary<string, Panel> formInstances = new Dictionary<string, Panel>();

        private static Panel dashboardPnl;
        private static Panel posPnl;
        private static Panel inventoryPnl;
        private static Panel profilePnl;
        private static Panel reportPnl;
        private static Form currentForm;

        public static void InitializePanels(Form passedForm, Panel dashboard, Panel pos,Panel inventory, Panel report , Panel profile)
        {
            currentForm = passedForm;
            dashboardPnl = dashboard;
            posPnl = pos;
            profilePnl = profile;
            reportPnl = report;
            inventoryPnl = inventory;
        }

        public static void ShowPanelByName(string panelName)
        {

            // Show the specified panel based on the name  
            switch (panelName.ToLower())
            {
                case "dashboard":
                    if (dashboardPnl != null)
                    {
                        if (posPnl != null) posPnl.Visible = false;
                        if (profilePnl != null) profilePnl.Visible = false;
                        if (reportPnl != null) reportPnl.Visible = false;
                        if (inventoryPnl != null) inventoryPnl.Visible = false;

                        dashboardPnl.Left = (currentForm.ClientSize.Width - dashboardPnl.Width) / 2;
                        dashboardPnl.Top = (currentForm.ClientSize.Height - dashboardPnl.Height) / 2 + 44;
                        dashboardPnl.Visible = true;
                        NavbarManager.SwitchTo("Dashboard");
                        currentForm.Text = "Dashboard";
                    }
                    break;
                case "pos":
                    if (posPnl != null) {
                        if (dashboardPnl != null) dashboardPnl.Visible = false;
                        if (profilePnl != null) profilePnl.Visible = false;
                        if (reportPnl != null) reportPnl.Visible = false;
                        if (inventoryPnl != null) inventoryPnl.Visible = false;

                        posPnl.Left = (currentForm.ClientSize.Width - posPnl.Width) / 2;
                        posPnl.Top = (currentForm.ClientSize.Height - posPnl.Height) / 2 + 44;
                        posPnl.Visible = true;
                        
                        NavbarManager.SwitchTo("POS");
                        currentForm.Text = "POS";

                    }
                    break;
                case "profile":
                    if (profilePnl != null) {
                        if (dashboardPnl != null) dashboardPnl.Visible = false;
                        if (posPnl != null) posPnl.Visible = false;
                        if (reportPnl != null) reportPnl.Visible = false;
                        if (inventoryPnl != null) inventoryPnl.Visible = false;

                        profilePnl.Left = (currentForm.ClientSize.Width - profilePnl.Width) / 2;
                    profilePnl.Top = (currentForm.ClientSize.Height - profilePnl.Height) / 2 + 44;
                    profilePnl.Visible = true;
                    NavbarManager.SwitchTo("Profile");
                    currentForm.Text = "Profile";
                    }
                    break;
                case "report":
                    if (reportPnl != null)
                    {
                        if (dashboardPnl != null) dashboardPnl.Visible = false;
                        if (posPnl != null) posPnl.Visible = false;
                        if (profilePnl != null) profilePnl.Visible = false;
                        if (inventoryPnl != null) inventoryPnl.Visible = false;

                        reportPnl.Left = (currentForm.ClientSize.Width - reportPnl.Width) / 2;
                        reportPnl.Top = (currentForm.ClientSize.Height - reportPnl.Height) / 2 + 44;

                        reportPnl.Visible = true;
                        NavbarManager.SwitchTo("Reports");
                        currentForm.Text = "Reports";
                    }
                    break;
                case "inventory":
                    if (inventoryPnl != null)
                    {
                        if (dashboardPnl != null) dashboardPnl.Visible = false;
                        if (posPnl != null) posPnl.Visible = false;
                        if (profilePnl != null) profilePnl.Visible = false;
                        if (reportPnl != null) reportPnl.Visible = false;

                        inventoryPnl.Left = (currentForm.ClientSize.Width - inventoryPnl.Width) / 2;
                        inventoryPnl.Top = (currentForm.ClientSize.Height - inventoryPnl.Height) / 2 + 44;

                        inventoryPnl.Visible = true;
                        NavbarManager.SwitchTo("Inventory");
                        currentForm.Text = "Inventory";
                    }
                    break;
                default:
                    throw new ArgumentException("Invalid panel name.");
            }
        }
    }
}
