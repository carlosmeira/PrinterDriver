// Type: SATOPrinterAPI.Printer
// Assembly: SATOPrinterAPI, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
//using System.Management;
using System.Reflection;
using System.Text;
using System.Threading;

namespace SATOPrinterAPI
{
  public class Printer
  {
    private InterfaceHelper interfaceConnection;
    private const int LastConnectLimit = 10;
    public List<Printer.USBInfo> USBDeviceLists;
    private List<Printer.TCPIPInfo> TCPIPList;
    private static volatile AutoResetEvent autoReset = new AutoResetEvent(false);
    private byte[] queryData;
    private bool queryFlag;

    public Printer.InterfaceType? Interface { get; set; }

    public bool PermanentConnect { get; set; }

    public string USBPortID { get; set; }

    public string COMPort { get; set; }

    public string LPTPort { get; set; }

    public string TCPIPPort { get; set; }

    public string TCPIPAddress { get; set; }

    public SerialCommSetting COMSetting { get; set; }

    public int Timeout { get; set; }

    public Printer()
    {
      this.PermanentConnect = false;
      this.COMSetting = new SerialCommSetting();
      this.Interface = new Printer.InterfaceType?();
      this.Timeout = 2500;
      /*if (Environment.Is64BitProcess)
        EmbeddedAssembly.Load("SATOPrinterAPI.Resources.x64.USBConnectLib.dll", "USBConnectLib.dll");
      else
        EmbeddedAssembly.Load("SATOPrinterAPI.Resources.x86.USBConnectLib.dll", "USBConnectLib.dll");
      */
      AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(Printer.CurrentDomain_AssemblyResolve);
      AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(Printer.CurrentDomain_AssemblyResolve);
    }

    private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args) => EmbeddedAssembly.Get(args.Name);

    /*public List<Printer.USBInfo> GetUSBList()
    {
      List<Printer.USBInfo> source = new List<Printer.USBInfo>();
      foreach (UsbInfo activeDeviceName in USBSender.GetActiveDeviceNames())
        source.Add(new Printer.USBInfo()
        {
          Name = activeDeviceName.Name,
          PortID = activeDeviceName.PortID
        });
      if (source != null)
        source = source.OrderBy<Printer.USBInfo, string>((Func<Printer.USBInfo, string>) (x => x.Name)).ToList<Printer.USBInfo>();
      return source;
    }
    */

    public List<Printer.TCPIPInfo> GetTCPIPList(int timeout = 2000)
    {
      Discovery discovery = new Discovery(new Del_VoidPrinters(this.OnEndDiscovery));
      if (timeout < 1500)
        timeout = 1500;
      int timespan = timeout - 1000;
      discovery.Start(timespan);
      Thread.Sleep(timeout);
      if (this.TCPIPList != null)
        this.TCPIPList = this.TCPIPList.OrderBy<Printer.TCPIPInfo, string>((Func<Printer.TCPIPInfo, string>) (x => x.Name)).ThenBy<Printer.TCPIPInfo, string>((Func<Printer.TCPIPInfo, string>) (y => y.IPAddress)).ToList<Printer.TCPIPInfo>();
      return this.TCPIPList;
    }

    private void OnEndDiscovery(List<IPConfig> list)
    {
      this.TCPIPList = new List<Printer.TCPIPInfo>();
      foreach (IPConfig ipConfig in list)
      {
        Printer.TCPIPInfo tcpip = new Printer.TCPIPInfo();
        tcpip.Name = ipConfig.Name;
        tcpip.IPAddress = ipConfig.IP;
        tcpip.MacAddress = ipConfig.MAC;
        tcpip.Details = ipConfig.Info;
        if (this.TCPIPList.FirstOrDefault<Printer.TCPIPInfo>((Func<Printer.TCPIPInfo, bool>) (x => x.MacAddress == tcpip.MacAddress)) == null)
          this.TCPIPList.Add(tcpip);
      }
    }
/*
    public List<string> GetLPTList()
    {
      List<string> source = new List<string>();
      foreach (ManagementObject instance in new ManagementClass("Win32_ParallelPort").GetInstances())
        source.Add(instance["name"].ToString());
      if (source != null)
        source = source.OrderBy<string, string>((Func<string, string>) (x => x)).ToList<string>();
      return source;
    }
*/
    public List<string> GetCOMList()
    {
      List<string> source = new List<string>();
      foreach (string portName in SerialPort.GetPortNames())
        source.Add(portName);
      if (source != null)
        source = source.OrderBy<string, string>((Func<string, string>) (x => x)).ToList<string>();
      return source;
    }

