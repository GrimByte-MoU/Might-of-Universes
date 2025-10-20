using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class ClockworkGear : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 999999;
            Projectile.scale *= 1.5f;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.active || player.dead)
            {
                Projectile.Kill();
                return;
            }

            // Orbit logic
            float radius = 240f; // 15 tiles
            float orbitSpeed = 0.03f;
            float angleOffset = MathHelper.ToRadians(120 * Projectile.ai[0]); // Gear 0/1/2 → 0°, 120°, 240°
            float rotation = Main.GameUpdateCount * orbitSpeed + angleOffset;

            Projectile.Center = player.Center + new Vector2(radius, 0f).RotatedBy(rotation);

            // Light emission — warm brass-like glow (Reddish-orange)
            Lighting.AddLight(Projectile.Center, 0.9f, 0.6f, 0.2f); // RGB values for a brassy look

            // Optional: spin gear visually
            //Projectile.rotation += 0.1f;
        }

        public override bool? CanHitNPC(NPC target) => false;

        public void TriggerHook(Vector2 toTarget)
        {
            if (Main.myPlayer == Projectile.owner)
            {
                Vector2 direction = toTarget.SafeNormalize(Vector2.UnitX) * 12f;
                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    Projectile.Center,
                    direction,
                    ModContent.ProjectileType<ClockworkHook>(),
                    100,
                    1f,
                    Projectile.owner
                );
            }
        }
    }
}

