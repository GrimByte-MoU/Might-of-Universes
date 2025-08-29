using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class TerraiumLightning : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 60;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 60;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
            Projectile.alpha = 255;
        }

        public override void AI()
        {
            int targetIndex = (int)Projectile.ai[0];
            int delay = (int)Projectile.ai[1];

            // Wait before striking
            if (Projectile.localAI[0]++ < delay)
                return;

            if (targetIndex >= 0 && targetIndex < Main.maxNPCs && Main.npc[targetIndex].active)
            {
                NPC target = Main.npc[targetIndex];

                // Position directly above target, then strike down
                Vector2 strikeStart = target.Center + new Vector2(0, -600f);
                Vector2 strikeEnd = target.Center;

                // Each tick, lerp down to target
                float progress = (Projectile.localAI[0] - delay) / 10f;
                progress = MathHelper.Clamp(progress, 0f, 1f);
                Projectile.Center = Vector2.Lerp(strikeStart, strikeEnd, progress);

                // Spawn electric dust trail
                for (int i = 0; i < 5; i++)
                {
                    Vector2 dustPos = Vector2.Lerp(strikeStart, strikeEnd, Main.rand.NextFloat());
                    Dust.NewDustPerfect(dustPos, DustID.Electric, Vector2.Zero, 150, Color.LightBlue, 1.5f).noGravity = true;
                }

                if (progress >= 1f)
                {
                    var hitInfo = new NPC.HitInfo()
                    {
                        Damage = Projectile.damage,
                        Knockback = 0f,
                        HitDirection = 0,
                        Crit = false
                    };
                    target.StrikeNPC(hitInfo);
                    Projectile.Kill();
                }
            }
            else
            {
                Projectile.Kill(); // if target despawns
            }
        }
    }
}
