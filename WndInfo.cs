using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BMhelper_WPF.BMhelper;

namespace BMhelper_WPF
{
    public class WndInfo
    {
        private static Dictionary<string, int> windowOffsets; //地图偏移字典

        static WndInfo()
        {
            windowOffsets = new Dictionary<string, int>
            {
                { "地图", 0x7c },
                { "包裹", 0x8c },
                { "分解", 0xA8 },
                { "洗练", 0xA8 },
                { "手工", 0xEC },
                { "商店", 0xB4 },
                { "商城", 0xD4},
                { "交易确认", 0xCC },
                { "对话框", 0x64 },
                { "魔盒", 0x90 },
                { "复活", 0x74 }
            };
        }

        public static bool IsWndOpen(string WndName)
        {
            int windowOffset = GetWndoffset(WndName);
            string addr = $"[[<BackMir.exe>+00892364]+{windowOffset.ToString("X")}]+4";
            return BMain.ReadMem(addr, "1") == 1;
        }
        public static int IsTagOpen(string WndName)
        {
            int windowOffset = GetWndoffset(WndName);
            string addr = $"[[<BackMir.exe>+00892364]+{windowOffset.ToString("X")}]+E8";
            return (int)BMain.ReadMem(addr, "1");
        }

        public static (int, int) GetWndXY(string WndName)
        {
            int windowOffset = GetWndoffset(WndName);
            string addr_x = $"[[<BackMir.exe>+00892364]+{windowOffset.ToString("X")}]+8";
            string addr_y = $"[[<BackMir.exe>+00892364]+{windowOffset.ToString("X")}]+c";
            int x = (int)BMain.ReadMem(addr_x,"4");
            int y = (int)BMain.ReadMem(addr_y,"4");
            return (x, y);
        }
        public static (int, int) GetFixXY()
        {
            GlobalVar.dm.GetWindowRect(GlobalVar.GameHwnd, out int x1, out int y1, out int x2, out int y2);
            int x = x1 +3;
            int y = y1 + 30;
            if (GlobalVar.IsWndBinded)
            {
                x = 0;
                y = 0;
            }
            return (x, y);
        }
        private static int GetWndoffset(string WndName)
        {
            if (windowOffsets.ContainsKey(WndName))
            {
                return windowOffsets[WndName];
            }
            else
            {
                throw new ArgumentException("窗口名称无效");
            }
        } 
    }
}
