using DuckyOne2Engine.ColorControls;
using System.Threading;
using System.Threading.Tasks;

namespace DuckyOne2Engine.DuckyDevices.ColorModes
{
    public class ShiftMode : IColorMode
    {
        private int Interval { get; }
        private int RgbPointer { get; set; }
        private bool IsShifting { get; set; } = true;
        private byte[][] BackRgbs { get; }

        public ShiftMode(byte[][] backRgbs, int interval = 350)
        {
            BackRgbs = backRgbs;
            Interval = interval;
        }

        public void Setup(IColorControl colorControl)
        {
            colorControl.SetAll(BackRgbs[RgbPointer]);
            colorControl.ApplyColors();
            Task.Run(() => Shift(colorControl));
        }

        public void Unload()
        {
            IsShifting = false;
        }

        private void Shift(IColorControl colorControl)
        {
            while (IsShifting)
            {
                Thread.Sleep(Interval);
                RgbPointer = RgbPointer > BackRgbs.Length - 2 ? 0 : RgbPointer + 1;
                colorControl.SetAll(BackRgbs[RgbPointer]);
                colorControl.ApplyColors();
            }
        }
    }
}
