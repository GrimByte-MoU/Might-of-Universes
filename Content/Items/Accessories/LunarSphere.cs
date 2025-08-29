using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Common;

namespace MightofUniverses.Content.Items.Accessories
{
    public class LunarSphere : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ItemRarityID.Purple;
            Item.DamageType = ModContent.GetInstance<PacifistDamageClass>();
            Item.accessory = true;
        }

       public override void UpdateAccessory(Player player, bool hideVisual)
{
    player.GetModPlayer<LunarSpherePlayer>().hasLunarSphere = true;
    int desiredSpheres = 5;
    int activeSpheres = 0;
    
    // Count existing spheres
    for (int i = 0; i < Main.maxProjectiles; i++)
    {
        Projectile proj = Main.projectile[i];
        if (proj.active && proj.type == ModContent.ProjectileType<LunarSphereProjectile>() && proj.owner == player.whoAmI)
        {
            activeSpheres++;
            proj.ai[0] = activeSpheres - 1;
        }
    }

    // Spawn missing spheres
    while (activeSpheres < desiredSpheres)
    {
        Projectile.NewProjectile(
            player.GetSource_Accessory(Item),
            player.Center,
            Microsoft.Xna.Framework.Vector2.Zero,
            ModContent.ProjectileType<LunarSphereProjectile>(),
            100,
            1f,
            player.whoAmI,
            ai0: activeSpheres
        );
        activeSpheres++;
    }
}
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<PrismaticSphere>(), 1)
                .AddIngredient(ItemID.LunarBar, 5)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}