using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SOAFramework.Library;

namespace SOAFramework.Service.Core.Model
{
    [DataContract]
    public class ServiceModel
    {
        [DataMember(EmitDefaultValue = false)]
        public ServiceInfo ServiceInfo { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public MethodInfo MethodInfo { get; set; }

        public List<IFilter> FilterList { get; set; }

        private static object locker = new object();

        [Execute]
        public ExecuteResult Invoke(IDictionary<string, object> args, ServiceSession session)
        {
            MethodInfo method = this.MethodInfo;
            if (method == null)
            {
                throw new Exception("方法不存在，错误的接口名或者方法！");
            }
            StackFrame frame = (new StackTrace()).GetFrames().FirstOrDefault(t => t.GetMethod().DeclaringType.Equals(typeof(ServiceModel)) &&
                    t.GetMethod().GetCustomAttribute<ExecuteAttribute>(false) != null);
            string sessionid = frame.GetMethod().GetHashCode().ToString();
            ServicePool.Instance.Session[sessionid] = session;
            List<object> listParameters = null;
            List<ParameterInfo> parameters = new List<ParameterInfo>();

            lock (locker)
            {
                parameters.AddRange(method.GetParameters());
                parameters.Sort((l, r) => l.Position - r.Position);
                if (parameters != null && parameters.Count > 0)
                {
                    listParameters = new List<object>();
                    foreach (var parameter in parameters)
                    {
                        object paramValue = null;
                        if (args.Keys.Contains(parameter.Name))
                        {
                            if (args[parameter.Name] is JObject)
                            {
                                var obj = args[parameter.Name] as JObject;
                                paramValue = obj.ToObject(parameter.ParameterType);
                            }
                            else if (args[parameter.Name] is JArray)
                            {
                                var arr = args[parameter.Name] as JArray;
                                var genericArgs = parameter.ParameterType.GetGenericArguments();
                                IList<object> list = arr.ToList(genericArgs[0]);
                                var paramList = Activator.CreateInstance(parameter.ParameterType);
                                MethodInfo addMethod = parameter.ParameterType.GetMethod("Add");
                                foreach (var obj in list)
                                {
                                    addMethod.Invoke(paramList, new object[] { obj.ChangeTypeTo(genericArgs[0]) });
                                }
                                paramValue = paramList;
                            }
                            else
                            {
                                paramValue = args[parameter.Name].ChangeTypeTo(parameter.ParameterType);
                            }
                            listParameters.Add(paramValue);
                        }
                    }
                }
            }
            object[] paramArray = null;
            if (listParameters != null)
            {
                paramArray = listParameters.ToArray();
            }
            try
            {
                var instance = Activator.CreateInstance(method.DeclaringType);
                object result = method.Invoke(instance, paramArray);
                return new ExecuteResult { SessionID = sessionid, Result = result };
            }
            catch (ArgumentException ex)
            {
                StringBuilder msg = new StringBuilder();
                msg.Append("参数不匹配！");
                foreach (var p in parameters)
                {
                    msg.AppendFormat("参数名：{0} 参数类型：{1}", p.Name, p.ParameterType.Name);
                }
                Exception exThrown = new Exception(msg.ToString());
                throw exThrown;
            }
        }
    }

    public class ExecuteResult
    {
        public object Result { get; set; }

        public string SessionID { get; set; }
    }
}
