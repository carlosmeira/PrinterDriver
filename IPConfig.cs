// Decompiled with JetBrains decompiler
// Type: SATOPrinterAPI.IPConfig
// Assembly: SATOPrinterAPI, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1CED7137-37FF-4597-9D4E-DAF488B01141
// Assembly location: D:\Downloads\Microsoft Edge\ApiPort.2.8.25.Offline\ApiPort.Offline\netcoreapp3.1\SATOPrinterAPI.dll

using System.Globalization;
using System.Net;

namespace SATOPrinterAPI
{
  internal struct IPConfig
  {
    public string Name;
    public string IP;
    public string Subnet;
    public string Gateway;
    public bool DHCPFlag;
    public bool RARPFlag;
    public string MAC;
    public string Info;
    public bool UdpBCmode;
    public string IF;
    public string Status;
    public string WSSID;
    public int WCh;
    public bool IsWLan;

    public static string MACArray2String(byte[] data, char seperator) => string.Format("{0:X2}" + seperator.ToString() + "{1:X2}" + seperator.ToString() + "{2:X2}" + seperator.ToString() + "{3:X2}" + seperator.ToString() + "{4:X2}" + seperator.ToString() + "{5:X2}", (object) data[0], (object) data[1], (object) data[2], (object) data[3], (object) data[4], (object) data[5]);

    public static byte[] MACString2Array(string data)
    {
      byte[] numArray = new byte[6];
      int num = 0;
      try
      {
        for (int startIndex = 0; startIndex < 18; startIndex += 3)
          numArray[num++] = byte.Parse(data.Substring(startIndex, 2), NumberStyles.HexNumber);
      }
      catch
      {
        numArray = (byte[]) null;
      }
      return numArray;
    }

    public static string IPArray2String(byte[] data) => new IPAddress(data).ToString();

    public static byte[] IPString2Array(string data) => IPAddress.Parse(data).GetAddressBytes();
  }
}
