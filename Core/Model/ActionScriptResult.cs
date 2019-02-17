using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    /// <summary>
    /// Result enum status
    /// </summary>
    public enum Result
    {
        Success = 1,
        Failed = 2
    }
    /// <summary>
    /// Action result
    /// </summary>
    public class ActionScriptResult
    {
        public Result? Result { get; set; }

        public string FailReason { get; set; }

        public string Action { get; set; }

        private readonly log4net.ILog log = log4net.LogManager.GetLogger("ActionScriptResult");

        public void GetResultMessage()
        {
            if (Result == Core.Result.Success)
            {
                log.InfoFormat("Client successfully executed method: '{0}'", Action);
            }
            else if (Result == Core.Result.Failed)
            {
                log.ErrorFormat("Client failed to execute method: '{0}' with error: '{1}'", Action, FailReason);
            }
            else
            {
                throw new Exception("Result type is unknown.");
            }
        }

    }
}
