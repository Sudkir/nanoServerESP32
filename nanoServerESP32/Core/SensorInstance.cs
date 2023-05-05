using nanoFramework.Networking;
using System;

namespace nanoServerESP32.WifiESP
{
    public static class SensorInstance
    {
        public static String ID { get; set; }
        public static String SSID { get; set; }
        public static String Password { get; set; }
        public static String HubIP { get; set; }
        public static NetworkHelperStatus Status { get; set; }
    }
}