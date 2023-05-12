using System;
using System.Collections;
using System.Device.Gpio;
using System.Diagnostics;

namespace nanoServerESP32.GpioCore
{
    public static class PinManagement
    {
        private static GpioController gpioController;
        private static GpioPin gpioRelayOne;
        private static GpioPin gpioRelayTwo;
        private static GpioPin gpioRelayThree;
       // private static GpioPin gpioRelayFour;
        private static ArrayList PinList;

        /// <summary>
        /// <br> Инициализация PinList </br>
        /// <br> Инициализация GpioController </br>
        /// <br> Инициализация GpioPins </br>
        /// </summary>
        public static void InitPinManagement()
        {
            gpioController = new GpioController();
            CreatePinList();
            CallbackForPinList();
        }

        /// <summary>
        /// Создания списка всех доступных для управления Gpio
        /// </summary>
        private static void CreatePinList()
        {
            PinList = new ArrayList();
            try
            {
                PinList.Add(value: gpioRelayOne = gpioController.OpenPin(25, PinMode.Output));
                PinList.Add(value: gpioRelayTwo = gpioController.OpenPin(26, PinMode.Output));
                PinList.Add(value: gpioRelayThree = gpioController.OpenPin(27, PinMode.Output));
               // PinList.Add(value: gpioRelayFour = gpioController.OpenPin(25, PinMode.Output));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// <br> Добавялет Callback </br>
        /// <br> None 0 Нет </br>
        /// <br> Rising 1 Активируется, когда значение закрепления переходит от низкого к высокому </br>
        /// <br> Falling 2 Активируется, когда значение контакта переходит от высокого к низкому </br>
        /// </summary>
        public static void CallbackForPinList()
        {
            try
            {
                if (PinList != null && !PinList.Count.Equals(0))
                {
                    foreach (var item in PinList)
                    {
                        var pin = (GpioPin)item;
                        gpioController.RegisterCallbackForPinValueChangedEvent(pin.PinNumber, PinEventTypes.Rising | PinEventTypes.Falling, Gpio_ValueChanged);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Прочитать состояние Gpio
        /// </summary>
        public static string ReadGpioState(string pin)
        {
            string pinValue = string.Empty;
            if (PinList != null && !PinList.Count.Equals(0))
            {
                foreach (var item in PinList)
                {
                    GpioPin pinItem = (GpioPin)item;
                    if (pinItem.PinNumber == Convert.ToInt32(pin))
                    {
                        pinValue = pinItem.().ToString();
                    }
                }
            }
            return pinValue;
        }

        /// <summary>
        /// <br> Изменяет PinValue у конкретного Gpio  </br>
        /// </summary>
        /// <param name="pin">Gpio</param>
        /// <param name="value">PinValue</param>
        public static void ChangePinValue(string pin, PinValue value)
        {
            if (PinList != null && !PinList.Count.Equals(0))
            {
                foreach (var item in PinList)
                {
                    GpioPin pinItem = (GpioPin)item;
                    if (pinItem.PinNumber == Convert.ToInt32(pin) && pinItem.GetPinMode() == PinMode.Output)
                    {
                        pinItem.Write(value);
                    }
                }
            }
        }

        /// <summary>
        /// If Gpio Value Changed Rising(Up) else ...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Gpio_ValueChanged(object sender, PinValueChangedEventArgs e)
        {
            if (e.ChangeType == PinEventTypes.Rising)
            {
                //
            }
            else
            {
                //
            }
        }
    }
}