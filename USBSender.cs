// Decompiled with JetBrains decompiler
// Type: SATOPrinterAPI.USBSender
// Assembly: SATOPrinterAPI, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1CED7137-37FF-4597-9D4E-DAF488B01141
// Assembly location: D:\Downloads\Microsoft Edge\ApiPort.2.8.25.Offline\ApiPort.Offline\netcoreapp3.1\SATOPrinterAPI.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
//using USBConnectLib;

namespace SATOPrinterAPI
{
  internal class USBSender
  {
    /*public static UsbInfo[] GetActiveDeviceNames() => USBSender.GetDeviceList(USBIDs.Valid_VIDs);

    private static bool compare(int val1, int val2) => val1 == val2;

    private static UsbInfo[] GetDeviceList(int[] VIDs)
    {
      string[] strArray1;
      string[] strArray2;
      int usbDeviceList = USBPort.GetUsbDeviceList(ref strArray1, ref strArray2);
      List<UsbInfo> usbInfoList = new List<UsbInfo>();
      for (int index = 0; index < usbDeviceList; ++index)
      {
        int pid;
        int vdid;
        USBSender.ExtractIDs(strArray2[index], out vdid, out pid);
        if (Array.Find<int>(VIDs, (Predicate<int>) (e => e == vdid)) == vdid)
        {
          if (Enum.IsDefined(typeof (USBIDs.PIDs), (object) pid))
          {
            string name = USBIDs.PIDtoString(pid) + "[" + USBSender.GetPortDisplay(strArray2[index]) + "]";
            usbInfoList.Add(new UsbInfo(name, strArray2[index]));
          }
          else
          {
            string name = USBIDs.PIDtoString(pid) + " [" + USBSender.GetPortDisplay(strArray2[index]) + "]";
            usbInfoList.Add(new UsbInfo(name, strArray2[index]));
          }
        }
      }
      return usbInfoList.ToArray();
    }

    public static string[] SendUSBCmd(string portid, byte[] data) => USBSender.ByteAHexStringA(USBSender.SendUSBCommand(portid, data, false, 2000));

    public static string[] SendUSBCmd(string portid, byte[] data, bool hasReply) => USBSender.ByteAHexStringA(USBSender.SendUSBCommand(portid, data, hasReply, 2000));

    public static string[] SendUSBCmd(string portid, string data, bool hasReply) => USBSender.ByteAHexStringA(USBSender.SendUSBCommand(portid, Encoding.ASCII.GetBytes(data), hasReply, 2000));

    public static string[] SendUSBCmd(string portid, string data, bool hasReply, int timeout) => USBSender.ByteAHexStringA(USBSender.SendUSBCommand(portid, Encoding.ASCII.GetBytes(data), hasReply, timeout));

    public static string[] SendUSBCmd(string portid, byte[] data, bool hasReply, int timeout) => USBSender.ByteAHexStringA(USBSender.SendUSBCommand(portid, data, hasReply, timeout));

    public static string[] ByteAHexStringA(byte[] bytedata)
    {
      if (bytedata == null)
        return (string[]) null;
      string[] strArray = new string[bytedata.Length];
      for (int index = 0; index < bytedata.Length; ++index)
        strArray[index] = bytedata[1].ToString("X2");
      return strArray;
    }

    public static byte[] SendUSBCommand(string portid, byte[] data) => USBSender.SendUSBCommand(portid, data, false, 2000);

    public static byte[] SendUSBCommand(string portid, byte[] data, bool hasReply) => USBSender.SendUSBCommand(portid, data, hasReply, 2000);

    public static byte[] SendUSBCommand(string portid, string data, bool hasReply) => USBSender.SendUSBCommand(portid, Encoding.ASCII.GetBytes(data), hasReply, 2000);

    public static byte[] SendUSBCommand(string portid, string data, bool hasReply, int timeout) => USBSender.SendUSBCommand(portid, Encoding.ASCII.GetBytes(data), hasReply, timeout);

    public static byte[] SendUSBCommand(string portid, byte[] data, bool hasReply, int timeout)
    {
      byte[] numArray = (byte[]) null;
      USBPort usbPort = new USBPort();
      try
      {
        if (usbPort.Open(portid))
        {
          usbPort.Write(data, 500);
          if (hasReply)
            numArray = usbPort.Read(256, timeout);
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
      finally
      {
        usbPort.Close();
      }
      return numArray;
    }

    private static string ExtractIDs(string input, out int vid, out int pid)
    {
      vid = 0;
      pid = 0;
      try
      {
        Match match = Regex.Match(input, "usb#vid_([\\da-fA-F]+)&pid_([\\da-fA-F]+)#(.*)#{");
        if (match.Groups.Count < 4)
          return "Error";
        vid = int.Parse(match.Groups[1].Value, NumberStyles.HexNumber);
        pid = int.Parse(match.Groups[2].Value, NumberStyles.HexNumber);
        return match.Groups[2].Value + "_" + match.Groups[3].Value;
      }
      catch
      {
        vid = 0;
        pid = 0;
        return "Error";
      }
    }

    public static string GetPortDisplay(string port)
    {
      try
      {
        Match match = Regex.Match(port, "usb#vid_[\\da-fA-F]+&pid_([\\da-fA-F]+#.*)#{");
        return match.Groups.Count < 2 ? "Error" : match.Groups[1].Value;
      }
      catch
      {
      }
      return port;
    }*/
  }
}
