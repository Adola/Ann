using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Player
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 

        static public Bitmap screenBMP;
        static public Graphics g;
        static public Size s;
        static public Rect emuRect;
        static String EmulatorProcessName;
        static Process[] processes;
        static IntPtr ptr;
        static Process proc;


        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string strClassName, string strWindowName);
        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);

        public struct Rect
        {
            public int Left { get; set; }
            public int Top { get; set; }
            public int Right { get; set; }
            public int Bottom { get; set; }
        }

        public static void updateScreenshot()
        {
            //setActiveScreen();
            //Console.Out.WriteLine(emuRect.Left + " "  + emuRect.Right + " "  + emuRect.Top + " "  + emuRect.Bottom + " ");
            screenBMP = new Bitmap(emuRect.Right - emuRect.Left + 1, emuRect.Bottom - emuRect.Top + 1);
            g = Graphics.FromImage(screenBMP);
            s = new Size(emuRect.Right - emuRect.Left + 1, emuRect.Bottom - emuRect.Top + 1);
            s.Width -= 9; s.Height -= (6+14);
            g.CopyFromScreen(emuRect.Left +8, emuRect.Top +30, 0, 0, s);
        }

        public static void setActiveScreen()
        {
            SetForegroundWindow(proc.MainWindowHandle); // ! DOES NOT WORK IF EMULATOR HAS BEEN MINIMIZED !
            GetWindowRect(ptr, ref emuRect);
        }

        [STAThread]
        static void Main( )
        {
            // initialize some variables
            
            EmulatorProcessName = "Jnes";
            processes = Process.GetProcessesByName(EmulatorProcessName);
            proc = processes[0];
            ptr = proc.MainWindowHandle;
            emuRect = new Rect();
            GetWindowRect(ptr, ref emuRect);
            s = new Size(emuRect.Right-emuRect.Left + 1, emuRect.Bottom - emuRect.Top + 1);
            // set the active screen to the emulator
            setActiveScreen();

            Application.EnableVisualStyles( );
            Application.SetCompatibleTextRenderingDefault( false );

            //Form tc = new testcontrols(); tc.Visible = true;
            //Form vb = new viewBlobs(); vb.Visible = true;

            Application.Run( new MainForm( ) );
        }
    }
}
