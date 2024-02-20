using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{

    public partial class InputBox : Form
    {
        public string UserInput { get; private set; } // 添加一个公共属性来存储用户输入
        public InputBox(string 标题,string 描述,string 默认)
        {
            InitializeComponent();
            DarkThemeTitleBar(this.Handle);
            ExtendFrame();
            this.Text = 标题;
            this.acrylicLabel1.Text = 描述;
            this.acrylicTextBox1.Text = 默认;
        }

        private void acrylicButton1_Click(object sender, EventArgs e)
        {
            UserInput = acrylicTextBox1.Text;
            this.DialogResult = DialogResult.OK; // 设置对话框结果为OK
            this.Close();
        }

        private void acrylicButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        // 定义边距结构
        public struct MARGINS
        {
            public int leftWidth;
            public int rightWidth;
            public int topHeight;
            public int bottomHeight;
        }
        [DllImport("dwmapi.dll")]
        public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);
        private void ExtendFrame()
        {
            MARGINS margins = new MARGINS()
            {
                leftWidth = 0,
                rightWidth = 0,
                topHeight = 200000000,
                bottomHeight = 0,

            };
            DwmExtendFrameIntoClientArea(this.Handle, ref margins);
        }
        [DllImport("DwmApi")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, int[] attrValue, int attrSize);

        public static void DarkThemeTitleBar(IntPtr hwnd)
        {
            if (DwmSetWindowAttribute(hwnd, 19, new[] { 1 }, 4) != 0)
            {
                DwmSetWindowAttribute(hwnd, 20, new[] { 1 }, 4);
            }
        }
    }
}
