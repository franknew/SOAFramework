﻿namespace AustinHarris.JsonRpc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using AustinHarris.JsonRpc;

    public static class ServiceBinder
    {
        public static void BindService<T>(string sessionID, Func<T> serviceFactory)
        {
            var instance = serviceFactory();
            var item = instance.GetType(); // var item = typeof(T);
            BindService(sessionID, item);
        }
        public static void BindService<T>(string sessionID) where T : new()
        {
            BindService(sessionID, () => new T());
        }
        public static void BindService<T>() where T : new()
        {
            BindService<T>(Handler.DefaultSessionId());
        }

        public static void BindService(string sessionID, Type type)
        {
            BindService(sessionID, type, typeof(JsonRpcClassAttribute));
        }

        public static void BindService<T>(string sessionID, Type type) where T : Attribute
        {
            BindService(sessionID, type, typeof(T));
        }

        public static void BindService(string sessionID, Type type, Type attr)
        {
            var rpctype = type.GetCustomAttributes(attr, true);
            if (rpctype == null || rpctype.Length == 0) return;
            var instance = Activator.CreateInstance(type);
            //var regMethod = typeof(Handler).GetMethod("Register");

            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(m => m.GetCustomAttributes(typeof(JsonRpcMethodAttribute), false).Length > 0);
            foreach (var meth in methods)
            {
                InitMethodHandler(sessionID, meth, instance);
            }
        }

        public static Handler InitMethodHandler(string sessionID, MethodInfo method, object instance)
        {
            Dictionary<string, Type> paras = new Dictionary<string, Type>();
            Dictionary<string, object> defaultValues = new Dictionary<string, object>(); // dictionary that holds default values for optional params.

            var paramzs = method.GetParameters();

            List<Type> parameterTypeArray = new List<Type>();
            for (int i = 0; i < paramzs.Length; i++)
            {
                // reflection attribute information for optional parameters
                //http://stackoverflow.com/questions/2421994/invoking-methods-with-optional-parameters-through-reflection
                paras.Add(paramzs[i].Name, paramzs[i].ParameterType);

                if (paramzs[i].IsOptional) // if the parameter is an optional, add the default value to our default values dictionary.
                    defaultValues.Add(paramzs[i].Name, paramzs[i].DefaultValue);
            }

            var resType = method.ReturnType;
            paras.Add("returns", resType); // add the return type to the generic parameters list.

            var handlerSession = Handler.GetSessionHandler(sessionID);

            var atdata = method.GetCustomAttributes(typeof(JsonRpcMethodAttribute), false);
            foreach (JsonRpcMethodAttribute handlerAttribute in atdata)
            {
                var methodName = handlerAttribute.JsonMethodName == string.Empty ? method.DeclaringType.FullName + "." + method.Name : handlerAttribute.JsonMethodName;
                var newDel = Delegate.CreateDelegate(System.Linq.Expressions.Expression.GetDelegateType(paras.Values.ToArray()), instance /*Need to add support for other methods outside of this instance*/, method);
                handlerSession.Register(methodName, newDel);
                //regMethod.Invoke(handlerSession, new object[] { methodName, newDel });
                handlerSession.MetaData.AddService(methodName, paras, defaultValues);
            }
            return handlerSession;
        }

        public static void BindService(string sessionID, Assembly ass)
        {
            var types = ass.GetTypes();
            foreach (var type in types)
            {
                BindService(sessionID, type);
            }
        }

        public static void BindService(string sessionID, AppDomain domain = null)
        {
            if (domain == null) domain = AppDomain.CurrentDomain;
            var ass = domain.GetAssemblies();
            foreach (var a in ass)
            {
                BindService(sessionID, a);
            }
        }
    }
}