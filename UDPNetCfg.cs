// Decompiled with JetBrains decompiler
// Type: SATOPrinterAPI.UDPNetCfg
// Assembly: SATOPrinterAPI, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1CED7137-37FF-4597-9D4E-DAF488B01141
// Assembly location: D:\Downloads\Microsoft Edge\ApiPort.2.8.25.Offline\ApiPort.Offline\netcoreapp3.1\SATOPrinterAPI.dll

using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SATOPrinterAPI
{
  internal class UDPNetCfg
  {
    private byte[] EEPDataIn1;
    private byte[] EEPDataIn2;
    private byte[] EEPDataInW;
    private IPConfig curPrinter;
    private const int Printer_Dataport = 9100;
    private const int Silex_UDPport = 19541;
    private const int SATO_UDPport = 19541;
    private const int Printronix_UDPport = 9;
    private const int Printronix_SNCport = 3001;
    private byte[] GetEEPDATA1 = new byte[510];
    private static readonly byte[] GetEEPDATA1_fix = new byte[88]
    {
      (byte) 0,
      (byte) 1,
      (byte) 0,
      (byte) 1,
      (byte) 71,
      (byte) 69,
      (byte) 84,
      (byte) 32,
      (byte) 69,
      (byte) 69,
      (byte) 80,
      (byte) 95,
      (byte) 68,
      (byte) 65,
      (byte) 84,
      (byte) 65,
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
      (byte) 2,
      (byte) 0
    };
    private byte[] GetEEPDATA2 = new byte[88]
    {
      (byte) 0,
      (byte) 2,
      (byte) 0,
      (byte) 1,
      (byte) 71,
      (byte) 69,
      (byte) 84,
      (byte) 32,
      (byte) 69,
      (byte) 69,
      (byte) 80,
      (byte) 95,
      (byte) 68,
      (byte) 65,
      (byte) 84,
      (byte) 65,
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
      (byte) 2,
      (byte) 0,
      (byte) 2,
      (byte) 0
    };
    private byte[] GetEEPDATAW = new byte[88]
    {
      (byte) 0,
      (byte) 2,
      (byte) 0,
      (byte) 1,
      (byte) 71,
      (byte) 69,
      (byte) 84,
      (byte) 32,
      (byte) 69,
      (byte) 69,
      (byte) 80,
      (byte) 95,
      (byte) 68,
      (byte) 65,
      (byte) 84,
      (byte) 65,
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
      (byte) 30,
      (byte) 0,
      (byte) 2,
      (byte) 0
    };
    private byte[] SetEEPData1 = new byte[510];
    private static readonly byte[] SetEEPData1_fix = new byte[88]
    {
      (byte) 0,
      (byte) 1,
      (byte) 0,
      (byte) 1,
      (byte) 83,
      (byte) 65,
      (byte) 86,
      (byte) 69,
      (byte) 32,
      (byte) 69,
      (byte) 69,
      (byte) 80,
      (byte) 95,
      (byte) 68,
      (byte) 65,
      (byte) 84,
      (byte) 65,
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
      (byte) 1,
      (byte) 166
    };
    private byte[] SetEEPData2 = new byte[510];
    private static readonly byte[] SetEEPData2_fix = new byte[88]
    {
      (byte) 0,
      (byte) 2,
      (byte) 0,
      (byte) 1,
      (byte) 83,
      (byte) 65,
      (byte) 86,
      (byte) 69,
      (byte) 32,
      (byte) 69,
      (byte) 69,
      (byte) 80,
      (byte) 95,
      (byte) 68,
      (byte) 65,
      (byte) 84,
      (byte) 65,
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
      (byte) 1,
      (byte) 166,
      (byte) 1,
      (byte) 166
    };
    private static readonly byte[] SetEEPData3_End = new byte[88]
    {
      (byte) 0,
      (byte) 3,
      (byte) 0,
      (byte) 1,
      (byte) 83,
      (byte) 65,
      (byte) 86,
      (byte) 69,
      (byte) 32,
      (byte) 69,
      (byte) 69,
      (byte) 80,
      (byte) 95,
      (byte) 68,
      (byte) 65,
      (byte) 84,
      (byte) 65,
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
      (byte) 32,
      (byte) 0,
      (byte) 0,
      (byte) 0
    };
    private static byte[] AssignIP1 = new byte[510]
    {
      (byte) 0,
      (byte) 1,
      (byte) 0,
      (byte) 1,
      (byte) 83,
      (byte) 69,
      (byte) 84,
      (byte) 32,
      (byte) 73,
      (byte) 80,
      (byte) 95,
      (byte) 65,
      (byte) 68,
      (byte) 68,
      (byte) 82,
      (byte) 58,
      (byte) 1,
      (byte) 6,
      (byte) 0,
      (byte) 128,
      (byte) 146,
      (byte) 54,
      (byte) 213,
      (byte) 100,
      (byte) 10,
      (byte) 25,
      (byte) 5,
      (byte) 135,
      (byte) 80,
      (byte) 80,
      (byte) 68,
      (byte) 65,
      (byte) 84,
      (byte) 65,
      (byte) 61,
      (byte) 67,
      (byte) 58,
      (byte) 92,
      (byte) 68,
      (byte) 111,
      (byte) 99,
      (byte) 117,
      (byte) 109,
      (byte) 101,
      (byte) 110,
      (byte) 116,
      (byte) 115,
      (byte) 32,
      (byte) 97,
      (byte) 110,
      (byte) 100,
      (byte) 32,
      (byte) 83,
      (byte) 101,
      (byte) 116,
      (byte) 116,
      (byte) 105,
      (byte) 110,
      (byte) 103,
      (byte) 115,
      (byte) 92,
      (byte) 121,
      (byte) 97,
      (byte) 110,
      (byte) 110,
      (byte) 97,
      (byte) 105,
      (byte) 110,
      (byte) 103,
      (byte) 92,
      (byte) 65,
      (byte) 112,
      (byte) 112,
      (byte) 108,
      (byte) 105,
      (byte) 99,
      (byte) 97,
      (byte) 116,
      (byte) 105,
      (byte) 111,
      (byte) 110,
      (byte) 32,
      (byte) 68,
      (byte) 97,
      (byte) 0,
      (byte) 0,
      (byte) 2,
      (byte) 0,
      (byte) 76,
      (byte) 73,
      (byte) 69,
      (byte) 78,
      (byte) 84,
      (byte) 78,
      (byte) 65,
      (byte) 77,
      (byte) 69,
      (byte) 61,
      (byte) 67,
      (byte) 111,
      (byte) 110,
      (byte) 115,
      (byte) 111,
      (byte) 108,
      (byte) 101,
      (byte) 0,
      (byte) 67,
      (byte) 111,
      (byte) 109,
      (byte) 109,
      (byte) 111,
      (byte) 110,
      (byte) 80,
      (byte) 114,
      (byte) 111,
      (byte) 103,
      (byte) 114,
      (byte) 97,
      (byte) 109,
      (byte) 70,
      (byte) 105,
      (byte) 108,
      (byte) 101,
      (byte) 115,
      (byte) 61,
      (byte) 67,
      (byte) 58,
      (byte) 92,
      (byte) 80,
      (byte) 114,
      (byte) 111,
      (byte) 103,
      (byte) 114,
      (byte) 97,
      (byte) 109,
      (byte) 32,
      (byte) 70,
      (byte) 105,
      (byte) 108,
      (byte) 101,
      (byte) 115,
      (byte) 92,
      (byte) 67,
      (byte) 111,
      (byte) 109,
      (byte) 109,
      (byte) 111,
      (byte) 110,
      (byte) 32,
      (byte) 70,
      (byte) 105,
      (byte) 108,
      (byte) 101,
      (byte) 115,
      (byte) 0,
      (byte) 67,
      (byte) 79,
      (byte) 77,
      (byte) 80,
      (byte) 85,
      (byte) 84,
      (byte) 69,
      (byte) 82,
      (byte) 78,
      (byte) 65,
      (byte) 77,
      (byte) 69,
      (byte) 61,
      (byte) 83,
      (byte) 73,
      (byte) 45,
      (byte) 78,
      (byte) 45,
      (byte) 49,
      (byte) 48,
      (byte) 48,
      (byte) 55,
      (byte) 50,
      (byte) 51,
      (byte) 55,
      (byte) 0,
      (byte) 67,
      (byte) 111,
      (byte) 109,
      (byte) 83,
      (byte) 112,
      (byte) 101,
      (byte) 99,
      (byte) 61,
      (byte) 67,
      (byte) 58,
      (byte) 92,
      (byte) 87,
      (byte) 73,
      (byte) 78,
      (byte) 68,
      (byte) 79,
      (byte) 87,
      (byte) 83,
      (byte) 92,
      (byte) 115,
      (byte) 121,
      (byte) 115,
      (byte) 116,
      (byte) 101,
      (byte) 109,
      (byte) 51,
      (byte) 50,
      (byte) 92,
      (byte) 99,
      (byte) 109,
      (byte) 100,
      (byte) 46,
      (byte) 101,
      (byte) 120,
      (byte) 101,
      (byte) 0,
      (byte) 70,
      (byte) 80,
      (byte) 95,
      (byte) 78,
      (byte) 79,
      (byte) 95,
      (byte) 72,
      (byte) 79,
      (byte) 83,
      (byte) 84,
      (byte) 95,
      (byte) 67,
      (byte) 72,
      (byte) 69,
      (byte) 67,
      (byte) 75,
      (byte) 61,
      (byte) 78,
      (byte) 79,
      (byte) 0,
      (byte) 72,
      (byte) 79,
      (byte) 77,
      (byte) 69,
      (byte) 61,
      (byte) 70,
      (byte) 58,
      (byte) 92,
      (byte) 110,
      (byte) 97,
      (byte) 105,
      (byte) 92,
      (byte) 0,
      (byte) 72,
      (byte) 79,
      (byte) 77,
      (byte) 69,
      (byte) 68,
      (byte) 82,
      (byte) 73,
      (byte) 86,
      (byte) 69,
      (byte) 61,
      (byte) 67,
      (byte) 58,
      (byte) 0,
      (byte) 72,
      (byte) 79,
      (byte) 77,
      (byte) 69,
      (byte) 80,
      (byte) 65,
      (byte) 84,
      (byte) 72,
      (byte) 61,
      (byte) 92,
      (byte) 68,
      (byte) 111,
      (byte) 99,
      (byte) 117,
      (byte) 109,
      (byte) 101,
      (byte) 110,
      (byte) 116,
      (byte) 115,
      (byte) 32,
      (byte) 97,
      (byte) 110,
      (byte) 100,
      (byte) 32,
      (byte) 83,
      (byte) 101,
      (byte) 116,
      (byte) 116,
      (byte) 105,
      (byte) 110,
      (byte) 103,
      (byte) 115,
      (byte) 92,
      (byte) 121,
      (byte) 97,
      (byte) 110,
      (byte) 110,
      (byte) 97,
      (byte) 105,
      (byte) 110,
      (byte) 103,
      (byte) 0,
      (byte) 108,
      (byte) 105,
      (byte) 98,
      (byte) 61,
      (byte) 67,
      (byte) 58,
      (byte) 92,
      (byte) 80,
      (byte) 114,
      (byte) 111,
      (byte) 103,
      (byte) 114,
      (byte) 97,
      (byte) 109,
      (byte) 32,
      (byte) 70,
      (byte) 105,
      (byte) 108,
      (byte) 101,
      (byte) 115,
      (byte) 92,
      (byte) 83,
      (byte) 81,
      (byte) 76,
      (byte) 88,
      (byte) 77,
      (byte) 76,
      (byte) 32,
      (byte) 52,
      (byte) 46,
      (byte) 48,
      (byte) 92,
      (byte) 98,
      (byte) 105,
      (byte) 110,
      (byte) 92,
      (byte) 0,
      (byte) 76,
      (byte) 79,
      (byte) 71,
      (byte) 79,
      (byte) 78,
      (byte) 83,
      (byte) 69,
      (byte) 82,
      (byte) 86,
      (byte) 69,
      (byte) 82,
      (byte) 61,
      (byte) 92,
      (byte) 92,
      (byte) 69,
      (byte) 88,
      (byte) 67,
      (byte) 72,
      (byte) 83,
      (byte) 86,
      (byte) 82,
      (byte) 48,
      (byte) 55,
      (byte) 0,
      (byte) 78,
      (byte) 85,
      (byte) 77,
      (byte) 66,
      (byte) 69,
      (byte) 82,
      (byte) 95,
      (byte) 79,
      (byte) 70,
      (byte) 95,
      (byte) 80,
      (byte) 82,
      (byte) 79,
      (byte) 67,
      (byte) 69,
      (byte) 83,
      (byte) 83,
      (byte) 79,
      (byte) 82,
      (byte) 83,
      (byte) 61,
      (byte) 50,
      (byte) 0,
      (byte) 79,
      (byte) 83,
      (byte) 61,
      (byte) 87,
      (byte) 105,
      (byte) 110,
      (byte) 100,
      (byte) 111,
      (byte) 119,
      (byte) 115,
      (byte) 95,
      (byte) 78,
      (byte) 84,
      (byte) 0,
      (byte) 80,
      (byte) 97,
      (byte) 116,
      (byte) 104,
      (byte) 61,
      (byte) 67,
      (byte) 58,
      (byte) 92,
      (byte) 87,
      (byte) 73,
      (byte) 78,
      (byte) 68,
      (byte) 79,
      (byte) 87,
      (byte) 83,
      (byte) 92,
      (byte) 115,
      (byte) 121,
      (byte) 115,
      (byte) 116,
      (byte) 101,
      (byte) 109,
      (byte) 51,
      (byte) 50,
      (byte) 59,
      (byte) 67,
      (byte) 58,
      (byte) 92,
      (byte) 87,
      (byte) 73,
      (byte) 78,
      (byte) 68,
      (byte) 79,
      (byte) 87,
      (byte) 83,
      (byte) 59,
      (byte) 67,
      (byte) 58,
      (byte) 92,
      (byte) 87,
      (byte) 73,
      (byte) 78,
      (byte) 68,
      (byte) 79,
      (byte) 87,
      (byte) 83,
      (byte) 92,
      (byte) 83,
      (byte) 121,
      (byte) 115,
      (byte) 116,
      (byte) 101,
      (byte) 109,
      (byte) 51,
      (byte) 50,
      (byte) 92,
      (byte) 87,
      (byte) 98,
      (byte) 101,
      (byte) 109,
      (byte) 59,
      (byte) 67,
      (byte) 58,
      (byte) 92,
      (byte) 80,
      (byte) 114,
      (byte) 111,
      (byte) 103,
      (byte) 114,
      (byte) 97,
      (byte) 109,
      (byte) 32,
      (byte) 70,
      (byte) 105,
      (byte) 108,
      (byte) 101,
      (byte) 115,
      (byte) 92,
      (byte) 73,
      (byte) 110,
      (byte) 116,
      (byte) 101,
      (byte) 108,
      (byte) 92,
      (byte) 87,
      (byte) 105,
      (byte) 114,
      (byte) 101,
      (byte) 108,
      (byte) 101,
      (byte) 115,
      (byte) 115,
      (byte) 92,
      (byte) 66,
      (byte) 105,
      (byte) 110,
      (byte) 92,
      (byte) 59,
      (byte) 67,
      (byte) 58,
      (byte) 92,
      (byte) 80,
      (byte) 114,
      (byte) 111,
      (byte) 103,
      (byte) 114,
      (byte) 97
    };
    private static byte[] AssignIP2 = new byte[88]
    {
      (byte) 0,
      (byte) 3,
      (byte) 0,
      (byte) 1,
      (byte) 83,
      (byte) 69,
      (byte) 84,
      (byte) 32,
      (byte) 73,
      (byte) 80,
      (byte) 95,
      (byte) 65,
      (byte) 68,
      (byte) 68,
      (byte) 82,
      (byte) 58,
      (byte) 1,
      (byte) 6,
      (byte) 0,
      (byte) 128,
      (byte) 146,
      (byte) 54,
      (byte) 213,
      (byte) 100,
      (byte) 10,
      (byte) 25,
      (byte) 5,
      (byte) 135,
      (byte) 80,
      (byte) 80,
      (byte) 68,
      (byte) 65,
      (byte) 84,
      (byte) 65,
      (byte) 61,
      (byte) 67,
      (byte) 58,
      (byte) 92,
      (byte) 68,
      (byte) 111,
      (byte) 99,
      (byte) 117,
      (byte) 109,
      (byte) 101,
      (byte) 110,
      (byte) 116,
      (byte) 115,
      (byte) 32,
      (byte) 97,
      (byte) 110,
      (byte) 100,
      (byte) 32,
      (byte) 83,
      (byte) 101,
      (byte) 116,
      (byte) 116,
      (byte) 105,
      (byte) 110,
      (byte) 103,
      (byte) 115,
      (byte) 92,
      (byte) 121,
      (byte) 97,
      (byte) 110,
      (byte) 110,
      (byte) 97,
      (byte) 105,
      (byte) 110,
      (byte) 103,
      (byte) 92,
      (byte) 65,
      (byte) 112,
      (byte) 112,
      (byte) 108,
      (byte) 105,
      (byte) 99,
      (byte) 97,
      (byte) 116,
      (byte) 105,
      (byte) 111,
      (byte) 110,
      (byte) 32,
      (byte) 68,
      (byte) 97,
      (byte) 0,
      (byte) 0,
      (byte) 2,
      (byte) 0
    };
    private byte[] RebootPrinterData = new byte[510];
    private static readonly byte[] RebootPrinterData_fix = new byte[25]
    {
      (byte) 0,
      (byte) 1,
      (byte) 0,
      (byte) 1,
      (byte) 69,
      (byte) 88,
      (byte) 69,
      (byte) 67,
      (byte) 32,
      (byte) 82,
      (byte) 69,
      (byte) 66,
      (byte) 79,
      (byte) 79,
      (byte) 84,
      (byte) 32,
      (byte) 83,
      (byte) 89,
      (byte) 83,
      (byte) 84,
      (byte) 69,
      (byte) 77,
      (byte) 58,
      (byte) 1,
      (byte) 6
    };
    private byte[] PollSend_1 = new byte[510]
    {
      (byte) 0,
      (byte) 1,
      (byte) 0,
      (byte) 1,
      (byte) 80,
      (byte) 79,
      (byte) 76,
      (byte) 76,
      (byte) 73,
      (byte) 78,
      (byte) 71,
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
      (byte) 2,
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
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0
    };
    private byte[] PollRec_1 = new byte[8]
    {
      (byte) 0,
      (byte) 1,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0
    };
    private byte[] PollSend_2 = new byte[88]
    {
      (byte) 0,
      (byte) 3,
      (byte) 0,
      (byte) 1,
      (byte) 80,
      (byte) 79,
      (byte) 76,
      (byte) 76,
      (byte) 73,
      (byte) 78,
      (byte) 71,
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
      (byte) 2,
      (byte) 0
    };
    private byte[] PollRec_2 = new byte[8]
    {
      (byte) 0,
      (byte) 3,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0
    };
    private byte[] GLDiscovery_pkg = new byte[57]
    {
      (byte) 0,
      (byte) 128,
      (byte) 114,
      (byte) 2,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 128,
      (byte) 114,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue,
      (byte) 10,
      (byte) 25,
      (byte) 5,
      (byte) 9,
      (byte) 7,
      (byte) 241,
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
      (byte) 1,
      (byte) 9,
      (byte) 0,
      (byte) 0,
      (byte) 2,
      (byte) 11,
      (byte) 0,
      (byte) 13,
      (byte) 18,
      (byte) 133,
      (byte) 201,
      (byte) 212,
      (byte) 160,
      (byte) 120,
      (byte) 82,
      (byte) 22,
      (byte) 54,
      (byte) 27,
      (byte) 250,
      (byte) 151,
      (byte) 30
    };
    private byte[] GLAssignPkg_fix = new byte[6]
    {
      (byte) 0,
      (byte) 128,
      (byte) 114,
      (byte) 2,
      (byte) 0,
      (byte) 0
    };
    private byte[] GLAssignPkg = new byte[36];

    public UDPNetCfg(IPConfig printer)
    {
      Array.Copy((Array) UDPNetCfg.GetEEPDATA1_fix, (Array) this.GetEEPDATA1, UDPNetCfg.GetEEPDATA1_fix.Length);
      Array.Copy((Array) UDPNetCfg.RebootPrinterData_fix, (Array) this.RebootPrinterData, UDPNetCfg.RebootPrinterData_fix.Length);
      Array.Copy((Array) this.GLAssignPkg_fix, (Array) this.GLAssignPkg, this.GLAssignPkg_fix.Length);
      this.curPrinter = printer;
    }

    public IPConfig RetriveGLNetCfg()
    {
      if (new Ping().Send(this.curPrinter.IP, 120).Status != IPStatus.Success)
        throw new Exception("IP address unreachable.");
      using (UdpClient udpClient1 = new UdpClient())
      {
        IPEndPoint localEP = new IPEndPoint(IPAddress.Any, 0);
        udpClient1.Client.Bind((EndPoint) localEP);
        int port = ((IPEndPoint) udpClient1.Client.LocalEndPoint).Port;
        byte[] addressBytes = Dns.GetHostAddresses(Dns.GetHostName())[0].GetAddressBytes();
        using (UdpClient udpClient2 = new UdpClient(this.curPrinter.IP, 9))
        {
          udpClient1.Client.ReceiveTimeout = 5000;
          Array.Copy((Array) addressBytes, 0, (Array) this.GLDiscovery_pkg, 12, addressBytes.Length);
          this.GLDiscovery_pkg[16] = (byte) (port >> 8 & (int) byte.MaxValue);
          this.GLDiscovery_pkg[17] = (byte) (port & (int) byte.MaxValue);
          udpClient2.Send(this.GLDiscovery_pkg, this.GLDiscovery_pkg.Length);
          IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
          byte[] data = udpClient1.Receive(ref remoteEP);
          udpClient1.Close();
          udpClient2.Close();
          return this.GL_ExtractData(data);
        }
      }
    }

    private IPConfig GL_ExtractData(byte[] data)
    {
      IPConfig data1 = new IPConfig();
      data1.IsWLan = false;
      if (data != null)
      {
        byte[] numArray1 = new byte[4];
        byte[] numArray2 = new byte[6];
        int sourceIndex1 = 6;
        Array.Copy((Array) data, sourceIndex1, (Array) numArray2, 0, numArray2.Length);
        int sourceIndex2 = 24;
        data1.MAC = IPConfig.MACArray2String(numArray2, '-');
        Array.Copy((Array) data, sourceIndex2, (Array) numArray1, 0, numArray1.Length);
        int sourceIndex3 = sourceIndex2 + numArray1.Length;
        data1.IP = IPConfig.IPArray2String(numArray1);
        Array.Copy((Array) data, sourceIndex3, (Array) numArray1, 0, numArray1.Length);
        int sourceIndex4 = sourceIndex3 + numArray1.Length;
        data1.Subnet = IPConfig.IPArray2String(numArray1);
        Array.Copy((Array) data, sourceIndex4, (Array) numArray1, 0, numArray1.Length);
        data1.Gateway = IPConfig.IPArray2String(numArray1);
      }
      return data1;
    }

    public IPConfig RetriveSatoNetCfg()
    {
      IPEndPoint localEP;
      if (this.curPrinter.IP != "0.0.0.0")
      {
        localEP = new IPEndPoint(IPAddress.Parse(this.curPrinter.IP), 19541);
        if (new Ping().Send(this.curPrinter.IP, 120).Status != IPStatus.Success)
          throw new Exception("IP address unreachable.");
      }
      else
        localEP = new IPEndPoint(IPAddress.Broadcast, NetConst.DiscvPortSato);
      using (UdpClient udpClient = new UdpClient(localEP))
      {
        udpClient.Client.ReceiveTimeout = 5000;
        udpClient.Send(NetConst.DiscvSato, NetConst.DiscvSato.Length);
        IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
        IPConfig printer;
        do
        {
          printer = LanFinder.ParsePrinter(udpClient.Receive(ref remoteEP));
        }
        while (printer.MAC != this.curPrinter.MAC);
        udpClient.Close();
        return printer;
      }
    }

    public IPConfig RetriveSilexNetCfg()
    {
      this.EEPDataIn1 = (byte[]) null;
      this.EEPDataIn2 = (byte[]) null;
      this.EEPDataInW = (byte[]) null;
      if (new Ping().Send(this.curPrinter.IP, 120).Status != IPStatus.Success)
        throw new Exception("IP address unreachable.");
      using (UdpClient udpClient = new UdpClient(this.curPrinter.IP, 19541))
      {
        udpClient.Client.ReceiveTimeout = 5000;
        udpClient.Send(this.GetEEPDATA1, this.GetEEPDATA1.Length);
        IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
        this.EEPDataIn1 = udpClient.Receive(ref remoteEP);
        udpClient.Send(this.GetEEPDATA2, this.GetEEPDATA2.Length);
        this.EEPDataIn2 = udpClient.Receive(ref remoteEP);
        udpClient.Send(this.GetEEPDATAW, this.GetEEPDATAW.Length);
        this.EEPDataInW = udpClient.Receive(ref remoteEP);
        udpClient.Close();
        return this.Silex_ExtractData();
      }
    }

    private void PrepareRebootData()
    {
      byte[] sourceArray = IPConfig.IPString2Array(this.curPrinter.IP);
      Array.Copy((Array) IPConfig.MACString2Array(this.curPrinter.MAC), 0, (Array) this.RebootPrinterData, UDPNetCfg.RebootPrinterData_fix.Length, 6);
      byte[] rebootPrinterData = this.RebootPrinterData;
      int destinationIndex = UDPNetCfg.RebootPrinterData_fix.Length + 6;
      Array.Copy((Array) sourceArray, 0, (Array) rebootPrinterData, destinationIndex, 4);
    }

    public bool RebootSilexNetServer() => this.RebootSilexNetServer(false);

    public bool RebootSilexNetServer(bool UseCurrent)
    {
      if (UseCurrent)
        this.PrepareRebootData();
      IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
      using (UdpClient udpClient = new UdpClient(this.curPrinter.IP, 19541))
      {
        udpClient.Client.ReceiveTimeout = 5000;
        udpClient.Send(this.RebootPrinterData, this.RebootPrinterData.Length);
        try
        {
          byte[] bytes = udpClient.Receive(ref remoteEP);
          if (bytes != null)
          {
            if (Encoding.ASCII.GetString(bytes).Contains("OK"))
              return true;
          }
        }
        catch
        {
        }
        udpClient.Close();
      }
      return false;
    }

    private bool AssignGLNetCfg(ref IPConfig netconfig)
    {
      using (UdpClient udpClient = new UdpClient())
      {
        udpClient.Client.ReceiveTimeout = 5000;
        IPEndPoint localEP = new IPEndPoint(IPAddress.Any, 0);
        udpClient.Client.Bind((EndPoint) localEP);
        IPEndPoint localEndPoint = udpClient.Client.LocalEndPoint as IPEndPoint;
        localEndPoint.Address = Dns.GetHostAddresses(Dns.GetHostName())[0];
        this.GL_ChangeData(ref netconfig, localEndPoint);
        IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
        byte[] numArray;
        if (this.curPrinter.IP.Equals("0.0.0.0"))
        {
          udpClient.EnableBroadcast = true;
          udpClient.Send(this.GLAssignPkg, this.GLAssignPkg.Length, IPAddress.Broadcast.ToString(), 9);
          numArray = udpClient.Receive(ref remoteEP);
        }
        else
        {
          udpClient.Send(this.GLAssignPkg, this.GLAssignPkg.Length, this.curPrinter.IP, 9);
          numArray = udpClient.Receive(ref remoteEP);
        }
        udpClient.Close();
        if (numArray != null)
          return true;
      }
      return false;
    }

    private void GL_ChangeData(ref IPConfig netconfig, IPEndPoint lep)
    {
      byte[] sourceArray1 = IPConfig.IPString2Array(netconfig.IP);
      byte[] sourceArray2 = IPConfig.IPString2Array(netconfig.Subnet);
      byte[] sourceArray3 = IPConfig.IPString2Array(netconfig.Gateway);
      byte[] sourceArray4 = IPConfig.MACString2Array(netconfig.MAC);
      int num1 = 6;
      byte[] glAssignPkg1 = this.GLAssignPkg;
      int destinationIndex1 = num1;
      Array.Copy((Array) sourceArray4, 0, (Array) glAssignPkg1, destinationIndex1, 6);
      int destinationIndex2 = num1 + 6;
      Array.Copy((Array) lep.Address.GetAddressBytes(), 0, (Array) this.GLAssignPkg, destinationIndex2, 4);
      int num2 = destinationIndex2 + 4;
      byte[] glAssignPkg2 = this.GLAssignPkg;
      int index1 = num2;
      int num3 = index1 + 1;
      int num4 = (int) (byte) ((int) byte.MaxValue & lep.Port >> 8);
      glAssignPkg2[index1] = (byte) num4;
      byte[] glAssignPkg3 = this.GLAssignPkg;
      int index2 = num3;
      int destinationIndex3 = index2 + 1;
      int num5 = (int) (byte) ((int) byte.MaxValue & lep.Port);
      glAssignPkg3[index2] = (byte) num5;
      Array.Copy((Array) sourceArray3, 0, (Array) this.GLAssignPkg, destinationIndex3, 4);
      int destinationIndex4 = destinationIndex3 + 4 + 2;
      Array.Copy((Array) sourceArray1, 0, (Array) this.GLAssignPkg, destinationIndex4, 4);
      int destinationIndex5 = destinationIndex4 + 4;
      Array.Copy((Array) sourceArray2, 0, (Array) this.GLAssignPkg, destinationIndex5, 4);
      int destinationIndex6 = destinationIndex5 + 4;
      Array.Copy((Array) sourceArray3, 0, (Array) this.GLAssignPkg, destinationIndex6, 4);
      int num6 = destinationIndex6 + 4;
    }

    public bool AssignNetCfg(ref IPConfig netconfig)
    {
      if (this.IsGLModel)
        return this.AssignGLNetCfg(ref netconfig);
      if (!this.IsSilexModel)
        return this.AssignSATONetCfg(ref netconfig);
      if (netconfig.UdpBCmode)
      {
        if (!this.AssignIPSilex(ref netconfig))
          return false;
        netconfig.UdpBCmode = false;
        this.curPrinter.UdpBCmode = false;
        this.curPrinter.IP = netconfig.IP;
        netconfig = this.RetriveSilexNetCfg();
        netconfig.IP = this.curPrinter.IP;
        Thread.Sleep(3000);
      }
      return this.AssignSilexNetCfg(ref netconfig);
    }

    private bool AssignSATONetCfg(ref IPConfig netconfig)
    {
      byte[] dgram = this.SATOBuildCMD(ref netconfig);
      IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 19541);
      string ip = this.curPrinter.IP;
      if (this.curPrinter.IP.Equals("0.0.0.0"))
        ip = IPAddress.Broadcast.ToString();
      using (UdpClient udpClient = new UdpClient(ip, 19541))
      {
        if (this.curPrinter.IP.Equals("0.0.0.0"))
          udpClient.EnableBroadcast = true;
        udpClient.Client.ReceiveTimeout = 5000;
        udpClient.Send(dgram, dgram.Length);
        Thread.Sleep(10);
        udpClient.Send(dgram, dgram.Length);
        Thread.Sleep(10);
        udpClient.Send(dgram, dgram.Length);
        byte[] numArray = udpClient.Receive(ref remoteEP);
        if (numArray != null && numArray[1] == (byte) 87 && numArray[2] == (byte) 88 && numArray[3] == (byte) 49)
        {
          udpClient.Close();
          return true;
        }
        udpClient.Close();
      }
      return false;
    }

    private byte[] SATOBuildCMD(ref IPConfig netconfig)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append('\u001B');
      stringBuilder.Append("WJ");
      if (netconfig.RARPFlag)
        stringBuilder.Append("1,");
      else
        stringBuilder.Append("0,");
      stringBuilder.Append('\u001B');
      stringBuilder.Append("WI");
      if (netconfig.DHCPFlag)
        stringBuilder.Append("1,");
      else
        stringBuilder.Append("0,");
      if (!netconfig.DHCPFlag)
      {
        if (!netconfig.RARPFlag)
        {
          stringBuilder.Append('\u001B');
          stringBuilder.Append("W1");
          byte[] numArray = IPConfig.IPString2Array(netconfig.IP);
          stringBuilder.Append(string.Format("{0:D3}{1:D3}{2:D3}{3:D3},", (object) numArray[0], (object) numArray[1], (object) numArray[2], (object) numArray[3]));
        }
        stringBuilder.Append('\u001B');
        stringBuilder.Append("W2");
        byte[] numArray1 = IPConfig.IPString2Array(netconfig.Subnet);
        stringBuilder.Append(string.Format("{0:D3}{1:D3}{2:D3}{3:D3},", (object) numArray1[0], (object) numArray1[1], (object) numArray1[2], (object) numArray1[3]));
        stringBuilder.Append('\u001B');
        stringBuilder.Append("W3");
        byte[] numArray2 = IPConfig.IPString2Array(netconfig.Gateway);
        stringBuilder.Append(string.Format("{0:D3}{1:D3}{2:D3}{3:D3},", (object) numArray2[0], (object) numArray2[1], (object) numArray2[2], (object) numArray2[3]));
      }
      byte[] numArray3 = IPConfig.MACString2Array(netconfig.MAC);
      stringBuilder.Append(string.Format("{0:X2}{1:X2}{2:X2}{3:X2}{4:X2}{5:X2}", (object) numArray3[0], (object) numArray3[1], (object) numArray3[2], (object) numArray3[3], (object) numArray3[4], (object) numArray3[5]));
      return Encoding.ASCII.GetBytes(stringBuilder.ToString());
    }

    private bool IsGLModel => this.curPrinter.IF.Equals("GL");

    private bool IsSilexModel => this.curPrinter.IF.Equals("Silex");

    private bool IsSATOModel => this.curPrinter.IF.Equals("SATO");

    private bool AssignIPSilex(ref IPConfig netconfig)
    {
      IPEndPoint remoteEP = new IPEndPoint(IPAddress.Broadcast, 19541);
      this.Silex_ChangeData0(ref netconfig);
      using (UdpClient udpClient = new UdpClient())
      {
        udpClient.EnableBroadcast = true;
        udpClient.Client.ReceiveTimeout = 300;
        udpClient.Send(UDPNetCfg.AssignIP1, UDPNetCfg.AssignIP1.Length, remoteEP);
        int num1 = 3;
        int num2 = 0;
        while (num1-- > 0)
        {
          byte[] numArray = udpClient.Receive(ref remoteEP);
          if (numArray != null && numArray.Length != 0)
          {
            if (numArray[0] == (byte) 0 && numArray[1] == (byte) 1)
            {
              if (num2 == 0)
                udpClient.Send(UDPNetCfg.AssignIP2, UDPNetCfg.AssignIP2.Length, remoteEP);
              udpClient.Close();
              return true;
            }
            udpClient.Close();
            return false;
          }
          udpClient.Send(UDPNetCfg.AssignIP2, UDPNetCfg.AssignIP2.Length, remoteEP);
          ++num2;
        }
        udpClient.Close();
      }
      return false;
    }

    private bool AssignSilexNetCfg(ref IPConfig netconfig)
    {
      this.Silex_ChangeData(ref netconfig);
      IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
      using (UdpClient udpClient = new UdpClient(this.curPrinter.IP, 19541))
      {
        udpClient.Client.ReceiveTimeout = 5000;
        udpClient.Send(this.SetEEPData1, this.SetEEPData1.Length);
        if (udpClient.Receive(ref remoteEP) != null)
        {
          udpClient.Send(this.SetEEPData2, this.SetEEPData2.Length);
          if (udpClient.Receive(ref remoteEP) != null)
          {
            udpClient.Send(UDPNetCfg.SetEEPData3_End, UDPNetCfg.SetEEPData3_End.Length);
            udpClient.Receive(ref remoteEP);
            return true;
          }
        }
        udpClient.Close();
      }
      return false;
    }

    private void Silex_ChangeData0(ref IPConfig netconfig)
    {
      byte[] sourceArray1 = IPConfig.IPString2Array(netconfig.IP);
      byte[] sourceArray2 = IPConfig.MACString2Array(netconfig.MAC);
      int destinationIndex1 = 18;
      Array.Copy((Array) sourceArray2, 0, (Array) UDPNetCfg.AssignIP1, destinationIndex1, 6);
      Array.Copy((Array) sourceArray2, 0, (Array) UDPNetCfg.AssignIP2, destinationIndex1, 6);
      int destinationIndex2 = 24;
      Array.Copy((Array) sourceArray1, 0, (Array) UDPNetCfg.AssignIP1, destinationIndex2, 4);
      Array.Copy((Array) sourceArray1, 0, (Array) UDPNetCfg.AssignIP2, destinationIndex2, 4);
    }

    private void Silex_ChangeData(ref IPConfig netconfig)
    {
      this.SetEEPData1.Initialize();
      Array.Copy((Array) UDPNetCfg.SetEEPData1_fix, (Array) this.SetEEPData1, UDPNetCfg.SetEEPData1_fix.Length);
      Array.Copy((Array) this.EEPDataIn1, 8, (Array) this.SetEEPData1, UDPNetCfg.SetEEPData1_fix.Length, this.SetEEPData1.Length - UDPNetCfg.SetEEPData1_fix.Length);
      this.SetEEPData2.Initialize();
      Array.Copy((Array) UDPNetCfg.SetEEPData2_fix, (Array) this.SetEEPData2, UDPNetCfg.SetEEPData2_fix.Length);
      Array.Copy((Array) this.EEPDataIn2, 8, (Array) this.SetEEPData2, 178, this.SetEEPData1.Length - 178);
      byte[] sourceArray1 = IPConfig.IPString2Array(netconfig.IP);
      byte[] sourceArray2 = IPConfig.IPString2Array(netconfig.Subnet);
      byte[] sourceArray3 = IPConfig.IPString2Array(netconfig.Gateway);
      byte[] sourceArray4 = IPConfig.MACString2Array(netconfig.MAC);
      int destinationIndex = 214;
      this.SetEEPData2[destinationIndex - 3] = netconfig.RARPFlag ? (byte) 0 : (byte) 1;
      this.SetEEPData2[destinationIndex - 2] = netconfig.DHCPFlag ? (byte) 0 : (byte) 1;
      Array.Copy((Array) sourceArray1, 0, (Array) this.SetEEPData2, destinationIndex, 4);
      Array.Copy((Array) sourceArray2, 0, (Array) this.SetEEPData2, destinationIndex + 4, 4);
      Array.Copy((Array) sourceArray3, 0, (Array) this.SetEEPData2, destinationIndex + 8, 4);
      Array.Copy((Array) sourceArray4, 0, (Array) this.RebootPrinterData, UDPNetCfg.RebootPrinterData_fix.Length, 6);
      Array.Copy((Array) sourceArray1, 0, (Array) this.RebootPrinterData, UDPNetCfg.RebootPrinterData_fix.Length + 6, 4);
    }

    private IPConfig Silex_ExtractData()
    {
      IPConfig data = new IPConfig();
      data.IsWLan = false;
      if (this.EEPDataIn1 != null && this.EEPDataIn2 != null)
      {
        byte[] numArray1 = new byte[4];
        byte[] numArray2 = new byte[6];
        int sourceIndex = 44;
        data.RARPFlag = this.EEPDataIn2[sourceIndex - 3] == (byte) 0;
        data.DHCPFlag = this.EEPDataIn2[sourceIndex - 2] == (byte) 0;
        Array.Copy((Array) this.EEPDataIn2, sourceIndex, (Array) numArray1, 0, 4);
        data.IP = IPConfig.IPArray2String(numArray1);
        Array.Copy((Array) this.EEPDataIn2, sourceIndex + 4, (Array) numArray1, 0, 4);
        data.Subnet = IPConfig.IPArray2String(numArray1);
        Array.Copy((Array) this.EEPDataIn2, sourceIndex + 8, (Array) numArray1, 0, 4);
        data.Gateway = IPConfig.IPArray2String(numArray1);
        Array.Copy((Array) this.EEPDataIn1, 10, (Array) numArray2, 0, 6);
        data.MAC = IPConfig.MACArray2String(numArray2, '-');
        data.WSSID = Encoding.ASCII.GetString(this.EEPDataInW, 24, 32).Trim(char.MinValue, ' ');
        data.WCh = (int) this.EEPDataInW[9];
        data.IsWLan = data.WCh > 0 && data.WCh < 20 && !string.IsNullOrEmpty(data.WSSID);
      }
      return data;
    }

    public bool RebootGLPrinter()
    {
      if (this.curPrinter.IP.Equals("0.0.0.0"))
        return false;
      using (TcpClient tcpClient = new TcpClient())
      {
        tcpClient.Connect(this.curPrinter.IP, 3001);
        byte[] buffer = new byte[1]{ (byte) 16 };
        if (tcpClient.Connected)
        {
          tcpClient.GetStream().Write(buffer, 0, 1);
          tcpClient.Close();
          return true;
        }
      }
      return false;
    }

    public bool RebootSilexPrinter()
    {
      if (this.curPrinter.IP.Equals("0.0.0.0"))
        return false;
      using (TcpClient tcpClient = new TcpClient())
      {
        try
        {
          tcpClient.Connect(this.curPrinter.IP, 9100);
          byte[] buffer1 = new byte[7]
          {
            (byte) 27,
            (byte) 65,
            (byte) 27,
            (byte) 68,
            (byte) 76,
            (byte) 27,
            (byte) 90
          };
          byte[] buffer2 = new byte[8]
          {
            (byte) 27,
            (byte) 65,
            (byte) 27,
            (byte) 82,
            (byte) 83,
            (byte) 48,
            (byte) 27,
            (byte) 90
          };
          if (tcpClient.Connected)
          {
            NetworkStream stream = tcpClient.GetStream();
            stream.Write(buffer1, 0, buffer1.Length);
            Thread.Sleep(6000);
            stream.Write(buffer2, 0, buffer2.Length);
            stream.Flush();
            stream.Close();
            tcpClient.Close();
            return true;
          }
        }
        catch
        {
        }
      }
      return false;
    }

    public IPConfig RetriveSATONetCfg() => this.curPrinter;

    public static string UDPStatusCheck(string ip, int port)
    {
      Socket socket = (Socket) null;
      string str = "";
      try
      {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        IPEndPoint remoteEP1 = new IPEndPoint(IPAddress.Parse(ip), port);
        byte[] bytes = Encoding.ASCII.GetBytes(("" + "\0\u0001\0\u0001" + "GET STATUS:prn1").ToString());
        byte[] numArray1 = new byte[510];
        Array.Copy((Array) bytes, (Array) numArray1, bytes.Length);
        socket.SendTo(numArray1, (EndPoint) remoteEP1);
        byte[] numArray2 = new byte[128];
        EndPoint remoteEP2 = (EndPoint) new IPEndPoint(IPAddress.Parse(ip), port);
        socket.ReceiveTimeout = 1000;
        int from = socket.ReceiveFrom(numArray2, ref remoteEP2);
        if (from > 10)
        {
          str = Encoding.ASCII.GetString(numArray2, 10, from - 10);
          int length = str.IndexOf("\r\n");
          str = str.Substring(0, length);
        }
      }
      catch
      {
        str = "Not connected";
      }
      finally
      {
        if (socket != null)
        {
          if (socket.Connected)
            socket.Disconnect(false);
          socket.Close();
        }
      }
      return str;
    }
  }
}
