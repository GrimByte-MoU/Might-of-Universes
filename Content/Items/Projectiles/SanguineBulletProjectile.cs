using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class SanguineBulletProjectile : MoUProjectile
    {
        public override void SafeSetDefaults()
        {
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.light = 0.5f;
            Projectile.extraUpdates = 1;
            AIType = ProjectileID.Bullet;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0.5f, 0f, 0f);
            
            if (Main.rand.NextBool(2))
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 
                    DustID.Blood, 0f, 0f, 100, default, 1f);
            }
            
            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
{
    Player player = Main.player[Projectile.owner];
    
    // 8% lifesteal (balanced)
    int heal = damageDone / 12;  // ~8% lifesteal
    player.statLife = System.Math.Min(player.statLife + heal, player.statLifeMax2);
    player.HealEffect(heal);
    
    // Visual blood particles
    for (int i = 0; i < 3; i++)
    {
        Vector2 vel = (player.Center - target.Center).SafeNormalize(Vector2.UnitY) * 4f;
        int dust = Dust.NewDust(target.Center, 0, 0, DustID.Blood, 
            vel.X, vel.Y, 100, default, 1.2f);
        Main.dust[dust].noGravity = true;
    }
}
    }
}
