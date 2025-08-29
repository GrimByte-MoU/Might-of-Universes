using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;
using MightofUniverses.Common;

namespace MightofUniverses.Content.Items.Accessories
{
    public class WormFamiliar : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.LightRed;
            Item.DamageType = ModContent.GetInstance<PacifistDamageClass>();
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Generic) += 0.08f;
            player.endurance += 0.08f;
            player.GetModPlayer<WormFangPlayer>().hasWormFamiliar = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<WormFang>())
                .AddIngredient(ItemID.WormScarf)
                .AddIngredient(ItemID.SoulofNight, 5)
                .AddIngredient(ItemID.CursedFlame, 5)
                .AddTile(TileID.DemonAltar)
                .Register();
        }
    }
}
