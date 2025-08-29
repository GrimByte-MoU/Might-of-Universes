using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Common;

namespace MightofUniverses.Content.Items.Accessories
{
    public class HellwyrmFamiliar : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Pink;
            Item.DamageType = ModContent.GetInstance<PacifistDamageClass>();
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Generic) += 0.09f;
            player.endurance += 0.09f;
            player.GetModPlayer<WormFangPlayer>().hasHellwyrmFamiliar = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<WormFamiliar>())
                .AddIngredient(ModContent.ItemType<UnderworldMass>(), 5)
                .AddIngredient(ItemID.MagmaStone)
                .AddIngredient(ItemID.HellstoneBar, 10)
                .AddTile(TileID.DemonAltar)
                .Register();
        }
    }
}
