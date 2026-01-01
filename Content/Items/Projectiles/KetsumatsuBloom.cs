using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Common;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class KetsumatsuBloom : MoUProjectile
    {
        public override void SafeSetDefaults()
        {
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 20; // ~0.33 seconds
            Projectile.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Projectile.tileCollide = false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Explode();
        }

        public override void Kill(int timeLeft)
        {
            Explode();
        }

        private void Explode()
        {
            if (Projectile.localAI[0] == 1f) return; // Prevent double-trigger
            Projectile.localAI[0] = 1f;

            // Radial burst of 24 sakura petals
            for (int i = 0; i < 24; i++)
            {
                Vector2 velocity = Vector2.UnitX.RotatedBy(MathHelper.TwoPi * i / 24f) * 8f;
                Projectile.NewProjectile(
                    Projectile.GetSource_Death(),
                    Projectile.Center,
                    velocity,
                    ModContent.ProjectileType<KetsumatsuBloomPetal>(),
                    (int)(Projectile.damage * 0.66f),
                    Projectile.knockBack,
                    Projectile.owner
                );
            }

            // Bloom dust effect
            for (int i = 0; i < 24; i++)
            {
                Dust.NewDustPerfect(Projectile.Center, DustID.PinkCrystalShard, Main.rand.NextVector2Circular(6f, 6f), 150, Color.LightPink, 1.5f).noGravity = true;
            }
        }
    public override void AI() {
    // Add light pink glow at projectile’s center
    Lighting.AddLight(Projectile.Center, 1.0f, 0.6f, 0.8f);
}

}

}
