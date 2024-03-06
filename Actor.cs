using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static BMhelper_WPF.BMhelper;

namespace BMhelper_WPF
{
    public class Actor
    {
        public long Health { get; set; } // 血量
        public long WeaponDur { get; set; } // 武器耐久
        public long RingDur { get; set; } // 左戒指耐久
        public long X { get; set; } // x坐标
        public long Y { get; set; } // y坐标
        public long Status { get; set; } // 人物状态
        public long Ride { get; set; } // 坐骑状态
        public long CT { get; set; } // CT状态
        public long AttickTarget { get; set; } // 普攻目标
        public long SkillTarget { get; set; } // 技能目标

        public Actor(long address)
        {
            RefreshActor(address);
        }
        // 设置普攻目标
        public void SetAttackTarget(long address)
        {
            BMain.WriteMen($"{(GlobalVar.playerBasicAddr + 0x880).ToString("X")}", "4", (int)address);
            
        }
        // 清楚普攻目标
        public void CleanAttackTarget(long address)
        {
            BMain.WriteMen($"{(GlobalVar.playerBasicAddr + 0x880).ToString("X")}", "4", 0);
            
        }

        // 设置技能目标
        public void SetSkillTarget(long address)
        {
            BMain.WriteMen($"{(GlobalVar.playerBasicAddr + 0xa3c).ToString("X")}", "4",(int)address);
            
        }
        // 清除技能目标
        public void CleantSkillTarget(long address)
        {
            BMain.WriteMen($"{(GlobalVar.playerBasicAddr + 0xa3c).ToString("X")}", "4", 0);
            
        }
        //输出人物信息
        public void EchoActorInfo() 
        {
            Rtb.EchoInfo($"人物血量：[{Health}]");
            Rtb.EchoInfo($"武器耐久：[{WeaponDur}]");
            Rtb.EchoInfo($"戒指耐久：[{RingDur}]");
            Rtb.EchoInfo($"人物坐标：[{X}，{Y}]");
            Rtb.EchoInfo($"人物状态：[{Status}]");
            Rtb.EchoInfo($"坐骑状态：[{Ride}]");
            Rtb.EchoInfo($"CT状态：[{CT}]");
            Rtb.EchoInfo($"普攻目标：[{AttickTarget.ToString("X8")}]");
            Rtb.EchoInfo($"技能目标：[{SkillTarget.ToString("X8")}]");
        }
        // 刷新人物信息
        public void RefreshActor(long address)
        {
            Health = BMain.ReadMem($"{(address + 0xA8).ToString("X")}", "4");
            WeaponDur = BMain.ReadMem($"{(address + 0x276).ToString("X")}", "2"); ;
            RingDur = BMain.ReadMem($"{(address + 0x4B6).ToString("X")}", "2"); ;
            X = BMain.ReadMem($"{(address + 0x4C).ToString("X")}", "4");
            Y = BMain.ReadMem($"{(address + 0x50).ToString("X")}", "4");
            Status = BMain.ReadMem($"{(address + 0x60).ToString("X")}", "4");
            Ride = BMain.ReadMem($"{(address + 0x214).ToString("X")}", "1"); ;
            CT = BMain.ReadMem($"{(address + 0xB18).ToString("X")}", "1"); ;
            AttickTarget = BMain.ReadMem($"{(address + 0x880).ToString("X")}", "4");
            SkillTarget = BMain.ReadMem($"{(address + 0xa3c).ToString("X")}", "4");
        }
    }
}
