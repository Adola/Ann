using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Player
{
    class threadController
    {
        public static bool skipNextRun1 = false, skipNextRun2 = false, skipNextRun3 = false; 


        public static void startThreads()
        {
            System.Timers.Timer t1 = new System.Timers.Timer { Enabled = true, Interval = 66 }; // update at ~15fps
            t1.Elapsed += delegate { t1run(); };

            System.Timers.Timer t2 = new System.Timers.Timer { Enabled = true, Interval = 33 }; // run tracking on every ~2nd frame (I think my laptop is too slow, this leads to problems) Might be able to lower it.
            t2.Elapsed += delegate { t2run();  };

            System.Timers.Timer t3 = new System.Timers.Timer { Enabled = true, Interval = 1000 }; // look for new objects every 1 seconds.
            t3.Elapsed += delegate { t3run();  };

            // A timer for the ANN to press buttons would go here, or run it right after something else.
        }

        public static void t1run()
        {
            if(! skipNextRun1)
            {
                Program.vb.repaint(); 
            }
            skipNextRun1 = false;
        }
        public static void t2run()
        {
            if (!skipNextRun2)
            {
                tracking.trackFoundObjects(); 
            }
            skipNextRun1 = false;
        }
        public static void t3run()
        {
            if (!skipNextRun3)
            {
                tracking.trackFoundObjects();
                tracking.trackEntireScreen(); 
            }
            skipNextRun1 = false;
        }
    }
}
