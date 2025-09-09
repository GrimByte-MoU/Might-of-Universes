using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class WardenCrossProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Projectile.penetrate = 9;
            Projectile.timeLeft = 180;
            Projectile.light = 1f;
        }

        public override void AI()
        {
            Projectile.rotation += 0.4f;
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Bone);
            Lighting.AddLight(Projectile.Center, 0f, 0f, 1f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            var reaper = player.GetModPlayer<ReaperPlayer>();
            reaper.AddSoulEnergy(4f, target.Center);
        }
    }
}
