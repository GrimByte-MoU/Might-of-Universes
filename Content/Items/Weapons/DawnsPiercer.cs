// Content/Items/Weapons/DawnsPiercer.cs
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Content.Rarities;
using MightofUniverses.Common.GlobalProjectiles;

namespace MightofUniverses.Content.Items.Weapons
{
    public class DawnsPiercer : ModItem
    {
        public override void SetDefaults()
        {
            Item.width        = 54;
            Item.height       = 28;
            Item.value        = Item.sellPrice(gold: 20);
            Item.rare         = ModContent.RarityType<SollutumRarity>();
            Item.damage       = 120;
            Item.DamageType   = DamageClass.Ranged;
            Item.useTime      = 12;
            Item.useAnimation = 12;
            Item.useStyle     = ItemUseStyleID.Shoot;
            Item.noMelee      = true;
            Item.knockBack    = 6f;
            Item.shoot        = ProjectileID.Stake;
            Item.shootSpeed   = 20f;
            Item.useAmmo      = AmmoID.Stake;
            Item.autoReuse    = true;
            Item.UseSound     = SoundID.Item11;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // 1) Two parallel stakes
            Vector2 perp = Vector2.Normalize(velocity).RotatedBy(MathHelper.PiOver2) * 10f;
            for (int i = -1; i <= 1; i += 2)
            {
                var proj = Projectile.NewProjectileDirect(
                    source,
                    position + perp * i,
                    velocity,
                    type,
                    damage,
                    knockback,
                    player.whoAmI
                );
                proj.penetrate += 3;
                proj.GetGlobalProjectile<DawnsPiercerGlobalProjectile>().ApplyDebuffs = true;
            }

            // 2) Four helix solar bolts
            float boltDamage = damage * 70 / 100f;
            float boltSpeed  = 16f;
            Vector2[] offsets = new[]
            {
                Vector2.Normalize(velocity).RotatedBy(MathHelper.PiOver2) * 8f,
                Vector2.Normalize(velocity).RotatedBy(MathHelper.PiOver4) * 8f
            };
            float[] phases = { 0, MathHelper.Pi, MathHelper.PiOver2, MathHelper.Pi + MathHelper.PiOver2 };

            for (int ring = 0; ring < 2; ring++)
            {
                for (int j = 0; j < 2; j++)
                {
                    Projectile.NewProjectile(
                        source,
                        position + offsets[ring] * (j == 0 ? 1 : -1),
                        velocity * 0.8f,
                        ModContent.ProjectileType<DawnsPiercerBolt>(),
                        (int)boltDamage,
                        knockback * 0.5f,
                        player.whoAmI,
                        phases[ring * 2 + j]
                    );
                }
            }

            return false; // we handled all projectiles
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.StakeLauncher)
                .AddIngredient(ModContent.ItemType<SollutumBar>(), 10)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}

