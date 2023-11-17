// Decompiled with JetBrains decompiler
// Type: SATOPrinterAPI.LanFinder
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
    internal class LanFinder
    {
        private static int SATOLANPORT = 19541;
        private static volatile bool closing = false;
        private static List<IPConfig> Printers = new List<IPConfig>();

        public static List<IPConfig> Results => LanFinder.Printers;

        private LanFinder()
        {
        }

        public static int Discover() => LanFinder.Discover(5000);

        public static int Discover(int delay_ms)
        {
            UdpClient state = (UdpClient)null;
            try
            {
                LanFinder.Printers.Clear();
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Broadcast, LanFinder.SATOLANPORT);
                state = new UdpClient();
                string s = "" + "\u0001LA";
                state.Send(Encoding.ASCII.GetBytes(s), s.Length, endPoint);
                LanFinder.closing = false;
                state.BeginReceive(new AsyncCallback(LanFinder.ReceiveCallback), (object)state);
                Thread.Sleep(delay_ms);
            }
            catch (Exception ex)
            {
                LanFinder.Printers.Clear();
                throw new Exception("Error: SATO LAN DISCOVER : " + ex.Message);
            }
            finally
            {
                if (state != null)
                {
                    LanFinder.closing = true;
                    state.Client.Shutdown(SocketShutdown.Both);
                    state.Close();
                }
            }
            return LanFinder.Printers.Count;
        }

        public static void ReceiveCallback(IAsyncResult ar)
        {
            int tickCount = Environment.TickCount;
            if (LanFinder.closing)
                return;
            UdpClient asyncState = (UdpClient)ar.AsyncState;
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
            try
            {
                if (asyncState == null || asyncState.Client == null)
                    return;
                byte[] numArray = asyncState.EndReceive(ar, ref remoteEP);
                if (!Encoding.ASCII.GetString(numArray).Contains("ERR(NOT SUPPORTED)") && numArray.Length == 59)
                {
                    IPConfig printer = LanFinder.ParsePrinter(numArray);
                    printer.IF = "SATO";
                    printer.Info = "SATO LAN Interface";
                    LanFinder.Printers.Add(printer);
                }
                asyncState.BeginReceive(new AsyncCallback(LanFinder.ReceiveCallback), (object)asyncState);
            }
            catch
            {
            }
        }

        public static IPConfig ParsePrinter(byte[] data)
        {
            int offset1 = 0;
            do
                ;
            while (data[offset1++] != (byte)2);
            IPConfig printer = new IPConfig();
            printer.MAC = NetConst.GetMACString(data, offset1, ':');
            int offset2 = offset1 + 7;
            printer.IP = NetConst.GetIPString(data, offset2, '.');
            int offset3 = offset2 + 5;
            printer.Subnet = NetConst.GetIPString(data, offset3, '.');
            int offset4 = offset3 + 5;
            printer.Gateway = NetConst.GetIPString(data, offset4, '.');
            int index1 = offset4 + 5;
            printer.Name = Encoding.ASCII.GetString(data, index1, 32).Trim(NetConst.TrimFilter);
            int num1 = index1 + 33;
            ref IPConfig local1 = ref printer;
            byte[] numArray1 = data;
            int index2 = num1;
            int num2 = index2 + 1;
            int num3 = numArray1[index2] == (byte)0 ? 0 : 1;
            local1.DHCPFlag = num3 != 0;
            ref IPConfig local2 = ref printer;
            byte[] numArray2 = data;
            int index3 = num2;
            int num4 = index3 + 1;
            int num5 = numArray2[index3] == (byte)0 ? 0 : 1;
            local2.RARPFlag = num5 != 0;
            return printer;
        }
    }
}
