using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using System;
using Terraria.GameContent;
using Microsoft.Xna.Framework.Graphics;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class EyeOfHorusMinion : ModProjectile
    {
        private const float IdleOffsetY = -64f;
        private const float DetectTiles = 50f; // around the PLAYER
        private const float DetectRangePx = DetectTiles * 16f; // 800 px
        private const int FireCooldownTicks = 30; // twice per second
        private const float ChainRangeTiles = 30f; // faster/longer chaining hops
        private const float ChainRangePx = ChainRangeTiles * 16f;
        private const float FirstBeamThicknessPx = 1.5f;

        private const float SpriteVisualOffsetX = 0f;
        private const float SpriteVisualOffsetY = 0f;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
            Main.projPet[Projectile.type] = true;

            ProjectileID.Sets.MinionSacrificable[Type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.minionSlots = 1f;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 18000;
            Projectile.netImportant = true;
            Projectile.spriteDirection = 1;
        }

        public override bool MinionContactDamage() => false;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.active || player.dead || !player.HasBuff(ModContent.BuffType<EyeOfHorusBuff>()))
            {
                Projectile.Kill();
                return;
            }
            Projectile.timeLeft = 2;

            // Rigid anchor above player's mounted center
            Projectile.Center = player.MountedCenter + new Vector2(0f, IdleOffsetY);
            Projectile.velocity = Vector2.Zero;
            Projectile.rotation = 0f;

            Lighting.AddLight(Projectile.Center, 0.15f, 0.12f, 0.02f);

            if (Main.rand.NextBool(3))
            {
                int dustType = Main.rand.NextBool() ? DustID.GoldFlame : DustID.Sand;
                var d = Dust.NewDustDirect(Projectile.Center - new Vector2(4, 4), 8, 8, dustType, 0f, 0f, 120, default, Main.rand.NextFloat(1.0f, 1.5f));
                d.noGravity = true;
                d.velocity = Main.rand.NextVector2Circular(0.9f, 0.9f) * 0.5f;
            }


            int fireTimer = (int)Projectile.localAI[0];
            if (fireTimer > 0) fireTimer--;
            Projectile.localAI[0] = fireTimer;

            if (fireTimer <= 0)
            {
                NPC target = FindClosestEnemyNearPlayer(player, DetectRangePx);
                if (target != null)
                {
                    // Visual: slim beam from eye to first target
                    DrawSlimZapBetween(Projectile.Center, target.Center, FirstBeamThicknessPx);
                    FireChain(player, target);
                    Projectile.localAI[0] = FireCooldownTicks;
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            int frames = Main.projFrames[Projectile.type];
            int frameHeight = tex.Height / Math.Max(1, frames);
            Rectangle src = new Rectangle(0, Projectile.frame * frameHeight, tex.Width, frameHeight);

            Vector2 drawPos = Projectile.Center - Main.screenPosition + new Vector2(SpriteVisualOffsetX, SpriteVisualOffsetY);
            Vector2 origin = new Vector2(tex.Width / 2f, frameHeight / 2f);

            Main.EntitySpriteDraw(
                tex,
                drawPos,
                src,
                lightColor,
                0f,
                origin,
                1f,
                SpriteEffects.None,
                0
            );

            return false;
        }

        private void FireChain(Player player, NPC initial)
        {
            Vector2 start = Projectile.Center;
            Vector2 toTarget = initial.Center - start;
            if (toTarget.LengthSquared() < 4f) toTarget = new Vector2(0f, -1f);
            toTarget.Normalize();

            float speed = 90f; // faster chaining
            int projType = ModContent.ProjectileType<HorusZapProjectile>();
            int baseDamage = Projectile.damage;

            int idx = Projectile.NewProjectile(Projectile.GetSource_FromThis(), start, toTarget * speed, projType, baseDamage, 0f, Projectile.owner,
                ai0: 0f, // step 0
                ai1: -1f // prev target id none
            );

            if (idx >= 0 && idx < Main.maxProjectiles)
            {
                Main.projectile[idx].originalDamage = baseDamage;
                Main.projectile[idx].localAI[0] = ChainRangePx; // pass chain range
            }
        }

        private NPC FindClosestEnemyNearPlayer(Player player, float rangePx)
        {
            NPC chosen = null;
            float best = rangePx * rangePx;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.CanBeChasedBy(this))
                {
                    float d2 = Vector2.DistanceSquared(player.Center, npc.Center);
                    if (d2 <= best)
                    {
                        best = d2;
                        chosen = npc;
                    }
                }
            }
            return chosen;
        }

        private void DrawSlimZapBetween(Vector2 a, Vector2 b, float thicknessPx)
        {
            Vector2 dir = b - a;
            float len = dir.Length();
            if (len < 1f) return;
            dir /= len;
            Vector2 perp = new Vector2(-dir.Y, dir.X);

            int points = Math.Max(8, (int)(len / 8f));
            for (int i = 0; i <= points; i++)
            {
                float t = i / (float)points;
                Vector2 p = Vector2.Lerp(a, b, t);

                // Very slim: 1-2 particles per step, small spread
                float off = Main.rand.NextFloat(-thicknessPx, thicknessPx);
                Vector2 pos = p + perp * off;

                int dustType = Main.rand.NextBool(3) ? DustID.Sand : DustID.GoldFlame;
                var d = Dust.NewDustDirect(pos - new Vector2(1, 1), 2, 2, dustType, 0f, 0f, 100, default, Main.rand.NextFloat(1.0f, 1.4f));
                d.noGravity = true;
                d.velocity *= 0f;
            }
        }
    }
}