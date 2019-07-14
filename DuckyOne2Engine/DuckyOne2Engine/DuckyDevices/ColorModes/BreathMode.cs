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

        public BreathMode(byte[] backRgb, int steps = 70)
        {
            BackRgb = backRgb;
            Steps = steps;
            CurrentStep = steps;
        }

        public void Setup(IColorControl colorControl)
        {
            colorControl.SetAll(BackRgb);
            colorControl.ApplyColors();
            Task.Run(() => Breath(colorControl));
        }

        public void Unload()
        {
            IsBreathing = false;
        }

        private void Breath(IColorControl colorControl, bool off = true)
        {
            while (IsBreathing)
            {
                Thread.Sleep(20);
                off = off ? CurrentStep - 1 >= 0 : CurrentStep + 1 > Steps;
                CurrentStep = off ? --CurrentStep : ++CurrentStep;
                colorControl.SetAll(NextColor());
                colorControl.ApplyColors();
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
