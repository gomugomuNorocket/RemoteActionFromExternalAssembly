using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Client
{
    public static class ActionScriptManager
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("ActionScriptManager");

        private static Assembly assembly { get; set; }

        private static object instance { get; set; }

        private static Type type { get; set; }
        /// <summary>
        /// Setting the actions script instance
        /// </summary>
        /// <param name="assemblyData"></param>
        /// <param name="typeName"></param>
        public static void SetActionScriptAssemblyData(byte[] assemblyData, string typeName)
        {
            try
            {
                log.DebugFormat("Setting Action script....");

                if (assemblyData == null)
                    throw new Exception(string.Format("Action script assembly data is not defined!"));

                assembly = Assembly.Load(assemblyData);

                if (assembly == null)
                    throw new Exception(string.Format("Failed to create Assembly!"));

                if (string.IsNullOrEmpty(typeName))
                    throw new Exception("Name space is not defined!");

                type = assembly.GetType(typeName);

                if (type == null)
                    throw new Exception(string.Format("Failed to create type of {0}!", typeName));

                instance = Activator.CreateInstance(type);

                if (instance == null)
                    throw new Exception(string.Format("Failed to create Action Script Instance!"));

                log.InfoFormat("Action script instance created successfully.");

            }
            catch (Exception ex)
            {
                log.ErrorFormat("Failed to set ActionsScript with error: '{0}'", ex.GetExceptionMessage());
            }
        }
        /// <summary>
        /// Invoking action from action script instance
        /// </summary>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static ActionScriptResult InvokeAction(string method, object[] parameters = null)
        {
            ActionScriptResult actionScriptResult = new ActionScriptResult();
            try
            {             

                log.InfoFormat("Invoking Action '{0}'....", method);

                actionScriptResult.Action = method;

                if (string.IsNullOrEmpty(method))
                    throw new Exception("Method name is not defined!");

                if (instance == null)
                    throw new Exception(string.Format("Action Script Instance is not defined!"));

                if (type == null)
                    throw new Exception(string.Format("Namespace is not defined!"));

                var methodInfo = type.GetMethod(method);

                if(methodInfo == null)
                    throw new Exception(string.Format("Method '{0}' doesn't exist!", method));

                ParameterInfo[] parameterDefinition = methodInfo.GetParameters();
                if (parameterDefinition.Count() > 0)
                {
                    if (parameterDefinition.Count() != parameters.Count())
                        throw new Exception("The number of parameters doesn't match!");

                    for (int i = 0; i < parameterDefinition.Count(); i++)
                    {
                        ParameterInfo info = parameterDefinition[i];
                        string _type = string.Empty;
                        try
                        {
                            _type = string.Format("System.{0}", info.ParameterType.Name);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(string.Format("Failed to get parameter type for: '{0}'!", info.ParameterType.Name));
                        }
                        try
                        {
                            parameters[i] = Convert.ChangeType(parameters[i], Type.GetType(_type));
                        }
                        catch(Exception ex)
                        {
                            throw new Exception(string.Format("Invalid cast parameter: '{0}' to type: '{1}'", parameters[i], _type));
                        }
                    }
                }     

                methodInfo.Invoke(instance, parameters);
                actionScriptResult.Result = Result.Success;

            }
            catch (Exception ex)
            {
                actionScriptResult.Result = Result.Failed;
                actionScriptResult.FailReason = ex.GetExceptionMessage();
            }

            actionScriptResult.GetResultMessage();

            return actionScriptResult;
        }
        /// <summary>
        /// Dispose
        /// </summary>
        public static void Dispose()
        {
            try
            {
                assembly = null;
                instance = null;
                type = null;
                GC.Collect();

            }
            catch (Exception ex)
            {
                log.ErrorFormat("Dispose failed with error: '{0}'", ex.GetExceptionMessage());
            }
          
        }



    }
}
