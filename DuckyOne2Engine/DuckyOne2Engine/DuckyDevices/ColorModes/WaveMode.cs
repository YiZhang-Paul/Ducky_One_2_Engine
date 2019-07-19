using DuckyOne2Engine.ColorControls;
using DuckyOne2Engine.KeyMappers;
using System.Threading;
using System.Threading.Tasks;

namespace DuckyOne2Engine.DuckyDevices.ColorModes
{
    public class WaveMode : IColorMode
    {
        private readonly string[][] _rings =
        {
            new[]
            {
                Keys.W, Keys.E, Keys.R, Keys.T, Keys.Y, Keys.U, Keys.I, Keys.O, Keys.P,
                Keys.Lbracket, Keys.Rbracket, Keys.Pipe, Keys.Delete, Keys.Enter,
                Keys.Quote, Keys.Semicolon, Keys.L, Keys.K, Keys.J, Keys.H, Keys.G,
                Keys.F, Keys.D, Keys.S
            },
            new[]
            {
                Keys.One, Keys.Two, Keys.Three, Keys.Four, Keys.Five, Keys.Six,
                Keys.Seven, Keys.Eight, Keys.Nine, Keys.Zero, Keys.Hyphen, Keys.Equal,
                Keys.Backspace, Keys.Insert, Keys.Home, Keys.End, Keys.Uarrow, Keys.Rshift,
                Keys.Question, Keys.Period, Keys.Comma, Keys.M, Keys.N, Keys.B, Keys.V,
                Keys.C, Keys.X, Keys.Z, Keys.A, Keys.Q,
            },
            new[]
            {
                Keys.Lctrl, Keys.Lshift, Keys.Caps, Keys.Tab, Keys.Backtick, Keys.Esc,
                Keys.F1, Keys.F2, Keys.F3, Keys.F4, Keys.F5, Keys.F6,
                Keys.F7, Keys.F8, Keys.F9, Keys.F10, Keys.F11, Keys.F12,
                Keys.Print, Keys.Scroll, Keys.Pause, Keys.Pageup, Keys.Pagedown,
                Keys.Rarrow, Keys.Darrow, Keys.Larrow, Keys.Rctrl, Keys.Fn,
                Keys.Rwindow, Keys.Ralt, Keys.Space, Keys.Lalt, Keys.Lwindow
            }
        };

        private int Stage { get; set; }
        private bool IsMoving { get; set; } = true;
        private byte[] BackRgb { get; }
        private byte[] WaveRgb { get; }

        public WaveMode(byte[] backRgb, byte[] waveRgb)
        {
            BackRgb = backRgb;
            WaveRgb = waveRgb;
        }

        public void Setup(IColorControl colorControl)
        {
            colorControl.SetAll(BackRgb);
            colorControl.ApplyColors();
            Task.Run(() => Blink(colorControl));
        }

        public void Unload()
        {
            IsMoving = false;
        }

        private void Blink(IColorControl colorControl)
        {
            const int transitionOne = 3;
            const int transitionTwo = 6;
            int index = 0;

            while (IsMoving)
            {
                Thread.Sleep(Stage >= transitionTwo ? 150 : 50);
                colorControl.SetAll(Stage == transitionTwo + 2 ? WaveRgb : BackRgb);

                if (Stage == transitionTwo + 1 && index > 1)
                {
                    colorControl.SetColors(_rings[1], WaveRgb);
                }

                if (Stage == transitionTwo && index > 2 || Stage > transitionTwo)
                {
                    colorControl.SetColors(_rings[2], WaveRgb);
                }

                if (index >= 0 && index < _rings.Length)
                {
                    colorControl.SetColors(_rings[index], WaveRgb);
                }

                var isOutward = Stage < transitionOne || Stage >= transitionTwo;
                index = isOutward ? index + 1 : index - 1;

                if (isOutward && index == _rings.Length + 1 || !isOutward && index == -2)
                {
                    Stage = Stage < transitionTwo + 2 ? Stage + 1 : 0;
                    index = Stage < transitionOne || Stage >= transitionTwo ? 0 : _rings.Length - 1;
                    Thread.Sleep(Stage >= transitionTwo ? 450 : 25);
                }

                colorControl.ApplyColors();
            }
        }
    }
}
