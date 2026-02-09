using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Common;

namespace MightofUniverses.Content.Items.Accessories
{
    public class IcefireSphere : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.LightRed;
            Item.DamageType = ModContent.GetInstance<PacifistDamageClass>();
            Item.accessory = true;
        }

       public override void UpdateAccessory(Player player, bool hideVisual)
{
    player.GetModPlayer<IcefireSpherePlayer>().hasIcefireSphere = true;
    int desiredSpheres = 2;
    int activeSpheres = 0;
    
    for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.type == ModContent.ProjectileType<IcefireSphereProjectile>() && proj.owner == player.whoAmI)
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
            ModContent.ProjectileType<IcefireSphereProjectile>(),
            45,
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
                .AddIngredient(ModContent.ItemType<Lavastone>(), 1)
                .AddIngredient(ModContent.ItemType<Coldstone>(), 1)
                .AddIngredient(ItemID.SoulofLight, 5)
                .AddIngredient(ItemID.SoulofNight, 5)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}