// Decompiled with JetBrains decompiler
// Type: SATOPrinterAPI.RawPrinterHelper
// Assembly: SATOPrinterAPI, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1CED7137-37FF-4597-9D4E-DAF488B01141
// Assembly location: D:\Downloads\Microsoft Edge\ApiPort.2.8.25.Offline\ApiPort.Offline\netcoreapp3.1\SATOPrinterAPI.dll

using System;
using System.IO;
using System.Runtime.InteropServices;

namespace SATOPrinterAPI
{
  internal class RawPrinterHelper
  {
    [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    public static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPStr)] string szPrinter, out IntPtr hPrinter, IntPtr pd);

    [DllImport("winspool.Drv", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    public static extern bool ClosePrinter(IntPtr hPrinter);

    [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    public static extern bool StartDocPrinter(
      IntPtr hPrinter,
      int level,
      [MarshalAs(UnmanagedType.LPStruct), In] RawPrinterHelper.DOCINFOA di);

    [DllImport("winspool.Drv", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    public static extern bool EndDocPrinter(IntPtr hPrinter);

    [DllImport("winspool.Drv", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    public static extern bool StartPagePrinter(IntPtr hPrinter);

    [DllImport("winspool.Drv", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    public static extern bool EndPagePrinter(IntPtr hPrinter);

    [DllImport("winspool.Drv", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    public static extern bool WritePrinter(
      IntPtr hPrinter,
      IntPtr pBytes,
      int dwCount,
      out int dwWritten);

    public static bool SendBytesToPrinter(string szPrinterName, IntPtr pBytes, int dwCount)
    {
      int dwWritten = 0;
      IntPtr hPrinter = new IntPtr(0);
      RawPrinterHelper.DOCINFOA di = new RawPrinterHelper.DOCINFOA();
      bool printer = false;
      di.pDocName = "RAW Data";
      di.pDataType = "RAW";
      if (RawPrinterHelper.OpenPrinter(szPrinterName.Normalize(), out hPrinter, IntPtr.Zero))
      {
        if (RawPrinterHelper.StartDocPrinter(hPrinter, 1, di))
        {
          if (RawPrinterHelper.StartPagePrinter(hPrinter))
          {
            printer = RawPrinterHelper.WritePrinter(hPrinter, pBytes, dwCount, out dwWritten);
            RawPrinterHelper.EndPagePrinter(hPrinter);
          }
          RawPrinterHelper.EndDocPrinter(hPrinter);
        }
        RawPrinterHelper.ClosePrinter(hPrinter);
      }
      if (!printer)
        Marshal.GetLastWin32Error();
      return printer;
    }

    public static bool SendFileToPrinter(string szPrinterName, string szFileName)
    {
      FileStream input = new FileStream(szFileName, FileMode.Open);
      BinaryReader binaryReader = new BinaryReader((Stream) input);
      byte[] numArray = new byte[input.Length];
      IntPtr num1 = new IntPtr(0);
      int int32 = Convert.ToInt32(input.Length);
      byte[] source = binaryReader.ReadBytes(int32);
      IntPtr num2 = Marshal.AllocCoTaskMem(int32);
      IntPtr destination = num2;
      int length = int32;
      Marshal.Copy(source, 0, destination, length);
      int num3 = RawPrinterHelper.SendBytesToPrinter(szPrinterName, num2, int32) ? 1 : 0;
      Marshal.FreeCoTaskMem(num2);
      return num3 != 0;
    }

    public static bool SendStringToPrinter(string szPrinterName, string szString)
    {
      int length = szString.Length;
      IntPtr coTaskMemAnsi = Marshal.StringToCoTaskMemAnsi(szString);
      RawPrinterHelper.SendBytesToPrinter(szPrinterName, coTaskMemAnsi, length);
      Marshal.FreeCoTaskMem(coTaskMemAnsi);
      return true;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class DOCINFOA
    {
      [MarshalAs(UnmanagedType.LPStr)]
      public string pDocName;
      [MarshalAs(UnmanagedType.LPStr)]
      public string pOutputFile;
      [MarshalAs(UnmanagedType.LPStr)]
      public string pDataType;
    }
  }
}
