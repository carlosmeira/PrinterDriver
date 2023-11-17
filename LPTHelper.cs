// Decompiled with JetBrains decompiler
// Type: SATOPrinterAPI.LPTHelper
// Assembly: SATOPrinterAPI, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1CED7137-37FF-4597-9D4E-DAF488B01141
// Assembly location: D:\Downloads\Microsoft Edge\ApiPort.2.8.25.Offline\ApiPort.Offline\netcoreapp3.1\SATOPrinterAPI.dll

using Microsoft.Win32.SafeHandles;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace SATOPrinterAPI
{
  internal class LPTHelper : InterfaceHelper
  {
    private SafeFileHandle handle;
    private FileStream lptStream;
    private int LPTtimeout = 2500;
    private bool adminRights;

    public LPTHelper(bool PermanentConnect)
      : base(PermanentConnect)
    {
    }

    public override void CBReceive(IAsyncResult ar)
    {
      if (!(ar.AsyncState is FileStream asyncState))
        return;
      try
      {
        int length = asyncState.EndRead(ar);
        if (length > 0)
        {
          byte[] numArray = new byte[length];
          Array.Copy((Array) this.buffer, (Array) numArray, length);
          GC.Collect();
          if (this.fOnReceiveCallBack != null)
            this.fOnReceiveCallBack(numArray);
        }
        this.buffer.Initialize();
        this.Gar = asyncState.BeginRead(this.buffer, 0, this.buffer.Length, new AsyncCallback(((InterfaceHelper) this).CBReceive), (object) asyncState);
      }
      catch (Exception ex)
      {
        asyncState?.Close();
        if (this.handle == null)
          return;
        if (!this.handle.IsInvalid)
          this.handle.Close();
        this.handle = (SafeFileHandle) null;
      }
    }

    public override void Open(string LPTName, object data, int timeout)
    {
      try
      {
        this.lptStream = (FileStream) null;
        this.LPTtimeout = timeout >= 2500 ? timeout : 2500;
        try
        {
          if (this.adminRights)
          {
            if (!SpoolerHelper.IsSpoolerRunning)
              goto label_5;
          }
          SpoolerHelper.StopSpooler();
          Thread.Sleep(250);
          this.adminRights = true;
        }
        catch
        {
          this.adminRights = false;
        }
label_5:
        int num = 1;
        if (this.adminRights)
        {
          do
          {
            Thread.Sleep(250);
            this.handle = FileIOHelper.CreateFile("\\\\.\\NONSPOOLED_" + LPTName, FileIOHelper.EFileAccess.GENERIC_READ | FileIOHelper.EFileAccess.GENERIC_WRITE, FileShare.ReadWrite, IntPtr.Zero, FileMode.OpenOrCreate, FileIOHelper.EFileAttributes.Normal, IntPtr.Zero);
          }
          while (this.handle.IsInvalid && num++ <= 3);
        }
        else
        {
          do
          {
            Thread.Sleep(250);
            this.handle = FileIOHelper.CreateFile("\\\\.\\" + LPTName, FileIOHelper.EFileAccess.GENERIC_WRITE, FileShare.Write, IntPtr.Zero, FileMode.Create, FileIOHelper.EFileAttributes.Normal, IntPtr.Zero);
          }
          while (this.handle.IsInvalid && num++ <= 3);
        }
        if (this.handle.IsInvalid)
        {
          int lastWin32Error = Marshal.GetLastWin32Error();
          throw new Exception("Fail to open LPT port [" + LPTName + ": " + new Win32Exception(lastWin32Error).Message + "]");
        }
        if (this.adminRights)
        {
          FileIOHelper.COMMTIMEOUTS lpCommTimeouts;
          if (FileIOHelper.GetCommTimeouts(this.handle, out lpCommTimeouts))
          {
            lpCommTimeouts.WriteTotalTimeoutConstant = this.LPTtimeout;
            lpCommTimeouts.WriteTotalTimeoutMultiplier = 0;
            lpCommTimeouts.ReadTotalTimeoutConstant = this.LPTtimeout;
            lpCommTimeouts.ReadIntervalTimeout = 200;
            lpCommTimeouts.ReadTotalTimeoutMultiplier = 0;
            FileIOHelper.SetCommTimeouts(this.handle, ref lpCommTimeouts);
          }
          this.lptStream = new FileStream(this.handle, FileAccess.ReadWrite);
        }
        else
        {
          FileIOHelper.COMMTIMEOUTS lpCommTimeouts;
          if (FileIOHelper.GetCommTimeouts(this.handle, out lpCommTimeouts))
          {
            lpCommTimeouts.WriteTotalTimeoutConstant = this.LPTtimeout;
            lpCommTimeouts.WriteTotalTimeoutMultiplier = 0;
            FileIOHelper.SetCommTimeouts(this.handle, ref lpCommTimeouts);
          }
          this.lptStream = new FileStream(this.handle, FileAccess.Write);
        }
        if (this.lptStream == null)
          throw new Exception("Fail to access LPT port [" + Marshal.GetLastWin32Error().ToString() + "]");
        if (!this.isPermanent)
          return;
        if (!this.adminRights)
          throw new Exception("Administrator rights is required for Permanent connection with LTP port, Please restart application and run with Administrator rights");
        if (this.Send(Utils.StringToByteArray('\u0005'.ToString()), 1) == null)
          return;
        this.Gar = this.lptStream.BeginRead(this.buffer, 0, this.buffer.Length, new AsyncCallback(((InterfaceHelper) this).CBReceive), (object) this.lptStream);
      }
      catch (Exception ex)
      {
        this.Close();
        throw new Exception(ex.Message);
      }
    }

    public override byte[] Send(byte[] data, int ReplyCnt)
    {
      try
      {
        if (this.lptStream == null || this.handle == null || this.handle.IsInvalid)
          throw new Exception("Couldn't establish connection");
        if ((ReplyCnt <= 0 || !this.adminRights) && ReplyCnt > 0)
          throw new Exception("Administrator rights is required for LTP port reply, Please restart application and run with Administrator rights");
        try
        {
          this.lptStream.Write(data, 0, data.Length);
          this.lptStream.Flush();
        }
        catch
        {
          int lastWin32Error = Marshal.GetLastWin32Error();
          if (lastWin32Error == 170)
            throw new Exception("Fail to access LPT port [timeout is occurred]");
          throw new Exception("Fail to access LPT port [" + new Win32Exception(lastWin32Error).Message + "]");
        }
        if (ReplyCnt > 0)
        {
          this.buffer.Initialize();
          int length = 0;
          DateTime now = DateTime.Now;
          while (length <= 0)
          {
            Thread.Sleep(100);
            length = this.lptStream.Read(this.buffer, 0, this.buffer.Length);
            if ((DateTime.Now - now).TotalMilliseconds <= (double) (this.LPTtimeout * 50))
              Thread.Sleep(50);
            else
              break;
          }
          byte[] destinationArray = length > 0 ? new byte[length] : throw new Exception("No Reply received!");
          Array.Copy((Array) this.buffer, (Array) destinationArray, length);
          GC.Collect();
          return destinationArray;
        }
      }
      catch (Exception ex)
      {
        this.Close();
        throw new Exception(ex.Message);
      }
      return (byte[]) null;
    }

    public override void Close()
    {
      try
      {
        if (this.adminRights)
        {
          if (!SpoolerHelper.IsSpoolerRunning)
            SpoolerHelper.StartSpooler();
        }
      }
      catch
      {
      }
      try
      {
        if (this.lptStream != null)
        {
          this.lptStream.Close();
          this.lptStream.Dispose();
        }
        if (this.handle == null)
          return;
        this.handle.Close();
        this.handle.Dispose();
      }
      catch
      {
      }
      finally
      {
        this.lptStream = (FileStream) null;
        this.handle = (SafeFileHandle) null;
      }
    }
  }
}
