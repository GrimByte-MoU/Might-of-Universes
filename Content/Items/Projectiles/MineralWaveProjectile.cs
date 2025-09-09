using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class MineralWaveProjectile : ModProjectile
    {
        private float speedMultiplier = 1f;

        public override void SetDefaults()
        {
            Projectile.width = 150;
            Projectile.height = 150;
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Projectile.penetrate = 4;
            Projectile.timeLeft = 60;
            Projectile.light = 1f;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            speedMultiplier *= 0.99f;
            Projectile.velocity *= speedMultiplier;
            Projectile.rotation = Projectile.velocity.ToRotation();
            
            for (int i = 0; i < 2; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Mythril);
            }
            Lighting.AddLight(Projectile.Center, 0f, 1f, 0.3f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            var reaper = player.GetModPlayer<ReaperPlayer>();
            reaper.AddSoulEnergy(2f, target.Center);
        }
    }
}