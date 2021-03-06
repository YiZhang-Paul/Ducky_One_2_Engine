﻿using DuckyOne2Engine.HidDevices;
using Moq;
using System.Linq;
using System.Text;

namespace DuckyOne2EngineTest.Mocks
{
    public static class MockDevice
    {
        public static StringBuilder SetupWrite(Mock<IHidDevice> device)
        {
            var builder = new StringBuilder();

            device.Setup(x => x.Write(It.IsAny<byte[]>(), It.IsAny<int>()))
                .Callback((byte[] input) =>
                {
                    var hex = input.Select(_ => _.ToString("X2"));
                    builder.Append($" {string.Join(" ", hex)}");
                });

            return builder;
        }
    }
}
