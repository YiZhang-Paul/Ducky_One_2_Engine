using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace UsbReportParser
{
    public class Parser
    {
        public IEnumerable<string> GetInput(string rawReport)
        {
            return GetReports(rawReport, @"Input Report");
        }

        public IEnumerable<string> GetOutput(string rawReport)
        {
            return GetReports(rawReport, @"Output Report");
        }

        private IEnumerable<string> GetReports(string rawReport, string pattern)
        {
            var reports = new List<string>();
            var lines = SplitToLines(rawReport);

            for (int i = 0; i < lines.Length; ++i)
            {
                if (Regex.IsMatch(lines[i], pattern))
                {
                    reports.Add(ExtractAllBytes(lines, ref i));
                }
            }

            return reports;
        }

        private string[] SplitToLines(string rawReport)
        {
            return rawReport.Split('\n').Select(_ => _.Trim()).ToArray();
        }

        private string ExtractAllBytes(string[] lines, ref int index)
        {
            var bytes = new StringBuilder();

            for (int i = 0; i < 4; ++i)
            {
                if (++index < lines.Length)
                {
                    bytes.Append($" {ExtractBytes(lines[index])}");
                }
            }

            return bytes.ToString().Trim();
        }

        private string ExtractBytes(string rawData)
        {
            return Regex.Match(rawData, @"(?=\s{2}).*(?<=\s{2})").Value.Trim();
        }
    }
}
