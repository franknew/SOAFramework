﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using AustinHarris.JsonRpc;
using SOAFramework.Library;

namespace MicroService.Library
{
    public class NodeServer
    {
        private string url;
        const string sessionName = "__nodeMethods";
        private HttpServer httpServer;

        public ServerStatus Status
        {
            get { return httpServer.Status; }
            set { httpServer.Status = value; }
        }

        public string Url
        {
            get
            {
                return url;
            }
        }

        public string CommonDllPath { get; set; }

        public NodeServer(string url, string commonDllPath = null)
        {
            this.url = url;
            CommonDllPath = commonDllPath;
            httpServer = new HttpServer(new string[] { this.url });
            //AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        public NodeServer(string commonDllPath = null)
        {
            CommonDllPath = commonDllPath;
            //AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        public NodeServer()
        {
            //AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        private string HttpServer_Executing(object sender, HttpExcutingEventArgs e)
        {
            string responseString = "";
            var type = ContentTypeEnum.Json;
            FilterHandler filterHandler = null;
            ISerializable serializor = null;
            string action = null;
            HttpServerContext httpContext = null;

            try
            {
                string rawUrl = e.Request.RawUrl.TrimEnd('/');
                int start = rawUrl.LastIndexOf("/");
                int end = rawUrl.LastIndexOf("?");
                if (end == -1) end = rawUrl.Length;
                action = rawUrl.Substring(start + 1, end - start - 1);
                httpContext = new HttpServerContext
                {
                    HttpContext = e.Context,
                    ServiceInfo = new ServiceModelInfo
                    {
                        ServiceName = action,

                    },
                    Args = new Dictionary<string, object>(),
                };
                type = ContentTypeConvert.Convert(e.Request.ContentType);
                serializor = SerializeFactory.Create(type);
                filterHandler = FilterHandler.GetSessionHandler(sessionName);
                string post;
                using (StreamReader reader = new StreamReader(e.Request.InputStream, Encoding.UTF8))
                {
                    post = reader.ReadToEnd();
                }
                Dictionary<string, object> args = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(post)) args = serializor.Deserialize<Dictionary<string, object>>(post);
                if (args == null) args = new Dictionary<string, object>();
                foreach (var key in e.Request.QueryString.AllKeys)
                {
                    if (!args.ContainsKey(key)) args[key] = e.Request.QueryString[key];
                }
                httpContext.Args = args;
                HttpServerContext.AddContext(Thread.CurrentContext.ContextID.ToString(), httpContext);
                JsonResponse response = null;
                var result = filterHandler.HandleOnActionExecuting(action, httpContext);
                if (!result.IsError)
                {

                    response = JsonRpcProcessor.ProcessReturnResponse(sessionName, new JsonRequest
                    {
                        Method = action,
                        Params = args,
                    }).FirstOrDefault();
                    response.Success = response.Error == null;
                    if (!response.Success) throw response.Error;
                    result = filterHandler.HandleOnActionExecuted(action, httpContext);
                }
                else response = new JsonResponse { Error = new JsonRpcException(result.Code, result.ErrorMessage, null) };
                response.Success = !result.IsError;
                if (response == null) responseString = "";
                else if (response.Success) responseString = serializor.Serialize(response.Result);
                else responseString = serializor.Serialize(response);
            }
            catch (Exception ex)
            {
                filterHandler.HandleOnException(action, httpContext, ex);
                JsonResponse response = new JsonResponse();
                response.Success = false;
                response.Error = new JsonRpcException(100, ex.Message, ex.InnerException);
                responseString = serializor.Serialize(response);
            }
            finally
            {
                HttpServerContext.RemoveContext(Thread.CurrentContext.ContextID.ToString());
            }
            return responseString;
        }

        public void Start(string url)
        {
            Start(url, ServerType.Server);
        }

        public void Start()
        {
            Start(null, ServerType.Server);
        }

        public void Start(string url , ServerType serverType)
        {
            try
            {
                ServiceBinder.BindService(sessionName);
                FilterBinder.BindFilter(sessionName);

                if(serverType== ServerType.Timing)
                {
                    new TimingTasksManager().StartTiming();//启动任务监听
                    TimingTasksBind.BindTask();//绑定任务
                }
              
                if (!string.IsNullOrEmpty(url)) this.url = url;
                if (httpServer == null) httpServer = new HttpServer(new string[] { this.url });
                httpServer.Executing += HttpServer_Executing;
                httpServer.Start();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Stop()
        {
            httpServer.Stop();
        }

        public void Close()
        {
            httpServer.Close();
        }

        //private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        //{
        //    Assembly result = null;
        //    result = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(t => t.Equals(args.RequestingAssembly));
        //    if (result != null) return result;
        //    AssemblyName name = new AssemblyName(args.Name);
        //    string dllName = string.Format(@"{0}\{1}.dll", CommonDllPath.TrimEnd('\\'), name.Name);
        //    result = AssemblyHelper.LoadCopy(dllName);
        //    return result;
        //}


    }
}
