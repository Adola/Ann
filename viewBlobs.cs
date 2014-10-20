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
        gameObject mariogo;
        public static string fps = "";
        public static IntPtr vbWindow;
        public static bool updating;
        public bool first;
        public Rectangle rect;
        public viewBlobs()
        {
            Program.usingpic = false;
            InitializeComponent();
            Program.updateScreenshot();
            bp = new Bitmap(Program.screenBMP.Width, Program.screenBMP.Height);

            //mario = (Bitmap)Player.Properties.Resources.mariosmall.Clone();
            mariogo = new gameObject(Player.Properties.Resources.mariosmall, Player.Properties.Resources.mariosmall.GetPixel(0, 0));
            vbWindow = this.Handle;
            updating = false;
            first = true;
            t = new System.Timers.Timer { Enabled = true, Interval = 1};
            t.Elapsed += delegate { updateBlobImage(); };
        }

        private void pictureBox1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            g = e.Graphics;
            Program.updateScreenshot();
            //g.DrawImage(Program.screenBMP, 0, 0);
            //Stopwatch sw = new Stopwatch(); sw.Start();
            Rectangle rect = mariogo.keyblobInrect(Program.screenBMP, new Rectangle(0, 0, Program.screenBMP.Width, Program.screenBMP.Height));
            //sw.Stop(); changeFPS("" + (1000.0 / sw.ElapsedMilliseconds));
            g.DrawImage(Program.screenBMP, 0, 0);
            g.DrawRectangle(new Pen(Color.Orange, 3), rect);
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
        

    }
}
