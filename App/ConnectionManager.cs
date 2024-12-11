using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System;

namespace XpandNPIManager.Helpers
{
    public class ConnectionManager
    {
        [DllImport("mpr.dll")]
        private static extern int WNetAddConnection2(NETRESOURCE netResource, string password, string username, int flags);

        [DllImport("mpr.dll")]
        private static extern int WNetCancelConnection2(string name, int flags, bool force);

        [StructLayout(LayoutKind.Sequential)]
        public class NETRESOURCE
        {
            public int Scope;
            public int ResourceType;
            public int DisplayType;
            public int Usage;
            public string LocalName;
            public string RemoteName;
            public string Comment;
            public string Provider;
        }

        /// <summary>
        /// Connects to a network path with the provided credentials.
        /// </summary>
        public static bool ConnectToNetwork(string networkPath, string username, string password)
        {
            using (var progressForm = new ProgressForm("Attempting to connect..."))
            {
                // Show the progress form
                progressForm.Show();
                progressForm.Refresh();

                try
                {
                    var netResource = new NETRESOURCE
                    {
                        Scope = 2,
                        ResourceType = 1,
                        DisplayType = 3,
                        Usage = 1,
                        RemoteName = networkPath
                    };

                    // Perform the connection
                    int result = WNetAddConnection2(netResource, password, username, 0);

                    if (result != 0)
                    {
                        Console.WriteLine($"Failed to connect. Error code: {result}");
                        return false;
                    }

                    return true;
                }
                finally
                {
                    // Ensure the progress form is closed
                    progressForm.Close();
                }
            }
        }


        /// <summary>
        /// Prompts the user for network credentials using a simple form.
        /// </summary>
        public static NetworkCredential PromptForCredentials()
        {
            using (var form = new Form())
            {
                form.Text = "Enter Network Credentials";
                form.Width = 400; // Increased width for better visibility
                form.Height = 200;
                form.StartPosition = FormStartPosition.CenterScreen;

                var lblUsername = new Label { Text = "Username:", Top = 20, Left = 20, Width = 80 };
                var txtUsername = new TextBox { Top = 20, Left = 120, Width = 200 };

                var lblPassword = new Label { Text = "Password:", Top = 60, Left = 20, Width = 80 };
                var txtPassword = new TextBox { Top = 60, Left = 120, Width = 200, UseSystemPasswordChar = true };

                var btnSubmit = new Button { Text = "Submit", Top = 100, Left = 120, Width = 80 };
                btnSubmit.DialogResult = DialogResult.OK;

                form.Controls.Add(lblUsername);
                form.Controls.Add(txtUsername);
                form.Controls.Add(lblPassword);
                form.Controls.Add(txtPassword);
                form.Controls.Add(btnSubmit);

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

        /// <summary>
        /// Tests if a network path is accessible.
        /// </summary>
        public static bool TestConnection(string networkPath)
        {
            try
            {
                return Directory.Exists(networkPath);
            }
            catch
            {
                return false; // Return false if an exception occurs
            }
        }

        /// <summary>
        /// Disconnects from the specified network path.
        /// </summary>
        public static void DisconnectNetwork(string networkPath)
        {
            try
            {
                WNetCancelConnection2(networkPath, 0, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to disconnect from network: {ex.Message}");
            }
        }
    }
}