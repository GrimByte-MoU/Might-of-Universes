// Content/NPCs/Bosses/Aegon/Aegon.cs

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent. Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria. ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Projectiles.EnemyProjectiles;

namespace MightofUniverses. Content.NPCs.Bosses.Aegon
{
    [AutoloadBossHead]
    public class Aegon : ModNPC
    {
        // ==================== AI PHASES ====================
        private enum Phase
        {
            Phase1_Immune = 0,          // Circling, immune, World Aegis fights
            Phase2_FirstAttacks = 1,    // 60% HP, Aegon attacks alone
            Phase3_WithShield = 2,      // Shield returns, both attack
            Phase4_ChunkCircle = 3,     // Orbiting chunks, mirage system
            Phase5_Patience = 4         // Final phase, patience mechanic
        }

        private Phase CurrentPhase
        {
            get => (Phase)NPC.ai[0];
            set => NPC. ai[0] = (float)value;
        }

        // ==================== AI TIMERS ====================
        private ref float AttackTimer => ref NPC.ai[1];
        private ref float AttackState => ref NPC.ai[2];
        private ref float SubTimer => ref NPC.ai[3];

        // ==================== ARENA ====================
        public AegonArena Arena { get; private set; }
        private Vector2 arenaCenter;
        private float arenaRadius;

        // Arena size constants
        private const float ARENA_RADIUS_NORMAL = 50.5f;
        private const float ARENA_RADIUS_EXPERT = 43.5f;
        private const float ARENA_RADIUS_MASTER = 37.5f;

        // ==================== REFERENCES ====================
        private int worldAegisIndex = -1;
        private int[] mirageIndices = new int[3] { -1, -1, -1 };
        private int mirageCount = 0;

        // ==================== PHASE 4 DATA ====================
        private List<int> orbitingChunks = new List<int>();
        private int chunksFired = 0;
        private float orbitAngle = 0f;

        // ==================== PHASE 2 DATA ====================
        private int currentAttack = -1;
        private int attackLoopCount = 0;

        // ==================== PHASE 3 DATA ====================
        private int phase3PositionIndex = 0; // 0=top, 1=mid-right, 2=bottom-left, 3=mid-left
        private bool phase3AttackCActive = false;

        // ==================== PHASE 5 DATA ====================
        private float patienceTimer = 0f;
        private bool patienceActive = false;

        // ==================== DAMAGE CAP ====================
        private int lastHitDamage = 0;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 1; // Single sprite, no animation

            NPCID.Sets. MPAllowedEnemies[Type] = true;
            NPCID.Sets.BossBestiaryPriority.Add(Type);

