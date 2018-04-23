using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace WCF_TCP_Chat
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Service";
            ServiceHost host = new ServiceHost(typeof(ChatService));
            host.Open();
            Console.WriteLine("\nChat is running ...\n");
            Console.WriteLine("Press Enter to stop the service\n");
            Console.ReadKey();
            host.Close();
        }
    }
}