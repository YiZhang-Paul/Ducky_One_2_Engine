namespace DuckyOne2Engine.Dtos
{
    public class ProgressModeDto
    {
        public string BackRgb { get; set; }
        public string ProgressRgb { get; set; }
        public string[] Secondary { get; set; } = new string[0];
        public int Speed { get; set; }
    }
}
