namespace Player
{
    partial class testcontrols
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
            this.UPbutton = new System.Windows.Forms.Button();
            this.DurationTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // UPbutton
            // 
            this.UPbutton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.UPbutton.Image = global::Player.Properties.Resources.buttonUP;
            this.UPbutton.Location = new System.Drawing.Point(46, 22);
            this.UPbutton.Name = "UPbutton";
            this.UPbutton.Size = new System.Drawing.Size(52, 53);
            this.UPbutton.TabIndex = 0;
            this.UPbutton.UseVisualStyleBackColor = true;
            this.UPbutton.Click += new System.EventHandler(this.button1_Click);
            // 
            // DurationTextBox
            // 
            this.DurationTextBox.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DurationTextBox.Location = new System.Drawing.Point(0, 207);
            this.DurationTextBox.Name = "DurationTextBox";
            this.DurationTextBox.Size = new System.Drawing.Size(122, 26);
            this.DurationTextBox.TabIndex = 1;
            this.DurationTextBox.Text = "1";
            this.DurationTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(-4, 236);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 19);
            this.label1.TabIndex = 2;
            this.label1.Text = "Duration (ms)";
            // 
            // testcontrols
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(313, 262);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DurationTextBox);
            this.Controls.Add(this.UPbutton);
            this.Name = "testcontrols";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.testcontrols_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button UPbutton;
        private System.Windows.Forms.TextBox DurationTextBox;
        private System.Windows.Forms.Label label1;
    }
}