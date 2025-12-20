using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Weapons
{
    public class ShadowDagger : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ShadowFlameKnife);
            Item.damage = 80;
            Item.rare = ItemRarityID.Lime;
            Item.value = Item.sellPrice(gold: 8);
            Item.shoot = ModContent.ProjectileType<Projectiles.ShadowDaggerProjectile>();
            Item.maxStack = 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.ShadowFlameKnife)
                .AddIngredient(ModContent.ItemType<EclipseLight>(), 15)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}