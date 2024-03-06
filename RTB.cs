using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using static BMhelper_WPF.BMhelper;

namespace BMhelper_WPF
{
    public static class Rtb
    {
        public static void EchoInfo(string message)
        {
            if (Application.Current.Dispatcher.CheckAccess())
            {
                // 当前线程是 UI 线程
                AppendText(message);
            }
            else
            {
                // 当前线程不是 UI 线程，需要通过 Dispatcher 调度到 UI 线程上执行
                Application.Current.Dispatcher.Invoke(() =>
                {
                    AppendText(message);
                });
            }
        }
        private static void AppendText(string message)
        {
            GlobalVar.RtbInfo.AppendText($"[{DateTime.Now.ToString("HH:mm:ss")}]  {message}\n");

            // 检查行数是否超过 99 行
            if (GlobalVar.RtbInfo.Document.Blocks.Count > 99)
            {
                // 删除最上面的行
                for (int i = 0; i < 2; i++) // 每个时间戳+消息为一行，所以这里删除两行
                {
                    GlobalVar.RtbInfo.Document.Blocks.Remove(GlobalVar.RtbInfo.Document.Blocks.FirstBlock);
                }
            }
            GlobalVar.RtbInfo.ScrollToEnd();
        }

    }
}
