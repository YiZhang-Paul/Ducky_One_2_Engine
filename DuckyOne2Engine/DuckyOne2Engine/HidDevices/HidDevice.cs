// source code borrowed from: https://github.com/Vidalee/DuckyVisual
using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace DuckyOne2Engine.HidDevices
{
    public class HidDevice : IHidDevice
    {
        public bool DeviceConnected { get; set; }
        public InterfaceDetails ProductInfo;
        public byte[] ReadData;
        public event DataReceivedEvent DataReceived;
        public delegate void DataReceivedEvent(byte[] message);

        private const int DIGCF_PRESENT = 0x2;
        private const int DIGCF_DEVICEINTERFACE = 0x10;
        private const uint GENERIC_READ = 0x80000000;
        private const uint GENERIC_WRITE = 0x40000000;
        private const uint FILE_SHARE_READ = 0x00000001;
        private const uint FILE_SHARE_WRITE = 0x00000002;
        private const uint OPEN_EXISTING = 3;

        private SafeFileHandle _handle_read;
        private SafeFileHandle _handle_write;
        private FileStream _fs_read;
        private FileStream _fs_write;
        private HIDP_CAPS _capabilities;
        private bool _useAsyncReads;

        public HidDevice(string devicePath, bool useAsyncReads)
        {
            SetupDevice(devicePath, useAsyncReads);

            if (!DeviceConnected)
            {
                throw new Exception("Device could not be found.");
            }
        }

        private void SetupDevice(string devicePath, bool useAsyncReads)
        {
            DeviceConnected = false;

            _handle_read = CreateFile
            (
                devicePath, GENERIC_READ | GENERIC_WRITE,
                FILE_SHARE_READ | FILE_SHARE_WRITE,
                IntPtr.Zero,
                OPEN_EXISTING,
                0,
                IntPtr.Zero
            );

            _handle_write = CreateFile
            (
                devicePath, GENERIC_READ | GENERIC_WRITE,
                FILE_SHARE_READ | FILE_SHARE_WRITE,
                IntPtr.Zero,
                OPEN_EXISTING,
                0,
                IntPtr.Zero
            );
            
            var ptrToPreParsedData = new IntPtr();
            _capabilities = new HIDP_CAPS();
            var attributes = new HIDD_ATTRIBUTES();
            HidD_GetPreparsedData(_handle_read, ref ptrToPreParsedData);
            HidP_GetCaps(ptrToPreParsedData, ref _capabilities);
            HidD_GetAttributes(_handle_read, ref attributes);

            string productName = "";
            string manfString = "";
            IntPtr buffer = Marshal.AllocHGlobal(126);//max alloc for string; 
            if (HidD_GetProductString(_handle_read, buffer, 126)) productName = Marshal.PtrToStringAuto(buffer);
            if (HidD_GetManufacturerString(_handle_read, buffer, 126)) manfString = Marshal.PtrToStringAuto(buffer);
            Marshal.FreeHGlobal(buffer);
            HidD_FreePreparsedData(ref ptrToPreParsedData);

            if (_handle_read.IsInvalid)
            {
                return;
            }

            DeviceConnected = true;
            ProductInfo = new InterfaceDetails();
            ProductInfo.DevicePath = devicePath;
            ProductInfo.Manufacturer = manfString;
            ProductInfo.Product = productName;
            ProductInfo.Pid = (ushort)attributes.ProductID;
            ProductInfo.Vid = (ushort)attributes.VendorID;
            ProductInfo.VersionNumber = (ushort)attributes.VersionNumber;
            ProductInfo.InReportByteLength = _capabilities.InputReportByteLength;
            ProductInfo.OutReportByteLength = _capabilities.OutputReportByteLength;

            _fs_read = new FileStream(_handle_read, FileAccess.ReadWrite, _capabilities.OutputReportByteLength, false);
            _fs_write = new FileStream(_handle_write, FileAccess.ReadWrite, _capabilities.InputReportByteLength, false);
            _useAsyncReads = useAsyncReads;

            if (useAsyncReads)
            {
                ReadAsync();
            }
        }

        public static InterfaceDetails[] GetConnectedDevices()
        {
            var devices = new InterfaceDetails[0];
            var devInfo = new SP_DEVINFO_DATA();
            var devIface = new SP_DEVICE_INTERFACE_DATA();
            var guid = new Guid();
            devInfo.cbSize = (uint)Marshal.SizeOf(devInfo);
            devIface.cbSize = (uint)Marshal.SizeOf(devIface);
            HidD_GetHidGuid(ref guid);
            IntPtr i = SetupDiGetClassDevs(ref guid, IntPtr.Zero, IntPtr.Zero, DIGCF_DEVICEINTERFACE | DIGCF_PRESENT);
            var didd = new SP_DEVICE_INTERFACE_DETAIL_DATA();
            didd.cbSize = IntPtr.Size == 8 ? 8 : 4 + Marshal.SystemDefaultCharSize;

            int j = -1;
            bool b = true;
            SafeFileHandle tempHandle;

            while (b)
            {
                j++;
                b = SetupDiEnumDeviceInterfaces(i, IntPtr.Zero, ref guid, (uint)j, ref devIface);
                Marshal.GetLastWin32Error();
                if (b == false)
                {
                    break;
                }

                uint requiredSize = 0;
                SetupDiGetDeviceInterfaceDetail(i, ref devIface, ref didd, 256, out requiredSize, ref devInfo);
                string devicePath = didd.DevicePath;

                tempHandle = CreateFile
                (
                    devicePath,
                    GENERIC_READ | GENERIC_WRITE,
                    FILE_SHARE_READ | FILE_SHARE_WRITE,
                    IntPtr.Zero,
                    OPEN_EXISTING,
                    0,
                    IntPtr.Zero
                );

                IntPtr ptrToPreParsedData = new IntPtr();
                bool ppdSucsess = HidD_GetPreparsedData(tempHandle, ref ptrToPreParsedData);

                if (ppdSucsess == false)
                {
                    continue;
                }

                var capabilities = new HIDP_CAPS();
                var attributes = new HIDD_ATTRIBUTES();
                HidP_GetCaps(ptrToPreParsedData, ref capabilities);
                HidD_GetAttributes(tempHandle, ref attributes);

                string productName = "";
                string serialNumber = "";
                string manfString = "";
                IntPtr buffer = Marshal.AllocHGlobal(126);//max alloc for string; 
                if (HidD_GetProductString(tempHandle, buffer, 126)) productName = Marshal.PtrToStringAuto(buffer);
                if (HidD_GetSerialNumberString(tempHandle, buffer, 126)) serialNumber = Marshal.PtrToStringAuto(buffer);
                if (HidD_GetManufacturerString(tempHandle, buffer, 126)) manfString = Marshal.PtrToStringAuto(buffer);
                Marshal.FreeHGlobal(buffer);
                HidD_FreePreparsedData(ref ptrToPreParsedData);
                var productInfo = new InterfaceDetails();
                productInfo.DevicePath = devicePath;
                productInfo.Manufacturer = manfString;
                productInfo.Product = productName;
                productInfo.Pid = (ushort)attributes.ProductID;
                productInfo.Vid = (ushort)attributes.VendorID;
                productInfo.VersionNumber = (ushort)attributes.VersionNumber;
                productInfo.InReportByteLength = capabilities.InputReportByteLength;
                productInfo.OutReportByteLength = capabilities.OutputReportByteLength;

                if (StringIsInteger(serialNumber))
                {
                    productInfo.SerialNumber = Convert.ToInt32(serialNumber);
                }

                int newSize = devices.Length + 1;
                Array.Resize(ref devices, newSize);
                devices[newSize - 1] = productInfo;
            }

            SetupDiDestroyDeviceInfoList(i);

            return devices;
        }

        private static bool StringIsInteger(string val)
        {
            Double result;

            return Double.TryParse
            (
                val,
                System.Globalization.NumberStyles.Integer,
                System.Globalization.CultureInfo.CurrentCulture,
                out result
            );
        }

        private void ReadAsync()
        {
            ReadData = new byte[_capabilities.InputReportByteLength];

            if (_fs_read.CanRead)
            {
                _fs_read.BeginRead(ReadData, 0, ReadData.Length, GetInputReportData, ReadData);
            }
            else
            {
                throw new Exception("Device is unable to read");
            }
        }

        private void GetInputReportData(IAsyncResult ar)
        {
            _fs_read.EndRead(ar);

            if (_fs_read.CanRead)
            {
                _fs_read.BeginRead(ReadData, 0, ReadData.Length, GetInputReportData, ReadData);
            }
            else
            {
                throw new Exception("Device is unable to read");
            }

            DataReceived(ReadData);
        }

        public bool Write(byte[] data)
        {
            if (data.Length > _capabilities.OutputReportByteLength)
            {
                throw new Exception($"Output report must not exceed {_capabilities.OutputReportByteLength - 1} bytes.");
            }

            try
            {
                if (_fs_write.CanWrite)
                {
                    _fs_write.Write(data, 0, data.Length);

                    return true;
                }
            }
            catch (Exception e)
            {
                throw new Exception($"File stream unable to write. Error: {e}");
            }

            return false;
        }

        public void Close()
        {
            if (_fs_read != null)
            {
                _fs_read.Close();
            }

            if (_fs_write != null)
            {
                _fs_write.Close();
            }

            if (_handle_read != null && !_handle_read.IsInvalid)
            {
                _handle_read.Close();
            }

            if (_handle_write != null && !_handle_write.IsInvalid)
            {
                _handle_write.Close();
            }

            DeviceConnected = false;
        }

        public void Dispose()
        {
            Close();
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern SafeFileHandle CreateFile
        (
            string lpFileName,
            uint dwDesiredAccess,
            uint dwShareMode,
            IntPtr lpSecurityAttributes,
            uint dwCreationDisposition,
            uint dwFlagsAndAttributes,
            IntPtr hTemplateFile
        );

        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool SetupDiDestroyDeviceInfoList(IntPtr DeviceInfoSet);

        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr SetupDiGetClassDevs
        (
            ref Guid ClassGuid,
            IntPtr Enumerator,
            IntPtr hwndParent,
            uint Flags
        );

        [DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern Boolean SetupDiEnumDeviceInterfaces
        (
            IntPtr hDevInfo,
            //ref SP_DEVINFO_DATA devInfo,
            IntPtr devInfo,
            ref Guid interfaceClassGuid,
            UInt32 memberIndex,
            ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData
        );

        [DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern Boolean SetupDiGetDeviceInterfaceDetail
        (
            IntPtr hDevInfo,
            ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData,
            ref SP_DEVICE_INTERFACE_DETAIL_DATA deviceInterfaceDetailData,
            UInt32 deviceInterfaceDetailDataSize,
            out UInt32 requiredSize,
            ref SP_DEVINFO_DATA deviceInfoData
        );

        [DllImport("hid.dll", SetLastError = true)]
        private static extern void HidD_GetHidGuid(ref Guid hidGuid);

        [DllImport("hid.dll", SetLastError = true)]
        private static extern Boolean HidD_FreePreparsedData(ref IntPtr PreparsedData);

        [DllImport("hid.dll", SetLastError = true)]
        private static extern bool HidD_GetPreparsedData(SafeFileHandle hObject, ref IntPtr PreparsedData);

        [DllImport("hid.dll", SetLastError = true)]
        private static extern int HidP_GetCaps(IntPtr pPHIDP_PREPARSED_DATA, ref HIDP_CAPS myPHIDP_CAPS);

        [DllImport("hid.dll", SetLastError = true)]
        private static extern Boolean HidD_GetAttributes(SafeFileHandle hObject, ref HIDD_ATTRIBUTES Attributes);

        [DllImport("hid.dll", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool HidD_GetProductString
        (
            SafeFileHandle hDevice,
            IntPtr Buffer,
            uint BufferLength
        );

        [DllImport("hid.dll", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool HidD_GetSerialNumberString
        (
            SafeFileHandle hDevice,
            IntPtr Buffer,
            uint BufferLength
        );

        [DllImport("hid.dll", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        private static extern Boolean HidD_GetManufacturerString
        (
            SafeFileHandle hDevice,
            IntPtr Buffer,
            uint BufferLength
        );

        [StructLayout(LayoutKind.Sequential)]
        private struct HIDP_CAPS
        {
            public UInt16 Usage;
            public UInt16 UsagePage;
            public UInt16 InputReportByteLength;
            public UInt16 OutputReportByteLength;
            public UInt16 FeatureReportByteLength;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
            public UInt16[] Reserved;			
            public UInt16 NumberLinkCollectionNodes;
            public UInt16 NumberInputButtonCaps;
            public UInt16 NumberInputValueCaps;
            public UInt16 NumberInputDataIndices;
            public UInt16 NumberOutputButtonCaps;
            public UInt16 NumberOutputValueCaps;
            public UInt16 NumberOutputDataIndices;
            public UInt16 NumberFeatureButtonCaps;
            public UInt16 NumberFeatureValueCaps;
            public UInt16 NumberFeatureDataIndices;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct SP_DEVINFO_DATA
        {
            public uint cbSize;
            public Guid ClassGuid;
            public uint DevInst;
            public IntPtr Reserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct SP_DEVICE_INTERFACE_DATA
        {
            public uint cbSize;
            public Guid InterfaceClassGuid;
            public uint Flags;
            public IntPtr Reserved;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct SP_DEVICE_INTERFACE_DETAIL_DATA
        {
            public int cbSize;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string DevicePath;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct HIDD_ATTRIBUTES
        {
            public Int32 Size;
            public Int16 VendorID;
            public Int16 ProductID;
            public Int16 VersionNumber;
        }
    }
}
