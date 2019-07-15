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
        private readonly string[] _innerKeys =
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

        private readonly string[] _outerKeys =
        {
            Keys.Lctrl, Keys.Lshift, Keys.Caps, Keys.Tab, Keys.Backtick, Keys.Esc,
            Keys.F1, Keys.F2, Keys.F3, Keys.F4, Keys.F5, Keys.F6, Keys.F7, Keys.F8,
            Keys.F9, Keys.F10, Keys.F11, Keys.F12, Keys.Print, Keys.Scroll, Keys.Pause,
            Keys.Pageup, Keys.Pagedown, Keys.Rarrow, Keys.Darrow, Keys.Larrow, Keys.Rctrl,
            Keys.Fn, Keys.Rwindow, Keys.Ralt, Keys.Space, Keys.Lalt, Keys.Lwindow
        };

        private int InnerSpeed { get; }
        private int OuterSpeed { get; }
        private List<int> InnerIndexes { get; set; }
        private List<int> OuterIndexes { get; set; }
        private bool IsProgressing { get; set; } = true;
        private byte[] BackRgb { get; }
        private byte[] InnerRgb { get; }
        private byte[] OuterRgb { get; }

        public ProgressMode
        (
            byte[] backRgb,
            byte[] innerRgb,
            byte[] outerRgb,
            int innerSpeed = 25,
            int outerSpeed = 55
        )
        {
            BackRgb = backRgb;
            InnerRgb = innerRgb;
            OuterRgb = outerRgb;
            InnerSpeed = innerSpeed;
            OuterSpeed = outerSpeed;
        }

        public void Setup(IColorControl colorControl)
        {
            colorControl.SetAll(BackRgb);
            colorControl.ApplyColors();
            Task.Run(() => InnerProgress(colorControl));
            Task.Run(() => OuterProgress(colorControl));
        }

        public void Unload()
        {
            IsProgressing = false;
        }

        private List<int> GetColorIndexes(int start, int total, int maxIndex)
        {
            var indexes = new List<int> { start };

            for (int i = 0; i < total - 1; ++i)
            {
                var last = indexes.Last();
                indexes.Add(last > 0 ? last - 1 : maxIndex);
            }

            return indexes;
        }

        private void InnerProgress(IColorControl colorControl)
        {
            const int ringLength = 18;

            if (InnerIndexes == null)
            {
                InnerIndexes = GetColorIndexes(0, ringLength, _innerKeys.Length - 1);
            }

            Progress(colorControl, InnerRgb, InnerSpeed, ringLength, InnerIndexes, _innerKeys);
        }

        private void OuterProgress(IColorControl colorControl)
        {
            const int ringLength = 26;

            if (OuterIndexes == null)
            {
                OuterIndexes = GetColorIndexes(0, ringLength, _outerKeys.Length - 1);
            }

            Progress(colorControl, OuterRgb, OuterSpeed, ringLength, OuterIndexes, _outerKeys);
        }

        private void Progress
        (
            IColorControl colorControl,
            byte[] color,
            int speed,
            int ringLength,
            List<int> activeIndexes,
            string[] allKeys
        )
        {
            while (IsProgressing)
            {
                Thread.Sleep(speed);
                SetColors(colorControl, allKeys, BackRgb);
                var start = activeIndexes[0] > allKeys.Length - 2 ? 0 : activeIndexes[0] + 1;
                activeIndexes.Clear();
                activeIndexes.AddRange(GetColorIndexes(start, ringLength, allKeys.Length - 1));
                var activeKeys = activeIndexes.Select(_ => allKeys[_]).ToArray();
                SetColors(colorControl, activeKeys, color);
                colorControl.ApplyColors();
            }
        }

        private void SetColors(IColorControl colorControl, IEnumerable<string> keys, byte[] color)
        {
            colorControl.SetColors(keys.Select(_ => new KeyColor(_, color)));
        }
    }
}
