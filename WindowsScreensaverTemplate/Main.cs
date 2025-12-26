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
        //runtime variables
        private static int counter;
        private int lastCounter;
        private static readonly Bitmap loadedImage = null;
        private static Point mouseLoc;
        private static readonly int timerInterval = (int)Math.Round((decimal)(1000 / Config.TARGETFRAMERATE));
        private Rectangle windowBounds = Rectangle.Empty;
        private static bool previewMode = false;
        private readonly int threadNum;
        private Bitmap[] scrImages;
        //in an ideal world, nothing below this comment should need to be changed for a simple screensaver
        public Main(Rectangle bounds, int threadIndex, Bitmap[] imgArr)
        {
            InitializeComponent();
            windowBounds = bounds;
            threadNum = threadIndex;
            if (imgArr != null)
            {
                scrImages = imgArr;
            }
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
            counter = Config.COUNTERSTART;
            //set form to match screen bounds
            if(windowBounds != Rectangle.Empty)
            {
                this.Bounds = windowBounds;
                //make integrated picturebox match parent
                animatedPictureBox.Width = windowBounds.Width + Config.OUTERBUFFER;
                animatedPictureBox.Height = windowBounds.Height + Config.OUTERBUFFER;
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
            Bitmap loadedImage;
            if (scrImages == null)
            {
                while (lastCounter == counter) { }
                lastCounter = counter;
                Bitmap curr = (Bitmap)Properties.Resources.ResourceManager.GetObject(Config.GetAssetString(counter));
                if (curr == null)
                {
                    counter = Config.COUNTERSTART;
                    curr = (Bitmap)Properties.Resources.ResourceManager.GetObject(Config.GetAssetString(counter));
                    if (curr == null)
                    {
                        throw new ArgumentException("Failure to load from Resource Manager. Ensure images are added to solution!");
                    }
                }
                loadedImage = curr;
            }
            else
            {
                while (lastCounter == counter) { }
                lastCounter = counter;
                if (counter < scrImages.Length - 1)
                {
                    counter++;
                }
                else
                {
                    counter = Config.COUNTERSTART;
                }
                loadedImage = scrImages[counter];
            }
            
            animatedPictureBox.Image = loadedImage;
            animatedPictureBox.Update();
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
                if (mouseLoc.X - e.Location.X > Config.MOUSEMOVESENSITIVITY || mouseLoc.Y - e.Location.Y > Config.MOUSEMOVESENSITIVITY)
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
