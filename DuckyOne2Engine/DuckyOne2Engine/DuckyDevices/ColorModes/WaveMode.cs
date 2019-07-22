using DuckyOne2Engine.ColorControls;
using DuckyOne2Engine.KeyMappers;
using System.Linq;
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
                Keys.Esc, Keys.F1, Keys.F2, Keys.F3, Keys.F4, Keys.F5, Keys.F6,
                Keys.F7, Keys.F8, Keys.F9, Keys.F10, Keys.F11, Keys.F12,
                Keys.Print, Keys.Scroll, Keys.Pause, Keys.Pageup, Keys.Pagedown,
                Keys.Rarrow, Keys.Darrow, Keys.Larrow, Keys.Rctrl, Keys.Fn,
                Keys.Rwindow, Keys.Ralt, Keys.Space, Keys.Lalt, Keys.Lwindow,
                Keys.Lctrl, Keys.Lshift, Keys.Caps, Keys.Tab, Keys.Backtick
            }
        };

        private int Stage { get; set; }
        private int Step { get; set; }
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
            while (IsMoving)
            {
                colorControl.SetAll(BackRgb);

                switch (Stage)
                {
                    case 0:
                        SetStageOneColors(colorControl);
                        break;
                    case 1:
                        SetStageTwoColors(colorControl);
                        break;
                    case 2:
                        SetStageThreeColors(colorControl);
                        break;
                }

                colorControl.ApplyColors();
            }
        }

        private void SetStageOneColors(IColorControl colorControl)
        {
            Thread.Sleep(100);

            if (Step == _rings.Length * 2)
            {
                Thread.Sleep(150);
            }

            int mod = Step % _rings.Length;
            int index = Step < _rings.Length * 2 ? mod : _rings.Length - 1 - mod;
            colorControl.SetColors(_rings[index], WaveRgb);

            if (Step++ == _rings.Length * 4)
            {
                Stage++;
                Step = 0;
            }
        }

        private void SetStageTwoColors(IColorControl colorControl)
        {
            Thread.Sleep(320);

            if (Step == 0 || Step == 3 || Step == 5)
            {
                Thread.Sleep(250);
            }

            colorControl.SetColors(_rings[Step % _rings.Length], WaveRgb);

            if (Step > 2 && Step < 5)
            {
                colorControl.SetColors(_rings.Last(), WaveRgb);
            }
            else if (Step >= 5)
            {
                colorControl.SetAll(WaveRgb);
            }

            if (Step++ == 6)
            {
                Stage++;
                Step = 0;
            }
        }

        private void SetStageThreeColors(IColorControl colorControl)
        {
            Thread.Sleep(20);

            for (int i = 0; i < Step; ++i)
            {
                string key;

                if (i < _rings[2].Length)
                {
                    key = _rings[2][i];
                }
                else if (i - _rings[2].Length < _rings[1].Length)
                {
                    key = _rings[1][i - _rings[2].Length];
                }
                else
                {
                    key = _rings[0][i - _rings[1].Length - _rings[2].Length];
                }

                colorControl.SetColor(new KeyColor(key, WaveRgb));
            }

            if (Step++ == _rings.Sum(_ => _.Length))
            {
                Stage = 0;
                Step = 0;
            }
        }
    }
}
