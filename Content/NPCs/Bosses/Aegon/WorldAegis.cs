// Content/NPCs/Bosses/Aegon/WorldAegis. cs

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using ReLogic.Content;
using MightofUniverses.Content.Items.Projectiles. EnemyProjectiles;

namespace MightofUniverses. Content.NPCs.Bosses.Aegon
{
    public class WorldAegis : ModNPC
    {
        // AI States
        private enum State
        {
            Phase1_Full = 0,              // 100% - 50% HP (full sprite, attacking)
            Phase2_FlewAway = 1,          // Off screen (damaged sprite, invulnerable)
            Phase3_Damaged = 2,           // Returns damaged, attacks with Aegon
            Phase4_ScatterFragments = 3,  // Flies off scattering fragments (ruined sprite)
            Phase5_HeavilyDamaged = 4     // Final phase - Patience (ruined sprite)
        }

        private State CurrentState
        {
            get => (State)NPC.ai[0];
            set => NPC.ai[0] = (float)value;
        }

        private ref float AttackTimer => ref NPC.ai[1];
        private ref float RotationAngle => ref NPC.ai[2];
        private ref float SpiralDirection => ref NPC.ai[3]; // 1 = clockwise, -1 = counterclockwise

        // Track which Aegon we belong to
        private int aegonIndex = -1;

        // Arena center for bobbing
        private Vector2 arenaCenter = Vector2.Zero;
        private bool arenaCenterSet = false;

        // Textures for different states
        private static Asset<Texture2D> textureNormal;
        private static Asset<Texture2D> textureDamaged;
        private static Asset<Texture2D> textureRuined;

        public override void SetStaticDefaults()
        {
            NPCID.Sets.MPAllowedEnemies[Type] = true;

            // Load all 3 textures
            if (Main.netMode != NetmodeID.Server)
            {
                textureNormal = ModContent.Request<Texture2D>("MightofUniverses/Content/NPCs/Bosses/Aegon/WorldAegis");
                textureDamaged = ModContent.Request<Texture2D>("MightofUniverses/Content/NPCs/Bosses/Aegon/WorldAegis_Damaged");
                textureRuined = ModContent.Request<Texture2D>("MightofUniverses/Content/NPCs/Bosses/Aegon/WorldAegis_Ruined");
            }
        }

        public override void SetDefaults()
        {
            NPC.width = 80;
            NPC.height = 80;
            NPC. damage = 0; // Doesn't deal contact damage
            NPC.defense = 100;
            NPC.lifeMax = 100000;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = null; // Custom handling
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.value = 0; // No value, not the main boss
            NPC.boss = false; // Not a boss itself
            NPC.aiStyle = -1;
            NPC.netAlways = true;
            NPC. dontTakeDamage = false;
            NPC.scale = 2.0f;

            if (Main.expertMode)
            {
                NPC.lifeMax = 150000;
                NPC.defense = 125;
            }

            if (Main.masterMode)
            {
                NPC.lifeMax = 200000;
                NPC.defense = 150;
            }
        }

        public override void OnSpawn(IEntitySource source)
        {
            // Store initial spawn position as arena center
            arenaCenter = NPC.Center;
            arenaCenterSet = true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            // Don't draw if too far off-screen during fly away phases
            if ((CurrentState == State.Phase2_FlewAway || CurrentState == State.Phase4_ScatterFragments))
            {
                if (Vector2.Distance(NPC.Center, Main.LocalPlayer.Center) > 2000f)
                    return false;
            }

            // Choose texture based on state
            Texture2D texture;
            
            if (CurrentState == State. Phase1_Full)
            {
                // Full shield (Phase 1 only)
                texture = textureNormal. Value;
            }
            else if (CurrentState == State.Phase2_FlewAway || CurrentState == State.Phase3_Damaged)
            {
                // Damaged shield (Phase 2 & 3)
                texture = textureDamaged. Value;
            }
            else // Phase4_ScatterFragments or Phase5_HeavilyDamaged
            {
                // Ruined shield (Phase 4 & 5)
                texture = textureRuined.Value;
            }

            Vector2 drawPos = NPC. Center - screenPos;
            Vector2 origin = texture.Size() / 2f;

            spriteBatch.Draw(
                texture,
                drawPos,
                null,
                drawColor,
                NPC.rotation,
                origin,
                NPC.scale,
                SpriteEffects.None,
                0f
            );

            return false; // Don't draw default sprite
        }

