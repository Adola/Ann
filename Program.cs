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

        //static public Bitmap screenBMP;
        static public Graphics g; static public bool lockg;
        static public Size s;
        static public Rect emuRect; static public bool lockemurect;
        static String EmulatorProcessName;
        static Process[] processes;
        public static IntPtr ptr;
        static Process proc;
        public static bool lockpic;
        public static int plusleft, plusright, plustop, plusbottom;
        public static Stopwatch trackTimer;
        public static viewBlobs vb;

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

        public static Bitmap updateScreenshot()
        {
            while (lockpic) ; lockpic = true;          
            Bitmap screen = new Bitmap(256, 224);

            while (lockg) ; lockg = true;              
            g = Graphics.FromImage(screen);

            while (lockemurect) ; lockemurect = true;  
            g.CopyFromScreen(emuRect.Left, emuRect.Top, 0, 0, new Size(256, 224));  // !!! hardcoded to size of NES/SNES game !!! 

            lockpic = false;
            lockemurect = false;
            lockg = false;

            return screen;
        }

        public static void setActiveScreen()
        {
            SetForegroundWindow(proc.MainWindowHandle); // !!! DOES NOT WORK IF EMULATOR HAS BEEN MINIMIZED !!!
            //GetWindowRect(ptr, ref emuRect);
            emuRect.Left += 3; emuRect.Top += 45; // !!! hardcoded to romove the frame around Jnes !!!
        }

        //[STAThread]
        static void Main()
        {
            // SETUP 

            // initialize variables
            
            lockpic = false;
            EmulatorProcessName = "Jnes";
            processes = Process.GetProcessesByName(EmulatorProcessName);
            proc = processes[0];
            ptr = proc.MainWindowHandle;
            emuRect = new Rect();
            GetWindowRect(ptr, ref emuRect);
            //screenBMP = new Bitmap(emuRect.Right - emuRect.Left + 1, emuRect.Bottom - emuRect.Top + 1);

            // set the active screen to the emulator
            setActiveScreen();

            //updateScreenshot();

            //Application.EnableVisualStyles( );
            //Application.SetCompatibleTextRenderingDefault( false );

            //Form tc = new testcontrols(); tc.Visible = true; 

            /*
            lockPicture("main 2");
            UnlockPicture();
            */
            
            
            tracking.loadSprites();

            vb = new viewBlobs();

            threadController.startThreads();

            Application.Run(vb);
            


            /*
            tracking.loadSprites();
            Bitmap bp = new Bitmap(@"C:\Users\Chris\Downloads\jnes_1_1_1\screenshots\snap0014.bmp");
            screenBMP = bp;
            Stopwatch sw = new Stopwatch(); sw.Start();
            tracking.trackEntireScreen();
            sw.Stop();
            Console.Out.WriteLine("took " + sw.ElapsedMilliseconds);
             */

        }

    }
}
