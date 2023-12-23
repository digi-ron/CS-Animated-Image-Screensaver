using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
        private static readonly int TARGETFRAMERATE = 25;
        //runtime variables
        private static int counter;
        private static Bitmap loadedImage = null;
        private static Point mouseLoc;
        private static int timerInterval = (int)(1000 / TARGETFRAMERATE);
        //in an ideal world, nothing below this commen should need to be changed for a simple screensaver
        public Main(Rectangle bounds)
        {
            InitializeComponent();
            //set running counter to init value
            counter = COUNTERSTART;
            //set form to match screen bounds
            this.Bounds = bounds;
            //make integrated picturebox match parent
            animatedPictureBox.Width = bounds.Width+OUTERBUFFER;
            animatedPictureBox.Height = bounds.Height+OUTERBUFFER;
            animatedPictureBox.Left = 0;
            animatedPictureBox.Top = 0;
            //start the clock
            animationTimer.Interval = timerInterval;
            animationTimer.Start();
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
                loadedImage = (Bitmap) Properties.Resources.ResourceManager.GetObject(resourceName);
                if(loadedImage == null)
                {
                    //if we don't revert the topmost argument, everything is blocked
                    this.TopMost = false;
                    //if you get to this line, you need to make sure that you have the resources in the right format
                    throw new ArgumentOutOfRangeException("resourceName");
                }
            }
        }

        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            Application.Exit();
        }

        private void animatedPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseLoc.X - e.Location.X > 3 || mouseLoc.Y - e.Location.Y > 3)
            {
                Application.Exit();
            }
            else
            {
                mouseLoc = e.Location;
            }
        }
    }
}
