using Core;
using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public static class HostManager
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("HostManager");

        private static IDisposable server { get; set; }

        public static void Start()
        {
            try
            {
                if (string.IsNullOrEmpty(ServerManager.ModuleSettings.Endpoint))
                    throw new Exception("Endpoint is not defined!");

                server = WebApp.Start<Startup>(ServerManager.ModuleSettings.Endpoint);
                log.InfoFormat("Server running on {0}...", ServerManager.ModuleSettings.Endpoint);
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Failed to Start with error: '{0}'", ex.GetExceptionMessage());
            }
        }
        public static void Stop()
        {
            try
            {
                if (server != null)
                {
                    server.Dispose();
                    log.InfoFormat("Server stoped.");
                }
                GC.Collect();
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Failed to Stop with error: '{0}'", ex.GetExceptionMessage());
            }
        }
    }
}
