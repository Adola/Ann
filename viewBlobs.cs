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

    // this class is just used to show the sprites that were found.
    public partial class viewBlobs : Form
    {

        Bitmap bp;
        Graphics c, g;
        System.Timers.Timer t;
        List<stripv2> mariostrips;
        Bitmap mario;
        Color mariobg;
        gameObject mariogo;
        public static string fps = "";
        public static IntPtr vbWindow;
        public static bool foundMario = false;
        public static bool first = true;
        public Rectangle rect;
        public Rectangle rectCopy;
        public viewBlobs()
        {
            Program.usingpic = false;
            InitializeComponent();
            Program.updateScreenshot();
            bp = new Bitmap(Program.screenBMP.Width, Program.screenBMP.Height);

            //mario = (Bitmap)Player.Properties.Resources.mariosmall.Clone();
            //mariogo = new gameObject(Player.Properties.Resources.mariosmall, Player.Properties.Resources.mariosmall.GetPixel(0, 0));
            vbWindow = this.Handle;
            t = new System.Timers.Timer { Enabled = true, Interval = 1};
            t.Elapsed += delegate { updateBlobImage(); };
        }

        private bool safeRect(Rectangle r)
        {
            return (r.Left >= 0 && r.Top >= 0 && (r.Left + r.Width) < Program.screenBMP.Width && (r.Top + r.Height) < Program.screenBMP.Height);
        }

        private void pictureBox1_Paint(object sender, System.Windows.Forms.PaintEventArgs e) // search for mario and draw a box around him
        {
             /*
             * 1) acess tracking's queue of stuff that's been found 
             * 2) just draw each sprite there to the screen
             */

            
            // !!! All code below will be removed/moved to the tracking class, this is here to save my test data !!!
            /*
            g = e.Graphics;
            Program.updateScreenshot();
            Stopwatch sw = new Stopwatch(); sw.Start();
            // time the speed of running the new stripblob algorithm
            List<blobv2> blobs = stripBlob.getblobsRect(Program.screenBMP, new Rectangle(0, 0, Program.screenBMP.Width, Program.screenBMP.Height));
            sw.Stop(); Console.Out.WriteLine("took " + sw.ElapsedMilliseconds);

            
            if (first)
            {
                rect = mariogo.keyblobInrect(Program.screenBMP, new Rectangle(0, 0, Program.screenBMP.Width, Program.screenBMP.Height));
                if (rect.Left != -1) first = false;
            }
            else if (safeRect(rect))
            {
                rectCopy = new Rectangle(rect.Left, rect.Top, rect.Width, rect.Height); // gives place on the screen
                rect = mariogo.keyblobInrect(Program.screenBMP, rect); // place inside the small searched rectangle

                if (rect.Left == -1) first = true; // didn't find it look in the full screen again 
                else rect = new Rectangle(rectCopy.Left, rectCopy.Top, rectCopy.Width, rectCopy.Height); // found it, the original rectangle is fine
            }
            else
                first = true;

            sw.Stop(); //changeFPS("" + (1000.0 / sw.ElapsedMilliseconds));
            Console.Out.WriteLine( (1000 * sw.ElapsedTicks) + " " + sw.ElapsedMilliseconds);

            
            g.DrawImage(Program.screenBMP, 0, 0);
            if(safeRect(rect)) g.DrawRectangle(new Pen(Color.Violet, 3), rect);
             */
        }

        public void updateBlobImage()
        {
                pictureBox1.Invalidate(); 
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
