namespace DuckyOne2Engine.HidDevices
{
    public interface IHidDevice
    {
        bool DeviceConnected { get; set; }
        void Write(byte[] data);
        void Close();
    }
}
