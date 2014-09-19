using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Player
{
    public partial class testcontrols : Form
    {
        private Controls cs;

        public testcontrols()
        {
            InitializeComponent();
            cs = new Controls();
            
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            Program.setActiveScreen();
            cs.useKey((byte)0x57, int.Parse(this.DurationTextBox.Text));
        }

        private void testcontrols_Load(object sender, EventArgs e)
        {

        }

    }
}
