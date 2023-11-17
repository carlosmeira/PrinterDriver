// Decompiled with JetBrains decompiler
// Type: SATOPrinterAPI.SocketHelper
// Assembly: SATOPrinterAPI, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1CED7137-37FF-4597-9D4E-DAF488B01141
// Assembly location: D:\Downloads\Microsoft Edge\ApiPort.2.8.25.Offline\ApiPort.Offline\netcoreapp3.1\SATOPrinterAPI.dll

using System;
using System.Diagnostics;
using System.Net.Sockets;

namespace SATOPrinterAPI
{
  internal class SocketHelper : InterfaceHelper
  {
    private Socket sock;
    private NetworkStream NStream;
    private bool _isUDP;
    private int TCPtimeout = 2500;

    public SocketHelper(bool PermanentConnect, bool IsUDP)
      : base(PermanentConnect)
    {
      this._isUDP = IsUDP;
    }

    public override void Open(string name, object data, int timeout)
    {
      try
      {
        this.TCPtimeout = timeout;
        int port = (int) data;
        this.sock = this._isUDP ? new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp) : new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        if (!this.sock.BeginConnect(name, port, (AsyncCallback) null, (object) this.sock).AsyncWaitHandle.WaitOne(this.TCPtimeout, false))
          throw new Exception("Host Unreachable!");
        this.sock.SendTimeout = this.TCPtimeout;
        this.sock.ReceiveTimeout = this.TCPtimeout;
        if (!this.isPermanent)
          return;
        this.Gar = this.sock.BeginReceive(this.buffer, 0, this.buffer.Length, SocketFlags.None, new AsyncCallback(((InterfaceHelper) this).CBReceive), (object) this.sock);
      }
      catch (Exception ex)
      {
        if (this.sock != null)
          this.sock.Close();
        this.sock = (Socket) null;
        throw new Exception("Fail to Open TCPIP port : " + ex.Message);
      }
    }

    public override void Close()
    {
      if (this.NStream != null)
      {
        this.NStream.Close();
        this.NStream = (NetworkStream) null;
      }
      if (this.sock == null)
        return;
      this.sock.Shutdown(SocketShutdown.Both);
      this.sock.Close();
      this.sock = (Socket) null;
    }

    public override void CBReceive(IAsyncResult ar)
    {
      if (!(ar.AsyncState is Socket asyncState))
        return;
      if (!asyncState.Connected)
        return;
      try
      {
        int length = asyncState.EndReceive(ar);
        if (length > 0)
        {
          byte[] numArray = new byte[length];
          Array.Copy((Array) this.buffer, (Array) numArray, length);
          if (this.fOnReceiveCallBack != null)
            this.fOnReceiveCallBack(numArray);
        }
        this.buffer.Initialize();
        this.Gar = asyncState.BeginReceive(this.buffer, 0, this.buffer.Length, SocketFlags.None, new AsyncCallback(((InterfaceHelper) this).CBReceive), (object) asyncState);
      }
      catch
      {
        if (asyncState == null)
          return;
        if (asyncState.Connected)
        {
          asyncState.Shutdown(SocketShutdown.Both);
          asyncState.Close();
        }
      }
    }

    public override byte[] Send(byte[] data, int ReplyCnt)
    {
      if (this.sock != null)
      {
        if (this.sock.Connected)
        {
          try
          {
            this.sock.Send(data);
            if (!this.isPermanent)
            {
              if (ReplyCnt > 0)
              {
                for (int index = 0; index < ReplyCnt; ++index)
                {
                  this.buffer.Initialize();
                  Stopwatch stopwatch = Stopwatch.StartNew();
                  this.sock.ReceiveTimeout = this.TCPtimeout;
                  int length = this.sock.Receive(this.buffer);
                  stopwatch.Stop();
                  if (length > 0)
                  {
                    byte[] destinationArray = new byte[length];
                    Array.Copy((Array) this.buffer, (Array) destinationArray, length);
                    return destinationArray;
                  }
                  if (index == ReplyCnt - 1)
                    throw new Exception("No Reply received!");
                }
              }
            }
          }
          catch (Exception ex)
          {
            if (this.sock != null)
            {
              this.sock.Shutdown(SocketShutdown.Both);
              this.sock.Close();
            }
            this.sock = (Socket) null;
            throw new Exception(ex.Message);
          }
          return (byte[]) null;
        }
      }
      throw new Exception("Couldn't establish connection");
    }
  }
}
