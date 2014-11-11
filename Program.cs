using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Player
{
    class Program
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
        public static IntPtr ptr;
        static Process proc;
        public static bool usingpic;
        public static int plusleft, plusright, plustop, plusbottom;


        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
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
            g = Graphics.FromImage(screenBMP);
            //stripBlob.Getscreen(g, 256, 224, emuRect.Left, emuRect.Top);
            g.CopyFromScreen(emuRect.Left, emuRect.Top, 0, 0, new Size(256, 224));  // !!! hardcoded to size of NES/SNES game !!! 
        }

        public static void setActiveScreen()
        {
            SetForegroundWindow(proc.MainWindowHandle); // !!! DOES NOT WORK IF EMULATOR HAS BEEN MINIMIZED !!!
            //GetWindowRect(ptr, ref emuRect);
            emuRect.Left += 3; emuRect.Top += 45; // !!! hardcoded to romove the frame around Jnes !!!
        }

        [STAThread]
        static void Main()
        {
            // initialize some variables
            
            EmulatorProcessName = "Jnes";
            processes = Process.GetProcessesByName(EmulatorProcessName);
            proc = processes[0];
            ptr = proc.MainWindowHandle;
            emuRect = new Rect();
            GetWindowRect(ptr, ref emuRect);
            screenBMP = new Bitmap(emuRect.Right - emuRect.Left + 1, emuRect.Bottom - emuRect.Top + 1);

            // set the active screen to the emulator
            setActiveScreen();
            updateScreenshot();

            Application.EnableVisualStyles( );
            Application.SetCompatibleTextRenderingDefault( false );

            //Form tc = new testcontrols(); tc.Visible = true;
            Form vb = new viewBlobs(); vb.Visible = true;
             
            Application.Run(new MainForm());
        }
    }
}
