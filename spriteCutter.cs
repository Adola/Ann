using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace Player
{
    class spriteCutter
    {
        static Color uniqueColor = Color.FromArgb(255, 193, 107, 255);  // a purple color, if the background is this color this program will fail

        public static void cutSpriteSheetUniform(String imagePaths, int w, int h, Boolean save)
        {
                Bitmap ss = new Bitmap(imagePaths);
                Bitmap sprite = new Bitmap(w, h);
                int num = 0;
                for (int i = 0; i < ss.Width; i+=w)
                {
                    for (int j = 0; j < ss.Height; j+=h)
                    {
                        // each tile -------------------
                        for(int k = 0; k < w; k++)
                        {
                            for (int l = 0; l < h; l++)
                            {
                                sprite.SetPixel(k, l, ss.GetPixel(i+k, j+l));
                            }
                        }
                        // save tile
                        sprite.Save("C:\\Users\\Chris\\Downloads\\sprites\\" + num + ".bmp");
                        num++;
                        // -----------------------------
                    }
                }
        }

        public static void cutSpriteSheet(String[] imagePaths, Boolean save) // !!! Must be a bitmap image, and must manually remove all extra things on sprite sheet (text etc) !!!
        {
            /*
             * 
             * 
            for (int i = 0; i < imagePaths.Length; i++)
            {
                Bitmap ss = new Bitmap(imagePaths[i]);

                Bitmap spriteSheet = new Bitmap(ss.Width, ss.Height);
                for (int y = 0; y < spriteSheet.Height; y++)
                    for (int x = 0; x < spriteSheet.Width; x++)
                        spriteSheet.SetPixel(x, y, ss.GetPixel(x, y));

                Bitmap spriteSheetCopy = (Bitmap)spriteSheet.Clone();

                // get bg (top left)
                Color bg = spriteSheet.GetPixel(0, 0); // assume top left is background color

                // make all pixels that are different from the bg color( they are part of a sprite) the same 
                for (int y = 0; y < spriteSheet.Height; y++)
                    for (int x = 0; x < spriteSheet.Width; x++)
                        if (spriteSheet.GetPixel(x, y) != bg)
                            spriteSheet.SetPixel(x, y, uniqueColor);

                // run the strip blob algorithm
                List<blobv2> blobs = stripBlob.getblobs(spriteSheet);

                // the rect of the blobs is not calculated normally, for speed, so calculate each of them.
                foreach (blobv2 b in blobs) stripBlob.setBlobRectangle(b);

                // each blobs rect now becomes the rect of a sprite
                List<Rectangle> spriteRectangles = new List<Rectangle>();
                foreach (blobv2 b in blobs) spriteRectangles.Add(new Rectangle(b.l, b.t, (b.r - b.l + 1), (b.b - b.t + 1)));
                foreach (blobv2 b in blobs) Console.Out.WriteLine("size of sprite is (" + (b.r - b.l + 1) + ", " + (b.b - b.t + 1) + ")");

                // make them into seperate bitmaps
                int spriteNum = 1;
                foreach (Rectangle r in spriteRectangles)
                {
                    if ((r.Width > 1 || r.Height > 1) && (r.Width != spriteSheet.Width && r.Height != spriteSheet.Height)) // ignore some odd stray pixels it's finding, and the background
                    {
                        Bitmap currentSprite = new Bitmap(r.Width, r.Height);
                        for (int y = r.Y; y < r.Y + r.Height; y++)
                            for (int x = r.X; x < r.X + r.Width; x++)
                                currentSprite.SetPixel(x - r.X, y - r.Y, spriteSheetCopy.GetPixel(x, y));
                        String spriteSavePath = imagePaths[i].Split('.')[0] + "  " + spriteNum + "." + imagePaths[i].Split('.')[1];
                        Console.Out.WriteLine("save path is " + spriteSavePath);
                        currentSprite.Save(spriteSavePath); spriteNum++;
                    }
                }
            }
             * 
             * 
             */
        }
    }
}
