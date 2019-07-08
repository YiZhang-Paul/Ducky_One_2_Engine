using System.Collections.Generic;

namespace DuckyOne2Engine.KeyColors
{
    public class KeyColorMapper : IKeyColorMapper
    {
        private readonly Dictionary<string, BytePosition[]> _keyMap;

        public KeyColorMapper()
        {
            _keyMap = new Dictionary<string, BytePosition[]>();
            SetBytePositions("ESC", 0, 25);
            SetBytePositions("`", 0, 28);
            SetBytePositions("TAB", 0, 31);
            SetBytePositions("CAPS", 0, 34);
            SetBytePositions("LSHIFT", 0, 37);
            SetBytePositions("LCTRL", 0, 40);
            SetBytePositions("1", 0, 46);
            SetBytePositions("Q", 0, 49);
            SetBytePositions("A", 0, 52);
            SetBytePositions("LWindow", 0, 58);
            SetBytePositions("F1", 0, 61);
            SetBytePositions("2", 0, 64);
            SetBytePositions("W", 1, 7);
            SetBytePositions("S", 1, 10);
            SetBytePositions("Z", 1, 13);
            SetBytePositions("LALT", 1, 16);
            SetBytePositions("F2", 1, 19);
            SetBytePositions("3", 1, 22);
            SetBytePositions("E", 1, 25);
            SetBytePositions("D", 1, 28);
            SetBytePositions("X", 1, 31);
            SetBytePositions("F3", 1, 37);
            SetBytePositions("4", 1, 40);
            SetBytePositions("R", 1, 43);
            SetBytePositions("F", 1, 46);
            SetBytePositions("C", 1, 49);
            SetBytePositions("F4", 1, 55);
            SetBytePositions("5", 1, 58);
            SetBytePositions("T", 1, 61);
            SetBytePositions("G", 1, 64);
            SetBytePositions("V", 2, 7);
            SetBytePositions("6", 2, 16);
            SetBytePositions("Y", 2, 19);
            SetBytePositions("H", 2, 22);
            SetBytePositions("B", 2, 25);
            SetBytePositions("SPACE", 2, 28);
            SetBytePositions("F5", 2, 31);
            SetBytePositions("7", 2, 34);
            SetBytePositions("U", 2, 37);
            SetBytePositions("J", 2, 40);
            SetBytePositions("N", 2, 43);
            SetBytePositions("F6", 2, 49);
            SetBytePositions("8", 2, 52);
            SetBytePositions("I", 2, 55);
            SetBytePositions("K", 2, 58);
            SetBytePositions("M", 2, 61);
            SetBytePositions("F7", 3, 7);
            SetBytePositions("9", 3, 10);
            SetBytePositions("O", 3, 13);
            SetBytePositions("L", 3, 16);
            SetBytePositions(",", 3, 19);
            SetBytePositions("F8", 3, 25);
            SetBytePositions("0", 3, 28);
            SetBytePositions("P", 3, 31);
            SetBytePositions(";", 3, 34);
            SetBytePositions(".", 3, 37);
            SetBytePositions("RALT", 3, 40);
            SetBytePositions("F9", 3, 43);
            SetBytePositions("-", 3, 46);
            SetBytePositions("[", 3, 49);
            SetBytePositions("'", 3, 52);
            SetBytePositions("?", 3, 55);
            SetBytePositions("F10", 3, 61);
            SetBytePositions("=", 3, 64);
            SetBytePositions("]", 4, 7);
            SetBytePositions("RWindow", 4, 16);
            SetBytePositions("F11", 4, 19);
            SetBytePositions("RSHIFT", 4, 31);
            SetBytePositions("FN", 4, 34);
            SetBytePositions("F12", 4, 37);
            SetBytePositions("BACKSPACE", 4, 40);
            SetBytePositions("|", 4, 43);
            SetBytePositions("ENTER", 4, 46);
            SetBytePositions("RCTRL", 4, 52);
            SetBytePositions("PRINT", 4, 55);
            SetBytePositions("INSERT", 4, 58);
            SetBytePositions("DELETE", 4, 61);
            SetBytePositions("LARROW", 5, 10);
            SetBytePositions("SCROLL", 5, 13);
            SetBytePositions("HOME", 5, 16);
            SetBytePositions("END", 5, 19);
            SetBytePositions("UARROW", 5, 25);
            SetBytePositions("DARROW", 5, 28);
            SetBytePositions("PAUSE", 5, 31);
            SetBytePositions("PAGEUP", 5, 34);
            SetBytePositions("PAGEDOWN", 5, 37);
            SetBytePositions("RARROW", 5, 46);
        }

        public IEnumerable<BytePosition> GetBytePositions(string key)
        {
            if (!_keyMap.ContainsKey(key))
            {
                return new BytePosition[0];
            }

            return _keyMap[key];
        }

        private void SetBytePositions(string key, int row, int index)
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
