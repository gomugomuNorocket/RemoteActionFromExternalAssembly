using Client.Model;
using Core;
using Core.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class ClientManager
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("ClientManager");

        public static ModuleSettings ModuleSettings { get; set; }

        public static ActionScriptSettings ActionScriptSettings { get; set; }

        public static void Initalize()
        {
            try
            {
                log.Debug("Initializing module settings....");
                ModuleSettings = (ModuleSettings)(dynamic)ConfigurationManager.GetSection("moduleSettings");
                log.InfoFormat("Module {0} initialized successfully.", ModuleSettings.Name);
            }
            catch (Exception ex)
            {
                string _error = string.Format("Failed to initialize module settings with error: '{0}'", ex.GetExceptionMessage());
                log.ErrorFormat(_error);
                throw new Exception(_error);
            }

            try
            {
                log.Debug("Initializing actions script settings....");
                ActionScriptSettings = (ActionScriptSettings)(dynamic)ConfigurationManager.GetSection("actionScriptSettings");
                log.Info("Action script settings initialized successfully.");
            }
            catch (Exception ex)
            {
                string _error = string.Format("Failed to initialize action script settings with error: '{0}'", ex.GetExceptionMessage());
                log.ErrorFormat(_error);
                throw new Exception(_error);
            }
        }
    }
}
