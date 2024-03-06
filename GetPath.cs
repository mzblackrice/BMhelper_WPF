using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace BMhelper_WPF
{
    public static class GetPath
    {
        // 获取指定窗口句柄对应的进程路径
        public static string GetPathFromHwnd(int WndHwnd)
        {
            IntPtr hWnd = (IntPtr)WndHwnd;
            uint processId;
            NativeMethods.GetWindowThreadProcessId(hWnd, out processId);
            Process process = Process.GetProcessById((int)processId);
            string processPath = process.MainModule.FileName;
            // 获取程序路径的上级目录
            string parentFolderPath = Path.GetDirectoryName(processPath);
            // 拼接 "过滤" 文件夹的路径
            string filteredFolderPath = Path.Combine(parentFolderPath, "过滤");
            return filteredFolderPath;
        }
        // 列举指定文件夹内部的所有文件，并输出完整文件名
        public static string[] ListFilesInFolder(int WndHwnd)
        {            
            string folderPath = GetPathFromHwnd(WndHwnd);
            if (Directory.Exists(folderPath))
            {
                string[] files = Directory.GetFiles(folderPath);
                return files;
                                
            }
            else
            {
                return new string[0];
            }
        }
        public static string PrintFilesInFolder(int WndHwnd)
        {
            string[] fileList = ListFilesInFolder(WndHwnd);
            if (fileList.Length>0)
            {
                string result = "";
                foreach (string file in fileList)
                {
                    result += file + Environment.NewLine;
                }
                return result;
            }
            else
            {
                return "文件夹不存在：";
            }
        }
        internal static class NativeMethods
        {
            // 导入 Windows API 函数
            [System.Runtime.InteropServices.DllImport("user32.dll")]
            internal static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);
        }
    }
}
