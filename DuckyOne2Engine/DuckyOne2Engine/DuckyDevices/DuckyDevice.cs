using DuckyOne2Engine.ColorControls;
using DuckyOne2Engine.DuckyDevices.ColorModes;
using DuckyOne2Engine.HidDevices;
using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace DuckyOne2Engine.DuckyDevices
{
    public class DuckyDevice
    {
        private IHidDevice Device { get; }
        private IColorControl ColorControl { get; }
        private IColorMode ColorMode { get; set; }

        public DuckyDevice(IHidDevice device, IColorControl colorControl, Action onExit)
        {
            Device = device;
            ColorControl = colorControl;
            Open();
            Thread.Sleep(1500);
            Setup(onExit);
        }

        private void Setup(Action onExit)
        {
            Hook.GlobalEvents().OnCombination(new Dictionary<Combination, Action>
            {
                { Combination.FromString("Control+Shift+M"), onExit }
            });
        }

        public DuckyDevice Use(IColorMode mode)
        {
            ColorMode?.Unload();
            ColorMode = mode;
            ColorMode.Setup(ColorControl);

            return this;
        }

        public void Open()
        {
            SendCommandFromFile(Device, "Commands/open.txt");
        }

        public void Close()
        {
            SendCommandFromFile(Device, "Commands/open.txt");
            Device.Close();
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
