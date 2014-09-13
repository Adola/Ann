using COLORBLOBBING;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;


namespace AforgeTestVision1
{
    
    /*
    class Program
    {
        static Random rand = new Random();

        static double distance(Point p1, Point p2)
        {
            double d = Math.Sqrt( Math.Pow((p2.X - p1.X),2) + Math.Pow((p2.Y - p1.Y),2) );
            return d;
        }

        static Point calcPointInDir(Point p, int dir)
        {
            int addtox = 0;
            int addtoy = 0;

                 if (dir == 2) { addtox =  1; addtoy =  0; } // right
            else if (dir == 3) { addtox =  1; addtoy =  1; } // D right
            else if (dir == 4) { addtox =  0; addtoy =  1; } // Down
            else if (dir == 5) { addtox = -1; addtoy =  1; } // D left
            else if (dir == 6) { addtox = -1; addtoy =  0; } // left
            else if (dir == 7) { addtox = -1; addtoy = -1; } // up left
            else if (dir == 0) { addtox =  0; addtoy = -1; } // up
            else if (dir == 1) { addtox =  1; addtoy = -1; } // up right

            p.X += addtox;
            p.Y += addtoy;

            return p;
        }

        static void Main(string[] args)
        {
            Bitmap scrn = new Bitmap("C:\\Users\\Chris\\Downloads\\jnes_1_1_1\\screenshots\\mario.bmp");
            Stopwatch stopWatch = new Stopwatch(); stopWatch.Start();

            List<strip> allstrips = new List<strip>();

                // Get every strip
            allstrips = strip.getstrips(scrn);

                // Turn the strips into blobs
            List<List<strip>> allblobs = blobs.getblobsinit(allstrips);
            
            int num = 0;
            foreach (List<strip> s in allblobs)
            {
                Bitmap bp = new Bitmap(scrn.Width, scrn.Height);
                bp = blobs.printblob(s, bp);
                bp.Save("C:\\Users\\Chris\\Downloads\\jnes_1_1_1\\screenshots\\blobs\\blob " + num + ".bmp");
                num++;
            }
            


            stopWatch.Stop(); Console.Out.WriteLine("time = " + stopWatch.ElapsedMilliseconds); stopWatch.Reset();
            Console.Out.WriteLine("Done");
            Console.ReadKey();
        }
    }
     */
}
