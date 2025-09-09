using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Common;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class GlitchBlast : ModProjectile
    {
        private float speedMultiplier = 1f;

        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Projectile.penetrate = 1;
            Projectile.timeLeft = 90;
            Projectile.light = 1f;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Projectile.velocity *= speedMultiplier;
            Projectile.rotation = Projectile.velocity.ToRotation();
            
            for (int i = 0; i < 2; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.RedTorch);
            }
        }
    }
}