using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static BMhelper_WPF.BMhelper;

namespace BMhelper_WPF
{
    public class Item
    {
        public long Id { get; set; }//物品ID
        public string Name { get; set; }//物品ID
        public long Quantity { get; set; }//物品数量
        public long BindingStatus { get; set; } // 绑定状态，使用位运算表示

        // 可以添加更多属性，比如等级、耐久度等

        public Item(long id, string name , long quantity, long bindingStatus)
        {
            Id = id;
            Name = name;
            Quantity = quantity;
            BindingStatus = bindingStatus;
        }
    }

    public class Package
    {
        private Item[,] items;

        public Package()
        {
            items = new Item[8, 10]; // 创建一个8x10的二维数组来表示包裹
        }

        // 向包裹中添加物品
        public void AddItem(int x, int y, long id, string name, long quantity, long bindingStatus)
        {
            items[x, y] = new Item(id, name , quantity, bindingStatus);
        }

        // 从包裹中移除物品
        public void RemoveItem(int x, int y)
        {
            items[x, y] = null;
        }

        // 获取包裹中某个格子的物品信息
        public Item GetItemAt(int x, int y)
        {
            return items[x, y];
        }

        // 检查包裹中某个格子是否有物品
        public bool HasItem(int x, int y)
        {
            return items[x, y] != null && items[x, y].Id != 0;
        }
        //统计空包裹
        public int CountNullItems()
        {
            int count = 0;
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    if (GetItemAt(x, y) == null)
                    {
                        count++;
                    }
                }
            }
            return count;
        }
        // 统计 package 实例中 ID 相同的物品的总数量
        public long TotalQuantityOfItemId(long id)
        {
            long totalQuantity = 0;

            for (int x = 0; x < items.GetLength(0); x++)
            {
                for (int y = 0; y < items.GetLength(1); y++)
                {
                    if (items[x, y] != null && items[x, y].Id == id)
                    {
                        totalQuantity += items[x, y].Quantity;
                    }
                }
            }

            return totalQuantity;
        }
        //查询物品坐标
        public (int x, int y) FindItemXY(object searchKey)
        {
            if (searchKey is long id)
            {
                for (int y = 0; y < items.GetLength(1); y++)
                {
                    for (int x = 0; x < items.GetLength(0); x++)
                    {
                        if (items[x, y] != null && items[x, y].Id == id)
                        {
                            return (x, y ); 
                        }
                    }
                }
            }
            else if (searchKey is string name)
            {
                for (int y = 0; y < items.GetLength(1); y++)
                {
                    for (int x = 0; x < items.GetLength(0); x++)
                    {
                        if (items[x, y] != null && items[x, y].Name.Contains(name))
                        {
                            return (x , y);
                        }
                    }
                }
            }
            return (-1,-1); // 未找到该物品
        }
        public void PrintItem()
        {
            for (int y = 0; y < items.GetLength(1); y++)
            {
                for (int x = 0; x < items.GetLength(0); x++)
                {
                    if (items[x, y] != null )
                    {
                        Rtb.EchoInfo($"包裹位置：{(x+1, y+1)}，ID：{items[x, y].Id.ToString("D4")}，物品数量：{items[x, y].Quantity.ToString("D3")}，物品名称：{items[x, y].Name}");
                    }
                }
            }
        }
        // 刷新背包数据的方法
        public void RefreshPackageData()
        {
            // 在这里实现刷新背包数据的逻辑，例如重新从数据库加载数据等           
            items = new Item[8, 10];
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    int BagNum = y * 8 + x;
                    long ItemID = BMain.ReadMem($"[[<BackMir.exe>+00389260]+850]+{(BagNum*0x60).ToString("X")}", "4");
                    if ((int)ItemID != 0)
                    {
                        string ItemName = GlobalVar.dm.ReadString(GlobalVar.GameHwnd, $"[[<BackMir.exe>+00389260]+850]+{(BagNum * 0x60 + 0x4).ToString("X")}", 0, 0);
                        long ItemQuantity = BMain.ReadMem($"[[<BackMir.exe>+00389260]+850]+{(BagNum * 0x60 + 0x1C).ToString("X")}", "1");
                        long ItemBindingStatus = BMain.ReadMem($"[[<BackMir.exe>+00389260]+850]+{(BagNum * 0x60 + 0x1E).ToString("X")}", "1");
                        if (ItemQuantity == 0) { ItemQuantity++; }
                        //Rtb.EchoInfo($"第{BagNum + 1}格包裹，在第{y + 1}排，第{x + 1}个");
                        //Rtb.EchoInfo($"物品ID[{ItemID}]，物品名称[{ItemName}]，物品数量[{ItemQuantity}]"); 
                        AddItem(x,y,ItemID,ItemName,ItemQuantity, ItemBindingStatus);
                    }
                    
                }
            }
        }
    }
}
