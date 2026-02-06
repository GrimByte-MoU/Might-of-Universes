using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MightofUniverses.Common;
using System;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class KasurikamaGiantSickle : MoUProjectile
    {
        private const float SpinRadius = 175f;
        private const float SpinSpeed = 0.5f;
        private float currentAngle = 0f;
        private bool hasCompletedSpin = false;

        public override void SafeSetDefaults()
        {
            Projectile.width = 96;
            Projectile.height = 96;
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Projectile.penetrate = -1;
            Projectile.timeLeft = 180;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.scale = 2.0f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (!player.active || player.dead)
            {
                Projectile.Kill();
                return;
            }

            currentAngle += SpinSpeed;

            if (currentAngle >= MathHelper.TwoPi && !hasCompletedSpin)
            {
                hasCompletedSpin = true;
                Projectile.Kill();
                return;
            }

            Vector2 offset = new Vector2(
                (float)Math.Cos(currentAngle) * SpinRadius,
                (float)Math.Sin(currentAngle) * SpinRadius
            );

            Projectile.Center = player.MountedCenter + offset;
            Projectile.rotation = currentAngle + MathHelper.PiOver4; // 45 degree tilt

            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Iron, 0f, 0f, 100, Color.DarkGray, 3.0f);
                dust.noGravity = true;
                dust.velocity = Main.rand.NextVector2Circular(5f, 5f);
            }

            Lighting.AddLight(Projectile.Center, 0.5f, 0.5f, 0.5f);
        }

        public override bool SafePreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Texture2D texture = ModContent.Request<Texture2D>("MightofUniverses/Content/Items/Projectiles/KasurikamaSpinProjectile").Value;
            Texture2D chainTexture = ModContent.Request<Texture2D>("Terraria/Images/Misc/Perlin").Value;

            Vector2 playerCenter = player.MountedCenter - Main.screenPosition;
            Vector2 projectileCenter = Projectile.Center - Main.screenPosition;

            DrawChain(chainTexture, playerCenter, projectileCenter, lightColor);

            Vector2 drawOrigin = texture.Size() * 0.5f;
            
            Main.EntitySpriteDraw(
                texture,
                projectileCenter,
                null,
                Color.White * 0.5f,
                Projectile.rotation,
                drawOrigin,
                Projectile.scale * 1.1f,
                SpriteEffects.None,
                0
            );

            Main.EntitySpriteDraw(
                texture,
                projectileCenter,
                null,
                lightColor,
                Projectile.rotation,
                drawOrigin,
                Projectile.scale,
                SpriteEffects.None,
                0
            );

            return false;
        }

        private void DrawChain(Texture2D chainTexture, Vector2 start, Vector2 end, Color lightColor)
        {
            Vector2 direction = end - start;
            float distance = direction.Length();
            direction.Normalize();

            int chainSegments = (int)(distance / 12f);
            for (int i = 0; i < chainSegments; i++)
            {
                Vector2 chainPos = start + direction * (i * 12f);
                Main.EntitySpriteDraw(
                    chainTexture,
                    chainPos,
                    new Rectangle(0, 0, 2, 2),
                    Color.DarkGray,
                    direction.ToRotation() + MathHelper.PiOver2,
                    new Vector2(1, 1),
                    3f,
                    SpriteEffects.None,
                    0
                );
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Bleeding, 240);

            for (int i = 0; i < 15; i++)
            {
                Dust dust = Dust.NewDustDirect(target.position, target.width, target.height, DustID.Blood, 0f, 0f, 100, default, 2.5f);
                dust.noGravity = true;
                dust.velocity = Main.rand.NextVector2Circular(6f, 6f);
            }
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);

            for (int i = 0; i < 30; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, Color.Gray, 2.5f);
                dust.noGravity = true;
                dust.velocity = Main.rand.NextVector2Circular(8f, 8f);
            }
        }
    }
}