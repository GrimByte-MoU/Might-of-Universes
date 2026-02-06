using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Accessories
{
    public class SkyRibbon : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 24;
            Item.value = Item.buyPrice(silver: 60);
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MOUPlayer>().EquippedSkyRibbon = true;
        }
    }
}