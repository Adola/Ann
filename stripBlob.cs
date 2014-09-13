using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace COLORBLOBBING
{
    // a blob is a list of connected strips
    // this returns a list of blobs.
    class blobs 
    {
        public static int[] IDs; public static int idInd = 0;
        public static List<strip> strips;
        public static List<List<strip>> allblobs;
        public static int blobNum;

        public static Bitmap printblob(List<strip> b, Bitmap bp)
        {
            foreach (strip s in b)
            {
                bp = strip.printstrip(s, bp);
            }
            return bp;
        }

        public static List<List<strip>> getblobsinit(List<strip> allstrips)
        {
            // get their IDs and sort them
            allblobs = new List<List<strip>>();
            IDs = new int[allstrips.Count];
            foreach (strip s in allstrips) { IDs[idInd] = s.id; idInd++; }
            Array.Sort(IDs);
            strips = allstrips;
            blobNum = -1; // gets incremeted after first blob list is made, used as index, so start as -1, so first value becomes 0

            getblob(true, null);
            return allblobs;
        }

        public static void getblob(bool istop, strip s)
        {
            if (s == null) // s is null, it's the first strip of a blob. (better way to indicate this? )
            {
                //Console.Out.WriteLine("number of blobs is " + (allblobs.Count +1) + " number of strips " + strips.Count );
                if (strips.Count == 0) { return; } // DONE!

                s = strips[0]; strips.RemoveAt(0); // grab a new strip that's still in the list. take first
                //Console.Out.WriteLine("the new starting strip is " + s.id);
                allblobs.Add(new List<strip>()); blobNum++; allblobs[blobNum].Add(s);
                getblob(false, s);
            }
            else
            {
                IDs[s.id] = -1; // remove it from the IDs array by setting the array at it's index to -1 

                // call again for each connected strip if still in the list
                foreach (strip sa in s.connectedAbove)
                {
                    if (IDs[sa.id] != -1)
                    {
                        allblobs[blobNum].Add(sa);
                        strips.Remove(sa);
                        getblob(false, sa);
                    }
                }
                foreach (strip sb in s.connectedBelow)
                {
                    if (IDs[sb.id] != -1)
                    {
                        allblobs[blobNum].Add(sb);
                        strips.Remove(sb);
                        getblob(false, sb);
                    }
                }
                
            }
            if (istop) getblob(true, null);
            return;
        }
    }


    class strip
    {
        public Point left;
        public Point right;
        public Color c;
        public List<strip> connectedAbove;
        public List<strip> connectedBelow;
        public int id;

        public strip()
        {
            connectedBelow = new List<strip>();
            connectedAbove = new List<strip>();
        }
    
        static public bool inRange(int x, int l, int r) // !!!! l must be less than r !!!!
        {
            if (x < l) return false;
            if (x > r) return false;
            return true;
        }

        static public bool stripsAreConnected(strip s1, strip s2)
        {
            if (s1.c != s2.c) // check that that are the same color
                return false;

            // strips are directly above or below each other 
            if (Math.Abs(s1.left.Y - s2.left.Y) > 1)
                return false;
            // they are now above or below each other, make sure they connect
            // strips are connected if the range of x values coincide 
            if (inRange(s1.left.X, s2.left.X, s2.right.X) || inRange(s1.right.X, s2.left.X, s2.right.X)
             || inRange(s2.left.X, s1.left.X, s1.right.X) || inRange(s2.right.X, s1.left.X, s1.right.X))
                return true;

            return  false;
        }

        static public Bitmap printstrip(strip s, Bitmap bp) // prints to the bitmap
        {
            for (int x = s.left.X; x <= s.right.X; x++) { bp.SetPixel(x, s.left.Y, s.c); }
            return bp;
        }

        static public List<strip> connectedStrips(List<strip> allstrips) 
        {
            List<strip> connectedStrips = new List<strip>();

            return connectedStrips;
        }

        static public List<strip> getBlobs(Bitmap bp)
        {
            int width = bp.Width; int height = bp.Height;

            List<strip> blobs = new List<strip>();
            List<Point> outlines = new List<Point>();
            
            
            
            return blobs; 
        }

        static public List<strip> getstrips(Bitmap scrn)
        {
            List<strip> strips = new List<strip>();
            int curid = 0;
            Color curpx;

            // 1.) Get each strip (Try to integrate step 2 here?)
            for (int y = 0; y < scrn.Height; y++)
            {
                // set up the first strip
                strip s; s = new strip();

                s.c = scrn.GetPixel(0, y);
                s.left = new Point(0, y);

                for (int x = 0; x < scrn.Width; x++)
                {
                    // place it in the array

                    curpx = scrn.GetPixel(x, y);
                    if (curpx != s.c) // reached a different color close the strip
                    {
                        s.right = new Point(x - 1, y); // add the rightmost point, prev pixel
                        s.id = curid; curid++; // give it a an id
                        strips.Add(s);

                        // set up the next strip
                        s = new strip();
                        s.left = new Point(x, y);
                        s.c = curpx;
                    }
                }
                // end last strip
                s.right = new Point(scrn.Width - 1, y); // add the rightmost point, prev pixel
                strips.Add(s); // add the strip to the list
                s.id = curid; curid++; // give it a an id
            }
            
            // 2.) connect the strips
            strip[,] stripArray = new strip[scrn.Width, scrn.Height];
            foreach (strip s in strips)
            {
                for (int x = s.left.X; x <= s.right.X; x++)
                    stripArray[x, s.left.Y] = s;
            }

            strip current = new strip(); strip above = new strip(); strip below = new strip();
            for (int y = 0; y < scrn.Height; y++)
            {
                for (int x = 0; x < scrn.Width; x++)
                {
                    current = stripArray[x, y];
                    if (y > 0) { above = stripArray[x, y - 1]; }
                    if (y < scrn.Height - 1) { below = stripArray[x, y + 1]; }
                    if (y > 0)
                    {
                        if (current.c == above.c)
                        {
                            if(! current.connectedAbove.Contains(above))
                                current.connectedAbove.Add(above);
                        } 
                    } 
                    if (y < scrn.Height - 1) 
                    { 
                        if (current.c == below.c) 
                        { 
                            if (! current.connectedBelow.Contains(below))
                                current.connectedBelow.Add(below); 
                        } 
                    } 
                }
            }

            foreach (strip s in strips)
            {
                //Console.Out.WriteLine("strip " + s.id + " of width " + (s.right.X - s.left.X) + " color is " + s.c.ToString() + 
                //    " has " + s.connectedAbove.Count + " connections above and " + s.connectedBelow.Count + " below \n");
            }

            return strips;
        }
        
    }
}
