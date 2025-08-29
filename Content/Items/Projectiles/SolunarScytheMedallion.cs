using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Common;

namespace MightofUniverses.Content.Items.Projectiles 
{
    public class SolunarScytheMedallion : ModProjectile
    {
        private float rotationSpeed = 0.1f;
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
            rotationSpeed += 0.002f;
            distanceFromCenter += 2f;

            // Update position relative to player in a circular motion
            float rotation = Projectile.ai[0] + rotationSpeed;
            Projectile.ai[0] = rotation;
            
            Vector2 offset = new Vector2(distanceFromCenter, 0).RotatedBy(rotation);
            Projectile.Center = Owner.Center + offset;
            Projectile.rotation += 0.2f;

            // Visual effects
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.PurpleTorch);
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.OrangeTorch);
            
            Lighting.AddLight(Projectile.Center, 0.8f, 0f, 0.8f); // Purple
            Lighting.AddLight(Projectile.Center, 1f, 0.5f, 0f); // Orange
        }
    }
}
