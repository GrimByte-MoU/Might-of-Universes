using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class StationaryEclipseProjectile : MoUProjectile
    {
        private const int DAMAGE_INTERVAL = 20;
        private int damageTimer;

        public override void SafeSetDefaults()
        {
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 180;
            Projectile.light = 1f;
            Projectile.tileCollide = false;
            Projectile.alpha = 100;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0.8f, 0.3f, 0f);
            
            // Visual effects
            for (int i = 0; i < 2; i++)
            {
                Vector2 dustPos = Projectile.Center + new Vector2(
                    Main.rand.Next(-32, 33),
                    Main.rand.Next(-32, 33)
                );
                
                Dust.NewDust(dustPos, 4, 4, DustID.OrangeTorch, 0f, 0f, 100, default, 1.5f);
            }

            // Damage logic
            damageTimer++;
            if (damageTimer >= DAMAGE_INTERVAL)
            {
                damageTimer = 0;
                DamageNearbyEnemies();
            }

            // Fade out near end of lifetime
            if (Projectile.timeLeft < 30)
            {
                Projectile.alpha += 5;
            }
        }

        private void DamageNearbyEnemies()
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && !npc.friendly)
                {
                    float distance = Vector2.Distance(Projectile.Center, npc.Center);
                    if (distance <= Projectile.width / 2)
                    {
                        npc.StrikeNPC(new NPC.HitInfo
                        {
                            Damage = Projectile.damage,
                            Knockback = 0f,
                            HitDirection = 0
                        });
                    }
                }
            }
        }
    }
}
