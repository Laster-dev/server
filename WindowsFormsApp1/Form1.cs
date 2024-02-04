using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using AcrylicUI.Controls;
using System.IO;
using System.Security.Cryptography;
using WindowsFormsApp1.Properties;
using System.Threading;
using Timer = System.Windows.Forms.Timer;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;

namespace WindowsFormsApp1
{

    public partial class Form1 : Form//AcrylicUI.Forms.AcrylicForm
    {
        private Timer blinkTimer;
        private bool isIconVisible = true;
        Icon TransparentIcon;
        [DllImport("DwmApi")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, int[] attrValue, int attrSize);

        public static void DarkThemeTitleBar(IntPtr hwnd)
        {
            if (DwmSetWindowAttribute(hwnd, 19, new[] { 1 }, 4) != 0)
            {
                DwmSetWindowAttribute(hwnd, 20, new[] { 1 }, 4);
            }
        }
        public Form1()
        {
            InitializeComponent();
            notifyIcon1.ContextMenuStrip = acrylicContextMenu1;
            notifyIcon1.Icon = this.Icon;
            notifyIcon1.Visible = true; // 显示托盘图标
            // 初始化Timer
            blinkTimer = new Timer();
            blinkTimer.Interval = 500; // 设置闪烁间隔为500毫秒
            blinkTimer.Tick += BlinkTimer_Tick;
            TransparentIcon = CreateTransparentIcon();
            
            ExtendFrame();
            DarkThemeTitleBar(this.Handle);


        }
        // 闪烁逻辑
        private Icon CreateTransparentIcon()
        {
            using (Bitmap transparentBitmap = new Bitmap(16, 16))
            {
                transparentBitmap.MakeTransparent(); // 将位图设置为透明
                using (Graphics graphics = Graphics.FromImage(transparentBitmap))
                {
                    graphics.Clear(Color.Transparent); // 使用透明颜色清除位图
                }
                // 从透明位图创建图标
                IntPtr hIcon = transparentBitmap.GetHicon();
                Icon transparentIcon = Icon.FromHandle(hIcon);
                return transparentIcon;
            }
        }

        private void BlinkTimer_Tick(object sender, EventArgs e)
        {
            isIconVisible = !isIconVisible;
            // 切换NotifyIcon的可见性
            if (isIconVisible)
            {
                notifyIcon1.Icon = TransparentIcon;
            }
            else
            {
                notifyIcon1.Icon = Icon;
            }


        }

        // 开始闪烁
        public void StartBlinking()
        {
            isIconVisible = true; // 确保从可见状态开始
            blinkTimer.Start();
        }

        // 停止闪烁
        public void StopBlinking()
        {
            blinkTimer.Stop();
            notifyIcon1.Icon = this.Icon;
        }
        bool bDisposed = false;
        private TcpListener listener;
        private CancellationTokenSource cancellationTokenSource;
        private int port;
        byte[] key = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10 };
        byte[] iv = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10 };

        public void Start()
        {

            port = int.Parse(Program.form1.toolStripTextBox1.Text);
            cancellationTokenSource = new CancellationTokenSource();
            StartBlinking();
            Task.Run(() => ListenForClients(cancellationTokenSource.Token), cancellationTokenSource.Token);
        }

        public void Stop()
        {
            cancellationTokenSource?.Cancel();
            listener?.Stop();
            StopBlinking();
            Addmsg("[ - ]监听服务已关闭", Color.Red);
        }

        private void ListenForClients(CancellationToken token)
        {
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            Addmsg($"[ * ]等待客户端在端口 {port} 上连接...\n", Color.Green);

            try
            {
                while (!token.IsCancellationRequested)
                {
                    if (listener.Pending())
                    {
                        TcpClient client = listener.AcceptTcpClient();
                        Task.Run(() => HandleClient(client, token), token);
                    }
                    else
                    {
                        Task.Delay(10).Wait();
                    }
                }
            }
            catch (Exception ex)
            {
                Addmsg($"[ - ]发生错误: {ex.Message}", Color.Red);
            }
            finally
            {
                listener.Stop();
            }
        }

        private void HandleClient(TcpClient client, CancellationToken token)
        {
            try
            {
                SendFile(client, File.ReadAllBytes(this.Text), key, iv);
                client.Close();
                Addmsg("[ + ]文件发送完成！", Color.Green);
            }
            catch (Exception ex)
            {
                Addmsg($"[ - ]处理客户端时发生错误: {ex.Message}", Color.Red);
            }
        }

        public static void Addmsg(string Msg)
        {
            Color c = Color.FromArgb(255, 220, 220, 220);
            Addmsg(Msg, c);
        }
        public static void Addmsg(string Msg, Color color)
        {
            var lv = new AcrylicListItem(DateTime.Now.ToString("hh:mm:ss") + "      " + Msg);
            lv.TextColor = color;
            try
            {
                if (Program.form1.InvokeRequired)
                {
                    _ = Program.form1.Invoke((MethodInvoker)(() =>
                    {
                        Program.form1.acrylicListView1.Items.Insert(0, lv);
                    }));
                }
                else
                {
                    Program.form1.acrylicListView1.Items.Insert(0, lv);
                }

            }
            catch { }
        }
        public void SendFile(TcpClient client, byte[] fileData, byte[] key, byte[] iv)
        {

            NetworkStream netStream = client.GetStream();

            byte[] encryptedData = AesEncryption.Encrypt(fileData, key, iv);

            netStream.Write(encryptedData, 0, encryptedData.Length);
            IPEndPoint remoteEndPoint = client.Client.RemoteEndPoint as IPEndPoint;
            if (remoteEndPoint != null)
            {
                string remoteAddress = remoteEndPoint.Address.ToString();
                Addmsg($"[ + ]客户端{remoteAddress} 已连接...");
            }


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
                this.Text = file;
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

        private void 开启ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            acrylicListView1.ShowIcons = true;
            if (bDisposed == false)
            {
                开启ToolStripMenuItem.Text = "Stop";
                toolStripTextBox1.Enabled = false;
                bDisposed = true;
                Start();
            }
            else
            {
                开启ToolStripMenuItem.Text = "Start";
                toolStripTextBox1.Enabled = true;
                bDisposed = false;
                Stop();

            }

        }

        private void cleanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            acrylicListView1.Items.Clear();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show(); // 显示窗体
            this.WindowState = FormWindowState.Normal; // 恢复窗体的正常状态
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true; // 取消关闭操作
                this.Hide(); // 隐藏窗体
            }
            else
            {
                base.OnFormClosing(e);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stop();
            GC.Collect();
            Process.GetCurrentProcess().Kill();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            GC.Collect();
            Addmsg($"[ + ]内存垃圾已清理", Color.Honeydew);
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            acrylicListView1.BackColor = Color.FromArgb(43, 43, 43);
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            acrylicListView1.BackColor = Color.FromArgb(0, 0, 0);
        }

        // 定义边距结构
        public struct MARGINS{   
            public int leftWidth;  
            public int rightWidth;  
            public int topHeight;   
            public int bottomHeight;
        }
        [DllImport("dwmapi.dll")] 
        public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);
        private void ExtendFrame(){
            MARGINS margins = new MARGINS()
            {
                leftWidth = 0,
                rightWidth = 0,
                topHeight = 20,
                bottomHeight = 0,

            };  
            DwmExtendFrameIntoClientArea(this.Handle, ref margins);
        }
        private void debugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            // AcrylicUI.Resources.Colors.
            DarkThemeTitleBar(this.Handle);
        }

        private void acrylicContextMenu1_Opening(object sender, CancelEventArgs e)
        {

        }
    }
}
