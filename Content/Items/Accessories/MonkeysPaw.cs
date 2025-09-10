using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Accessories
{
    public class MonkeysPaw : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 28;
            Item.accessory = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(gold: 2);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var reaper = player.GetModPlayer<ReaperPlayer>();
            var acc = player.GetModPlayer<ReaperAccessoryPlayer>();

            // +15% max health (additive approach)
            player.statLifeMax2 += (int)(player.statLifeMax2 * 0.15f);
            reaper.maxSoulEnergy += 50f;

            // Increase damage taken by 25%: negative endurance
            player.endurance -= 0.25f;
            if (player.endurance < -0.9f) player.endurance = -0.9f;

            acc.HasMonkeysPaw = true;
        }
    }
}