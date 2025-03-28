using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace TrayPing
{
    class PreferencesForm : Form 
    {
        private TextBox targetHostTextBox;
        private NumericUpDown pingFrequencyBox;
        private CheckBox autolaunchCheckBox;
        
        public PreferencesForm()
        {
            Text = "Preferences";
            Size = new System.Drawing.Size(300, 220);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MinimizeBox = false;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;

            Label targetHostLabel = new Label {
                Text = "Target Host", 
                Left = 10,
                Top = 20,
                Width = 100,
                FlatStyle = FlatStyle.Flat
            };
            targetHostTextBox = new TextBox { 
                Text = Properties.Settings.Default.TargetHost, 
                Left = 120, 
                Top = 20, 
                Width = 150, 
                BorderStyle = BorderStyle.FixedSingle,
            };

            Label pingFrequencyLabel = new Label {
                Text = "Ping frequency", 
                Left = 10, 
                Top = 60, 
                Width = 100,
                FlatStyle = FlatStyle.Flat
            };
            pingFrequencyBox = new NumericUpDown { 
                Value = Properties.Settings.Default.PingFrequency, 
                Minimum = 1, 
                Left = 120, 
                Top = 60, 
                Width = 150,
                BorderStyle = BorderStyle.FixedSingle,
            };

            autolaunchCheckBox = new CheckBox
            {
                Text = "Run on system startup",
                Left = 10,
                Top = 100,
                Width = 200,
                TextAlign = ContentAlignment.MiddleLeft,
                AutoSize = true,
                FlatStyle = FlatStyle.Flat
            };
            autolaunchCheckBox.Checked = Autolaunch.IsEnabled();

            Button saveButton = new Button { 
                Text = "Save",
                Left = 100,
                Top = 140,
                Width = 80,
                FlatStyle = FlatStyle.Flat
            };
            saveButton.Click += SavePreferences;

            Button cancelButton = new Button { 
                Text = "Cancel", 
                Left = 190, 
                Top = 140, 
                Width = 80, 
                FlatStyle = FlatStyle.Flat
            };
            cancelButton.Click += (s, e) => this.Close();

            Controls.Add(targetHostLabel);
            Controls.Add(targetHostTextBox);
            Controls.Add(pingFrequencyLabel);
            Controls.Add(pingFrequencyBox);
            Controls.Add(autolaunchCheckBox);
            Controls.Add(saveButton);
            Controls.Add(cancelButton);
        }

        private void SavePreferences(object sender, EventArgs e)
        { 
              
            if (!string.IsNullOrEmpty(targetHostTextBox.Text)) 
            {
                Properties.Settings.Default.TargetHost = targetHostTextBox.Text;
                Properties.Settings.Default.PingFrequency = (int)pingFrequencyBox.Value;

                if (autolaunchCheckBox.Checked)
                    Autolaunch.Toggle(true);
                else 
                    Autolaunch.Toggle(false);

                Properties.Settings.Default.Save();
                this.Close();
            }
            else 
            {
                MessageBox.Show(
                    "Target host cannot be none", 
                    "Error saving preferences", 
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}
