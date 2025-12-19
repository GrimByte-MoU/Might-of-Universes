using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Accessories
{
    public class GravediggersRing : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 22;
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(gold: 3);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var reaper = player.GetModPlayer<ReaperPlayer>();
            var acc = player.GetModPlayer<ReaperAccessoryPlayer>();

            player.GetDamage<ReaperDamageClass>() += 0.10f;
            reaper.maxSoulEnergy += 75f;

            acc.HasGravediggersRing = true;
            acc.RefundChance += 0.10f;
            acc.RefundFraction = System.Math.Max(acc.RefundFraction, 0.50f);
        }
    }
}