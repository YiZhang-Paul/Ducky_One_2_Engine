using DuckyOne2Engine.DuckyDevices;
using Gma.System.MouseKeyHook;

namespace DuckyOne2Engine
{
    public static class Cache
    {
        public static DuckyDevice ActiveDuckyDevice { get; set; }
        public static IKeyboardMouseEvents GlobalKeyboardEvents { get; set; }
    }
}
