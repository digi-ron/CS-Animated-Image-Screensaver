using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsScreensaverTemplate
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //this will display a instance of the screensaver on each active screen
            foreach(Screen scr in Screen.AllScreens)
            {
                Application.Run(new Main(scr.Bounds));
            }
        }
    }
}
