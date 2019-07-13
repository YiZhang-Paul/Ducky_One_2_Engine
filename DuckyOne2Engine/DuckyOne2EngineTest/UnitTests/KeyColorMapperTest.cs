using DuckyOne2Engine.KeyMappers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace DuckyOne2EngineTest.UnitTests
{
    [TestClass]
    public class KeyColorMapperTest
    {
        private KeyColorMapper _mapper;

        private string GetBytePositionsString(string key)
        {
            var positions = _mapper.GetBytePositions(key);

            return string.Join("|", positions.Select(_ => _.ToString()));
        }

        [TestInitialize]
        public void Setup()
        {
            _mapper = new KeyColorMapper();
        }

        [TestMethod]
        public void GetBytePositionsShouldReturnBytePositionsForExistingKeys()
        {
            Assert.AreEqual("0-25|0-26|0-27", GetBytePositionsString(Keys.Esc));
            Assert.AreEqual("0-28|0-29|0-30", GetBytePositionsString(Keys.Backtick));
            Assert.AreEqual("0-31|0-32|0-33", GetBytePositionsString(Keys.Tab));
            Assert.AreEqual("0-34|0-35|0-36", GetBytePositionsString(Keys.Caps));
            Assert.AreEqual("0-37|0-38|0-39", GetBytePositionsString(Keys.Lshift));
            Assert.AreEqual("0-40|0-41|0-42", GetBytePositionsString(Keys.Lctrl));
            Assert.AreEqual("0-46|0-47|0-48", GetBytePositionsString(Keys.One));
            Assert.AreEqual("0-49|0-50|0-51", GetBytePositionsString(Keys.Q));
            Assert.AreEqual("0-52|0-53|0-54", GetBytePositionsString(Keys.A));
            Assert.AreEqual("0-58|0-59|0-60", GetBytePositionsString(Keys.Lwindow));
            Assert.AreEqual("0-61|0-62|0-63", GetBytePositionsString(Keys.F1));
            Assert.AreEqual("0-64|1-5|1-6", GetBytePositionsString(Keys.Two));
            Assert.AreEqual("1-7|1-8|1-9", GetBytePositionsString(Keys.W));
            Assert.AreEqual("1-10|1-11|1-12", GetBytePositionsString(Keys.S));
            Assert.AreEqual("1-13|1-14|1-15", GetBytePositionsString(Keys.Z));
            Assert.AreEqual("1-16|1-17|1-18", GetBytePositionsString(Keys.Lalt));
            Assert.AreEqual("1-19|1-20|1-21", GetBytePositionsString(Keys.F2));
            Assert.AreEqual("1-22|1-23|1-24", GetBytePositionsString(Keys.Three));
            Assert.AreEqual("1-25|1-26|1-27", GetBytePositionsString(Keys.E));
            Assert.AreEqual("1-28|1-29|1-30", GetBytePositionsString(Keys.D));
            Assert.AreEqual("1-31|1-32|1-33", GetBytePositionsString(Keys.X));
            Assert.AreEqual("1-37|1-38|1-39", GetBytePositionsString(Keys.F3));
            Assert.AreEqual("1-40|1-41|1-42", GetBytePositionsString(Keys.Four));
            Assert.AreEqual("1-43|1-44|1-45", GetBytePositionsString(Keys.R));
            Assert.AreEqual("1-46|1-47|1-48", GetBytePositionsString(Keys.F));
            Assert.AreEqual("1-49|1-50|1-51", GetBytePositionsString(Keys.C));
            Assert.AreEqual("1-55|1-56|1-57", GetBytePositionsString(Keys.F4));
            Assert.AreEqual("1-58|1-59|1-60", GetBytePositionsString(Keys.Five));
            Assert.AreEqual("1-61|1-62|1-63", GetBytePositionsString(Keys.T));
            Assert.AreEqual("1-64|2-5|2-6", GetBytePositionsString(Keys.G));
            Assert.AreEqual("2-7|2-8|2-9", GetBytePositionsString(Keys.V));
            Assert.AreEqual("2-16|2-17|2-18", GetBytePositionsString(Keys.Six));
            Assert.AreEqual("2-19|2-20|2-21", GetBytePositionsString(Keys.Y));
            Assert.AreEqual("2-22|2-23|2-24", GetBytePositionsString(Keys.H));
            Assert.AreEqual("2-25|2-26|2-27", GetBytePositionsString(Keys.B));
            Assert.AreEqual("2-28|2-29|2-30", GetBytePositionsString(Keys.Space));
            Assert.AreEqual("2-31|2-32|2-33", GetBytePositionsString(Keys.F5));
            Assert.AreEqual("2-34|2-35|2-36", GetBytePositionsString(Keys.Seven));
            Assert.AreEqual("2-37|2-38|2-39", GetBytePositionsString(Keys.U));
            Assert.AreEqual("2-40|2-41|2-42", GetBytePositionsString(Keys.J));
            Assert.AreEqual("2-43|2-44|2-45", GetBytePositionsString(Keys.N));
            Assert.AreEqual("2-49|2-50|2-51", GetBytePositionsString(Keys.F6));
            Assert.AreEqual("2-52|2-53|2-54", GetBytePositionsString(Keys.Eight));
            Assert.AreEqual("2-55|2-56|2-57", GetBytePositionsString(Keys.I));
            Assert.AreEqual("2-58|2-59|2-60", GetBytePositionsString(Keys.K));
            Assert.AreEqual("2-61|2-62|2-63", GetBytePositionsString(Keys.M));
            Assert.AreEqual("3-7|3-8|3-9", GetBytePositionsString(Keys.F7));
            Assert.AreEqual("3-10|3-11|3-12", GetBytePositionsString(Keys.Nine));
            Assert.AreEqual("3-13|3-14|3-15", GetBytePositionsString(Keys.O));
            Assert.AreEqual("3-16|3-17|3-18", GetBytePositionsString(Keys.L));
            Assert.AreEqual("3-19|3-20|3-21", GetBytePositionsString(Keys.Comma));
            Assert.AreEqual("3-25|3-26|3-27", GetBytePositionsString(Keys.F8));
            Assert.AreEqual("3-28|3-29|3-30", GetBytePositionsString(Keys.Zero));
            Assert.AreEqual("3-31|3-32|3-33", GetBytePositionsString(Keys.P));
            Assert.AreEqual("3-34|3-35|3-36", GetBytePositionsString(Keys.Semicolon));
            Assert.AreEqual("3-37|3-38|3-39", GetBytePositionsString(Keys.Period));
            Assert.AreEqual("3-40|3-41|3-42", GetBytePositionsString(Keys.Ralt));
            Assert.AreEqual("3-43|3-44|3-45", GetBytePositionsString(Keys.F9));
            Assert.AreEqual("3-46|3-47|3-48", GetBytePositionsString(Keys.Hyphen));
            Assert.AreEqual("3-49|3-50|3-51", GetBytePositionsString(Keys.Lbracket));
            Assert.AreEqual("3-52|3-53|3-54", GetBytePositionsString(Keys.Quote));
            Assert.AreEqual("3-55|3-56|3-57", GetBytePositionsString(Keys.Question));
            Assert.AreEqual("3-61|3-62|3-63", GetBytePositionsString(Keys.F10));
            Assert.AreEqual("3-64|4-5|4-6", GetBytePositionsString(Keys.Equal));
            Assert.AreEqual("4-7|4-8|4-9", GetBytePositionsString(Keys.Rbracket));
            Assert.AreEqual("4-16|4-17|4-18", GetBytePositionsString(Keys.Rwindow));
            Assert.AreEqual("4-19|4-20|4-21", GetBytePositionsString(Keys.F11));
            Assert.AreEqual("4-31|4-32|4-33", GetBytePositionsString(Keys.Rshift));
            Assert.AreEqual("4-34|4-35|4-36", GetBytePositionsString(Keys.Fn));
            Assert.AreEqual("4-37|4-38|4-39", GetBytePositionsString(Keys.F12));
            Assert.AreEqual("4-40|4-41|4-42", GetBytePositionsString(Keys.Backspace));
            Assert.AreEqual("4-43|4-44|4-45", GetBytePositionsString(Keys.Pipe));
            Assert.AreEqual("4-46|4-47|4-48", GetBytePositionsString(Keys.Enter));
            Assert.AreEqual("4-52|4-53|4-54", GetBytePositionsString(Keys.Rctrl));
            Assert.AreEqual("4-55|4-56|4-57", GetBytePositionsString(Keys.Print));
            Assert.AreEqual("4-58|4-59|4-60", GetBytePositionsString(Keys.Insert));
            Assert.AreEqual("4-61|4-62|4-63", GetBytePositionsString(Keys.Delete));
            Assert.AreEqual("5-10|5-11|5-12", GetBytePositionsString(Keys.Larrow));
            Assert.AreEqual("5-13|5-14|5-15", GetBytePositionsString(Keys.Scroll));
            Assert.AreEqual("5-16|5-17|5-18", GetBytePositionsString(Keys.Home));
            Assert.AreEqual("5-19|5-20|5-21", GetBytePositionsString(Keys.End));
            Assert.AreEqual("5-25|5-26|5-27", GetBytePositionsString(Keys.Uarrow));
            Assert.AreEqual("5-28|5-29|5-30", GetBytePositionsString(Keys.Darrow));
            Assert.AreEqual("5-31|5-32|5-33", GetBytePositionsString(Keys.Pause));
            Assert.AreEqual("5-34|5-35|5-36", GetBytePositionsString(Keys.Pageup));
            Assert.AreEqual("5-37|5-38|5-39", GetBytePositionsString(Keys.Pagedown));
            Assert.AreEqual("5-46|5-47|5-48", GetBytePositionsString(Keys.Rarrow));
        }
    }
}
