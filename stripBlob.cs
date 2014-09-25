using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace Player
{
    class stripv2
    {
        public Point left;
        public Point right;
        public Color c;
        public stripv2 parent;
        //public List<Point> connectedBelow;
        public List<stripv2> connectedBelow;
        public int id;

        public stripv2()
        {
            left = new Point();
            right = new Point();
            connectedBelow = new List<stripv2>();
            parent = null;
        }
    }

    class blobv2
    {
        public List<stripv2> strips;
        public int numPixels;
        public int l, r, t, b;
        public blobv2()
        {
            strips = new List<stripv2>();
            numPixels = 0;
            l = -1; r = 1930; t = -1; b = 1090;
        }
    }
    class blobfinder
    {
        public static List<Rectangle> getBlobBoundingRectangles(List<List<stripv2>> blobs)
        {
            // return a list of 2 element point arrays, these have the top left, and bottom right point of the rectangle respectively
            List<Rectangle> rl = new List<Rectangle>();
            int tlx, tly, brx, bry; // top left x, top left y...

            // look at each blob
            foreach (List<stripv2> ls in blobs)
            {
                // look at each strip in the blob to findf the correct bounding rectangle
                // initialize the points to the first blob
                tlx = blobs[0][0].left.X;
                tly = blobs[0][0].left.Y;
                brx = blobs[0][0].right.X;
                bry = blobs[0][0].right.Y;

                foreach (stripv2 s in ls)
                {
                    if (s.left.X < tlx) tlx = s.left.X;
                    if (s.right.X > brx) tlx = s.right.X;

                    if (s.left.Y < tly) tly = s.left.Y;
                    if (s.left.Y > bry) bry = s.left.Y;
                }
                Rectangle r = new Rectangle(tlx, tly, (brx - tlx), (bry - tly));
                rl.Add(r);
            }
            return rl;
        }


        static public Bitmap printstrip(stripv2 s, Bitmap bp) // prints to the bitmap
        {
            for (int x = s.left.X; x <= s.right.X; x++) { bp.SetPixel(x, s.left.Y, s.c); }
            return bp;
        }
        public static List<blobv2> getstripsv2(Bitmap bp)
        {
            // very slight speed up for reading a full screen.
            Color[,] carr = new Color[bp.Width, bp.Height];
            for (int y = 0; y < bp.Height; y++) for (int x = 0; x < bp.Width; x++) carr[x,y] = bp.GetPixel(x, y);


            List<stripv2> strips = new List<stripv2>();
            stripv2[] aboveList = new stripv2[bp.Width];
            stripv2[] curList   = new stripv2[bp.Width];
            //bool[] connectedBelow = new bool[bp.Width];
            stripv2 s = new stripv2();
            //stripv2[,] striparray = new stripv2[bp.Width, bp.Height];
            bool onnewstrip = true;

            for (int y = 0; y < bp.Height; y++)
            {
                // start first strips
                s = new stripv2(); s.left.Y = y; s.left.X = 0; s.c = carr[0,y]; s.id = strips.Count;
                onnewstrip = true;
                for (int x = 0; x < bp.Width; x++)
                {
                    // color change for current strip? end strip
                    if (s.c != carr[x,y])
                    {
                        s.right.X = x - 1; // last pixel of current strip was previous x value
                        s.right.Y = y;
                        strips.Add(s);
                        // set up next strip
                        s = new stripv2(); s.left.Y = y; s.left.X = x; s.c = carr[x,y]; s.id = strips.Count;
                    }
                    curList[x] = s;

                    if (x > 0 && curList[x - 1].c != curList[x].c) onnewstrip = true;
                    if (x > 0 && y>0 && aboveList[x - 1].c != aboveList[x].c) onnewstrip = true;
                    if (y > 0 && onnewstrip && (s.c == aboveList[x].c))
                    {
                        aboveList[x].connectedBelow.Add(s);
                        onnewstrip = false;
                    }
                }
                // close last strips
                s.right.X = bp.Width - 1; // last pixel of current strip was previous x value 
                s.right.Y = y;
                s.id = strips.Count;
                strips.Add(s);
                curList.CopyTo(aboveList, 0);
            }

            int[] strarr = new int[strips.Count];
            int strarrind = 0;
            int last = 0;

            List<stripv2> curlist = new List<stripv2>();
            List<stripv2> curlistsub = new List<stripv2>();

            stripv2 par = new stripv2();
            stripv2 cur = new stripv2();
            bool done = false;

            while (true)
            {
                strarrind = last;
                while (strarrind < strarr.Length && strarr[strarrind] == -1) strarrind++;

                if (strarrind == strarr.Length) break; 

                par = strips[strarrind];
                last = strarrind;

                par.parent = par;

                strarr[strarrind] = -1;

                curlist.Add(par);
                done = false;
                while (!done)
                {
                    if (curlist.Count == 0)
                        done = true;

                    curlistsub.Clear();

                    foreach (stripv2 cs in curlist)
                    {
                        foreach (stripv2 cb in cs.connectedBelow)
                        {
                            if (cb.parent == null)
                            {
                                cb.parent = par;
                                curlistsub.Add(cb);
                            }
                            else
                            {
                                par.parent = cb.parent;
                            }

                        }
                    }
                    curlist.Clear();
                    curlist.AddRange(curlistsub);

                    foreach (stripv2 r in curlist) strarr[r.id] = -1;
                }
            }

            int blobnum = 1; // start at 1 so 0 can represent that the elements of the lookup table have not been set.
            int[] lookuptable = new int[strips.Count + 1];

            foreach (stripv2 sv2 in strips)
            {
                sv2.parent = sv2.parent.parent;
                if (lookuptable[sv2.parent.id] == 0)
                {
                    lookuptable[sv2.parent.id] = blobnum;
                    blobnum++;
                }
            }
            
            List<blobv2> blobs = new List<blobv2>();
            while(blobs.Count < blobnum -1) blobs.Add(new blobv2());
            int ind = 0;
            foreach (stripv2 sv2 in strips)
            {
                ind = lookuptable[sv2.parent.id] - 1;
                blobs[ind].strips.Add(sv2);
                blobs[ind].numPixels++;
            }

            return blobs;
        }
    }
}
