using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SOAFramework.Library
{
    public class HttpServer
    {
        private string[] _prefixes;
        private HttpListener _listener;
        public event HttpExecutingHandler Executing;

        private ServerStatus status = ServerStatus.Wait;

        public ServerStatus Status
        {
            get
            {
                return status;
            }

            set
            {
                status = value;
            }
        }

        public HttpServer()
        {
        }

        public HttpServer(string[] prefixes = null)
        {
            this._prefixes = prefixes == null ? new string[] { "http://localhost:80/" } : prefixes;
        }

        public void Start(string[] prefixes = null)
        {
            if (_listener == null) _listener = new HttpListener();
            if (prefixes != null) _prefixes = prefixes;
            foreach (var prefix in _prefixes)
            {
                string p = prefix.TrimEnd('/') + "/";
                _listener.Prefixes.Add(p);
            }
            _listener.Start();
            status = ServerStatus.Start;
            var result = _listener.BeginGetContext(new AsyncCallback(GetContextCallback), _listener);
        }

        public void GetContextCallback(IAsyncResult result)
        {
            var listener = result.AsyncState as HttpListener;
            listener.BeginGetContext(new AsyncCallback(GetContextCallback), listener);
            HttpListenerContext context = listener.EndGetContext(result);
            HttpListenerRequest request = context.Request;
            // Obtain a response object.
            HttpListenerResponse response = context.Response;
            string responseString = Executing?.Invoke(this, new HttpExcutingEventArgs { Context = context, Request = request, Response = response });
            //nsole.WriteLine(request.RequestTraceIdentifier.ToString());

            byte[] buffer = new byte[0];
            if (!string.IsNullOrEmpty(responseString)) buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            // Get a response stream and write the response to it.
            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();
            Thread.Sleep(1);
        }

        public void Stop()
        {
            _listener.Stop();
            status = ServerStatus.Stop;
        }

        public void Close()
        {
            _listener.Close();
            status = ServerStatus.Close;
        }
    }

    public delegate string HttpExecutingHandler(object sender, HttpExcutingEventArgs e);
}

public class HttpExcutingEventArgs : EventArgs
{
    public HttpListenerContext Context { get; set; }

    public HttpListenerRequest Request { get; set; }

    public HttpListenerResponse Response { get; set; }
}

public enum ServerStatus
{
    Wait,
    Start,
    Stop,
    Close,
}