    private void Open()
    {
      if (this.interfaceConnection != null)
      {
        this.interfaceConnection.Close();
        this.interfaceConnection = (InterfaceHelper) null;
      }
      Printer.InterfaceType? nullable = this.Interface;
      if (nullable.HasValue)
      {
        switch (nullable.GetValueOrDefault())
        {
          case Printer.InterfaceType.TCPIP:
            this.interfaceConnection = (InterfaceHelper) new SocketHelper(this.PermanentConnect, false);
            this.interfaceConnection.Open(this.TCPIPAddress, (object) int.Parse(this.TCPIPPort), this.Timeout);
            this.interfaceConnection.fOnReceiveCallBack = new DelegateVoidByteArray(this.ReadCallBack);
            return;
          case Printer.InterfaceType.USB:
            this.interfaceConnection = (InterfaceHelper) new USBHelper(this.PermanentConnect);
            this.interfaceConnection.Open(this.USBPortID, (object) 0, this.Timeout);
            this.interfaceConnection.fOnReceiveCallBack = new DelegateVoidByteArray(this.ReadCallBack);
            return;
          case Printer.InterfaceType.COM:
            this.interfaceConnection = (InterfaceHelper) new COMHelper(this.PermanentConnect);
            if (this.COMPort.Length > 0)
            {
              this.COMPort = this.COMPort.Replace(" [USB2Serial]", "");
              this.COMPort = this.COMPort.Replace(":", "");
            }
            this.interfaceConnection.Open(this.COMPort, (object) this.COMSetting, this.Timeout);
            this.interfaceConnection.fOnReceiveCallBack = new DelegateVoidByteArray(this.ReadCallBack);
            return;
          case Printer.InterfaceType.LPT:
            this.interfaceConnection = (InterfaceHelper) new LPTHelper(this.PermanentConnect);
            this.LPTPort = this.LPTPort.Replace(":", "");
            this.interfaceConnection.Open(this.LPTPort, (object) 0, this.Timeout);
            this.interfaceConnection.fOnReceiveCallBack = new DelegateVoidByteArray(this.ReadCallBack);
            return;
        }
      }
      this.interfaceConnection = (InterfaceHelper) null;
      throw new Exception("Invalid Interface type");
    }

    public void Send(byte[] Data)
    {
      if (!this.PermanentConnect || this.interfaceConnection == null)
      {
        this.Open();
        Thread.Sleep(100);
      }
      this.InterfaceSend(Data, !this.PermanentConnect ? 0 : -1);
      if (this.PermanentConnect)
        return;
      this.Close();
    }

    public byte[] Query(byte[] Data)
    {
      this.Open();
      byte[] numArray = this.InterfaceSend(Data, 3);
      this.Close();
      return numArray;
    }

    private void Close()
    {
      if (this.interfaceConnection == null)
        return;
      this.interfaceConnection.Close();
      this.interfaceConnection = (InterfaceHelper) null;
    }

    private byte[] InterfaceSend(byte[] data, int ReplyCnt)
    {
      try
      {
        if (this.interfaceConnection != null)
        {
          byte[] numArray = this.interfaceConnection.Send(data, ReplyCnt);
          if (ReplyCnt > 0)
            return numArray;
        }
      }
      catch (Exception ex)
      {
        this.interfaceConnection = (InterfaceHelper) null;
        throw new Exception(ex.Message);
      }
      return (byte[]) null;
    }

    public void Connect()
    {
      if (this.PermanentConnect)
      {
        this.Open();
      }
      else
      {
        this.Open();
        Thread.Sleep(100);
        this.Close();
      }
    }

    public void Disconnect() => this.Close();

