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

        public BlinkMode(byte[] backRgb, int interval = 750)
        {
            BackRgb = backRgb;
            Interval = interval;
        }

        public void Setup(IColorControl colorControl)
        {
            colorControl.SetAll(BackRgb);
            colorControl.ApplyColors();
            Task.Run(() => Blink(colorControl));
        }

        public void Unload()
        {
            IsBlinking = false;
        }

        private void Blink(IColorControl colorControl)
        {
            while (IsBlinking)
            {
                Thread.Sleep(Interval);
                IsOn = !IsOn;
                colorControl.SetAll(IsOn ? BackRgb : new byte[] { 0, 0, 0 });
                colorControl.ApplyColors();
            }
        }
    }
}
