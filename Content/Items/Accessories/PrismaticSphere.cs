using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Common;

namespace MightofUniverses.Content.Items.Accessories
{
    public class PrismaticSphere : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Yellow;
            Item.DamageType = ModContent.GetInstance<PacifistDamageClass>();
            Item.accessory = true;
        }

       public override void UpdateAccessory(Player player, bool hideVisual)
{
    player.GetModPlayer<PrismaticSpherePlayer>().hasPrismaticSphere = true;
    int desiredSpheres = 4;
    int activeSpheres = 0;
    
    for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.type == ModContent.ProjectileType<PrismaticSphereProjectile>() && proj.owner == player.whoAmI)
                {
                    activeSpheres++;
                    proj.ai[0] = activeSpheres - 1;
                }
            }

    while (activeSpheres < desiredSpheres)
    {
        Projectile.NewProjectile(
            player.GetSource_Accessory(Item),
            player.Center,
            Vector2.Zero,
            ModContent.ProjectileType<PrismaticSphereProjectile>(),
            75,
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
                .AddIngredient(ModContent.ItemType<BSCU>(), 1)
                .AddIngredient(ModContent.ItemType<PrismaticCatalyst>(), 5)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}