using DuckyOne2Engine.ColorControls;
using System.Threading;
using System.Threading.Tasks;

namespace DuckyOne2Engine.DuckyDevices.ColorModes
{
    public class BlinkMode : IColorMode
    {
        private int Interval { get; }
        private bool IsOn { get; set; } = true;
        private bool IsBlinking { get; set; } = true;
        private byte[] BackRgb { get; }
        private IColorControl ColorControl { get; }

        public BlinkMode(IColorControl colorControl, byte[] backRgb, int interval = 750)
        {
            ColorControl = colorControl;
            BackRgb = backRgb;
            Interval = interval;
        }

        public void Setup()
        {
            ColorControl.SetAll(BackRgb);
            ColorControl.ApplyColors();
            Task.Run(() => Blink());
        }

        public void Unload()
        {
            IsBlinking = false;
        }

        private void Blink()
        {
            while (IsBlinking)
            {
                Thread.Sleep(Interval);
                IsOn = !IsOn;
                ColorControl.SetAll(IsOn ? BackRgb : new byte[] { 0, 0, 0 });
                ColorControl.ApplyColors();
            }
        }
    }
}
