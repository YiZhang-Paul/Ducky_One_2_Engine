using DuckyOne2Engine.KeyColors;
using HidLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DuckyOne2Engine
{
    class Program
    {
        static void Main(string[] args)
        {
            var vendorId = 0x04D9;
            var productId = 0x0356;
            var devices = HidDevices.Enumerate(vendorId, productId);

            using (var device = FindDevice(devices, "mi_01"))
            {
                if (device == null)
                {
                    return;
                }

                SendCommandFromFile(device, "Commands/open.txt");
                var mapper = new KeyColorMapper();
                var controller = new ColorControl(device, mapper);

                controller.SetColors(new[]
                {
                    new Tuple<string, byte[]>("ESC", new byte[] {0, 0, 255}),
                    new Tuple<string, byte[]>("G", new byte[] {255, 0, 0}),
                    new Tuple<string, byte[]>("SPACE", new byte[] {255, 0, 255}),
                    new Tuple<string, byte[]>("G", new byte[] {255, 255, 0}),
                    new Tuple<string, byte[]>("?", new byte[] {255, 255, 255})
                });

                controller.ApplyColors();
                Console.ReadKey();
                SendCommandFromFile(device, "Commands/close.txt");
            }
        }

        static IHidDevice FindDevice(IEnumerable<IHidDevice> devices, string name)
        {
            return devices.FirstOrDefault(_ => Regex.IsMatch(_.DevicePath, name));
        }

        static void SendCommandFromFile(IHidDevice device, string name)
        {
            var reports = ParseReports(name);

            foreach (var input in reports)
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
                    var bytes = hex.Select(_ => Convert.ToByte($"0x{_}", 16)).ToArray();
                    reports[i] = new byte[bytes.Length + 1];
                    Array.Copy(bytes, 0, reports[i], 1, bytes.Length);
                }

                return reports;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return new byte[0][];
            }
        }
    }
}
