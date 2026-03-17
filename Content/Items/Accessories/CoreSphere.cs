using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Common;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Accessories
{
    public class CoreSphere : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.sellPrice(gold: 15);
            Item.rare = ModContent.RarityType<TerraiumRarity>();
            Item.DamageType = ModContent.GetInstance<PacifistDamageClass>();
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<CoreSpherePlayer>().hasCoreSphere = true;

            for (int i = 0; i < 3; i++)
            {
                bool foundInner = false;
                for (int p = 0; p < Main.maxProjectiles; p++)
                {
                    Projectile proj = Main.projectile[p];
                    if (proj.active && proj.type == ModContent.ProjectileType<MoltenCoreSphereProjectile>() && 
                        proj.owner == player.whoAmI && proj.ai[0] == i)
                    {
                        foundInner = true;
                        break;
                    }
                }

                if (!foundInner && Main.myPlayer == player.whoAmI)
                {
                    Projectile.NewProjectile(
                        player.GetSource_Accessory(Item),
                        player.Center,
                        Microsoft.Xna.Framework.Vector2.Zero,
                        ModContent.ProjectileType<MoltenCoreSphereProjectile>(),
                        250,
                        2f,
                        player.whoAmI,
                        i
                    );
                }
            }

            for (int i = 0; i < 6; i++)
            {
                bool foundOuter = false;
                for (int p = 0; p < Main.maxProjectiles; p++)
                {
                    Projectile proj = Main.projectile[p];
                    if (proj.active && proj.type == ModContent.ProjectileType<CoreSphereProjectile>() && 
                        proj.owner == player.whoAmI && proj.ai[0] == i)
                    {
                        foundOuter = true;
                        break;
                    }
                }

                if (!foundOuter && Main.myPlayer == player.whoAmI)
                {
                    Projectile.NewProjectile(
                        player.GetSource_Accessory(Item),
                        player.Center,
                        Vector2.Zero,
                        ModContent.ProjectileType<CoreSphereProjectile>(),
                        150,
                        2f,
                        player.whoAmI,
                        i
                    );
                }
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.LavaBucket, 10)
                .AddIngredient<Lavastone>()
                .AddIngredient(ModContent.ItemType<TerraiumBar>(), 7)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}