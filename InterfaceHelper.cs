// Decompiled with JetBrains decompiler
// Type: SATOPrinterAPI.InterfaceHelper
// Assembly: SATOPrinterAPI, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1CED7137-37FF-4597-9D4E-DAF488B01141
// Assembly location: D:\Downloads\Microsoft Edge\ApiPort.2.8.25.Offline\ApiPort.Offline\netcoreapp3.1\SATOPrinterAPI.dll

using System;

namespace SATOPrinterAPI
{
  internal abstract class InterfaceHelper
  {
    protected bool isPermanent;
    protected const int BufSize = 1048576;
    protected byte[] buffer = new byte[1048576];
    protected IAsyncResult Gar;
    public DelegateVoidByteArray fOnReceiveCallBack;

    public InterfaceHelper(bool PermanentConnect) => this.isPermanent = PermanentConnect;

    public abstract void Open(string name, object data, int timeout);

    public abstract void Close();

    public abstract void CBReceive(IAsyncResult ar);

    public abstract byte[] Send(byte[] data, int ReplyCnt);
  }
}
