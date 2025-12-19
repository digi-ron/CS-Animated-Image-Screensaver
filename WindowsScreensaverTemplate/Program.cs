using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args.Length > 0)
            {
                string mainArgument = "";
                string secondaryArgument = "";
                if (args[0].Contains(":"))
                {
                    string[] newArgs = args[0].Split(':');
                    mainArgument = newArgs[0].Trim().ToLower();
                    secondaryArgument = newArgs[1].Trim();
                } else
                {
                    mainArgument = args[0].Trim().ToLower();
                }
                if(mainArgument == "/p")
                {
                    //TODO: try to fix this, may be a result of the picturebox but it doesn't render in child previews
                    //Application.Run(new Main(new IntPtr(long.Parse(secondaryArgument))));
                } else if (mainArgument == "/c")
                {
                    //insert any code you would like for a config screen
                } else
                {
                    Start();
                }
            } else
            {
                Start();
            }
        }

        static void Start()
        {
            //this will display a instance of the screensaver on each active screen
            foreach (Screen scr in Screen.AllScreens)
            {
                int index = Array.IndexOf(Screen.AllScreens, scr);
                //Application.Run(new Main(scr.Bounds));
                var thread = new Thread(() =>
                {
                    var form = new Main(scr.Bounds, index);
                    form.StartPosition = FormStartPosition.Manual;
                    form.Bounds = scr.Bounds;
                    Application.Run(form);
                });
                thread.Start();
            }
        }
    }
}
