using System;
using System.Collections.Generic;

namespace DuckyOne2Engine.ColorControl
{
    public interface IColorControl
    {
        bool SetColor(Tuple<string, byte[]> color);
        bool SetColors(IEnumerable<Tuple<string, byte[]>> colors);
        void SetAll(byte[] color);
        void ApplyColors();
    }
}
