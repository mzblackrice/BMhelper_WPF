using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BMhelper_WPF
{
    class BMhelper
    {
        public class BMain
        {
            public static void MainWork()
            {
                RegDm();                
                
            }
            //注册大漠
            public static void RegDm()
            {
                int dm_ret = GlobalVar.dm.Reg("xf30557fc317f617eead33dfc8de3bdd4ab9043", "xh34e44xok7sgp7");
                if (dm_ret == 1)
                {
                    Rtb.EchoInfo($"注册成功，当前版本：{GlobalVar.dm.Ver()}");
                }
                else
                {
                    Rtb.EchoInfo($"注册失败，错误代码：{dm_ret}");
                }
            }
            //查询窗口
            public static List<string> FindWnd()
            {
                List<string> playerNames = new List<string>(); // 使用动态大小的列表来存储玩家名称
                string Hwnds = GlobalVar.dm.EnumWindow(0, "BackMir", "HGE__WNDCLASS", 1 + 2 + 4 + 8 + 16);
                if (Hwnds.Length > 0)
                {
                    string[] Hwnd = Hwnds.Split(',');
                    foreach (string s in Hwnd)
                    {
                        string title = GlobalVar.dm.GetWindowTitle(Convert.ToInt32(s));
                        string[] titles = title.Split('-');
                        if (titles.Length > 1) // 确保标题分割后有足够的元素
                        {
                            //playerName = titles[2];
                            playerNames.Add(titles[2]); // 将玩家名称添加到列表中
                        }
                        //GlobalVar.GameHwnd = Convert.ToInt32(s);
                        //Rtb.EchoInfo($"找到游戏窗口：{title}");
                        //Rtb.EchoInfo($"游戏窗口句柄：{s}");
                        //Rtb.EchoInfo($"角色名字：{playerName}");                        
                    }
                    return playerNames;
                }
                else
                {
                    Rtb.EchoInfo($"没找到游戏窗口");
                    return playerNames;
                }
            }
            //读写内存
            public static long ReadMem(string addr ,string datetype) 
            {
                /*
                type 整形数: 整数类型,取值如下
                0 : 32位有符号
                1 : 16 位有符号
                2 : 8位有符号
                3 : 64位
                4 : 32位无符号
                5 : 16位无符号
                6 : 8位无符号
                */
                int tpe;
                switch (datetype)
                {
                    case "+4":
                        tpe = 0; break;
                    case "+2":
                        tpe = 1; break;
                    case "+1":
                        tpe = 2; break;
                    case "8":
                        tpe = 3; break;
                    case "4":
                        tpe = 4; break;
                    case "2":
                        tpe = 5; break;
                    case "1":
                        tpe = 6; break;
                    default:
                        tpe = 0;
                        break;
                }
                long value = GlobalVar.dm.ReadInt(GlobalVar.GameHwnd, addr, tpe);
                return value;
            }
            public static string ReadName(string addr)
            {
                int addressInt = Convert.ToInt32(addr, 16);
                int addrName = addressInt + 0x6C;
                string value = GlobalVar.dm.ReadString(GlobalVar.GameHwnd, addrName.ToString("X"), 0, 0);
                return value;
            }
            public static string FindMonster(string MonsterName) 
            {                   
                string FormatCode(string code)
                {
                    // 去除特征码字符串中的空格
                    code = code.Replace(" ", "");

                    // 将字符串按照每两个字符一组进行分割，并反转顺序
                    string[] byteStrings = new string[code.Length / 2];
                    for (int i = 0; i < byteStrings.Length; i++)
                    {
                        byteStrings[i] = code.Substring(i * 2, 2);
                    }
                    Array.Reverse(byteStrings);
                    // 将分割后的字符串数组拼接成新的字符串
                    string formattedCode = string.Join(" ", byteStrings);
                    return formattedCode;
                }
                if (ZD.Codes.ContainsKey(MonsterName))
                {
                    string code = ZD.Codes[MonsterName];
                    string formattedCode = FormatCode(code);
                    return GlobalVar.dm.FindDataEx(GlobalVar.GameHwnd, "00000000-FFFFFFFF", formattedCode, 4, 1, 1);
                }
                else
                {
                    return "";
                }                
            }
            public static void WriteMen(string addr, string datetype ,int data)
            {
                /*
                type 整形数: 整数类型,取值如下
                0 : 32位
                1 : 16 位
                2 : 8位
                3 : 64位
                */
                int tpe;
                switch (datetype)
                {  
                    case "4":
                        tpe = 0; break;
                    case "2":
                        tpe = 1; break;
                    case "1":
                        tpe = 2; break;
                    case "8":
                        tpe = 3; break;
                    default:
                        tpe = 0;
                        break;
                }
                long value = GlobalVar.dm.WriteInt(GlobalVar.GameHwnd, addr, tpe ,data);

            }
            //委托任务
            public static async void DoWork()
            {
                if (GlobalVar.HotKeyAction != null)
                {
                    await Task.Run(() => GlobalVar.HotKeyAction()); // 在后台线程上执行委托    
                }
            }
            //洗装备 升星 洗练
            public static void WashItem()
            {
                OpenWnd("包裹");
                OpenWnd("洗练");
                Thread.Sleep(GlobalVar.DelyXXS);
                PickItem(7,9);
                Thread.Sleep(GlobalVar.DelyXXS);
                PutItem("洗练",0);
                Thread.Sleep(GlobalVar.DelyXXS);
                PickItem(7, 8);
                Thread.Sleep(GlobalVar.DelyXXS);
                PutItem("洗练",1);
                Thread.Sleep(GlobalVar.DelyXXS);
                ClickEnter("洗练");
                Thread.Sleep(GlobalVar.DelyXXS);
                PickItem(7, 9 , 2);
            }
            public static string FlyToHit( string MonsterName)
            {
                string monsterAddr = FindMonster(MonsterName);
                if (monsterAddr.Length >0 ) 
                {
                    string[] addrs = monsterAddr.Split('|');
                    foreach (string s in addrs)
                    {
                        if (ReadName(s) == MonsterName)
                        {
                            int address = Convert.ToInt32(s,16);
                            int Health =(int) BMain.ReadMem($"{(address + 0xA8).ToString("X")}", "4");
                            if (Health > 0)
                            {
                                int X = (int)BMain.ReadMem($"{(address + 0x4C).ToString("X")}", "4");
                                int Y = (int)BMain.ReadMem($"{(address + 0x50).ToString("X")}", "4");
                                SendKey($"@move {X},{Y}");
                                //Rtb.EchoInfo($"{ReadName(s)}：坐标{(X,Y)},血量{Health}");
                                Thread.Sleep(GlobalVar.DelyXS);
                                GlobalVar.PlayerStatus.SetAttackTarget(Convert.ToInt64(s, 16));
                                return s;
                            }                            
                        }
                    }
                }
                return null;
            }
            //打开关闭功能窗口
            public static void OpenWnd(string WndName)
            {                
                switch (WndName)
                {
                    case "地图":
                        if (!WndInfo.IsWndOpen(WndName))
                        {
                            SendKey("Tab");
                        }
                        break;
                    case "商城":
                        if (!WndInfo.IsWndOpen(WndName))
                        {
                            SendKey("S");
                        }
                        break;
                    case "包裹":
                        if (!WndInfo.IsWndOpen(WndName))
                        {
                            SendKey("B");
                        }
                        break;
                    case "分解":
                        if (!WndInfo.IsWndOpen(WndName))
                        {
                            SendKey("M");
                            Thread.Sleep(GlobalVar.DelyS);
                        }                        
                        if (WndInfo.IsTagOpen(WndName) != 0)
                        {
                            (int fixx, int fixy) = WndInfo.GetFixXY();
                            (int x, int y) = WndInfo.GetWndXY(WndName);
                            GlobalVar.dm.MoveTo(58  + x + fixx, 62 + y + fixy);
                            GlobalVar.dm.LeftClick();
                        }
                        break;
                    case "洗练":
                        if (!WndInfo.IsWndOpen(WndName))
                        {
                            SendKey("M");
                            Thread.Sleep(GlobalVar.DelyS);
                        }                        
                        if (WndInfo.IsTagOpen(WndName) != 1)
                        {
                            (int fixx, int fixy) = WndInfo.GetFixXY();
                            (int x, int y) = WndInfo.GetWndXY(WndName);
                            GlobalVar.dm.MoveTo(58 + 70 + x + fixx, 62 + y + fixy);
                            GlobalVar.dm.LeftClick();
                        }
                        break;
                    case "手工":
                        if (!WndInfo.IsWndOpen(WndName))
                        {
                            if (!WndInfo.IsWndOpen("分解"))
                            {
                                SendKey("M");
                                Thread.Sleep(GlobalVar.DelyS);
                            }
                            (int x, int y) = WndInfo.GetWndXY("分解");
                            (int fixx, int fixy) = WndInfo.GetFixXY();
                            GlobalVar.dm.MoveTo(58 + 140 + x + fixx, 62 + y + fixy);
                            GlobalVar.dm.LeftClick();
                            Thread.Sleep(GlobalVar.DelyS);
                            
                        }
                        if (WndInfo.IsTagOpen(WndName) !=1)
                        {
                            (int fixx, int fixy) = WndInfo.GetFixXY();
                            (int x,int  y) = WndInfo.GetWndXY(WndName);
                            GlobalVar.dm.MoveTo(61 + 70 + x + fixx, 62 + y + fixy);
                            GlobalVar.dm.LeftClick();
                        }                        
                        break;
                    case "魔盒":
                        if (!WndInfo.IsWndOpen(WndName))
                        {
                            (int x ,int y ) = GlobalVar.PlayerBag.FindItemXY(WndName);
                            PickItem(x, y ,1);
                            WriteMen("[[<BackMir.exe>+00892364]+90]+8","4",350);
                            WriteMen("[[<BackMir.exe>+00892364]+90]+C", "4", 0);
                            WriteMen("[[<BackMir.exe>+00892364]+90]+10", "4", 650);
                            WriteMen("[[<BackMir.exe>+00892364]+90]+14", "4", 240);
                        }
                        break;
                    default:
                        break;
                }
                
            }
            //关闭窗口
            public static void CloseWnd(string WndName)
            {
                switch (WndName)
                {
                    case "地图":
                        if (WndInfo.IsWndOpen(WndName))
                        {
                            SendKey("Tab");
                        }
                        break;
                    case "商城":
                        if (WndInfo.IsWndOpen(WndName))
                        {
                            SendKey("S");
                        }
                        break;
                    case "包裹":
                        if (WndInfo.IsWndOpen(WndName))
                        {
                            SendKey("B");
                        }
                        break;
                    case "分解":
                        if (WndInfo.IsWndOpen(WndName))
                        {
                            SendKey("M");
                        }
                        break;
                    case "洗练":
                        if (WndInfo.IsWndOpen(WndName))
                        {
                            SendKey("M");
                        }
                        break;
                    case "手工":
                        if (WndInfo.IsWndOpen(WndName))
                        {
                            (int fixx, int fixy) = WndInfo.GetFixXY();
                            (int x, int y) = WndInfo.GetWndXY(WndName);
                            GlobalVar.dm.MoveTo(330 + x + fixx, 47 + y + fixy);
                            GlobalVar.dm.LeftClick();
                        }
                        break;
                    case "魔盒":
                        if (WndInfo.IsWndOpen(WndName))
                        {
                            (int fixx, int fixy) = WndInfo.GetFixXY();
                            (int x, int y) = WndInfo.GetWndXY(WndName);
                            GlobalVar.dm.MoveTo(272 + x + fixx, 47 + y + fixy);
                            GlobalVar.dm.LeftClick();
                        }
                        break;
                    default:
                        break;
                }

            }
            //发送按键
            public static void SendKey(string key) 
            {
                if (!GlobalVar.IsWndBinded)
                {
                    GlobalVar.dm.SetWindowState(GlobalVar.GameHwnd, 1);
                    Thread.Sleep(GlobalVar.DelyS);
                }
                if (key.Contains("@"))
                {
                    if (key.Contains("move"))
                    {
                        OpenWnd("地图");
                    }
                    GlobalVar.dm.KeyPressChar("enter");
                    Thread.Sleep(GlobalVar.DelyS);
                    GlobalVar.dm.SendString(GlobalVar.GameHwnd,key);
                    Thread.Sleep(GlobalVar.DelyS);
                    GlobalVar.dm.KeyPressChar("enter");
                    Thread.Sleep(GlobalVar.DelyS);
                    GlobalVar.dm.MoveTo(48, 687);
                    GlobalVar.dm.LeftClick();
                    CloseWnd("地图");
                }
                else 
                {
                    GlobalVar.dm.KeyPressChar(key);
                } 
            }
            //绑定窗口
            public static int BindWnd(int type)
            {
                if (type == 1)
                {
                    long WndState = GlobalVar.dm.GetWindowState((int)GlobalVar.GameHwnd, 0);
                    if (WndState == 1)
                    {
                        int dm_ret = GlobalVar.dm.BindWindowEx(GlobalVar.GameHwnd, "dx.graphic.2d",
                                "dx.mouse.position.lock.api|dx.mouse.position.lock.message|dx.mouse.clip.lock.api|dx.mouse.input.lock.api|dx.mouse.state.api|dx.mouse.api|dx.mouse.cursor",
                                "dx.keypad.input.lock.api|dx.keypad.state.api|dx.keypad.api",
                                "dx.public.active.api|dx.public.active.message", 0);
                        if (dm_ret != 0)
                        {
                            Rtb.EchoInfo($"窗口绑定成功");
                            GlobalVar.IsWndBinded = true;
                            return 1;
                        }
                        else
                        {
                            Rtb.EchoInfo($"窗口绑定失败");
                            GlobalVar.IsWndBinded = false;
                            return 0;
                        }
                    }
                    else 
                    {
                        Rtb.EchoInfo($"窗口不存在");
                        GlobalVar.IsWndBinded = false;
                        return 0;
                    }                    
                }
                else 
                {
                    GlobalVar.dm.UnBindWindow();
                    Rtb.EchoInfo($"窗口已解绑");
                    GlobalVar.IsWndBinded = false;
                    return 1;
                }
            }
            //点击包裹物品
            public static void PickItem(int x, int y , int type =0)
            {
                OpenWnd("包裹");
                Thread.Sleep(GlobalVar.DelyS);
                (int Wndx, int Wndy) = WndInfo.GetWndXY("包裹");
                (int fixx, int fixy) = WndInfo.GetFixXY();
                if (x != -1)
                {
                    GlobalVar.dm.MoveTo(35 + (x * 41) + Wndx + fixx, 97 + (y * 41) + Wndy + fixy);
                    Thread.Sleep(GlobalVar.DelyXS);
                    if (type == 0)
                    {
                        GlobalVar.dm.LeftClick();
                        //Thread.Sleep(GlobalVar.DelyXS);
                    }
                    else if (type == 1)
                    {
                        GlobalVar.dm.RightClick();
                        //Thread.Sleep(GlobalVar.DelyXS);
                    }                    
                }
            }
            public static void PutItem(string WndName, int location)
            {
                switch (WndName)
                {
                    case "分解":
                    case "洗练":
                        (int Wndx, int Wndy) = WndInfo.GetWndXY("洗练");
                        (int fixx, int fixy) = WndInfo.GetFixXY();
                        switch (location)
                        {
                            case 0:
                                GlobalVar.dm.MoveTo(151+Wndx+fixx, 282+Wndy+fixy);
                                Thread.Sleep((int)GlobalVar.DelyXS);
                                GlobalVar.dm.LeftClick();
                                break;
                            case 1:
                                GlobalVar.dm.MoveTo(119 + Wndx + fixx, 225 + Wndy + fixy);
                                Thread.Sleep((int)GlobalVar.DelyXS);
                                GlobalVar.dm.LeftClick();
                                break;
                            case 2:
                                GlobalVar.dm.MoveTo(93 + Wndx + fixx, 170 + Wndy + fixy);
                                Thread.Sleep((int)GlobalVar.DelyXS);
                                GlobalVar.dm.LeftClick();
                                break;
                            case 3:
                                GlobalVar.dm.MoveTo(148 + Wndx + fixx, 131 + Wndy + fixy);
                                Thread.Sleep((int)GlobalVar.DelyXS);
                                GlobalVar.dm.LeftClick();
                                break;
                            case 4:
                                GlobalVar.dm.MoveTo(213 + Wndx + fixx, 170 + Wndy + fixy);
                                Thread.Sleep((int)GlobalVar.DelyXS);
                                GlobalVar.dm.LeftClick();
                                break;
                            case 5:
                                GlobalVar.dm.MoveTo(187 + Wndx + fixx, 227 + Wndy + fixy);
                                Thread.Sleep((int)GlobalVar.DelyXS);
                                GlobalVar.dm.LeftClick();
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }
            }
            public static void ClickEnter(string WndName)
            {
                switch (WndName)
                {
                    case "分解":
                    case "洗练":
                        (int Wndx, int Wndy) = WndInfo.GetWndXY("洗练");
                        (int fixx, int fixy) = WndInfo.GetFixXY();                        
                        GlobalVar.dm.MoveTo(149 + Wndx + fixx, 390 + Wndy + fixy);
                        Thread.Sleep((int)GlobalVar.DelyXS);
                        GlobalVar.dm.LeftClick();
                        break;
                    default:
                        break;                    
                }
            }
        }

        
    }
}
