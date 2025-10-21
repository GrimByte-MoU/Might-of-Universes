using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class TeslaSpear : MoUProjectile
    {
        public override void SafeSetDefaults()
        {
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Projectile.penetrate = -1;
            Projectile.timeLeft = 180;
            Projectile.light = 1f;
        }

         public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Electrified, 300);
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Electric);
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.MartianSaucerSpark);
            Lighting.AddLight(Projectile.Center, 0f, 0.5f, 2f);
        }
    }
}