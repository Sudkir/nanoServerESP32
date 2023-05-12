using nanoFramework.WebServer;
using nanoServerESP32.GpioCore;
using System;
using System.Device.Gpio;
using System.Diagnostics;
using System.Net;
using System.Web;

namespace nanoServerESP32.ServerESP.Controllers
{
    public class GetInfoController
    {
        [Route("test"), Route("Test2"), Route("tEst42"), Route("TEST")]
        [CaseSensitive]
        [Method("GET")]
        public void RoutePostTest(WebServerEventArgs e)
        {
            string route = $"The route asked is {e.Context.Request.RawUrl.TrimStart('/').Split('/')[0]} {DateTime.UtcNow}";
            e.Context.Response.ContentType = "text/plain";
            nanoFramework.WebServer.WebServer.OutPutStream(e.Context.Response, route);
        }

        [Route("test3/t")]
        [CaseSensitive]
        [Method("GET")]
        public void RoutePostTest3(WebServerEventArgs e)
        {
            string route = $"The route asked is {e.Context.Request.RawUrl.TrimStart('/').Split('/')[0]} {DateTime.UtcNow}";
            e.Context.Response.ContentType = "text/plain";
            nanoFramework.WebServer.WebServer.OutPutStream(e.Context.Response, route);
        }


        [Route("test/switch/value")]
        public void RouteSwitch(WebServerEventArgs e)
        {
            try
            {
                var rawUrl = e.Context.Request.RawUrl;
                var urlParams = WebServer.DecodeParam(rawUrl);
                if (urlParams is null)
                {
                    Debug.WriteLine("BadRequest");
                    nanoFramework.WebServer.WebServer.OutputHttpCode(e.Context.Response, HttpStatusCode.BadRequest);
                }
                else
                {
                    foreach (var param in urlParams)
                    {
                        PinValue pinValue = Convert.ToInt32(param.Value);
                        string pinName = param.Name;
                        PinManagement.ChangePinValue(pinName, pinValue);
                    }
                    nanoFramework.WebServer.WebServer.OutputHttpCode(e.Context.Response, HttpStatusCode.OK);
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("BadRequest");
                nanoFramework.WebServer.WebServer.OutputHttpCode(e.Context.Response, HttpStatusCode.BadRequest);
            }
        }

        [Route("test/switch/state")]
        public void RouteSwitchState(WebServerEventArgs e)
        {
            try
            {
                var rawUrl = e.Context.Request.RawUrl;
                var urlParams = WebServer.DecodeParam(rawUrl);
                if (urlParams is null)
                {
                    Debug.WriteLine("BadRequest url params is null");
                    nanoFramework.WebServer.WebServer.OutputHttpCode(e.Context.Response, HttpStatusCode.BadRequest);
                }
                else
                {
                    string state = string.Empty;
                    foreach (var param in urlParams)
                    {
                        PinValue pinValue = Convert.ToInt32(param.Value);
                        string pinName = param.Name;
                         state = PinManagement.ReadGpioState(pinName);
                    }

                    string route = @$" ""state"":""{state}"" ";
                    e.Context.Response.ContentType = "application/json";
                    nanoFramework.WebServer.WebServer.OutPutStream(e.Context.Response, route);


                    //nanoFramework.WebServer.WebServer.OutputHttpCode(e.Context.Response, HttpStatusCode.OK);
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("BadRequest");
                nanoFramework.WebServer.WebServer.OutputHttpCode(e.Context.Response, HttpStatusCode.BadRequest);
            }
        }

        [Route("urlencode")]
        public void UrlEncode(WebServerEventArgs e)
        {
            try
            {
                var rawUrl = e.Context.Request.RawUrl;
                var paramsUrl = WebServer.DecodeParam(rawUrl);
                string ret = "Parameters | Encoded | Decoded";
                foreach (var param in paramsUrl)
                {
                    ret += $"{param.Name} | ";
                    ret += $"{param.Value} | ";
                    // Need to wait for latest version of System.Net
                    // See https://github.com/nanoframework/lib-nanoFramework.System.Net.Http/blob/develop/nanoFramework.System.Net.Http/Http/System.Net.HttpUtility.cs
                    ret += $"{HttpUtility.UrlDecode(param.Value)}";
                    ret += "\r\n";
                }
                WebServer.OutPutStream(e.Context.Response, ret);
            }
            catch (System.Exception)
            {
                Debug.WriteLine("BadRequest");
                nanoFramework.WebServer.WebServer.OutputHttpCode(e.Context.Response, HttpStatusCode.BadRequest);
            }
        }
    }
}