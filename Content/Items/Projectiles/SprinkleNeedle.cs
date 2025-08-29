using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class SprinkleNeedle : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 60;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
        }

        public override void AI()
        {
            Projectile.velocity.Y += 0.1f;
            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        public override Color? GetAlpha(Color lightColor)
        {
            Color[] sprinkleColors = new Color[]
            {
                Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.Blue, Color.Violet
            };
            return sprinkleColors[Projectile.whoAmI % sprinkleColors.Length];
        }
    }
}
