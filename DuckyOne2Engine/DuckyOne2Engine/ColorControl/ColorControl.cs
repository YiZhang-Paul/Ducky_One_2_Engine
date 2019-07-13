using DuckyOne2Engine.HidDevices;
using DuckyOne2Engine.KeyMappers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckyOne2Engine.ColorControl
{
    public class ColorControl : IColorControl
    {
        private IHidDevice Device { get; }
        private IKeyColorMapper Mapper { get; }
        private byte[][] CurrentReport { get; }

        private byte[][] BlankReport
        {
            get
            {
                var report = new List<byte[]>();
                AddOpenRequest(report);
                AddColorRequests(report);
                AddCloseRequest(report);

                return report.ToArray();
            }
        }

        public ColorControl(IHidDevice device, IKeyColorMapper mapper)
        {
            Device = device;
            Mapper = mapper;
            CurrentReport = BlankReport;
        }

        public bool SetColor(Tuple<string, byte[]> color)
        {
            var positions = Mapper.GetBytePositions(color.Item1).ToArray();

            if (!positions.Any())
            {
                return false;
            }

            for (int i = 0; i < positions.Length; ++i)
            {
                var row = positions[i].Row + 1;
                var index = positions[i].Index;
                CurrentReport[row][index] = color.Item2[i];
            }

            return true;
        }

        public bool SetColors(IEnumerable<Tuple<string, byte[]>> colors)
        {
            bool result = true;

            foreach (var color in colors)
            {
                result = result && SetColor(color);
            }

            return result;
        }

        public void SetAll(byte[] color)
        {
            foreach (var key in Keys.AllKeys)
            {
                SetColor(new Tuple<string, byte[]>(key, color));
            }
        }

        public void ApplyColors()
        {
            foreach (var input in CurrentReport)
            {
                Device.Write(input);
            }
        }

        private void AddOpenRequest(List<byte[]> report)
        {
            byte[] request = new byte[65];
            request[1] = 0x56;
            request[2] = 0x81;
            request[5] = 0x01;
            request[9] = 0x07;
            request[13] = 0xAA;
            request[14] = 0xAA;
            request[15] = 0xAA;
            request[16] = 0xAA;
            report.Add(request);
        }

        private void AddColorRequests(List<byte[]> report)
        {
            var requests = new List<byte[]>();

            for (int i = 0; i < 7; ++i)
            {
                byte[] request = new byte[65];
                request[1] = 0x56;
                request[2] = 0x83;
                request[3] = Convert.ToByte(i);
                requests.Add(request);
            }

            requests[0][5] = 0x01;
            requests[0][9] = 0x80;
            requests[0][10] = 0x01;
            requests[0][12] = 0xC1;
            requests[0][17] = 0xFF;
            requests[0][18] = 0xFF;
            requests[0][19] = 0xFF;
            requests[0][20] = 0xFF;
            report.AddRange(requests);
        }

        private void AddCloseRequest(List<byte[]> report)
        {
            byte[] request = new byte[65];
            request[1] = 0x51;
            request[2] = 0x28;
            request[5] = 0xFF;
            report.Add(request);
        }
    }
}
