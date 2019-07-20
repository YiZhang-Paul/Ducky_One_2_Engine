using DuckyOne2Engine.ColorControls;
using DuckyOne2Engine.KeyMappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DuckyOne2Engine.DuckyDevices.ColorModes
{
    public class ProgressMode : IColorMode
    {
        private readonly int _totalActivePrimary = 18;

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

        private readonly Tuple<byte[], byte[]>[] _secondaryRgbs =
        {
            new Tuple<byte[], byte[]>(new byte[] { 0, 0, 85 }, new byte[] { 0, 0, 255 }), 
            new Tuple<byte[], byte[]>(new byte[] { 0, 85, 85 }, new byte[] { 0, 255, 255 }), 
            new Tuple<byte[], byte[]>(new byte[] { 22, 35, 75 }, new byte[] { 65, 105, 225 }), 
            new Tuple<byte[], byte[]>(new byte[] { 0, 85, 0 }, new byte[] { 0, 255, 0 }), 
            new Tuple<byte[], byte[]>(new byte[] { 85, 0, 85 }, new byte[] { 255, 0, 255 }),
            new Tuple<byte[], byte[]>(new byte[] { 85, 0, 0 }, new byte[] { 255, 0, 0 }), 
            new Tuple<byte[], byte[]>(new byte[] { 85, 23, 0 }, new byte[] { 255, 69, 0 }),
            new Tuple<byte[], byte[]>(new byte[] { 85, 54, 41 }, new byte[] { 255, 160, 122 }),
            new Tuple<byte[], byte[]>(new byte[] { 68, 68, 0 }, new byte[] { 204, 204, 0 })
        };

        private int Speed { get; }
        private int CurrentStep { get; set; }
        private int SecondaryRgbPointer { get; set; }
        private List<int> ActivePrimary { get; set; }
        private List<byte[]> SecondaryRgbs { get; set; }
        private bool IsProgressing { get; set; } = true;
        private byte[] BackRgb { get; }
        private byte[] ProgressRgb { get; }

        public ProgressMode(byte[] backRgb, byte[] progressRgb, int speed = 25)
        {
            BackRgb = backRgb;
            ProgressRgb = progressRgb;
            Speed = speed;
            ActivePrimary = GetIndexes(0, _totalActivePrimary, _primaryKeys.Length - 1);
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
                SetPrimaryColors(colorControl);
                SetSecondaryColors(colorControl);
                colorControl.ApplyColors();
            }
        }

        private void SetPrimaryColors(IColorControl colorControl)
        {
            var start = ActivePrimary[0] > _primaryKeys.Length - 2 ? 0 : ActivePrimary[0] + 1;
            ActivePrimary = GetIndexes(start, _totalActivePrimary, _primaryKeys.Length - 1);
            colorControl.SetColors(ActivePrimary.Select(_ => _primaryKeys[_]), ProgressRgb);
        }

        private void SetSecondaryColors(IColorControl colorControl)
        {
            SecondaryRgbs = SecondaryRgbs ?? GetSecondaryColors();
            SecondaryRgbs = SecondaryRgbs.Skip(1).Concat(SecondaryRgbs.Take(1)).ToList();

            for (int i = 0; i < _secondaryKeys.Length; ++i)
            {
                colorControl.SetColor(new KeyColor(_secondaryKeys[i], SecondaryRgbs[i]));
            }

            if (++CurrentStep == 10)
            {
                CurrentStep = 0;
                SecondaryRgbs = GetSecondaryColors();
            }
        }

        private List<byte[]> GetSecondaryColors()
        {
            var random = new Random();
            SecondaryRgbPointer = SecondaryRgbPointer > _secondaryRgbs.Length - 2 ? 0 : ++SecondaryRgbPointer;
            var colorPair = _secondaryRgbs[SecondaryRgbPointer];
            var colors = Enumerable.Repeat(colorPair.Item1, _secondaryKeys.Length).ToList();

            for (int i = 0; i < 4; ++i)
            {
                var start = i * 8 + random.Next(3);
                var end = start + Math.Max(3, random.Next(4));

                for (int j = start; j <= end; ++j)
                {
                    colors[j] = colorPair.Item2;
                }
            }

            return colors;
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
