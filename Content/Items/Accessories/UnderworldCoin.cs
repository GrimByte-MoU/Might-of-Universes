using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace MightofUniverses.Content.Items.Accessories
{
    public class UnderworldCoin : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.buyPrice(gold: 2);
            Item.rare = ItemRarityID.Orange;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.maxMinions += 1;

            // +2% summon damage per minion currently summoned (counts slots used)
            // Use slotsMinions (float) and round up to get "per minion" feel even with >1 slot minions
            int minionCount = (int)Math.Ceiling(player.slotsMinions);
            if (minionCount > 0)
                player.GetDamage(DamageClass.Summon) += 0.02f * minionCount;
        }
    }
}