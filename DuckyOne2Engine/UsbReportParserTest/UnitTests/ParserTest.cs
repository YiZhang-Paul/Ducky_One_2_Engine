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
            START	0001	18:57:15.433											
            URB	0002	18:57:18.051	2.617536 s		Bulk or Interrupt Transfer	Output Report (Len 64)	out	01:01:04	FFFFE586C1AC6630h	000000d4	usbccgp	FFFFE586BEAD89A0h	
            00000000  51 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  Q...............
            00000010  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
            00000020  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
            00000030  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
            URB	0003	18:57:18.051	2.617542 s		Bulk or Interrupt Transfer	Output Report (Len 64)	out	01:01:04	FFFFE586BFCFB9D0h		USBPcap	FFFFE586BEAD89A0h	
            00000000  51 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  Q...............
            00000010  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
            00000020  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
            00000030  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
            URB	0004	18:57:18.051	2.617546 s		Bulk or Interrupt Transfer	Output Report (Len 64)	out	01:01:04	FFFFE586C7AD62A0h		ACPI	FFFFE586BEAD89A0h	
            00000000  51 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  Q...............
            00000010  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
            00000020  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
            00000030  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
            URB	0005	18:57:18.051	2.617551 s		Bulk or Interrupt Transfer	Output Report (Len 64)	out	01:01:04	FFFFE586BE0A34E0h	USBPDO-6	USBHUB3	FFFFE586BEAD89A0h	
            00000000  51 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  Q...............
            00000010  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
            00000020  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
            00000030  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
            URB	0006-0005	18:57:18.052	2.618681 s	1.130 ms	Bulk or Interrupt Transfer	64 bytes buffer	out	01:01:04	FFFFE586BE0A34E0h	USBPDO-6	USBHUB3	FFFFE586BEAD89A0h	Success (Success)
            URB	0007-0004	18:57:18.052	2.618683 s	1.137 ms	Bulk or Interrupt Transfer	64 bytes buffer	out	01:01:04	FFFFE586C7AD62A0h		ACPI	FFFFE586BEAD89A0h	Success (Success)
            URB	0008-0003	18:57:18.052	2.618685 s	1.143 ms	Bulk or Interrupt Transfer	64 bytes buffer	out	01:01:04	FFFFE586BFCFB9D0h		USBPcap	FFFFE586BEAD89A0h	Success (Success)
            URB	0009-0002	18:57:18.052	2.618685 s	1.150 ms	Bulk or Interrupt Transfer	64 bytes buffer	out	01:01:04	FFFFE586C1AC6630h	000000d4	usbccgp	FFFFE586BEAD89A0h	Success (Success)
            URB	0010-0000	18:57:18.061	2.627734 s		Bulk or Interrupt Transfer	Input Report (Len 64)	in	01:01:83	FFFFE586BE0A34E0h	USBPDO-6	USBHUB3	FFFFE586BE5F7010h	Success (Success)
            00000000  51 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  Q...............
            00000010  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
            00000020  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
            00000030  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
            URB	0011-0000	18:57:18.061	2.627737 s		Bulk or Interrupt Transfer	Input Report (Len 64)	in	01:01:83	FFFFE586C7AD62A0h		ACPI	FFFFE586BE5F7010h	Success (Success)
            00000000  51 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  Q...............
            00000010  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
            00000020  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
            00000030  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
            URB	0012-0000	18:57:18.061	2.627739 s		Bulk or Interrupt Transfer	Input Report (Len 64)	in	01:01:83	FFFFE586BFCFB9D0h		USBPcap	FFFFE586BE5F7010h	Success (Success)
            00000000  51 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  Q...............
            00000010  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
            00000020  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
            00000030  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
            URB	0013-0000	18:57:18.061	2.627740 s		Bulk or Interrupt Transfer	Input Report (Len 64)	in	01:01:83	FFFFE586C1AC6630h	000000d4	usbccgp	FFFFE586BE5F7010h	Success (Success)
            00000000  51 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  Q...............
            00000010  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
            00000020  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
            00000030  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
            URB	0014	18:57:18.061	2.627770 s		Bulk or Interrupt Transfer	64 bytes buffer	in	01:01:83	FFFFE586C1AC6630h	000000d4	usbccgp	FFFFE586BE5F7010h	
            URB	0015	18:57:18.061	2.627783 s		Bulk or Interrupt Transfer	64 bytes buffer	in	01:01:83	FFFFE586BFCFB9D0h		USBPcap	FFFFE586BE5F7010h	
            URB	0016	18:57:18.061	2.627784 s		Bulk or Interrupt Transfer	64 bytes buffer	in	01:01:83	FFFFE586C7AD62A0h		ACPI	FFFFE586BE5F7010h	
            URB	0017	18:57:18.061	2.627785 s		Bulk or Interrupt Transfer	64 bytes buffer	in	01:01:83	FFFFE586BE0A34E0h	USBPDO-6	USBHUB3	FFFFE586BE5F7010h	
            URB	0018-0000	18:57:18.062	2.628689 s		Bulk or Interrupt Transfer	Input Report (Len 64)	in	01:01:83	FFFFE586BE0A34E0h	USBPDO-6	USBHUB3	FFFFE586CBA32010h	Success (Success)
            00000000  42 20 00 00 01 00 00 00 00 00 00 00 00 00 00 00  B ..............
            00000010  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
            00000020  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
            00000030  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
            URB	0019-0000	18:57:18.062	2.628693 s		Bulk or Interrupt Transfer	Input Report (Len 64)	in	01:01:83	FFFFE586C7AD62A0h		ACPI	FFFFE586CBA32010h	Success (Success)
            00000000  42 20 00 00 01 00 00 00 00 00 00 00 00 00 00 00  B ..............
            00000010  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
            00000020  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
            00000030  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
            URB	0020-0000	18:57:18.062	2.628695 s		Bulk or Interrupt Transfer	Input Report (Len 64)	in	01:01:83	FFFFE586BFCFB9D0h		USBPcap	FFFFE586CBA32010h	Success (Success)
            00000000  42 20 00 00 01 00 00 00 00 00 00 00 00 00 00 00  B ..............
            00000010  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
            00000020  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
            00000030  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
            URB	0021-0000	18:57:18.062	2.628696 s		Bulk or Interrupt Transfer	Input Report (Len 64)	in	01:01:83	FFFFE586C1AC6630h	000000d4	usbccgp	FFFFE586CBA32010h	Success (Success)
            00000000  42 20 00 00 01 00 00 00 00 00 00 00 00 00 00 00  B ..............
            00000010  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
            00000020  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
            00000030  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
            URB	0022	18:57:18.062	2.628715 s		Bulk or Interrupt Transfer	64 bytes buffer	in	01:01:83	FFFFE586C1AC6630h	000000d4	usbccgp	FFFFE586CBA32010h	
            URB	0023	18:57:18.062	2.628718 s		Bulk or Interrupt Transfer	64 bytes buffer	in	01:01:83	FFFFE586BFCFB9D0h		USBPcap	FFFFE586CBA32010h	
            URB	0024	18:57:18.062	2.628720 s		Bulk or Interrupt Transfer	64 bytes buffer	in	01:01:83	FFFFE586C7AD62A0h		ACPI	FFFFE586CBA32010h	
            URB	0025	18:57:18.062	2.628721 s		Bulk or Interrupt Transfer	64 bytes buffer	in	01:01:83	FFFFE586BE0A34E0h	USBPDO-6	USBHUB3	FFFFE586CBA32010h	
            URB	0026	18:57:18.066	2.632496 s		Bulk or Interrupt Transfer	Output Report (Len 64)	out	01:01:04	FFFFE586C1AC6630h	000000d4	usbccgp	FFFFE586C2BF49A0h	
            00000000  52 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  R...............
            00000010  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
            00000020  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
            00000030  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
            URB	0027	18:57:18.066	2.632505 s		Bulk or Interrupt Transfer	Output Report (Len 64)	out	01:01:04	FFFFE586BFCFB9D0h		USBPcap	FFFFE586C2BF49A0h	
            00000000  52 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  R...............
            00000010  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
            00000020  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
            00000030  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
            URB	0028	18:57:18.066	2.632512 s		Bulk or Interrupt Transfer	Output Report (Len 64)	out	01:01:04	FFFFE586C7AD62A0h		ACPI	FFFFE586C2BF49A0h	
            00000000  52 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  R...............
            00000010  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
            00000020  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
            00000030  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
            URB	0029	18:57:18.066	2.632517 s		Bulk or Interrupt Transfer	Output Report (Len 64)	out	01:01:04	FFFFE586BE0A34E0h	USBPDO-6	USBHUB3	FFFFE586C2BF49A0h	
            00000000  52 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  R...............
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
                "51 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00",
                "42 20 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00"
            };

            var input = _parser.GetInput(RawData);

            CollectionAssert.AreEqual(expected, input.ToArray());
        }

        [TestMethod]
        public void GetOutputShouldReturnParsedUsbOutput()
        {
            var expected = new[]
            {
                "51 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00",
                "52 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00"
            };

            var output = _parser.GetOutput(RawData);

            CollectionAssert.AreEqual(expected, output.ToArray());
        }
    }
}