        public override void AI()
        {
            // Find parent Aegon
            if (aegonIndex == -1 || ! Main.npc[aegonIndex].active || Main.npc[aegonIndex].type != ModContent.NPCType<Aegon>())
            {
                FindAegon();
                
                if (aegonIndex == -1)
                {
                    NPC.active = false;
                    return;
                }
            }

            NPC aegon = Main.npc[aegonIndex];

            // GENTLE BOBBING UP AND DOWN
            float bobSpeed = 0.03f;
            float bobHeight = 8f; // Bobs 8 pixels up and down
            float bobOffset = (float)Math.Sin(AttackTimer * bobSpeed) * bobHeight;

            // Apply bobbing only if not flying away
            if (CurrentState != State.Phase2_FlewAway && CurrentState != State.Phase4_ScatterFragments)
            {
                if (arenaCenterSet)
                {
                    // Float at arena center with gentle bobbing
                    NPC. Center = new Vector2(arenaCenter.X, arenaCenter.Y + bobOffset);
                    NPC.velocity = Vector2.Zero;
                }
            }

            // Attack based on state
            switch (CurrentState)
            {
                case State.Phase1_Full:
                    Phase1Attacks();
                    break;

                case State.Phase2_FlewAway:
                    Phase2Inactive();
                    break;

                case State.Phase3_Damaged:
                    Phase3Attacks();
                    break;

                case State.Phase4_ScatterFragments:
                    Phase4FlyAway();
                    break;

                case State.Phase5_HeavilyDamaged:
                    Phase5Attacks();
                    break;
            }

            AttackTimer++;
        }

        // ==================== PHASE 1: FULL SHIELD (100% - 50% HP) ====================
        private void Phase1Attacks()
        {
            Player target = Main. player[NPC.target];

            // Radial burst of 8 (or 12 at 80% HP) World Aegis Bolts, 3 times per second (20 frames)
            if (AttackTimer % 60 == 0)
            {
                int projectileCount = NPC.life / (float)NPC.lifeMax > 0.8f ? 8 : 12;

                for (int i = 0; i < projectileCount; i++)
                {
                    float angle = (i / (float)projectileCount) * MathHelper.TwoPi + RotationAngle;
                    Vector2 velocity = new Vector2(
                        (float)Math.Cos(angle),
                        (float)Math.Sin(angle)
                    ) * 4f;

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                            ModContent.ProjectileType<WorldAegisBolt>(),
                            GetDamage(95), 0f);
                    }
                }
            }

