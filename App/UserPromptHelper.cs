using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XpandNPIManager.Helpers
{
    public static class UserPromptHelper
    {
        public static NetworkCredential PromptForCredentials()
        {
            using (var form = new Form())
            {
                form.Text = "Enter Network Credentials";
                form.Width = 400; // Increased width for better visibility
                form.Height = 200; // Adjusted height
                form.StartPosition = FormStartPosition.CenterScreen; // Center the form

                // Labels and TextBoxes
                var lblUsername = new Label { Text = "Username:", Top = 20, Left = 20, Width = 80 };
                var txtUsername = new TextBox { Top = 20, Left = 120, Width = 200 };

                var lblPassword = new Label { Text = "Password:", Top = 60, Left = 20, Width = 80 };
                var txtPassword = new TextBox { Top = 60, Left = 120, Width = 200, UseSystemPasswordChar = true };

                // Submit button
                var btnSubmit = new Button { Text = "Submit", Top = 100, Left = 120, Width = 80 };
                btnSubmit.DialogResult = DialogResult.OK;

                // Add controls to the form
                form.Controls.Add(lblUsername);
                form.Controls.Add(txtUsername);
                form.Controls.Add(lblPassword);
                form.Controls.Add(txtPassword);
                form.Controls.Add(btnSubmit);

                // Show the form and handle result
                if (form.ShowDialog() == DialogResult.OK)
                {
                    if (!string.IsNullOrWhiteSpace(txtUsername.Text) && !string.IsNullOrWhiteSpace(txtPassword.Text))
                    {
                        return new NetworkCredential(txtUsername.Text, txtPassword.Text);
                    }
                }
            }

            return null;
        }

    }
}