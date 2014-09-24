using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;


class stripv2
{
    public Point left;
    public Point right;
    public Color c;
    public stripv2 parent;
    //public List<Point> connectedBelow;
    public List<Point> connectedBelow;
    public int id;

    public stripv2()
    {
        left = new Point();
        right = new Point();
        connectedBelow = new List<Point>();
        parent = null;
    }
}

/*
class smallBlobFinder
{


}
*/

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
    public static List<List<stripv2>> getstripsv2(Bitmap bp)
    {
        List<stripv2> strips = new List<stripv2>();
        stripv2[,] striparray = new stripv2[bp.Width, bp.Height];
        int curid = 0;
        Color curpx;
        bool canbenewstrip = true;

        //Stopwatch sw = new Stopwatch(); sw.Start();

        for (int y = 0; y < bp.Height; y++)
        {
            // set up the first strip
            stripv2 s= new stripv2();
            s.c = bp.GetPixel(0, y);
            s.left = new Point(0, y);
            s.id = curid; curid++;
            canbenewstrip = true;
            for (int x = 0; x < bp.Width; x++)
            {
                curpx = bp.GetPixel(x, y);
                if (curpx != s.c) // reached a different color close the strip
                {
                    s.right = new Point(x - 1, y); // add the rightmost point, prev pixel
                    strips.Add(s);

                    // put it in the array
                    for(int i = s.left.X; i <= s.right.X; i++) striparray[i, y] = s;

                    // set up the next strip
                    canbenewstrip = true;
                    s = new stripv2();
                    s.id = curid; curid++;
                    s.left = new Point(x, y);
                    s.c = curpx;
                }


                //Console.Out.WriteLine("\nx " + x + " y " + y + " can be new " + canbenewstrip);
                // connected below?
                if (y < bp.Height - 1 && bp.GetPixel(x, y + 1) == s.c)
                {
                    if (canbenewstrip)
                    {
                        //Console.Out.WriteLine("added x " + x + " y " + (y + 1));
                        s.connectedBelow.Add(new Point(x, y + 1));
                        canbenewstrip = false;
                    }
                }
                else
                    canbenewstrip = true;

            }
            // end last strip
            s.right = new Point(bp.Width - 1, y); // add the rightmost point, prev pixel
            strips.Add(s); // add the strip to the list
            for (int i = s.left.X; i <= s.right.X; i++) striparray[i, y] = s;
        }

        /*
        sw.Stop();
        Console.Out.WriteLine("--------------------------first part take " + sw.ElapsedMilliseconds);
        sw.Reset();
        */

        // try to build a blob
        // grab the first one

        //List<stripv2> blobstrips = new List<stripv2>(); blobstrips.AddRange(strips);

        int[] strarr = new int[strips.Count];
        int strarrind = 0;
        int last = 0;

        List<stripv2> curlist = new List<stripv2>();
        List<stripv2> curlistsub = new List<stripv2>();

        stripv2 par = new stripv2();
        stripv2 cur = new stripv2();
        bool done = false;

        //sw.Start();

        while (true)
        {
            //par = strips[0]; 
            strarrind = last;
            while (strarrind < strarr.Length && strarr[strarrind] == -1) strarrind++;

            if (strarrind == strarr.Length) break; //d done

            par = strips[strarrind];
            last = strarrind;

            par.parent = par; 

            //strips.RemoveAt(0);
            strarr[strarrind] = -1;

            curlist.Add(par);
            done = false;
            while (!done)
            {
                //Console.Out.WriteLine(curlist.Count);
                if (curlist.Count == 0)
                    done = true;

                curlistsub.Clear();

                foreach (stripv2 cs in curlist)
                {
                    //Console.Out.WriteLine("strip id " + cs.id);
                    foreach (Point cb in cs.connectedBelow)
                    {

                        cur = striparray[cb.X, cb.Y];

                        if (cur.parent == null)
                        {
                            cur.parent = par;
                            curlistsub.Add(cur);
                        }
                        else
                        {
                            par.parent = cur.parent;
                        }

                    }
                }
                curlist.Clear();
                curlist.AddRange(curlistsub);

                foreach (stripv2 r in curlist) strarr[r.id] = -1;
            }
        }

        /*
        sw.Stop();
        Console.Out.WriteLine("--------------------------second part part take " + sw.ElapsedMilliseconds);
        sw.Reset();
        */


        int blobnum = 1; // start at 1 so 0 can represent that the elements of the lookup table have not been set.
        int[] lookuptable = new int[strips.Count + 1];
        
        //sw.Start();

        foreach (stripv2 sv2 in strips)
        {
            sv2.parent = sv2.parent.parent;
            if (lookuptable[sv2.parent.id] == 0)
            {
                lookuptable[sv2.parent.id] = blobnum;
                blobnum++;
            }
        }

        /*
        sw.Stop();
        Console.Out.WriteLine("--------------------------third part take " + sw.ElapsedMilliseconds);
        sw.Reset();

        sw.Start();
        */ 

        List<List<stripv2>> blobs = new List<List<stripv2>>();
        while( blobs.Count < blobnum -1 ) blobs.Add(new List<stripv2>());
       
        foreach (stripv2 sv2 in strips)
        {
            blobs[ lookuptable[sv2.parent.id] -1 ].Add(sv2);
        }
        
        /*
        sw.Stop();
        Console.Out.WriteLine("--------------------------fourth part take " + sw.ElapsedMilliseconds);
        sw.Reset();
        */

        return blobs;
    }
}

/*
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
            allblobs.Clear(); blobNum++; allblobs[blobNum].Add(s);
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

        return false;
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
                        if (!current.connectedAbove.Contains(above))
                            current.connectedAbove.Add(above);
                    }
                }
                if (y < scrn.Height - 1)
                {
                    if (current.c == below.c)
                    {
                        if (!current.connectedBelow.Contains(below))
                            current.connectedBelow.Add(below);
                    }
                }
            }
        }
        return strips;
    }
   
}
*/