using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MightofUniverses.Content.Prefixes
{
    public abstract class HealthPrefixBase : ModPrefix
    {
        public override PrefixCategory Category => PrefixCategory.Accessory;

        public override float RollChance(Item item) => 1f;

        public override void ModifyValue(ref float valueMult)
        {
            valueMult *= 1f + (0.05f * GetPrefixPower());
        }

        public abstract int GetPrefixPower();

        public abstract HealthPrefixType GetPrefixType();

        public int GetMaxHealthBonus(int power)
        {
            switch (power)
            {
                case 1: return 10;
                case 2: return 15;
                case 3: return 20;
                case 4: return 25;
                default: return 0;
            }
        }

        public int GetLifeRegenBonus(int power)
        {
            return power * 1;
        }

        public override IEnumerable<TooltipLine> GetTooltipLines(Item item)
        {
            int power = GetPrefixPower();
            
            switch (GetPrefixType())
            {
                case HealthPrefixType.MaxHealth:
                    yield return new TooltipLine(Mod, "PrefixMaxHealth", 
                        $"+{GetMaxHealthBonus(power)} max health")
                    {
                        IsModifier = true
                    };
                    break;
                    
                case HealthPrefixType.LifeRegen:
                    yield return new TooltipLine(Mod, "PrefixLifeRegen", 
                        $"+{GetLifeRegenBonus(power)} life regeneration")
                    {
                        IsModifier = true
                    };
                    break;
            }
        }
    }

    public enum HealthPrefixType
    {
        MaxHealth,
        LifeRegen
    }

    #region Max Health Prefixes
    
    public class HardyPrefix : HealthPrefixBase
    {
        public override int GetPrefixPower() => 1;
        public override HealthPrefixType GetPrefixType() => HealthPrefixType.MaxHealth;
        
        public override void SetStaticDefaults()
        {
        }
    }
    
    public class HealthyPrefix : HealthPrefixBase
    {
        public override int GetPrefixPower() => 2;
        public override HealthPrefixType GetPrefixType() => HealthPrefixType.MaxHealth;
        
        public override void SetStaticDefaults()
        {
        }
    }
    
    public class VitalPrefix : HealthPrefixBase
    {
        public override int GetPrefixPower() => 3;
        public override HealthPrefixType GetPrefixType() => HealthPrefixType.MaxHealth;
        
        public override void SetStaticDefaults()
        {
        }
    }
    
    public class RobustPrefix : HealthPrefixBase
    {
        public override int GetPrefixPower() => 4;
        public override HealthPrefixType GetPrefixType() => HealthPrefixType.MaxHealth;
        
        public override void SetStaticDefaults()
        {
        }
    }
    
    #endregion
    
    #region Life Regeneration Prefixes
    
    public class HealingPrefix : HealthPrefixBase
    {
        public override int GetPrefixPower() => 1;
        public override HealthPrefixType GetPrefixType() => HealthPrefixType.LifeRegen;
        
        public override void SetStaticDefaults()
        {
        }
    }
    
    public class RegenerativePrefix : HealthPrefixBase
    {
        public override int GetPrefixPower() => 2;
        public override HealthPrefixType GetPrefixType() => HealthPrefixType.LifeRegen;
        
        public override void SetStaticDefaults()
        {
        }
    }
    
    public class MendingPrefix : HealthPrefixBase
    {
        public override int GetPrefixPower() => 3;
        public override HealthPrefixType GetPrefixType() => HealthPrefixType.LifeRegen;
        
        public override void SetStaticDefaults()
        {
        }
    }
    
    public class RestoringPrefix : HealthPrefixBase
    {
        public override int GetPrefixPower() => 4;
        public override HealthPrefixType GetPrefixType() => HealthPrefixType.LifeRegen;
        
        public override void SetStaticDefaults()
        {
        }
    }
    
    #endregion
}