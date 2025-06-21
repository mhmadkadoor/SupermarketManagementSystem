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
                var nextForm = formInstances[formKey];
                nextForm.Location = currentForm.Location;
                nextForm.Size = currentForm.Size;
                Application.DoEvents();
                currentForm.Hide();

                // Show as modal dialog and ensure it's in front and focused
                nextForm.Shown += (s, e) =>
                {
                    nextForm.BringToFront();
                    nextForm.Activate();
                };
                nextForm.ShowDialog(currentForm);
                
                // After the modal dialog closes, show the current form again
                currentForm.Show();
                
            }
        }



    }
}
