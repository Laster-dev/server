using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class Settings
    {
        public static List<string> C { get; set; } = new List<string>(); // 初始化名单
        public static bool B_or_W { get; set; } //黑白名单
    }
}
