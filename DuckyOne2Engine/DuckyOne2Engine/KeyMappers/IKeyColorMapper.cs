using System.Collections.Generic;

namespace DuckyOne2Engine.KeyMappers
{
    public interface IKeyColorMapper
    {
        IEnumerable<BytePosition> GetBytePositions(Keys key);
    }
}
