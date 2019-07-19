using System.Collections.Generic;

namespace DuckyOne2Engine.ColorControls
{
    public interface IColorControl
    {
        bool SetColor(KeyColor color);
        bool SetColors(IEnumerable<KeyColor> colors);
        bool SetColors(IEnumerable<string> keys, byte[] color);
        void SetAll(byte[] color);
        void ApplyColors();
    }
}
