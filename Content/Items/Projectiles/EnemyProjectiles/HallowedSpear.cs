using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles. EnemyProjectiles
{
    public class HallowedSpear :  MoUProjectile
    {
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

        private Vector2 targetPosition; // Store where player WAS

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SafeSetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 8;
            Projectile.aiStyle = -1;
            Projectile. hostile = true;
            Projectile.friendly = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.alpha = 0;
            Projectile.light = 0.8f;
            Projectile. ignoreWater = true;
            Projectile.tileCollide = true;
        }

        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
{
    modifiers.FinalDamage.Base = Projectile.damage;
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

            Lighting.AddLight(Projectile.Center, 0.9f, 0.8f, 0.3f);

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

            if (StateTimer >= 20) // After ~0.33 seconds, stop
            {
                // STORE player position when stopping (not live tracking)
                Player target = Main.player[Player.FindClosest(Projectile.position, Projectile.width, Projectile.height)];
                targetPosition = target.Center; // ← SNAPSHOT, not updated

                Projectile.velocity = Vector2.Zero;
                Projectile. tileCollide = false;

                State = AIState.Spinning;
                StateTimer = 0;
            }
        }

        private void AI_Spinning()
        {
            Projectile.rotation += 0.3f;

            if (StateTimer >= 60) // Spin for 1 second
            {
                // Launch toward STORED position (not current player position)
                Vector2 direction = (targetPosition - Projectile.Center).SafeNormalize(Vector2.UnitX);
                Projectile.velocity = direction * 16f;

                State = AIState. Homing;
                StateTimer = 0;
            }
        }

        private void AI_Homing()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();

            // NO HOMING - just fly straight toward stored position
            // (Or very minimal correction)
            
            // Optional:  VERY gentle course correction (almost none)
            Vector2 direction = (targetPosition - Projectile.Center).SafeNormalize(Vector2.Zero);
            Projectile.velocity = Vector2.Lerp(Projectile.velocity, direction * 16f, 0.005f); // ← 0.005 = almost no homing
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            int rebukeDuration = 60;
            int subjugatedDuration = 30;

            if (Main.expertMode)
            {
                rebukeDuration = 120;
                subjugatedDuration = 60;
            }

            if (Main.masterMode)
            {
                rebukeDuration = 180;
                subjugatedDuration = 90;
            }

            target.AddBuff(ModContent. BuffType<RebukingLight>(), rebukeDuration);
            target.AddBuff(ModContent.BuffType<Subjugated>(), subjugatedDuration);
        }

        public override void Kill(int timeLeft)
        {
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
                dust.noGravity = true;
            }
        }

        public override bool SafePreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Microsoft.Xna.Framework.Graphics. Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type]. Value;

            for (int i = 0; i < Projectile.oldPos. Length; i++)
            {
                Vector2 drawPos = Projectile.oldPos[i] - Main.screenPosition + new Vector2(Projectile.width, Projectile.height) / 2f;
                Color color = Color.Gold * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length) * 0.5f;
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, texture.Size() / 2f, Projectile.scale, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
            }

            return true;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}