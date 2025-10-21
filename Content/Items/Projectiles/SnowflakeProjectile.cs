using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class SnowflakeProjectile : MoUProjectile
    {
        public override void SafeSetDefaults()
        {
           Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Projectile.penetrate = 2;
            Projectile.timeLeft = 180;
            Projectile.light = 1f;
            Projectile.aiStyle = -1;
        }

        public override void AI()
        {
            //Projectile.rotation += 0.4f;
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Snow);
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Ice);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Frostburn, 180);
            Player player = Main.player[Projectile.owner];
            var reaper = player.GetModPlayer<ReaperPlayer>();
            reaper.AddSoulEnergy(5f, target.Center);
        }
    }
}