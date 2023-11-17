// Decompiled with JetBrains decompiler
// Type: SATOPrinterAPI.EmbeddedAssembly
// Assembly: SATOPrinterAPI, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1CED7137-37FF-4597-9D4E-DAF488B01141
// Assembly location: D:\Downloads\Microsoft Edge\ApiPort.2.8.25.Offline\ApiPort.Offline\netcoreapp3.1\SATOPrinterAPI.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;

namespace SATOPrinterAPI
{
  internal class EmbeddedAssembly
  {
    private static Dictionary<string, Assembly> dic;

    public static void Load(string Resouces, string FileName)
    {
      if (EmbeddedAssembly.dic == null)
        EmbeddedAssembly.dic = new Dictionary<string, Assembly>();
      byte[] numArray = (byte[]) null;
      Assembly asm = (Assembly) null;
      using (Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(Resouces))
      {
        numArray = manifestResourceStream != null ? new byte[(int) manifestResourceStream.Length] : throw new Exception(Resouces + " is not found in Embedded Resources.");
        manifestResourceStream.Read(numArray, 0, (int) manifestResourceStream.Length);
        try
        {
          asm = Assembly.Load(numArray);
          if (!EmbeddedAssembly.dic.Where<KeyValuePair<string, Assembly>>((Func<KeyValuePair<string, Assembly>, bool>) (x => x.Key == asm.FullName)).FirstOrDefault<KeyValuePair<string, Assembly>>().Equals((object) new KeyValuePair<string, Assembly>()))
            return;
          EmbeddedAssembly.dic.Add(asm.FullName, asm);
          return;
        }
        catch
        {
        }
      }
      bool flag = false;
      string path = "";
      using (SHA1CryptoServiceProvider cryptoServiceProvider = new SHA1CryptoServiceProvider())
      {
        string str1 = BitConverter.ToString(cryptoServiceProvider.ComputeHash(numArray)).Replace("-", string.Empty);
        if (Environment.Is64BitProcess)
        {
          path = Path.GetTempPath() + "SATOAPI\\x64\\";
          if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        }
        else
        {
          path = Path.GetTempPath() + "SATOAPI\\x86\\";
          if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        }
        path += FileName;
        if (File.Exists(path))
        {
          byte[] buffer = File.ReadAllBytes(path);
          string str2 = BitConverter.ToString(cryptoServiceProvider.ComputeHash(buffer)).Replace("-", string.Empty);
          flag = str1 == str2;
        }
        else
          flag = false;
      }
      if (!flag)
        File.WriteAllBytes(path, numArray);
      asm = Assembly.LoadFile(path);
      if (!EmbeddedAssembly.dic.Where<KeyValuePair<string, Assembly>>((Func<KeyValuePair<string, Assembly>, bool>) (x => x.Key == asm.FullName)).FirstOrDefault<KeyValuePair<string, Assembly>>().Equals((object) new KeyValuePair<string, Assembly>()))
        return;
      EmbeddedAssembly.dic.Add(asm.FullName, asm);
    }

    public static Assembly Get(string assemblyFullName)
    {
      if (EmbeddedAssembly.dic == null || EmbeddedAssembly.dic.Count == 0)
        return (Assembly) null;
      return EmbeddedAssembly.dic.ContainsKey(assemblyFullName) ? EmbeddedAssembly.dic[assemblyFullName] : (Assembly) null;
    }
  }
}
