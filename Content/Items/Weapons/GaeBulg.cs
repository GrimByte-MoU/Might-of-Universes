using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Weapons
{
    public class GaeBulg : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.Gungnir);
            Item.damage = 95;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(gold: 10);
            Item.shoot = ModContent.ProjectileType<Projectiles.GaeBulgProjectile>();
            Item.scale = 1.5f;
            Item.maxStack = 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Gungnir)
                .AddIngredient(ModContent.ItemType<HallowedLight>(), 15)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}