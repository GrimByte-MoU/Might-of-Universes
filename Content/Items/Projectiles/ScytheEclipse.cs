using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Common;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Projectiles 
{
    public class ScytheEclipse : ModProjectile
    {
        private float rotationSpeed = 0.2f;
        private float distanceFromCenter = 50f;
        private Player Owner => Main.player[Projectile.owner];

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = false;
            Projectile.light = 1f;
        }

        public override void AI()
        {
            rotationSpeed += 0.005f;
            distanceFromCenter += 2f;

            // Update position relative to player in a circular motion
            float rotation = Projectile.ai[0] + rotationSpeed;
            Projectile.ai[0] = rotation;
            
            Vector2 offset = new Vector2(distanceFromCenter, 0).RotatedBy(rotation);
            Projectile.Center = Owner.Center + offset;
            Projectile.rotation += 0.4f;

            // Visual effects
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch);
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.OrangeTorch);
            Lighting.AddLight(Projectile.Center, 1f, 0.5f, 0f); // Orange
        }
    }
}