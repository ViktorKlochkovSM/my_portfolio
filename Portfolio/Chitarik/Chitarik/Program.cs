using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Chitarik
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            SerializeStatic.Load(typeof(Settings), Settings.FileNameSettings);
            Application.Run(new Form2());
        }
    }
}
