using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XpandNPIManager.Helpers
{
    public class ProgressForm : Form // Ensure this inherits from Form
    {
        private Label lblMessage;

        public ProgressForm(string message)
        {
            Text = "Connecting...";
            Width = 300;
            Height = 120;
            StartPosition = FormStartPosition.CenterScreen;

            lblMessage = new Label
            {
                Text = message,
                Dock = DockStyle.Fill,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            };
            Controls.Add(lblMessage);

            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            ControlBox = false;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ProgressForm
            // 
            this.ClientSize = new System.Drawing.Size(278, 244);
            this.Name = "ProgressForm";
            this.Load += new System.EventHandler(this.ProgressForm_Load);
            this.ResumeLayout(false);

        }

        private void ProgressForm_Load(object sender, EventArgs e)
        {

        }
    }
}