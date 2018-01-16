using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniQueue
{
    

    public partial class SettingsDialog : Form
    {
        MiniQueueWindow q;
        bool unsavedSettings = false;

        public SettingsDialog(MiniQueueWindow queueWindow)
        {
            InitializeComponent();
            q = queueWindow;
        }

        private void SettingsDialog_Load(object sender, EventArgs e)
        {
            syncSettingsDialog();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            if (unsavedSettings)
            {
                var confirmExitWithoutSaving = MessageBox.Show("You have unsaved settings. Are you sure you wish to exit?", "Unsaved Settings", MessageBoxButtons.YesNo);
                
                if (confirmExitWithoutSaving == DialogResult.Yes)
                {
                    this.Close();
                    unsavedSettings = false;
                }
            }

            else { this.Close(); }
            
        }

        private void applyButton_Click(object sender, EventArgs e)
        {
            saveSettings();

        }

        private void saveSettings()
        {
            //Update settings in the settings file
            Properties.Settings.Default.SizeMode = this.sizeSettingComboBox.Text;
            Properties.Settings.Default.TextColor = colorDialog1.Color;
            Properties.Settings.Default.BackColor = colorDialog2.Color;
            Properties.Settings.Default.ErrorColor = colorDialog3.Color;
            Properties.Settings.Default.UpdateInterval = int.Parse(updateIntervalComboBox.SelectedItem.ToString());
            Properties.Settings.Default.HideTitleBar = hideTitleBarCheckBox.Checked;

            //save user settings
            Properties.Settings.Default.Save();

            //live update queue window
            q.updateAndPropegateSettings();

            //reset tracking bool
            unsavedSettings = false;

        }

        private void okButton_Click(object sender, EventArgs e)
        {
            //OK = save and exit
            saveSettings();
            this.Close();
        }

        private void textColorPickerButton_Click(object sender, EventArgs e)
        {
            //basic color picker - allow the user to select text color
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                textColorPickerButton.BackColor = colorDialog1.Color;

                if(Properties.Settings.Default.TextColor != colorDialog1.Color) { unsavedSettings = true; }
            }
        }

        private void sizeSettingComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(Properties.Settings.Default.SizeMode != sizeSettingComboBox.Text) { unsavedSettings = true; }
        }

        private void bgColorPickerButton_Click(object sender, EventArgs e)
        {
            //basic color picker - allow the user to select background color
            if (colorDialog2.ShowDialog() == DialogResult.OK)
            {
                bgColorPickerButton.BackColor = colorDialog2.Color;

                if (Properties.Settings.Default.BackColor != colorDialog2.Color) { unsavedSettings = true; }
            }
        }

        private void errorColorPickerButton_Click(object sender, EventArgs e)
        {
            //basic color picker - allow the user to select error color
            if (colorDialog3.ShowDialog() == DialogResult.OK)
            {
                errorColorPickerButton.BackColor = colorDialog3.Color;

                if (Properties.Settings.Default.ErrorColor != colorDialog3.Color) { unsavedSettings = true; }
            }
        }

        private void updateIntervalComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (int.Parse(updateIntervalComboBox.SelectedItem.ToString()) != Properties.Settings.Default.UpdateInterval) { unsavedSettings = true; }
        }

        private void restoreDefaultsButton_Click(object sender, EventArgs e)
        {
            var confirmOverwrite = MessageBox.Show("This will overwrite all custom settings currently in place", "Restore Defaults?", MessageBoxButtons.YesNo);

            if (confirmOverwrite == DialogResult.Yes)
            {
                Properties.Settings.Default.Reset();
                syncSettingsDialog();
                q.updateAndPropegateSettings();
            }
            
        }

        private void syncSettingsDialog()
        {
            for (int x = 0; x < sizeSettingComboBox.Items.Count; x++)
            {
                if (string.Equals(sizeSettingComboBox.GetItemText(sizeSettingComboBox.Items[x]), Properties.Settings.Default.SizeMode, StringComparison.OrdinalIgnoreCase))
                { sizeSettingComboBox.SelectedIndex = x; }
            }

            for (int x = 0; x < updateIntervalComboBox.Items.Count; x++)
            {
                if (int.Parse(updateIntervalComboBox.Items[x].ToString()) == Properties.Settings.Default.UpdateInterval)
                { updateIntervalComboBox.SelectedIndex = x; }
            }

            textColorPickerButton.BackColor = Properties.Settings.Default.TextColor;
            bgColorPickerButton.BackColor = Properties.Settings.Default.BackColor;
            errorColorPickerButton.BackColor = Properties.Settings.Default.ErrorColor;
            colorDialog1.Color = Properties.Settings.Default.TextColor;
            colorDialog2.Color = Properties.Settings.Default.BackColor;
            colorDialog3.Color = Properties.Settings.Default.ErrorColor;
            hideTitleBarCheckBox.Checked = Properties.Settings.Default.HideTitleBar;
        }
    }
}
