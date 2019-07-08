using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace UsbReportParser
{
    public class Parser
    {
        public IEnumerable<string> GetOutput(string rawReport)
        {
            var output = new List<string>();
            var lines = rawReport.Split('\n').Select(_ => _.Trim()).ToArray();

            for (int i = 0; i < lines.Length; ++i)
            {
                if (Regex.IsMatch(lines[i], @"Output Report"))
                {
                    var report = new StringBuilder();

                    for (int j = 0; j < 4; ++j)
                    {
                        if (++i < lines.Length)
                        {
                            report.Append($" {ExtractBytes(lines[i])}");
                        }
                    }

                    output.Add(report.ToString().Trim());
                }
            }

            return output;
        }

        private string ExtractBytes(string rawData)
        {
            return Regex.Match(rawData, @"(?=\s{2}).*(?<=\s{2})").Value.Trim();
        }
    }
}
