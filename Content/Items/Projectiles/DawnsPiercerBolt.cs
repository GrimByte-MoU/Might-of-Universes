// Content/Projectiles/DawnsPiercerBolt.cs
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class DawnsPiercerBolt : MoUProjectile
    {

        public override void SafeSetDefaults()
        {
            Projectile.width        = 8;
            Projectile.height       = 8;
            Projectile.aiStyle      = 0;
            Projectile.friendly     = true;
            Projectile.hostile      = false;
            Projectile.penetrate    = 3;
            Projectile.DamageType   = DamageClass.Ranged;
            Projectile.timeLeft = 60;
            Projectile.ignoreWater  = true;
            Projectile.tileCollide  = true;
        }

        public override void AI()
        {
            float phase   = Projectile.ai[0];
            float speed   = Projectile.velocity.Length();
            float progress= (60 - Projectile.timeLeft) / 60f * MathHelper.TwoPi * 2;
            float angle   = phase + progress;
            Vector2 perp  = Projectile.velocity.SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.PiOver2);
            Vector2 offset= perp * (float)(Math.Sin(angle) * 6f);

            Projectile.position += offset;
            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Sunfire>(), 180);
            target.AddBuff(BuffID.Daybreak, 180);
        }
    }
}
