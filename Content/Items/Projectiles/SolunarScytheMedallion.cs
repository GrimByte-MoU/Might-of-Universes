using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Common;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Projectiles 
{
    public class SolunarScytheMedallion : MoUProjectile
    {
        private float rotationSpeed = 0.1f;
        private float distanceFromCenter = 50f;
        private Player Owner => Main.player[Projectile.owner];

        public override void SafeSetDefaults()
        {
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

            float rotation = Projectile.ai[0] + rotationSpeed;
            Projectile.ai[0] = rotation;
            
            Vector2 offset = new Vector2(distanceFromCenter, 0).RotatedBy(rotation);
            Projectile.Center = Owner.Center + offset;
            Projectile.rotation += 0.2f;

            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.PurpleTorch);
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.OrangeTorch);
            
            Lighting.AddLight(Projectile.Center, 0.8f, 0f, 0.8f);
            Lighting.AddLight(Projectile.Center, 1f, 0.5f, 0f);
        }
    }
}
