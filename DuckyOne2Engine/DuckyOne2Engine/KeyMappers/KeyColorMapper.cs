using System.Collections.Generic;

namespace DuckyOne2Engine.KeyMappers
{
    public class KeyColorMapper : IKeyColorMapper
    {
        private readonly Dictionary<Keys, BytePosition[]> _keyMap;

        public KeyColorMapper()
        {
            _keyMap = new Dictionary<Keys, BytePosition[]>();
            SetBytePositions(Keys.Esc, 0, 25);
            SetBytePositions(Keys.Backtick, 0, 28);
            SetBytePositions(Keys.Tab, 0, 31);
            SetBytePositions(Keys.Caps, 0, 34);
            SetBytePositions(Keys.Lshift, 0, 37);
            SetBytePositions(Keys.Lctrl, 0, 40);
            SetBytePositions(Keys.One, 0, 46);
            SetBytePositions(Keys.Q, 0, 49);
            SetBytePositions(Keys.A, 0, 52);
            SetBytePositions(Keys.Lwindow, 0, 58);
            SetBytePositions(Keys.F1, 0, 61);
            SetBytePositions(Keys.Two, 0, 64);
            SetBytePositions(Keys.W, 1, 7);
            SetBytePositions(Keys.S, 1, 10);
            SetBytePositions(Keys.Z, 1, 13);
            SetBytePositions(Keys.Lalt, 1, 16);
            SetBytePositions(Keys.F2, 1, 19);
            SetBytePositions(Keys.Three, 1, 22);
            SetBytePositions(Keys.E, 1, 25);
            SetBytePositions(Keys.D, 1, 28);
            SetBytePositions(Keys.X, 1, 31);
            SetBytePositions(Keys.F3, 1, 37);
            SetBytePositions(Keys.Four, 1, 40);
            SetBytePositions(Keys.R, 1, 43);
            SetBytePositions(Keys.F, 1, 46);
            SetBytePositions(Keys.C, 1, 49);
            SetBytePositions(Keys.F4, 1, 55);
            SetBytePositions(Keys.Five, 1, 58);
            SetBytePositions(Keys.T, 1, 61);
            SetBytePositions(Keys.G, 1, 64);
            SetBytePositions(Keys.V, 2, 7);
            SetBytePositions(Keys.Six, 2, 16);
            SetBytePositions(Keys.Y, 2, 19);
            SetBytePositions(Keys.H, 2, 22);
            SetBytePositions(Keys.B, 2, 25);
            SetBytePositions(Keys.Space, 2, 28);
            SetBytePositions(Keys.F5, 2, 31);
            SetBytePositions(Keys.Seven, 2, 34);
            SetBytePositions(Keys.U, 2, 37);
            SetBytePositions(Keys.J, 2, 40);
            SetBytePositions(Keys.N, 2, 43);
            SetBytePositions(Keys.F6, 2, 49);
            SetBytePositions(Keys.Eight, 2, 52);
            SetBytePositions(Keys.I, 2, 55);
            SetBytePositions(Keys.K, 2, 58);
            SetBytePositions(Keys.M, 2, 61);
            SetBytePositions(Keys.F7, 3, 7);
            SetBytePositions(Keys.Nine, 3, 10);
            SetBytePositions(Keys.O, 3, 13);
            SetBytePositions(Keys.L, 3, 16);
            SetBytePositions(Keys.Comma, 3, 19);
            SetBytePositions(Keys.F8, 3, 25);
            SetBytePositions(Keys.Zero, 3, 28);
            SetBytePositions(Keys.P, 3, 31);
            SetBytePositions(Keys.Semicolon, 3, 34);
            SetBytePositions(Keys.Period, 3, 37);
            SetBytePositions(Keys.Ralt, 3, 40);
            SetBytePositions(Keys.F9, 3, 43);
            SetBytePositions(Keys.Hyphen, 3, 46);
            SetBytePositions(Keys.Lbracket, 3, 49);
            SetBytePositions(Keys.Quote, 3, 52);
            SetBytePositions(Keys.Question, 3, 55);
            SetBytePositions(Keys.F10, 3, 61);
            SetBytePositions(Keys.Equal, 3, 64);
            SetBytePositions(Keys.Rbracket, 4, 7);
            SetBytePositions(Keys.Rwindow, 4, 16);
            SetBytePositions(Keys.F11, 4, 19);
            SetBytePositions(Keys.Rshift, 4, 31);
            SetBytePositions(Keys.Fn, 4, 34);
            SetBytePositions(Keys.F12, 4, 37);
            SetBytePositions(Keys.Backspace, 4, 40);
            SetBytePositions(Keys.Pipe, 4, 43);
            SetBytePositions(Keys.Enter, 4, 46);
            SetBytePositions(Keys.Rctrl, 4, 52);
            SetBytePositions(Keys.Print, 4, 55);
            SetBytePositions(Keys.Insert, 4, 58);
            SetBytePositions(Keys.Delete, 4, 61);
            SetBytePositions(Keys.Larrow, 5, 10);
            SetBytePositions(Keys.Scroll, 5, 13);
            SetBytePositions(Keys.Home, 5, 16);
            SetBytePositions(Keys.End, 5, 19);
            SetBytePositions(Keys.Uarrow, 5, 25);
            SetBytePositions(Keys.Darrow, 5, 28);
            SetBytePositions(Keys.Pause, 5, 31);
            SetBytePositions(Keys.Pageup, 5, 34);
            SetBytePositions(Keys.Pagedown, 5, 37);
            SetBytePositions(Keys.Rarrow, 5, 46);
        }

        public IEnumerable<BytePosition> GetBytePositions(Keys key)
        {
            if (!_keyMap.ContainsKey(key))
            {
                return new BytePosition[0];
            }

            return _keyMap[key];
        }

        private void SetBytePositions(Keys key, int row, int index)
        {
            var positions = new BytePosition[3];

            for (int i = 0; i < positions.Length; ++i)
            {
                positions[i] = new BytePosition(row, index);

                if (++index > 64)
                {
                    index = 5;
                    row++;
                }
            }

            _keyMap[key] = positions;
        }
    }
}
