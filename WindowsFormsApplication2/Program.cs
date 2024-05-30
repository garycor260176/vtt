using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    static class Program
    {
        static void GetItemPortion()
        {
            servVTT vtt = new servVTT(config.Read(), "GetItemPortion");
            vtt.GetItemPortion_serv(true);
        }

        static void GetCategories()
        {
            servVTT vtt = new servVTT(config.Read(), "GetCategories");
            vtt.GetCategories_serv(true);
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length> 0 && args[0] == "/hide")
            {
                GetCategories();
                GetItemPortion();
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
