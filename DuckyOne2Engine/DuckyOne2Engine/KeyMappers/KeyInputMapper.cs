using HidLibrary;
using System.Collections.Generic;

namespace DuckyOne2Engine.KeyMappers
{
    public class KeyInputMapper : IKeyInputMapper
    {
        public IEnumerable<string> KeyPressed => new string[0];

        private readonly Dictionary<byte, string> _keyMap;
        private IHidDevice Device { get; }

        public KeyInputMapper(IHidDevice device)
        {
            _keyMap = new Dictionary<byte, string>
            {

            };

            Device = device;
        }

        public IEnumerable<string> ToKeys(byte[] inputs)
        {
            return new string[0];
        }
    }
}
