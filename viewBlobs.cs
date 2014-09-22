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
        bool usingpic;
        Stopwatch sw;
        Bitmap bp;

        public viewBlobs()
        {
            usingpic = false;
            InitializeComponent();
            Program.updateScreenshot();
            bp = new Bitmap(Program.screenBMP.Width, Program.screenBMP.Height);

            sw = new Stopwatch();
            var t = new System.Timers.Timer { Enabled = true, Interval = 2000 };
            t.Elapsed += delegate { updateBlobImage(); };
        }

        private void pictureBox1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
                Graphics g = e.Graphics;
                g.DrawImage(bp, 0, 0);
                this.pictureBox1.Update();
        }

        public void updateBlobImage()
        {
            sw.Reset(); sw.Start();
            Program.updateScreenshot();
            List<strip> strips = strip.getstrips(Program.screenBMP);
            //List<strip> strips = strip.getstrips(new Bitmap("C:\\Users\\Chris\\Downloads\\jnes_1_1_1\\screenshots\\mario.bmp"));
            //List<List<strip>> allblobs = blobs.getblobsinit(strips);
            bp = new Bitmap(Program.screenBMP.Width, Program.screenBMP.Height);
            foreach (strip s in strips)
            {
                strip.printstrip(s, bp);
            }
            sw.Stop();
            Console.Out.WriteLine("time used to get strips" + sw.ElapsedMilliseconds);
            this.pictureBox1.Invalidate();
            
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
    }
}
