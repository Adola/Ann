// Simple Player sample application
// AForge.NET framework
// http://www.aforgenet.com/framework/
//
// Copyright © AForge.NET, 2006-2011
// contacts@aforgenet.com
//

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

using AForge.Video;
using AForge.Video.DirectShow;
using System.Threading;
using System.Runtime.InteropServices;
using AForge.Imaging;
using AForge.Math.Geometry;
using System.Drawing.Imaging;
using System.Drawing;
using AForge;
using AForge.Imaging.Filters;

namespace Player
{
    public partial class MainForm : Form
    {

        private Grayscale grayscaleFilter = new Grayscale(1,1,1);
        private EuclideanColorFiltering euclideanFilter = new EuclideanColorFiltering();
        BlobCounter blobCounter = new BlobCounter(); 

        private Stopwatch stopWatch = null;

        // Class constructor
        public MainForm( )
        {
            InitializeComponent( );
        }

        private void MainForm_FormClosing( object sender, FormClosingEventArgs e )
        {
            CloseCurrentVideoSource( );
        }

        // "Exit" menu item clicked
        private void exitToolStripMenuItem_Click( object sender, EventArgs e )
        {
            this.Close( );
        }

        // Open local video capture device
        private void localVideoCaptureDeviceToolStripMenuItem_Click( object sender, EventArgs e )
        {
            VideoCaptureDeviceForm form = new VideoCaptureDeviceForm( );

            if ( form.ShowDialog( this ) == DialogResult.OK )
            {
                // create video source
                VideoCaptureDevice videoSource = form.VideoDevice;

                // open it
                OpenVideoSource( videoSource );
            }
        }

        // Open video file using DirectShow
        private void openVideofileusingDirectShowToolStripMenuItem_Click( object sender, EventArgs e )
        {
            if ( openFileDialog.ShowDialog( ) == DialogResult.OK )
            {
                // create video source
                FileVideoSource fileSource = new FileVideoSource( openFileDialog.FileName );

                // open it
                OpenVideoSource( fileSource );
            }
        }

        // Open JPEG URL
        private void openJPEGURLToolStripMenuItem_Click( object sender, EventArgs e )
        {
            URLForm form = new URLForm( );

            form.Description = "Enter URL of an updating JPEG from a web camera:";
            form.URLs = new string[]
				{
					"http://195.243.185.195/axis-cgi/jpg/image.cgi?camera=1",
				};

            if ( form.ShowDialog( this ) == DialogResult.OK )
            {
                // create video source
                JPEGStream jpegSource = new JPEGStream( form.URL );

                // open it
                OpenVideoSource( jpegSource );
            }
        }

        // Open MJPEG URL
        private void openMJPEGURLToolStripMenuItem_Click( object sender, EventArgs e )
        {
            URLForm form = new URLForm( );

            form.Description = "Enter URL of an MJPEG video stream:";
            form.URLs = new string[]
				{
					"http://195.243.185.195/axis-cgi/mjpg/video.cgi?camera=4",
					"http://195.243.185.195/axis-cgi/mjpg/video.cgi?camera=3",
				};

            if ( form.ShowDialog( this ) == DialogResult.OK )
            {
                // create video source
                MJPEGStream mjpegSource = new MJPEGStream( form.URL );

                // open it
                OpenVideoSource( mjpegSource );
            }
        }

        // Open video source
        private void OpenVideoSource( IVideoSource source )
        {
            // set busy cursor
            this.Cursor = Cursors.WaitCursor;

            // stop current video source
            CloseCurrentVideoSource( );

            // start new video source
            videoSourcePlayer.VideoSource = source;
            videoSourcePlayer.Start( );

            // reset stop watch
            stopWatch = null;

            // start timer
            timer.Start( );

            this.Cursor = Cursors.Default;
        }

        // Close video source if it is running
        private void CloseCurrentVideoSource( )
        {
            if ( videoSourcePlayer.VideoSource != null )
            {
                videoSourcePlayer.SignalToStop( );

                // wait ~ 3 seconds
                for ( int i = 0; i < 30; i++ )
                {
                    if ( !videoSourcePlayer.IsRunning )
                        break;
                    System.Threading.Thread.Sleep( 100 );
                }

                if ( videoSourcePlayer.IsRunning )
                {
                    videoSourcePlayer.Stop( );
                }

                videoSourcePlayer.VideoSource = null;
            }
        }