            NPCID.Sets. NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                CustomTexturePath = "MightofUniverses/Content/NPCs/Bosses/Aegon/Aegon",
                PortraitScale = 0.6f,
                PortraitPositionYOverride = 0f,
            };
            NPCID. Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
        }

        public override void SetDefaults()
        {
            NPC.width = 120;
            NPC.height = 120;
            NPC.defense = 100;
            NPC.lifeMax = 300000;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.value = Item.buyPrice(gold: 50);
            NPC.SpawnWithHigherTime(30);
            NPC.boss = true;
            NPC. npcSlots = 15f;
            NPC.aiStyle = -1;
            NPC.netAlways = true;
            NPC.scale = 1.5f;

            if (Main.expertMode)
            {
                NPC.lifeMax = 450000;
                NPC.defense = 125;
            }

            if (Main.masterMode)
            {
                NPC.lifeMax = 600000;
                NPC.defense = 150;
            }

            Music = MusicID.Boss2; 
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes. Surface,
                new FlavorTextBestiaryInfoElement(
                    "Aegon, the eternal guardian born from the world's core.  " +
                    "His legendary World Aegis has shielded countless civilizations from annihilation.  " +
                    "Until now, nothing has been able to even chip the Aegis."
                )
            });
        }

        public override void OnSpawn(IEntitySource source)
        {
            // Set arena size based on difficulty
            if (Main.masterMode)
                arenaRadius = ARENA_RADIUS_MASTER;
            else if (Main. expertMode)
                arenaRadius = ARENA_RADIUS_EXPERT;
            else
                arenaRadius = ARENA_RADIUS_NORMAL;

            arenaCenter = NPC.Center;

            // Spawn message
            if (Main.netMode != NetmodeID.Server)
            {
                Main.NewText("Aegon, the World's Aegis has been summoned!", 175, 75, 255);
            }

            // Create arena
            Arena = new AegonArena(arenaCenter);
            Arena. Create();

            // Start in Phase 1 (immune)
            CurrentPhase = Phase.Phase1_Immune;
            NPC. dontTakeDamage = true; // Immune until shield reaches 50% HP

            // Spawn World Aegis
            SpawnWorldAegis();
        }

        public override void AI()
        {
            // Find target player
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target]. dead || ! Main.player[NPC.target].active)
            {
                NPC.TargetClosest();
            }

            Player target = Main.player[NPC.target];

            if (! target.active || target.dead)
            {
                DespawnBoss();
                return;
            }

            EnforceArenaConfinement(target);

            if (Arena != null && Arena.IsOutsideArena(NPC.Center))
            {
                NPC. dontTakeDamage = true; // Immune outside arena
                
                // Teleport back to arena
                NPC.Center = Arena.ClampToArena(NPC. Center);
            }
            else if (CurrentPhase != Phase.Phase1_Immune)
            {
                // Only vulnerable inside arena (and not in Phase 1)
                NPC.dontTakeDamage = false;
            }

            // Phase-based AI
            switch (CurrentPhase)
            {
                case Phase.Phase1_Immune:
                    Phase1_CircleArena();
                    break;

                case Phase.Phase2_FirstAttacks:
                    Phase2_AttackPatterns();
                    break;

                case Phase.Phase3_WithShield:
                    Phase3_CombinedAttacks();
                    break;

                case Phase.Phase4_ChunkCircle:
                    Phase4_MirageAndChunks();
                    break;

                case Phase.Phase5_Patience:
                    Phase5_PatienceMechanic();
                    break;
            }

            AttackTimer++;
        }

        // ==================== PHASE 1: IMMUNE & CIRCLING ====================
        private void Phase1_CircleArena()
{
    // Aegon circles the arena clockwise OUTSIDE the walls
    // World Aegis does all the attacking

    float circleSpeed = 0.02f;
    orbitAngle += circleSpeed;

    // Position Aegon OUTSIDE the arena (10-15 tiles beyond the wall)
    float orbitRadius = (arenaRadius + 15) * 16f; // ← INCREASED from +5 to +15
    NPC. Center = arenaCenter + new Vector2(
        (float)Math.Cos(orbitAngle) * orbitRadius,
        (float)Math.Sin(orbitAngle) * orbitRadius
    );

    NPC.velocity = Vector2.Zero;

    // Face inward toward arena center
    NPC.rotation = (arenaCenter - NPC.Center).ToRotation() + MathHelper.PiOver2;

    // Check if World Aegis reached 50% HP
    if (worldAegisIndex != -1 && Main.npc[worldAegisIndex]. active)
    {
        NPC worldAegis = Main.npc[worldAegisIndex];
        if (worldAegis.life <= worldAegis.lifeMax * 0.5f)
        {
            TransitionToPhase2();
        }
    }

    // At 70% World Aegis HP, start firing Hallowed Spears
    if (worldAegisIndex != -1 && Main.npc[worldAegisIndex].active)
    {
        NPC worldAegis = Main.npc[worldAegisIndex];
        float aegisHPPercent = worldAegis.life / (float)worldAegis.lifeMax;

        if (aegisHPPercent <= 0.7f && AttackTimer % 180 == 0)
        {
            FireHallowedSpearSingle();
        }

        if (aegisHPPercent <= 0.6f && AttackTimer % 180 == 0)
        {
            FireHallowedSpearDouble();
        }
    }
}

        private void FireHallowedSpearSingle()
        {
            Player target = Main.player[NPC. target];
            Vector2 spawnPos = NPC.Center;
            Vector2 direction = (target.Center - spawnPos).SafeNormalize(Vector2.UnitX);

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Projectile.NewProjectile(NPC.GetSource_FromAI(), spawnPos, direction * 5f,
                    ModContent.ProjectileType<HallowedSpear>(),
                    GetDamage(120), 0f);
            }
        }

        private void FireHallowedSpearDouble()
        {
            Player target = Main.player[NPC.target];
            Vector2 spawnPos = NPC. Center;
            float baseAngle = (target.Center - spawnPos).ToRotation();

            for (int i = 0; i < 2; i++)
            {
                float spread = MathHelper. Lerp(-0.22f, 0.22f, i / 1f); // 25 degrees ≈ 0.44 radians / 2
                float angle = baseAngle + spread;

                Vector2 velocity = new Vector2(
                    (float)Math.Cos(angle),
                    (float)Math.Sin(angle)
                ) * 5f;

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), spawnPos, velocity,
                        ModContent.ProjectileType<HallowedSpear>(),
                        GetDamage(120), 0f);
                }
            }
        }

        // ==================== TRANSITION:  PHASE 1 → PHASE 2 ====================
        private void TransitionToPhase2()
        {
            CurrentPhase = Phase.Phase2_FirstAttacks;
            NPC.dontTakeDamage = false; // Now vulnerable
            AttackTimer = 0;
            currentAttack = -1;

            // World Aegis flies away
            if (worldAegisIndex != -1 && Main.npc[worldAegisIndex].active)
            {
                var aegis = Main.npc[worldAegisIndex]. ModNPC as WorldAegis;
                aegis?. TransitionToPhase2AndFlyAway();
            }

            // Move Aegon to arena center
            NPC.Center = arenaCenter;
            NPC.velocity = Vector2.Zero;

            // Visual effect
            for (int i = 0; i < 40; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(6f, 6f);
                int dust = Dust.NewDust(NPC.Center, NPC.width, NPC.height, DustID.Stone, velocity.X, velocity.Y, 100, Color.Gold, 2f);
                Main.dust[dust].noGravity = true;
            }
        }

        // ==================== PHASE 2: FIRST ATTACKS ====================
        private void Phase2_AttackPatterns()
        {
            // Aegon does NOT deal contact damage in Phase 2
            NPC.damage = 0;

            Player target = Main.player[NPC.target];

            // Choose random attack if not currently doing one
            if (currentAttack == -1)
            {
                currentAttack = Main.rand.Next(3); // 0 = A, 1 = B, 2 = C
                AttackTimer = 0;
                SubTimer = 0;
            }

            // Execute current attack
            switch (currentAttack)
            {
                case 0:
                    Phase2_AttackA();
                    break;
                case 1:
                    Phase2_AttackB();
                    break;
                case 2:
                    Phase2_AttackC();
                    break;
            }

            // Check for Phase 3 transition (Aegon reaches 60% HP)
            if (NPC.life <= NPC.lifeMax * 0.6f)
            {
                TransitionToPhase3();
            }
        }

        private void Phase2_AttackA()
        {
            // Three spreads of three Hallowed Spears (20 degree spread)
            Player target = Main.player[NPC.target];

            if (SubTimer == 0 || SubTimer == 30 || SubTimer == 60) // 3 spreads, 0.5 seconds apart
            {
                for (int i = 0; i < 3; i++)
                {
                    float baseAngle = (target.Center - NPC.Center).ToRotation();
                    float spread = MathHelper.Lerp(-0.35f, 0.35f, i / 2f); // 20 degrees ≈ 0.35 radians
                    float angle = baseAngle + spread;

                    Vector2 velocity = new Vector2(
                        (float)Math.Cos(angle),
                        (float)Math.Sin(angle)
                    ) * 5f;

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(NPC. GetSource_FromAI(), NPC.Center, velocity,
                            ModContent.ProjectileType<HallowedSpear>(),
                            GetDamage(120), 0f);
                    }
                }
            }

            SubTimer++;

            // Move towards player slowly
            Vector2 targetPos = target.Center;
            Vector2 direction = (targetPos - NPC. Center).SafeNormalize(Vector2.Zero);
            NPC.velocity = direction * 3f;

            // Attack complete after 3 spreads + delay
            if (SubTimer >= 90)
            {
                currentAttack = -1;
                SubTimer = 0;
            }
        }

        private void Phase2_AttackB()
        {
            Player target = Main.player[NPC.target];

            // Radial burst of 12 Hallowed Spears
            if (SubTimer == 0)
            {
                for (int i = 0; i < 12; i++)
                {
                    float angle = (i / 12f) * MathHelper.TwoPi;

                    Vector2 velocity = new Vector2(
                        (float)Math.Cos(angle),
                        (float)Math.Sin(angle)
                    ) * 5f;

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(NPC. GetSource_FromAI(), NPC.Center, velocity,
                            ModContent.ProjectileType<HallowedSpear>(),
                            GetDamage(120), 0f);
                    }
                }
            }

            // Rapidly fire Forest Leaf for 2 seconds (120 frames)
            if (SubTimer > 30 && SubTimer <= 150) // After radial burst
            {
                if (SubTimer % 5 == 0) // Very rapid
                {
                    Vector2 velocity = (target.Center - NPC.Center).SafeNormalize(Vector2.UnitX) * 8f;

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                            ModContent.ProjectileType<ForestLeaf>(),
                            GetDamage(100), 0f);
                    }
                }
            }

            SubTimer++;

            // Drift slowly
            NPC.velocity *= 0.95f;

            // Attack complete
            if (SubTimer >= 180)
            {
                currentAttack = -1;
                SubTimer = 0;
            }
        }

        private void Phase2_AttackC()
        {
            Player target = Main. player[NPC.target];

            // Three spreads of 3 Ocean Spheres centered at player
            if (SubTimer == 0 || SubTimer == 40 || SubTimer == 80)
            {
                for (int i = 0; i < 3; i++)
                {
                    float baseAngle = (target.Center - NPC.Center).ToRotation();
                    float spread = MathHelper. Lerp(-0.3f, 0.3f, i / 2f);
                    float angle = baseAngle + spread;

                    Vector2 velocity = new Vector2(
                        (float)Math.Cos(angle),
                        (float)Math.Sin(angle)
                    ) * 6f;

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(NPC. GetSource_FromAI(), NPC.Center, velocity,
                            ModContent.ProjectileType<OceanSphere>(),
                            GetDamage(120), 0f);
                    }
                }
            }

            SubTimer++;

            // Move towards player
            Vector2 direction = (target.Center - NPC.Center).SafeNormalize(Vector2.Zero);
            NPC.velocity = direction * 4f;

            // Attack complete
            if (SubTimer >= 120)
            {
                currentAttack = -1;
                SubTimer = 0;
            }
        }

        // ==================== TRANSITION: PHASE 2 → PHASE 3 ====================
        private void TransitionToPhase3()
        {
            CurrentPhase = Phase.Phase3_WithShield;
            AttackTimer = 0;
            currentAttack = -1;
            phase3PositionIndex = 0;

            // World Aegis returns
            if (worldAegisIndex != -1 && Main.npc[worldAegisIndex]. active)
            {
                var aegis = Main.npc[worldAegisIndex].ModNPC as WorldAegis;
                aegis?.ReturnToArenaForPhase3(arenaCenter);
            }

            // Move Aegon to top position
            MoveToPhase3Position();

            // Visual effect
            for (int i = 0; i < 50; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(7f, 7f);
                int dust = Dust.NewDust(NPC.Center, NPC.width, NPC.height, DustID.Stone, velocity.X, velocity.Y, 100, Color.Orange, 2.5f);
                Main.dust[dust].noGravity = true;
            }
        }

        // ==================== PHASE 3: WITH SHIELD ====================
        private void Phase3_CombinedAttacks()
        {
            // Aegon has 50% damage reduction in Phase 3
            // (Handled in ModifyIncomingHit)

            // Choose random attack
            if (currentAttack == -1)
            {
                currentAttack = Main. rand.Next(3); // 0 = A, 1 = B, 2 = C
                AttackTimer = 0;
                SubTimer = 0;
                phase3AttackCActive = false;

                // Move to next position
                MoveToPhase3Position();
            }

            // Execute attack
            switch (currentAttack)
            {
                case 0:
                    Phase3_AttackA();
                    break;
                case 1:
                    Phase3_AttackB();
                    break;
                case 2:
                    Phase3_AttackC();
                    break;
            }

            // Check if World Aegis reached 1 HP (transition to Phase 4)
            if (worldAegisIndex != -1 && Main.npc[worldAegisIndex].active)
            {
                NPC worldAegis = Main.npc[worldAegisIndex];
                if (worldAegis. life <= 1)
                {
                    TransitionToPhase4();
                }
            }
        }

        private void MoveToPhase3Position()
        {
            // Cycle through positions:  top → mid-right → bottom-left → mid-left
            Vector2 targetPos = phase3PositionIndex switch
            {
                0 => arenaCenter + new Vector2(0, -(arenaRadius - 15) * 16f), // Top
                1 => arenaCenter + new Vector2((arenaRadius - 15) * 16f, 0),  // Mid-right
                2 => arenaCenter + new Vector2(-(arenaRadius - 15) * 16f, (arenaRadius - 15) * 16f / 2f), // Bottom-left
                3 => arenaCenter + new Vector2(-(arenaRadius - 15) * 16f, 0), // Mid-left
                _ => arenaCenter
            };

            NPC.Center = targetPos;
            NPC.velocity = Vector2.Zero;

            phase3PositionIndex = (phase3PositionIndex + 1) % 4;
        }

        private void Phase3_AttackA()
        {
            // Fire 3 lightly homing Underworld Fireballs (5 degree spread)
            Player target = Main.player[NPC.target];

            if (SubTimer == 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    float baseAngle = (target.Center - NPC.Center).ToRotation();
                    float spread = MathHelper.Lerp(-0.087f, 0.087f, i / 2f); // 5 degrees ≈ 0.087 radians
                    float angle = baseAngle + spread;

                    Vector2 velocity = new Vector2(
                        (float)Math.Cos(angle),
                        (float)Math.Sin(angle)
                    ) * 6f;

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(NPC. GetSource_FromAI(), NPC.Center, velocity,
                            ModContent.ProjectileType<UnderworldFireball>(),
                            GetDamage(110), 0f);
                    }
                }

                // World Aegis fires a fireball too
                if (worldAegisIndex != -1 && Main. npc[worldAegisIndex].active)
                {
                    var aegis = Main.npc[worldAegisIndex]. ModNPC as WorldAegis;
                    aegis?.FireWorldAegisFireball();
                }
            }

            SubTimer++;

            // Attack complete
            if (SubTimer >= 90)
            {
                currentAttack = -1;
                SubTimer = 0;
            }
        }

        private void Phase3_AttackB()
        {
            Player target = Main.player[NPC.target];

            // Double helix of Crimruption Bolts, 2/second
            if (SubTimer % 30 == 0 && SubTimer < 120) // 4 helixes total
            {
                float angle = SubTimer * 0.3f; // Rotating angle

                for (int i = 0; i < 2; i++)
                {
                    float helixAngle = angle + (i * MathHelper.Pi);
                    Vector2 velocity = new Vector2(
                        (float)Math.Cos(helixAngle),
                        (float)Math.Sin(helixAngle)
                    ) * 5f;

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(NPC. GetSource_FromAI(), NPC.Center, velocity,
                            ModContent.ProjectileType<CrimruptionBolt>(),
                            GetDamage(110), 0f);
                    }
                }
            }

            // Radial burst of 10 Jungle Needles after helixes
            if (SubTimer == 150)
            {
                for (int i = 0; i < 10; i++)
                {
                    float angle = (i / 10f) * MathHelper.TwoPi;

                    Vector2 velocity = new Vector2(
                        (float)Math.Cos(angle),
                        (float)Math.Sin(angle)
                    ) * 4f;

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(NPC. GetSource_FromAI(), NPC.Center, velocity,
                            ModContent.ProjectileType<JungleNeedle>(),
                            GetDamage(95), 0f);
                    }
                }
            }

            SubTimer++;

            // Attack complete
            if (SubTimer >= 180)
            {
                currentAttack = -1;
                SubTimer = 0;
            }
        }

        private void Phase3_AttackC()
        {
            Player target = Main.player[NPC.target];

            // Start attack
            if (! phase3AttackCActive)
            {
                phase3AttackCActive = true;

                // World Aegis starts water spiral
                if (worldAegisIndex != -1 && Main. npc[worldAegisIndex].active)
                {
                    var aegis = Main.npc[worldAegisIndex]. ModNPC as WorldAegis;
                    bool clockwise = Main.rand.NextBool();
                    aegis?.StartWaterSpiral(clockwise);
                }
            }

            // Sky Sparks rain down, 5/second
            if (SubTimer % 12 == 0)
            {
                Vector2 spawnPos = target.Center + new Vector2(Main.rand.NextFloat(-400f, 400f), -600f);

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), spawnPos, new Vector2(0, 8f),
                        ModContent.ProjectileType<SkySpark>(),
                        GetDamage(100), 0f);
                }
            }

            // Snow spikes, 4/second
            if (SubTimer % 15 == 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    float baseAngle = (target.Center - NPC.Center).ToRotation();
                    float spread = MathHelper.Lerp(-0.3f, 0.3f, i / 2f);
                    float angle = baseAngle + spread;

                    Vector2 velocity = new Vector2(
                        (float)Math.Cos(angle),
                        (float)Math.Sin(angle)
                    ) * 6f;

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(NPC. GetSource_FromAI(), NPC.Center, velocity,
                            ModContent.ProjectileType<SnowSpike>(),
                            GetDamage(110), 0f);
                    }
                }
            }

            // World Aegis fires water spiral
            if (worldAegisIndex != -1 && Main. npc[worldAegisIndex].active)
            {
                var aegis = Main.npc[worldAegisIndex].ModNPC as WorldAegis;
                aegis?.FireWaterSpiral();
            }

            SubTimer++;

            // Attack lasts 5 seconds
            if (SubTimer >= 300)
            {
                currentAttack = -1;
                SubTimer = 0;
                phase3AttackCActive = false;
            }
        }

        // ==================== TRANSITION:  PHASE 3 → PHASE 4 ====================
        private void TransitionToPhase4()
        {
            CurrentPhase = Phase.Phase4_ChunkCircle;
            AttackTimer = 0;
            currentAttack = -1;
            SubTimer = 0;
            chunksFired = 0;

            // World Aegis flies off scattering fragments
            if (worldAegisIndex != -1 && Main. npc[worldAegisIndex].active)
            {
                var aegis = Main.npc[worldAegisIndex].ModNPC as WorldAegis;
                aegis?.TransitionToPhase4AndScatterFragments();
            }

            // Aegon pauses
            NPC.velocity = Vector2.Zero;

            SpawnOrbitingChunks();

            // Visual effect
            for (int i = 0; i < 60; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(8f, 8f);
                int dust = Dust.NewDust(NPC.Center, NPC.width, NPC.height, DustID.Stone, velocity.X, velocity.Y, 100, Color.Red, 2.5f);
                Main.dust[dust].noGravity = true;
            }
        }
        // ==================== PHASE 4: MIRAGE & ORBITING CHUNKS ====================
        
        private void SpawnOrbitingChunks()
        {
            orbitingChunks. Clear();
            orbitAngle = 0f;

            // Spawn 24 Aegis Chunks in a circle around Aegon
            for (int i = 0; i < 24; i++)
            {
                float angle = (i / 24f) * MathHelper.TwoPi;
                Vector2 offset = new Vector2(
                    (float)Math.Cos(angle),
                    (float)Math.Sin(angle)
                ) * 100f; // 100 pixels from Aegon

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int projIndex = Projectile.NewProjectile(
                        NPC. GetSource_FromAI(),
                        NPC.Center + offset,
                        Vector2.Zero,
                        ModContent.ProjectileType<AegisChunk>(),
                        GetDamage(130),
                        0f
                    );

                    orbitingChunks.Add(projIndex);
                }
            }

            chunksFired = 0;
        }

        private void Phase4_MirageAndChunks()
        {
            Player target = Main.player[NPC. target];

            // Stay 40 blocks to the RIGHT of player
            Vector2 desiredPos = target.Center + new Vector2(40 * 16f, 0);
            
            // Clamp to arena
            if (Arena != null)
            {
                desiredPos = Arena.ClampToArena(desiredPos);
            }

            // Smooth movement
            Vector2 direction = desiredPos - NPC. Center;
            float distance = direction.Length();
            if (distance > 10f)
            {
                direction. Normalize();
                NPC.velocity = direction * 8f;
            }
            else
            {
                NPC.velocity *= 0.9f;
            }

            // Update orbiting chunks
            UpdateOrbitingChunks();

            // Fire one chunk every other second (120 frames)
            if (AttackTimer % 120 == 0 && chunksFired < 24)
            {
                FireOrbitingChunk(target.Center);
            }

            // Choose random attack (can't choose E when chunks remain)
            if (currentAttack == -1)
            {
                if (chunksFired >= 24)
                {
                    // All chunks fired - MUST do Attack E
                    currentAttack = 4; // Attack E
                }
                else
                {
                    // Random attack A, B, C, or D (not E)
                    currentAttack = Main.rand.Next(4); // 0-3
                }
                
                AttackTimer = 0;
                SubTimer = 0;
            }

            // Execute attacks
            switch (currentAttack)
            {
                case 0:
                    Phase4_AttackA();
                    break;
                case 1:
                    Phase4_AttackB();
                    break;
                case 2:
                    Phase4_AttackC();
                    break;
                case 3:
                    Phase4_AttackD();
                    break;
                case 4:
                    Phase4_AttackE();
                    break;
            }

            // Check for Phase 5 transition (Aegon reaches 1% HP)
            if (NPC. life <= NPC.lifeMax * 0.01f)
            {
                TransitionToPhase5();
            }
        }

        private void UpdateOrbitingChunks()
        {
            // Rotate chunks around Aegon
            orbitAngle += 0.05f;

            for (int i = 0; i < orbitingChunks.Count; i++)
            {
                int projIndex = orbitingChunks[i];
                if (projIndex >= 0 && projIndex < Main.maxProjectiles && Main.projectile[projIndex].active)
                {
                    Projectile chunk = Main.projectile[projIndex];
                    
                    float angle = orbitAngle + (i / (float)orbitingChunks.Count) * MathHelper.TwoPi;
                    Vector2 offset = new Vector2(
                        (float)Math.Cos(angle),
                        (float)Math.Sin(angle)
                    ) * 100f;

                    chunk.Center = NPC.Center + offset;
                    chunk.rotation = angle;
                }
            }
        }

        private void FireOrbitingChunk(Vector2 targetPos)
        {
            if (orbitingChunks.Count == 0) return;

            // Get first chunk and fire it at player
            int projIndex = orbitingChunks[0];
            orbitingChunks.RemoveAt(0);

            if (projIndex >= 0 && projIndex < Main.maxProjectiles && Main.projectile[projIndex].active)
            {
                Projectile chunk = Main.projectile[projIndex];
                Vector2 velocity = (targetPos - chunk.Center).SafeNormalize(Vector2.UnitX) * 12f; // Rapid speed
                chunk.velocity = velocity;
            }

            chunksFired++;
        }

        private void Phase4_AttackA()
        {
            Player target = Main.player[NPC.target];

            // Send circle of 5 Aegis Fragments, repeat 5 times
            if (SubTimer % 45 == 0 && SubTimer < 225) // Every 0.75 seconds, 5 times
            {
                for (int i = 0; i < 5; i++)
                {
                    float angle = (i / 5f) * MathHelper.TwoPi;

                    Vector2 velocity = new Vector2(
                        (float)Math.Cos(angle),
                        (float)Math.Sin(angle)
                    ) * 6f;

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                            ModContent.ProjectileType<AegisFragment>(),
                            GetDamage(125), 0f);
                    }
                }

                // Mirage mirrors this attack (flipped)
                FireMirageAttackA(target.Center);
            }

            SubTimer++;

            if (SubTimer >= 270)
            {
                currentAttack = -1;
                SubTimer = 0;
            }
        }

        private void Phase4_AttackB()
        {
            Player target = Main.player[NPC.target];

            // Spread of 9 Aegis Shards with varying speeds, twice
            if (SubTimer == 0 || SubTimer == 60)
            {
                for (int i = 0; i < 9; i++)
                {
                    float baseAngle = (target.Center - NPC.Center).ToRotation();
                    float spread = MathHelper.Lerp(-0.5f, 0.5f, i / 8f);
                    float angle = baseAngle + spread;

                    float speed = 5f + Main.rand.NextFloat(-1f, 2f); // Varying speeds

                    Vector2 velocity = new Vector2(
                        (float)Math.Cos(angle),
                        (float)Math.Sin(angle)
                    ) * speed;

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                            ModContent.ProjectileType<AegisShard>(),
                            GetDamage(120), 0f);
                    }
                }

                // Mirage mirrors
                FireMirageAttackB(target.Center);
            }

            SubTimer++;

            if (SubTimer >= 90)
            {
                currentAttack = -1;
                SubTimer = 0;
            }
        }

        private void Phase4_AttackC()
        {
            Player target = Main.player[NPC.target];

            // Aegis Shards rain down, 10/second
            if (SubTimer % 6 == 0)
            {
                Vector2 spawnPos = target.Center + new Vector2(Main.rand.NextFloat(-400f, 400f), -600f);

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), spawnPos, new Vector2(0, 10f),
                        ModContent.ProjectileType<AegisShard>(),
                        GetDamage(120), 0f);
                }
            }

            // Occasionally send Aegis Fragment at player (every other second)
            if (SubTimer % 120 == 0)
            {
                Vector2 velocity = (target.Center - NPC.Center).SafeNormalize(Vector2.UnitX) * 7f;

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                        ModContent.ProjectileType<AegisFragment>(),
                        GetDamage(125), 0f);
                }

                // Mirage ONLY sends fragments (doesn't copy raining shards)
                FireMirageAttackC_Fragment(target.Center);
            }

            SubTimer++;

            // Lasts 2 seconds
            if (SubTimer >= 120)
            {
                currentAttack = -1;
                SubTimer = 0;
            }
        }

        private void Phase4_AttackD()
        {
            Player target = Main.player[NPC. target];

            // Move from bottom to top of arena over 5 seconds
            float duration = 300f; // 5 seconds
            float progress = SubTimer / duration;

            // Calculate vertical position
            float bottomY = arenaCenter.Y + (arenaRadius - 10) * 16f;
            float topY = arenaCenter.Y - (arenaRadius - 10) * 16f;
            float currentY = MathHelper.Lerp(bottomY, topY, progress);

            // Set position
            NPC.Center = new Vector2(NPC.Center.X, currentY);

            // Fire Aegis Shards horizontally, 5/second
            if (SubTimer % 12 == 0)
            {
                Vector2 velocity = new Vector2(-8f, 0); // Shoot left

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                        ModContent. ProjectileType<AegisShard>(),
                        GetDamage(120), 0f);
                    Main.projectile[proj].ai[0] = 1f;
                }

                // Mirage mirrors (shoots right)
                FireMirageAttackD(progress, true);
            }

            SubTimer++;

            // After reaching top, call them back
            if (SubTimer >= duration)
            {
                // Call back all shards
                RecallAegisShards();

                currentAttack = -1;
                SubTimer = 0;
            }
        }

        private void RecallAegisShards()
        {
            // Find all Aegis Shard projectiles with ai[0] = 1 (stopped at edge)
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.type == ModContent.ProjectileType<AegisShard>() && proj.ai[0] == 1f)
                {
                    // Send back towards Aegon
                    Vector2 velocity = (NPC.Center - proj.Center).SafeNormalize(Vector2.UnitX) * 10f;
                    proj.velocity = velocity;
                    proj.ai[0] = 2f; // Mark as "recalled"
                }
            }
        }

        private void Phase4_AttackE()
{
    Player target = Main.player[NPC.target];

    // Green aura effect
    if (SubTimer == 0)
    {
        NPC.damage = GetContactDamage(150);
        
        for (int i = 0; i < 30; i++)
        {
            int dust = Dust.NewDust(NPC.Center, NPC.width, NPC.height, DustID.GreenTorch, 0f, 0f, 100, Color. Lime, 2f);
            Main.dust[dust].noGravity = true;
        }
    }

    // Charge across screen
    if (SubTimer < 60) // Charge for 1 second
    {
        Vector2 chargeDirection = new Vector2(-1, 0); // Charge left
        NPC.velocity = chargeDirection * 20f; // Fast charge
    }
    else if (SubTimer == 60)
    {
        // Reached other side
        NPC.velocity = Vector2.Zero;

        // Disable contact damage after charge
        NPC.damage = 0;

        // Create mirage if none exists
        if (GetActiveMirageCount() == 0)
        {
            CreateMirage(target.Center + new Vector2(-40 * 16f, 0)); // 40 blocks LEFT of player
        }
    }
    else if (SubTimer < 120) // Charge back
    {
        // Re-enable contact damage for return charge
        NPC.damage = GetContactDamage(150);
        
        Vector2 chargeDirection = new Vector2(1, 0); // Charge right
        NPC. velocity = chargeDirection * 20f;
    }
    else if (SubTimer == 120)
    {
        // Back in position
        NPC.velocity = Vector2.Zero;
        
        // Disable contact damage
        NPC.damage = 0;

        // Respawn 24 orbiting chunks
        SpawnOrbitingChunks();

        currentAttack = -1;
        SubTimer = 0;
    }

    SubTimer++;
}

        // ==================== MIRAGE ATTACK HELPERS ====================

        private void FireMirageAttackA(Vector2 playerPos)
        {
            for (int i = 0; i < mirageIndices.Length; i++)
            {
                int index = mirageIndices[i];
                if (index != -1 && Main.npc[index].active)
                {
                    var mirage = Main.npc[index]. ModNPC as AegonMirage;
                    mirage?. MirrorAttackA_AegisFragmentCircle(playerPos, true);
                }
            }
        }

        private void FireMirageAttackB(Vector2 playerPos)
        {
            for (int i = 0; i < mirageIndices. Length; i++)
            {
                int index = mirageIndices[i];
                if (index != -1 && Main.npc[index].active)
                {
                    var mirage = Main.npc[index].ModNPC as AegonMirage;
                    mirage?. MirrorAttackB_AegisShardSpread(playerPos, true);
                }
            }
        }

        private void FireMirageAttackC_Fragment(Vector2 playerPos)
        {
            for (int i = 0; i < mirageIndices. Length; i++)
            {
                int index = mirageIndices[i];
                if (index != -1 && Main.npc[index].active)
                {
                    var mirage = Main.npc[index].ModNPC as AegonMirage;
                    mirage?. FireAegisFragment(playerPos, true);
                }
            }
        }

        private void FireMirageAttackD(float progress, bool shootingUp)
        {
            for (int i = 0; i < mirageIndices.Length; i++)
            {
                int index = mirageIndices[i];
                if (index != -1 && Main.npc[index]. active)
                {
                    var mirage = Main.npc[index].ModNPC as AegonMirage;
                    mirage?.MirrorAttackD_RapidShards(progress, shootingUp, true);
                }
            }
        }

        private int GetActiveMirageCount()
        {
            int count = 0;
            for (int i = 0; i < mirageIndices.Length; i++)
            {
                if (mirageIndices[i] != -1 && Main.npc[mirageIndices[i]]. active)
                {
                    count++;
                }
            }
            return count;
        }

        private void CreateMirage(Vector2 position)
        {
            // Find empty slot
            for (int i = 0; i < mirageIndices.Length; i++)
            {
                if (mirageIndices[i] == -1 || ! Main.npc[mirageIndices[i]].active)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        int npcIndex = NPC.NewNPC(
                            NPC.GetSource_FromAI(),
                            (int)position.X,
                            (int)position.Y,
                            ModContent.NPCType<AegonMirage>()
                        );

                        mirageIndices[i] = npcIndex;

                        if (Main.netMode == NetmodeID.Server)
                        {
                            NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, npcIndex);
                        }
                    }
                    break;
                }
            }

            mirageCount = GetActiveMirageCount();
        }

        // ==================== TRANSITION:  PHASE 4 → PHASE 5 ====================
        private void TransitionToPhase5()
        {
            CurrentPhase = Phase. Phase5_Patience;
            NPC.life = (int)(NPC.lifeMax * 0.01f); // Lock at 1%
            NPC. dontTakeDamage = true; // Immune
            AttackTimer = 0;
            patienceTimer = 0;
            patienceActive = true;

            // Stop moving
            NPC.velocity = Vector2.Zero;

            // World Aegis returns for final phase
            if (worldAegisIndex != -1 && Main.npc[worldAegisIndex].active)
            {
                var aegis = Main.npc[worldAegisIndex].ModNPC as WorldAegis;
                aegis?.ReturnForPhase5(arenaCenter);
            }

            // Create mirages until 3 exist
            while (GetActiveMirageCount() < 3)
            {
                CreateMirageInCorner();
            }

            // Position mirages in corners
            PositionMiragesInCorners();

            // Visual effect
            for (int i = 0; i < 80; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(10f, 10f);
                int dust = Dust.NewDust(NPC.Center, NPC.width, NPC.height, DustID.Stone, velocity.X, velocity.Y, 100, Color.DarkRed, 3f);
                Main.dust[dust].noGravity = true;
            }
        }

        private void CreateMirageInCorner()
        {
            Vector2 position = arenaCenter; // Placeholder
            CreateMirage(position);
        }

        private void PositionMiragesInCorners()
        {
            // 3 mirages in 3 corners (top-left, top-right, bottom-left/right)
            Vector2[] cornerPositions = new Vector2[]
            {
                arenaCenter + new Vector2(-(arenaRadius - 20) * 16f, -(arenaRadius - 20) * 16f), // Top-left
                arenaCenter + new Vector2((arenaRadius - 20) * 16f, -(arenaRadius - 20) * 16f),  // Top-right
                arenaCenter + new Vector2((arenaRadius - 20) * 16f, (arenaRadius - 20) * 16f)    // Bottom-right
            };

            int mirageIndex = 0;
            for (int i = 0; i < mirageIndices.Length; i++)
            {
                if (mirageIndices[i] != -1 && Main.npc[mirageIndices[i]].active && mirageIndex < cornerPositions.Length)
                {
                    var mirage = Main.npc[mirageIndices[i]]. ModNPC as AegonMirage;
                    mirage?.SetPosition(cornerPositions[mirageIndex]);
                    mirageIndex++;
                }
            }
        }
                // ==================== PHASE 5: PATIENCE MECHANIC ====================
        
        private void Phase5_PatienceMechanic()
        {
            Player target = Main.player[NPC.target];

            // Move Aegon to center if not already there
            if (patienceTimer == 0)
            {
                NPC.Center = arenaCenter;
                NPC.velocity = Vector2.Zero;
            }

            // Disable healing for player
            DisablePlayerHealing(target);

            // Patience lasts 30 seconds (1800 frames)
            if (patienceTimer < 1800)
            {
                // STAGE 1: 0-5 seconds (0-300 frames)
                // World Aegis fires slow World Aegis Leaf radial bursts (handled by WorldAegis AI)

                // STAGE 2: 5-10 seconds (300-600 frames)
                if (patienceTimer >= 300)
                {
                    // Aegon + Mirages fire Jungle Needles centered on arena center, 3x/second
                    if (patienceTimer % 20 == 0)
                    {
                        FireJungleNeedleSpread();
                        FireMirageJungleNeedles();
                    }
                }

                // STAGE 3: 10-15 seconds (600-900 frames)
                if (patienceTimer >= 600)
                {
                    // Aegon + Mirages fire Crimruption Bolts centered on center, 3x/second
                    if (patienceTimer % 20 == 0)
                    {
                        FireCrimruptionBolts();
                        FireMirageCrimruptionBolts();
                    }
                }

                // STAGE 4: 15-20 seconds (900-1200 frames)
                if (patienceTimer >= 900)
                {
                    // Aegon + Mirages fire Ocean Spheres towards center, 3x/second
                    if (patienceTimer % 20 == 0)
                    {
                        FireOceanSphere();
                        FireMirageOceanSpheres();
                    }
                }

                // STAGE 5: 20-30 seconds (1200-1800 frames)
                if (patienceTimer >= 1200)
                {
                    // Aegon + Mirages fire Hallowed Spears at player, 1x/second
                    if (patienceTimer % 60 == 0)
                    {
                        FireHallowedSpearsAtPlayer();
                        FireMirageHallowedSpears();
                    }
                }

                patienceTimer++;
            }
            else
            {
                // Patience complete - make World Aegis vulnerable
                if (worldAegisIndex != -1 && Main.npc[worldAegisIndex].active)
                {
                    var aegis = Main.npc[worldAegisIndex].ModNPC as WorldAegis;
                    aegis?.MakeVulnerable();
                }

                patienceActive = false;

                // Re-enable healing
                EnablePlayerHealing(target);

                // Message
                if (Main.netMode != NetmodeID.Server)
                {
                    Main.NewText("The shield is vulnerable! Strike it down!", 255, 215, 0);
                }

                // Visual effect
                for (int i = 0; i < 100; i++)
                {
                    Vector2 velocity = Main. rand.NextVector2Circular(12f, 12f);
                    int dust = Dust.NewDust(arenaCenter, 100, 100, DustID. Stone, velocity.X, velocity.Y, 100, Color.Gold, 3f);
                    Main.dust[dust].noGravity = true;
                }
            }
        }

        // Patience Stage 2:  Jungle Needles
        private void FireJungleNeedleSpread()
        {
            int spreadCount = 11;
            float baseAngle = (arenaCenter - NPC.Center).ToRotation();

            for (int i = 0; i < spreadCount; i++)
            {
                float offsetAngle = MathHelper. Lerp(-MathHelper.PiOver4, MathHelper.PiOver4, i / 10f);
                float angle = baseAngle + offsetAngle;

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

        private void FireMirageJungleNeedles()
        {
            for (int i = 0; i < mirageIndices.Length; i++)
            {
                int index = mirageIndices[i];
                if (index != -1 && Main.npc[index]. active)
                {
                    var mirage = Main.npc[index].ModNPC as AegonMirage;
                    mirage?.FireJungleNeedleSpread(arenaCenter, true);
                }
            }
        }

        // Patience Stage 3: Crimruption Bolts
        private void FireCrimruptionBolts()
        {
            int spreadCount = 3;
            float baseAngle = (arenaCenter - NPC. Center).ToRotation();

            for (int i = 0; i < spreadCount; i++)
            {
                float offsetAngle = MathHelper.Lerp(-0.2f, 0.2f, i / 2f);
                float angle = baseAngle + offsetAngle;

                Vector2 velocity = new Vector2(
                    (float)Math.Cos(angle),
                    (float)Math.Sin(angle)
                ) * 5f;

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                        ModContent. ProjectileType<CrimruptionBolt>(),
                        GetDamage(110), 0f);
                }
            }
        }

        private void FireMirageCrimruptionBolts()
        {
            for (int i = 0; i < mirageIndices.Length; i++)
            {
                int index = mirageIndices[i];
                if (index != -1 && Main. npc[index].active)
                {
                    var mirage = Main.npc[index]. ModNPC as AegonMirage;
                    mirage?.FireCrimruptionBolts(arenaCenter, true);
                }
            }
        }

        // Patience Stage 4: Ocean Spheres
        private void FireOceanSphere()
        {
            float angle = (arenaCenter - NPC.Center).ToRotation();

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

        private void FireMirageOceanSpheres()
        {
            for (int i = 0; i < mirageIndices.Length; i++)
            {
                int index = mirageIndices[i];
                if (index != -1 && Main.npc[index].active)
                {
                    var mirage = Main. npc[index].ModNPC as AegonMirage;
                    mirage?.FireOceanSphere(arenaCenter, true);
                }
            }
        }

        // Patience Stage 5: Hallowed Spears at player
        private void FireHallowedSpearsAtPlayer()
        {
            Player target = Main.player[NPC.target];
            int spreadCount = 3;
            float baseAngle = (target.Center - NPC.Center).ToRotation();

            for (int i = 0; i < spreadCount; i++)
            {
                float offsetAngle = MathHelper.Lerp(-0.35f, 0.35f, i / 2f); // 20 degrees
                float angle = baseAngle + offsetAngle;

                Vector2 velocity = new Vector2(
                    (float)Math.Cos(angle),
                    (float)Math.Sin(angle)
                ) * 4f;

                if (Main. netMode != NetmodeID. MultiplayerClient)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                        ModContent.ProjectileType<HallowedSpear>(),
                        GetDamage(120), 0f);
                }
            }
        }

        private void FireMirageHallowedSpears()
        {
            Player target = Main.player[NPC.target];

            for (int i = 0; i < mirageIndices. Length; i++)
            {
                int index = mirageIndices[i];
                if (index != -1 && Main.npc[index].active)
                {
                    var mirage = Main.npc[index].ModNPC as AegonMirage;
                    mirage?. FireHallowedSpears(target.Center, true);
                }
            }
        }

        // Disable player healing during Patience
        private void DisablePlayerHealing(Player player)
        {
            player.lifeRegen = -999; // Prevent ALL life regeneration
            player.lifeRegenTime = 0;
            player.potionDelay = int.MaxValue; // Can't use healing potions
        }

        // Re-enable healing after Patience
        private void EnablePlayerHealing(Player player)
        {
            player.potionDelay = 0; // Can use potions again
            // lifeRegen will naturally restore on its own
        }

        // ==================== FIGHT END ====================
        
        public void EndFight()
        {

            NPC.velocity = Vector2.Zero;
            NPC. dontTakeDamage = true;
            NPC.ai[1] = 180;

            // Visual effect - shield shattering
            for (int i = 0; i < 150; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(15f, 15f);
                int dust = Dust.NewDust(arenaCenter, 100, 100, DustID.Stone, velocity.X, velocity.Y, 100, Color.White, 3f);
                Main.dust[dust].noGravity = true;
            }

            // Despawn Aegon after pause
            NPC.active = false;

            // Remove arena
            Arena?. Remove();

            // Despawn mirages
            DespawnMirages();

            // Victory message
            if (Main.netMode != NetmodeID.Server)
            {
                Main. NewText("Aegon has been defeated.. .", 50, 125, 255);
            }
        }


        // ==================== HELPER METHODS ====================

        private void SpawnWorldAegis()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                int npcIndex = NPC.NewNPC(
                    NPC.GetSource_FromAI(),
                    (int)arenaCenter.X,
                    (int)arenaCenter.Y,
                    ModContent.NPCType<WorldAegis>()
                );

                worldAegisIndex = npcIndex;

                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, npcIndex);
                }
            }
        }
        
        private int GetContactDamage(int baseDamage)
{
    if (Main.masterMode)
        return baseDamage * 3; // 150 → 450 damage
    if (Main.expertMode)
        return baseDamage * 2; // 150 → 300 damage
    return baseDamage;         // 150 damage
}

        private void DespawnBoss()
        {
            // Fly away
            NPC.velocity.Y = -15f;
            NPC.noTileCollide = true;

            // Despawn after flying far enough
            if (NPC.timeLeft > 10)
                NPC.timeLeft = 10;

            // Remove arena
            Arena?.Remove();

            // Despawn World Aegis
            if (worldAegisIndex != -1 && Main.npc[worldAegisIndex].active)
            {
                Main.npc[worldAegisIndex].active = false;
            }

            // Despawn mirages
            DespawnMirages();
        }

        private void DespawnMirages()
        {
            for (int i = 0; i < mirageIndices.Length; i++)
            {
                if (mirageIndices[i] != -1 && Main.npc[mirageIndices[i]].active)
                {
                    Main.npc[mirageIndices[i]].active = false;
                }
                mirageIndices[i] = -1;
            }
        }

        private void EnforceArenaConfinement(Player player)
        {
            if (Arena == null || !Arena.IsActive) return;

            // If player leaves arena, teleport them back
            if (Arena.IsOutsideArena(player.Center))
            {
                player.Center = Arena.ClampToArena(player.Center);
                player.velocity = Vector2.Zero;

                // Damage player for trying to escape (optional)
                if (Main.rand.NextBool(20)) // Don't spam damage
                {
                    player. Hurt(Terraria.DataStructures.PlayerDeathReason. ByCustomReason(player.name + " tried to escape Aegon's wrath"), 100, 0);
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

        // ==================== DAMAGE REDUCTION (PHASE 3) ====================

        public override void ModifyIncomingHit(ref NPC.HitModifiers modifiers)
        {
            // Phase 3: 50% damage reduction
            if (CurrentPhase == Phase.Phase3_WithShield)
            {
                modifiers.FinalDamage *= 0.5f;
            }

            // Damage softcap:  If damage > 12% of max HP, reduce by 95%
            int maxDamageBeforeCap = (int)(NPC.lifeMax * 0.12f);
            
            // We'll handle this in ModifyHitByItem and ModifyHitByProjectile
        }

        public override void ModifyHitByItem(Player player, Item item, ref NPC.HitModifiers modifiers)
        {
            // Apply damage softcap
            ApplySoftcap(ref modifiers);
        }

        public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            // Apply damage softcap
            ApplySoftcap(ref modifiers);
        }

        private void ApplySoftcap(ref NPC.HitModifiers modifiers)
        {
            // Calculate potential damage (approximate)
            int estimatedDamage = (int)modifiers.FinalDamage. Base;
            int maxDamageBeforeCap = (int)(NPC.lifeMax * 0.12f);

            if (estimatedDamage > maxDamageBeforeCap)
            {
                // Damage over cap is reduced by 95%
                int excessDamage = estimatedDamage - maxDamageBeforeCap;
                int reducedExcess = (int)(excessDamage * 0.05f); // Keep 5%
                int finalDamage = maxDamageBeforeCap + reducedExcess;

                // Apply modifier
                modifiers.FinalDamage. Flat = finalDamage;
            }
        }

        // ==================== DESPAWN & DEATH ====================

        public override void OnKill()
        {

            if (Main.netMode != NetmodeID.Server)
            {
                Main.NewText("Aegon has been defeated!", 50, 125, 255);
            }

            // Remove arena
            Arena?.Remove();

            // Despawn World Aegis
            if (worldAegisIndex != -1 && Main.npc[worldAegisIndex].active)
            {
                Main.npc[worldAegisIndex].active = false;
            }

            // Despawn mirages
            DespawnMirages();

            // Trigger aftermath
        }

        public override bool CheckActive()
        {
            return false; // Never naturally despawn
        }

        // ==================== LOOT (TEMPORARY - TREASURE BAG LATER) ====================

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            // TODO: Add treasure bag, trophy, relic, mask, etc.
            // For now, just drop some placeholder items

            // Drop Aegis Shards (crafting material)
            npcLoot.Add(ItemDropRule.Common(ItemID.FragmentSolar, 1, 20, 40)); // Placeholder
        }
    }
}