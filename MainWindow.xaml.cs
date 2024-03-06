using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using static BMhelper_WPF.BMhelper;

namespace BMhelper_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainViewModel ViewModel { get; set; }
        private HwndSource hwndSource;

        //程序主入口
        public MainWindow()
        {
            InitializeComponent();//初始化绘制UI 
            GlobalVar.RtbInfo = RtbInfo; // 设置 GlobalVar 中的 RtbInfo
            BMain.MainWork(); // 调用 BMhelper.cs 文件中的方法
            btnfindwnd();//程序启动自动查询一次窗口
            InitializeTimer();//初始化定时器
            InitializeValuet();//初始化UIvalue
        }
        //按钮 测试 的事件
        private void BtnTest_Click(object sender, RoutedEventArgs e)
        {
            //Rtb.EchoInfo($"全局变量游戏窗口句柄：[{GlobalVar.GameHwnd}]");
            //Rtb.EchoInfo($"全局变量辅助窗口句柄：[{GlobalVar.GameFzHwnd}]");
            //GlobalVar.PlayerBag.RefreshPackageData();
            //long Id = GlobalVar.PlayerBag.GetItemAt(1, 1).Id;
            //Rtb.EchoInfo($"包裹内含有名称[{GlobalVar.PlayerBag.GetItemAt(1, 1).Name}]，ID：[{Id}]的物品数量为: {(GlobalVar.PlayerBag.TotalQuantityOfItemId(Id))}");
            //Rtb.EchoInfo($"空包裹：{GlobalVar.PlayerBag.CountNullItems()}");
            //GlobalVar.PlayerStatus.EchoActorInfo();
            //Rtb.EchoInfo($"{WndInfo.GetFixXY()}");
            //BMain.FlyToHit("魔龙教主");
            //GlobalVar.PlayerStatus.SetAttackTarget(0X1BC06270);
            Rtb.EchoInfo($"{GetPath.PrintFilesInFolder(GlobalVar.GameHwnd)}");
        }
        //按钮查询窗口的事件
        private void BtnFindWnd_Click(object sender, RoutedEventArgs e)
        {
            btnfindwnd();
        }
        //角色下拉框选项变更事件
        private void CboxPlayerName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CboxPlayerName.SelectedIndex != -1)
            {
                long hwnd = GlobalVar.dm.FindWindowEx(0, "HGE__WNDCLASS", CboxPlayerName.Text);
                if (hwnd != 0)
                {
                    string title = GlobalVar.dm.GetWindowTitle(Convert.ToInt32(hwnd));
                    string[] titles = title.Split('-');
                    if (titles.Length > 1) // 确保标题分割后有足够的元素
                    {
                        Rtb.EchoInfo($"选择游戏窗口：[{title}]");
                        Rtb.EchoInfo($"游戏窗口句柄：[{hwnd}]");
                        Rtb.EchoInfo($"角色名字：[{titles[2]}]");
                        GlobalVar.GameHwnd = (int)hwnd;
                        GlobalVar.GlPath = GetPath.GetPathFromHwnd((int)hwnd);
                        SetComboBoxItem((int)hwnd, CbGlFile);
                        this.Title = this.Title + $"   {titles[2]}";
                        hwnd = GlobalVar.dm.FindWindowEx(0, "AutoIt v3 GUI", CboxPlayerName.Text);
                        if (hwnd != 0)
                        {
                            title = GlobalVar.dm.GetWindowTitle(Convert.ToInt32(hwnd));
                            Rtb.EchoInfo($"找到辅助窗口：[{title}]");
                            Rtb.EchoInfo($"辅助窗口句柄：[{hwnd}]");
                            GlobalVar.GameFzHwnd = (int)hwnd;                            
                        }
                    }
                }
            }
            
        }
        //关闭窗口事件
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("确定要关闭辅助吗？", "确认", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.No)
            {
                HotKeyManager.UnregisterHotKey(hwndSource.Handle);//注销热键
                e.Cancel = true; // 取消关闭操作
            }
        }        
        //初始化UIvalue
        private void InitializeValuet() 
        {          
            this.Title = $"BMHepler V.0.1.0";
        }

        //按钮查询窗口的具体逻辑
        private void btnfindwnd() 
        {
            List<string> playerName = BMain.FindWnd();
            // 给ComboBox下拉选项赋值
            CboxPlayerName.ItemsSource = playerName; // 直接将列表赋值给ItemsSource属性
            CboxPlayerName.SelectedIndex = -1;//为了解决selectedindex不变 不会触发selectechange事件的小bug
            CboxPlayerName.SelectedIndex = 0;
        }
        //初始化定时器
        private void InitializeTimer()
        {
            // 创建一个 DispatcherTimer 实例
            DispatcherTimer timer = new DispatcherTimer();

            // 设置时间间隔为 500 毫秒
            timer.Interval = TimeSpan.FromMilliseconds(300);

            // 添加定时器触发事件处理程序
            timer.Tick += Timer_Tick;

            // 启动定时器
            timer.Start();
        }
        //定时器 工作内容
        private void Timer_Tick(object sender, EventArgs e)
        {
            // 调用第一个方法
            TimerWork1();
            // 调用第二个方法
            TimerWork2();
            // 调用第三个方法
            //TimerWork3();
        }
        //定时器内各个TimerWork的具体逻辑
        private void TimerWork1()
        {
            if (GlobalVar.GameHwnd !=0)
            {
                long WndState = GlobalVar.dm.GetWindowState((int)GlobalVar.GameHwnd, 0);
                if (WndState == 1)
                {
                    if (GlobalVar.GameFzHwnd != 0)
                    {
                        WndState = GlobalVar.dm.GetWindowState((int)GlobalVar.GameFzHwnd, 0);
                        if(WndState == 1)
                        {
                            LbGameWnd.Content ="游戏窗口存在！";
                            LbGameFzWnd.Content = "辅助窗口存在！";
                            LbGameWnd.Foreground = Brushes.Black; // 使用黑色作为前景色
                            LbGameFzWnd.Foreground = Brushes.Black; // 使用红色作为前景色
                            if (GlobalVar.PlayerStatus == null)
                            {
                                GlobalVar.playerBasicAddr = BMain.ReadMem("<BackMir.exe>+00389260", "4");
                                GlobalVar.PlayerStatus = new Actor(GlobalVar.playerBasicAddr);                                
                            }
                        }
                        else 
                        {
                            LbGameWnd.Content = "游戏窗口存在！";
                            LbGameFzWnd.Content = "辅助窗口不存在！";
                            LbGameWnd.Foreground = Brushes.Black; // 使用黑色作为前景色
                            LbGameFzWnd.Foreground = Brushes.Red; // 使用红色作为前景色
                        }
                    }
                }
                else
                {
                    LbGameWnd.Content = "游戏窗口不存在！";
                    LbGameFzWnd.Content = "辅助窗口不存在！";
                    LbGameWnd.Foreground = Brushes.Red; // 使用红色作为前景色
                    LbGameFzWnd.Foreground = Brushes.Red; // 使用红色作为前景色
                    if (GlobalVar.PlayerStatus != null) 
                    {
                        GlobalVar.playerBasicAddr = 0;
                        GlobalVar.PlayerStatus = null;//游戏不存在就且对象不为NULL则设定为NULL
                    }
                }
            }
            
        }
        private void TimerWork2()
        {
            GlobalVar.PlayerBag.RefreshPackageData();
            if (GlobalVar.playerBasicAddr != 0) 
            {
                GlobalVar.PlayerStatus.RefreshActor(GlobalVar.playerBasicAddr);
                ViewModel = new MainViewModel();
                DataContext = ViewModel;
                // 在这里添加Actor对象到Actors集合中
                ViewModel.Actors.Add(GlobalVar.PlayerStatus);
                // 可以继续添加更多的Actor对象
            }

        }
        //函数运行时间测试
        public static void TestExecutionTime(Action action)
        {
            /*测试方法：
            TestExecutionTime(() =>
            {
                // 要测试的函数调用放在这里
                RefreshPackageData();
            });
            */
            Stopwatch stopwatch = Stopwatch.StartNew();
            action(); // 执行传入的委托
            stopwatch.Stop();
            long elapsedTimeMicroseconds = stopwatch.ElapsedTicks * 1000000L / Stopwatch.Frequency;
            Rtb.EchoInfo($"此函数运行时间{elapsedTimeMicroseconds}微秒");
        }
        //移动窗口事件
        private void BtnMoveWnd_Click(object sender, RoutedEventArgs e)
        {
            if (CbWndPos.Text == "左上") 
            {
                GlobalVar.dm.MoveWindow(GlobalVar.GameHwnd, 0, 0);
            }
            else if (CbWndPos.Text == "居中") 
            {
                double screenWidth = SystemParameters.PrimaryScreenWidth;
                double screenHeight = SystemParameters.PrimaryScreenHeight;
                int windowWidth = 1030; // 替换为你的窗口宽度
                int windowHeight = 797; // 替换为你的窗口高度
                // 计算窗口在水平和垂直方向上的居中位置
                int horizontalPosition = (int)((screenWidth - windowWidth) / 2);
                int verticalPosition = (int)((screenHeight - windowHeight) / 2);
                GlobalVar.dm.MoveWindow(GlobalVar.GameHwnd, horizontalPosition, verticalPosition);
            }
        }
        //置顶窗口事件
        private void BtnTopWnd_Click(object sender, RoutedEventArgs e)
        {
            if (BtnTopWnd.Content.ToString() == "置顶")
            {
                /*
                0 : 关闭指定窗口
                1 : 激活指定窗口
                2 : 最小化指定窗口,但不激活
                3 : 最小化指定窗口,并释放内存,但同时也会激活窗口.(释放内存可以考虑用FreeProcessMemory函数)
                4 : 最大化指定窗口,同时激活窗口.
                5 : 恢复指定窗口 ,但不激活
                6 : 隐藏指定窗口
                7 : 显示指定窗口
                8 : 置顶指定窗口
                9 : 取消置顶指定窗口
                10 : 禁止指定窗口
                11 : 取消禁止指定窗口
                12 : 恢复并激活指定窗口
                13 : 强制结束窗口所在进程.
                14 : 闪烁指定的窗口
                15 : 使指定的窗口获取输入焦点
                */
                // 将按钮内容更改为 "取消"
                BtnTopWnd.Content = "取消";
                GlobalVar.dm.SetWindowState(GlobalVar.GameHwnd,8);

            }
            else
            {
                // 将按钮内容更改为 "置顶"
                BtnTopWnd.Content = "置顶";
                GlobalVar.dm.SetWindowState(GlobalVar.GameHwnd, 9);
            }
        }
        //热键相关
        private void CbRegisterHotKey_Checked(object sender, RoutedEventArgs e)
        {
            // 注册热键
            hwndSource = PresentationSource.FromVisual(this) as HwndSource;
            if (hwndSource != null)
            {
                hwndSource.AddHook(HwndHook);
                HotKeyManager.RegisterHotKey(hwndSource.Handle);
            }
        }
        private void CbRegisterHotKey_Unchecked(object sender, RoutedEventArgs e)
        {
            // 注销热键
            if (hwndSource != null)
            {
                HotKeyManager.UnregisterHotKey(hwndSource.Handle);
            }
        }
        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;

            if (msg == WM_HOTKEY && wParam.ToInt32() == HotKeyManager.HOTKEY_ID)
            {
                // 热键被触发
                //Rtb.EchoInfo("按下了快捷键");
                BMain.DoWork();
                handled = true;
            }

            return IntPtr.Zero;
        }
        //热键选项下拉框事件
        private void CbDoWork_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // 从ComboBox中获取选定的操作，然后将其分配给全局变量
            switch (CbDoWork.SelectedIndex) 
            {
                case 0:
                    GlobalVar.HotKeyAction = () =>
                    {
                        // 这里不会执行任何代码，只是把选定的操作分配给全局变量
                        // 在调用DoWork时，选定的操作会执行
                        BMain.WashItem();
                    };
                    break;
                case 1:
                    GlobalVar.HotKeyAction = () =>
                    {
                        // 这里不会执行任何代码，只是把选定的操作分配给全局变量
                        // 在调用DoWork时，选定的操作会执行
                        //BMain.FlyToHit();
                    };
                    break;
                case 2:
                    GlobalVar.HotKeyAction = () =>
                    {
                        // 这里不会执行任何代码，只是把选定的操作分配给全局变量
                        // 在调用DoWork时，选定的操作会执行
                    };
                    break;
                default:
                    GlobalVar.HotKeyAction = () =>
                    {
                        // 这里不会执行任何代码，只是把选定的操作分配给全局变量
                        // 在调用DoWork时，选定的操作会执行
                    };
                    break;
            }
        }

        private void CbBindWnd_Click(object sender, RoutedEventArgs e)
        {
            if (CbBindWnd.IsChecked == true) 
            {
                if (BMain.BindWnd(1) == 1)
                {
                    ((CheckBox)sender).IsChecked = true;
                }
                else 
                {
                    ((CheckBox)sender).IsChecked = false;
                }
            }
            else 
            {
                if (BMain.BindWnd(0) == 1)
                {
                    ((CheckBox)sender).IsChecked = false;
                }
            }            
        }
        //设置combobox  选项
        private void SetComboBoxItem(int WndHwnd, ComboBox comboBox)
        {
            string[] fileList = GetPath.ListFilesInFolder(WndHwnd);
            string[] fileNames = fileList.Select(file => System.IO.Path.GetFileNameWithoutExtension(file)).ToArray();
            // 清空 ComboBox 的数据源
            comboBox.ItemsSource = null;
            comboBox.Items.Clear(); // 清空ComboBox的项集合
            comboBox.ItemsSource = fileNames;
            // 查找包含指定字符串的项的索引
            int index = comboBox.Items.Cast<string>().ToList().FindIndex(item => item.Contains("自动副本专用"));
            // 如果找到了包含指定字符串的项，则设置 ComboBox 的 SelectedIndex 属性
            if (index != -1)
            {
                comboBox.SelectedIndex = index;
            }
            else
            {
                index = comboBox.Items.Cast<string>().ToList().FindIndex(item => item.Contains("常用"));
                if (index != -1)
                {
                    comboBox.SelectedIndex = index;
                }
                else 
                {
                    comboBox.SelectedIndex = 0;
                }
            }
        }
    }
}