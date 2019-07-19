using DuckyOne2Engine.ColorControls;
using System.Threading;
using System.Threading.Tasks;

namespace DuckyOne2Engine.DuckyDevices.ColorModes
{
    public class BlinkMode : IColorMode
    {
        private int Interval { get; }
        private bool IsBlinking { get; set; } = true;
        private bool IsOn { get; set; }
        private string[] SpecialKeys { get; }
        private byte[] BackRgb { get; }
        private byte[] BlinkRgb { get; }
        private byte[] SpecialRgb { get; }

        public BlinkMode
        (
            byte[] backRgb,
            byte[] blinkRgb,
            byte[] specialRgb,
            string[] specialKeys,
            int interval = 550
        )
        {
            BackRgb = backRgb;
            BlinkRgb = blinkRgb;
            SpecialRgb = specialRgb;
            SpecialKeys = specialKeys;
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
                colorControl.SetAll(IsOn ? BlinkRgb : BackRgb);
                colorControl.SetColors(SpecialKeys, SpecialRgb);
                colorControl.ApplyColors();
            }
        }
    }
}
