﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using UsbReportParser;

namespace UsbReportParserTest.UnitTests
{
    [TestClass]
    public class ParserTest
    {
        private const string RawData =
        @"
            USBlyzer Report

            Capture List

            Type	Seq	Time	Elapsed	Duration	Request	Request Details	I/O	C:I:E	Device Object	Device Name	Driver Name	IRP	IRP Status (URB Status)
            START	0001	21:51:03.639											
            URB	0002	21:51:06.887	3.248430 s		Bulk or Interrupt Transfer	Output Report (Len 64)	out	01:01:04	FFFF9C89E71A3870h	000000d1	usbccgp	FFFF9C89FC3189A0h	
            00000000  41 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00  A...............
            00000010  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
            00000020  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
            00000030  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
            URB	0003-0002	21:51:06.888	3.249542 s	1.112 ms	Bulk or Interrupt Transfer	64 bytes buffer	out	01:01:04	FFFF9C89E71A3870h	000000d1	usbccgp	FFFF9C89FC3189A0h	Success (Success)
            URB	0004	21:51:06.889	3.250523 s		Bulk or Interrupt Transfer	64 bytes buffer	in	01:01:83	FFFF9C89E71A3870h	000000d1	usbccgp	FFFF9C89EF0DC010h	
            URB	0005	21:51:06.892	3.253349 s		Bulk or Interrupt Transfer	Output Report (Len 64)	out	01:01:04	FFFF9C89E71A3870h	000000d1	usbccgp	FFFF9C89FCDC69A0h	
            00000000  56 81 00 00 02 00 00 00 02 00 00 00 AA AA AA AA  V...............
            00000010  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
            00000020  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
            00000030  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
            URB	0006-0005	21:51:06.893	3.254476 s	1.128 ms	Bulk or Interrupt Transfer	64 bytes buffer	out	01:01:04	FFFF9C89E71A3870h	000000d1	usbccgp	FFFF9C89FCDC69A0h	Success (Success)
            URB	0007	21:51:06.894	3.255487 s		Bulk or Interrupt Transfer	64 bytes buffer	in	01:01:83	FFFF9C89E71A3870h	000000d1	usbccgp	FFFF9C89F2CEE010h	
            URB	0008	21:51:06.895	3.256617 s		Bulk or Interrupt Transfer	Output Report (Len 64)	out	01:01:04	FFFF9C89E71A3870h	000000d1	usbccgp	FFFF9C89FB4169A0h	
            00000000  56 83 00 00 0C 00 0C 00 03 00 00 00 07 00 01 00  V...............
            00000010  00 01 00 C1 00 00 00 00 01 1C 49 FF 00 00 00 00  ..........I.....
            00000020  00 80 00 80 08 10 00 00 FF FF FF FF 00 00 04 00  ................
            00000030  00 80 00 00 FF FF FF FF FF FF FF FF FF FF FF FF  ................
            URB	0009-0008	21:51:06.896	3.257478 s	861 us	Bulk or Interrupt Transfer	64 bytes buffer	out	01:01:04	FFFF9C89E71A3870h	000000d1	usbccgp	FFFF9C89FB4169A0h	Success (Success)
            URB	0010-0004	21:51:06.897	3.258491 s	7.968 ms	Bulk or Interrupt Transfer	Input Report (Len 64)	in	01:01:83	FFFF9C89E71A3870h	000000d1	usbccgp	FFFF9C89EF0DC010h	Success (Success)
            00000000  56 83 00 00 00 00 00 00 00 00 00 00 00 00 00 00  V...............
            00000010  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
            00000020  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
            00000030  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
            URB	0011	21:51:06.897	3.258506 s		Bulk or Interrupt Transfer	64 bytes buffer	in	01:01:83	FFFF9C89E71A3870h	000000d1	usbccgp	FFFF9C89EF0DC010h	
            URB	0012	21:51:06.898	3.259661 s		Bulk or Interrupt Transfer	Output Report (Len 64)	out	01:01:04	FFFF9C89E71A3870h	000000d1	usbccgp	FFFF9C89FA5E19A0h	
            00000000  56 83 01 00 FF FF FF FF FF FF FF FF FF FF FF FF  V...............
            00000010  FF FF FF FF FF FF FF FF 09 00 09 00 00 00 00 00  ................
            00000020  04 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
            00000030  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
            URB	0013-0012	21:51:06.899	3.260478 s	817 us	Bulk or Interrupt Transfer	64 bytes buffer	out	01:01:04	FFFF9C89E71A3870h	000000d1	usbccgp	FFFF9C89FA5E19A0h	Success (Success)
            URB	0014-0007	21:51:06.900	3.261492 s	6.005 ms	Bulk or Interrupt Transfer	Input Report (Len 64)	in	01:01:83	FFFF9C89E71A3870h	000000d1	usbccgp	FFFF9C89F2CEE010h	Success (Success)
            00000000  56 83 01 00 00 00 00 00 00 00 00 00 00 00 00 00  V...............
            00000010  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
            00000020  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
            00000030  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................

            This report was generated by USBlyzer http://www.usblyzer.com/
        ";

        private Parser _parser;

        [TestInitialize]
        public void Setup()
        {
            _parser = new Parser();
        }

        [TestMethod]
        public void GetInputShouldReturnParsedUsbInput()
        {
            var expected = new[]
            {
                "56 83 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00",
                "56 83 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00"
            };

            var input = _parser.GetInput(RawData);

            CollectionAssert.AreEqual(expected, input.ToArray());
        }

        [TestMethod]
        public void GetOutputShouldReturnParsedUsbOutput()
        {
            var expected = new[]
            {
                "41 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00",
                "56 81 00 00 02 00 00 00 02 00 00 00 AA AA AA AA 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00",
                "56 83 00 00 0C 00 0C 00 03 00 00 00 07 00 01 00 00 01 00 C1 00 00 00 00 01 1C 49 FF 00 00 00 00 00 80 00 80 08 10 00 00 FF FF FF FF 00 00 04 00 00 80 00 00 FF FF FF FF FF FF FF FF FF FF FF FF",
                "56 83 01 00 FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF 09 00 09 00 00 00 00 00 04 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00"
            };

            var output = _parser.GetOutput(RawData);

            CollectionAssert.AreEqual(expected, output.ToArray());
        }
    }
}
