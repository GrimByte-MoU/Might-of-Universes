// Content/NPCs/Bosses/Aegon/AegonMirage.cs

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ReLogic.Content;
using MightofUniverses.Content.Items.Projectiles.EnemyProjectiles;

namespace MightofUniverses.Content.NPCs.Bosses.Aegon
{
    public class AegonMirage : ModNPC
    {
        // AI references
        private ref float AttackTimer => ref NPC.ai[0];
        private ref float AttackState => ref NPC.ai[1];
        private ref float SubTimer => ref NPC.ai[2];

        // Track which Aegon we belong to
        private int aegonIndex = -1;

        // Position data
        private Vector2 targetPosition; // Where mirage should stay

        public override void SetStaticDefaults()
        {
            NPCID.Sets.MPAllowedEnemies[Type] = true;
            
            // DON'T attract homing projectiles
            NPCID. Sets.ProjectileNPC[Type] = false;
        }

        public override void SetDefaults()
        {
            NPC.width = 120;
            NPC.height = 120;
            NPC. damage = 0; // Doesn't deal contact damage (mirrors can't hit you)
            NPC.defense = 9999; // Effectively invincible
            NPC. lifeMax = 1;
            NPC.life = 1;
            NPC.HitSound = null; // No hit sound
            NPC.DeathSound = null; // No death sound
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.value = 0; // No value
            NPC.boss = false; // Not a boss itself
            NPC.aiStyle = -1;
            NPC.netAlways = true;
            NPC. dontTakeDamage = true; // INVINCIBLE
            NPC. dontCountMe = true; // DON'T attract homing projectiles or targeting
            NPC.immortal = true; // Can't be killed
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            // Use Aegon's sprite but make it slightly transparent
            Texture2D texture = ModContent.Request<Texture2D>("MightofUniverses/Content/NPCs/Bosses/Aegon/Aegon").Value;

            Vector2 drawPos = NPC.Center - screenPos;
            Vector2 origin = texture.Size() / 2f;

            // Draw with transparency (ghostly effect)
            Color mirageColor = drawColor * 0.6f; // 60% opacity

            spriteBatch.Draw(
                texture,
                drawPos,
                null,
                mirageColor,
                NPC.rotation,
                origin,
                NPC.scale,
                SpriteEffects.None,
                0f
            );

            // Optional: Draw a subtle glow outline
            for (int i = 0; i < 4; i++)
            {
                Vector2 offset = new Vector2(
                    (float)Math.Cos(MathHelper.TwoPi * i / 4f),
                    (float)Math.Sin(MathHelper.TwoPi * i / 4f)
                ) * 2f;

                spriteBatch.Draw(
                    texture,
                    drawPos + offset,
                    null,
                    Color. Cyan * 0.3f,
                    NPC. rotation,
                    origin,
                    NPC.scale,
                    SpriteEffects. None,
                    0f
                );
            }

            return false; // Don't draw default sprite
        }

        public override void AI()
        {
            // Find parent Aegon
            if (aegonIndex == -1 || ! Main.npc[aegonIndex].active || Main.npc[aegonIndex].type != ModContent.NPCType<Aegon>())
            {
                FindAegon();
                
                // If no Aegon found, disappear
                if (aegonIndex == -1)
                {
                    NPC.active = false;
                    return;
                }
            }

            NPC aegon = Main.npc[aegonIndex];
            Player target = Main.player[aegon. target];

            // Drift particles (ghostly effect)
            if (Main.rand.NextBool(5))
            {
                int dust = Dust.NewDust(NPC.Center, NPC. width, NPC.height, DustID.Electric, 0f, 0f, 100, Color.Cyan, 0.8f);
                Main. dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.3f;
            }

            AttackTimer++;
        }

        // ==================== POSITION MANAGEMENT ====================

        /// <summary>
        /// Set the mirage's position (called by Aegon)
        /// </summary>
        public void SetPosition(Vector2 position)
        {
            NPC.Center = position;
            targetPosition = position;
            NPC.velocity = Vector2.Zero;
        }

        /// <summary>
        /// Set target position for smooth movement
        /// </summary>
        public void SetTargetPosition(Vector2 position)
        {
            targetPosition = position;
        }

        /// <summary>
        /// Move towards target position smoothly
        /// </summary>
        public void MoveToTarget(float speed = 10f)
        {
            Vector2 direction = targetPosition - NPC.Center;
            float distance = direction.Length();

            if (distance > speed)
            {
                direction. Normalize();
                NPC.velocity = direction * speed;
            }
            else
            {
                NPC.Center = targetPosition;
                NPC.velocity = Vector2.Zero;
            }
        }

