using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class EmberfangsBlade : MoUProjectile
    {
        private enum BladeState
        {
            Spinning = 0,
            Sweeping = 1,
            Returning = 2
        }

        private BladeState CurrentState
        {
            get => (BladeState)Projectile.ai[1];
            set => Projectile.ai[1] = (float)value;
        }

        private bool IsOuterBlade => Projectile.ai[0] == 1f;

        private float orbitAngle = 0f;
        private float orbitRadius;
        private float orbitSpeed;

        private float sweepStartAngle;
        private float sweepCurrentAngle;
        private float sweepProgress = 0f;
        private float sweepDuration;
        private const float targetSweepDistance = 250f;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SafeSetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.netImportant = true;
            Projectile.scale = 1.0f;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (!player.active || player.dead)
            {
                Projectile.Kill();
                return;
            }

            if (IsOuterBlade)
            {
                orbitRadius = 140f;
                orbitSpeed = 0.3f;
                sweepDuration = 25f;
            }
            else
            {
                orbitRadius = 100f;
                orbitSpeed = -0.2f;
                sweepDuration = 15f;
            }

            if (CurrentState == BladeState.Spinning)
            {
                SpinningBehavior(player);
            }
            else if (CurrentState == BladeState.Sweeping)
            {
                SweepingBehavior(player);
            }
            else if (CurrentState == BladeState.Returning)
            {
                ReturningBehavior(player);
            }

            EmitDust();
            Lighting.AddLight(Projectile.Center, 0.9f, 0.5f, 0.1f);
        }

        private void SpinningBehavior(Player player)
        {
            if (!player.channel)
            {
                CurrentState = BladeState.Sweeping;
                sweepProgress = 0f;
                
                Vector2 toMouse = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.UnitX);
                float mouseAngle = toMouse.ToRotation();
                
                if (IsOuterBlade)
                {
                    sweepStartAngle = mouseAngle - MathHelper.PiOver2;
                }
                else
                {
                    sweepStartAngle = mouseAngle + MathHelper.PiOver2;
                }
                
                sweepCurrentAngle = sweepStartAngle;
                Projectile.damage = (int)(Projectile.damage * 2 / (IsOuterBlade ? 0.75f : 1.5f));
                
                return;
            }

            orbitAngle += orbitSpeed;
            
            Vector2 offset = new Vector2(
                (float)Math.Cos(orbitAngle) * orbitRadius,
                (float)Math.Sin(orbitAngle) * orbitRadius
            );

            Projectile.Center = player.MountedCenter + offset;
            Projectile.rotation = orbitAngle;

            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.direction = player.DirectionTo(Main.MouseWorld).X > 0 ? 1 : -1;

            Projectile.timeLeft = 600;
        }

        private void SweepingBehavior(Player player)
        {
            sweepProgress++;

            if (sweepProgress >= sweepDuration)
            {
                CurrentState = BladeState.Returning;
                return;
            }

            float t = sweepProgress / sweepDuration;
            t = 1f - (float)Math.Pow(1f - t, 3);

            float sweepAmount = MathHelper.PiOver2 * t;
            
            if (IsOuterBlade)
            {
                sweepCurrentAngle = sweepStartAngle + sweepAmount;
            }
            else
            {
                sweepCurrentAngle = sweepStartAngle - sweepAmount;
            }

            float currentDistance = MathHelper.Lerp(orbitRadius, targetSweepDistance, t);

            Vector2 sweepOffset = new Vector2(
                (float)Math.Cos(sweepCurrentAngle) * currentDistance,
                (float)Math.Sin(sweepCurrentAngle) * currentDistance
            );

            Projectile.Center = player.MountedCenter + sweepOffset;
            Projectile.rotation = sweepCurrentAngle;
            Projectile.velocity = Vector2.Zero;
        }

        private void ReturningBehavior(Player player)
        {
            Vector2 toPlayer = player.Center - Projectile.Center;
            float distance = toPlayer.Length();

            if (distance < 30f)
            {
                Projectile.Kill();
                return;
            }

            toPlayer.Normalize();
            Projectile.velocity = toPlayer * 25f;
            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        private void EmitDust()
        {
            if (Main.rand.NextBool(3))
            {
                int dustType = IsOuterBlade ? DustID.Torch : DustID.FlameBurst;
                Dust dust = Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    dustType,
                    0,
                    0,
                    100,
                    default,
                    1.5f
                );
                dust.noGravity = true;
                dust.velocity *= 0.5f;
            }

            if (CurrentState == BladeState.Sweeping && Main.rand.NextBool(2))
            {
                Dust ember = Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.Torch,
                    0,
                    0,
                    100,
                    Color.OrangeRed,
                    2f
                );
                ember.noGravity = true;
                ember.velocity = Projectile.velocity * 0.3f;
            }
        }

        public override bool SafePreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 origin = texture.Size() * 0.5f;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;

            DrawChain(player.MountedCenter, Projectile.Center, lightColor);

            SpriteEffects flip = IsOuterBlade ? SpriteEffects.None : SpriteEffects.FlipVertically;

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float trailAlpha = 1f - (i / (float)Projectile.oldPos.Length);
                Vector2 trailPos = Projectile.oldPos[i] + Projectile.Size * 0.5f - Main.screenPosition;
                Color trailColor = Color.OrangeRed * trailAlpha * 0.5f;

                Main.EntitySpriteDraw(
                    texture,
                    trailPos,
                    null,
                    trailColor,
                    Projectile.oldRot[i],
                    origin,
                    Projectile.scale * (1f - i * 0.1f),
                    flip,
                    0
                );
            }

            Main.EntitySpriteDraw(
                texture,
                drawPos,
                null,
                lightColor,
                Projectile.rotation,
                origin,
                Projectile.scale,
                flip,
                0
            );

            Main.EntitySpriteDraw(
                texture,
                drawPos,
                null,
                Color.White * 0.5f,
                Projectile.rotation,
                origin,
                Projectile.scale,
                flip,
                0
            );

            return false;
        }

        private void DrawChain(Vector2 start, Vector2 end, Color lightColor)
        {
            Vector2 direction = end - start;
            float distance = direction.Length();
            
            if (distance < 10f)
                return;

            direction.Normalize();
            float rotation = direction.ToRotation();

            int numSegments = (int)(distance / 8f);
            
            Texture2D pixel = ModContent.Request<Texture2D>("Terraria/Images/Misc/Perlin").Value;
            
            for (int i = 0; i < numSegments; i++)
            {
                float progress = i / (float)numSegments;
                Vector2 position = Vector2.Lerp(start, end, progress);
                Color chainColor = Color.Lerp(Color.Gray, Color.OrangeRed, progress * 0.7f);
                
                Main.EntitySpriteDraw(
                    pixel,
                    position - Main.screenPosition,
                    new Rectangle(0, 0, 4, 4),
                    chainColor * 0.9f,
                    rotation,
                    new Vector2(2, 2),
                    new Vector2(8f / 4f, 2.5f / 4f),
                    SpriteEffects.None,
                    0
                );
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<CoreHeat>(), 180);

            for (int i = 0; i < 15; i++)
            {
                Dust dust = Dust.NewDustDirect(
                    target.position,
                    target.width,
                    target.height,
                    DustID.Torch,
                    0,
                    0,
                    100,
                    default,
                    2f
                );
                dust.noGravity = true;
                dust.velocity = Main.rand.NextVector2Circular(5f, 5f);
            }

            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item74, target.Center);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.Lerp(lightColor, Color.Orange, 0.5f);
        }
    }
}