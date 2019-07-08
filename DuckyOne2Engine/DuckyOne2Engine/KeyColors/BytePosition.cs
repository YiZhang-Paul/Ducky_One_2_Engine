namespace DuckyOne2Engine.KeyColors
{
    public class BytePosition
    {
        public int Row { get; set; }
        public int Index { get; set; }

        public BytePosition(int row, int index)
        {
            Row = row;
            Index = index;
        }

        public override string ToString()
        {
            return $"{Row}-{Index}";
        }
    }
}
