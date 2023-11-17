// Decompiled with JetBrains decompiler
// Type: SATOPrinterAPI.GLLanFinder
// Assembly: SATOPrinterAPI, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1CED7137-37FF-4597-9D4E-DAF488B01141
// Assembly location: D:\Downloads\Microsoft Edge\ApiPort.2.8.25.Offline\ApiPort.Offline\netcoreapp3.1\SATOPrinterAPI.dll

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SATOPrinterAPI
{
  internal class GLLanFinder
  {
    private static List<IPConfig> Printers = new List<IPConfig>();
    private static byte[] udp_pkg = new byte[57]
    {
      (byte) 0,
      (byte) 128,
      (byte) 114,
      (byte) 2,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 128,
      (byte) 114,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      (byte) 10,
      (byte) 25,
      (byte) 5,
      (byte) 9,
      (byte) 7,
      (byte) 241,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 1,
      (byte) 9,
      (byte) 0,
      (byte) 0,
      (byte) 2,
      (byte) 11,
      (byte) 0,
      (byte) 13,
      (byte) 18,
      (byte) 133,
      (byte) 201,
      (byte) 212,
      (byte) 160,
      (byte) 120,
      (byte) 82,
      (byte) 22,
      (byte) 54,
      (byte) 27,
      (byte) 250,
      (byte) 151,
      (byte) 30
    };
    private static List<IPAddress> GL_List;
    private static int curPort = 0;
    private static UdpClient _udpClient0 = (UdpClient) null;
    private static volatile bool closing = false;
    public static int Timeout = 5000;
    private static int bb;

    public static List<IPConfig> Results => GLLanFinder.Printers;

    private GLLanFinder()
    {
    }

    public static int Discover() => GLLanFinder.FindPrinterIPs(5000).Count;

    public static int Discover(int timeout) => GLLanFinder.FindPrinterIPs(timeout).Count;

    private static List<IPAddress> FindPrinterIPs(int Tout_ms)
    {
      GLLanFinder.GL_List = new List<IPAddress>();
      GLLanFinder.GL_List.Clear();
      GLLanFinder.Printers.Clear();
      GLLanFinder.curPort = 0;
      GLLanFinder._udpClient0 = new UdpClient(new IPEndPoint(GLLanFinder.GetLocalDNSHostIP(), 0));
      UdpClient udpClient = new UdpClient();
      GLLanFinder.curPort = ((IPEndPoint) GLLanFinder._udpClient0.Client.LocalEndPoint).Port;
      IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Broadcast, 9);
      GLLanFinder.closing = false;
      GLLanFinder._udpClient0.BeginReceive(new AsyncCallback(GLLanFinder.ReceiveCallback), (object) GLLanFinder._udpClient0);
      udpClient.EnableBroadcast = true;
      IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("255.255.255.255"), 9);
      IPAddress[] hostAddresses = Dns.GetHostAddresses(Dns.GetHostName());
      int index1 = 0;
      IPAddress[] ipAddressArray = hostAddresses;
      for (int index2 = 0; index2 < ipAddressArray.Length && ipAddressArray[index2].AddressFamily != AddressFamily.InterNetwork; ++index2)
        ++index1;
      if (hostAddresses.Length != 0)
      {
        byte[] addressBytes = GLLanFinder.GetLocalDNSHostIP().GetAddressBytes();
        if (GLLanFinder.GetLocalDNSHostIP() == IPAddress.Any)
          addressBytes = hostAddresses[index1].GetAddressBytes();
        GLLanFinder.udp_pkg[12] = addressBytes[0];
        GLLanFinder.udp_pkg[13] = addressBytes[1];
        GLLanFinder.udp_pkg[14] = addressBytes[2];
        GLLanFinder.udp_pkg[15] = addressBytes[3];
      }
      GLLanFinder.udp_pkg[16] = (byte) (GLLanFinder.curPort >> 8 & (int) byte.MaxValue);
      GLLanFinder.udp_pkg[17] = (byte) (GLLanFinder.curPort & (int) byte.MaxValue);
      GLLanFinder.bb = Environment.TickCount;
      try
      {
        udpClient.Send(GLLanFinder.udp_pkg, 57, endPoint);
      }
      catch
      {
      }
      Thread.Sleep(Tout_ms);
      GLLanFinder.closing = true;
      udpClient.Close();
      GLLanFinder._udpClient0.Close();
      return GLLanFinder.GL_List;
    }

    public static void ReceiveCallback(IAsyncResult ar)
    {
      int tickCount = Environment.TickCount;
      int num = GLLanFinder.closing ? 1 : 0;
      GLLanFinder.bb = tickCount;
      if (GLLanFinder.closing)
        return;
      UdpClient asyncState = (UdpClient) ar.AsyncState;
      IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
      try
      {
        if (asyncState == null || asyncState.Client == null)
          return;
        byte[] data = asyncState.EndReceive(ar, ref remoteEP);
        GLLanFinder.Printers.Add(new IPConfig()
        {
          IF = "GL",
          MAC = NetConst.GetMACString(data, 6, ':'),
          IP = NetConst.GetIPString(data, 24, '.'),
          Subnet = NetConst.GetIPString(data, 28, '.'),
          Gateway = NetConst.GetIPString(data, 32, '.'),
          Info = "GL Printer with default IP setting",
          Name = "GL Printer"
        });
        GLLanFinder.GL_List.Add(remoteEP.Address);
        asyncState.BeginReceive(new AsyncCallback(GLLanFinder.ReceiveCallback), (object) asyncState);
      }
      catch (Exception ex)
      {
      }
    }

    internal static IPAddress GetLocalDNSHostIP()
    {
      foreach (IPAddress hostAddress in Dns.GetHostAddresses(Dns.GetHostName()))
      {
        if (hostAddress.AddressFamily == AddressFamily.InterNetwork)
          return hostAddress;
      }
      return IPAddress.Any;
    }
  }
}
