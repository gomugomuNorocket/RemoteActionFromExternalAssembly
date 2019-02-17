using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger("Program");

        static void Main(string[] args)
        {
            log.Info("Starting...Press 'Exit' to exit.");
            ///Initalize Manager
            ClientManager.Initalize();
            ///Starting the client connection
            HostManager.Start();           

            while (true)
            {
                ///Exit for closing
                string input = Console.ReadLine();
                if (input == "Exit") { 
                    break;
                }
            }
            ///Stoping the client connection
            HostManager.Stop();

            ActionScriptManager.Dispose();

            log.Info("Closing...");
        }
    }
}
