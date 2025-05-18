using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SupermarketManagementSystem
{
    internal class FormManager
    {
        private static Dictionary<string, Form> formInstances = new Dictionary<string, Form>();

        // Define an event for tracking preload progress
        public static event Action<string, int, int> OnFormPreloaded;

        public static async Task PreloadFormsAsync()
        {
            string[] formKeys = {"DashboardForm", "LoginForm"};
            int totalForms = formKeys.Length;
            int loadedForms = 0;

            foreach (var formKey in formKeys)
            {
                Application.DoEvents(); // Process UI events to keep the application responsive
                // 🚨 Check if the form already exists
                if (!formInstances.ContainsKey(formKey) || formInstances[formKey].IsDisposed)
                {
                    await Task.Delay(50); // Delay to prevent UI lag

                    switch (formKey)
                    {
                        case "DashboardForm": formInstances[formKey] = new DashboardForm(); break;
                        case "LoginForm": formInstances[formKey] = new LoginForm(); break;

                    }

                    formInstances[formKey].StartPosition = FormStartPosition.Manual;
                    formInstances[formKey].Hide();

                    loadedForms++;
                    OnFormPreloaded?.Invoke(formKey, loadedForms, totalForms);
                }
                else
                {
                    Console.WriteLine($"Skipped: {formKey} is already preloaded.");
                }
            }
        }
        public static void PreloadForm(string formKey)
        {
            // Check if form already exists or is disposed
            if (!formInstances.ContainsKey(formKey) || formInstances[formKey].IsDisposed)
            {
                Console.WriteLine($"Preloading {formKey}...");

                switch (formKey)
                {
                    case "DashboardForm": formInstances[formKey] = new DashboardForm(); break;
                    case "LoginForm": formInstances[formKey] = new LoginForm(); break;

                }

                formInstances[formKey].StartPosition = FormStartPosition.Manual;
                formInstances[formKey].Hide();

                Console.WriteLine($"{formKey} preloaded successfully!");
            }
            else
            {
                Console.WriteLine($"{formKey} is already preloaded.");
            }
        }

        public static void ShowForm(Form currentForm, string formKey)
        {
            if (formInstances.ContainsKey(formKey) && !formInstances[formKey].IsDisposed)
            {
                formInstances[formKey].Location = currentForm.Location;
                formInstances[formKey].Size = currentForm.Size;
                Application.DoEvents();
                formInstances[formKey].Show();
                currentForm.Hide();
            }
        }



    }
}
