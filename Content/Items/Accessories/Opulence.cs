using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Accessories
{
    public class Opulence : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ItemRarityID.Expert;
            Item.expert = true;
            Item.accessory = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<OpulencePlayer>().hasOpulence = true;
            player.hasLuckyCoin = true;
            player.goldRing = true;
            player.GetDamage(DamageClass.Generic) += GetDamageBonus(player);
            
            if (player.CountItem(ItemID.PlatinumCoin) >= 5)
            {
                player.GetCritChance(DamageClass.Generic) += 5f;
                player.statDefense += 7;
                player.endurance += 0.05f;
            }
        }

        private float GetDamageBonus(Player player)
        {
            int platinumCoins = player.CountItem(ItemID.PlatinumCoin);
            float damageBonus = platinumCoins * 0.5f;
            return damageBonus > 0.75f ? 0.75f : damageBonus;
        }
    }
}


