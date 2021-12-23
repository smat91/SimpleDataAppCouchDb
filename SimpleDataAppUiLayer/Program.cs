using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using SimpleDataAppDataLayer;

namespace SimpleDataAppUiLayer
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
#if DEBUG
            NativeMethods.AllocConsole();
            Console.WriteLine("Debug Console");
#endif
            DataAccess.InitViews();
            Application.Run(new Naviagtion());
#if DEBUG
            NativeMethods.FreeConsole();
#endif
        }
    }
}
