using DuckyOne2Engine.ColorControls;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DuckyOne2Engine.DuckyDevices.ColorModes
{
    public class BreathMode : IColorMode
    {
        private int Steps { get; }
        private int CurrentStep { get; set; }
        private bool IsBreathing { get; set; } = true;
        private byte[] BackRgb { get; }
        private IColorControl ColorControl { get; }

        public BreathMode(IColorControl colorControl, byte[] backRgb, int steps = 70)
        {
            ColorControl = colorControl;
            BackRgb = backRgb;
            Steps = steps;
            CurrentStep = steps;
        }

        public void Setup()
        {
            ColorControl.SetAll(BackRgb);
            ColorControl.ApplyColors();
            Task.Run(() => Breath());
        }

        public void Unload()
        {
            IsBreathing = false;
        }

        private void Breath(bool off = true)
        {
            while (IsBreathing)
            {
                Thread.Sleep(20);
                off = off ? CurrentStep - 1 >= 0 : CurrentStep + 1 > Steps;
                CurrentStep = off ? --CurrentStep : ++CurrentStep;
                ColorControl.SetAll(NextColor());
                ColorControl.ApplyColors();
            }
        }

        private byte[] NextColor()
        {
            byte NextValue(byte value)
            {
                var delta = (int)Math.Ceiling((double)value / Steps);

                return (byte)Math.Max(0, value - (Steps - CurrentStep) * delta);
            }

            var r = NextValue(BackRgb[0]);
            var g = NextValue(BackRgb[1]);
            var b = NextValue(BackRgb[2]);

            return new[] { r, g, b };
        }
    }
}
