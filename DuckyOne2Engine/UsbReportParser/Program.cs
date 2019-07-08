using System;
using System.IO;

namespace UsbReportParser
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var parser = new Parser();
                var file = File.ReadAllText("raw_report.txt");
                var input = parser.GetInput(file);
                var output = parser.GetOutput(file);
                File.WriteAllLines("input.txt", input);
                File.WriteAllLines("output.txt", output);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
