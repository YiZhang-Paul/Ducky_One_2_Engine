using DuckyOne2Engine.KeyMappers;
using Gma.System.MouseKeyHook;
using HidLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Keys = DuckyOne2Engine.KeyMappers.Keys;

namespace DuckyOne2Engine
{
    class Program
    {
        private static IHidDevice _device;
        private static ColorControl _controller;

        static void Main(string[] args)
        {
            _device = FindDevice(0x04D9, 0x0356, "mi_01");

            if (_device != null)
            {
                _device.OpenDevice();
                _controller = new ColorControl(_device, new KeyColorMapper());
                _controller.SetAll(new byte[] { 0x00, 0x00, 0x00 });
                _controller.ApplyColors();
                ListenKeypress();
                Application.Run(new ApplicationContext());
            }
        }

        static void ListenKeypress()
        {
            Hook.GlobalEvents().KeyPress += (sender, e) =>
            {
                var map = new Dictionary<char, Keys>
                {
                    {'a', Keys.A },
                    {'b', Keys.B },
                    {'c', Keys.C },
                    {'d', Keys.D },
                    {'e', Keys.E },
                    {'f', Keys.F },
                    {'g', Keys.G },
                    {'h', Keys.H },
                    {'i', Keys.I },
                    {'j', Keys.J },
                    {'k', Keys.K },
                    {'l', Keys.L },
                    {'m', Keys.M },
                    {'n', Keys.N },
                    {'o', Keys.O },
                    {'p', Keys.P },
                    {'q', Keys.Q },
                    {'r', Keys.R },
                    {'s', Keys.S },
                    {'t', Keys.T },
                    {'u', Keys.U },
                    {'v', Keys.V },
                    {'w', Keys.W },
                    {'x', Keys.X },
                    {'y', Keys.Y },
                    {'z', Keys.Z }
                };

                var key = map.ContainsKey(e.KeyChar) ? map[e.KeyChar] : Keys.Backspace;
                _controller.SetColor(new Tuple<Keys, byte[]>(key, new byte[] { 55, 55, 55 }));
                _controller.ApplyColors();
            };
        }

        static IHidDevice FindDevice(int vendorId, int productId, string name)
        {
            var devices = HidDevices.Enumerate(vendorId, productId);

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
