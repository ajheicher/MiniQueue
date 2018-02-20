namespace MiniQueue
{
    partial class SettingsDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.sizeSettingComboBox = new System.Windows.Forms.ComboBox();
            this.applyButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.label3 = new System.Windows.Forms.Label();
            this.textColorPickerButton = new System.Windows.Forms.Button();
            this.restoreDefaultsButton = new System.Windows.Forms.Button();
            this.bgColorPickerButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.colorDialog2 = new System.Windows.Forms.ColorDialog();
            this.errorColorPickerButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.colorDialog3 = new System.Windows.Forms.ColorDialog();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.updateIntervalComboBox = new System.Windows.Forms.ComboBox();
            this.hideTitleBarCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Queue Size";
            // 
            // sizeSettingComboBox
            // 
            this.sizeSettingComboBox.FormattingEnabled = true;
            this.sizeSettingComboBox.Items.AddRange(new object[] {
            "Very Small",
            "Medium"});
            this.sizeSettingComboBox.Location = new System.Drawing.Point(108, 49);
            this.sizeSettingComboBox.Name = "sizeSettingComboBox";
            this.sizeSettingComboBox.Size = new System.Drawing.Size(81, 21);
            this.sizeSettingComboBox.TabIndex = 1;
            this.sizeSettingComboBox.SelectedIndexChanged += new System.EventHandler(this.sizeSettingComboBox_SelectedIndexChanged);
            // 
            // applyButton
            // 
            this.applyButton.Location = new System.Drawing.Point(105, 236);
            this.applyButton.Name = "applyButton";
            this.applyButton.Size = new System.Drawing.Size(75, 23);
            this.applyButton.TabIndex = 2;
            this.applyButton.Text = "Apply";
            this.applyButton.UseVisualStyleBackColor = true;
            this.applyButton.Click += new System.EventHandler(this.applyButton_Click);
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(197, 236);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 3;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(12, 236);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Text Color";
            // 
            // textColorPickerButton
            // 
            this.textColorPickerButton.Location = new System.Drawing.Point(108, 78);
            this.textColorPickerButton.Name = "textColorPickerButton";
            this.textColorPickerButton.Size = new System.Drawing.Size(81, 22);
            this.textColorPickerButton.TabIndex = 7;
            this.textColorPickerButton.UseVisualStyleBackColor = true;
            this.textColorPickerButton.Click += new System.EventHandler(this.textColorPickerButton_Click);
            // 
            // restoreDefaultsButton
            // 
            this.restoreDefaultsButton.Location = new System.Drawing.Point(12, 204);
            this.restoreDefaultsButton.Name = "restoreDefaultsButton";
            this.restoreDefaultsButton.Size = new System.Drawing.Size(168, 22);
            this.restoreDefaultsButton.TabIndex = 8;
            this.restoreDefaultsButton.Text = "Restore System Defaults";
            this.restoreDefaultsButton.UseVisualStyleBackColor = true;
            this.restoreDefaultsButton.Click += new System.EventHandler(this.restoreDefaultsButton_Click);
            // 
            // bgColorPickerButton
            // 
            this.bgColorPickerButton.Location = new System.Drawing.Point(108, 112);
            this.bgColorPickerButton.Name = "bgColorPickerButton";
            this.bgColorPickerButton.Size = new System.Drawing.Size(81, 22);
            this.bgColorPickerButton.TabIndex = 10;
            this.bgColorPickerButton.UseVisualStyleBackColor = true;
            this.bgColorPickerButton.Click += new System.EventHandler(this.bgColorPickerButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 117);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Background Color";
            // 
            // errorColorPickerButton
            // 
            this.errorColorPickerButton.Location = new System.Drawing.Point(108, 146);
            this.errorColorPickerButton.Name = "errorColorPickerButton";
            this.errorColorPickerButton.Size = new System.Drawing.Size(81, 22);
            this.errorColorPickerButton.TabIndex = 12;
            this.errorColorPickerButton.UseVisualStyleBackColor = true;
            this.errorColorPickerButton.Click += new System.EventHandler(this.errorColorPickerButton_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 151);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Error Color";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 23);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Update Interval";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(146, 26);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(49, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Seconds";
            // 
            // updateIntervalComboBox
            // 
            this.updateIntervalComboBox.FormattingEnabled = true;
            this.updateIntervalComboBox.Items.AddRange(new object[] {
            "5",
            "10",
            "15",
            "20",
            "25",
            "30",
            "60",
            "120"});
            this.updateIntervalComboBox.Location = new System.Drawing.Point(108, 23);
            this.updateIntervalComboBox.Name = "updateIntervalComboBox";
            this.updateIntervalComboBox.Size = new System.Drawing.Size(38, 21);
            this.updateIntervalComboBox.TabIndex = 16;
            this.updateIntervalComboBox.SelectedIndexChanged += new System.EventHandler(this.updateIntervalComboBox_SelectedIndexChanged);
            // 
            // hideTitleBarCheckBox
            // 
            this.hideTitleBarCheckBox.AutoSize = true;
            this.hideTitleBarCheckBox.Location = new System.Drawing.Point(12, 181);
            this.hideTitleBarCheckBox.Name = "hideTitleBarCheckBox";
            this.hideTitleBarCheckBox.Size = new System.Drawing.Size(179, 17);
            this.hideTitleBarCheckBox.TabIndex = 18;
            this.hideTitleBarCheckBox.Text = "Hide TitleBar (EXPERIMENTAL)";
            this.hideTitleBarCheckBox.UseVisualStyleBackColor = true;
            // 
            // SettingsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 278);
            this.Controls.Add(this.hideTitleBarCheckBox);
            this.Controls.Add(this.updateIntervalComboBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.errorColorPickerButton);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.bgColorPickerButton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.restoreDefaultsButton);
            this.Controls.Add(this.textColorPickerButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.applyButton);
            this.Controls.Add(this.sizeSettingComboBox);
            this.Controls.Add(this.label1);
            this.Name = "SettingsDialog";
            this.Text = "Settings";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.SettingsDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox sizeSettingComboBox;
        private System.Windows.Forms.Button applyButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button textColorPickerButton;
        private System.Windows.Forms.Button restoreDefaultsButton;
        private System.Windows.Forms.Button bgColorPickerButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ColorDialog colorDialog2;
        private System.Windows.Forms.Button errorColorPickerButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ColorDialog colorDialog3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox updateIntervalComboBox;
        private System.Windows.Forms.CheckBox hideTitleBarCheckBox;
    }
}