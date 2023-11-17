// Decompiled with JetBrains decompiler
// Type: SATOPrinterAPI.Utils
// Assembly: SATOPrinterAPI, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1CED7137-37FF-4597-9D4E-DAF488B01141
// Assembly location: D:\Downloads\Microsoft Edge\ApiPort.2.8.25.Offline\ApiPort.Offline\netcoreapp3.1\SATOPrinterAPI.dll

//using Sato.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;

namespace SATOPrinterAPI
{
  public class Utils
  {
    private static bool assemblyDefined;

    private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args) => EmbeddedAssembly.Get(args.Name);

    private static void DefineAssembly()
    {
      EmbeddedAssembly.Load("SATOPrinterAPI.Resources.AnyCPU.SatoGraphicUtils.dll", "SatoGraphicUtils.dll");
      AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(Utils.CurrentDomain_AssemblyResolve);
      AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(Utils.CurrentDomain_AssemblyResolve);
      Utils.assemblyDefined = true;
    }

    //private static void ConvertGraphicToFile(string GraphicFile, string TempDir) => SbplConverter.Convert(GraphicFile, TempDir, true);

    /*public static string ConvertGraphicToSBPL(string GraphicFilePath, bool deleteFile = false)
    {
      if (!Utils.assemblyDefined)
        Utils.DefineAssembly();
      string str1 = Path.GetTempPath() + "SBPL_" + (object) Guid.NewGuid() + ".txt";
      Utils.ConvertGraphicToFile(GraphicFilePath, str1);
      if (deleteFile)
        System.IO.File.Delete(GraphicFilePath);
      byte[] bytes = System.IO.File.ReadAllBytes(str1);
      System.IO.File.Delete(str1);
      string str2 = new ASCIIEncoding().GetString(bytes);
      string str3 = "";
      if (bytes != null && bytes.Length > 8)
      {
        str3 = str2.Substring(0, 9);
        for (int index = 9; index < bytes.Length; ++index)
          str3 += Utils.IntToHex((int) bytes[index], 1);
      }
      return str3.Replace("\u001BGB", "\u001BGH");
    }*/

    /*public static string ConvertGraphicToSBPL(Uri GraphicFilePath)
    {
      if (GraphicFilePath.Scheme == Uri.UriSchemeFile)
        return Utils.ConvertGraphicToSBPL(GraphicFilePath.LocalPath);
      if (!(GraphicFilePath.Scheme == Uri.UriSchemeHttp) && !(GraphicFilePath.Scheme == Uri.UriSchemeHttps))
        return (string) null;
      byte[] file = Utils.HTTPGetFile(GraphicFilePath);
      string str = Path.GetTempPath() + "GP_" + (object) Guid.NewGuid() + ".txt";
      using (FileStream fileStream = new FileStream(str, FileMode.Create, FileAccess.Write))
        fileStream.Write(file, 0, file.Length);
      return Utils.ConvertGraphicToSBPL(str, true);
    }*/

    public static string CommandDataReplace(
      string CommandFilePath,
      Dictionary<string, string> VariablesValue,
      string encoding = "ansi",
      bool deleteFile = false)
    {
      byte[] Data = System.IO.File.ReadAllBytes(CommandFilePath);
      if (deleteFile)
        System.IO.File.Delete(CommandFilePath);
      string encoding1 = encoding;
      string str = Utils.ByteArrayToString(Data, encoding1);
      foreach (string key in VariablesValue.Keys)
        str = str.Replace(key, VariablesValue[key]);
      return str;
    }

    public static string CommandDataReplace(
      Uri CommandFilePath,
      Dictionary<string, string> VariablesValue,
      string encoding = "ansi")
    {
      if (CommandFilePath.Scheme == Uri.UriSchemeFile)
        return Utils.CommandDataReplace(CommandFilePath.LocalPath, VariablesValue, encoding);
      if (!(CommandFilePath.Scheme == Uri.UriSchemeHttp) && !(CommandFilePath.Scheme == Uri.UriSchemeHttps))
        return (string) null;
      byte[] file = Utils.HTTPGetFile(CommandFilePath);
      string str = Path.GetTempPath() + "CD_" + (object) Guid.NewGuid() + ".txt";
      using (FileStream fileStream = new FileStream(str, FileMode.Create, FileAccess.Write))
        fileStream.Write(file, 0, file.Length);
      return Utils.CommandDataReplace(str, VariablesValue, encoding, true);
    }

