namespace DuckyOne2Engine.ColorControls
{
    public class KeyColor
    {
        public string Key { get; set; }
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }

        public KeyColor(string key, byte[] rgb)
        {
            Key = key;
            R = rgb[0];
            G = rgb[1];
            B = rgb[2];
        }
    }
}
