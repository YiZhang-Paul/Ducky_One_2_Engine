using System.Collections.Generic;
using System.Linq;
using DuckyOne2Engine.KeyMappers;
using HidLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

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
            Assert.AreEqual("ESC", _mapper.ToKeys(new byte[] { 0x29 }).First());
            Assert.AreEqual("`", _mapper.ToKeys(new byte[] { 0x35 }).First());
            Assert.AreEqual("TAB", _mapper.ToKeys(new byte[] { 0x2B }).First());
            Assert.AreEqual("CAPS", _mapper.ToKeys(new byte[] { 0x39 }).First());
            Assert.AreEqual("LSHIFT", _mapper.ToKeys(new byte[] { 0x02 }).First());
            Assert.AreEqual("LCTRL", _mapper.ToKeys(new byte[] { 0x01 }).First());
            Assert.AreEqual("1", _mapper.ToKeys(new byte[] { 0x1E }).First());
            Assert.AreEqual("Q", _mapper.ToKeys(new byte[] { 0x14 }).First());
            Assert.AreEqual("A", _mapper.ToKeys(new byte[] { 0x04 }).First());
            Assert.AreEqual("LWINDOW", _mapper.ToKeys(new byte[] { 0x08 }).First());
            Assert.AreEqual("F1", _mapper.ToKeys(new byte[] { 0x3A }).First());
            Assert.AreEqual("2", _mapper.ToKeys(new byte[] { 0x1F }).First());
            Assert.AreEqual("W", _mapper.ToKeys(new byte[] { 0x1A }).First());
            Assert.AreEqual("S", _mapper.ToKeys(new byte[] { 0x16 }).First());
            Assert.AreEqual("Z", _mapper.ToKeys(new byte[] { 0x1D }).First());
            Assert.AreEqual("LALT", _mapper.ToKeys(new byte[] { 0x04 }).First());
            Assert.AreEqual("F2", _mapper.ToKeys(new byte[] { 0x3B }).First());
            Assert.AreEqual("3", _mapper.ToKeys(new byte[] { 0x20 }).First());
            Assert.AreEqual("E", _mapper.ToKeys(new byte[] { 0x08 }).First());
            Assert.AreEqual("D", _mapper.ToKeys(new byte[] { 0x07 }).First());
            Assert.AreEqual("X", _mapper.ToKeys(new byte[] { 0x1B }).First());
            Assert.AreEqual("F3", _mapper.ToKeys(new byte[] { 0x3C }).First());
            Assert.AreEqual("4", _mapper.ToKeys(new byte[] { 0x21 }).First());
            Assert.AreEqual("R", _mapper.ToKeys(new byte[] { 0x15 }).First());
            Assert.AreEqual("F", _mapper.ToKeys(new byte[] { 0x09 }).First());
            Assert.AreEqual("C", _mapper.ToKeys(new byte[] { 0x06 }).First());
            Assert.AreEqual("F4", _mapper.ToKeys(new byte[] { 0x3D }).First());
            Assert.AreEqual("5", _mapper.ToKeys(new byte[] { 0x22 }).First());
            Assert.AreEqual("T", _mapper.ToKeys(new byte[] { 0x17 }).First());
            Assert.AreEqual("G", _mapper.ToKeys(new byte[] { 0x0A }).First());
            Assert.AreEqual("V", _mapper.ToKeys(new byte[] { 0x19 }).First());
            Assert.AreEqual("6", _mapper.ToKeys(new byte[] { 0x23 }).First());
            Assert.AreEqual("Y", _mapper.ToKeys(new byte[] { 0x1C }).First());
            Assert.AreEqual("H", _mapper.ToKeys(new byte[] { 0x0B }).First());
            Assert.AreEqual("B", _mapper.ToKeys(new byte[] { 0x05 }).First());
            Assert.AreEqual("SPACE", _mapper.ToKeys(new byte[] { 0x2C }).First());
            Assert.AreEqual("F5", _mapper.ToKeys(new byte[] { 0x3E }).First());
            Assert.AreEqual("7", _mapper.ToKeys(new byte[] { 0x24 }).First());
            Assert.AreEqual("U", _mapper.ToKeys(new byte[] { 0x18 }).First());
            Assert.AreEqual("J", _mapper.ToKeys(new byte[] { 0x0D }).First());
            Assert.AreEqual("N", _mapper.ToKeys(new byte[] { 0x11 }).First());
            Assert.AreEqual("F6", _mapper.ToKeys(new byte[] { 0x3F }).First());
            Assert.AreEqual("8", _mapper.ToKeys(new byte[] { 0x25 }).First());
            Assert.AreEqual("I", _mapper.ToKeys(new byte[] { 0x0C }).First());
            Assert.AreEqual("K", _mapper.ToKeys(new byte[] { 0x0E }).First());
            Assert.AreEqual("M", _mapper.ToKeys(new byte[] { 0x10 }).First());
            Assert.AreEqual("F7", _mapper.ToKeys(new byte[] { 0x40 }).First());
            Assert.AreEqual("9", _mapper.ToKeys(new byte[] { 0x26 }).First());
            Assert.AreEqual("O", _mapper.ToKeys(new byte[] { 0x27 }).First());
            Assert.AreEqual("L", _mapper.ToKeys(new byte[] { 0x0F }).First());
            Assert.AreEqual(",", _mapper.ToKeys(new byte[] { 0x36 }).First());
            Assert.AreEqual("F8", _mapper.ToKeys(new byte[] { 0x41 }).First());
            Assert.AreEqual("0", _mapper.ToKeys(new byte[] { 0x27 }).First());
            Assert.AreEqual("P", _mapper.ToKeys(new byte[] { 0x13 }).First());
            Assert.AreEqual(";", _mapper.ToKeys(new byte[] { 0x33 }).First());
            Assert.AreEqual(".", _mapper.ToKeys(new byte[] { 0x37 }).First());
            Assert.AreEqual("RALT", _mapper.ToKeys(new byte[] { 0x40 }).First());
            Assert.AreEqual("F9", _mapper.ToKeys(new byte[] { 0x42 }).First());
            Assert.AreEqual("-", _mapper.ToKeys(new byte[] { 0x2D }).First());
            Assert.AreEqual("[", _mapper.ToKeys(new byte[] { 0x2F }).First());
            Assert.AreEqual("'", _mapper.ToKeys(new byte[] { 0x34 }).First());
            Assert.AreEqual("?", _mapper.ToKeys(new byte[] { 0x38 }).First());
            Assert.AreEqual("F10", _mapper.ToKeys(new byte[] { 0x43 }).First());
            Assert.AreEqual("=", _mapper.ToKeys(new byte[] { 0x2E }).First());
            Assert.AreEqual("]", _mapper.ToKeys(new byte[] { 0x30 }).First());
            Assert.AreEqual("RWINDOW", _mapper.ToKeys(new byte[] { 0x80 }).First());
            Assert.AreEqual("F11", _mapper.ToKeys(new byte[] { 0x44 }).First());
            Assert.AreEqual("RSHIFT", _mapper.ToKeys(new byte[] { 0x20 }).First());
            Assert.AreEqual("F12", _mapper.ToKeys(new byte[] { 0x45 }).First());
            Assert.AreEqual("BACKSPACE", _mapper.ToKeys(new byte[] { 0x2A }).First());
            Assert.AreEqual("|", _mapper.ToKeys(new byte[] { 0x31 }).First());
            Assert.AreEqual("ENTER", _mapper.ToKeys(new byte[] { 0x28 }).First());
            Assert.AreEqual("RCTRL", _mapper.ToKeys(new byte[] { 0x10 }).First());
            Assert.AreEqual("PRINT", _mapper.ToKeys(new byte[] { 0x46 }).First());
            Assert.AreEqual("INSERT", _mapper.ToKeys(new byte[] { 0x49 }).First());
            Assert.AreEqual("DELETE", _mapper.ToKeys(new byte[] { 0x4C }).First());
            Assert.AreEqual("LARROW", _mapper.ToKeys(new byte[] { 0x50 }).First());
            Assert.AreEqual("SCROLL", _mapper.ToKeys(new byte[] { 0x47 }).First());
            Assert.AreEqual("HOME", _mapper.ToKeys(new byte[] { 0x4A }).First());
            Assert.AreEqual("END", _mapper.ToKeys(new byte[] { 0x4D }).First());
            Assert.AreEqual("UARROW", _mapper.ToKeys(new byte[] { 0x52 }).First());
            Assert.AreEqual("DARROW", _mapper.ToKeys(new byte[] { 0x51 }).First());
            Assert.AreEqual("PAUSE", _mapper.ToKeys(new byte[] { 0x48 }).First());
            Assert.AreEqual("PAGEUP", _mapper.ToKeys(new byte[] { 0x4B }).First());
            Assert.AreEqual("PAGEDOWN", _mapper.ToKeys(new byte[] { 0x4E }).First());
            Assert.AreEqual("RARROW", _mapper.ToKeys(new byte[] { 0x4F }).First());
        }

        [TestMethod]
        public void ToKeysShouldReturnCorrespondingKeysForInputs()
        {
            string[] expected = { "TAB", "A", "F1", "SPACE" };
            byte[] inputs = { 0x2B, 0x04, 0x3A, 0x2C };

            var result = _mapper.ToKeys(inputs);

            CollectionAssert.AreEqual(expected, result.ToArray());
        }
    }
}
