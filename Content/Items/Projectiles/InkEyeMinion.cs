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
    public class InkEyeMinion : ModProjectile
    {
        private const float IdleOffsetY = -64f;
        private const float DetectTiles = 50f;
        private const float DetectRangePx = DetectTiles * 16f;
        private const int FireCooldownTicks = 60;
        private const float ChainRangeTiles = 25f;
        private const float ChainRangePx = ChainRangeTiles * 16f;

        // Beam visuals
        private const float FirstBeamThicknessPx = 2f;
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
            Projectile.minionSlots = 1f; // costs 1 slot, item enforces only one exists
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 18000;
            Projectile.netImportant = true;

            // We draw it ourselves at Center; don't rely on spriteDirection flipping offsets
            Projectile.spriteDirection = 1;
        }

        public override bool MinionContactDamage() => false;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.active || player.dead || !player.HasBuff(ModContent.BuffType<InkEyeBuff>()))
            {
                Projectile.Kill();
                return;
            }
            Projectile.timeLeft = 2;

            // Rigidly lock above the player's mounted center (best reference for head position)
            Projectile.Center = player.MountedCenter + new Vector2(0f, IdleOffsetY);
            Projectile.velocity = Vector2.Zero;
            Projectile.rotation = 0f;

            // "Black light" vibe (Terraria light is additive; this is a faint desaturated violet/gray)
            Lighting.AddLight(Projectile.Center, 0.05f, 0.02f, 0.06f);

            // Black particles around the eye
            if (Main.rand.NextBool(3))
            {
                var d = Dust.NewDustDirect(Projectile.Center - new Vector2(4, 4), 8, 8, DustID.Smoke, 0f, 0f, 160, Color.Black, Main.rand.NextFloat(1.2f, 1.7f));
                d.noGravity = true;
                d.velocity = Main.rand.NextVector2Circular(0.8f, 0.8f);
            }

            // Fire once per second if enemy within 30 tiles of the PLAYER
            int fireTimer = (int)Projectile.localAI[0];
            if (fireTimer > 0) fireTimer--;
            Projectile.localAI[0] = fireTimer;

            if (fireTimer <= 0)
            {
                NPC target = FindClosestEnemyNearPlayer(player, DetectRangePx);
                if (target != null)
                {
                    // Visual: draw a thick beam from the eye to the first target
                    DrawThickZapBetween(Projectile.Center, target.Center, FirstBeamThicknessPx);

                    FireChain(player, target);
                    Projectile.localAI[0] = FireCooldownTicks;
                }
            }
        }

        // Manual draw at Projectile.Center so the sprite is exactly where the dust is.
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            int frames = Main.projFrames[Projectile.type];
            int frameHeight = tex.Height / Math.Max(1, frames);
            Rectangle src = new Rectangle(0, Projectile.frame * frameHeight, tex.Width, frameHeight);

            // Draw at Center with adjustable visual offset
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

            return false; // we've drawn it
        }

        private void FireChain(Player player, NPC initial)
        {
            Vector2 start = Projectile.Center;
            Vector2 toTarget = initial.Center - start;
            if (toTarget.LengthSquared() < 4f)
                toTarget = new Vector2(0f, -1f);

            toTarget.Normalize();
            float speed = 75f;

            int projType = ModContent.ProjectileType<InkZapProjectile>();
            int baseDamage = Projectile.damage; // uses item's damage and player summon modifiers

            int idx = Projectile.NewProjectile(Projectile.GetSource_FromThis(), start, toTarget * speed, projType, baseDamage, 0f, Projectile.owner,
                ai0: 0f, // chain step = 0 (100%)
                ai1: -1f // previous target id = -1 (none)
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

        // Thick "black pixel" beam with perpendicular spread, dust-based
        private void DrawThickZapBetween(Vector2 a, Vector2 b, float thicknessPx)
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

                int perPoint = 3;
                for (int j = 0; j < perPoint; j++)
                {
                    float off = Main.rand.NextFloat(-thicknessPx, thicknessPx);
                    Vector2 pos = p + perp * off;

                    var d = Dust.NewDustDirect(pos - new Vector2(2, 2), 4, 4, DustID.Smoke, 0f, 0f, 200, Color.Black, Main.rand.NextFloat(1.5f, 2.2f));
                    d.noGravity = true;
                    d.velocity *= 0f;
                }
            }
        }
    }
}