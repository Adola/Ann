using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;

namespace Player
{
    public class stripv2
    {
        public Point left;
        public Point right;
        public Color c;
        public stripv2 parent;
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

    public class blobv2
    {
        public List<stripv2> strips;
        public stripv2 parent;
        public int numPixels;
        public int l, r, t, b;
        public blobv2()
        {
            strips = new List<stripv2>();
            parent = new stripv2();
            numPixels = 0;
            l = 0xFFFFFFF; r = - 0xFFFFFFF; t = 0xFFFFFFF; b = - 0xFFFFFFF; // will difine smallest rectangle to confine the blob. left, right(x values) and top, bottom(y vales)
        }
    }


    public class stripBlob
    {
        public static bool getStripsIsLocked = false;

        [DllImport("gdi32.dll", EntryPoint = "BitBlt")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BitBlt(
            [In()] System.IntPtr hdc, int x, int y, int cx, int cy,
            [In()] System.IntPtr hdcSrc, int x1, int y1, uint rop);

        public static void setBlobRectangle(blobv2 blob)
        {
            foreach(stripv2 s in blob.strips) 
            {
                if (s.left.X < blob.l)
                {
                    blob.l = s.left.X;
                }
                if (s.right.X > blob.r) blob.r = s.right.X;

                if (s.left.Y < blob.t) blob.t = s.left.Y;
                if (s.left.Y > blob.b) blob.b = s.left.Y;
            }
        }

        public static byte GetBitsPerPixel(PixelFormat pixelFormat)
        {
            switch (pixelFormat)
            {
                case PixelFormat.Format24bppRgb:
                    return 24;
                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppPArgb:
                case PixelFormat.Format32bppRgb:
                    return 32;
                default:
                    throw new ArgumentException("Only 24 and 32 bit images are supported");
            }
        }

        public static unsafe void drawStrip(stripv2 s, Graphics g) // prints to the bitmap
        {
            Pen p = new Pen(s.c, (float) 3);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            g.DrawLine(p, s.left.X, s.left.Y, s.right.X, s.right.Y);
            
        }

        public static bool stripsMatch(stripv2 s1, stripv2 s2)
        {
            return (stripWidth(s1) == stripWidth(s2) && s1.c == s2.c);
        }

        public static int stripWidth(stripv2 s)
        {
            return (s.right.X - s.left.X + 1);
        }


        public static bool similarcolor(Color c1, Color c2)
        {
            int threshold = 1;
            return (Math.Abs(c1.R - c2.R) < threshold && Math.Abs(c1.G - c2.G) < threshold && Math.Abs(c1.B - c2.B) < threshold );
        }

        public unsafe static blobv2 getKeyBlob(Bitmap sprite)
        {

            return new blobv2();
        }

        public unsafe static List<blobv2> getstripsInRectFaster(Bitmap bp, Rectangle rect)
        {
            List<stripv2> strips = new List<stripv2>();
            try
            {
                BitmapData bData = bp.LockBits(rect, ImageLockMode.ReadOnly, bp.PixelFormat);
                byte bitsPerPixel = GetBitsPerPixel(bp.PixelFormat);
                int size = bData.Stride * bData.Height;
                byte* scan0 = (byte*)bData.Scan0.ToPointer();
                int colWidth = bData.Width * bitsPerPixel / 8;

                strips = new List<stripv2>();
                stripv2[] aboveList = new stripv2[bp.Width];
                stripv2[] curList = new stripv2[bp.Width];
                stripv2 s = new stripv2();
                bool onnewstrip = true;

                Color curpixel;
                byte* data;

                for (int y = 0; y < rect.Height; y++)
                {
                    // start first strip 
                    s = new stripv2();
                    s.left.Y = y;
                    s.left.X = 0;

                    data = scan0 + y * bData.Stride + 0 * bitsPerPixel / 8;
                    s.c = Color.FromArgb(255, data[2], data[1], data[0]);

                    s.id = strips.Count;
                    //s = new stripv2(); s.left.Y = y; s.left.X = 0; s.c = carr[0, y]; s.id = strips.Count;
                    onnewstrip = true;
                    for (int x = 0; x < rect.Width; x++)
                    {
                        // load the current pixel
                        data = scan0 + y * bData.Stride + x * bitsPerPixel / 8;
                        curpixel = Color.FromArgb(255, data[2], data[1], data[0]);

                        // color change for current strip? end strip
                        if (s.c != curpixel)
                        {
                            s.right.X = x - 1; // last pixel of current strip was previous x value
                            s.right.Y = y;
                            strips.Add(s);
                            // set up next strip
                            s = new stripv2();
                            s.left.Y = y;
                            s.left.X = x;

                            data = scan0 + y * bData.Stride + x * bitsPerPixel / 8;
                            s.c = Color.FromArgb(255, data[2], data[1], data[0]);

                            s.id = strips.Count;
                        }
                        curList[x] = s;

                        if (x > 0 && curList[x - 1].c != curList[x].c) onnewstrip = true;
                        if (x > 0 && y > 0 && aboveList[x - 1].c != aboveList[x].c) onnewstrip = true;
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

                bp.UnlockBits(bData);
            }
            catch (System.ArgumentException ae) { } // \o/

            return getblobsRect(strips);
        }


        public static List<blobv2> getblobsRect(List<stripv2> strips)
        {
            /*****************************************************/
            // get strips 
            /*****************************************************/

            int[] strarr = new int[strips.Count];
            int strarrind = 0;
            int last = 0;

            List<stripv2> curlist = new List<stripv2>();
            List<stripv2> curlistsub = new List<stripv2>();

            stripv2 par = new stripv2();
            stripv2 cur = new stripv2();
            bool done = false;

            /*****************************************************/
            // connect the strips
            // this algorithm connects strips from top to bottom
            /*****************************************************/

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

            /*****************************************************/
            // handle a case where the blob forms a v sort of shape. where the right top needs to have the same parent (meaning it's the same blob)
            // This handles that case and gives each strip a blob number that goes in the lookup table
            /*****************************************************/

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

            /*****************************************************/
            // create blobs and add the strips to the correct blobs
            /*****************************************************/

            List<blobv2> blobs = new List<blobv2>();
            while(blobs.Count < blobnum -1) blobs.Add(new blobv2());
            int ind = 0;
            foreach (stripv2 sv2 in strips)
            {
                ind = lookuptable[sv2.parent.id] - 1;
                if (sv2.parent.id == sv2.id) blobs[ind].parent = sv2; // if the strip is the first one of the blob, set it as the parent of the blob. This will be used for hash lookup, comparing blobs etc
                blobs[ind].strips.Add(sv2);
                blobs[ind].numPixels += (sv2.right.X - sv2.left.X + 1);

            }
            return blobs;

        }
    }
}
