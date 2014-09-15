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
        public bool fuck;
        [DllImport("user32.dll", SetLastError = true)]
        public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
        public const int KEYEVENTF_KEYDOWN = 0x0001; //Key down flag
        public const int KEYEVENTF_KEYUP = 0x0002; //Key up flag
        /* virtual key codes
                             VK_LBUTTON 	01 	Left mouse button
                    VK_RBUTTON 	02 	Right mouse button
                    VK_CANCEL 	03 	Control-break processing
                    VK_MBUTTON 	04 	Middle mouse button (three-button mouse)
                    VK_BACK 	08 	BACKSPACE key
                    VK_TAB 	09 	TAB key
                    VK_CLEAR 	0C 	CLEAR key
                    VK_RETURN 	0D 	ENTER key
                    VK_SHIFT 	10 	SHIFT key
                    VK_CONTROL 	11 	CTRL key
                    VK_MENU 	12 	ALT key
                    VK_PAUSE 	13 	PAUSE key
                    VK_CAPITAL 	14 	CAPS LOCK key
                    VK_ESCAPE 	1B 	ESC key
                    VK_SPACE 	20 	SPACEBAR
                    VK_PRIOR 	21 	PAGE UP key
                    VK_NEXT 	22 	PAGE DOWN key
                    VK_END 	23 	END key
                    VK_HOME 	24 	HOME key
                    VK_LEFT 	25 	LEFT ARROW key
                    VK_UP 	26 	UP ARROW key
                    VK_RIGHT 	27 	RIGHT ARROW key
                    VK_DOWN 	28 	DOWN ARROW key
                    VK_SELECT 	29 	SELECT key
                    VK_PRINT 	2A 	PRINT key
                    VK_EXECUTE 	2B 	EXECUTE key
                    VK_SNAPSHOT 	2C 	PRINT SCREEN key
                    VK_INSERT 	2D 	INS key
                    VK_DELETE 	2E 	DEL key
                    VK_HELP 	2F 	HELP key
	                    30 	0 key
	                    31 	1 key
	                    32 	2 key
	                    33 	3 key
	                    34 	4 key
	                    35 	5 key
	                    36 	6 key
	                    37 	7 key
	                    38 	8 key
	                    39 	9 key
	                    41 	A key
	                    42 	B key
	                    43 	C key
	                    44 	D key
	                    45 	E key
	                    46 	F key
	                    47 	G key
	                    48 	H key
	                    49 	I key
	                    4A 	J key
	                    4B 	K key
	                    4C 	L key
	                    4D 	M key
	                    4E 	N key
	                    4F 	O key
	                    50 	P key
	                    51 	Q key
	                    52 	R key
	                    53 	S key
	                    54 	T key
	                    55 	U key
	                    56 	V key
	                    57 	W key
	                    58 	X key
	                    59 	Y key
	                    5A 	Z key
         
         */

        public Controls()
        {
           
        }

        public void keydown(byte VK)
        {
            keybd_event(VK, 0, KEYEVENTF_KEYUP, 0);
            fuck = false;
        }

        public void keyup(byte VK)
        {
            keybd_event(VK, 0, KEYEVENTF_KEYDOWN, 0);
        }
        
        public void sendAKey(byte VK, int duration)
        {
            
         Timer myTimer = new System.Timers.Timer();
            var t = new Timer { Enabled = true, Interval = duration};
            t.Elapsed += delegate { keydown(VK); };
            keyup(VK);
            t.Start();

        }
    }
}
