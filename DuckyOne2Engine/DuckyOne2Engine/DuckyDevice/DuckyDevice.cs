using DuckyOne2Engine.HidDevices;
using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace DuckyOne2Engine.DuckyDevice
{
    public class DuckyDevice
    {
        private IHidDevice Device { get; }

        public DuckyDevice(IHidDevice device)
        {
            Device = device;
            SendCommandFromFile(Device, "../Commands/open.txt");
            Thread.Sleep(1500);
        }

        private void SendCommandFromFile(IHidDevice device, string name)
        {
            foreach (var line in File.ReadAllLines(name))
            {
                var hex = line.Trim().Split(' ');
                device.Write(hex.Select(_ => Convert.ToByte(_, 16)).ToArray());
            }
        }
    }
}
