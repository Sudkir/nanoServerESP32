using nanoFramework.WebServer;
using System;
using System.Diagnostics;
using System.Net;

namespace nanoServerESP32.ServerESP
{
    internal class StartServer
    {
        public StartServer(int port = 80, HttpProtocol protocol = HttpProtocol.Http, Type[] controllers = null)
        {
            //WebServer server = new WebServer(80, HttpProtocol.Http);
            WebServer server = new WebServer(port, protocol,controllers);
            // Add a handler for commands that are received by the server.
            server.CommandReceived += ServerCommandReceived;
            // Start the server.
            server.Start();
        }

        private void ServerCommandReceivedREST(object obj, WebServerEventArgs e)
        {
            var url = e.Context.Request.RawUrl;
            Debug.WriteLine($"Command received: {url}, Method: {e.Context.Request.HttpMethod}");

            if (url.ToLower().IndexOf("/api/") == 0)
            {
                string ret = $"Your request type is: {e.Context.Request.HttpMethod}\r\n";
                ret += $"The request URL is: {e.Context.Request.RawUrl}\r\n";
                var parameters = WebServer.DecodeParam(e.Context.Request.RawUrl);
                if (parameters != null)
                {
                    ret += "List of url parameters:\r\n";
                    foreach (var param in parameters)
                    {
                        ret += $"  Parameter name: {param.Name}, value: {param.Value}\r\n";
                    }
                }

                if (e.Context.Request.Headers != null)
                {
                    ret += $"Number of headers: {e.Context.Request.Headers.Count}\r\n";
                }
                else
                {
                    ret += "There is no header in this request\r\n";
                }

                foreach (var head in e.Context.Request.Headers?.AllKeys)
                {
                    ret += $"  Header name: {head}, Values:";
                    var vals = e.Context.Request.Headers.GetValues(head);
                    foreach (var val in vals)
                    {
                        ret += $"{val} ";
                    }

                    ret += "\r\n";
                }

                if (e.Context.Request.ContentLength64 > 0)
                {

                    ret += $"Size of content: {e.Context.Request.ContentLength64}\r\n";
                    byte[] buff = new byte[e.Context.Request.ContentLength64];
                    e.Context.Request.InputStream.Read(buff, 0, buff.Length);
                    ret += $"Hex string representation:\r\n";
                    for (int i = 0; i < buff.Length; i++)
                    {
                        ret += buff[i].ToString("X") + " ";
                    }

                }

                WebServer.OutPutStream(e.Context.Response, ret);
            }
            else
            {
                WebServer.OutputHttpCode(e.Context.Response, HttpStatusCode.NotFound);
            }
        }

        private void ServerCommandReceived(object obj, WebServerEventArgs e)
        {
            var url = e.Context.Request.RawUrl;
            Debug.WriteLine($"Command received: {url}, Method: {e.Context.Request.HttpMethod}");

            if (url.ToLower() == "/sayhello")
            {
                // This is simple raw text returned
                WebServer.OutPutStream(e.Context.Response, "It's working, url is empty, this is just raw text, /sayhello is just returning a raw text");
            }
            else
            {
                WebServer.OutputHttpCode(e.Context.Response, HttpStatusCode.NotFound);
            }
        }
    }
}