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
        Bitmap bp;
        Graphics c, g;
        System.Timers.Timer t;
        List<stripv2> mariostrips;
        Bitmap mario;
        Color mariobg;
        public static string fps = "";

        public viewBlobs()
        {
            Program.usingpic = false;
            InitializeComponent();
            Program.updateScreenshot();
            bp = new Bitmap(Program.screenBMP.Width, Program.screenBMP.Height);

            // this example will show the program finding mario on screen to do this we must know all of marios blobs
            // a picture of a small mario facing right is included in resources
            // the light blue bits of the background have to be removed here. I don't know a good fix for this yet.
            // searching for mario will happen in updateBlobImage()
            mario = (Bitmap)Player.Properties.Resources.mariosmall.Clone();
            //mario = new Bitmap("C:\\Users\\Chris\\Downloads\\jnes_1_1_1\\screenshots\\mariosmall.bmp");
            mariostrips = stripBlob.getUnconnectedStrips(mario); // we assume we already have used blobs to find a mario, so now we don't need the connected strips. (unconnected algo runs faster)
            mariobg = Color.FromArgb(255, 168, 255, 255);
            /*
            int index = 0;
            while (index < mariostrips.Count)
            {
                if (mariostrips[index].c == mariobg)
                    mariostrips.RemoveAt(index);
                index++;
            }
             */
            t = new System.Timers.Timer { Enabled = true, Interval = 10}; 
            t.Elapsed += delegate { updateBlobImage(); };
        }

        private void pictureBox1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if (!Program.usingpic)
            {
                g = e.Graphics;
                g.ScaleTransform(3, 3);
                g.DrawImage(bp, 0, 0);
            }
        }

        public void updateBlobImage()
        {
            if (!Program.usingpic)
            {

                Program.updateScreenshot();
                while (Program.usingpic) ;
                Program.usingpic = true;

                c = Graphics.FromImage(bp);
                c.Clear(Color.White);

                List<stripv2> screenStrips = stripBlob.getUnconnectedStrips(Program.screenBMP);

                foreach (stripv2 s in screenStrips)
                    bp = stripBlob.printstrip(s, bp);

                /*
                List<stripv2> screenStrips = stripBlob.getUnconnectedStrips(Program.screenBMP);
                int[][] screenStripsHashTable = stripBlob.hashstrips(screenStrips);

                List<stripv2> curStripsOnScreen = new List<stripv2>();
                List<stripv2> screenMario = new List<stripv2>();  // need a list of strips of mario from the screen because we need to know where mario is on screen.
                foreach (stripv2 m in mariostrips)
                {
                    foreach (stripv2 n in stripBlob.stripInSetOfStrips(m, screenStrips, screenStripsHashTable))
                        bp = stripBlob.printstrip(n, bp);
                }
                // draw those strips to the screen
                */     

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
            this.Controls.Add(pictureBox1);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        
        public static void AppendTextBox(string value)
        {
            if (textBox1.InvokeRequired)
            {
                textBox1.Invoke(new Action<string>(AppendTextBox), new object[] { value });
                return;
            }
            textBox1.Text = fps;
        }

        public static void changeFPS(string newfps) 
        {
            fps = newfps;
            AppendTextBox(fps);
        }
        

    }
}
