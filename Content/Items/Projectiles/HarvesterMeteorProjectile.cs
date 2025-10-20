using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class HarvesterMeteorProjectile : ModProjectile
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
        }

        public override void AI()
        {
            //Projectile.rotation += 0.5f;
            for (int i = 0; i < 3; i++)
            {
                Projectile.rotation = Projectile.velocity.ToRotation();
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Meteorite);
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch);
            }
            Lighting.AddLight(Projectile.Center, 1.2f, 0.6f, 0f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
         target.AddBuff(BuffID.OnFire3, 600);
        }
    }
}
