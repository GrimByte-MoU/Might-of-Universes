using Terraria;
using Terraria.ModLoader;
using MightofUniverses.Content.Prefixes;

namespace MightofUniverses.Content.Items.GlobalItems
{
    public class HealthPrefixGlobalItem : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.accessory;
        }

        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (item.prefix == 0) return;

            ModPrefix prefix = PrefixLoader.GetPrefix(item.prefix);
            if (prefix is HealthPrefixBase healthPrefix)
            {
                int power = healthPrefix.GetPrefixPower();
                
                switch (healthPrefix.GetPrefixType())
                {
                    case HealthPrefixType.MaxHealth:
                        player.statLifeMax2 += healthPrefix.GetMaxHealthBonus(power);
                        break;
                        
                    case HealthPrefixType.LifeRegen:
                        player.lifeRegen += healthPrefix.GetLifeRegenBonus(power);
                        break;
                }
            }
        }
    }
}