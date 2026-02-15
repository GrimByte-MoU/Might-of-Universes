using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MightofUniverses.Content.Items.Projectiles.EnemyProjectiles
{
    public class AegonJungleSigilBolt : MoUProjectile
    {
        public override void SafeSetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.aiStyle = 0;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();

            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.JungleGrass,
                    0f, 0f,
                    100,
                    Color.LimeGreen,
                    1.2f
                );
                dust.noGravity = true;
                dust.velocity *= 0.3f;
            }

            Lighting.AddLight(Projectile.Center, 0.5f, 1f, 0f);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            int duration = 300;

            target.AddBuff(BuffID.Poisoned, duration);
            target.AddBuff(BuffID.Weak, duration);

            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.JungleGrass, 0, 0, 100, Color.LimeGreen, 1.5f);
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 5; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.JungleGrass, 0, 0, 100, Color.LimeGreen, 1f);
            }
        }
    }
}