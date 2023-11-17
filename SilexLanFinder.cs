// Decompiled with JetBrains decompiler
// Type: SATOPrinterAPI.SilexLanFinder
// Assembly: SATOPrinterAPI, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1CED7137-37FF-4597-9D4E-DAF488B01141
// Assembly location: D:\Downloads\Microsoft Edge\ApiPort.2.8.25.Offline\ApiPort.Offline\netcoreapp3.1\SATOPrinterAPI.dll

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SATOPrinterAPI
{
  internal class SilexLanFinder
  {
    private static volatile bool closing = false;
    private static List<IPConfig> Printers = new List<IPConfig>();

    public static List<IPConfig> Results => SilexLanFinder.Printers;

    private SilexLanFinder()
    {
    }

    public static int Discover() => SilexLanFinder.Discover(5000);

    public static int Discover(int delay_ms)
    {
      UdpClient state = (UdpClient) null;
      try
      {
        SilexLanFinder.Printers.Clear();
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Broadcast, NetConst.DiscvPortSilex);
        state = new UdpClient();
        state.Send(NetConst.DiscvSilex, NetConst.DiscvSilex.Length, endPoint);
        SilexLanFinder.closing = false;
        state.BeginReceive(new AsyncCallback(SilexLanFinder.ReceiveCallback), (object) state);
        Thread.Sleep(delay_ms);
      }
      catch (Exception ex)
      {
        SilexLanFinder.Printers.Clear();
        throw new Exception("Error: SATO LAN DISCOVER : " + ex.Message);
      }
      finally
      {
        if (state != null)
        {
          SilexLanFinder.closing = true;
          state.Client.Shutdown(SocketShutdown.Both);
          state.Close();
        }
      }
      return SilexLanFinder.Printers.Count;
    }

    public static void ReceiveCallback(IAsyncResult ar)
    {
      if (SilexLanFinder.closing)
        return;
      UdpClient asyncState = (UdpClient) ar.AsyncState;
      IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
      try
      {
        if (asyncState == null || asyncState.Client == null)
          return;
        byte[] numArray = asyncState.EndReceive(ar, ref remoteEP);
        if (!Encoding.ASCII.GetString(numArray).Contains("ERR(NOT SUPPORTED)"))
          SilexLanFinder.Printers.Add(new IPConfig()
          {
            IF = "Silex",
            Name = Encoding.ASCII.GetString(numArray, 4, 32).Trim(NetConst.TrimFilter),
            MAC = NetConst.GetMACString(numArray, 56, ':'),
            IP = NetConst.GetIPString(numArray, 66, '.'),
            Info = Encoding.ASCII.GetString(numArray, 36, 20).Trim(NetConst.TrimFilter)
          });
        asyncState.BeginReceive(new AsyncCallback(SilexLanFinder.ReceiveCallback), (object) asyncState);
      }
      catch (Exception ex)
      {
      }
    }
  }
}
