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

    }
}