    public Printer.Status GetPrinterStatus()
    {
      string str = (string) null;
      if (!this.Interface.HasValue)
        throw new Exception("Connection type is not defined");
      byte[] Data = new byte[1]{ (byte) 5 };
      if (!this.PermanentConnect)
      {
        this.queryData = this.Query(Data);
      }
      else
      {
        this.queryFlag = true;
        this.Send(Data);
        Printer.autoReset.WaitOne(this.Timeout, false);
      }
      if (this.queryData != null)
        str = Encoding.UTF8.GetString(this.queryData);
      if (str == null)
        return (Printer.Status) null;
      char ch = '\u0002';
      Printer.Status ps = new Printer.Status();
      ps.JobID = str.Substring(str.IndexOf(ch) + 1, 2).Trim();
      ps.Code = str.Substring(str.IndexOf(ch) + 3, 1);
      ps.Buffer = int.Parse(str.Substring(str.IndexOf(ch) + 4, 6));
      if (str.Substring(str.IndexOf(ch) + 1).Length > 10)
        ps.JobName = str.Substring(str.IndexOf(ch) + 10, 16).Trim();
      ps.Raw = str;
      this.AssignStatusState(ref ps);
      return ps;
    }

    public void TestPrint(bool TestPrintCommand = false, bool CompactPrinter = false, bool MBPrinter = false)
    {
      if (!this.Interface.HasValue)
        throw new Exception("Connection type is not defined");
      if (TestPrintCommand)
      {
        int num = 27;
        StringBuilder stringBuilder = new StringBuilder("");
        stringBuilder.Append((char) num);
        stringBuilder.Append("A");
        stringBuilder.Append((char) num);
        if (CompactPrinter)
          stringBuilder.Append("TP,3");
        else if (MBPrinter)
          stringBuilder.Append("PA3");
        else
          stringBuilder.Append("TP3");
        stringBuilder.Append((char) num);
        stringBuilder.Append("Z");
        this.Send(Utils.StringToByteArray(stringBuilder.ToString()));
      }
      else
      {
        using (Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SATOPrinterAPI.Resources.TestPrintCmd.prn"))
        {
          using (StreamReader streamReader = new StreamReader(manifestResourceStream))
          {
            using (MemoryStream destination = new MemoryStream())
            {
              streamReader.BaseStream.CopyTo((Stream) destination);
              this.Send(destination.ToArray());
            }
          }
        }
      }
    }

    public void ClearBuffer()
    {
      if (!this.Interface.HasValue)
        throw new Exception("Connection type is not defined");
      int num = 27;
      StringBuilder stringBuilder = new StringBuilder("");
      stringBuilder.Append((char) num);
      stringBuilder.Append("A");
      stringBuilder.Append((char) num);
      stringBuilder.Append("*");
      stringBuilder.Append((char) num);
      stringBuilder.Append("Z");
      this.Send(Utils.StringToByteArray(stringBuilder.ToString()));
    }

    public void Reprint()
    {
      if (!this.Interface.HasValue)
        throw new Exception("Connection type is not defined");
      int num = 27;
      StringBuilder stringBuilder = new StringBuilder("");
      stringBuilder.Append((char) num);
      stringBuilder.Append("A");
      stringBuilder.Append((char) num);
      stringBuilder.Append("C");
      stringBuilder.Append((char) num);
      stringBuilder.Append("Z");
      this.Send(Utils.StringToByteArray(stringBuilder.ToString()));
    }

    private void AssignStatusState(ref Printer.Status ps)
    {
      switch (ps.Code)
      {
        case "!":
          ps.IsOnline = true;
          ps.IsError = false;
          ps.Description = "BATTERY NEAR END";
          ps.State = "WAIT TO RECEIVE";
          break;
        case "\"":
          ps.IsOnline = true;
          ps.IsError = false;
          ps.Description = "BATTERY NEAR END & RIBBON NEAR END";
          ps.State = "WAIT TO RECEIVE";
          break;
        case "#":
          ps.IsOnline = true;
          ps.IsError = false;
          ps.Description = "BATTERY NEAR END & BUFFER NEAR FULL";
          ps.State = "WAIT TO RECEIVE";
          break;
        case "$":
          ps.IsOnline = true;
          ps.IsError = false;
          ps.Description = "BATTERY NEAR END & RIBBON NEAR END & BUFFER NEAR FULL";
          ps.State = "WAIT TO RECEIVE";
          break;
        case "%":
          ps.IsOnline = true;
          ps.IsError = false;
          ps.Description = "BATTERY NEAR END";
          ps.State = "PRINTING";
          break;
        case "&":
          ps.IsOnline = true;
          ps.IsError = false;
          ps.Description = "BATTERY NEAR END & RIBBON NEAR END";
          ps.State = "PRINTING";
          break;
        case "'":
          ps.IsOnline = true;
          ps.IsError = false;
          ps.Description = "BATTERY NEAR END & BUFFER NEAR FULL";
          ps.State = "PRINTING";
          break;
        case "(":
          ps.IsOnline = true;
          ps.IsError = false;
          ps.Description = "BATTERY NEAR END & RIBBON NEAR END & BUFFER NEAR FULL";
          ps.State = "PRINTING";
          break;
        case ")":
          ps.IsOnline = true;
          ps.IsError = false;
          ps.Description = "BATTERY NEAR END";
          ps.State = "STANDBY";
          break;
        case "*":
          ps.IsOnline = true;
          ps.IsError = false;
          ps.Description = "BATTERY NEAR END & RIBBON NEAR END";
          ps.State = "STANDBY";
          break;
        case "+":
          ps.IsOnline = true;
          ps.IsError = false;
          ps.Description = "BATTERY NEAR END & BUFFER NEAR FULL";
          ps.State = "STANDBY";
          break;
        case ",":
          ps.IsOnline = true;
          ps.IsError = false;
          ps.Description = "BATTERY NEAR END & RIBBON NEAR END & BUFFER NEAR FULL";
          ps.State = "STANDBY";
          break;
        case "-":
          ps.IsOnline = true;
          ps.IsError = false;
          ps.Description = "BATTERY NEAR END";
          ps.State = "ANALYZING / EDITING";
          break;
        case ".":
          ps.IsOnline = true;
          ps.IsError = false;
          ps.Description = "BATTERY NEAR END & RIBBON NEAR END";
          ps.State = "ANALYZING / EDITING";
          break;
        case "/":
          ps.IsOnline = true;
          ps.IsError = false;
          ps.Description = "BATTERY NEAR END & BUFFER NEAR FULL";
          ps.State = "ANALYZING / EDITING";
          break;
        case "0":
          ps.IsOnline = false;
          ps.IsError = false;
          ps.Description = "NO ERROR";
          ps.State = "OFFLINE";
          break;
        case "1":
          ps.IsOnline = false;
          ps.IsError = false;
          ps.Description = "RIBBON/LABEL NEAR END";
          ps.State = "OFFLINE";
          break;
        case "2":
          ps.IsOnline = false;
          ps.IsError = false;
          ps.Description = "BUFFER NEAR FULL";
          ps.State = "OFFLINE";
          break;
        case "3":
          ps.IsOnline = false;
          ps.IsError = false;
          ps.Description = "RIBBON/LABEL NEAR END & BUFFER NEAR FULL";
          ps.State = "OFFLINE";
          break;
        case "4":
          ps.IsOnline = false;
          ps.IsError = false;
          ps.Description = "BATTERY NEAR END";
          ps.State = "OFFLINE";
          break;
        case "5":
          ps.IsOnline = false;
          ps.IsError = false;
          ps.Description = "BATTERY NEAR END & RIBBON NEAR END";
          ps.State = "OFFLINE";
          break;
        case "6":
          ps.IsOnline = false;
          ps.IsError = false;
          ps.Description = "BATTERY NEAR END & BUFFER NEAR FULL";
          ps.State = "OFFLINE";
          break;
        case "7":
          ps.IsOnline = false;
          ps.IsError = false;
          ps.Description = "BATTERY NEAR END & RIBBON NEAR END & BUFFER NEAR FULL";
          ps.State = "OFFLINE";
          break;
        case "@":
          ps.IsOnline = true;
          ps.IsError = false;
          ps.Description = "BATTERY NEAR END & RIBBON NEAR END & BUFFER NEAR FULL";
          ps.State = "ANALYZING / EDITING";
          break;
        case "A":
          ps.IsOnline = true;
          ps.IsError = false;
          ps.Description = "NO ERROR";
          ps.State = "WAIT TO RECEIVE";
          break;
        case "B":
          ps.IsOnline = true;
          ps.IsError = false;
          ps.Description = "RIBBON/LABEL NEAR END";
          ps.State = "WAIT TO RECEIVE";
          break;
        case "C":
          ps.IsOnline = true;
          ps.IsError = false;
          ps.Description = "BUFFER NEAR FULL";
          ps.State = "WAIT TO RECEIVE";
          break;
        case "D":
          ps.IsOnline = true;
          ps.IsError = false;
          ps.Description = "RIBBON/LABEL NEAR END & BUFFER NEAR FULL";
          ps.State = "WAIT TO RECEIVE";
          break;
        case "E":
          ps.IsOnline = true;
          ps.IsError = false;
          ps.Description = "PRINTER PAUSED (NO ERROR)";
          ps.State = "WAIT TO RECEIVE";
          break;
        case "G":
          ps.IsOnline = true;
          ps.IsError = false;
          ps.Description = "NO ERROR";
          ps.State = "PRINTING";
          break;
        case "H":
          ps.IsOnline = true;
          ps.IsError = false;
          ps.Description = "RIBBON/LABEL NEAR END";
          ps.State = "PRINTING";
          break;
        case "I":
          ps.IsOnline = true;
          ps.IsError = false;
          ps.Description = "BUFFER NEAR FULL";
          ps.State = "PRINTING";
          break;
        case "J":
          ps.IsOnline = true;
          ps.IsError = false;
          ps.Description = "RIBBON/LABEL NEAR END & BUFFER NEAR FULL";
          ps.State = "PRINTING";
          break;
        case "K":
          ps.IsOnline = true;
          ps.IsError = false;
          ps.Description = "PRINTER PAUSED (NO ERROR)";
          ps.State = "PRINTING";
          break;
        case "M":
          ps.IsOnline = true;
          ps.IsError = false;
          ps.Description = "NO ERROR";
          ps.State = "STANDBY";
          break;
        case "N":
          ps.IsOnline = true;
          ps.IsError = false;
          ps.Description = "RIBBON/LABEL NEAR END";
          ps.State = "STANDBY";
          break;
        case "O":
          ps.IsOnline = true;
          ps.IsError = false;
          ps.Description = "BUFFER NEAR FULL";
          ps.State = "STANDBY";
          break;
        case "P":
          ps.IsOnline = true;
          ps.IsError = false;
          ps.Description = "RIBBON/LABEL NEAR END & BUFFER NEAR FULL";
          ps.State = "STANDBY";
          break;
        case "Q":
          ps.IsOnline = true;
          ps.IsError = false;
          ps.Description = "PRINTER PAUSED (NO ERROR)";
          ps.State = "STANDBY";
          break;
        case "S":
          ps.IsOnline = true;
          ps.IsError = false;
          ps.Description = "NO ERROR";
          ps.State = "ANALYZING / EDITING";
          break;
        case "T":
          ps.IsOnline = true;
          ps.IsError = false;
          ps.Description = "RIBBON/LABEL NEAR END";
          ps.State = "ANALYZING / EDITING";
          break;
        case "U":
          ps.IsOnline = true;
          ps.IsError = false;
          ps.Description = "BUFFER NEAR FULL";
          ps.State = "ANALYZING / EDITING";
          break;
        case "V":
          ps.IsOnline = true;
          ps.IsError = false;
          ps.Description = "RIBBON/LABEL NEAR END & BUFFER NEAR FULL";
          ps.State = "ANALYZING / EDITING";
          break;
        case "W":
          ps.IsOnline = true;
          ps.IsError = false;
          ps.Description = "PRINTER PAUSED (NO ERROR)";
          ps.State = "ANALYZING / EDITING";
          break;
        case "a":
          ps.IsOnline = false;
          ps.IsError = true;
          ps.Description = "BUFFER OVERFLOW";
          ps.State = "ERROR";
          break;
        case "b":
          ps.IsOnline = false;
          ps.IsError = true;
          ps.Description = "HEAD OPEN";
          ps.State = "ERROR";
          break;
        case "c":
          ps.IsOnline = false;
          ps.IsError = true;
          ps.Description = "PAPER END";
          ps.State = "ERROR";
          break;
        case "d":
          ps.IsOnline = false;
          ps.IsError = true;
          ps.Description = "RIBBON END";
          ps.State = "ERROR";
          break;
        case "e":
          ps.IsOnline = false;
          ps.IsError = true;
          ps.Description = "MEDIA ERROR";
          ps.State = "ERROR";
          break;
        case "f":
          ps.IsOnline = false;
          ps.IsError = true;
          ps.Description = "SENSOR ERROR";
          ps.State = "ERROR";
          break;
        case "g":
          ps.IsOnline = false;
          ps.IsError = true;
          ps.Description = "HEAD ERROR";
          ps.State = "ERROR";
          break;
        case "h":
          ps.IsOnline = false;
          ps.IsError = true;
          ps.Description = "CUTTER/COVER OPEN ERROR";
          ps.State = "ERROR";
          break;
        case "i":
          ps.IsOnline = false;
          ps.IsError = true;
          ps.Description = "CARD ERROR";
          ps.State = "ERROR";
          break;
        case "j":
          ps.IsOnline = false;
          ps.IsError = true;
          ps.Description = "CUTTER ERROR";
          ps.State = "ERROR";
          break;
        case "k":
          ps.IsOnline = false;
          ps.IsError = true;
          ps.Description = "OTHER ERRORS";
          ps.State = "ERROR";
          break;
        case "o":
          ps.IsOnline = false;
          ps.IsError = true;
          ps.Description = "RFID TAG ERROR";
          ps.State = "ERROR";
          break;
        case "p":
          ps.IsOnline = false;
          ps.IsError = true;
          ps.Description = "RFID PROTECT ERROR";
          ps.State = "ERROR";
          break;
        case "q":
          ps.IsOnline = false;
          ps.IsError = true;
          ps.Description = "BATTERY ERROR";
          ps.State = "ERROR";
          break;
        default:
          ps.IsOnline = false;
          ps.IsError = false;
          ps.Description = (string) null;
          ps.State = (string) null;
          break;
      }
    }

    private void ReadCallBack(byte[] data)
    {
      if (this.queryFlag)
      {
        this.queryData = data;
        Printer.autoReset.Set();
        this.queryFlag = false;
      }
      else
        this.OnByteAvailable((object) this, new Printer.ByteAvailableEventArgs(data));
    }

    public event EventHandler<Printer.ByteAvailableEventArgs> ByteAvailable;

    public virtual void OnByteAvailable(object sender, Printer.ByteAvailableEventArgs e)
    {
      EventHandler<Printer.ByteAvailableEventArgs> byteAvailable = this.ByteAvailable;
      if (byteAvailable == null)
        return;
      byteAvailable(sender, e);
    }

    public enum InterfaceType
    {
      TCPIP,
      USB,
      COM,
      LPT,
    }

    [Serializable]
    public class USBInfo
    {
      public string Name { get; set; }

      public string PortID { get; set; }
    }

    [Serializable]
    public class TCPIPInfo
    {
      public string Name { get; set; }

      public string MacAddress { get; set; }

      public string IPAddress { get; set; }

      public string Details { get; set; }
    }

    [Serializable]
    public class Status
    {
      public bool IsOnline { get; set; }

      public bool IsError { get; set; }

      public string Description { get; set; }

      public string State { get; set; }

      public string Code { get; set; }

      public int Buffer { get; set; }

      public string JobName { get; set; }

      public string JobID { get; set; }

      public string Raw { get; set; }
    }

    public class ByteAvailableEventArgs : EventArgs
    {
      public byte[] Data;

      public ByteAvailableEventArgs(byte[] data) => this.Data = data;
    }
  }
}
