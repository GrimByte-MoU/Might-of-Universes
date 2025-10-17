using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Weapons
{
    public class Titanoboa : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ScourgeoftheCorruptor);
            Item.damage = 110;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.shootSpeed = 18f;
            Item.rare = ItemRarityID.Cyan;
            Item.value = Item.sellPrice(gold: 20);
            Item.shoot = ModContent.ProjectileType<Projectiles.TitanoboaProjectile>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.ScourgeoftheCorruptor)
                .AddIngredient(ModContent.ItemType<AncientBone>(), 15)
                .AddIngredient(ModContent.ItemType<TarChunk>(), 15)
                .AddIngredient(ModContent.ItemType<PrehistoricAmber>(), 15)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}