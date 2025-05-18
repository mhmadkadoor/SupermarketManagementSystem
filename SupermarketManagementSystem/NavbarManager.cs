using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SupermarketManagementSystem
{
    internal static class NavbarManager
    {
        private static Dictionary<string, Label> labelInstances = new Dictionary<string, Label>();

        private static Label dashboardNavLbl;
        private static Label posNavLbl;
        private static Label inventoryNavLbl;
        private static Label reportsNavLbl;
        private static Label profileNavLbl;
        private static Label logoutNavLbl;

        public static void InitializeLabels(
            Label dashboard,
            Label pos,
            Label inventory,
            Label reports,
            Label profile,
            Label logout)
        {
            labelInstances.Clear();

            dashboardNavLbl = dashboard;
            posNavLbl = pos;
            inventoryNavLbl = inventory;
            reportsNavLbl = reports;
            profileNavLbl = profile;
            logoutNavLbl = logout;

            labelInstances["Dashboard"] = dashboardNavLbl;
            labelInstances["POS"] = posNavLbl;
            labelInstances["Inventory"] = inventoryNavLbl;
            labelInstances["Reports"] = reportsNavLbl;
            labelInstances["Profile"] = profileNavLbl;
            labelInstances["Logout"] = logoutNavLbl;
        }

        public static void SwitchTo(string selectedKey)
        {
            // Reset all labels to default color
            foreach (var label in labelInstances.Values)
            {
                label.ForeColor = Color.White;
            }

            // Highlight the selected label
            if (labelInstances.TryGetValue(selectedKey, out var selectedLabel))
            {
                selectedLabel.ForeColor = Color.Brown;
            }
            else
            {
                throw new KeyNotFoundException($"Label control '{selectedKey}' not found.");
            }
        }
    }
}
