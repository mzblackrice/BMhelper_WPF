using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMhelper_WPF
{
    public unsafe struct ItemData
    {
        public int ID;
        public fixed byte Name[20];
        public byte Quality;
        public byte Cooldown;
        public byte ItemLevel;
        public short StackableQuantity;
        public byte BindStatus;
        public byte MovementSpeed;
        public byte Weight;
        public byte RequirementType;
        public byte RequirementValue;
        public byte GenderRequirement;
        public short ItemType;
        public short MaxAttack;
        public short MinAttack;
        public short MaxDefense;
        public short MinDefense;
        public short MaxMagicDefense;
        public short MinMagicDefense;
        public short MaxMagicAttack;
        public short MinMagicAttack;
        public short MaxMagic;
        public short MinMagic;
        public short MaxDurability;
        public short MinDurability;
        public int WeaponGrowthValue;
        public int IdentificationProperties;
        public byte PotentialProperties;
        public byte MaxPotentialLevel;
        public byte MinAwakeningTimes;
        public byte InstantMagicRecoveryValue;
        public int PrefixIndex;
        public byte SaveType;
        public short EquipmentStarLevel;
        public short DisplayPattern;
        public short Price;
        public int ItemTag;
    }
}
