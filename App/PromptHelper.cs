using System.Windows.Forms;

namespace XpandNPIManager.Helpers
{
    public static class PromptHelper
    {
        /// <summary>
        /// Prompts the user to select a location for saving a ZIP file.
        /// </summary>
        /// <returns>Returns the selected file path or null if the user cancels the dialog.</returns>
        public static string PromptForOutputZipLocation()
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Title = "Select Output ZIP Location";
                saveFileDialog.Filter = "ZIP files (*.zip)|*.zip";
                saveFileDialog.DefaultExt = "zip";
                saveFileDialog.AddExtension = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    return saveFileDialog.FileName;
                }
            }
            return null;
        }

        /// <summary>
        /// Prompts the user to input a list of part numbers.
        /// </summary>
        /// <returns>Returns the inputted part numbers as a string or null if the user cancels the dialog.</returns>
        public static string PromptForPartNumbers()
        {
            using (Form inputForm = new Form())
            {
                inputForm.Width = 400;
                inputForm.Height = 300;
                inputForm.Text = "Enter Part Numbers (one per line)";

                TextBox textBox = new TextBox
                {
                    Multiline = true,
                    Dock = DockStyle.Fill,
                    ScrollBars = ScrollBars.Vertical
                };
                inputForm.Controls.Add(textBox);

                Button okButton = new Button { Text = "OK", Dock = DockStyle.Bottom };
                okButton.Click += (s, e) => inputForm.DialogResult = DialogResult.OK;
                inputForm.Controls.Add(okButton);

                if (inputForm.ShowDialog() == DialogResult.OK)
                {
                    return textBox.Text;
                }
            }
            return null;
        }
    }
}
