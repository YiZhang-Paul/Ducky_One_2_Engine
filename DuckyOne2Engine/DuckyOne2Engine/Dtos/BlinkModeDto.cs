namespace DuckyOne2Engine.Dtos
{
    public class BlinkModeDto
    {
        public string BackRgb { get; set; }
        public string BlinkRgb { get; set; }
        public string SpecialRgb { get; set; }
        public string[] SpecialKeys { get; set; }
        public int Interval { get; set; }
    }
}
