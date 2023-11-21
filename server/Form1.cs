using System.Net.Sockets;
using System.Net;
using System.Text;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;
using System.Security.Cryptography;
using System.Diagnostics;
using System.IO.Compression;
using System.Reflection;

namespace server
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            byte[] key = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10 };
            byte[] iv = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10 };

            TcpListener listener = new TcpListener(IPAddress.Any, 12345);
            listener.Start();
            textBox1.Invoke(new Action(() =>
            {
                // 在UI线程中访问TextBox1
                textBox1.Text += "等待客户端连接...\n";
            }));


            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                textBox1.Invoke(new Action(() =>
                {
                    // 在UI线程中访问TextBox1
                    textBox1.Text += "客户端已连接。\n";
                }));
                // 发送文件
                SendFile(client, textBox2.Text, key, iv);

                client.Close();
                textBox1.Invoke(new Action(() =>
                {
                    // 在UI线程中访问TextBox1
                    textBox1.Text += "客户端已断开连接。\n";
                }));
            }
        }
        public static void SendFile(TcpClient client, string filePath, byte[] key, byte[] iv)
        {
            NetworkStream netStream = client.GetStream();

            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            {
                byte[] fileData = new byte[fileStream.Length];
                fileStream.Read(fileData, 0, fileData.Length);

                byte[] encryptedData = AesEncryption.Encrypt(fileData, key, iv);

                netStream.Write(encryptedData, 0, encryptedData.Length);
            }

            Console.WriteLine("文件发送完成。");
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

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files)
            {
                // 在这里处理拖拽到控件上的文件
                // file 变量包含了拖拽到控件上的文件路径
                //MessageBox.Show("拖拽的文件路径：" + file);
                textBox2.Text = file;
            }
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void 生成器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("builder.exe");
        }
    }
}