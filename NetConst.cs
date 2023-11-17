// Decompiled with JetBrains decompiler
// Type: SATOPrinterAPI.NetConst
// Assembly: SATOPrinterAPI, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1CED7137-37FF-4597-9D4E-DAF488B01141
// Assembly location: D:\Downloads\Microsoft Edge\ApiPort.2.8.25.Offline\ApiPort.Offline\netcoreapp3.1\SATOPrinterAPI.dll

using System;

namespace SATOPrinterAPI
{
  internal class NetConst
  {
    public static int DiscvPortSilex = 19541;
    public static byte[] DiscvSilex = new byte[88]
    {
      (byte) 0,
      (byte) 4,
      (byte) 149,
      (byte) 6,
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
      (byte) 0
    };
    public static int DiscvPortSato = 19541;
    public static byte[] DiscvSato = new byte[3]
    {
      (byte) 1,
      (byte) 76,
      (byte) 65
    };
    public static char[] TrimFilter = new char[2]
    {
      ' ',
      char.MinValue
    };

    public static string GetMACString(byte[] data, int offset, char delimiter)
    {
      byte[] subBytes = NetConst.GetSubBytes(data, offset, 6);
      string macString = "";
      foreach (byte num in subBytes)
      {
        if (!string.IsNullOrEmpty(macString))
          macString += delimiter.ToString();
        macString += num.ToString("X2");
      }
      return macString;
    }

    public static string GetIPString(byte[] data, int offset, char delimiter)
    {
      byte[] subBytes = NetConst.GetSubBytes(data, offset, 4);
      string ipString = "";
      foreach (byte num in subBytes)
      {
        if (!string.IsNullOrEmpty(ipString))
          ipString += delimiter.ToString();
        ipString += num.ToString();
      }
      return ipString;
    }

    public static byte[] GetSubBytes(byte[] data, int offset, int count)
    {
      byte[] destinationArray = new byte[count];
      Array.Copy((Array) data, offset, (Array) destinationArray, 0, count);
      return destinationArray;
    }
  }
}
