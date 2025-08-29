using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MightofUniverses.Content.Prefixes
{
    // Base class for health-related prefixes
    public abstract class HealthPrefixBase : ModPrefix
    {
        // These prefixes can apply to any accessory
        public override PrefixCategory Category => PrefixCategory.Accessory;

        // Default roll chance
        public override float RollChance(Item item) => 1f;

        // Modify the item value based on the prefix power
        public override void ModifyValue(ref float valueMult)
        {
            valueMult *= 1f + (0.05f * GetPrefixPower());
        }

        // Abstract method to get the power level of this prefix (1-4)
        protected abstract int GetPrefixPower();

        // Abstract method to get the prefix effect type
        protected abstract HealthPrefixType GetPrefixType();

        // Update the player stats when this prefix is on an equipped accessory
// Update the player stats when this prefix is on an equipped accessory
public void UpdateAccessory(Item item, Player player, bool hideVisual)
{
    int power = GetPrefixPower();
    
    switch (GetPrefixType())
    {
        case HealthPrefixType.MaxHealth:
            // +10/+20/+35/+50 max health
            player.statLifeMax2 += GetMaxHealthBonus(power);
            break;
            
        case HealthPrefixType.LifeRegen:
            // +1/+2/+3/+4 life regeneration
            player.lifeRegen += GetLifeRegenBonus(power);
            break;
    }
}

        // Helper methods to get the bonus values based on power level
        protected int GetMaxHealthBonus(int power)
        {
            switch (power)
            {
                case 1: return 10;
                case 2: return 20;
                case 3: return 35;
                case 4: return 50;
                default: return 0;
            }
        }

        protected int GetLifeRegenBonus(int power)
        {
            return power * 1; // +1/+2/+3/+4 life regen based on power level
        }

        // Get tooltip lines for the prefix
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

    // Enum to define the different types of health prefixes
    public enum HealthPrefixType
    {
        MaxHealth,
        LifeRegen
    }

    #region Max Health Prefixes
    
    // Tier 1: +10 max health
    public class HardyPrefix : HealthPrefixBase
    {
        protected override int GetPrefixPower() => 1;
        protected override HealthPrefixType GetPrefixType() => HealthPrefixType.MaxHealth;
        
        public override void SetStaticDefaults()
        {
            // In 1.4.4, display names are typically set in localization files
            // If you're not using localization, uncomment this:
            // DisplayName.SetDefault("Hearty");
        }
    }
    
    // Tier 2: +20 max health
    public class HealthyPrefix : HealthPrefixBase
    {
        protected override int GetPrefixPower() => 2;
        protected override HealthPrefixType GetPrefixType() => HealthPrefixType.MaxHealth;
        
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Healthy");
        }
    }
    
    // Tier 3: +35 max health
    public class VitalPrefix : HealthPrefixBase
    {
        protected override int GetPrefixPower() => 3;
        protected override HealthPrefixType GetPrefixType() => HealthPrefixType.MaxHealth;
        
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Vital");
        }
    }
    
    // Tier 4: +50 max health
    public class RobustPrefix : HealthPrefixBase
    {
        protected override int GetPrefixPower() => 4;
        protected override HealthPrefixType GetPrefixType() => HealthPrefixType.MaxHealth;
        
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Robust");
        }
    }
    
    #endregion
    
    #region Life Regeneration Prefixes
    
    // Tier 1: +1 life regeneration
    public class HealingPrefix : HealthPrefixBase
    {
        protected override int GetPrefixPower() => 1;
        protected override HealthPrefixType GetPrefixType() => HealthPrefixType.LifeRegen;
        
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Healing");
        }
    }
    
    // Tier 2: +2 life regeneration
    public class RegenerativePrefix : HealthPrefixBase
    {
        protected override int GetPrefixPower() => 2;
        protected override HealthPrefixType GetPrefixType() => HealthPrefixType.LifeRegen;
        
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Regenerative");
        }
    }
    
    // Tier 3: +3 life regeneration
    public class MendingPrefix : HealthPrefixBase
    {
        protected override int GetPrefixPower() => 3;
        protected override HealthPrefixType GetPrefixType() => HealthPrefixType.LifeRegen;
        
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Mending");
        }
    }
    
    // Tier 4: +4 life regeneration
    public class RestoringPrefix : HealthPrefixBase
    {
        protected override int GetPrefixPower() => 4;
        protected override HealthPrefixType GetPrefixType() => HealthPrefixType.LifeRegen;
        
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Restoring");
        }
    }
    
    #endregion
}
