using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class GreedySoul : MoUProjectile
    {
        private float speedMultiplier = 1f;

        public override void SafeSetDefaults()
        {
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Projectile.penetrate = 4;
            Projectile.timeLeft = 300;
            Projectile.light = 1f;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            
            for (int i = 0; i < 2; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GoldCoin);
                 Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemEmerald);
            }
            Lighting.AddLight(Projectile.Center, 0f, 0.5f, 0f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            player.lifeSteal += 0.1f;
        }
    }
}