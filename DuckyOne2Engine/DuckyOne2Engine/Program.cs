using DuckyOne2Engine.ColorControls;
using DuckyOne2Engine.DuckyDevices;
using DuckyOne2Engine.DuckyDevices.ColorModes;
using DuckyOne2Engine.HidDevices;
using DuckyOne2Engine.KeyMappers;
using Gma.System.MouseKeyHook;
using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace DuckyOne2Engine
{
    public class Program
    {
        static void Main(string[] args)
        {
            const string name = "vid_04d9&pid_0356&mi_01";
            const string host = "http://localhost:4000/";
            var devices = HidDevice.GetConnectedDevices();
            var path = devices.FirstOrDefault(_ => Regex.IsMatch(_.DevicePath, name)).DevicePath;

            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            var device = new HidDevice(path, false);
            var controller = new ColorControl(device, new KeyColorMapper());

            using (WebApp.Start(host))
            {
                Console.WriteLine($"Server started listening on: {host}");
                Cache.GlobalKeyboardEvents = Hook.GlobalEvents();
                Cache.ActiveDuckyDevice = new DuckyDevice(device, controller);
                Cache.ActiveDuckyDevice.Use(new ReactiveMode(new byte[] { 0, 175, 175 }, new byte[] { 255, 255, 255 }, 15));

                Cache.GlobalKeyboardEvents.OnCombination(new Dictionary<Combination, Action>
                {
                    { Combination.FromString("Control+Shift+M"), Exit }
                });

                Application.Run(new ApplicationContext());
            }
        }

        private static void Exit()
        {
            Cache.ActiveDuckyDevice?.Close();
            Application.Exit();
        }
    }
}
