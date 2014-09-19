using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Timers;

namespace Player
{
    class Controls
    {
        public String cName = ""; public String wName = "";
        [DllImport("user32.dll", SetLastError = true)]
        public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
        public const int KEYEVENTF_KEYDOWN = 0x0001; //Key down flag
        public const int KEYEVENTF_KEYUP = 0x0002; //Key up flag
        public Controls()
        {
           
        }

        public void keydown(byte VK)
        {
            keybd_event(VK, 0, KEYEVENTF_KEYDOWN, 0);
        }

        public void keyup(byte VK, System.Timers.Timer t)
        {
            keybd_event(VK, 0, KEYEVENTF_KEYUP, 0);
            t.Dispose();
        }
        public void useKey(byte VK, int duration) 
        {
            var t = new System.Timers.Timer { Enabled = true, Interval = duration  };
            t.Elapsed += delegate { keyup(VK, t); }; t.AutoReset = false;
            keydown(VK);
        }
    }
}
