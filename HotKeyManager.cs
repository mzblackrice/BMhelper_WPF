using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BMhelper_WPF
{
    public class HotKeyManager
    {
        // 声明API函数
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        // 常量
        private const int MOD_CONTROL = 0x0002;
        private const int VK_NUMPAD1 = 0x61;

        // 公共静态属性
        public static int HOTKEY_ID { get; private set; } = 9000;

        // 注册热键
        public static void RegisterHotKey(IntPtr hWnd)
        {
            if (!RegisterHotKey(hWnd, HOTKEY_ID, MOD_CONTROL, VK_NUMPAD1))
            {
                Rtb.EchoInfo("快捷键注册失败");
            }
            else
            {
                Rtb.EchoInfo("快捷键注册成功");
            }
        }

        // 注销热键
        public static void UnregisterHotKey(IntPtr hWnd)
        {
            UnregisterHotKey(hWnd, HOTKEY_ID);
            Rtb.EchoInfo("快捷键已注销");
        }
    }
}
