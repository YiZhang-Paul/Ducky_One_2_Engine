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
        private static IHidDevice _device;

        static void Main(string[] args)
        {
            const string name = "vid_04d9&pid_0356&mi_01";
            var devices = HidDevice.GetConnectedDevices();
            var path = devices.FirstOrDefault(_ => Regex.IsMatch(_.DevicePath, name)).DevicePath;

            if (path?.Length > 0)
            {
                _device = new HidDevice(path, false);
                var controller = new ColorControl(_device, new KeyColorMapper());

                // reactive mode
                // var backRgb = new byte[] { 1, 28, 73 };
                // var activeRgb = new byte[] { 255, 255, 255 };
                // var mode = new ReactiveMode(controller, backRgb, activeRgb);

                // breath mode
                // var backRgb = new byte[] { 255, 255, 0 };
                // var mode = new BreathMode(controller, backRgb);

                // blink mode
                var backRgb = new byte[] { 255, 0, 0 };
                var mode = new BlinkMode(controller, backRgb);

                _duckyDevice = new DuckyDevice(_device, Exit);
                _duckyDevice.Use(mode);
                Application.Run(new ApplicationContext());
            }
        }

        private static void Exit()
        {
            _duckyDevice?.Close();
            _device?.Close();
            Application.Exit();
        }
    }
}
