using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MightofUniverses.Content.Items.Buffs;
using System;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class IceAgeBlizzardBall : MoUProjectile
    {
        private enum BallState
        {
            Orbiting,
            Firing
        }

        private BallState CurrentState
        {
            get => (BallState)Projectile.ai[0];
            set => Projectile.ai[0] = (float)value;
        }

        private float orbitAngle;
        private float orbitRadius = 80f;
        private int orbitTimer = 0;
        private const int orbitDuration = 120;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SafeSetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Projectile.penetrate = 4;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
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

            if (CurrentState == BallState.Orbiting)
            {
                OrbitBehavior(player);
            }
            else if (CurrentState == BallState.Firing)
            {
                FiringBehavior();
            }

            Projectile.rotation += 0.3f;

            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.IceTorch, 0f, 0f, 100, Color.Cyan, 2.5f);
                dust.noGravity = true;
                dust.velocity *= 0.3f;
            }

            Lighting.AddLight(Projectile.Center, 0.5f, 0.9f, 1.2f);
        }

        private void OrbitBehavior(Player player)
        {
            orbitTimer++;

            float ballIndex = Projectile.ai[1];
            float rotationSpeed = 0.08f;
            orbitAngle = MathHelper.TwoPi / 8f * ballIndex + (orbitTimer * rotationSpeed);

            Vector2 offset = new Vector2(
                (float)Math.Cos(orbitAngle) * orbitRadius,
                (float)Math.Sin(orbitAngle) * orbitRadius
            );

            Projectile.Center = player.MountedCenter + offset;
            Projectile.velocity = Vector2.Zero;

            if (orbitTimer >= orbitDuration)
            {
                Vector2 toMouse = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.UnitX);
                float mouseAngle = toMouse.ToRotation();
                float normalizedOrbitAngle = orbitAngle % MathHelper.TwoPi;
                float normalizedMouseAngle = mouseAngle % MathHelper.TwoPi;
                if (normalizedOrbitAngle < 0) normalizedOrbitAngle += MathHelper.TwoPi;
                if (normalizedMouseAngle < 0) normalizedMouseAngle += MathHelper.TwoPi;
                
                float angleDiff = Math.Abs(normalizedOrbitAngle - normalizedMouseAngle);
                if (angleDiff > MathHelper.Pi) angleDiff = MathHelper.TwoPi - angleDiff;

                if (angleDiff < MathHelper.PiOver4)
                {
                    CurrentState = BallState.Firing;
                    Vector2 fireDirection = (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.UnitX);
                    Projectile.velocity = fireDirection * 20f;
                    Projectile.tileCollide = true;

                    SoundEngine.PlaySound(SoundID.Item30, Projectile.Center);
                }
            }
        }

        private void FiringBehavior()
        {
        }

        public override bool SafePreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 drawOrigin = texture.Size() * 0.5f;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float trailAlpha = 1f - (i / (float)Projectile.oldPos.Length);
                Vector2 trailPos = Projectile.oldPos[i] + Projectile.Size * 0.5f - Main.screenPosition;
                Color trailColor = Color.Cyan * trailAlpha * 0.6f;

                Main.EntitySpriteDraw(texture, trailPos, null, trailColor, Projectile.oldRot[i], drawOrigin, Projectile.scale * 0.9f, SpriteEffects.None, 0);
            }

            Main.EntitySpriteDraw(texture, drawPos, null, lightColor, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(texture, drawPos, null, Color.White * 0.7f, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);

            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<SheerCold>(), 300);

            for (int i = 0; i < 20; i++)
            {
                Dust dust = Dust.NewDustDirect(target.position, target.width, target.height, DustID.IceTorch, 0f, 0f, 100, Color.Cyan, 3.0f);
                dust.noGravity = true;
                dust.velocity = Main.rand.NextVector2Circular(6f, 6f);
            }
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item27, Projectile.Center);

            for (int i = 0; i < 30; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.IceTorch, 0f, 0f, 100, Color.Cyan, 3.5f);
                dust.noGravity = true;
                dust.velocity = Main.rand.NextVector2Circular(8f, 8f);
            }
        }
    }
}