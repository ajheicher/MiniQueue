namespace MiniQueue
{
    partial class MiniQueueWindow
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
            this.label2 = new System.Windows.Forms.Label();
            this.contactWaitingValue = new System.Windows.Forms.TextBox();
            this.longestWaitingValue = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Contacts Waiting";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(165, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Longest Waiting";
            // 
            // contactWaitingValue
            // 
            this.contactWaitingValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.contactWaitingValue.Location = new System.Drawing.Point(15, 33);
            this.contactWaitingValue.Name = "contactWaitingValue";
            this.contactWaitingValue.ReadOnly = true;
            this.contactWaitingValue.Size = new System.Drawing.Size(85, 80);
            this.contactWaitingValue.TabIndex = 2;
            this.contactWaitingValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // longestWaitingValue
            // 
            this.longestWaitingValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.longestWaitingValue.Location = new System.Drawing.Point(106, 33);
            this.longestWaitingValue.Name = "longestWaitingValue";
            this.longestWaitingValue.ReadOnly = true;
            this.longestWaitingValue.Size = new System.Drawing.Size(205, 80);
            this.longestWaitingValue.TabIndex = 3;
            this.longestWaitingValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // MiniQueueWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(323, 125);
            this.Controls.Add(this.longestWaitingValue);
            this.Controls.Add(this.contactWaitingValue);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "MiniQueueWindow";
            this.Text = "Mini Queue";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.MiniQueueWindow_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox contactWaitingValue;
        private System.Windows.Forms.TextBox longestWaitingValue;
    }
}