    private static string IntToHex(int data, int numBytes)
    {
      string format = string.Format("X{0}", (object) (numBytes * 2));
      if (data < 0)
        data += (int) Math.Pow(2.0, (double) (numBytes * 8));
      byte[] byteString = Utils.ParseByteString(data.ToString(format));
      StringBuilder stringBuilder = new StringBuilder();
      if (byteString != null && byteString.Length != 0)
      {
        for (int index = 0; index < byteString.Length; ++index)
          stringBuilder.Append(byteString[index].ToString("X2"));
      }
      return stringBuilder.ToString();
    }

    private static byte[] ParseByteString(string data)
    {
      byte[] byteString = (byte[]) null;
      if (data != null && data.Length > 0)
      {
        if (data.Length % 2 != 0)
          data = "0" + data;
        int length = data.Length / 2;
        byteString = new byte[length];
        for (int index = 0; index < length; ++index)
        {
          string str = data.Substring(index * 2, 2);
          byteString[index] = Convert.ToByte(str, 16);
        }
      }
      return byteString;
    }

    public static byte[] StringToByteArray(string Data, string encoding = "ansi")
    {
      if (encoding == null)
        return Encoding.Default.GetBytes(Data);
      byte[] bytes;
      switch (encoding.ToLower())
      {
        case "ascii":
          bytes = Encoding.ASCII.GetBytes(Data);
          break;
        case "utf7":
          bytes = Encoding.UTF7.GetBytes(Data);
          break;
        case "utf8":
          bytes = Encoding.UTF8.GetBytes(Data);
          break;
        case "ansi":
          bytes = Encoding.Default.GetBytes(Data);
          break;
        case "utf16":
          bytes = Encoding.Unicode.GetBytes(Data);
          break;
        case "utf32":
          bytes = Encoding.UTF32.GetBytes(Data);
          break;
        default:
          bytes = Encoding.Default.GetBytes(Data);
          break;
      }
      return bytes;
    }

    public static string ByteArrayToString(byte[] Data, string encoding = "ansi")
    {
      if (encoding == null)
        return Encoding.Default.GetString(Data);
      string str;
      switch (encoding.ToLower())
      {
        case "ascii":
          str = Encoding.ASCII.GetString(Data);
          break;
        case "utf7":
          str = Encoding.UTF7.GetString(Data);
          break;
        case "utf8":
          str = Encoding.UTF8.GetString(Data);
          break;
        case "ansi":
          str = Encoding.Default.GetString(Data);
          break;
        case "utf16":
          str = Encoding.Unicode.GetString(Data);
          break;
        case "utf32":
          str = Encoding.UTF32.GetString(Data);
          break;
        default:
          str = Encoding.Default.GetString(Data);
          break;
      }
      return str;
    }

    private static byte[] LocalGetFile(Uri url)
    {
      string localPath = url.LocalPath;
      byte[] buffer = (byte[]) null;
      try
      {
        if (System.IO.File.Exists(localPath))
        {
          FileStream fileStream = System.IO.File.OpenRead(localPath);
          buffer = new byte[fileStream.Length];
          int num = fileStream.Read(buffer, 0, buffer.Length);
          fileStream.Close();
          int length = buffer.Length;
          if (num != length)
            throw new Exception("Unable to read all data from file : " + url.ToString());
        }
      }
      catch (Exception ex)
      {
        throw new Exception("Fail to load local file : " + localPath, ex);
      }
      return buffer;
    }

    private static byte[] HTTPGetFile(Uri url)
    {
      byte[] file = (byte[]) null;
      try
      {
        HttpWebResponse response = (HttpWebResponse) WebRequest.Create(url).GetResponse();
        Stream stream = response.StatusCode == HttpStatusCode.OK ? response.GetResponseStream() : throw new Exception("HTTP Reply : [" + response.StatusCode.ToString() + "]" + response.StatusDescription);
        using (MemoryStream destination = new MemoryStream())
        {
          stream.CopyTo((Stream) destination);
          file = destination.ToArray();
        }
        stream.Close();
        response.Close();
      }
      catch (Exception ex)
      {
        throw new Exception("Fail to load remote file : " + url.ToString(), ex);
      }
      return file;
    }
  }
}
