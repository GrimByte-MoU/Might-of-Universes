using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles.EnemyProjectiles
{
    public class HallowedSpear : ModProjectile
    {
        // AI states
        private enum AIState
        {
            Moving,
            Spinning,
            Homing
        }

        private AIState State
        {
            get => (AIState)Projectile.ai[0];
            set => Projectile.ai[0] = (float)value;
        }

        private float StateTimer
        {
            get => Projectile.ai[1];
            set => Projectile. ai[1] = value;
        }

        private Vector2 targetPosition;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 8;
            Projectile.aiStyle = -1;
            Projectile.hostile = true;
            Projectile. friendly = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.alpha = 0;
            Projectile.light = 0.8f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
        }

        public override void AI()
        {
            StateTimer++;

            switch (State)
            {
                case AIState.Moving:
                    AI_Moving();
                    break;

                case AIState.Spinning:
                    AI_Spinning();
                    break;

                case AIState.Homing:
                    AI_Homing();
                    break;
            }

            // Lighting effect
            Lighting.AddLight(Projectile.Center, 0.9f, 0.8f, 0.3f);

            // Dust trail
            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.GoldCoin,
                    0f, 0f, 100,
                    default(Color),
                    1f
                );
                dust.noGravity = true;
                dust.velocity *= 0.2f;
            }
        }

        private void AI_Moving()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();

            // After traveling ~5 blocks (80 pixels), stop
            if (StateTimer >= 20) // ~0.33 seconds at 60fps
            {
                // Store player position when stopping
                Player target = Main.player[Player.FindClosest(Projectile.position, Projectile.width, Projectile.height)];
                targetPosition = target.Center;

                // Stop moving
                Projectile.velocity = Vector2.Zero;
                Projectile.tileCollide = false; // Don't collide while spinning

                // Switch to spinning
                State = AIState.Spinning;
                StateTimer = 0;
            }
        }

        private void AI_Spinning()
        {
            // Rapid spin
            Projectile.rotation += 0.3f;

            // Spin for ~1 second
            if (StateTimer >= 60)
            {
                // Calculate direction to player
                Player target = Main.player[Player.FindClosest(Projectile.position, Projectile.width, Projectile.height)];
                Vector2 direction = (target. Center - Projectile.Center).SafeNormalize(Vector2.UnitX);

                // Launch at player
                Projectile.velocity = direction * 16f; // Fast homing speed

                // Switch to homing
                State = AIState.Homing;
                StateTimer = 0;
            }
        }

        private void AI_Homing()
        {
            // Point in direction of movement
            Projectile.rotation = Projectile. velocity.ToRotation();

            // Gentle homing (not too aggressive)
            Player target = Main.player[Player. FindClosest(Projectile. position, Projectile.width, Projectile.height)];
            Vector2 direction = (target.Center - Projectile.Center).SafeNormalize(Vector2.Zero);
            
            // Slight homing adjustment
            Projectile.velocity = Vector2.Lerp(Projectile.velocity, direction * 16f, 0.02f);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            int rebukeDuration = 60; // 1 second (Normal)
            int subjugatedDuration = 30; // 0.5 seconds (Normal)

            if (Main.expertMode)
            {
                rebukeDuration = 120; // 2 seconds
                subjugatedDuration = 60; // 1 second
            }

            if (Main.masterMode)
            {
                rebukeDuration = 180; // 3 seconds
                subjugatedDuration = 90; // 1.5 seconds
            }

            // Apply custom debuffs (you'll need to create these)
            target.AddBuff(ModContent.BuffType<RebukingLight>(), rebukeDuration);
            target.AddBuff(ModContent.BuffType<Subjugated>(), subjugatedDuration);
        }

        public override void Kill(int timeLeft)
        {
            // Golden explosion
            for (int i = 0; i < 20; i++)
            {
                Dust dust = Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.GoldCoin,
                    Main.rand.NextFloat(-4f, 4f),
                    Main.rand.NextFloat(-4f, 4f),
                    100, default(Color), 1.5f
                );
                dust. noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Microsoft.Xna.Framework.Graphics. Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type]. Value;

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 drawPos = Projectile.oldPos[i] - Main.screenPosition + new Vector2(Projectile.width, Projectile.height) / 2f;
                Color color = Color.Gold * ((Projectile.oldPos. Length - i) / (float)Projectile.oldPos.Length) * 0.5f;
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, texture.Size() / 2f, Projectile.scale, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
            }

            return true;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White; // Always bright
        }
    }
}