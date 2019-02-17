using Client.Hub;
using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public static class HostManager
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("HostManager");

        private static HubClientBase hubClient { get; set; }

        public static void Start()
        {
            try
            {
                if (string.IsNullOrEmpty(ClientManager.ModuleSettings.Endpoint))
                    throw new Exception("Endpoint is not defined!");

                hubClient = new HubClientBase(ClientManager.ModuleSettings.Endpoint);
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
                hubClient.Stop();
                GC.Collect();
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Failed to Stop with error: '{0}'", ex.GetExceptionMessage());
            }
        }
    }
}
