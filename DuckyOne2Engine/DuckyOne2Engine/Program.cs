using DuckyOne2Engine.ColorControls;
using DuckyOne2Engine.DuckyDevices;
using DuckyOne2Engine.DuckyDevices.ColorModes;
using DuckyOne2Engine.HidDevices;
using DuckyOne2Engine.KeyMappers;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace DuckyOne2Engine
{
    class Program
    {
        private static DuckyDevice _duckyDevice;

        static void Main(string[] args)
        {
            const string name = "vid_04d9&pid_0356&mi_01";
            var devices = HidDevice.GetConnectedDevices();
            var path = devices.FirstOrDefault(_ => Regex.IsMatch(_.DevicePath, name)).DevicePath;

            if (path?.Length > 0)
            {
                var device = new HidDevice(path, false);
                var controller = new ColorControl(device, new KeyColorMapper());
                var backRgb = new byte[] { 1, 28, 73 };
                var activeRgb = new byte[] { 255, 255, 255 };
                _duckyDevice = new DuckyDevice(device, Exit);
                _duckyDevice.UseMode(new ReactiveMode(controller, backRgb, activeRgb));
                Application.Run(new ApplicationContext());
            }
        }

        private static void Exit()
        {
            _duckyDevice.Close();
            Application.Exit();
        }
    }
}