            // World Aegis Leaf at 90% HP, twice per second (30 frames)
            if (NPC.life / (float)NPC.lifeMax <= 0.9f && AttackTimer % 30 == 0)
            {
                Vector2 velocity = (target. Center - NPC.Center).SafeNormalize(Vector2.UnitX) * 6f;

                if (Main. netMode != NetmodeID. MultiplayerClient)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                        ModContent.ProjectileType<WorldAegisLeaf>(),
                        GetDamage(100), 0f);
                }
            }
        }

        // ==================== TRANSITION: PHASE 1 → PHASE 2 ====================
        // Called by Aegon when shield reaches 50% HP
        public void TransitionToPhase2AndFlyAway()
        {
            CurrentState = State.Phase2_FlewAway;
            NPC. dontTakeDamage = true;  // Immune
            NPC. dontCountMe = true;     // Don't attract homing
            AttackTimer = 0;
            
            // Move off-screen upward
            NPC.velocity = new Vector2(0, -10f);
            
            // Visual effect - shield cracks and flies away
            for (int i = 0; i < 30; i++)
            {
                Vector2 velocity = Main.rand. NextVector2Circular(4f, 4f);
                int dust = Dust.NewDust(NPC.Center, NPC.width, NPC.height, DustID.Stone, velocity.X, velocity.Y, 100, Color.Orange, 1.5f);
                Main. dust[dust].noGravity = true;
            }
        }

        // ==================== PHASE 2: FLEW AWAY (OFF-SCREEN) ====================
        private void Phase2Inactive()
        {
            // Keep moving away for 2 seconds, then stop far above arena
            if (AttackTimer < 120) // 2 seconds
            {
                NPC.velocity. Y = -5f;
            }
            else
            {
                NPC.velocity = Vector2.Zero;
                // Stay off-screen, don't attack, invulnerable
            }
        }

        // ==================== TRANSITION: PHASE 2 → PHASE 3 ====================
        // Called by Aegon when Phase 3 starts (Aegon reaches 60% HP)
        public void ReturnToArenaForPhase3(Vector2 center)
        {
            CurrentState = State.Phase3_Damaged;
            NPC.dontTakeDamage = false; // Vulnerable again
            NPC.dontCountMe = false;    // Can be targeted
            NPC.Center = center;        // Snap back to center
            NPC.velocity = Vector2.Zero;
            AttackTimer = 0;
            
            // Store arena center for bobbing
            arenaCenter = center;
            arenaCenterSet = true;
            
            // Visual effect - dramatic return (still damaged sprite)
            for (int i = 0; i < 40; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(5f, 5f);
                int dust = Dust.NewDust(NPC.Center, NPC.width, NPC.height, DustID.Stone, velocity.X, velocity.Y, 100, Color.Orange, 1.8f);
                Main.dust[dust].noGravity = true;
            }
        }

        // ==================== PHASE 3: DAMAGED SHIELD RETURNS ====================
        private void Phase3Attacks()
        {
            Player target = Main.player[NPC.target];

            // Radial burst of 12 World Aegis Bolts, 3 times per second (20 frames)
            if (AttackTimer % 20 == 0)
            {
                for (int i = 0; i < 12; i++)
                {
                    float angle = (i / 12f) * MathHelper.TwoPi + RotationAngle;
                    Vector2 velocity = new Vector2(
                        (float)Math.Cos(angle),
                        (float)Math.Sin(angle)
                    ) * 4f;

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(NPC. GetSource_FromAI(), NPC.Center, velocity,
                            ModContent.ProjectileType<WorldAegisBolt>(),
                            GetDamage(95), 0f);
                    }
                }
            }

            // Additional attacks triggered by Aegon's signals during specific attacks
        }

        // Called by Aegon during Attack A in Phase 3
        public void FireWorldAegisFireball()
        {
            Player target = Main.player[NPC.target];
            Vector2 velocity = (target.Center - NPC.Center).SafeNormalize(Vector2.UnitX) * 7f;

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                    ModContent.ProjectileType<WorldAegisFireball>(),
                    GetDamage(95), 0f);
            }
        }

        // Called by Aegon during Attack C in Phase 3
        public void StartWaterSpiral(bool clockwise)
        {
            SpiralDirection = clockwise ? 1f : -1f;
        }

        public void FireWaterSpiral()
        {
            // Two-pointed spiral, 10 times per second (6 frames)
            if (AttackTimer % 6 == 0)
            {
                for (int i = 0; i < 2; i++)
                {
                    float baseAngle = RotationAngle * SpiralDirection;
                    float angle = baseAngle + (i * MathHelper.Pi);
                    Vector2 velocity = new Vector2(
                        (float)Math.Cos(angle),
                        (float)Math.Sin(angle)
                    ) * 5f;

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(NPC. GetSource_FromAI(), NPC.Center, velocity,
                            ModContent.ProjectileType<WorldAegisWater>(),
                            GetDamage(95), 0f);
                    }
                }
            }
        }

        // ==================== TRANSITION: PHASE 3 → PHASE 4 ====================
        // Called when World Aegis reaches 1 HP in Phase 3
        public void TransitionToPhase4AndScatterFragments()
        {
            CurrentState = State.Phase4_ScatterFragments;
            NPC.life = 1;               // Lock at 1 HP
            NPC.dontTakeDamage = true;  // Immune
            NPC. dontCountMe = true;     // Don't attract homing
            AttackTimer = 0;
            
            // Move off-screen again
            NPC.velocity = new Vector2(0, -8f);
            
            // Scatter World Aegis Fragments in random directions while leaving
            for (int i = 0; i < 20; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(8f, 8f);
                
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                        ModContent.ProjectileType<AegisFragment>(),
                        GetDamage(125), 0f);
                }
            }
            
            // Visual effect - shield shatters further and flies away
            for (int i = 0; i < 50; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(6f, 6f);
                int dust = Dust.NewDust(NPC.Center, NPC.width, NPC.height, DustID.Stone, velocity.X, velocity.Y, 100, Color.Red, 2f);
                Main.dust[dust].noGravity = true;
            }
        }

        // ==================== PHASE 4: SCATTER FRAGMENTS (OFF-SCREEN) ====================
        private void Phase4FlyAway()
        {
            // Keep moving away for 2 seconds
            if (AttackTimer < 120) // 2 seconds
            {
                NPC.velocity.Y = -6f;
            }
            else
            {
                NPC. velocity = Vector2.Zero;
                // Stay off-screen (now with ruined sprite)
            }
        }

        // ==================== TRANSITION:  PHASE 4 → PHASE 5 ====================
        // Called by Aegon when starting Patience (Phase 5)
        public void ReturnForPhase5(Vector2 center)
        {
            CurrentState = State.Phase5_HeavilyDamaged;
            NPC. dontCountMe = false;    // Can be targeted (but still immune until patience ends)
            NPC.Center = center;        // Snap to center
            NPC.velocity = Vector2.Zero;
            NPC.life = 1;
            NPC.dontTakeDamage = true;  // Still immune until patience mechanic completes
            AttackTimer = 0;
            
            // Store arena center for bobbing
            arenaCenter = center;
            arenaCenterSet = true;
            
            // Visual effect - most dramatic return (ruined sprite)
            for (int i = 0; i < 60; i++)
            {
                Vector2 velocity = Main. rand.NextVector2Circular(7f, 7f);
                int dust = Dust.NewDust(NPC.Center, NPC.width, NPC.height, DustID.Stone, velocity.X, velocity.Y, 100, Color.DarkRed, 2.5f);
                Main.dust[dust].noGravity = true;
            }
        }

        // ==================== PHASE 5: PATIENCE (FINAL PHASE) ====================
        private void Phase5Attacks()
        {
            // Slow moving radial burst of 12 World Aegis Leaf, 3 times per second (20 frames)
            if (AttackTimer % 20 == 0)
            {
                for (int i = 0; i < 12; i++)
                {
                    float angle = (i / 12f) * MathHelper.TwoPi + RotationAngle;
                    Vector2 velocity = new Vector2(
                        (float)Math.Cos(angle),
                        (float)Math.Sin(angle)
                    ) * 2f; // Slow speed

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                            ModContent.ProjectileType<WorldAegisLeaf>(),
                            GetDamage(100), 0f);
                    }
                }
            }
        }

        // Called by Aegon after Patience mechanic completes (30 seconds)
        public void MakeVulnerable()
        {
            NPC.dontTakeDamage = false; // Now can be killed to end the fight
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

        public override void OnKill()
        {
            // When shield is destroyed in Phase 5, end the fight
            if (CurrentState == State.Phase5_HeavilyDamaged && ! NPC. dontTakeDamage)
            {
                // Signal Aegon that fight is over
                if (aegonIndex != -1 && Main.npc[aegonIndex]. active)
                {
                    var aegon = Main.npc[aegonIndex]. ModNPC as Aegon;
                    aegon?. EndFight();
                }
            }
        }

        public override bool CheckActive()
        {
            return false; // Never despawn naturally
        }
    }
}