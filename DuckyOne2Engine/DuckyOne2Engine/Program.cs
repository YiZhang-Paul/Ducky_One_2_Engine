using DuckyOne2Engine.HidDevices;
using DuckyOne2Engine.KeyMappers;
using Gma.System.MouseKeyHook;
using System;
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
                var controller = new ColorControl(device, new KeyColorMapper());
                SendCommandFromFile(device, "Commands/open.txt");
                Thread.Sleep(1500);
                controller.SetAll(new byte[] { 0x00, 0x00, 0x00 });
                controller.ApplyColors();
                ListenKeypress(controller);
                Application.Run(new ApplicationContext());
            }
        }

        static void ListenKeypress(ColorControl controller)
        {
            Hook.GlobalEvents().KeyDown += (sender, e) =>
            {
                var key = e.KeyCode.ToString();
                var color = new byte[] { 255, 255, 0 };
                controller.SetColor(new Tuple<string, byte[]>(key, color));
                controller.ApplyColors();
            };
        }

        static void SendCommandFromFile(IHidDevice device, string name)
        {
            foreach (var input in ParseReports(name))
            {
                device.Write(input);
            }
        }

        static byte[][] ParseReports(string name)
        {
            try
            {
                var lines = File.ReadAllLines(name);
                var reports = new byte[lines.Length][];

                for (int i = 0; i < lines.Length; ++i)
                {
                    var hex = lines[i].Trim().Split(' ');
                    reports[i] = hex.Select(_ => Convert.ToByte(_, 16)).ToArray();
                }

                return reports;
            }
            catch
            {
                return new byte[0][];
            }
        }
    }
}
