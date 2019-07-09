using System.Collections.Generic;

namespace DuckyOne2Engine.KeyMappers
{
    public interface IKeyInputMapper
    {
        IEnumerable<string> KeyPressed { get; }
        IEnumerable<string> ToKeys(byte[] inputs);
    }
}
