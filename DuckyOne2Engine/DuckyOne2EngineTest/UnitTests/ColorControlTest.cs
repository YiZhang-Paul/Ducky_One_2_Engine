using DuckyOne2Engine;
using DuckyOne2Engine.HidDevices;
using DuckyOne2Engine.KeyMappers;
using DuckyOne2EngineTest.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace DuckyOne2EngineTest.UnitTests
{
    [TestClass]
    public class ColorControlTest
    {
        private Tuple<Keys, byte[]>[] _colors;
        private BytePosition[][] _positions;
        private Mock<IHidDevice> _device;
        private Mock<IKeyColorMapper> _mapper;
        private ColorControl _control;

        [TestInitialize]
        public void Setup()
        {
            _colors = new[]
            {
                new Tuple<Keys, byte[]>(Keys.G, new byte[] {0, 0, 255}),
                new Tuple<Keys, byte[]>(Keys.Five, new byte[] {255, 255, 0}),
                new Tuple<Keys, byte[]>(Keys.Space, new byte[] {255, 0, 255})
            };

            _positions = new[]
            {
                new[]
                {
                    new BytePosition(1, 64),
                    new BytePosition(2, 5),
                    new BytePosition(2, 6)
                },
                new[]
                {
                    new BytePosition(1, 58),
                    new BytePosition(1, 59),
                    new BytePosition(1, 60)
                },
                new[]
                {
                    new BytePosition(2, 28),
                    new BytePosition(2, 29),
                    new BytePosition(2, 30)
                }
            };

            _device = new Mock<IHidDevice>();
            _mapper = new Mock<IKeyColorMapper>();
            _control = new ColorControl(_device.Object, _mapper.Object);
        }

        [TestMethod]
        public void SetColorShouldNotSendRequestsToUsbDevice()
        {
            _control.SetColor(_colors[0]);

            _device.Verify(x => x.Write(It.IsAny<byte[]>()), Times.Never);
        }

        [TestMethod]
        public void SetColorShouldSetColorOnReport()
        {
            var expected = new []
            {
                "00 56 81 00 00 01 00 00 00 07 00 00 00 AA AA AA AA 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00",
                "00 56 83 00 00 01 00 00 00 80 01 00 C1 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00",
                "00 56 83 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF 00 00 00 00 00",
                "00 56 83 02 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00",
                "00 56 83 03 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00",
                "00 56 83 04 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00",
                "00 56 83 05 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00",
                "00 56 83 06 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00",
                "00 51 28 00 00 FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00"
            };

            var color = _colors[1];
            var bytes = MockDevice.SetupWrite(_device);
            _mapper.Setup(x => x.GetBytePositions(color.Item1)).Returns(_positions[1]);

            _control.SetColor(color);
            _control.ApplyColors();

            Assert.AreEqual(string.Join(" ", expected), bytes.ToString().Trim());
        }

        [TestMethod]
        public void SetColorsShouldNotSendRequestsToUsbDevice()
        {
            _control.SetColors(_colors);

            _device.Verify(x => x.Write(It.IsAny<byte[]>()), Times.Never);
        }

        [TestMethod]
        public void SetColorsShouldSetColorsOnReport()
        {
            var expected = new[]
            {
                "00 56 81 00 00 01 00 00 00 07 00 00 00 AA AA AA AA 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00",
                "00 56 83 00 00 01 00 00 00 80 01 00 C1 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00",
                "00 56 83 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF 00 00 00 00 00",
                "00 56 83 02 00 00 FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF 00 FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00",
                "00 56 83 03 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00",
                "00 56 83 04 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00",
                "00 56 83 05 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00",
                "00 56 83 06 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00",
                "00 51 28 00 00 FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00"
            };

            var bytes = MockDevice.SetupWrite(_device);

            _mapper.SetupSequence(x => x.GetBytePositions(It.IsAny<Keys>()))
                .Returns(_positions[0])
                .Returns(_positions[1])
                .Returns(_positions[2]);

            _control.SetColors(_colors);
            _control.ApplyColors();

            Assert.AreEqual(string.Join(" ", expected), bytes.ToString().Trim());
        }

        [TestMethod]
        public void ApplyColorsShouldSendRequestsToUsbDevice()
        {
            _control.ApplyColors();

            _device.Verify(x => x.Write(It.IsAny<byte[]>()), Times.Exactly(9));
        }

        [TestMethod]
        public void ApplyColorsShouldSendBlankReportOnDefault()
        {
            var expected = new[]
            {
                "00 56 81 00 00 01 00 00 00 07 00 00 00 AA AA AA AA 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00",
                "00 56 83 00 00 01 00 00 00 80 01 00 C1 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00",
                "00 56 83 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00",
                "00 56 83 02 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00",
                "00 56 83 03 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00",
                "00 56 83 04 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00",
                "00 56 83 05 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00",
                "00 56 83 06 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00",
                "00 51 28 00 00 FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00"
            };

            var bytes = MockDevice.SetupWrite(_device);

            _control.ApplyColors();

            Assert.AreEqual(string.Join(" ", expected), bytes.ToString().Trim());
        }
    }
}
