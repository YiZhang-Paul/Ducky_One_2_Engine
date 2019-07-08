using System.Collections.Generic;

namespace DuckyOne2Engine.KeyColors
{
    public interface IKeyColorMapper
    {
        IEnumerable<BytePosition> GetBytePositions(string key);
    }
}
