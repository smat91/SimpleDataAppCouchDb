using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyCouch;

namespace SimpleDataApp
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
            InitViews();
            Application.Run(new Naviagtion());
        }

        private static async void InitViews()
        {
            using (var client = new MyCouchClient("http://admin:admin@localhost:5984/", "testdb"))
            {
                var getResponse = await client.Documents.GetAsync("_design/orders");
                if (getResponse.IsEmpty)
                {
                    var putResponse = await client.Documents.PostAsync(Views.Views.OrderList);
                    Console.WriteLine(putResponse.ToStringDebugVersion());
                }
            }
        }
    }
}
