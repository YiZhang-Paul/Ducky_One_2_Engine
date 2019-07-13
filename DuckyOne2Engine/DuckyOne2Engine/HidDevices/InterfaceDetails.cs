// source code borrowed from: https://github.com/Vidalee/DuckyVisual
namespace DuckyOne2Engine.HidDevices
{
    public struct InterfaceDetails
    {
        public string Manufacturer;
        public string Product;
        public int SerialNumber;
        public ushort Vid;
        public ushort Pid;
        public string DevicePath;
        public int InReportByteLength;
        public int OutReportByteLength;
        public ushort VersionNumber;
    }
}