        // ==================== PHASE 4 MIRRORED ATTACKS ====================

        /// <summary>
        /// Mirror Aegon's Attack A (Phase 4) - Circle of Aegis Fragments
        /// Fires 5 fragments in a circle that split into spreads on wall hit
        /// </summary>
        public void MirrorAttackA_AegisFragmentCircle(Vector2 playerPosition, bool flipped)
        {
            int fragmentCount = 5;

            for (int i = 0; i < fragmentCount; i++)
            {
                float angle = (i / (float)fragmentCount) * MathHelper.TwoPi;

                if (flipped)
                {
                    angle = -angle; // Mirror rotation
                }

                Vector2 velocity = new Vector2(
                    (float)Math.Cos(angle),
                    (float)Math.Sin(angle)
                ) * 6f;

                if (Main. netMode != NetmodeID. MultiplayerClient)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                        ModContent.ProjectileType<AegisFragment>(),
                        GetDamage(125), 0f);
                }
            }
        }

        /// <summary>
        /// Mirror Aegon's Attack B (Phase 4) - Aegis Shard spread
        /// Fires 9 shards with varying speeds
        /// </summary>
        public void MirrorAttackB_AegisShardSpread(Vector2 playerPosition, bool flipped)
        {
            int spreadCount = 9;
            float baseAngle = (playerPosition - NPC.Center).ToRotation();

            for (int i = 0; i < spreadCount; i++)
            {
                float offsetAngle = MathHelper. Lerp(-0.5f, 0.5f, i / (float)(spreadCount - 1));
                float angle = baseAngle + offsetAngle;

                if (flipped)
                {
                    angle = MathHelper.Pi - angle;
                }

                // Varying speeds
                float speed = 4f + Main.rand.NextFloat(-1f, 2f);

                Vector2 velocity = new Vector2(
                    (float)Math.Cos(angle),
                    (float)Math.Sin(angle)
                ) * speed;

                if (Main. netMode != NetmodeID. MultiplayerClient)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                        ModContent.ProjectileType<AegisShard>(),
                        GetDamage(120), 0f);
                }
            }
        }

        /// <summary>
        /// Fire Aegis Fragment at player (during Phase 4 Attack C)
        /// EXCEPTION: Doesn't copy raining shards, just sends fragments
        /// </summary>
        public void FireAegisFragment(Vector2 playerPosition, bool flipped)
        {
            float angle = (playerPosition - NPC.Center).ToRotation();

            if (flipped)
            {
                angle = MathHelper.Pi - angle;
            }

            Vector2 velocity = new Vector2(
                (float)Math.Cos(angle),
                (float)Math.Sin(angle)
            ) * 7f;

            if (Main. netMode != NetmodeID. MultiplayerClient)
            {
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                    ModContent.ProjectileType<AegisFragment>(),
                    GetDamage(125), 0f);
            }
        }

        /// <summary>
        /// Mirror Aegon's Attack D (Phase 4) - Rapid fire shards
        /// Fires horizontal shards (flipped direction)
        /// </summary>
        public void MirrorAttackD_RapidShards(float verticalProgress, bool shootingUp, bool flipped)
        {
            // Fire shards horizontally at current vertical position
            float angle = flipped ? MathHelper.Pi :  0f; // Shoot opposite direction

            Vector2 velocity = new Vector2(
                (float)Math.Cos(angle),
                (float)Math.Sin(angle)
            ) * 8f;

            if (Main. netMode != NetmodeID. MultiplayerClient)
            {
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                    ModContent.ProjectileType<AegisShard>(),
                    GetDamage(120), 0f);
            }
        }

        // ==================== PHASE 5 PATIENCE ATTACKS ====================

        /// <summary>
        /// Fire a spread of Jungle Needles centered on arena (mirrored from Aegon)
        /// Phase 5 Patience - starts after 5 seconds, fires 3x/second
        /// </summary>
        public void FireJungleNeedleSpread(Vector2 arenaCenter, bool flipped)
        {
            int spreadCount = 11;
            float baseAngle = (arenaCenter - NPC. Center).ToRotation();

            for (int i = 0; i < spreadCount; i++)
            {
                float offsetAngle = MathHelper.Lerp(-MathHelper.PiOver4, MathHelper.PiOver4, i / (float)(spreadCount - 1));
                float angle = baseAngle + offsetAngle;

                // Flip direction if this is a mirrored mirage
                if (flipped)
                {
                    angle = MathHelper.Pi - angle;
                }

                Vector2 velocity = new Vector2(
                    (float)Math.Cos(angle),
                    (float)Math.Sin(angle)
                ) * 3f; // Very slow

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                        ModContent.ProjectileType<JungleNeedle>(),
                        GetDamage(95), 0f);
                }
            }
        }

        /// <summary>
        /// Fire Crimruption Bolts towards arena center
        /// Phase 5 Patience - starts after 10 seconds, fires 3x/second
        /// </summary>
        public void FireCrimruptionBolts(Vector2 arenaCenter, bool flipped)
        {
            int spreadCount = 3;
            float baseAngle = (arenaCenter - NPC. Center).ToRotation();

            for (int i = 0; i < spreadCount; i++)
            {
                float offsetAngle = MathHelper.Lerp(-0.2f, 0.2f, i / (float)(spreadCount - 1));
                float angle = baseAngle + offsetAngle;

                if (flipped)
                {
                    angle = MathHelper. Pi - angle;
                }

                Vector2 velocity = new Vector2(
                    (float)Math.Cos(angle),
                    (float)Math.Sin(angle)
                ) * 5f;

                if (Main. netMode != NetmodeID. MultiplayerClient)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                        ModContent.ProjectileType<CrimruptionBolt>(),
                        GetDamage(110), 0f);
                }
            }
        }

        /// <summary>
        /// Fire Ocean Sphere towards arena center
        /// Phase 5 Patience - starts after 15 seconds, fires 3x/second
        /// </summary>
        public void FireOceanSphere(Vector2 arenaCenter, bool flipped)
        {
            float angle = (arenaCenter - NPC.Center).ToRotation();

            if (flipped)
            {
                angle = MathHelper.Pi - angle;
            }

            Vector2 velocity = new Vector2(
                (float)Math.Cos(angle),
                (float)Math.Sin(angle)
            ) * 6f;

            if (Main. netMode != NetmodeID. MultiplayerClient)
            {
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                    ModContent.ProjectileType<OceanSphere>(),
                    GetDamage(120), 0f);
            }
        }

        /// <summary>
        /// Fire Hallowed Spears towards player
        /// Phase 5 Patience - starts after 20 seconds, fires 1x/second
        /// </summary>
        public void FireHallowedSpears(Vector2 playerPosition, bool flipped)
        {
            int spreadCount = 3;
            float baseAngle = (playerPosition - NPC.Center).ToRotation();

            for (int i = 0; i < spreadCount; i++)
            {
                float offsetAngle = MathHelper.Lerp(-0.35f, 0.35f, i / (float)(spreadCount - 1)); // 20 degrees ≈ 0.35 radians
                float angle = baseAngle + offsetAngle;

                if (flipped)
                {
                    angle = MathHelper.Pi - angle;
                }

                Vector2 velocity = new Vector2(
                    (float)Math.Cos(angle),
                    (float)Math.Sin(angle)
                ) * 4f;

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                        ModContent. ProjectileType<HallowedSpear>(),
                        GetDamage(120), 0f);
                }
            }
        }

        // ==================== HELPER METHODS ====================

        private void FindAegon()
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active && Main. npc[i].type == ModContent.NPCType<Aegon>())
                {
                    aegonIndex = i;
                    return;
                }
            }
        }

        private int GetDamage(int baseDamage)
        {
            if (Main.masterMode)
                return baseDamage * 3;
            if (Main.expertMode)
                return baseDamage * 2;
            return baseDamage;
        }

        public override bool?  CanBeHitByItem(Player player, Item item)
        {
            return false; // Can't be hit by weapons
        }

        public override bool?  CanBeHitByProjectile(Projectile projectile)
        {
            return false; // Can't be hit by projectiles
        }

        public override bool CheckActive()
        {
            return false; // Never despawn naturally
        }

        public override void OnKill()
        {
            // Visual effect when disappearing
            for (int i = 0; i < 20; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(4f, 4f);
                int dust = Dust.NewDust(NPC.Center, NPC.width, NPC.height, DustID.Electric, velocity. X, velocity.Y, 100, Color.Cyan, 1.2f);
                Main.dust[dust].noGravity = true;
            }
        }
    }
}