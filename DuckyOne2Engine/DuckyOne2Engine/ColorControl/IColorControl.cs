using System.Collections.Generic;

namespace DuckyOne2Engine.ColorControl
{
    public interface IColorControl
    {
        bool SetColor(KeyColor color);
        bool SetColors(IEnumerable<KeyColor> colors);
        void SetAll(byte[] color);
        void ApplyColors();
    }
}
