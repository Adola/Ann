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
            this.SuspendLayout();
            // 
            // UPbutton
            // 
            this.UPbutton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.UPbutton.Location = new System.Drawing.Point(52, 27);
            this.UPbutton.Name = "UPbutton";
            this.UPbutton.Size = new System.Drawing.Size(75, 23);
            this.UPbutton.TabIndex = 0;
            this.UPbutton.UseVisualStyleBackColor = true;
            this.UPbutton.Click += new System.EventHandler(this.button1_Click);
            // 
            // testcontrols
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.UPbutton);
            this.Name = "testcontrols";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button UPbutton;
    }
}