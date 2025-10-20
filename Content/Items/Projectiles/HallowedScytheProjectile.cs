using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class HallowedScytheProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Projectile.penetrate = 6;
            Projectile.timeLeft = 180;
            Projectile.light = 1f;
        }

        public override void AI()
        {
            //Projectile.rotation += 0.4f;
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.HallowedTorch);
            Lighting.AddLight(Projectile.Center, 1f, 1f, 1f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            var reaper = player.GetModPlayer<ReaperPlayer>();
            reaper.AddSoulEnergy(3f, target.Center);
        }
    }
}