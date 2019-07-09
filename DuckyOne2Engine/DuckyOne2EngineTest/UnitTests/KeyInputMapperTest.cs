using DuckyOne2Engine.KeyMappers;
using HidLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;

namespace DuckyOne2EngineTest.UnitTests
{
    [TestClass]
    public class KeyInputMapperTest
    {
        private Mock<IHidDevice> _device;
        private KeyInputMapper _mapper;

        [TestInitialize]
        public void Setup()
        {
            _device = new Mock<IHidDevice>();
            _mapper = new KeyInputMapper(_device.Object);
        }

        [TestMethod]
        public void ToKeysShouldReturnEmptyCollectionWhenInputIsEmpty()
        {
            Assert.IsFalse(_mapper.ToKeys(new byte[0]).Any());
        }

        [TestMethod]
        public void ToKeysShouldReturnCorrespondingKeyForInput()
        {
            Assert.AreEqual(Keys.Esc, _mapper.ToKeys(new byte[] { 0x29 }).First());
            Assert.AreEqual(Keys.Backtick, _mapper.ToKeys(new byte[] { 0x35 }).First());
            Assert.AreEqual(Keys.Tab, _mapper.ToKeys(new byte[] { 0x2B }).First());
            Assert.AreEqual(Keys.Caps, _mapper.ToKeys(new byte[] { 0x39 }).First());
            Assert.AreEqual(Keys.Lshift, _mapper.ToKeys(new byte[] { 0x02 }).First());
            Assert.AreEqual(Keys.Lctrl, _mapper.ToKeys(new byte[] { 0x01 }).First());
            Assert.AreEqual(Keys.One, _mapper.ToKeys(new byte[] { 0x1E }).First());
            Assert.AreEqual(Keys.Q, _mapper.ToKeys(new byte[] { 0x14 }).First());
            Assert.AreEqual(Keys.A, _mapper.ToKeys(new byte[] { 0x04 }).First());
            Assert.AreEqual(Keys.Lwindow, _mapper.ToKeys(new byte[] { 0x08 }).First());
            Assert.AreEqual(Keys.F1, _mapper.ToKeys(new byte[] { 0x3A }).First());
            Assert.AreEqual(Keys.Two, _mapper.ToKeys(new byte[] { 0x1F }).First());
            Assert.AreEqual(Keys.W, _mapper.ToKeys(new byte[] { 0x1A }).First());
            Assert.AreEqual(Keys.S, _mapper.ToKeys(new byte[] { 0x16 }).First());
            Assert.AreEqual(Keys.Z, _mapper.ToKeys(new byte[] { 0x1D }).First());
            Assert.AreEqual(Keys.Lalt, _mapper.ToKeys(new byte[] { 0x04 }).First());
            Assert.AreEqual(Keys.F2, _mapper.ToKeys(new byte[] { 0x3B }).First());
            Assert.AreEqual(Keys.Three, _mapper.ToKeys(new byte[] { 0x20 }).First());
            Assert.AreEqual(Keys.E, _mapper.ToKeys(new byte[] { 0x08 }).First());
            Assert.AreEqual(Keys.D, _mapper.ToKeys(new byte[] { 0x07 }).First());
            Assert.AreEqual(Keys.X, _mapper.ToKeys(new byte[] { 0x1B }).First());
            Assert.AreEqual(Keys.F3, _mapper.ToKeys(new byte[] { 0x3C }).First());
            Assert.AreEqual(Keys.Four, _mapper.ToKeys(new byte[] { 0x21 }).First());
            Assert.AreEqual(Keys.R, _mapper.ToKeys(new byte[] { 0x15 }).First());
            Assert.AreEqual(Keys.F, _mapper.ToKeys(new byte[] { 0x09 }).First());
            Assert.AreEqual(Keys.C, _mapper.ToKeys(new byte[] { 0x06 }).First());
            Assert.AreEqual(Keys.F4, _mapper.ToKeys(new byte[] { 0x3D }).First());
            Assert.AreEqual(Keys.Five, _mapper.ToKeys(new byte[] { 0x22 }).First());
            Assert.AreEqual(Keys.T, _mapper.ToKeys(new byte[] { 0x17 }).First());
            Assert.AreEqual(Keys.G, _mapper.ToKeys(new byte[] { 0x0A }).First());
            Assert.AreEqual(Keys.V, _mapper.ToKeys(new byte[] { 0x19 }).First());
            Assert.AreEqual(Keys.Six, _mapper.ToKeys(new byte[] { 0x23 }).First());
            Assert.AreEqual(Keys.Y, _mapper.ToKeys(new byte[] { 0x1C }).First());
            Assert.AreEqual(Keys.H, _mapper.ToKeys(new byte[] { 0x0B }).First());
            Assert.AreEqual(Keys.B, _mapper.ToKeys(new byte[] { 0x05 }).First());
            Assert.AreEqual(Keys.Space, _mapper.ToKeys(new byte[] { 0x2C }).First());
            Assert.AreEqual(Keys.F5, _mapper.ToKeys(new byte[] { 0x3E }).First());
            Assert.AreEqual(Keys.Seven, _mapper.ToKeys(new byte[] { 0x24 }).First());
            Assert.AreEqual(Keys.U, _mapper.ToKeys(new byte[] { 0x18 }).First());
            Assert.AreEqual(Keys.J, _mapper.ToKeys(new byte[] { 0x0D }).First());
            Assert.AreEqual(Keys.N, _mapper.ToKeys(new byte[] { 0x11 }).First());
            Assert.AreEqual(Keys.F6, _mapper.ToKeys(new byte[] { 0x3F }).First());
            Assert.AreEqual(Keys.Eight, _mapper.ToKeys(new byte[] { 0x25 }).First());
            Assert.AreEqual(Keys.I, _mapper.ToKeys(new byte[] { 0x0C }).First());
            Assert.AreEqual(Keys.K, _mapper.ToKeys(new byte[] { 0x0E }).First());
            Assert.AreEqual(Keys.M, _mapper.ToKeys(new byte[] { 0x10 }).First());
            Assert.AreEqual(Keys.F7, _mapper.ToKeys(new byte[] { 0x40 }).First());
            Assert.AreEqual(Keys.Nine, _mapper.ToKeys(new byte[] { 0x26 }).First());
            Assert.AreEqual(Keys.O, _mapper.ToKeys(new byte[] { 0x27 }).First());
            Assert.AreEqual(Keys.L, _mapper.ToKeys(new byte[] { 0x0F }).First());
            Assert.AreEqual(Keys.Comma, _mapper.ToKeys(new byte[] { 0x36 }).First());
            Assert.AreEqual(Keys.F8, _mapper.ToKeys(new byte[] { 0x41 }).First());
            Assert.AreEqual(Keys.Zero, _mapper.ToKeys(new byte[] { 0x27 }).First());
            Assert.AreEqual(Keys.P, _mapper.ToKeys(new byte[] { 0x13 }).First());
            Assert.AreEqual(Keys.Semicolon, _mapper.ToKeys(new byte[] { 0x33 }).First());
            Assert.AreEqual(Keys.Period, _mapper.ToKeys(new byte[] { 0x37 }).First());
            Assert.AreEqual(Keys.Ralt, _mapper.ToKeys(new byte[] { 0x40 }).First());
            Assert.AreEqual(Keys.F9, _mapper.ToKeys(new byte[] { 0x42 }).First());
            Assert.AreEqual(Keys.Hyphen, _mapper.ToKeys(new byte[] { 0x2D }).First());
            Assert.AreEqual(Keys.Lbracket,_mapper.ToKeys(new byte[] { 0x2F }).First());
            Assert.AreEqual(Keys.Quote, _mapper.ToKeys(new byte[] { 0x34 }).First());
            Assert.AreEqual(Keys.Question, _mapper.ToKeys(new byte[] { 0x38 }).First());
            Assert.AreEqual(Keys.F10, _mapper.ToKeys(new byte[] { 0x43 }).First());
            Assert.AreEqual(Keys.Equal, _mapper.ToKeys(new byte[] { 0x2E }).First());
            Assert.AreEqual(Keys.Rbracket, _mapper.ToKeys(new byte[] { 0x30 }).First());
            Assert.AreEqual(Keys.Rwindow, _mapper.ToKeys(new byte[] { 0x80 }).First());
            Assert.AreEqual(Keys.F11, _mapper.ToKeys(new byte[] { 0x44 }).First());
            Assert.AreEqual(Keys.Rshift, _mapper.ToKeys(new byte[] { 0x20 }).First());
            Assert.AreEqual(Keys.F12, _mapper.ToKeys(new byte[] { 0x45 }).First());
            Assert.AreEqual(Keys.Backspace, _mapper.ToKeys(new byte[] { 0x2A }).First());
            Assert.AreEqual(Keys.Pipe, _mapper.ToKeys(new byte[] { 0x31 }).First());
            Assert.AreEqual(Keys.Enter, _mapper.ToKeys(new byte[] { 0x28 }).First());
            Assert.AreEqual(Keys.Rctrl, _mapper.ToKeys(new byte[] { 0x10 }).First());
            Assert.AreEqual(Keys.Print, _mapper.ToKeys(new byte[] { 0x46 }).First());
            Assert.AreEqual(Keys.Insert, _mapper.ToKeys(new byte[] { 0x49 }).First());
            Assert.AreEqual(Keys.Delete, _mapper.ToKeys(new byte[] { 0x4C }).First());
            Assert.AreEqual(Keys.Larrow, _mapper.ToKeys(new byte[] { 0x50 }).First());
            Assert.AreEqual(Keys.Scroll, _mapper.ToKeys(new byte[] { 0x47 }).First());
            Assert.AreEqual(Keys.Home, _mapper.ToKeys(new byte[] { 0x4A }).First());
            Assert.AreEqual(Keys.End, _mapper.ToKeys(new byte[] { 0x4D }).First());
            Assert.AreEqual(Keys.Uarrow, _mapper.ToKeys(new byte[] { 0x52 }).First());
            Assert.AreEqual(Keys.Darrow, _mapper.ToKeys(new byte[] { 0x51 }).First());
            Assert.AreEqual(Keys.Pause, _mapper.ToKeys(new byte[] { 0x48 }).First());
            Assert.AreEqual(Keys.Pageup, _mapper.ToKeys(new byte[] { 0x4B }).First());
            Assert.AreEqual(Keys.Pagedown, _mapper.ToKeys(new byte[] { 0x4E }).First());
            Assert.AreEqual(Keys.Rarrow, _mapper.ToKeys(new byte[] { 0x4F }).First());
        }

        [TestMethod]
        public void ToKeysShouldReturnCorrespondingKeysForInputs()
        {
            var expected = new[] { Keys.Tab, Keys.A, Keys.F1, Keys.Space };
            byte[] inputs = { 0x2B, 0x04, 0x3A, 0x2C };

            var result = _mapper.ToKeys(inputs);

            CollectionAssert.AreEqual(expected, result.ToArray());
        }
    }
}
