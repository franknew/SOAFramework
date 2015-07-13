using Newtonsoft.Json.Linq;
using SOAFramework.Service.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SOAFramework.Library;
using System.IO;

namespace SOAFramework.Service.Core.Model
{
    public static class ServiceModelExtension
    {
        public static object Invoke(this ServiceModel model, IDictionary<string, object> args)
        {
            MethodInfo method = model.MethodInfo;
            if (method == null)
            {
                throw new Exception("方法不存在，错误的接口名或者方法！");
            }
            var instance = Activator.CreateInstance(method.DeclaringType);
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.AddRange(method.GetParameters());
            parameters.Sort((l, r) => l.Position - r.Position);
            List<object> listParameters = null;
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
                    }
                    listParameters.Add(paramValue);
                }
            }
            object[] paramArray = null;
            if (listParameters != null)
            {
                paramArray = listParameters.ToArray();
            }
            try
            {
                object result = method.Invoke(instance, paramArray);
                return result;
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

        public static Stream ToStream(this ServerResponse response, bool zipped = false)
        {
            Stream stream = null;
            string zippedjson = null;
            string json = null;
            //if (response.IsError)
            //{
                json = JsonHelper.Serialize(response);
            //}
            //else
            //{
            //    json = JsonHelper.Serialize(response.Data);
            //}
            if (zipped)
            {
                zippedjson = ZipHelper.Zip(json);
            }
            else
            {
                zippedjson = json;
            }
            //zippedjson = json;
            stream = new MemoryStream(Encoding.UTF8.GetBytes(zippedjson));
            return stream;
        }
    }
}
