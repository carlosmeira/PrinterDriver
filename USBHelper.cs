// Decompiled with JetBrains decompiler
// Type: SATOPrinterAPI.USBHelper
// Assembly: SATOPrinterAPI, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1CED7137-37FF-4597-9D4E-DAF488B01141
// Assembly location: D:\Downloads\Microsoft Edge\ApiPort.2.8.25.Offline\ApiPort.Offline\netcoreapp3.1\SATOPrinterAPI.dll

using System;
using System.Collections.Generic;
using System.Management;
using System.Threading;
//using USBConnectLib;

namespace SATOPrinterAPI
{
  internal class USBHelper : InterfaceHelper
  {
    //private USBPort usbport;
    private volatile bool RunThreadFlag;
    private int USBtimeout = 2500;
    private Thread th;

    public USBHelper(bool PermanentConnect)
      : base(PermanentConnect)
    {
    }

        public override void CBReceive(IAsyncResult ar)
        {
            throw new NotImplementedException();
        }

        public override void Close()
        {
            throw new NotImplementedException();
        }

        public override void Open(string name, object data, int timeout)
        {
            throw new NotImplementedException();
        }

        public override byte[] Send(byte[] data, int ReplyCnt)
        {
            throw new NotImplementedException();
        }

        /*public override void Open(string name, object data, int timeout)
        {
          try
          {
            this.USBtimeout = timeout;
            if (this.usbport != null)
              this.Close();
            //this.usbport = new USBPort();
            if (!this.usbport.Open(name))
              throw new Exception("Fail to open USB port : " + name + ".");
            if (!this.isPermanent)
              return;
            this.StartReadThread();
          }
          catch (Exception ex)
          {
            this.Close();
            throw new Exception(ex.Message);
          }
        }

        private void StartReadThread()
        {
          if (this.RunThreadFlag)
          {
            this.RunThreadFlag = false;
            Thread.Sleep(10);
          }
          if (this.th != null)
          {
            if (this.th.IsAlive)
              this.th.Abort();
            this.th = (Thread) null;
          }
          this.RunThreadFlag = true;
          this.th = new Thread(new ThreadStart(this.ReadThread));
          this.th.Start();
        }

        private void ReadThread()
        {
          while (this.RunThreadFlag)
          {
            byte[] data = this.usbport.Read(1048576, this.USBtimeout);
            if (data != null && data.Length > 1)
            {
              if (this.fOnReceiveCallBack != null)
                this.fOnReceiveCallBack(data);
              Thread.Sleep(100);
            }
            else
              Thread.Sleep(100);
          }
        }

        public override void Close()
        {
          if (this.RunThreadFlag)
          {
            this.RunThreadFlag = false;
            Thread.Sleep(100);
          }
          if (this.th != null)
          {
            if (this.th.IsAlive)
              this.th.Abort();
            this.th = (Thread) null;
          }
          if (this.usbport == null)
            return;
          if (this.usbport.IsOpen)
            this.usbport.Close();
          this.usbport = (USBPort) null;
        }

        public override byte[] Send(byte[] data, int ReplyCnt)
        {
          if (this.usbport != null)
          {
            if (this.usbport.IsOpen)
            {
              try
              {
                int length = data.Length;
                int sourceIndex = 0;
                byte[] destinationArray;
                for (; length > 0; length -= destinationArray.Length)
                {
                  destinationArray = new byte[Math.Min(length, 64)];
                  Array.Copy((Array) data, sourceIndex, (Array) destinationArray, 0, destinationArray.Length);
                  this.usbport.Write(destinationArray, this.USBtimeout);
                  sourceIndex += destinationArray.Length;
                }
                if (!this.isPermanent)
                {
                  if (ReplyCnt > 0)
                  {
                    if (0 < ReplyCnt)
                    {
                      Thread.Sleep(100);
                      this.buffer.Initialize();
                      this.buffer = this.usbport.Read(1048576, this.USBtimeout);
                      return this.buffer != null && this.buffer.Length != 0 ? this.buffer : throw new Exception("No Reply received!");
                    }
                  }
                }
              }
              catch (Exception ex)
              {
                this.Close();
                throw new Exception(ex.Message);
              }
              return (byte[]) null;
            }
          }
          throw new Exception("Couldn't establish connection");
        }

        public override void CBReceive(IAsyncResult ar)
        {
        }

        public static List<string> GetUSB2SerialPorts()
        {
          List<string> usB2SerialPorts = new List<string>();
          ManagementObjectSearcher managementObjectSearcher = (ManagementObjectSearcher) null;
          ManagementObjectCollection objectCollection = (ManagementObjectCollection) null;
          try
          {
            managementObjectSearcher = new ManagementObjectSearcher("root\\WMI", "select * from MSSerial_PortName");
            objectCollection = managementObjectSearcher.Get();
            foreach (ManagementObject managementObject in objectCollection)
            {
              if (((string) managementObject["InstanceName"]).Contains("USB"))
                usB2SerialPorts.Add((string) managementObject["PortName"]);
            }
          }
          catch
          {
            return (List<string>) null;
          }
          finally
          {
            objectCollection?.Dispose();
            managementObjectSearcher?.Dispose();
          }
          return usB2SerialPorts;
        }*/
    }
}
