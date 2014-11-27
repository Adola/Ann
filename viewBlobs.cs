using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Player
{

    // this class is just used to show the sprites that were found.
    public partial class viewBlobs : Form
    {
        Graphics g; bool lockg;
        public System.Timers.Timer track;
        public static string fps = "";
        public static Stopwatch countUntilRunTracking;
        public static Stopwatch countUntilRunTrackingFound;

        public viewBlobs()
        {
            InitializeComponent();
            countUntilRunTracking = new Stopwatch(); countUntilRunTracking.Start();
            countUntilRunTrackingFound = new Stopwatch(); countUntilRunTrackingFound.Start();

        }
        
        public void repaint()
        {
            this.pictureBox1.Invalidate();
        }

        private void pictureBox1_Paint(object sender, System.Windows.Forms.PaintEventArgs e) // search for mario and draw a box around him
        {
            while (lockg) ; lockg = true;

            try
            {
                g = e.Graphics;
                Bitmap screen = Program.updateScreenshot();
                g.DrawImage(screen, 0, 0);
                foreach (gameObject go in tracking.gameObjects)
                {
                    g.DrawRectangle(new Pen(Color.Violet, 3), tracking.getSpriteRectangle(go));
                }
            }
            catch (System.InvalidOperationException ioe)
            {
                threadController.skipNextRun1 = true;
                Console.Out.WriteLine("skipNextRuned thread ONE");
            }

            lockg = false;
        }
        private void viewBlobs_Load(object sender, EventArgs e)
        {
            pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            this.Controls.Add(pictureBox1);
        }

        // !!! Code below is not used !!!














        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        
        public void AppendTextBox(string value)
        {
            if (textBox1.InvokeRequired)
            {
                textBox1.Invoke(new Action<string>(AppendTextBox), new object[] { value });
                return;
            }
            textBox1.Text = fps;
         
        }

        public void changeFPS(string newfps) 
        {
            fps = newfps;
            AppendTextBox(fps);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }
        

    }
}
