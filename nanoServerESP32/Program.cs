using nanoFramework.Networking;
using nanoServerESP32.GpioCore;
using nanoServerESP32.ServerESP;
using nanoServerESP32.WifiESP;
using System;
using System.Device.Gpio;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace nanoServerESP32
{
    public class Program
    {
        public static void Main()
        {
            Debug.WriteLine("START ESP by KARL");

            PinManagement.InitPinManagement();

            //WirelessMain wifi = new();
            if (WifiNetworkHelper.ConnectDhcp("9164058067", "9164058067", token: new CancellationTokenSource(10000).Token))
            {

                Debug.WriteLine($"We have a valid date: {DateTime.UtcNow}");
                //Start WebServerWLAN
                WebServerWLAN serverWLAN = new WebServerWLAN();

            }
            else
            {
                Debug.WriteLine($"ERROR 404");
            }

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
