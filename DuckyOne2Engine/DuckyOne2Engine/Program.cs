using DuckyOne2Engine.ColorControls;
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

            var device = new HidDevice(path, false);
            var controller = new ColorControl(device, new KeyColorMapper());

            // reactive mode
            //var backRgb = new byte[] { 1, 28, 73 };
            //var activeRgb = new byte[] { 255, 255, 255 };
            //var mode = new ReactiveMode(controller, backRgb, activeRgb);

            // breath mode
            //var backRgb = new byte[] { 255, 255, 0 };
            //var mode = new BreathMode(controller, backRgb);

            // blink mode
            //var backRgb = new byte[] { 255, 0, 0 };
            //var mode = new BlinkMode(controller, backRgb);

            // sprint mode
            var backRgb = new byte[] { 1, 28, 73 };
            var sprintRgb = new byte[] { 0, 0, 255 };
            var mode = new SprintMode(controller, backRgb, sprintRgb);

            ActiveDevice = new DuckyDevice(device, Exit);
            ActiveDevice.Use(mode);

            using (WebApp.Start(host))
            {
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
