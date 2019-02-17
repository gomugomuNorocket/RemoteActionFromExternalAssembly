using Core;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Hub
{
    public class HubClientBase
    {
        private readonly log4net.ILog log = log4net.LogManager.GetLogger("HubClientBase");

        private HubConnection connection { get; set; }

        IHubProxy hub { get; set; }

        public HubClientBase(string url)
        {
            connection = new HubConnection(url);
            hub = connection.CreateHubProxy("ServerHub");

            log.DebugFormat("Connecting to {0}...", url);
            ///Register on events
            RegisterOnServerEvents();
            ///Starting the connection
            connection.Start().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    log.ErrorFormat("There was an error opening the connection: {0}", task.Exception.GetExceptionMessage());
                }
                else
                {
                    log.Info("Connected successfully.");

                    log.Info("Requesting action script...");
                    ///Request assembly data from server
                    this.CallServerMethod("RequestActionScript");

                   
                }
            }).Wait();
            ///Setting the events
            connection.ConnectionSlow += ConnectionSlow;
            connection.Reconnected += Reconnected;
            connection.Reconnecting += Reconnecting;


        }
        /// <summary>
        /// ConnectionSlow event
        /// </summary>
        private void ConnectionSlow()
        {
            log.Info("Connection Slow...");
        }
        /// <summary>
        /// Reconnected event
        /// </summary>
        private void Reconnected()
        {
            log.Info("Reconnected...");
        }
        /// <summary>
        /// Reconnecting event
        /// </summary>
        private void Reconnecting()
        {
            log.Info("Reconnecting...");
        }
        /// <summary>
        /// Register on server events
        /// </summary>
        private void RegisterOnServerEvents()
        {
            hub.On<byte[], string>("SetActionScriptAssemblyData", (assembly, typeName) =>
            {
                ActionScriptManager.SetActionScriptAssemblyData(assembly, typeName);

                if (ClientManager.ActionScriptSettings.RunImmediately)
                {
                    log.Info("Requesting server preconfigured actions...");
                    ///Request action script commands
                    this.CallServerMethod("RequestActionScriptCommands");
                }
            });

            hub.On<string, object[]> ("invokeAction", (method, parameters) =>
            {
                
                ActionScriptResult result = ActionScriptManager.InvokeAction(method, parameters);
               
                ///Informs the server about the result
                this.CallServerMethod("SetActionResultResponse", result);

            });
           
        }
        /// <summary>
        /// Stop
        /// </summary>
        public void Stop()
        {
            if(connection != null)
                connection.Stop();
        }
        /// <summary>
        /// Call a server side method
        /// </summary>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        public void CallServerMethod(string method, params object[] parameters)
        {
            hub.Invoke<string>(method, parameters).ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    log.ErrorFormat("Calling method: '{1}' failed with error: {0}", task.Exception.GetExceptionMessage(), method);
                }
                else
                {
                    //log.DebugFormat("Method '{0}' called successfully.", method);
                }
            });
        }
    }
}
