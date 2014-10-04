using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Player
{
    class gameObject
    {
        public static Color black = Color.FromArgb(255, 2, 2, 2);

        public Bitmap objectImage; public Color bg;
        public blobv2 keyBlob; // assume the object has a complicated, unique blob, that we can search for. 
                        // the other blobs (ex. 1 black pixel) is not helpful, so we can search for the object only by the key blob. 
        // stuff for finding movement? 

        public gameObject(Bitmap image, Color bgc)
        {
            objectImage = image; bg = bgc;
            keyBlob = getKeyBlob();
            setKeyBlobRect(keyBlob);

        }

        public blobv2 getKeyBlob()
        {
            List<blobv2> objectBlobs = stripBlob.getblobs(objectImage);
            blobv2 key = new blobv2(); double keyComplexity = 0;
            double curComplexity = 0;
            foreach (blobv2 ob in objectBlobs)
            {
                if (ob.parent.c != bg && ob.parent.c != black) // black is also generally a bad color to track as outlines merge to one blob.
                {
                    setKeyBlobRect(ob);

                    curComplexity = blobcomplexity(ob);
                    if (curComplexity > keyComplexity)
                    {
                        key = ob;
                        keyComplexity = curComplexity;
                    }
                }
            }
            return key;
        }

        public double blobcomplexity(blobv2 b)  // calculated by "holes" in the blob (these occur along edges of the image as well)
        {
            int gapCount = 0;
            foreach (stripv2 s in b.strips)
                gapCount += s.connectedBelow.Count;

            return gapCount * ((double) ( (b.r - b.l + 1) * (b.b - b.t + 1)) / b.strips.Count  );
        }

        public blobv2 setKeyBlobRect(blobv2 kb)
        {
            foreach (stripv2 s in kb.strips)
            {
                if (s.left.X < kb.l) kb.l = s.left.X;
                if (s.right.X > kb.r) kb.r = s.right.X;

                if (s.left.Y < kb.t) kb.t = s.left.Y;
                if (s.left.Y > kb.b) kb.b = s.left.Y;
            }
            return kb;
        }

        public Rectangle keyblobInrectstrips(Bitmap bp, Rectangle rect)
        {
            List<stripv2> bpstrips = stripBlob.getstripsInRect(bp, rect);
            int[][] bpstripsHashTable = stripBlob.hashstrips(bpstrips);
            
            bool allin = true;
            foreach(stripv2 s in keyBlob.strips)
                if (stripBlob.stripInSetOfStrips(s, bpstrips, bpstripsHashTable).Count == 0)
                {
                    allin = false;
                    break;
                }
            if (allin) return rect;

            return new Rectangle(-1, -1, -1, -1);
        }

        public Rectangle entireObjectFromKeyBlobPosition()
        {
            return new Rectangle(-1, -1, -1, -1);
        }

        public Rectangle keyblobInrect(Bitmap bp, Rectangle rect)
        {
            List<blobv2> bpstrips = stripBlob.getblobsRect(bp, rect);
            //Console.Out.WriteLine("count of blobs " + bpstrips.Count);
            int[][] bpstripsHashTable = stripBlob.hashblobs(bpstrips);

            int hind = stripBlob.getBlobHash(keyBlob) % bpstrips.Count;

            //Console.Out.WriteLine("hind " + hind + " s count " + bpstrips.Count);
            int[] bucket = bpstripsHashTable[hind];

            int elementcout = bucket[0];

            int index = 1;
            blobv2 curblob = new blobv2();
            bool found = false;
            while (index <= elementcout)
            {
                curblob = bpstrips[ bucket[index] ];
                //Console.Out.WriteLine(curblob.parent.c + " " + keyBlob.parent.c + " " + curblob.numPixels + " " + keyBlob.numPixels + " " + " " + " " + " " + " " + " ");
                if (curblob.parent.c == keyBlob.parent.c && curblob.numPixels == keyBlob.numPixels && stripBlob.stripWidth(curblob.parent) == stripBlob.stripWidth(keyBlob.parent))
                {
                    found = true;
                    break;
                }
                index++;
            }
            if (found)
            {
                curblob = setKeyBlobRect(curblob);
                Console.Out.WriteLine("before add " + curblob.l + " " + curblob.r + " " + curblob.t + " " + curblob.b);
                return new Rectangle(curblob.l - keyBlob.parent.left.X, curblob.t - keyBlob.parent.left.Y, objectImage.Width, objectImage.Height) ;
                
            }
            return new Rectangle(-1,-1,-1,-1);
        }

    }
}