        // New frame received by the player
        private void videoSourcePlayer_NewFrame( object sender, ref Bitmap image )
        {
            DateTime now = DateTime.Now;
            //image = grayscaleFilter.Apply(image)
            //Graphics g = Graphics.FromImage( new Bitmap(grayscaleFilter.Apply(image)) );
            Graphics g = Graphics.FromImage( image );
            //processBlobs(grayscaleFilter.Apply(image));
            //processBlobs(image);
            processBlobsEuclideanFiltering(image); 
            //Bitmap current = grayscaleFilter.Apply(image);
            //string filepath = Environment.CurrentDirectory;
            //string fileName = System.IO.Path.Combine(filepath, @"name.bmp");
            //current.Save(fileName);
            //current.Dispose();

            //processBlobs(image); 

            // paint current time
            SolidBrush brush = new SolidBrush( Color.Red );
            g.DrawString( now.ToString( ), this.Font, brush, new PointF( 5, 5 ) );
            brush.Dispose( );

            g.Dispose( );
        }

        // On timer event - gather statistics
        private void timer_Tick( object sender, EventArgs e )
        {
            IVideoSource videoSource = videoSourcePlayer.VideoSource;

            if ( videoSource != null )
            {
                // get number of frames since the last timer tick
                int framesReceived = videoSource.FramesReceived;

                if ( stopWatch == null )
                {
                    stopWatch = new Stopwatch( );
                    stopWatch.Start( );
                }
                else
                {
                    stopWatch.Stop( );

                    float fps = 1000.0f * framesReceived / stopWatch.ElapsedMilliseconds;
                    fpsLabel.Text = fps.ToString( "F2" ) + " fps";

                    stopWatch.Reset( );
                    stopWatch.Start( );
                }
            }
        }

        private void openActiveWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Rectangle rectangleWindow = new Rectangle();
            RECT rect;
            int length;
            StringBuilder sb;


            Console.WriteLine("You have 2 seconds to make another window active");
            Console.WriteLine("otherwise dimensions will be for this console window");
            Thread.Sleep(2000);

            IntPtr hWnd = GetForegroundWindow();
            length = GetWindowTextLength(hWnd);
            sb = new StringBuilder(length + 1);

            GetWindowText(hWnd, sb, sb.Capacity);
            GetWindowRect(hWnd, out rect);

            Console.WriteLine("\nActive window title is '{0}'", sb.ToString());
            Console.WriteLine("Width is {0} pixels", rect.Width);
            Console.WriteLine("Height is {0} pixels", rect.Height);

            rectangleWindow.Location = new System.Drawing.Point(rect.Left, rect.Top);
            rectangleWindow.Width = rect.Width;
            rectangleWindow.Height = rect.Height;

            OpenVideoSource(new ScreenCaptureStream(rectangleWindow, 0));
        }

        private void processBlobsEuclideanFiltering(Bitmap image)
        {

            // create filter
            EuclideanColorFiltering filter = new EuclideanColorFiltering();
            // set center colol and radius
            RGB tempColor = new RGB();
            tempColor.Red = 240;
            tempColor.Green = 134;
            tempColor.Blue = 61;

            filter.CenterColor = tempColor; 
            filter.Radius = 10;
            // apply the filter
            filter.ApplyInPlace(image);


            // set center colol and radius
            tempColor.Red = 206;
            tempColor.Green = 94;
            tempColor.Blue = 22;

            filter.CenterColor = tempColor;
            filter.Radius = 100;
            // apply the filter
            filter.ApplyInPlace(image);

            BitmapData objectsData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, image.PixelFormat);
            // grayscaling
            UnmanagedImage grayImage = grayscaleFilter.Apply(new UnmanagedImage(objectsData));
            // unlock image
            image.UnlockBits(objectsData);


