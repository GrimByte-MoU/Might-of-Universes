using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System;
using Terraria.ID;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Projectiles
{
    // ai[0] = phase (0 or π)
    // localAI[0] ticks, localAI[1] previous sine
    public class SolunarProjectile : MoUProjectile
    {
        private const float HelixSpeed = 0.65f;
        private const float HelixAmplitude = 6f;

        public override void SafeSetDefaults()
        {
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Projectile.penetrate = 2;
            Projectile.timeLeft = 180;
            Projectile.light = 0.8f;
            Projectile.tileCollide = true;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();

            Vector2 forward = Projectile.velocity.SafeNormalize(Vector2.UnitX);
            Vector2 perp = forward.RotatedBy(MathHelper.PiOver2);

            float ticks = Projectile.localAI[0]++;
            float prev = Projectile.localAI[1];
            float sine = (float)Math.Sin(ticks * HelixSpeed + Projectile.ai[0]);
            float delta = sine - prev;

            Projectile.position += perp * (delta * HelixAmplitude);
            Projectile.localAI[1] = sine;

            if (Main.rand.NextBool(4))
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.PurpleTorch, 0f, 0f, 150);
            if (Main.rand.NextBool(5))
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.OrangeTorch, 0f, 0f, 150);

            Lighting.AddLight(Projectile.Center, 0.9f, 0.3f, 0.9f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Main.player[Projectile.owner].GetModPlayer<ReaperPlayer>().AddSoulEnergy(1f, target.Center);
        }
    }
}