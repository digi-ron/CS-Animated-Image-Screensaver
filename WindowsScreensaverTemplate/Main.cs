using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsScreensaverTemplate
{
    public partial class Main : Form
    {
        //developer variables
        private static readonly string FRAMENUMFORMAT = "000";
        private static readonly string FRAMEPREFIX = "frame_";
        private static readonly int OUTERBUFFER = 2;
        private static readonly int COUNTERSTART = 1;
        private static readonly int MOUSEMOVESENSITIVITY = 3;
        private static readonly int TARGETFRAMERATE = 30;
        //runtime variables
        private static int counter;
        private static Bitmap loadedImage = null;
        private static Point mouseLoc;
        private static int timerInterval = (int)Math.Round((decimal)(1000 / TARGETFRAMERATE));
        private Rectangle windowBounds = Rectangle.Empty;
        private static bool previewMode = false;
        private int threadNum;
        //in an ideal world, nothing below this comment should need to be changed for a simple screensaver
        public Main(Rectangle bounds, int threadIndex)
        {
            InitializeComponent();
            windowBounds = bounds;
            threadNum = threadIndex;
            Init();
        }

        public Main(IntPtr windowHandler)
        {
            InitializeComponent();
            SetParent(this.Handle, windowHandler);
            SetWindowLong(this.Handle, -16, new IntPtr(GetWindowLong(this.Handle, -16) | 0x40000000));
            Rectangle parent;
            GetClientRect(windowHandler, out parent);
            Size = parent.Size;
            Location = new Point(0, 0);
            previewMode = true;
            Init();
        }

        //dll imports
        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr child, IntPtr parent);
        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr window, int index, IntPtr newLong);
        [DllImport("user32.dll")]
        static extern int GetWindowLong(IntPtr window, int index);
        [DllImport("user32.dll")]
        static extern bool GetClientRect(IntPtr window, out Rectangle rectangle);


        private void Init()
        {
            //set running counter to init value
            counter = COUNTERSTART;
            //set form to match screen bounds
            if(windowBounds != Rectangle.Empty)
            {
                this.Bounds = windowBounds;
                //make integrated picturebox match parent
                animatedPictureBox.Width = windowBounds.Width + OUTERBUFFER;
                animatedPictureBox.Height = windowBounds.Height + OUTERBUFFER;
                animatedPictureBox.Left = 0;
                animatedPictureBox.Top = 0;

            } else
            {
                previewMode = true;
            }
            
            //start the clock
            animationTimer.Interval = timerInterval;
            
            animationTimer.Start();
            Cursor.Hide();
        }

        private void animationTimer_Tick(object sender, EventArgs e)
        {
            string resourceName = $"{FRAMEPREFIX}{counter.ToString(FRAMENUMFORMAT)}";
            //resource comes back as Object, cast to Bitmap
            loadedImage = (Bitmap)Properties.Resources.ResourceManager.GetObject(resourceName);
            if(loadedImage == null)
            {
                counter = COUNTERSTART;
                resourceName = $"{FRAMEPREFIX}{counter.ToString(FRAMENUMFORMAT)}";
                if (threadNum == 0)
                {
                    loadedImage = (Bitmap)Properties.Resources.ResourceManager.GetObject(resourceName);
                    if (loadedImage == null)
                    {
                        //if we don't revert the topmost argument, everything is blocked
                        this.TopMost = false;
                        //if you get to this line, you need to make sure that you have the resources in the right format
                        throw new ArgumentOutOfRangeException(resourceName);
                    }
                }
                else
                {
                    while (loadedImage == null)
                    {
                        Console.WriteLine("Waiting on main thread");
                    }
                }
            }
            animatedPictureBox.Image = loadedImage;
            animatedPictureBox.Update();
            if (threadNum == 0) { counter++; }
        }

        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            if(!previewMode)
            {
                Application.Exit();
            }
        }

        private void animatedPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if(!previewMode)
            {
                if (mouseLoc.X - e.Location.X > MOUSEMOVESENSITIVITY || mouseLoc.Y - e.Location.Y > MOUSEMOVESENSITIVITY)
                {
                    Application.Exit();
                }
                else
                {
                    mouseLoc = e.Location;
                }
            }
        }

        private void animatedPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if(!previewMode)
            {
                Application.Exit();
            }
        }
    }
}
