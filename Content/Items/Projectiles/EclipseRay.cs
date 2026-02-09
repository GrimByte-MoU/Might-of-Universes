using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System;
using Terraria.ID;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class EclipseRay : MoUProjectile
    {
        private const float HelixSpeed = 0.80f;
        private const float HelixAmplitude = 8f;

        public override void SafeSetDefaults()
        {
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Projectile.penetrate = 3;
            Projectile.timeLeft = 180;
            Projectile.light = 1f;
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

            if (Main.rand.NextBool(3))
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 140);
            if (Main.rand.NextBool(4))
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.OrangeTorch, 0f, 0f, 140);

            Lighting.AddLight(Projectile.Center, 1f, 0.5f, 0.1f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Main.player[Projectile.owner].GetModPlayer<ReaperPlayer>().AddSoulEnergy(2f, target.Center);
        }
    }
}