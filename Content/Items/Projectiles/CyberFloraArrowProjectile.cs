using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class CyberFloraArrowProjectile : MoUProjectile
    {
        public override void SafeSetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.arrow = true;
            Projectile.aiStyle = ProjAIStyleID.Arrow;
            AIType = ProjectileID.WoodenArrowFriendly;
        }

        public override void OnSpawn(IEntitySource source)
        {
            for (int i = 0; i < 2; i++)
            {
                Vector2 offset = Main.rand.NextVector2Circular(30f, 30f);
                Vector2 velocity = Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(15f)) * 0.9f;

                Projectile.NewProjectile(
                    source,
                    Projectile.Center + offset,
                    velocity,
                    ModContent.ProjectileType<CyberFloraFlower>(),
                    (int)(Projectile.damage * 0.5f),
                    Projectile.knockBack * 0.5f,
                    Projectile.owner
                );
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<NaturesToxin>(), 180);
        }

        public override void AI()
        {
        Projectile.rotation = Projectile.velocity.ToRotation();
            if (Main.rand.NextBool(5))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.JungleGrass);
                dust.noGravity = true;
                dust.scale = 0.8f;
            }
        }
    }
}