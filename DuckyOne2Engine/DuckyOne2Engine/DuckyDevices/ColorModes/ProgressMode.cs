using DuckyOne2Engine.ColorControls;
using DuckyOne2Engine.KeyMappers;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DuckyOne2Engine.DuckyDevices.ColorModes
{
    public class ProgressMode : IColorMode
    {
        private readonly int _totalActivePrimary = 18;
        private readonly int _totalActiveSecondary = 26;

        private readonly string[] _primaryKeys =
        {
            Keys.One, Keys.Q, Keys.A, Keys.Z, Keys.X, Keys.C, Keys.D, Keys.E,
            Keys.Three, Keys.Four, Keys.Five, Keys.T, Keys.G, Keys.B, Keys.N,
            Keys.M, Keys.J, Keys.U, Keys.Seven, Keys.Eight, Keys.Nine, Keys.O,
            Keys.L, Keys.Period, Keys.Question, Keys.Quote, Keys.Lbracket, Keys.Hyphen,
            Keys.Equal, Keys.Z, Keys.X, Keys.S, Keys.W, Keys.Two, Keys.Three, Keys.Four,
            Keys.R, Keys.F, Keys.V, Keys.B, Keys.N, Keys.H, Keys.Y, Keys.Six, Keys.Seven,
            Keys.Eight, Keys.I, Keys.K, Keys.Comma, Keys.Period, Keys.Question,
            Keys.Semicolon, Keys.P, Keys.Zero, Keys.Hyphen, Keys.Equal, Keys.Rbracket
        };

        private readonly string[] _secondaryKeys =
        {
            Keys.Lctrl, Keys.Lshift, Keys.Caps, Keys.Tab, Keys.Backtick, Keys.Esc,
            Keys.F1, Keys.F2, Keys.F3, Keys.F4, Keys.F5, Keys.F6, Keys.F7, Keys.F8,
            Keys.F9, Keys.F10, Keys.F11, Keys.F12, Keys.Print, Keys.Scroll, Keys.Pause,
            Keys.Pageup, Keys.Pagedown, Keys.Rarrow, Keys.Darrow, Keys.Larrow, Keys.Rctrl,
            Keys.Fn, Keys.Rwindow, Keys.Ralt, Keys.Space, Keys.Lalt, Keys.Lwindow
        };

        private int Speed { get; }
        private List<int> ActivePrimary { get; set; }
        private List<int> ActiveSecondary { get; set; }
        private bool IsProgressing { get; set; } = true;
        private byte[] BackRgb { get; }
        private byte[] PrimaryRgb { get; }
        private byte[] SecondaryRgb { get; }

        public ProgressMode(byte[] backRgb, byte[] primaryRgb, byte[] secondaryRgb, int speed = 25)
        {
            BackRgb = backRgb;
            PrimaryRgb = primaryRgb;
            SecondaryRgb = secondaryRgb;
            Speed = speed;
            ActivePrimary = GetIndexes(0, _totalActivePrimary, _primaryKeys.Length - 1);
            ActiveSecondary = GetIndexes(0, _totalActiveSecondary, _secondaryKeys.Length - 1);
        }

        public void Setup(IColorControl colorControl)
        {
            colorControl.SetAll(BackRgb);
            colorControl.ApplyColors();
            Task.Run(() => Progress(colorControl));
        }

        public void Unload()
        {
            IsProgressing = false;
        }

        private void Progress(IColorControl colorControl)
        {
            while (IsProgressing)
            {
                Thread.Sleep(Speed);
                colorControl.SetAll(BackRgb);
                var start = ActivePrimary[0] > _primaryKeys.Length - 2 ? 0 : ActivePrimary[0] + 1;
                ActivePrimary = GetIndexes(start, _totalActivePrimary, _primaryKeys.Length - 1);
                colorControl.SetColors(ActivePrimary.Select(_ => _primaryKeys[_]), PrimaryRgb);
                start = ActiveSecondary[0] > _secondaryKeys.Length - 2 ? 0 : ActiveSecondary[0] + 1;
                ActiveSecondary = GetIndexes(start, _totalActiveSecondary, _secondaryKeys.Length - 1);
                colorControl.SetColors(ActiveSecondary.Select(_ => _secondaryKeys[_]), SecondaryRgb);
                colorControl.ApplyColors();
            }
        }

        private List<int> GetIndexes(int start, int total, int maxIndex)
        {
            var indexes = new List<int> { start };

            for (int i = 0; i < total - 1; ++i)
            {
                var last = indexes.Last();
                indexes.Add(last > 0 ? last - 1 : maxIndex);
            }

            return indexes;
        }
    }
}
