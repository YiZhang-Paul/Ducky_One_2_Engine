using DuckyOne2Engine.HidDevices;
using DuckyOne2Engine.KeyMappers;
using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace DuckyOne2Engine
{
    class Program
    {
        static void Main(string[] args)
        {
            var name = "vid_04d9&pid_0356&mi_01";
            var devices = HidDevice.GetConnectedDevices();
            var path = devices.FirstOrDefault(_ => Regex.IsMatch(_.DevicePath, name)).DevicePath;

            if (path?.Length > 0)
            {
                var device = new HidDevice(path, false);
                SendCommandFromFile(device, "Commands/open.txt");
                Thread.Sleep(1500);
                SetupKeyboard(device);
                Application.Run(new ApplicationContext());
            }
        }

        static void SendCommandFromFile(IHidDevice device, string name)
        {
            foreach (var line in File.ReadAllLines(name))
            {
                var hex = line.Trim().Split(' ');
                device.Write(hex.Select(_ => Convert.ToByte(_, 16)).ToArray());
            }
        }

        static void SetupKeyboard(IHidDevice device)
        {
            var controller = new ColorControl(device, new KeyColorMapper());
            controller.SetAll(new byte[] { 1, 28, 73 });
            controller.ApplyColors();

            Hook.GlobalEvents().OnCombination(new Dictionary<Combination, Action>
            {
                { Combination.FromString("Control+Shift+M"), () => Exit(device) }
            });

            Hook.GlobalEvents().KeyDown += (sender, e) =>
            {
                var key = e.KeyCode.ToString();
                var color = new byte[] { 255, 255, 255 };
                controller.SetColor(new Tuple<string, byte[]>(key, color));
                controller.ApplyColors();
            };
        }

        static void Exit(IHidDevice device)
        {
            SendCommandFromFile(device, "Commands/open.txt");
            Application.Exit();
        }
    }
}
