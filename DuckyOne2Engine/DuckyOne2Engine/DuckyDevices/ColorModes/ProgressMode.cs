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
            Keys.One, Keys.Q, Keys.A, Keys.Z,
            Keys.X, Keys.S, Keys.W, Keys.Two,
            Keys.Three, Keys.E, Keys.D, Keys.C,
            Keys.V, Keys.F, Keys.R, Keys.Four,
            Keys.Five, Keys.T, Keys.G, Keys.B,
            Keys.N, Keys.H, Keys.Y, Keys.Six,
            Keys.Seven, Keys.U, Keys.J, Keys.M,
            Keys.Comma, Keys.K, Keys.I, Keys.Eight,
            Keys.Nine, Keys.O, Keys.L, Keys.Period,
            Keys.Question, Keys.Semicolon, Keys.P, Keys.Zero,
            Keys.Hyphen, Keys.Lbracket, Keys.Quote,
            Keys.Rbracket, Keys.Equal
        };

        private readonly string[] _outerKeys =
        {
            Keys.Lctrl, Keys.Lshift, Keys.Caps, Keys.Tab, Keys.Backtick, Keys.Esc,
            Keys.F1, Keys.F2, Keys.F3, Keys.F4, Keys.F5, Keys.F6, Keys.F7, Keys.F8, Keys.F9, Keys.F10, Keys.F11, Keys.F12,
            Keys.Print, Keys.Scroll, Keys.Pause, Keys.Pageup, Keys.Pagedown, Keys.Rarrow, Keys.Darrow, Keys.Larrow,
            Keys.Rctrl, Keys.Fn, Keys.Rwindow, Keys.Ralt, Keys.Space, Keys.Lalt, Keys.Lwindow
        };

        private readonly byte[][] _rgb =
        {
            new byte[] { 255, 0, 0 }, 
            new byte[] { 0, 255, 0 }, 
            new byte[] { 0, 0, 255 }
        };

        private int InnerSpeed { get; }
        private int OuterSpeed { get; }
        private int InnerRgbPointer { get; set; }
        private int OuterRgbPointer { get; set; } = 1;
        private int[] InnerIndexes { get; set; }
        private int[] OuterIndexes { get; set; }
        private bool IsProgressing { get; set; } = true;
        private byte[] BackRgb { get; }
        private byte[] InnerRgb { get; }
        private byte[] OuterRgb { get; }

        public ProgressMode
        (
            byte[] backRgb,
            byte[] innerRgb,
            byte[] outerRgb,
            int innerSpeed = 40,
            int outerSpeed = 70
        )
        {
            BackRgb = backRgb;
            InnerRgb = innerRgb;
            OuterRgb = outerRgb;
            InnerSpeed = innerSpeed;
            OuterSpeed = outerSpeed;
            InnerIndexes = GetInnerIndexes(0);
            OuterIndexes = GetOuterIndexes(0);
        }

        public void Setup(IColorControl colorControl)
        {
            colorControl.SetAll(BackRgb);
            colorControl.ApplyColors();
            Task.Run(() => SetRgbPointers());
            Task.Run(() => InnerProgress(colorControl));
            Task.Run(() => OuterProgress(colorControl));
        }

        public void Unload()
        {
            IsProgressing = false;
        }

        private int[] GetInnerIndexes(int start)
        {
            return GetColorIndexes(start, 15, _innerKeys.Length - 1);
        }

        private int[] GetOuterIndexes(int start)
        {
            return GetColorIndexes(start, 22, _outerKeys.Length - 1);
        }

        private int[] GetColorIndexes(int start, int total, int maxIndex)
        {
            var indexes = new List<int> { start };

            for (int i = 0; i < total - 1; ++i)
            {
                var last = indexes.Last();
                indexes.Add(last > 0 ? last - 1 : maxIndex);
            }

            return indexes.ToArray();
        }

        private void SetRgbPointers()
        {
            while (IsProgressing)
            {
                Thread.Sleep(500);
                InnerRgbPointer = InnerRgbPointer > _rgb.Length - 2 ? 0 : InnerRgbPointer + 1;
                OuterRgbPointer = OuterRgbPointer > _rgb.Length - 2 ? 0 : OuterRgbPointer + 1;
            }
        }

        private void InnerProgress(IColorControl colorControl)
        {
            while (IsProgressing)
            {
                Thread.Sleep(InnerSpeed);
                SetColors(colorControl, _innerKeys, BackRgb);
                var start = InnerIndexes[0] > _innerKeys.Length - 2 ? 0 : InnerIndexes[0] + 1;
                InnerIndexes = GetInnerIndexes(start);
                var keys = InnerIndexes.Select(_ => _innerKeys[_]).ToArray();
                SetColors(colorControl, keys, InnerRgb);

                for (int i = 0; i < 2; ++i)
                {
                    if (i < keys.Length)
                    {
                        colorControl.SetColor(new KeyColor(keys[i], _rgb[InnerRgbPointer]));
                    }
                }

                colorControl.ApplyColors();
            }
        }

        private void OuterProgress(IColorControl colorControl)
        {
            while (IsProgressing)
            {
                Thread.Sleep(OuterSpeed);
                SetColors(colorControl, _outerKeys, BackRgb);
                var start = OuterIndexes[0] > _outerKeys.Length - 2 ? 0 : OuterIndexes[0] + 1;
                OuterIndexes = GetOuterIndexes(start);
                var keys = OuterIndexes.Select(_ => _outerKeys[_]).ToArray();
                SetColors(colorControl, keys, OuterRgb);

                for (int i = 0; i < 2; ++i)
                {
                    if (i < keys.Length)
                    {
                        colorControl.SetColor(new KeyColor(keys[0], _rgb[OuterRgbPointer]));
                    }
                }

                colorControl.ApplyColors();
            }
        }

        private void SetColors(IColorControl colorControl, IEnumerable<string> keys, byte[] color)
        {
            colorControl.SetColors(keys.Select(_ => new KeyColor(_, color)));
        }
    }
}
