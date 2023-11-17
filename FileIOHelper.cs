// Decompiled with JetBrains decompiler
// Type: SATOPrinterAPI.FileIOHelper
// Assembly: SATOPrinterAPI, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1CED7137-37FF-4597-9D4E-DAF488B01141
// Assembly location: D:\Downloads\Microsoft Edge\ApiPort.2.8.25.Offline\ApiPort.Offline\netcoreapp3.1\SATOPrinterAPI.dll

using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace SATOPrinterAPI
{
  internal class FileIOHelper
  {
    internal const uint ERROR_FILE_NOT_FOUND = 2;
    internal const uint ERROR_INVALID_NAME = 123;
    internal const uint ERROR_ACCESS_DENIED = 5;
    internal const uint ERROR_IO_PENDING = 997;
    internal const int INVALID_HANDLE_VALUE = -1;
    internal const uint CREATE_NEW = 1;
    internal const uint CREATE_ALWAYS = 2;
    internal const uint OPEN_EXISTING = 3;
    internal const uint OPEN_ALWAYS = 4;
    internal const uint TRUNCATE_EXISTING = 5;
    internal const uint EV_RXCHAR = 1;
    internal const uint EV_RXFLAG = 2;
    internal const uint EV_TXEMPTY = 4;
    internal const uint EV_CTS = 8;
    internal const uint EV_DSR = 16;
    internal const uint EV_RLSD = 32;
    internal const uint EV_BREAK = 64;
    internal const uint EV_ERR = 128;
    internal const uint EV_RING = 256;
    internal const uint EV_PERR = 512;
    internal const uint EV_RX80FULL = 1024;
    internal const uint EV_EVENT1 = 2048;
    internal const uint EV_EVENT2 = 4096;
    internal const uint SETXOFF = 1;
    internal const uint SETXON = 2;
    internal const uint SETRTS = 3;
    internal const uint CLRRTS = 4;
    internal const uint SETDTR = 5;
    internal const uint CLRDTR = 6;
    internal const uint RESETDEV = 7;
    internal const uint SETBREAK = 8;
    internal const uint CLRBREAK = 9;
    internal const uint MS_CTS_ON = 16;
    internal const uint MS_DSR_ON = 32;
    internal const uint MS_RING_ON = 64;
    internal const uint MS_RLSD_ON = 128;
    internal const uint CE_RXOVER = 1;
    internal const uint CE_OVERRUN = 2;
    internal const uint CE_RXPARITY = 4;
    internal const uint CE_FRAME = 8;
    internal const uint CE_BREAK = 16;
    internal const uint CE_TXFULL = 256;
    internal const uint CE_PTO = 512;
    internal const uint CE_IOE = 1024;
    internal const uint CE_DNS = 2048;
    internal const uint CE_OOP = 4096;
    internal const uint CE_MODE = 32768;

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static extern SafeFileHandle CreateFile(
      string fileName,
      [MarshalAs(UnmanagedType.U4)] FileIOHelper.EFileAccess dwDesiredAccess,
      [MarshalAs(UnmanagedType.U4)] FileShare dwShareMode,
      IntPtr lpSecurityAttributes,
      [MarshalAs(UnmanagedType.U4)] FileMode dwCreationDisposition,
      [MarshalAs(UnmanagedType.U4)] FileIOHelper.EFileAttributes dwFlagsAndAttributes,
      IntPtr hTemplateFile);

    [DllImport("kernel32.dll")]
    internal static extern bool CloseHandle(SafeFileHandle hObject);

    [DllImport("kernel32.dll")]
    internal static extern bool GetHandleInformation(SafeFileHandle hObject, out uint lpdwFlags);

    [DllImport("kernel32.dll")]
    internal static extern bool GetCommState(SafeFileHandle hFile, ref FileIOHelper.DCB lpDCB);

    [DllImport("kernel32.dll")]
    internal static extern bool GetCommTimeouts(
      SafeFileHandle hFile,
      out FileIOHelper.COMMTIMEOUTS lpCommTimeouts);

    [DllImport("kernel32.dll")]
    internal static extern bool BuildCommDCBAndTimeouts(
      string lpDef,
      ref FileIOHelper.DCB lpDCB,
      ref FileIOHelper.COMMTIMEOUTS lpCommTimeouts);

    [DllImport("kernel32.dll")]
    internal static extern bool SetCommState(SafeFileHandle hFile, [In] ref FileIOHelper.DCB lpDCB);

    [DllImport("kernel32.dll")]
    internal static extern bool SetCommTimeouts(
      SafeFileHandle hFile,
      [In] ref FileIOHelper.COMMTIMEOUTS lpCommTimeouts);

    [DllImport("kernel32.dll")]
    internal static extern bool SetupComm(SafeFileHandle hFile, uint dwInQueue, uint dwOutQueue);

    [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern bool WriteFile(
      SafeFileHandle fFile,
      byte[] lpBuffer,
      uint nNumberOfBytesToWrite,
      out uint lpNumberOfBytesWritten,
      IntPtr lpOverlapped);

    [DllImport("kernel32.dll")]
    internal static extern bool SetCommMask(SafeFileHandle hFile, uint dwEvtMask);

    [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern bool WaitCommEvent(
      SafeFileHandle hFile,
      IntPtr lpEvtMask,
      IntPtr lpOverlapped);

    [DllImport("kernel32.dll")]
    internal static extern bool CancelIo(SafeFileHandle hFile);

    [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern bool ReadFile(
      SafeFileHandle hFile,
      [Out] byte[] lpBuffer,
      uint nNumberOfBytesToRead,
      out uint nNumberOfBytesRead,
      IntPtr lpOverlapped);

    [DllImport("kernel32.dll")]
    internal static extern bool TransmitCommChar(SafeFileHandle hFile, byte cChar);

    [DllImport("kernel32.dll")]
    internal static extern bool EscapeCommFunction(SafeFileHandle hFile, uint dwFunc);

    [DllImport("kernel32.dll")]
    internal static extern bool GetCommModemStatus(SafeFileHandle hFile, out uint lpModemStat);

    [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern bool GetOverlappedResult(
      SafeFileHandle hFile,
      IntPtr lpOverlapped,
      out uint nNumberOfBytesTransferred,
      bool bWait);

    [DllImport("kernel32", SetLastError = true)]
    internal static extern int GetLastError();

    [DllImport("kernel32.dll")]
    internal static extern bool ClearCommError(
      SafeFileHandle hFile,
      out uint lpErrors,
      IntPtr lpStat);

    [DllImport("kernel32.dll")]
    internal static extern bool ClearCommError(
      SafeFileHandle hFile,
      out uint lpErrors,
      out FileIOHelper.COMSTAT cs);

    [DllImport("kernel32.dll")]
    internal static extern bool GetCommProperties(
      SafeFileHandle hFile,
      out FileIOHelper.COMMPROP cp);

    [Flags]
    public enum EFileAccess : uint
    {
      GENERIC_READ = 2147483648, // 0x80000000
      GENERIC_WRITE = 1073741824, // 0x40000000
      GENERIC_EXECUTE = 536870912, // 0x20000000
      GENERIC_READWRITE = 486539264, // 0x1D000000
    }

    [Flags]
    public enum EFileAttributes : uint
    {
      Write_Through = 2147483648, // 0x80000000
      Overlapped = 1073741824, // 0x40000000
      NoBuffering = 536870912, // 0x20000000
      RandomAccess = 268435456, // 0x10000000
      SequentialScan = 134217728, // 0x08000000
      DeleteOnClose = 67108864, // 0x04000000
      BackupSemantics = 33554432, // 0x02000000
      PosixSemantics = 16777216, // 0x01000000
      OpenReparsePoint = 2097152, // 0x00200000
      OpenNoRecall = 1048576, // 0x00100000
      FirstPipeInstance = 524288, // 0x00080000
      Readonly = 1,
      Hidden = 2,
      System = 4,
      Directory = 16, // 0x00000010
      Archive = 32, // 0x00000020
      Device = 64, // 0x00000040
      Normal = 128, // 0x00000080
      Temporary = 256, // 0x00000100
      SparseFile = 512, // 0x00000200
      ReparsePoint = 1024, // 0x00000400
      Compressed = 2048, // 0x00000800
      Offline = 4096, // 0x00001000
      NotContentIndexed = 8192, // 0x00002000
      Encrypted = 16384, // 0x00004000
    }

    internal struct COMMTIMEOUTS
    {
      internal int ReadIntervalTimeout;
      internal int ReadTotalTimeoutMultiplier;
      internal int ReadTotalTimeoutConstant;
      internal int WriteTotalTimeoutMultiplier;
      internal int WriteTotalTimeoutConstant;
    }

    internal struct DCB
    {
      internal int DCBlength;
      internal int BaudRate;
      internal int PackedValues;
      internal short wReserved;
      internal short XonLim;
      internal short XoffLim;
      internal byte ByteSize;
      internal byte Parity;
      internal byte StopBits;
      internal byte XonChar;
      internal byte XoffChar;
      internal byte ErrorChar;
      internal byte EofChar;
      internal byte EvtChar;
      internal short wReserved1;

      internal void init(
        bool parity,
        bool outCTS,
        bool outDSR,
        int dtr,
        bool inDSR,
        bool txc,
        bool xOut,
        bool xIn,
        int rts)
      {
        this.DCBlength = 28;
        this.PackedValues = 32769;
        if (parity)
          this.PackedValues |= 2;
        if (outCTS)
          this.PackedValues |= 4;
        if (outDSR)
          this.PackedValues |= 8;
        this.PackedValues |= (dtr & 3) << 4;
        if (inDSR)
          this.PackedValues |= 64;
        if (txc)
          this.PackedValues |= 128;
        if (xOut)
          this.PackedValues |= 256;
        if (xIn)
          this.PackedValues |= 512;
        this.PackedValues |= (rts & 3) << 12;
      }
    }

    internal struct OVERLAPPED
    {
      internal UIntPtr Internal;
      internal UIntPtr InternalHigh;
      internal uint Offset;
      internal uint OffsetHigh;
      internal IntPtr hEvent;
    }

    internal struct COMSTAT
    {
      internal const uint fCtsHold = 1;
      internal const uint fDsrHold = 2;
      internal const uint fRlsdHold = 4;
      internal const uint fXoffHold = 8;
      internal const uint fXoffSent = 16;
      internal const uint fEof = 32;
      internal const uint fTxim = 64;
      internal uint Flags;
      internal uint cbInQue;
      internal uint cbOutQue;
    }

    internal struct COMMPROP
    {
      internal ushort wPacketLength;
      internal ushort wPacketVersion;
      internal uint dwServiceMask;
      internal uint dwReserved1;
      internal uint dwMaxTxQueue;
      internal uint dwMaxRxQueue;
      internal uint dwMaxBaud;
      internal uint dwProvSubType;
      internal uint dwProvCapabilities;
      internal uint dwSettableParams;
      internal uint dwSettableBaud;
      internal ushort wSettableData;
      internal ushort wSettableStopParity;
      internal uint dwCurrentTxQueue;
      internal uint dwCurrentRxQueue;
      internal uint dwProvSpec1;
      internal uint dwProvSpec2;
      internal byte wcProvChar;
    }
  }
}
