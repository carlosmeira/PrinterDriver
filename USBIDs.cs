// Decompiled with JetBrains decompiler
// Type: SATOPrinterAPI.USBIDs
// Assembly: SATOPrinterAPI, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1CED7137-37FF-4597-9D4E-DAF488B01141
// Assembly location: D:\Downloads\Microsoft Edge\ApiPort.2.8.25.Offline\ApiPort.Offline\netcoreapp3.1\SATOPrinterAPI.dll

using System;
using System.Runtime.InteropServices;

namespace SATOPrinterAPI
{
  [StructLayout(LayoutKind.Sequential, Size = 1)]
  internal struct USBIDs
  {
    public static int SatoVendorID = 2088;
    public static int OKIVendorID = 1724;
    public static int LexmarkVendorID = 1085;
    public static int ATenVendorID = 1367;
    public static int[] Valid_VIDs = new int[4]
    {
      USBIDs.LexmarkVendorID,
      USBIDs.ATenVendorID,
      USBIDs.OKIVendorID,
      USBIDs.SatoVendorID
    };
    public static int[] ATenPIDs = new int[1]{ 8198 };

    public static string PIDtoString(int pid) => Enum.Parse(typeof (USBIDs.PIDs), pid.ToString()).ToString();

    public enum PIDs
    {
      CL408e = 83, // 0x00000053
      CL412e = 84, // 0x00000054
      CL608e = 86, // 0x00000056
      CL612e = 87, // 0x00000057
      GT408e = 113, // 0x00000071
      GT412e = 114, // 0x00000072
      GT424e = 115, // 0x00000073
      LM408e = 128, // 0x00000080
      LM412e = 129, // 0x00000081
      MB400i = 130, // 0x00000082
      MB410i = 131, // 0x00000083
      CT408i = 132, // 0x00000084
      CT412i = 133, // 0x00000085
      CT424i = 134, // 0x00000086
      GL408e = 135, // 0x00000087
      GL412e = 136, // 0x00000088
      GL424e = 137, // 0x00000089
      S8408DT = 149, // 0x00000095
      S8412DT = 150, // 0x00000096
      S8424DT = 151, // 0x00000097
      S8400DT = 152, // 0x00000098
      CL4NX_203dpi = 290, // 0x00000122
      CL4NX_305dpi = 291, // 0x00000123
      CL4NX_609dpi = 292, // 0x00000124
      ATEN_LPT = 8198, // 0x00002006
    }
  }
}
