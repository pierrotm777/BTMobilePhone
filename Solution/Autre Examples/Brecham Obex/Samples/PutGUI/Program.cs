using System;
using System.Windows.Forms;

namespace PutGuiCs
{

    static class Program
    {
        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
#if ! NETCF
        [STAThread]
#endif
        static void Main()
        {
#if ! NETCF
            Application.EnableVisualStyles();
#if ! FX1_1
            Application.SetCompatibleTextRenderingDefault(false);
#endif  // ! NETCF
#endif  // ! FX1_1
            Application.Run(new FormPutGuiCs());
        }//fn

    }//class
}