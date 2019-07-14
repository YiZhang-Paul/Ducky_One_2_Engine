﻿using DuckyOne2Engine.ColorControls;
using DuckyOne2Engine.DuckyDevices;
using DuckyOne2Engine.DuckyDevices.ColorModes;
using DuckyOne2Engine.HidDevices;
using DuckyOne2Engine.KeyMappers;
using Microsoft.Owin.Hosting;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace DuckyOne2Engine
{
    public class Program
    {
        public static DuckyDevice ActiveDevice;

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

            var backRgb = new byte[] { 1, 28, 73 };
            var activeRgb = new byte[] { 255, 255, 255 };
            var device = new HidDevice(path, false);
            var controller = new ColorControl(device, new KeyColorMapper());
            var mode = new ReactiveMode(backRgb, activeRgb);

            using (WebApp.Start(host))
            {
                ActiveDevice = new DuckyDevice(device, controller, Exit).Use(mode);
                Cache.ActiveDuckyDevice = ActiveDevice;
                Console.WriteLine($"Server started listening on: {host}");
                Application.Run(new ApplicationContext());
            }
        }

        private static void Exit()
        {
            ActiveDevice?.Close();
            Application.Exit();
        }
    }
}
