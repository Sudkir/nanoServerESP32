using nanoFramework.Networking;
using System;

namespace nanoServerESP32.WifiESP
{
    public  class WifiInstance
    {
        public WifiInstance() => Status = NetworkHelperStatus.None;

        public  String SSID { get; set; }
        public  String Password { get; set; }
        public  NetworkHelperStatus Status { get; set; }
    }
}