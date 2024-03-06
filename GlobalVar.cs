using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BMhelper_WPF
{  
    public static class GlobalVar
    {
        //人物背包静态实例
        public static Package PlayerBag;
        //大漠静态实例
        public static dmsoft dm;
        //多档延迟
        public static int DelyXXS = 10;
        public static int DelyXS = 50;
        public static int DelyS = 200;
        //是否绑定判断值
        public static bool IsWndBinded { get; set; } 
        //日志控件变量
        public static RichTextBox RtbInfo { get; set; }  
        //人物状态
        public static Actor PlayerStatus { get; set; }
        //快捷键执行函数的变量
        public static Action HotKeyAction { get; set; }
        //人物基址
        public static long playerBasicAddr { get; set; }
        //游戏和辅助的窗口句柄
        public static int GameHwnd { get; set; }
        public static int GameFzHwnd { get; set; }
        // 应用程序路径
        public static string AppPath;
        public static string GlPath { get; set; }
        static GlobalVar()
        {
            // 获取 AppData\Roaming 文件夹的路径
            string roamingPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            // 拼接后续路径
            AppPath = System.IO.Path.Combine(roamingPath, @"Tencent\MiniBrowser\Storage\Legal\Files\Version\Data");
            // 在构造函数中初始化全局变量或实例
            PlayerBag = new Package();
            dm = new dmsoft();
            IsWndBinded = false;
            //设置大漠路径和字库
            dm.SetPath(AppPath);
            dm.SetDict(0, "dm_soft.txt");
        }

    }
    
}
