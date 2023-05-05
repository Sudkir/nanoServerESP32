using nanoFramework.WebServer;
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
            string route = $"The route asked is {e.Context.Request.RawUrl.TrimStart('/').Split('/')[0]}";
            e.Context.Response.ContentType = "text/plain";
            nanoFramework.WebServer.WebServer.OutPutStream(e.Context.Response, route);
        }

        [Route("test/any")]
        public void RouteAnyTest(WebServerEventArgs e)
        {
            nanoFramework.WebServer.WebServer.OutputHttpCode(e.Context.Response, HttpStatusCode.OK);
        }

        [Route("urlencode")]
        public void UrlEncode(WebServerEventArgs e)
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
    }
}
