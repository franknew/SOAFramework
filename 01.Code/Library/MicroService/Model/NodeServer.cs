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
        private SimpleLogger logger = new SimpleLogger();
        private int executingCount = 0;
        private int finishedCount = 0;
        private AppDomain apiDomain = null;
        private List<Assembly> _assList = new List<Assembly>();


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

        public int ExecutingCount
        {
            get
            {
                return executingCount;
            }
        }

        public int FinishedCount
        {
            get
            {
                return finishedCount;
            }
        }

        public AppDomain ApiDomain
        {
            get
            {
                return apiDomain;
            }

            set
            {
                apiDomain = value;
            }
        }

        public List<Assembly> AssList { get => _assList; set => _assList = value; }

        public NodeServer(string url, AppDomain domain = null)
        {
            this.url = url;
            this.apiDomain = domain;
            this.apiDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            httpServer = new HttpServer(new string[] { this.url });
        }

        public NodeServer()
        {
        }

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            Assembly assembly = null;
            var asses = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var ass in asses)
            {
                if (ass.FullName.Equals(args.Name)) return ass;
            }
            return assembly;
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
                //如果是更新状态就把请求卡住，等更新完毕了再继续执行
                while (true)
                {
                    if (Status != ServerStatus.Updating) break;
                    else Thread.Sleep(500);
                }
                string rawUrl = e.Request.RawUrl.TrimEnd('/');
                int start = rawUrl.LastIndexOf("/");
                int end = rawUrl.LastIndexOf("?");
                if (end == -1) end = rawUrl.Length;
                action = rawUrl.Substring(start + 1, end - start - 1);
                executingCount++;
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
                if (serializor == null) throw new Exception(string.Format("没有找到序列器，Content-Type:{0}", e.Request.ContentType));
                filterHandler = FilterHandler.GetSessionHandler(sessionName);
                string post;

                using (StreamReader reader = new StreamReader(e.Request.InputStream, Encoding.UTF8))
                {

                    post = reader.ReadToEnd();
                    logger.Write("Post数据:" + post);
                }

                Dictionary<string, object> args = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(post)) args = serializor.Deserialize<Dictionary<string, object>>(post);
                if (args == null) args = new Dictionary<string, object>();
                foreach (var key in e.Request.QueryString.AllKeys)
                {
                    if (!args.ContainsKey(key)) args[key] = e.Request.QueryString[key];
                }
                httpContext.Args = args;
                HttpServerContext.AddContext(Thread.CurrentThread.ManagedThreadId.ToString(), httpContext);
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

                var returnSerial = SerializeFactory.Create(httpContext.MsgReturnType);
                if (returnSerial != null && returnSerial is XmlSerializor)
                {
                    ((XmlSerializor)returnSerial).HasXmlHeader = httpContext.HasXmlHeader;
                }
                if (response == null) responseString = "";
                else if (response.Success) responseString = returnSerial.Serialize(response.Result);
                else responseString = returnSerial.Serialize(response);
            }
            catch (Exception ex)
            {
                try
                {
                    filterHandler.HandleOnException(action, httpContext, ex);
                }
                catch (Exception exOut)
                {
                    ex = exOut;
                }
                JsonResponse response = new JsonResponse();
                response.Success = false;
                response.Error = new JsonRpcException(100, ex.Message, ex.InnerException);
                responseString = serializor.Serialize(response);
            }
            finally
            {
                HttpServerContext.RemoveContext(Thread.CurrentThread.ManagedThreadId.ToString());
                executingCount--;
                finishedCount++;
            }
            return responseString;
        }

        public void Start(string url)
        {
            Start(url, null, null);
        }

        public void Start()
        {
            Start(null, null, null);
        }

        public void Start(string url, AppDomain domain, List<Assembly> list)
        {
            try
            {
                apiDomain = domain;
                if (!string.IsNullOrEmpty(url)) this.url = url;
                Bind(domain, ServerType.Server, list);
                if (httpServer == null) httpServer = new HttpServer(new string[] { this.url });
                httpServer.Executing += HttpServer_Executing;
                httpServer.Start();
            }
            catch (Exception ex)
            {
                logger.WriteException(ex);
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

        public void Update(AppDomain domain, int serverType, List<Assembly> assList = null)
        {
            if (apiDomain == null) return;
            this.Status = ServerStatus.Updating;
            while (executingCount > 0)
            {
                Thread.Sleep(500);
            }
            ServiceBinder.Clear(sessionName);
            FilterBinder.Clear(sessionName);
            //AppDomain.Unload(apiDomain);
            apiDomain = domain;
            Bind(domain, ServerType.Server, assList);
            Status = ServerStatus.Running;
        }

        private void Bind(AppDomain domain, ServerType serverType, List<Assembly> assList = null)
        {
            var server = ServerTypeFactory.Create(serverType);
            server.Bind(sessionName, domain, assList);
        }
    }
}
