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
            try
            {
                //string[] allfiles = Directory.GetFiles(@"Controllers");
                //foreach (string filename in allfiles)
                //{
                //    Debug.WriteLine(filename);
                //}

                WebServer server = new WebServer(port, protocol, new Type[] { typeof(GetInfoController) });
                // Add a handler for commands that are received by the server.
                //server.CommandReceived += ServerCommandReceived;
                // Start the server.
                server.Start();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}