using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using MightofUniverses.Common;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class FrigidHeartShard : MoUProjectile
    {
        public override void SafeSetDefaults()
        {
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = ModContent.GetInstance<PacifistDamageClass>();
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            float orbitRadius = 40f;

            if (Projectile.ai[0] == 0)
            {
                Projectile.ai[0] = Main.rand.NextFloat(MathHelper.TwoPi);
            }

            Projectile.ai[0] += 0.05f;
            Vector2 offset = new Vector2(orbitRadius, 0f).RotatedBy(Projectile.ai[0]);
            Projectile.Center = player.Center + offset;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.Kill();
        }

        public override void Kill(int timeLeft)
        {
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ProjectileID.IceBlock, 100, 0, Projectile.owner);
        }
    }
}
