using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Player
{
    public partial class viewBlobs : Form
    {
        Stopwatch sw;
        Bitmap bp;
        Graphics g, c;
        public System.Timers.Timer t;

        public viewBlobs()
        {
            Program.usingpic = false;
            InitializeComponent();
            Program.updateScreenshot();
            bp = new Bitmap(Program.screenBMP.Width, Program.screenBMP.Height);

            t = new System.Timers.Timer { Enabled = true, Interval = int.Parse(textBox1.Text) }; // maximum speed is running every ~300ms on my laptop. Any faster and it causes it to crash. 
            t.Elapsed += delegate { updateBlobImage(); };
        }

        private void pictureBox1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if (!Program.usingpic)
            {
                g = e.Graphics;
                g.DrawImage(bp, 0, 0);
                this.pictureBox1.Update();
            }
        }

        public void updateBlobImage()
        {
            if (!Program.usingpic)
            {
                Program.usingpic = true;

                Program.updateScreenshot();
                //What is a 'strip'? cc: The new version of them are now called stripv2 I'll fix it.  
                // This form also tends to crash a lot. I'll see if I can figure out why tomorrow.

                List<List<stripv2>> strips = blobfinder.getstripsv2(Program.screenBMP);
                c = Graphics.FromImage(bp);
                c.Clear(Color.White);
                foreach (List<stripv2> ls in strips)
                {
                    foreach (stripv2 s in ls)
                        blobfinder.printstrip(s, bp);
                }


                Program.usingpic = false;

                this.pictureBox1.Invalidate();
            }
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void viewBlobs_Load(object sender, EventArgs e)
        {
            pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            // Add the PictureBox control to the Form. 
            this.Controls.Add(pictureBox1);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            t.Interval = int.Parse(textBox1.Text) + 10;
        }
    }
}
