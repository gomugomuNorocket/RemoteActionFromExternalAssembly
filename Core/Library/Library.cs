using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public static class Library
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("Library");
        public static string GetExceptionMessage(this Exception exception)
        {
            try
            {
                return exception.GetBaseException().Message;
            }
            catch (Exception ex)
            {
                string _error = "Failed to get exception base message";
                log.ErrorFormat(_error);
                return _error;
            }

        }
        public static string GetExceptionMessage(this AggregateException exception)
        {
            try
            {
                return string.Join(",", exception.InnerExceptions.Select(x => x.GetBaseException().Message));
            }
            catch (Exception ex)
            {
                string _error = "Failed to get exception base message";
                log.ErrorFormat(_error);
                return _error;
            }

        }
    }
}
