using Microsoft.AspNet.SignalR;
using Server.Hub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger("Program");

       

        static void Main(string[] args)
        {
            log.Info("Starting...Press 'Exit' to exit.");
            log.Info("You can insert command with the following format: MethodName param1 param2 ...");
            
            ///Initalize Manager
            ServerManager.Initalize();
            ///Starting the server
            HostManager.Start();

            string _configuredCommands = "The preconfigured actions are ";
            foreach(Server.Model.Action action in ServerManager.ActionScriptSettings.Actions)
            {
                _configuredCommands += string.Format("\n {0} \t with {1} parameter(s)", action.Method, action.Params.Count());
            }
            log.Info(_configuredCommands);

            while (true)
            {      
                string input = Console.ReadLine();
                ///Exit for closing
                if (input == "Exit")
                {
                    break;
                }
                else
                {
                    ServerHub.TriggerActionScriptMethodFromUserInput(input);
                }

            }
            ///Stoping the server
            HostManager.Stop();

            log.Info("Closing...");
        }
    }
}
