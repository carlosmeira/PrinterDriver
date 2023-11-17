// Decompiled with JetBrains decompiler
// Type: SATOPrinterAPI.SocketServer
// Assembly: SATOPrinterAPI, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1CED7137-37FF-4597-9D4E-DAF488B01141
// Assembly location: D:\Downloads\Microsoft Edge\ApiPort.2.8.25.Offline\ApiPort.Offline\netcoreapp3.1\SATOPrinterAPI.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
//using WebSocketSharp;
//using WebSocketSharp.Server;
//using ErrorEventArgs = WebSocketSharp.ErrorEventArgs;

namespace SATOPrinterAPI
{
  public class SocketServer
  {
    /*public X509Certificate2 Certificate { get; set; }

    private static WebSockeServer wServer { get; set; }

    private static int serverPort { get; set; }

    private static string encoding { get; set; }

    private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args) => EmbeddedAssembly.Get(args.Name);

    public SocketServer(int Port = 8055)
    {
      EmbeddedAssembly.Load("SATOPrinterAPI.Resources.AnyCPU.Newtonsoft.Json.dll", "Newtonsoft.Json.dll");
      EmbeddedAssembly.Load("SATOPrinterAPI.Resources.AnyCPU.websocket-sharp.dll", "websocket-sharp.dll");
      AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(SocketServer.CurrentDomain_AssemblyResolve);
      AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(SocketServer.CurrentDomain_AssemblyResolve);
      SocketServer.serverPort = Port;
    }

    public void Start()
    {
      if (SocketServer.wServer != null)
        this.Stop();
      SocketServer.wServer = new WebSocketServer(SocketServer.serverPort, this.Certificate != null);
      if (this.Certificate != null)
        SocketServer.wServer.SslConfiguration.ServerCertificate = this.Certificate;
      SocketServer.wServer.AddWebSocketService<SocketServer.APISocket>("/satoprinterapi");
      SocketServer.wServer.AddWebSocketService<SocketServer.APISocket>("/SATOPrinterAPI");
      SocketServer.wServer.Start();
    }

    public void Stop()
    {
      if (SocketServer.wServer == null)
        return;
      SocketServer.wServer.Stop();
      SocketServer.wServer = (WebSocketServer) null;
    }

    private class APISocket : WebSocketBehavior
    {
      protected virtual void OnMessage(MessageEventArgs e)
      {
        if (e.IsText)
          this.ProcessMessage(Utils.StringToByteArray(e.Data, SocketServer.encoding), true);
        else
          this.ProcessMessage(e.RawData, false);
      }

      protected virtual void OnClose(CloseEventArgs e)
      {
      }

      protected virtual void OnOpen()
      {
        string str = (string) null;
        if (this.Context.QueryString != null && this.Context.QueryString.Count > 0)
        {
          SocketServer.encoding = this.Context.QueryString["encoding"];
          if (this.Context.QueryString.Count != 1 || SocketServer.encoding == null)
            str = "Do not define any parameters that SocketServer not implemented/supported, connection aborted.";
        }
        if (string.IsNullOrEmpty(str))
        {
          this.Send("Socket server connected");
        }
        else
        {
          this.Send(JsonConvert.SerializeObject((object) new Dictionary<string, string>()
          {
            {
              "Error",
              str
            }
          }));
          this.Context.WebSocket.Close();
        }
      }

      protected virtual void OnError(ErrorEventArgs e)
      {
        if (SocketServer.wServer == null)
          return;
        this.Send(JsonConvert.SerializeObject((object) new Dictionary<string, string>()
        {
          {
            "Socket Error",
            e.Message
          }
        }));
      }

      private void ProcessMessage(byte[] JBytes, bool isText)
      {
        try
        {
          JObject jObj = (JObject) JsonConvert.DeserializeObject(Utils.ByteArrayToString(JBytes, SocketServer.encoding));
          JToken jtoken1 = jObj.GetValue("method", StringComparison.OrdinalIgnoreCase);
          switch (jtoken1 != null ? jtoken1.Value<string>().ToLower() : (string) null)
          {
            case null:
              this.Send(JsonConvert.SerializeObject((object) new Dictionary<string, string>()
              {
                {
                  "Error",
                  "Please ensure 'Method' defined in JSON data"
                }
              }));
              break;
            case "driver.clearspoolerprintjobs":
              Driver driver1 = new Driver();
              if (this.DriverMethod(jObj, "clearspoolerprintjobs", isText, ref driver1) != null)
              {
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                dictionary.Add("Result", "Executed");
                if (isText)
                  this.Send(JsonConvert.SerializeObject((object) dictionary));
                else
                  this.Send(new byte[1]{ (byte) 6 });
              }
              driver1 = (Driver) null;
              break;
            case "driver.getdriverinfo":
              Driver driver2 = new Driver();
              byte[] Data1 = this.DriverMethod(jObj, "getdriverinfo", isText, ref driver2);
              if (Data1 != null)
              {
                if (isText)
                  this.Send(Utils.ByteArrayToString(Data1, SocketServer.encoding));
                else
                  this.Send(Data1);
              }
              driver2 = (Driver) null;
              break;
            case "driver.getdriverlist":
              Driver driver3 = new Driver();
              if (isText)
                this.Send(JsonConvert.SerializeObject((object) driver3.GetDriverList()));
              else
                this.Send(SocketServer.APISocket.SerializeToBytes<List<Driver.Info>>(driver3.GetDriverList()));
              break;
            case "driver.getportinfobydrivername":
              Driver driver4 = new Driver();
              byte[] Data2 = this.DriverMethod(jObj, "getportinfobydrivername", isText, ref driver4);
              if (Data2 != null)
              {
                if (isText)
                  this.Send(Utils.ByteArrayToString(Data2, SocketServer.encoding));
                else
                  this.Send(Data2);
              }
              driver4 = (Driver) null;
              break;
            case "driver.getportinfobyname":
              Driver driver5 = new Driver();
              byte[] Data3 = this.DriverMethod(jObj, "getportinfobyname", isText, ref driver5);
              if (Data3 != null)
              {
                if (isText)
                  this.Send(Utils.ByteArrayToString(Data3, SocketServer.encoding));
                else
                  this.Send(Data3);
              }
              driver5 = (Driver) null;
              break;
            case "driver.getportnames":
              Driver driver6 = new Driver();
              if (isText)
                this.Send(JsonConvert.SerializeObject((object) driver6.GetPortNames()));
              else
                this.Send(SocketServer.APISocket.SerializeToBytes<List<string>>(driver6.GetPortNames()));
              break;
            case "driver.getspoolerprintjobsnumber":
              Driver driver7 = new Driver();
              byte[] Data4 = this.DriverMethod(jObj, "getspoolerprintjobsnumber", isText, ref driver7);
              if (Data4 != null)
              {
                if (isText)
                  this.Send(Utils.ByteArrayToString(Data4, SocketServer.encoding));
                else
                  this.Send(Data4);
              }
              driver7 = (Driver) null;
              break;
            case "driver.getversion":
              Driver driver8 = new Driver();
              byte[] Data5 = this.DriverMethod(jObj, "getversion", isText, ref driver8);
              if (Data5 != null)
              {
                if (isText)
                  this.Send(Utils.ByteArrayToString(Data5, SocketServer.encoding));
                else
                  this.Send(Data5);
              }
              driver8 = (Driver) null;
              break;
            case "driver.sendrawdata":
              Driver driver9 = new Driver();
              if (this.DriverMethod(jObj, "sendrawdata", isText, ref driver9) != null)
              {
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                dictionary.Add("Result", "Executed");
                if (isText)
                  this.Send(JsonConvert.SerializeObject((object) dictionary));
                else
                  this.Send(new byte[1]{ (byte) 6 });
              }
              driver9 = (Driver) null;
              break;
            case "driver.setdriverinfo":
              Driver driver10 = new Driver();
              if (this.DriverMethod(jObj, "setdriverinfo", isText, ref driver10) != null)
              {
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                dictionary.Add("Result", "Executed");
                if (isText)
                  this.Send(JsonConvert.SerializeObject((object) dictionary));
                else
                  this.Send(new byte[1]{ (byte) 6 });
              }
              driver10 = (Driver) null;
              break;
            case "printer.clearbuffer":
              Printer printer1 = new Printer();
              this.SetPrinterTimeout(jObj, ref printer1);
              if (this.SetInterfaceAndPort(jObj, ref printer1))
              {
                printer1.ClearBuffer();
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                dictionary.Add("Result", "Executed");
                if (isText)
                  this.Send(JsonConvert.SerializeObject((object) dictionary));
                else
                  this.Send(new byte[1]{ (byte) 6 });
              }
              break;
            case "printer.getcomlist":
              Printer p1 = new Printer();
              this.SetPrinterTimeout(jObj, ref p1);
              if (isText)
                this.Send(JsonConvert.SerializeObject((object) p1.GetCOMList()));
              else
                this.Send(SocketServer.APISocket.SerializeToBytes<List<string>>(p1.GetCOMList()));
              break;
            case "printer.getlptlist":
              Printer p2 = new Printer();
              this.SetPrinterTimeout(jObj, ref p2);
              if (isText)
                this.Send(JsonConvert.SerializeObject((object) p2.GetLPTList()));
              else
                this.Send(SocketServer.APISocket.SerializeToBytes<List<string>>(p2.GetLPTList()));
              break;
            case "printer.getprinterstatus":
              Printer printer2 = new Printer();
              this.SetPrinterTimeout(jObj, ref printer2);
              if (this.SetInterfaceAndPort(jObj, ref printer2))
              {
                if (isText)
                  this.Send(JsonConvert.SerializeObject((object) printer2.GetPrinterStatus()));
                else
                  this.Send(SocketServer.APISocket.SerializeToBytes<Printer.Status>(printer2.GetPrinterStatus()));
              }
              break;
            case "printer.gettcpiplist":
              Printer p3 = new Printer();
              this.SetPrinterTimeout(jObj, ref p3);
              if (isText)
                this.Send(JsonConvert.SerializeObject((object) p3.GetTCPIPList()));
              else
                this.Send(SocketServer.APISocket.SerializeToBytes<List<Printer.TCPIPInfo>>(p3.GetTCPIPList()));
              break;
            /*case "printer.getusblist":
              Printer p4 = new Printer();
              this.SetPrinterTimeout(jObj, ref p4);
              if (isText)
                this.Send(JsonConvert.SerializeObject((object) p4.GetUSBList()));
              else
                this.Send(SocketServer.APISocket.SerializeToBytes<List<Printer.USBInfo>>(p4.GetUSBList()));
              break;*/
            /*case "printer.query":
              Printer printer3 = new Printer();
              this.SetPrinterTimeout(jObj, ref printer3);
              byte[] numArray = this.DataSendQuery(jObj, "Query", isText, ref printer3);
              if (numArray != null)
              {
                if (isText)
                  this.Send(Convert.ToBase64String(numArray));
                else
                  this.Send(SocketServer.APISocket.SerializeToBytes<string>(Utils.ByteArrayToString(numArray, SocketServer.encoding)));
              }
              break;
            case "printer.reprint":
              Printer printer4 = new Printer();
              this.SetPrinterTimeout(jObj, ref printer4);
              if (this.SetInterfaceAndPort(jObj, ref printer4))
              {
                printer4.Reprint();
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                dictionary.Add("Result", "Executed");
                if (isText)
                  this.Send(JsonConvert.SerializeObject((object) dictionary));
                else
                  this.Send(new byte[1]{ (byte) 6 });
              }
              break;
            case "printer.send":
              Printer printer5 = new Printer();
              this.SetPrinterTimeout(jObj, ref printer5);
              if (this.DataSendQuery(jObj, "Send", isText, ref printer5) != null)
              {
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                dictionary.Add("Result", "Executed");
                if (isText)
                  this.Send(JsonConvert.SerializeObject((object) dictionary));
                else
                  this.Send(new byte[1]{ (byte) 6 });
              }
              break;
            case "printer.testprint":
              Printer printer6 = new Printer();
              this.SetPrinterTimeout(jObj, ref printer6);
              if (this.SetInterfaceAndPort(jObj, ref printer6))
              {
                printer6.TestPrint();
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                dictionary.Add("Result", "Executed");
                if (isText)
                  this.Send(JsonConvert.SerializeObject((object) dictionary));
                else
                  this.Send(new byte[1]{ (byte) 6 });
              }
              break;
            case "utils.commanddatareplace":
              JObject jobject1 = jObj.GetValue("parameters", StringComparison.OrdinalIgnoreCase)?.ToObject<JObject>();
              if (jobject1 == null)
              {
                this.Send(JsonConvert.SerializeObject((object) new Dictionary<string, string>()
                {
                  {
                    "Error",
                    "Please ensure 'Parameters' defined in JSON data and must be match with API method parameters"
                  }
                }));
                break;
              }
              JToken jtoken2 = jobject1.GetValue("commandfilepath", StringComparison.OrdinalIgnoreCase);
              string uriString1 = jtoken2 != null ? jtoken2.Value<string>() : (string) null;
              JObject jobject2 = jobject1.GetValue("variablesvalue", StringComparison.OrdinalIgnoreCase)?.ToObject<JObject>();
              if (uriString1 != null && jobject2 != null)
              {
                string str = Utils.CommandDataReplace(new Uri(uriString1), jobject2.ToObject<Dictionary<string, string>>(), SocketServer.encoding);
                if (str != null)
                {
                  if (isText)
                  {
                    this.Send(Convert.ToBase64String(Utils.StringToByteArray(str, SocketServer.encoding)));
                    break;
                  }
                  this.Send(SocketServer.APISocket.SerializeToBytes<string>(str));
                  break;
                }
                this.Send(JsonConvert.SerializeObject((object) new Dictionary<string, string>()
                {
                  {
                    "Error",
                    "Unable to read graphic file"
                  }
                }));
                break;
              }
              this.Send(JsonConvert.SerializeObject((object) new Dictionary<string, string>()
              {
                {
                  "Error",
                  "Parameters 'CommandFilePath' and/or 'VariablesValue' is not specified"
                }
              }));
              break;
            /*case "utils.convertgraphictosbpl":
              JObject jobject3 = jObj.GetValue("parameters", StringComparison.OrdinalIgnoreCase)?.ToObject<JObject>();
              if (jobject3 == null)
              {
                this.Send(JsonConvert.SerializeObject((object) new Dictionary<string, string>()
                {
                  {
                    "Error",
                    "Please ensure 'Parameters' defined in JSON data and must be match with API method parameters"
                  }
                }));
                break;
              }
              JToken jtoken3 = jobject3.GetValue("graphicfilepath", StringComparison.OrdinalIgnoreCase);
              string uriString2 = jtoken3 != null ? jtoken3.Value<string>() : (string) null;
              if (uriString2 != null)
              {
                string sbpl = Utils.ConvertGraphicToSBPL(new Uri(uriString2));
                if (sbpl != null)
                {
                  if (isText)
                  {
                    this.Send(Convert.ToBase64String(Utils.StringToByteArray(sbpl, SocketServer.encoding)));
                    break;
                  }
                  this.Send(SocketServer.APISocket.SerializeToBytes<string>(sbpl));
                  break;
                }
                this.Send(JsonConvert.SerializeObject((object) new Dictionary<string, string>()
                {
                  {
                    "Error",
                    "Unable to read graphic file"
                  }
                }));
                break;
              }
              this.Send(JsonConvert.SerializeObject((object) new Dictionary<string, string>()
              {
                {
                  "Error",
                  "Parameter 'GraphicFilePath' is not specified"
                }
              }));
              break;*/
            /*default:
              this.Send(JsonConvert.SerializeObject((object) new Dictionary<string, string>()
              {
                {
                  "Error",
                  "Method is not implemented"
                }
              }));
              break;
          }
        }
        catch (Exception ex)
        {
          this.Send(JsonConvert.SerializeObject((object) new Dictionary<string, string>()
          {
            {
              "Error",
              ex.Message
            }
          }));
        }
      }

      private Printer.InterfaceType? GetInterface(string interfaceType)
      {
        switch (interfaceType.ToUpper())
        {
          case "TCPIP":
            return new Printer.InterfaceType?(Printer.InterfaceType.TCPIP);
          case "COM":
            return new Printer.InterfaceType?(Printer.InterfaceType.COM);
          case "USB":
            return new Printer.InterfaceType?(Printer.InterfaceType.USB);
          case "LPT":
            return new Printer.InterfaceType?(Printer.InterfaceType.LPT);
          default:
            return new Printer.InterfaceType?();
        }
      }

      private bool SetInterfaceAndPort(JObject jObj, ref Printer printer)
      {
        Printer printer1 = printer;
        JToken jtoken1 = jObj.GetValue("interface", StringComparison.OrdinalIgnoreCase);
        Printer.InterfaceType? nullable1 = this.GetInterface(jtoken1 != null ? jtoken1.Value<string>() : (string) null);
        printer1.Interface = nullable1;
        Printer.InterfaceType? nullable2 = printer.Interface;
        if (!nullable2.HasValue)
        {
          this.Send(JsonConvert.SerializeObject((object) new Dictionary<string, string>()
          {
            {
              "Error",
              "Interface not found, please define supported printer interface"
            }
          }));
          return false;
        }
        nullable2 = printer.Interface;
        if (nullable2.HasValue)
        {
          switch (nullable2.GetValueOrDefault())
          {
            case Printer.InterfaceType.TCPIP:
              Printer printer2 = printer;
              JToken jtoken2 = jObj.GetValue("tcpipaddress", StringComparison.OrdinalIgnoreCase);
              string str1 = jtoken2 != null ? jtoken2.Value<string>() : (string) null;
              printer2.TCPIPAddress = str1;
              Printer printer3 = printer;
              JToken jtoken3 = jObj.GetValue("tcpipport", StringComparison.OrdinalIgnoreCase);
              string str2 = jtoken3 != null ? jtoken3.Value<string>() : (string) null;
              printer3.TCPIPPort = str2;
              break;
            case Printer.InterfaceType.USB:
              Printer printer4 = printer;
              JToken jtoken4 = jObj.GetValue("usbportid", StringComparison.OrdinalIgnoreCase);
              string str3 = jtoken4 != null ? jtoken4.Value<string>() : (string) null;
              printer4.USBPortID = str3;
              break;
            case Printer.InterfaceType.COM:
              Printer printer5 = printer;
              JToken jtoken5 = jObj.GetValue("comport", StringComparison.OrdinalIgnoreCase);
              string str4 = jtoken5 != null ? jtoken5.Value<string>() : (string) null;
              printer5.COMPort = str4;
              JObject jobject = jObj.GetValue("comsetting", StringComparison.OrdinalIgnoreCase)?.ToObject<JObject>();
              if (jobject != null)
              {
                JToken jtoken6 = jobject.GetValue("baudrate", StringComparison.OrdinalIgnoreCase);
                string s = jtoken6 != null ? jtoken6.Value<string>() : (string) null;
                JToken jtoken7 = jobject.GetValue("flowcontrol", StringComparison.OrdinalIgnoreCase);
                string str5 = jtoken7 != null ? jtoken7.Value<string>() : (string) null;
                JToken jtoken8 = jobject.GetValue("parameters", StringComparison.OrdinalIgnoreCase);
                string str6 = jtoken8 != null ? jtoken8.Value<string>() : (string) null;
                if (s != null)
                  printer.COMSetting.Baudrate = int.Parse(s);
                if (str5 != null)
                  printer.COMSetting.FlowControl = str5;
                if (str6 != null)
                {
                  printer.COMSetting.Parameters = str6;
                  break;
                }
                break;
              }
              break;
            case Printer.InterfaceType.LPT:
              Printer printer6 = printer;
              JToken jtoken9 = jObj.GetValue("lptport", StringComparison.OrdinalIgnoreCase);
              string str7 = jtoken9 != null ? jtoken9.Value<string>() : (string) null;
              printer6.LPTPort = str7;
              break;
          }
        }
        return true;
      }

      private byte[] DataSendQuery(JObject jObj, string method, bool isText, ref Printer printer)
      {
        int num = this.SetInterfaceAndPort(jObj, ref printer) ? 1 : 0;
        byte[] numArray = (byte[]) null;
        if (num != 0)
        {
          JObject jobject = jObj.GetValue("parameters", StringComparison.OrdinalIgnoreCase)?.ToObject<JObject>();
          if (jobject == null)
          {
            this.Send(JsonConvert.SerializeObject((object) new Dictionary<string, string>()
            {
              {
                "Error",
                "Please ensure 'Parameters' defined in JSON data and must be match with API method parameters"
              }
            }));
          }
          else
          {
            byte[] output = (byte[]) null;
            JToken jtoken = jobject.GetValue("data", StringComparison.OrdinalIgnoreCase);
            string str = jtoken != null ? jtoken.Value<string>() : (string) null;
            if (!this.IsBase64(str, out output))
              output = Utils.StringToByteArray(str, SocketServer.encoding);
            if (output == null)
              this.Send(JsonConvert.SerializeObject((object) new Dictionary<string, string>()
              {
                {
                  "Error",
                  "Parameter 'Data' is not defined"
                }
              }));
            else if (method.ToLower() == "send")
            {
              printer.Send(output);
              numArray = new byte[1]{ (byte) 6 };
            }
            else if (method.ToLower() == "query")
              numArray = printer.Query(output);
          }
        }
        return numArray;
      }

      private void SetPrinterTimeout(JObject jObj, ref Printer p)
      {
        JToken jtoken = jObj.GetValue("timeout", StringComparison.OrdinalIgnoreCase);
        string s = jtoken != null ? jtoken.Value<string>() : (string) null;
        int result;
        if (s == null || !int.TryParse(s, out result))
          return;
        p.Timeout = result;
      }

      private byte[] DriverMethod(JObject jObj, string method, bool isText, ref Driver driver)
      {
        byte[] numArray = (byte[]) null;
        JObject jobject1 = jObj.GetValue("parameters", StringComparison.OrdinalIgnoreCase)?.ToObject<JObject>();
        if (jobject1 == null)
        {
          this.Send(JsonConvert.SerializeObject((object) new Dictionary<string, string>()
          {
            {
              "Error",
              "Please ensure 'Parameters' defined in JSON data and must be match with API method parameters"
            }
          }));
        }
        else
        {
          switch (method)
          {
            case "clearspoolerprintjobs":
              JToken jtoken1 = jobject1.GetValue("drivername", StringComparison.OrdinalIgnoreCase);
              string DriverName1 = jtoken1 != null ? jtoken1.Value<string>() : (string) null;
              if (DriverName1 != null)
              {
                driver.ClearSpoolerPrintJobs(DriverName1);
                numArray = new byte[1]{ (byte) 6 };
                break;
              }
              this.Send(JsonConvert.SerializeObject((object) new Dictionary<string, string>()
              {
                {
                  "Error",
                  "Parameter 'DriverName' is not specified"
                }
              }));
              break;
            case "getdriverinfo":
              JToken jtoken2 = jobject1.GetValue("drivername", StringComparison.OrdinalIgnoreCase);
              string DriverName2 = jtoken2 != null ? jtoken2.Value<string>() : (string) null;
              if (DriverName2 != null)
              {
                Driver.Info driverInfo = driver.GetDriverInfo(DriverName2);
                if (driverInfo != null)
                {
                  numArray = !isText ? SocketServer.APISocket.SerializeToBytes<Driver.Info>(driverInfo) : Utils.StringToByteArray(JsonConvert.SerializeObject((object) driverInfo), SocketServer.encoding);
                  break;
                }
                this.Send(JsonConvert.SerializeObject((object) new Dictionary<string, string>()
                {
                  {
                    "Error",
                    "Driver not found"
                  }
                }));
                break;
              }
              this.Send(JsonConvert.SerializeObject((object) new Dictionary<string, string>()
              {
                {
                  "Error",
                  "Parameter 'DriverName' is not specified"
                }
              }));
              break;
            case "getportinfobydrivername":
              JToken jtoken3 = jobject1.GetValue("drivername", StringComparison.OrdinalIgnoreCase);
              string DriverName3 = jtoken3 != null ? jtoken3.Value<string>() : (string) null;
              if (DriverName3 != null)
              {
                Driver.PortInfo infoByDriverName = driver.GetPortInfoByDriverName(DriverName3);
                if (infoByDriverName != null)
                {
                  numArray = !isText ? SocketServer.APISocket.SerializeToBytes<Driver.PortInfo>(infoByDriverName) : Utils.StringToByteArray(JsonConvert.SerializeObject((object) infoByDriverName), SocketServer.encoding);
                  break;
                }
                this.Send(JsonConvert.SerializeObject((object) new Dictionary<string, string>()
                {
                  {
                    "Error",
                    "Driver not found"
                  }
                }));
                break;
              }
              this.Send(JsonConvert.SerializeObject((object) new Dictionary<string, string>()
              {
                {
                  "Error",
                  "Parameter 'DriverName' is not specified"
                }
              }));
              break;
            case "getportinfobyname":
              JToken jtoken4 = jobject1.GetValue("portname", StringComparison.OrdinalIgnoreCase);
              string PortName = jtoken4 != null ? jtoken4.Value<string>() : (string) null;
              if (PortName != null)
              {
                Driver.PortInfo portInfoByName = driver.GetPortInfoByName(PortName);
                if (portInfoByName != null)
                {
                  numArray = !isText ? SocketServer.APISocket.SerializeToBytes<Driver.PortInfo>(portInfoByName) : Utils.StringToByteArray(JsonConvert.SerializeObject((object) portInfoByName), SocketServer.encoding);
                  break;
                }
                this.Send(JsonConvert.SerializeObject((object) new Dictionary<string, string>()
                {
                  {
                    "Error",
                    "Port not found"
                  }
                }));
                break;
              }
              this.Send(JsonConvert.SerializeObject((object) new Dictionary<string, string>()
              {
                {
                  "Error",
                  "Parameter 'DriverName' is not specified"
                }
              }));
              break;
            case "getspoolerprintjobsnumber":
              JToken jtoken5 = jobject1.GetValue("drivername", StringComparison.OrdinalIgnoreCase);
              string DriverName4 = jtoken5 != null ? jtoken5.Value<string>() : (string) null;
              if (DriverName4 != null)
              {
                numArray = !isText ? SocketServer.APISocket.SerializeToBytes<int>(driver.GetSpoolerPrintJobsNumber(DriverName4)) : Utils.StringToByteArray(JsonConvert.SerializeObject((object) driver.GetSpoolerPrintJobsNumber(DriverName4)), SocketServer.encoding);
                break;
              }
              this.Send(JsonConvert.SerializeObject((object) new Dictionary<string, string>()
              {
                {
                  "Error",
                  "Parameter 'DriverName' is not specified"
                }
              }));
              break;
            case "getversion":
              JToken jtoken6 = jobject1.GetValue("drivername", StringComparison.OrdinalIgnoreCase);
              string DriverName5 = jtoken6 != null ? jtoken6.Value<string>() : (string) null;
              if (DriverName5 != null)
              {
                string version = driver.GetVersion(DriverName5);
                if (version != null)
                {
                  numArray = !isText ? SocketServer.APISocket.SerializeToBytes<string>(version) : Utils.StringToByteArray(version, SocketServer.encoding);
                  break;
                }
                this.Send(JsonConvert.SerializeObject((object) new Dictionary<string, string>()
                {
                  {
                    "Error",
                    "Driver not found"
                  }
                }));
                break;
              }
              this.Send(JsonConvert.SerializeObject((object) new Dictionary<string, string>()
              {
                {
                  "Error",
                  "Parameter 'DriverName' is not specified"
                }
              }));
              break;
            case "sendrawdata":
              JToken jtoken7 = jobject1.GetValue("drivername", StringComparison.OrdinalIgnoreCase);
              string DriverName6 = jtoken7 != null ? jtoken7.Value<string>() : (string) null;
              byte[] output = (byte[]) null;
              JToken jtoken8 = jobject1.GetValue("data", StringComparison.OrdinalIgnoreCase);
              string str1 = jtoken8 != null ? jtoken8.Value<string>() : (string) null;
              if (!this.IsBase64(str1, out output))
                output = Utils.StringToByteArray(str1, SocketServer.encoding);
              if (DriverName6 != null && output != null)
              {
                if (driver.SendRawData(DriverName6, output))
                {
                  numArray = new byte[1]{ (byte) 6 };
                  break;
                }
                this.Send(JsonConvert.SerializeObject((object) new Dictionary<string, string>()
                {
                  {
                    "Error",
                    "Driver send data error, please ensure the driver name existed in system"
                  }
                }));
                break;
              }
              this.Send(JsonConvert.SerializeObject((object) new Dictionary<string, string>()
              {
                {
                  "Error",
                  "Parameter 'DriverName' and/or 'Data' is not specified"
                }
              }));
              break;
            case "setdriverinfo":
              JToken jtoken9 = jobject1.GetValue("drivername", StringComparison.OrdinalIgnoreCase);
              string DriverName7 = jtoken9 != null ? jtoken9.Value<string>() : (string) null;
              JObject jobject2 = jobject1.GetValue("driver.info", StringComparison.OrdinalIgnoreCase)?.ToObject<JObject>();
              if (DriverName7 != null && jobject2 != null)
              {
                JToken jtoken10 = jobject2.GetValue("portname", StringComparison.OrdinalIgnoreCase);
                string str2 = jtoken10 != null ? jtoken10.Value<string>() : (string) null;
                JToken jtoken11 = jobject2.GetValue("online", StringComparison.OrdinalIgnoreCase);
                string str3 = jtoken11 != null ? jtoken11.Value<string>() : (string) null;
                JToken jtoken12 = jobject2.GetValue("default", StringComparison.OrdinalIgnoreCase);
                string str4 = jtoken12 != null ? jtoken12.Value<string>() : (string) null;
                JToken jtoken13 = jobject2.GetValue("bidirectional", StringComparison.OrdinalIgnoreCase);
                string str5 = jtoken13 != null ? jtoken13.Value<string>() : (string) null;
                Driver.Info driverInfo = driver.GetDriverInfo(DriverName7);
                if (str2 != null)
                  driverInfo.PortName = str2;
                if (str3 != null)
                  driverInfo.Online = bool.Parse(str3);
                if (str4 != null)
                  driverInfo.Default = bool.Parse(str4);
                if (str5 != null)
                  driverInfo.Bidirectional = bool.Parse(str5);
                driver.SetDriverInfo(driverInfo);
                numArray = new byte[1]{ (byte) 6 };
                break;
              }
              this.Send(JsonConvert.SerializeObject((object) new Dictionary<string, string>()
              {
                {
                  "Error",
                  "Parameter 'DriverName' and/or 'Driver.Info' is not specified"
                }
              }));
              break;
          }
        }
        return numArray;
      }

      private bool IsBase64(string base64String, out byte[] output)
      {
        output = (byte[]) null;
        if (string.IsNullOrWhiteSpace(base64String) || base64String.ToLower().Contains("\\u"))
          return false;
        base64String = base64String.Trim();
        if (base64String.Length % 4 == 0)
        {
          if (Regex.IsMatch(base64String, "^[a-zA-Z0-9\\+/]*={0,3}$", RegexOptions.None))
          {
            try
            {
              output = Convert.FromBase64String(base64String);
              return true;
            }
            catch (FormatException ex)
            {
            }
          }
        }
        return false;
      }

      private static byte[] SerializeToBytes<T>(T source)
      {
        using (MemoryStream serializationStream = new MemoryStream())
        {
          new BinaryFormatter().Serialize((Stream) serializationStream, (object) source);
          return serializationStream.ToArray();
        }
      }

      private static T DeserializeFromBytes<T>(byte[] source)
      {
        using (MemoryStream serializationStream = new MemoryStream(source))
          return (T) new BinaryFormatter().Deserialize((Stream) serializationStream);
      }
    }*/
  }
}
