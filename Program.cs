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
            screenBMP = new Bitmap(emuRect.Right - emuRect.Left + 1, emuRect.Bottom - emuRect.Top + 1);
            g = Graphics.FromImage(screenBMP);
            g.CopyFromScreen(emuRect.Left, emuRect.Top, 0, 0, s);
        }

        [STAThread]
        static void Main( )
        {
            String EmulatorProcessName = "Jnes";
            Process[] processes = Process.GetProcessesByName(EmulatorProcessName);
            Process proc = processes[0];
            IntPtr ptr = proc.MainWindowHandle;
            emuRect = new Rect();
            GetWindowRect(ptr, ref emuRect);
            s = new Size(emuRect.Right-emuRect.Left + 1, emuRect.Bottom - emuRect.Top + 1);
            SetForegroundWindow(proc.MainWindowHandle); // ! DOES NOT WORK IF EMULATOR HAS BEEN MINIMIZED !

            Form tc = new testcontrols();
            tc.Visible = true;
            Application.EnableVisualStyles( );
            Application.SetCompatibleTextRenderingDefault( false );
            Application.Run( new MainForm( ) );

        }
    }
}
