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
                Text = "www.google.com", 
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
                Value = 1, 
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

            Button saveButton = new Button { 
                Text = "Save",
                Left = 100,
                Top = 140,
                Width = 80,
                FlatStyle = FlatStyle.Flat
            };
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
    }
}
