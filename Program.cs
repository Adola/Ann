using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [STAThread]
        static void Main( )
        {
            // set Jnes as the foreground and focused on window. 
            var prc = Process.GetProcessesByName("Jnes");
            if (prc.Length > 0)
                SetForegroundWindow(prc[0].MainWindowHandle);

            
            Application.EnableVisualStyles( );
            Application.SetCompatibleTextRenderingDefault( false );
            Application.Run( new MainForm( ) );
            
        }
    }
}