            //AForge.Math.Vector3 vec = new AForge.Math.Vector3(1, 1, 1);
            //vec.Normalize();
            //grayscaleFilter = new Grayscale(vec.X, vec.Y, vec.Z);

            //Bitmap current = image.Clone() as Bitmap;
            //string filepath = Environment.CurrentDirectory;
            //string fileName = System.IO.Path.Combine(filepath, @"name.bmp");
            //current.Save(fileName);
            //current.Dispose();

            //blobCounter.MinWidth = 5;
            //blobCounter.MinHeight = 5;
            //blobCounter.FilterBlobs = true;
            //blobCounter.ObjectsOrder = ObjectsOrder.Size;
            //blobCounter.ProcessImage(grayImage);
            ////blobCounter.BackgroundThreshold = (Color.FromArgb(20,20,20));
            ////blobCounter.ProcessImage(grayscaleFilter.Apply(image));
            //Rectangle[] rects = blobCounter.GetObjectsRectangles();
            //foreach (Rectangle recs in rects)
            //{
            //    if (rects.Length > 0)
            //    {
            //        foreach (Rectangle objectRect in rects)
            //        {

            //            Graphics g = Graphics.FromImage(image);

            //            using (Pen pen = new Pen(Color.FromArgb(160, 255, 160), 5))
            //            {
            //                g.DrawRectangle(pen, objectRect);
            //            }

            //            g.Dispose();
            //        }

            //    }
            //}


            blobCounter.MinWidth = 5;
            blobCounter.MinHeight = 5;
            blobCounter.FilterBlobs = true;
            blobCounter.ObjectsOrder = ObjectsOrder.Size;
            blobCounter.ProcessImage(grayImage);
            Rectangle[] rects = blobCounter.GetObjectsRectangles();
            foreach (Rectangle recs in rects)
                if (rects.Length > 0)
                {
                    Rectangle objectRect = rects[0];
                    Graphics g = Graphics.FromImage(image);
                    using (Pen pen = new Pen(Color.FromArgb(160, 255, 160), 5))
                    {
                        g.DrawRectangle(pen, objectRect);
                    }
                    g.Dispose();
                }

        }

        private void processBlobs(System.Drawing.Bitmap image)
        {
            // process image with blob counter
            BlobCounter blobCounter = new BlobCounter();

            //blobCounter.BackgroundThreshold = Color.FromArgb(128,128,128);
            blobCounter.ProcessImage(image);
            Blob[] blobs = blobCounter.GetObjectsInformation();

            // create convex hull searching algorithm
            GrahamConvexHull hullFinder = new GrahamConvexHull();

            // lock image to draw on it
            BitmapData data = image.LockBits(
                new Rectangle(0, 0, image.Width, image.Height),
                    ImageLockMode.ReadWrite, image.PixelFormat);

            // process each blob
            foreach (Blob blob in blobs)
            {
                List<IntPoint> leftPoints, rightPoints;
                List<IntPoint> edgePoints = new List<IntPoint>(); 

                // get blob's edge points
                blobCounter.GetBlobsLeftAndRightEdges(blob, out leftPoints, out rightPoints);

                edgePoints.AddRange(leftPoints);
                edgePoints.AddRange(rightPoints);

                // blob's convex hull
                List<IntPoint> hull = hullFinder.FindHull(edgePoints);

                Drawing.Polygon(data, hull, Color.Red);
            }

            image.UnlockBits(data);
        }

        #region User32 calls. 
            [DllImport("user32.dll")]
            static extern IntPtr GetForegroundWindow();

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

            [DllImport("user32.dll")]
            static extern int GetWindowTextLength(IntPtr hWnd);

            [DllImport("user32.dll")]
            static extern int GetWindowRect(IntPtr hWnd, out RECT rect);
        #endregion

        #region User32 Structs
                [StructLayout(LayoutKind.Sequential)]
                public struct RECT
                {
                    public int Left;
                    public int Top;
                    public int Right;
                    public int Bottom;

                    public int Width
                    {
                        get { return Right - Left; }
                    }

                    public int Height
                    {
                        get { return Bottom - Top; }
                    }
                }
            #endregion

    }
}
