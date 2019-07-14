using DuckyOne2Engine.ColorControls;
using DuckyOne2Engine.HidDevices;
using DuckyOne2Engine.KeyMappers;
using DuckyOne2EngineTest.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DuckyOne2EngineTest.IntegrationTests
{
    [TestClass]
    public class ColorControlTest
    {
        private Mock<IHidDevice> _device;
        private ColorControl _control;

        [TestInitialize]
        public void Setup()
        {
            _device = new Mock<IHidDevice>();
            _control = new ColorControl(_device.Object, new KeyColorMapper());
        }

        [TestMethod]
        public void SetAllShouldSetColorForAllKeys()
        {
            var expected = new[]
            {
                "00 56 81 00 00 01 00 00 00 07 00 00 00 AA AA AA AA 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00",
                "00 56 83 00 00 01 00 00 00 80 01 00 C1 00 00 00 00 FF FF FF FF 00 00 00 00 55 56 57 55 56 57 55 56 57 55 56 57 55 56 57 55 56 57 00 00 00 55 56 57 55 56 57 55 56 57 00 00 00 55 56 57 55 56 57 55",
                "00 56 83 01 00 56 57 55 56 57 55 56 57 55 56 57 55 56 57 55 56 57 55 56 57 55 56 57 55 56 57 55 56 57 00 00 00 55 56 57 55 56 57 55 56 57 55 56 57 55 56 57 00 00 00 55 56 57 55 56 57 55 56 57 55",
                "00 56 83 02 00 56 57 55 56 57 00 00 00 00 00 00 55 56 57 55 56 57 55 56 57 55 56 57 55 56 57 55 56 57 55 56 57 55 56 57 55 56 57 55 56 57 00 00 00 55 56 57 55 56 57 55 56 57 55 56 57 55 56 57 00",
                "00 56 83 03 00 00 00 55 56 57 55 56 57 55 56 57 55 56 57 55 56 57 00 00 00 55 56 57 55 56 57 55 56 57 55 56 57 55 56 57 55 56 57 55 56 57 55 56 57 55 56 57 55 56 57 55 56 57 00 00 00 55 56 57 55",
                "00 56 83 04 00 56 57 55 56 57 00 00 00 00 00 00 55 56 57 55 56 57 00 00 00 00 00 00 00 00 00 55 56 57 55 56 57 55 56 57 55 56 57 55 56 57 55 56 57 00 00 00 55 56 57 55 56 57 55 56 57 55 56 57 00",
                "00 56 83 05 00 00 00 00 00 00 55 56 57 55 56 57 55 56 57 55 56 57 00 00 00 55 56 57 55 56 57 55 56 57 55 56 57 55 56 57 00 00 00 00 00 00 55 56 57 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00",
                "00 56 83 06 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00",
                "00 51 28 00 00 FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00"
            };

            var bytes = MockDevice.SetupWrite(_device);

            _control.SetAll(new byte[] { 0x55, 0x56, 0x57 });
            _control.ApplyColors();

            Assert.AreEqual(string.Join(" ", expected), bytes.ToString().Trim());
        }
    }
}
