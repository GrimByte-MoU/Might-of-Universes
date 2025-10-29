using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class PrismaticBolt : ModProjectile
    {
        private float hue;

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Summon;
        }

        public override void AI()
        {
            hue += 0.02f;
            if (hue > 1f)
                hue -= 1f;
            Color rainbowColor = Main.hslToRgb(hue, 1f, 0.5f);
            Lighting.AddLight(Projectile.Center, rainbowColor.ToVector3() * 0.7f);
            Projectile.rotation += 0.4f;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, texture.Size() / 2, Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<PrismaticRend>(), 180);
        }
    }
}