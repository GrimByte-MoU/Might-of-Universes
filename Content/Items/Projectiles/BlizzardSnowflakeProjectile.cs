using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class BlizzardSnowflakeProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Projectile.penetrate = 3;
            Projectile.timeLeft = 180;
            Projectile.light = 1f;
            Projectile.aiStyle = 1;
            Projectile.scale = 0.3f;
        }

        public override void AI()
        {
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Snow);
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Ice);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
           target.AddBuff(BuffID.Frostburn, 120);
        }
    }
}