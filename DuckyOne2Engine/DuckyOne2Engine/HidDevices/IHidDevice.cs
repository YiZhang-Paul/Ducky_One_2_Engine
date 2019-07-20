using System;

namespace DuckyOne2Engine.HidDevices
{
    public interface IHidDevice : IDisposable
    {
        bool DeviceConnected { get; set; }
        bool Write(byte[] data);
        void Close();
    }
}
