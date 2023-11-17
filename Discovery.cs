// Decompiled with JetBrains decompiler
// Type: SATOPrinterAPI.Discovery
// Assembly: SATOPrinterAPI, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1CED7137-37FF-4597-9D4E-DAF488B01141
// Assembly location: D:\Downloads\Microsoft Edge\ApiPort.2.8.25.Offline\ApiPort.Offline\netcoreapp3.1\SATOPrinterAPI.dll

using System;
using System.Collections.Generic;
using System.Threading;

namespace SATOPrinterAPI
{
  internal class Discovery
  {
    private Del_VoidPrinters fOnEnd;
    private int ThreadCounter;
    private List<IPConfig> PList;
    private object locker = new object();
    private object plistLocker = new object();

    public Discovery(Del_VoidPrinters func)
    {
      this.fOnEnd = func;
      this.PList = new List<IPConfig>();
    }

    public void Start(int timespan)
    {
      Thread thread1 = new Thread(new ParameterizedThreadStart(this.SilexFinder));
      Thread thread2 = new Thread(new ParameterizedThreadStart(this.SatoFinder));
      Thread thread3 = new Thread(new ParameterizedThreadStart(this.GLFinder));
      this.ThreadCounter = 0;
      this.PList.Clear();
      // ISSUE: variable of a boxed type
      int parameter1 = timespan;
      thread2.Start((object) parameter1);
      Thread.Sleep(50);
      // ISSUE: variable of a boxed type
      int parameter2 = timespan;
      thread1.Start((object) parameter2);
      Thread.Sleep(50);
      thread3.Start((object) timespan);
    }

    private void EndCheck()
    {
      lock (this.locker)
      {
        ++this.ThreadCounter;
        if (this.ThreadCounter != 3 || this.fOnEnd == null)
          return;
        this.fOnEnd(this.PList);
      }
    }

    private void SilexFinder(object data)
    {
      SilexLanFinder.Discover((int) data);
      List<IPConfig> collection = new List<IPConfig>();
      for (int index = 0; index < SilexLanFinder.Results.Count; ++index)
      {
        IPConfig result = SilexLanFinder.Results[index];
        result.Status = UDPNetCfg.UDPStatusCheck(result.IP, 19541);
        collection.Add(result);
      }
      lock (this.plistLocker)
        this.PList.AddRange((IEnumerable<IPConfig>) collection);
      this.EndCheck();
    }

    private void SatoFinder(object data)
    {
      LanFinder.Discover((int) data);
      List<IPConfig> collection = new List<IPConfig>();
      for (int index = 0; index < LanFinder.Results.Count; ++index)
      {
        IPConfig result = LanFinder.Results[index];
        collection.Add(result);
      }
      lock (this.plistLocker)
        this.PList.AddRange((IEnumerable<IPConfig>) collection);
      this.EndCheck();
    }

    private void GLFinder(object data)
    {
      GLLanFinder.Discover((int) data);
      List<IPConfig> collection = new List<IPConfig>();
      for (int index = 0; index < GLLanFinder.Results.Count; ++index)
      {
        IPConfig result = GLLanFinder.Results[index];
        collection.Add(result);
      }
      lock (this.plistLocker)
        this.PList.AddRange((IEnumerable<IPConfig>) collection);
      this.EndCheck();
    }
  }
}
