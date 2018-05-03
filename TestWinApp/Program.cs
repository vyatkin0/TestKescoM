using System;
using System.ServiceModel.Web;

namespace TestWinApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Service started");
            WebServiceHost webHost = null;
            try
            {
                webHost = new WebServiceHost(
                typeof(TestWinService)
                );
                webHost.Open();
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                if (webHost != null)
                    webHost.Abort();
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
