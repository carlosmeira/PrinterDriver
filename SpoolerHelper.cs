// Decompiled with JetBrains decompiler
// Type: SATOPrinterAPI.SpoolerHelper
// Assembly: SATOPrinterAPI, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1CED7137-37FF-4597-9D4E-DAF488B01141
// Assembly location: D:\Downloads\Microsoft Edge\ApiPort.2.8.25.Offline\ApiPort.Offline\netcoreapp3.1\SATOPrinterAPI.dll

using System.ServiceProcess;
using System.Threading;

namespace SATOPrinterAPI
{
  internal class SpoolerHelper
  {
    public static bool Flag;
    public static bool TheAppFlag;

    public static ServiceController GetNamedService(string name)
    {
      foreach (ServiceController service in ServiceController.GetServices())
      {
        if (name == service.ServiceName)
          return service;
      }
      return (ServiceController) null;
    }

    public static bool StopSpooler()
    {
      ServiceController namedService = SpoolerHelper.GetNamedService("Spooler");
      if (namedService == null || namedService.Status != ServiceControllerStatus.Running || !namedService.CanStop)
        return false;
      namedService.Stop();
      while (SpoolerHelper.GetNamedService("Spooler").Status == ServiceControllerStatus.Running)
        Thread.Sleep(100);
      return true;
    }

    public static void StartSpooler()
    {
      ServiceController namedService = SpoolerHelper.GetNamedService("Spooler");
      if (namedService == null || namedService.Status != ServiceControllerStatus.Stopped)
        return;
      namedService.Start();
    }

    public static bool IsSpoolerRunning
    {
      get
      {
        ServiceController namedService = SpoolerHelper.GetNamedService("Spooler");
        return namedService != null && namedService.Status == ServiceControllerStatus.Running;
      }
    }
  }
}
