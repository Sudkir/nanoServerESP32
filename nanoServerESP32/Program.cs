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
        private static GpioController s_GpioController;
        
        public static void Main()
        {
            s_GpioController = new GpioController();
            // ESP32 DevKit: 4 is a valid GPIO pin in, some boards 
            // like Xiuxin ESP32 may require GPIO Pin 2 instead.

            // WiFiWLAN wifi = new();

            // wifi.Connect();




            WiFiAP wifi = new();

           


        }
    }
}
