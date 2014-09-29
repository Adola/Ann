﻿using System;
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
        public stripv2 parent;
        public int numPixels;
        public int l, r, t, b; // !!! these are only set in the gameObjects class !!!
        public blobv2()
        {
            strips = new List<stripv2>();
            parent = new stripv2();
            numPixels = 0;
            l = 1930; r = -1; t =  1090; b = -1; // will difine smallest rectangle to confine the blob. left, right(x values) and top, bottom(y vales)
        }
    }

    class stripBlob
    {
        const int bigP = 7907;

        public static bool stripsMatch(stripv2 s1, stripv2 s2)
        {
            return (stripWidth(s1) == stripWidth(s2) && s1.c == s2.c);
        }

        public static blobv2 findBlobInImage(List<blobv2> lookfor, List<blobv2> imageBlobs, int[][] imageBlobsHashTable)
        {


            return null;
        }
        
        public static List<stripv2> stripInSetOfStrips(stripv2 s, List<stripv2> ls, int[][] lshashtable)  // this destroys the hashtable!!!  (position is not used for hashing, so the returned element is removed. (gives uniqe return values)
        {
            // get the hash value of the strip, and search each strip in that bucket to look for a match
            int hind = getStripHash(s) % ls.Count;
            int[] bucket = lshashtable[hind];
            int bucketindex = 1;
            List<stripv2> retstrips = new List<stripv2>();

            bucketindex = 1;
            while (bucketindex <= bucket[0])
            {
                if (stripsMatch(ls[bucket[bucketindex]], s))
                {
                    retstrips.Add(ls[bucket[bucketindex]]);
                    
                    // remove that index from the hashtable

                    bucket[0] -= 1;
                    while (bucketindex + 1 < bucket.Length)
                    {
                        bucket[bucketindex] = bucket[bucketindex + 1];
                        bucketindex++;
                    }
                    bucketindex = 0;
                }
                bucketindex++;
            }
            return retstrips;
        }
         

        public static int getBlobHash(blobv2 b)
        {
            
            int hind = bigP * (b.parent.c.ToArgb() / 0xFFFF) * stripWidth(b.parent);
            return Math.Abs(hind);

        }

        public static int getStripHash(stripv2 s)
        {
            int hind =  (bigP * (s.c.ToArgb()/ 0xFFFF) * stripWidth(s));
            return Math.Abs(hind);
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

        static public Bitmap printstrip(stripv2 s, Bitmap bp) // prints to the bitmap
        {

            //Console.Out.WriteLine("strip color is " + s.c + " location is left " + s.left + " right " + s.right);
            for (int x = s.left.X; x <= s.right.X; x++) { bp.SetPixel(x, s.left.Y, s.c); }
            return bp;
        }

        public static int[][] hashstrips(List<stripv2> strips)
        {
            // Create a hashtable for the blobs
            int stripcount = strips.Count;
            int ind = 0;
            int hind = 0;
            int elemCount = 0;
            int[][] hashLookup = new int[stripcount][]; // uses buckets, first element is the count of elements, next ones give index in blob list
            for (int i = 0; i < stripcount; i++) hashLookup[i] = new int[2]; // fill the hashtable with buckets, start with size of 2, element count and one index
            foreach (stripv2 s in strips)
            {
                hind = getStripHash(s) % (strips.Count);
                elemCount = hashLookup[hind][0];
                // fits in array?
                if (elemCount < hashLookup[hind].Length - 1)
                {
                    hashLookup[hind][elemCount + 1] = ind;
                    hashLookup[hind][0] += 1;
                }
                else // else make the bucket larger
                {
                    int[] newbucket = new int[elemCount * 2 + 1];
                    hashLookup[hind].CopyTo(newbucket, 0);
                    hashLookup[hind] = newbucket;
                    hashLookup[hind][elemCount + 1] = ind;
                    hashLookup[hind][0] += 1;
                    //Console.Out.WriteLine("l : " + newbucket.Length + " lenght of new hl thing is " + hashLookup[hind].Length + " elem count was " + elemCount);
                }
                ind++;
            }


            int[] markoff = new int[strips.Count];
            for (int i = 0; i < markoff.Length; i++)
                markoff[i] = i;
            int count = 0;

            return hashLookup;
        }

        public static int[][] hashblobs(List<blobv2> blobs)
        {
            // Create a hashtable for the blobs
            int blobcount = blobs.Count;
            int ind = 0;
            int hind = 0;
            int elemCount = 0;
            int[][] hashLookup = new int[blobcount][]; // uses buckets, first element is the count of elements, next ones give index in blob list
            for (int i = 0; i < blobcount; i++) hashLookup[i] = new int[2]; // fill the hashtable with buckets, start with size of 2, element count and one index
            foreach (blobv2 s in blobs)
            {
                hind = getBlobHash(s) % (blobs.Count);
                elemCount = hashLookup[hind][0];
                // fits in array?
                if (elemCount < hashLookup[hind].Length - 1)
                {
                    hashLookup[hind][elemCount + 1] = ind;
                    hashLookup[hind][0] += 1;
                }
                else // else make the bucket larger
                {
                    int[] newbucket = new int[elemCount * 2 + 1];
                    hashLookup[hind].CopyTo(newbucket, 0);
                    hashLookup[hind] = newbucket;
                    hashLookup[hind][elemCount + 1] = ind;
                    hashLookup[hind][0] += 1;
                    //Console.Out.WriteLine("l : " + newbucket.Length + " lenght of new hl thing is " + hashLookup[hind].Length + " elem count was " + elemCount);
                }
                ind++;
            }


            int[] markoff = new int[blobs.Count];
            for (int i = 0; i < markoff.Length; i++)
                markoff[i] = i;
            int count = 0;

            return hashLookup;
        }

        public static List<stripv2> getUnconnectedStripsInRect(Bitmap bp, Rectangle rect) // speedier version used for creating strips to search through rather than for distinguishing blobs
        {
            Stopwatch sw = new Stopwatch(); sw.Start();
            List<stripv2> strips = new List<stripv2>();
            stripv2 s = new stripv2();

            for (int y = rect.Top; y <= rect.Bottom; y++)
            {
                // start first strips
                s = new stripv2(); s.left.Y = y; s.left.X = 0; s.c = bp.GetPixel(0, y); s.id = strips.Count;
                for (int x = rect.Left; x <= rect.Right; x++)
                {
                    // color change for current strip? end strip
                    if (s.c != bp.GetPixel(x, y))
                    {
                        s.right.X = x - 1; // last pixel of current strip was previous x value
                        s.right.Y = y;
                        strips.Add(s);
                        // set up next strip
                        s = new stripv2(); s.left.Y = y; s.left.X = x; s.c = bp.GetPixel(x, y); s.id = strips.Count;
                    }
                }
                // close last strips
                s.right.X = bp.Width - 1; // last pixel of current strip was previous x value 
                s.right.Y = y;
                s.id = strips.Count;
                strips.Add(s);
            }
            sw.Stop(); viewBlobs.changeFPS("" + (1000.0 / sw.ElapsedMilliseconds));


            return strips;
        }

        public static List<stripv2> getUnconnectedStrips(Bitmap bp) // speedier version used for creating strips to search through rather than for distinguishing blobs
        {
            Stopwatch sw = new Stopwatch(); sw.Start();
            List<stripv2> strips = new List<stripv2>();
            stripv2 s = new stripv2();

            for (int y = 0; y < bp.Height; y++)
            {
                // start first strips
                s = new stripv2(); s.left.Y = y; s.left.X = 0; s.c = bp.GetPixel(0, y); s.id = strips.Count;
                for (int x = 0; x < bp.Width; x++)
                {
                    // color change for current strip? end strip
                    if (s.c != bp.GetPixel(x, y))
                    {
                        s.right.X = x - 1; // last pixel of current strip was previous x value
                        s.right.Y = y;
                        strips.Add(s);
                        // set up next strip
                        s = new stripv2(); s.left.Y = y; s.left.X = x; s.c = bp.GetPixel(x, y); s.id = strips.Count;
                    }
                }
                // close last strips
                s.right.X = bp.Width - 1; // last pixel of current strip was previous x value 
                s.right.Y = y;
                s.id = strips.Count;
                strips.Add(s);
            }
            sw.Stop(); viewBlobs.changeFPS("" + (1000.0 / sw.ElapsedMilliseconds)); 
            

            return strips;
        }

        public static List<stripv2> getstripsInRect(Bitmap bp, Rectangle rect)
        {
            List<stripv2> strips = new List<stripv2>();
            stripv2[] aboveList = new stripv2[bp.Width];
            stripv2[] curList = new stripv2[bp.Width];
            stripv2 s = new stripv2();
            bool onnewstrip = true;

            for (int y = rect.Top; y <= rect.Bottom; y++)
            {
                // start first strips
                s = new stripv2(); s.left.Y = y; s.left.X = rect.Left; s.c = bp.GetPixel(rect.Left, y); s.id = strips.Count;
                onnewstrip = true;
                for (int x = rect.Left; x <= rect.Right; x++)
                {
                    // color change for current strip? end strip
                    if (s.c != bp.GetPixel(x, y))
                    {
                        s.right.X = x - 1; // last pixel of current strip was previous x value
                        s.right.Y = y;
                        strips.Add(s);
                        // set up next strip
                        s = new stripv2(); s.left.Y = y; s.left.X = x; s.c = bp.GetPixel(x, y); s.id = strips.Count;
                    }
                    curList[x] = s;
                    
                    if (x > rect.Left && curList[x - 1].c != curList[x].c) onnewstrip = true;
                    if (x > rect.Left && y > rect.Top && aboveList[x - 1].c != aboveList[x].c) onnewstrip = true;
                    if (y > rect.Top && onnewstrip && (s.c == aboveList[x].c))
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

            return strips;
        }

        public static List<stripv2> getstrips(Bitmap bp)
        {
            List<stripv2> strips = new List<stripv2>();
            stripv2[] aboveList = new stripv2[bp.Width];
            stripv2[] curList = new stripv2[bp.Width];
            stripv2 s = new stripv2();
            bool onnewstrip = true;

            for (int y = 0; y < bp.Height; y++)
            {
                // start first strips
                s = new stripv2(); s.left.Y = y; s.left.X = 0; s.c = bp.GetPixel(0,y); s.id = strips.Count;
                onnewstrip = true;
                for (int x = 0; x < bp.Width; x++)
                {
                    // color change for current strip? end strip
                    if (s.c != bp.GetPixel(x,y))
                    {
                        s.right.X = x - 1; // last pixel of current strip was previous x value
                        s.right.Y = y;
                        strips.Add(s);
                        // set up next strip
                        s = new stripv2(); s.left.Y = y; s.left.X = x; s.c = bp.GetPixel(x, y); s.id = strips.Count;
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

            return strips;
        }

        public static List<blobv2> getblobsRect(Bitmap bp, Rectangle rect)
        {
            /*****************************************************/
            // get strips 
            /*****************************************************/

            List<stripv2> strips = getstripsInRect(bp, rect);

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
    

        public static List<blobv2> getblobs(Bitmap bp)
        {
            /*****************************************************/
            // get strips 
            /*****************************************************/

            List<stripv2> strips = getstrips(bp);

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
