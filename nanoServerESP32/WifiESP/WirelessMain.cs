﻿using Iot.Device.DhcpServer;
using nanoFramework.Networking;
using nanoFramework.Runtime.Native;
using nanoServerESP32.ServerESP;
using System;
using System.Device.Gpio;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;

namespace nanoServerESP32.WifiESP
{
    public class WirelessMain
    {
        public const string SoftApIP = "192.168.4.1";

        // Start Simple WebServer

        private static WebServerAP serverAP = new WebServerAP();

        // Connected Station count
        private static int connectedCount = 0;

        // GPIO pin used to put device into AP set-up mode
        private const int SETUP_PIN = 5;

        /// <summary>
        /// Check Wi-Fi connect
        /// </summary>
        public WirelessMain()
        {
            Debug.WriteLine("WirelessMain to WiFI Soft ");
            StartWLANServerNew();
            //var gpioController = new GpioController();
            //GpioPin setupButton = gpioController.OpenPin(SETUP_PIN, PinMode.InputPullUp);

            //if (!Wireless80211.IsEnabled() || (setupButton.Read() == PinValue.High))
            //{
            //    // If Wireless station is not enabled then start Soft AP to allow Wireless configuration
            //    // or Button pressed
            //    StartAPServer();
            //}
            //else
            //{
            //    StartWLANServerNew();
            //}
        }

        /// <summary>
        /// Start AP Server
        /// </summary>
        private void StartAPServer()
        {
            Wireless80211.Disable();
            if (WirelessAP.Setup() == false)
            {
                // Reboot device to Activate Access Point on restart
                Debug.WriteLine($"Setup Soft AP, Rebooting device");
                Power.RebootDevice();
            }

            var dhcpserver = new DhcpServer
            {
                CaptivePortalUrl = $"http://{WirelessAP.SoftApIP}"
            };
            var dhcpInitResult = dhcpserver.Start(IPAddress.Parse(WirelessAP.SoftApIP), new IPAddress(new byte[] { 255, 255, 255, 0 }));
            if (!dhcpInitResult)
            {
                Debug.WriteLine($"Error initializing DHCP server.");
            }

            Debug.WriteLine($"Running Soft AP, waiting for client to connect");
            Debug.WriteLine($"Soft AP IP address :{WirelessAP.GetIP()}");

            // Link up Network event to show Stations connecting/disconnecting to Access point.
            NetworkChange.NetworkAPStationChanged += NetworkChange_NetworkAPStationChanged;
            // Now that the normal Wifi is deactivated, that we have setup a static IP
            // We can start the Web server
            serverAP.Start();
        }

        private void StartWLANServerNew()
        {
            Debug.WriteLine($"Running in normal mode, connecting to Access point");
            //var conf = Wireless80211.GetConfiguration();
            // For devices like STM32, the password can't be read
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
        }


        /// <summary>
        /// Start WLAN Server after connect Wifi
        /// </summary>
        /// 
        private void StartWLANServer()
        {
            Debug.WriteLine($"Running in normal mode, connecting to Access point");
            var conf = Wireless80211.GetConfiguration();

            bool success;

            // For devices like STM32, the password can't be read
            if (string.IsNullOrEmpty(conf.Password))
            {
                // In this case, we will let the automatic connection happen
                success = WifiNetworkHelper.Reconnect(requiresDateTime: true, token: new CancellationTokenSource(60000).Token);
            }
            else
            {
                // If we have access to the password, we will force the reconnection
                // This is mainly for ESP32 which will connect normaly like that.
                success = WifiNetworkHelper.ConnectDhcp(conf.Ssid, conf.Password, requiresDateTime: true, token: new CancellationTokenSource(60000).Token);
            }

            if (success)
            {
                Debug.WriteLine($"Connection is {success}");
                Debug.WriteLine($"We have a valid date: {DateTime.UtcNow}");
                //Start WebServerWLAN
               // WebServerWLAN serverWLAN = new WebServerWLAN();
            }
            else
            {
                Debug.WriteLine($"Something wrong happened, can't connect at all");
            }
        }

        /// <summary>
        /// Event handler for Stations connecting or Disconnecting
        /// </summary>
        /// <param name="NetworkIndex">The index of Network Interface raising event</param>
        /// <param name="e">Event argument</param>
        private static void NetworkChange_NetworkAPStationChanged(int NetworkIndex, NetworkAPStationEventArgs e)
        {
            Debug.WriteLine($"NetworkAPStationChanged event Index:{NetworkIndex} Connected:{e.IsConnected} Station:{e.StationIndex} ");

            // if connected then get information on the connecting station
            if (e.IsConnected)
            {
                WirelessAPConfiguration wapconf = WirelessAPConfiguration.GetAllWirelessAPConfigurations()[0];
                WirelessAPStation station = wapconf.GetConnectedStations(e.StationIndex);

                string macString = BitConverter.ToString(station.MacAddress);
                Debug.WriteLine($"Station mac {macString} Rssi:{station.Rssi} PhyMode:{station.PhyModes} ");

                connectedCount++;

                // Start web server when it connects otherwise the bind to network will fail as
                // no connected network. Start web server when first station connects
                if (connectedCount == 1)
                {
                    // Wait for Station to be fully connected before starting web server
                    // other you will get a Network error
                    Thread.Sleep(2000);
                    serverAP.Start();
                }
            }
            else
            {
                // Station disconnected. When no more station connected then stop web server
                if (connectedCount > 0)
                {
                    connectedCount--;
                    if (connectedCount == 0)
                        serverAP.Stop();
                }
            }
        }
    }
}