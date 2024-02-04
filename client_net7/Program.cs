using System;
using System.Text;

class Program
{
    static byte[] GenerateAESKey()
    {
        // 获取当前时间
        DateTime now = DateTime.Now;

        // 格式化时间到分钟，用作密钥的一部分
        string keyStr = now.ToString("yyyyMMddHHmm");

        // 确保密钥字符串长度为16
        while (keyStr.Length < 16)
        {
            keyStr += keyStr;
        }
        keyStr = keyStr.Substring(0, 16);

        // 将字符串转换为byte[]数组
        byte[] aesKey = Encoding.UTF8.GetBytes(keyStr);

        return aesKey;
    }

    static void Main(string[] args)
    {
        byte[] aesKey = GenerateAESKey();

        Console.Write("Generated AES Key: ");
        foreach (var b in aesKey)
        {
            Console.Write($"{b:X2}"); // 以十六进制格式打印
        }
        Console.WriteLine();
    }
}
