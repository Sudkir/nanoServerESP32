using nanoFramework.WebServer;
using nanoServerESP32.ServerESP.Controllers;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace nanoServerESP32.ServerESP
{
    public class WebServerWLAN
    {
        // private WebServer server;

        public WebServerWLAN(int port = 80, HttpProtocol protocol = HttpProtocol.Http, Type[] controllers = null)
        {
            //WebServer server = new WebServer(80, HttpProtocol.Http);

            if (controllers is null)
            {
               // controllers[0] = typeof(GetInfoController);


                string[] allfiles = Directory.GetFiles(@"Controllers");
                foreach (string filename in allfiles)
                {
                    Console.WriteLine(filename);
                }

            }

            WebServer server = new WebServer(port, protocol, new Type[] { typeof(GetInfoController)});
            // Add a handler for commands that are received by the server.
            server.CommandReceived += ServerCommandReceived;
            // Start the server.
            server.Start();
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