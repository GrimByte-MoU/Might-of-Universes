using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Content.Rarities;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Content.Items.Projectiles;

namespace MightofUniverses.Content.Items.Accessories
{
    public class WindQuiver : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.sellPrice(platinum: 1);
            Item.rare = ModContent.RarityType<TerraiumRarity>();
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Ranged) += 0.20f;
            
            player.ammoCost75 = true;
            if (Main.rand.NextBool(3))
            {
                player.ammoCost80 = false;
            }
            
            player.aggro -= 400;
            
            var quiverPlayer = player.GetModPlayer<WindQuiverPlayer>();
            quiverPlayer.hasWindQuiver = true;
            quiverPlayer.arrowSpeedBonus = 1.0f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.StalkersQuiver, 1)
                .AddIngredient(ItemID.MoltenQuiver, 1)
                .AddIngredient(ItemID.ShroomiteBar, 10)
                .AddIngredient(ModContent.ItemType<TerraiumBar>(), 7)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }

    public class WindQuiverPlayer : ModPlayer
    {
        public bool hasWindQuiver = false;
        public float arrowSpeedBonus = 0f;

        public override void ResetEffects()
        {
            hasWindQuiver = false;
            arrowSpeedBonus = 0f;
        }
    }

    public class WindQuiverProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (projectile.owner < 0 || projectile.owner >= Main.maxPlayers)
                return;

            Player owner = Main.player[projectile.owner];
            var quiverPlayer = owner.GetModPlayer<WindQuiverPlayer>();
            
            if (!quiverPlayer.hasWindQuiver)
                return;

            if (!IsArrowProjectile(projectile))
                return;

            projectile.velocity *= 1f + quiverPlayer.arrowSpeedBonus;
        }

        public override void AI(Projectile projectile)
        {
            if (projectile.owner < 0 || projectile.owner >= Main.maxPlayers)
                return;

            Player owner = Main.player[projectile.owner];
            if (!owner.GetModPlayer<WindQuiverPlayer>().hasWindQuiver)
                return;

            if (!IsArrowProjectile(projectile))
                return;

            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(
                    projectile.position,
                    projectile.width,
                    projectile.height,
                    DustID.Electric,
                    0f, 0f, 100,
                    Color.Cyan,
                    1.2f
                );
                dust.noGravity = true;
                dust.velocity *= 0.3f;
            }

            Lighting.AddLight(projectile.Center, 0.2f, 0.5f, 1.0f);
        }

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (projectile.owner < 0 || projectile.owner >= Main.maxPlayers)
                return;

            Player owner = Main.player[projectile.owner];
            if (!owner.GetModPlayer<WindQuiverPlayer>().hasWindQuiver)
                return;

            if (!IsArrowProjectile(projectile))
                return;

            target.AddBuff(ModContent.BuffType<DeltaShock>(), 180);

            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(
                    target.position,
                    target.width,
                    target.height,
                    DustID.Electric,
                    0f, 0f, 100,
                    Color.Yellow,
                    1.5f
                );
                dust.noGravity = true;
                dust.velocity = Main.rand.NextVector2Circular(4f, 4f);
            }
        }

        private bool IsArrowProjectile(Projectile projectile)
        {
            if (projectile.arrow)
                return true;

            int type = projectile.type;
            
            if (type == ProjectileID.WoodenArrowFriendly ||
                type == ProjectileID.FlamingArrow ||
                type == ProjectileID.UnholyArrow ||
                type == ProjectileID.JestersArrow ||
                type == ProjectileID.HellfireArrow ||
                type == ProjectileID.HolyArrow ||
                type == ProjectileID.CursedArrow ||
                type == ProjectileID.FrostburnArrow ||
                type == ProjectileID.ChlorophyteArrow ||
                type == ProjectileID.IchorArrow ||
                type == ProjectileID.VenomArrow ||
                type == ProjectileID.BoneArrow ||
                type == ProjectileID.BoneArrowFromMerchant ||
                type == ProjectileID.MoonlordArrow ||
                type == ProjectileID.DD2PhoenixBowShot)
                return true;

            if (type == ModContent.ProjectileType<CyberFloraArrowProjectile>() ||
                type == ModContent.ProjectileType<CyberFloraFlower>() ||
                type == ModContent.ProjectileType<DeltaStormArrowProjectile>() ||
                type == ModContent.ProjectileType<PyroclastArrow>() ||
                type == ModContent.ProjectileType<GaiasArrow>() ||
                type == ModContent.ProjectileType<GaiasShard>() ||
                type == ModContent.ProjectileType<MoltenCoreArrowProjectile>() ||
                type == ModContent.ProjectileType<NegativeColdArrowProjectile>())
                return true;

            return false;
        }
    }
}