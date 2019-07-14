using DuckyOne2Engine.ColorControls;

namespace DuckyOne2Engine.DuckyDevices.ColorModes
{
    public interface IColorMode
    {
        void Setup(IColorControl colorControl);
        void Unload();
    }
}
