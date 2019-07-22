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
using System.Threading;
using System.Threading.Tasks;
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
                Cache.ActiveDuckyDevice = new DuckyDevice(device, controller);
                Cache.GlobalKeyboardEvents = Hook.GlobalEvents();

                Cache.GlobalKeyboardEvents.OnCombination(new Dictionary<Combination, Action>
                {
                    { Combination.FromString("Control+Shift+M"), Exit }
                });

                Task.Run(() => Loop());
                Application.Run(new ApplicationContext());
            }
        }

        private static void Exit()
        {
            Cache.ActiveDuckyDevice?.Close();
            Application.Exit();
        }

        private static void Loop(int counter = -1)
        {
            while (true)
            {
                counter = counter == 4 ? -1 : counter + 1;

                switch (counter)
                {
                    case 0:
                        Cache.ActiveDuckyDevice.Use(new ProgressMode(new byte[] { 255, 0, 255 }, new byte[] { 0, 255, 225 }, 70));
                        Thread.Sleep(1620000);
                        break;
                    case 1:
                        Cache.ActiveDuckyDevice.Use(new SprintMode(new byte[] { 149, 0, 149 }, new byte[] { 229, 0, 229 }, new byte[] { 0, 0, 255 }, 75));
                        Thread.Sleep(1620000);
                        break;
                    case 2:
                        Cache.ActiveDuckyDevice.Use(new WaveMode(new byte[] { 30, 68, 123 }, new byte[] { 0, 0, 255 }));
                        Thread.Sleep(120000);
                        break;
                    case 3:
                        Cache.ActiveDuckyDevice.Use(new BlinkMode(new byte[] { 55, 55, 55 }, new byte[] { 255, 99, 71 }, new byte[] { 255, 0, 0 }, new[] { "D3" }));
                        Thread.Sleep(120000);
                        break;
                    case 4:
                        Cache.ActiveDuckyDevice.Use(new ReactiveMode(new byte[] { 0, 85, 85 }, new byte[] { 255, 255, 255 }, 15));
                        Thread.Sleep(120000);
                        break;
                }
            }
        }
    }
}
