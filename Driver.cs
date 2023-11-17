// Decompiled with JetBrains decompiler
// Type: SATOPrinterAPI.Driver
// Assembly: SATOPrinterAPI, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1CED7137-37FF-4597-9D4E-DAF488B01141
// Assembly location: D:\Downloads\Microsoft Edge\ApiPort.2.8.25.Offline\ApiPort.Offline\netcoreapp3.1\SATOPrinterAPI.dll

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
//using System.Management;
//using System.Printing;
using System.Runtime.InteropServices;
using System.Security.AccessControl;

namespace SATOPrinterAPI
{
    public class Driver
    {
    } 
}
/*[DllImport("winspool.drv", EntryPoint = "EnumPortsA", CharSet = CharSet.Ansi, SetLastError = true)]
    private static extern int EnumPorts(
      string pName,
      int Level,
      IntPtr lpbPorts,
      int cbBuf,
      ref int pcbNeeded,
      ref int pcReturned);

    public List<Driver.Info> GetDriverList()
    {
      List<Driver.Info> infoList = new List<Driver.Info>();
      foreach (ManagementObject managementObject in new ManagementObjectSearcher("SELECT Name,DriverName,PortName,WorkOffline,Default,EnableBIDI from Win32_Printer WHERE DriverName LIKE '%SATO%'").Get().Cast<ManagementObject>().Select<ManagementObject, ManagementObject>((Func<ManagementObject, ManagementObject>) (x => x)))
        infoList.Add(new Driver.Info()
        {
          DriverName = managementObject.Properties["Name"].Value.ToString(),
          PrinterModel = managementObject.Properties["DriverName"].Value.ToString(),
          PortName = managementObject.Properties["PortName"].Value.ToString(),
          Online = !managementObject.Properties["WorkOffline"].Value.ToString().ToLower().Equals("true"),
          Default = bool.Parse(managementObject.Properties["Default"].Value.ToString()),
          Bidirectional = managementObject.Properties["EnableBIDI"].Value.ToString().ToLower().Equals("true")
        });
      return infoList.Count > 0 ? infoList : (List<Driver.Info>) null;
    }

    public Driver.Info GetDriverInfo(string DriverName)
    {
      ManagementObjectCollection source = new ManagementObjectSearcher(string.Format("SELECT Name,DriverName,PortName,WorkOffline,Default,EnableBIDI FROM Win32_Printer Where Name = '{0}'", (object) DriverName)).Get();
      if (source == null || source.Count <= 0)
        return (Driver.Info) null;
      ManagementObject managementObject = source.OfType<ManagementObject>().FirstOrDefault<ManagementObject>();
      return new Driver.Info()
      {
        DriverName = managementObject.Properties["Name"].Value.ToString(),
        PrinterModel = managementObject.Properties[nameof (DriverName)].Value.ToString(),
        PortName = managementObject.Properties["PortName"].Value.ToString(),
        Online = !managementObject.Properties["WorkOffline"].Value.ToString().ToLower().Equals("true"),
        Default = bool.Parse(managementObject.Properties["Default"].Value.ToString()),
        Bidirectional = managementObject.Properties["EnableBIDI"].Value.ToString().ToLower().Equals("true")
      };
    }

    public void SetDriverInfo(Driver.Info Driver)
    {
      PutOptions options = new PutOptions();
      options.Type = PutType.UpdateOnly;
      foreach (ManagementObject managementObject in new ManagementObjectSearcher(string.Format("Select * from Win32_Printer Where Name = '{0}'", (object) Driver.DriverName)).Get())
      {
        if (Driver.DriverName != null && Driver.DriverName.Length > 0)
          managementObject.Properties["Name"].Value = (object) Driver.DriverName;
        if (Driver.PortName != null && Driver.PortName.Length > 0)
          managementObject.Properties["PortName"].Value = (object) Driver.PortName;
        managementObject.Properties["WorkOffline"].Value = (object) !Driver.Online;
        managementObject.Properties["Default"].Value = (object) Driver.Default;
        managementObject.Properties["EnableBIDI"].Value = (object) Driver.Bidirectional;
        managementObject.Put(options);
      }
    }

    public int GetSpoolerPrintJobsNumber(string DriverName)
    {
      PrintQueue printQueue = new LocalPrintServer().GetPrintQueues().Cast<PrintQueue>().Where<PrintQueue>((Func<PrintQueue, bool>) (x => x.FullName == DriverName)).FirstOrDefault<PrintQueue>();
      int spoolerPrintJobsNumber = -1;
      if (printQueue != null)
        spoolerPrintJobsNumber = printQueue.NumberOfJobs;
      return spoolerPrintJobsNumber;
    }

    public void ClearSpoolerPrintJobs(string DriverName)
    {
      PrintQueue printQueue = new LocalPrintServer().GetPrintQueues().Cast<PrintQueue>().Where<PrintQueue>((Func<PrintQueue, bool>) (x => x.FullName == DriverName)).FirstOrDefault<PrintQueue>();
      if (printQueue == null)
        return;
      printQueue.Refresh();
      foreach (PrintSystemJobInfo printJobInfo in printQueue.GetPrintJobInfoCollection())
        printJobInfo.Cancel();
    }

    public Driver.PortInfo GetPortInfoByDriverName(string DriverName) => this.RetrievePortInfo(DriverName, (string) null);

    public Driver.PortInfo GetPortInfoByName(string PortName) => this.RetrievePortInfo((string) null, PortName);

    private Driver.PortInfo RetrievePortInfo(string DriverName, string PortName)
    {
      Driver.PortInfo portInfo = new Driver.PortInfo();
      string portName = "";
      if (DriverName != null)
      {
        ManagementObject managementObject = new ManagementObjectSearcher(string.Format("SELECT PortName from Win32_Printer WHERE Name = '{0}'", (object) DriverName)).Get().OfType<ManagementObject>().FirstOrDefault<ManagementObject>();
        if (managementObject == null)
          return (Driver.PortInfo) null;
        portName = managementObject.Properties[nameof (PortName)].Value.ToString();
      }
      else if (PortName != null)
        portName = PortName;
      RegistryKey registryKey1 = (RegistryKey) null;
      RegistryKey registryKey2 = (RegistryKey) null;
      bool flag = false;
      string name = "System\\CurrentControlSet\\Control\\Print\\Monitors";
      using (RegistryKey registryKey3 = Registry.LocalMachine.OpenSubKey(name))
      {
        foreach (string str in ((IEnumerable<string>) registryKey3.GetSubKeyNames()).Where<string>((Func<string, bool>) (x => x.ToUpper().Contains("SATO") && x.ToUpper().Contains("ADVANCED PORT"))).ToList<string>())
        {
          using (RegistryKey registryKey4 = Registry.LocalMachine.OpenSubKey("System\\CurrentControlSet\\Control\\Print\\Monitors\\" + str + "\\Ports"))
          {
            using (List<string>.Enumerator enumerator = ((IEnumerable<string>) registryKey4.GetSubKeyNames()).Where<string>((Func<string, bool>) (x => x == portName)).ToList<string>().GetEnumerator())
            {
              if (enumerator.MoveNext())
              {
                string current = enumerator.Current;
                registryKey2 = Registry.LocalMachine.OpenSubKey("System\\CurrentControlSet\\Control\\Print\\Monitors\\" + str + "\\Ports\\" + portName, RegistryKeyPermissionCheck.Default, RegistryRights.QueryValues);
                flag = true;
              }
            }
          }
          if (flag)
            break;
        }
        if (!flag)
        {
          foreach (string str in ((IEnumerable<string>) registryKey3.GetSubKeyNames()).Where<string>((Func<string, bool>) (x => x.ToUpper().Contains("STANDARD TCP/IP"))).ToList<string>())
          {
            using (RegistryKey registryKey5 = Registry.LocalMachine.OpenSubKey("System\\CurrentControlSet\\Control\\Print\\Monitors\\" + str + "\\Ports"))
            {
              using (List<string>.Enumerator enumerator = ((IEnumerable<string>) registryKey5.GetSubKeyNames()).Where<string>((Func<string, bool>) (x => x == portName)).ToList<string>().GetEnumerator())
              {
                if (enumerator.MoveNext())
                {
                  string current = enumerator.Current;
                  registryKey1 = Registry.LocalMachine.OpenSubKey("System\\CurrentControlSet\\Control\\Print\\Monitors\\" + str + "\\Ports\\" + portName, RegistryKeyPermissionCheck.Default, RegistryRights.QueryValues);
                }
              }
            }
          }
        }
      }
      portInfo.Name = portName;
      if (registryKey2 != null)
      {
        portInfo.IPAddress = (string) registryKey2.GetValue("IPAddress", (object) string.Empty, RegistryValueOptions.DoNotExpandEnvironmentNames);
        int num = (int) registryKey2.GetValue("PortNumber");
        portInfo.Port = num.ToString();
        portInfo.Interface = Printer.InterfaceType.TCPIP;
        return portInfo;
      }
      if (registryKey1 != null)
      {
        portInfo.IPAddress = (string) registryKey1.GetValue("HostName", (object) string.Empty, RegistryValueOptions.DoNotExpandEnvironmentNames);
        int num = (int) registryKey1.GetValue("PortNumber");
        portInfo.Port = num.ToString();
        portInfo.Interface = Printer.InterfaceType.TCPIP;
        return portInfo;
      }
      /*if (portName.Contains("USB"))
      {
        string str = this.USBPortMatch(portName);
        if (str != null)
        {
          portInfo.IPAddress = (string) null;
          portInfo.Port = str;
          portInfo.Interface = Printer.InterfaceType.USB;
          return portInfo;
        }
      }
      /*else
      {
        if (portName.Contains("COM"))
        {
          portInfo.IPAddress = (string) null;
          portInfo.Port = portName.Replace(":", "");
          portInfo.Interface = Printer.InterfaceType.COM;
          return portInfo;
        }
        if (portName.Contains("LPT"))
        {
          portInfo.IPAddress = (string) null;
          portInfo.Port = portName.Replace(":", "");
          portInfo.Interface = Printer.InterfaceType.LPT;
          return portInfo;
        }
      }
      return (Driver.PortInfo) null;
    }

    public List<string> GetPortNames()
    {
      int pcbNeeded = 0;
      int pcReturned = 0;
      IntPtr num = IntPtr.Zero;
      IntPtr zero1 = IntPtr.Zero;
      List<string> portNames = new List<string>();
      Driver.EnumPorts("", 2, num, 0, ref pcbNeeded, ref pcReturned);
      try
      {
        num = Marshal.AllocHGlobal(Convert.ToInt32(pcbNeeded + 1));
        if (Driver.EnumPorts("", 2, num, pcbNeeded, ref pcbNeeded, ref pcReturned) == 0)
          throw new Win32Exception(Marshal.GetLastWin32Error());
        IntPtr ptr = num;
        Driver.PORT_INFO_2[] portInfo2Array = new Driver.PORT_INFO_2[pcReturned];
        for (int index = 0; index < pcReturned; ++index)
        {
          portInfo2Array[index] = (Driver.PORT_INFO_2) Marshal.PtrToStructure(ptr, typeof (Driver.PORT_INFO_2));
          ptr = (IntPtr) (ptr.ToInt32() + Marshal.SizeOf(typeof (Driver.PORT_INFO_2)));
        }
        zero1 = IntPtr.Zero;
        for (int index = 0; index < pcReturned; ++index)
          portNames.Add(portInfo2Array[index].pPortName);
        portNames.Sort();
      }
      catch (Exception ex)
      {
        throw new Exception(string.Format("Error getting available ports: {0}", (object) ex.Message));
      }
      finally
      {
        if (num != IntPtr.Zero)
        {
          Marshal.FreeHGlobal(num);
          IntPtr zero2 = IntPtr.Zero;
          zero1 = IntPtr.Zero;
        }
      }
      return portNames;
    }

    public string GetVersion(string DriverName)
    {
      string str1 = (string) null;
      string str2 = new ManagementObjectSearcher(string.Format("Select * from Win32_Printer Where Name = '{0}'", (object) DriverName)).Get().Cast<ManagementObject>().Select<ManagementObject, string>((Func<ManagementObject, string>) (x => x.Properties[nameof (DriverName)].Value.ToString())).FirstOrDefault<string>();
      if (str2 != null)
      {
        string fileName = new ManagementObjectSearcher(string.Format("Select * from Win32_PrinterDriver Where Name Like '%{0}%'", (object) str2)).Get().Cast<ManagementObject>().Select<ManagementObject, string>((Func<ManagementObject, string>) (x => x.Properties["DriverPath"].Value.ToString())).FirstOrDefault<string>();
        if (fileName != null)
        {
          FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(fileName);
          str1 = versionInfo.CompanyName + "|" + versionInfo.FileVersion;
        }
      }
      return str1.ToUpper();
    }

    public bool SendRawData(string DriverName, string Data) => RawPrinterHelper.SendStringToPrinter(DriverName, Data);

    public bool SendRawData(string DriverName, byte[] Data)
    {
      IntPtr num1 = new IntPtr(0);
      int length = Data.Length;
      IntPtr num2 = Marshal.AllocCoTaskMem(length);
      Marshal.Copy(Data, 0, num2, length);
      return RawPrinterHelper.SendBytesToPrinter(DriverName, num2, length);
    }

    /*private string USBPortMatch(string Port)
    {
      List<Printer.USBInfo> usbList = new Printer().GetUSBList();
      List<string> stringList1 = new List<string>();
      List<string> stringList2 = new List<string>();
      string str1 = "";
      string str2 = "";
      foreach (ManagementObject managementObject in new ManagementObjectSearcher(string.Format("SELECT DeviceID,Service FROM Win32_PnPEntity WHERE DeviceID LIKE '%{0}%' OR Service LIKE '%usbprint%'", (object) Port)).Get())
      {
        if (managementObject.Properties["DeviceID"].Value != null && managementObject.Properties["DeviceID"].Value.ToString().Contains(Port))
        {
          str1 = managementObject.Properties["DeviceID"].Value.ToString().Split('\\')[2].Replace(Port, "");
          str1 = str1.Substring(0, str1.Length - 1);
        }
        if (managementObject.Properties["Service"].Value != null && managementObject.Properties["Service"].Value.ToString().ToLower().Contains("usbprint"))
        {
          string[] strArray = managementObject.Properties["DeviceID"].Value.ToString().Split('\\');
          stringList1.Add(strArray[1]);
          stringList2.Add(strArray[2]);
        }
      }
      for (int index = 0; index < stringList1.Count; ++index)
      {
        try
        {
          if (((string) Registry.LocalMachine.OpenSubKey("System\\CurrentControlSet\\Enum\\USB\\" + stringList1[index].ToUpper() + "\\" + stringList2[index].ToLower()).GetValue("ParentIdPrefix", (object) string.Empty, RegistryValueOptions.DoNotExpandEnvironmentNames)).ToUpper() == str1.ToUpper())
          {
            str2 = "USB#" + stringList1[index].ToUpper() + "#" + stringList2[index].ToUpper();
            break;
          }
        }
        catch
        {
        }
      }
      foreach (Printer.USBInfo usbInfo in usbList)
      {
        if (usbInfo.PortID.ToUpper().Contains(str2))
          return usbInfo.PortID;
      }
      return (string) null;
    }

    /*[Serializable]
    public class Info
    {
      public string PrinterModel { get; set; }

      public string DriverName { get; set; }

      public string PortName { get; set; }

      public bool Online { get; set; }

      public bool Default { get; set; }

      public bool Bidirectional { get; set; }
    }

    [Serializable]
    public class PortInfo
    {
      public string Name { get; set; }

      public string IPAddress { get; set; }

      public string Port { get; set; }

      public Printer.InterfaceType Interface { get; set; }
    }

    private struct PORT_INFO_2
    {
      public string pPortName;
      public string pMonitorName;
      public string pDescription;
      public Driver.PortType fPortType;
      internal int Reserved;
    }

    [Flags]
    private enum PortType
    {
      write = 1,
      read = 2,
      redirected = 4,
      net_attached = 8,
    }
  }
}
*/