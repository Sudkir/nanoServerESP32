using nanoFramework.Networking;
using System;
using System.Device.Wifi;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Threading;

namespace nanoServerESP32.WifiESP
{
   public class WiFiWLAN
    {
        //const string Ssid = "9164058067";
        //const string Password = "9164058067";
        CancellationTokenSource cs = new(60000);
        WifiInstance wifi = new();
        // Get the first WiFI Adapter
        WifiAdapter wifiAdapter;
        public WiFiWLAN()
        {
            wifi.SSID = "9164058067";
            wifi.Password = "9164058067";
            wifi.Status = WifiNetworkHelper.Status;

            wifiAdapter = WifiAdapter.FindAllAdapters()[0];
            wifiAdapter.AvailableNetworksChanged += Wifi_AvailableNetworksChanged;

            Thread.Sleep(10_000);

            Connect();
        }

        public void Disconnect()
        {
            WifiNetworkHelper.Disconnect();
            wifi.Status = WifiNetworkHelper.Status;
        }

        public void Connect()
        {
            try
            {
                while (true)
                {
                    try
                    {
                        Debug.WriteLine("starting Wi-Fi scan");
                        wifiAdapter.ScanAsync();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Failure starting a scan operation: {ex}");
                    }

                    Thread.Sleep(30000);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("message:" + ex.Message);
                Debug.WriteLine("stack:" + ex.StackTrace);
            }

        }

        public void ConnectDefault()
        {
            var success = WifiNetworkHelper.ConnectDhcp(wifi.SSID, wifi.Password, requiresDateTime: true, token: cs.Token);
            wifi.Status = WifiNetworkHelper.Status;
            if (!success)
            {
                // Something went wrong, you can get details with the ConnectionError property:
                Debug.WriteLine($"Can't connect to the network, error: {WifiNetworkHelper.Status}");
                if (WifiNetworkHelper.HelperException != null)
                {
                    Debug.WriteLine($"ex: {WifiNetworkHelper.HelperException}");
                }
            }
        }

        public void ConnectFixAddress()
        {
            CancellationTokenSource cs = new(60000);
            var success = WifiNetworkHelper.ConnectFixAddress(wifi.SSID, wifi.Password, new IPConfiguration("192.168.1.7", "255.255.255.0", "192.168.1.1"), requiresDateTime: true, token: cs.Token);
            wifi.Status = WifiNetworkHelper.Status;
            if (!success)
            {
                // Something went wrong, you can get details with the ConnectionError property:
                Debug.WriteLine($"Can't connect to the network, error: {WifiNetworkHelper.Status}");
                if (WifiNetworkHelper.HelperException != null)
                {
                    Debug.WriteLine($"ex: {WifiNetworkHelper.HelperException}");
                }
            }
        }




        /// <summary>
        /// Event handler for when Wifi scan completes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private  void Wifi_AvailableNetworksChanged(WifiAdapter sender, object e)
        {
            Debug.WriteLine("Wifi_AvailableNetworksChanged - get report");

            // Get Report of all scanned Wifi networks
            WifiNetworkReport report = sender.NetworkReport;

            // Enumerate though networks looking for our network
            foreach (WifiAvailableNetwork net in report.AvailableNetworks)
            {
                // Show all networks found
                Debug.WriteLine($"Net SSID :{net.Ssid},  BSSID : {net.Bsid},  rssi : {net.NetworkRssiInDecibelMilliwatts.ToString()},  signal : {net.SignalBars.ToString()}");

                // If its our Network then try to connect
                if (net.Ssid == wifi.SSID)
                {
                    // Disconnect in case we are already connected
                    sender.Disconnect();

                    // Connect to network
                    WifiConnectionResult result = sender.Connect(net, WifiReconnectionKind.Automatic, wifi.Password);

                    // Display status
                    if (result.ConnectionStatus == WifiConnectionStatus.Success)
                    {
                        Debug.WriteLine("Connected to Wifi network");
                    }
                    else
                    {
                        Debug.WriteLine($"Error {result.ConnectionStatus.ToString()} connecting o Wifi network");
                    }
                }
            }
        }


    }
}
