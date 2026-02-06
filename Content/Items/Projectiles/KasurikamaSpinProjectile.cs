using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MightofUniverses.Common;
using MightofUniverses.Common.Players;
using System;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class KasurikamaSpinProjectile : MoUProjectile
    {
        private const float SpinRadius = 90f;
        private const float SpinSpeed = 0.25f;
        private float currentAngle = 0f;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
        }

        public override void SafeSetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Projectile.penetrate = -1;
            Projectile.timeLeft = 3600;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.ownerHitCheck = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (!player.active || player.dead || !player.channel)
            {
                Projectile.Kill();
                return;
            }

            player.itemTime = 2;
            player.itemAnimation = 2;
            player.heldProj = Projectile.whoAmI;

            currentAngle += SpinSpeed;
            if (currentAngle >= MathHelper.TwoPi)
            {
                currentAngle -= MathHelper.TwoPi;
            }

            Vector2 offset = new Vector2(
                (float)Math.Cos(currentAngle) * SpinRadius,
                (float)Math.Sin(currentAngle) * SpinRadius
            );

            Projectile.Center = player.MountedCenter + offset;
            Projectile.rotation = currentAngle + MathHelper.PiOver4; // 45 degree tilt

            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Iron, 0f, 0f, 100, Color.Gray, 1.5f);
                dust.noGravity = true;
                dust.velocity *= 0.3f;
            }
        }

        public override bool SafePreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Texture2D chainTexture = ModContent.Request<Texture2D>("Terraria/Images/Misc/Perlin").Value;

            Vector2 playerCenter = player.MountedCenter - Main.screenPosition;
            Vector2 projectileCenter = Projectile.Center - Main.screenPosition;

            DrawChain(chainTexture, playerCenter, projectileCenter, lightColor);

            Vector2 drawOrigin = texture.Size() * 0.5f;
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

            int chainSegments = (int)(distance / 8f);
            for (int i = 0; i < chainSegments; i++)
            {
                Vector2 chainPos = start + direction * (i * 8f);
                Main.EntitySpriteDraw(
                    chainTexture,
                    chainPos,
                    new Rectangle(0, 0, 2, 2),
                    Color.Gray * 0.8f,
                    direction.ToRotation() + MathHelper.PiOver2,
                    new Vector2(1, 1),
                    2f,
                    SpriteEffects.None,
                    0
                );
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            var reaper = player.GetModPlayer<ReaperPlayer>();
            reaper.AddSoulEnergy(4f, target.Center);

            target.AddBuff(BuffID.Bleeding, 120);

            for (int i = 0; i < 5; i++)
            {
                Dust dust = Dust.NewDustDirect(target.position, target.width, target.height, DustID.Blood, 0f, 0f, 100, default, 1.5f);
                dust.noGravity = true;
                dust.velocity = Main.rand.NextVector2Circular(3f, 3f);
            }
        }
    }
}