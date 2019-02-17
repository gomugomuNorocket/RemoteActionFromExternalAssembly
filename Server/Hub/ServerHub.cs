using Core;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Owin.Cors;
using Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Hub
{
    [HubName("ServerHub")]
    public class ServerHub : Microsoft.AspNet.SignalR.Hub
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("ServerHub");

        #region Events
        /// <summary>
        /// OnConnected event
        /// </summary>
        /// <returns></returns>
        public override Task OnConnected()
        {
            log.InfoFormat("Client Connected...");
            return base.OnConnected();
        }
        /// <summary>
        /// OnDisconnected event
        /// </summary>
        /// <param name="stopCalled"></param>
        /// <returns></returns>
        public override Task OnDisconnected(bool stopCalled)
        {
            log.InfoFormat("Client Disconnected...");
            return base.OnDisconnected(stopCalled);
        }
        /// <summary>
        /// OnReconnected event
        /// </summary>
        /// <returns></returns>
        public override Task OnReconnected()
        {
            log.InfoFormat("Client Reconnected...");
            return base.OnReconnected();
        }
        #endregion

        #region Methods
        ///// <summary>
        ///// Client joins group
        ///// </summary>
        ///// <param name="group"></param>
        //public void JoinGroup(string group)
        //{
        //    try
        //    {
        //        log.InfoFormat("Joining group: '{0}'....", group);
        //        this.Groups.Add(this.Context.ConnectionId, group);
        //        log.InfoFormat("'Client joined group: '{0}' successfully.", group);
        //    }
        //    catch (Exception ex)
        //    {
        //        log.ErrorFormat("'JoinGroup' Method failed with error: '{0}'", ex.GetExceptionMessage());
        //    }
        //}
        ///// <summary>
        ///// Client lieaves group
        ///// </summary>
        ///// <param name="group"></param>
        //public void LeaveGroup(string group)
        //{
        //    try
        //    {
        //        log.InfoFormat("Requested 'LeaveGroup' Method with params = group: '{0}'.", group);
        //        this.Groups.Remove(this.Context.ConnectionId, group);
        //        log.InfoFormat("'LeaveGroup' Method with params = group: '{0}' finished successfully.", group);
        //    }
        //    catch (Exception ex)
        //    {
        //        log.ErrorFormat("'LeaveGroup' Method with params = group: '{0}' failed with error: '{2}'", group, ex.GetExceptionMessage());
        //    }
        //}
        /// <summary>
        /// Client request to load action script assembly data
        /// </summary>
        public void RequestActionScript()
        {
            try
            {
                log.InfoFormat("Client requested action script....");

                string path = Path.GetFullPath(ServerManager.ActionScriptSettings.Path.Replace("{RootPath}", ServerManager.ModuleSettings.RootPath));

                log.DebugFormat("Retrieving actions script assembly data....");

                byte[] data = System.IO.File.ReadAllBytes(path);

                log.DebugFormat("Actions script assembly loaded successfully.");

                log.DebugFormat("Sending actions script assembly data....");

                Clients.Caller.SetActionScriptAssemblyData(data, ServerManager.ActionScriptSettings.Namespace);

                log.InfoFormat("Actions script assembly data sent to client.");

            }
            catch (Exception ex)
            {
                log.ErrorFormat("'RequestActionScript' Method failed with error: '{0}'", ex.GetExceptionMessage());
            }

        }
        /// <summary>
        /// Client informs server for the action result
        /// </summary>
        /// <param name="result"></param>
        public void SetActionResultResponse(ActionScriptResult result)
        {
            try
            {
                if(result.Result == Result.Success)
                {
                    log.InfoFormat("Client successfully executed method: '{0}'", result.Action);
                }
                else if (result.Result == Result.Failed)
                {
                    log.ErrorFormat("Client failed to execute method: '{0}' with error: '{1}'", result.Action, result.FailReason);
                }
                else
                {
                    throw new Exception("Result type is unknown.");
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("'SetActionResultResponse' Method failed with error: '{0}'", ex.GetExceptionMessage());
            }
        }




        /// <summary>
        /// Client request action commands
        /// </summary>
        public void RequestActionScriptCommands()
        {
            try
            {
                Task.Run(() => {
                    foreach (Server.Model.Action action in ServerManager.ActionScriptSettings.Actions)
                    {
                        Thread.Sleep(ServerManager.ActionScriptSettings.DelayBeforeAction);
                        log.InfoFormat("Invoking action: '{0}' with parameters: '{1}' to caller client.", action.Method, string.Join(", ", action.Params));
                        Clients.Caller.invokeAction(action.Method, action.Params.ToArray());
                    }
                });
                
            }
            catch (Exception ex)
            {
                log.ErrorFormat("'RequestActionScriptCommands' Method failed with error: '{0}'", ex.GetExceptionMessage());
            }            
        }


        public static void TriggerActionScriptMethodFromUserInput(string userInput)
        {
            try
            {
                string method = string.Empty;
                List<object> parameters = new List<object>();

                try
                {                   
                    string[] options = userInput.Split(' ');

                    for (var i = 0; i < options.Length; i++)
                    {
                        if (i == 0)
                            method = options[i];
                        else
                        {
                            if(!string.IsNullOrEmpty(options[i]))
                                parameters.Add(options[i]);
                        }
                    }


                    log.DebugFormat("User requested the action: '{0}' with parameters: '{1}'", method, string.Join(", ", parameters));
                }
                catch(Exception ex)
                {
                    throw new Exception(string.Format("Failed to extract method and parameters from user input: '{0}' with error: '{1}'", userInput, ex.GetExceptionMessage()));
                }

                try
                {
                    if (!string.IsNullOrEmpty(method))
                    {
                        IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<ServerHub>();
                        log.InfoFormat("Invoking action: '{0}' with parameters: '{1}' to all connected clients.", method, string.Join(", ", parameters));
                        hubContext.Clients.All.invokeAction(method, parameters.ToArray());
                    }
                }
                catch(Exception ex)
                {
                    throw new Exception(string.Format("Failed to invoke client action with error: '{0}'", ex.GetExceptionMessage()));
                }

            }
            catch (Exception ex)
            {
                log.ErrorFormat("'TriggerActionScriptMethod' Method failed with error: '{0}'", ex.GetExceptionMessage());
            }
        }

      
        #endregion
    }
}
