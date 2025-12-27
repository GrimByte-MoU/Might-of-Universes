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
using MightofUniverses.Content.Items.Projectiles. EnemyProjectiles;

namespace MightofUniverses. Content.NPCs.Bosses.Aegon
{
    [AutoloadBossHead]
    public class Aegon : ModNPC
    {
        // ==================== AI PHASES ====================
        private enum Phase
        {
            Phase1_Immune = 0,
            Phase2_FirstAttacks = 1,
            Phase3_WithShield = 2,
            Phase4_ChunkCircle = 3,
            Phase5_Patience = 4
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

        private const float ARENA_RADIUS_NORMAL = 50.5f;
        private const float ARENA_RADIUS_EXPERT = 43.5f;
        private const float ARENA_RADIUS_MASTER = 37.5f;

        // ==================== REFERENCES ====================
        private int worldAegisIndex = -1;
        private List<int> mirageIndices = new List<int> { -1, -1, -1 };

        // ==================== PHASE 4 DATA ====================
        private List<int> orbitingChunks = new List<int>();
        private int chunksFired = 0;
        private float orbitAngle = 0f;

        // ==================== PHASE 2/3 DATA ====================
        private int currentAttack = -1;
        private int phase3PositionIndex = 0;

        // ==================== PHASE 5 DATA ====================
        private float patienceTimer = 0f;
        private bool patienceActive = false;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 1;
            NPCID. Sets.MPAllowedEnemies[Type] = true;
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
            NPC.defense = 120;
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
                NPC.defense = 150;
            }

            if (Main.masterMode)
            {
                NPC. lifeMax = 600000;
                NPC.defense = 180;
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
            if (Main.masterMode)
                arenaRadius = ARENA_RADIUS_MASTER;
            else if (Main.expertMode)
                arenaRadius = ARENA_RADIUS_EXPERT;
            else
                arenaRadius = ARENA_RADIUS_NORMAL;

            arenaCenter = NPC.Center;

            if (Main.netMode != NetmodeID.Server)
            {
                Main.NewText("Aegon, the World's Aegis has been summoned!", 175, 75, 255);
            }

            Arena = new AegonArena(arenaCenter);
            Arena.Create();

            CurrentPhase = Phase.Phase1_Immune;
            NPC. dontTakeDamage = true;

            SpawnWorldAegis();
        }
                public override void AI()
        {
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target]. dead || !Main.player[NPC. target].active)
            {
                NPC.TargetClosest();
            }

            Player target = Main.player[NPC.target];

            // ==================== DESPAWN WHEN PLAYER DIES ====================
            if (! target.active || target.dead)
            {
                NPC.life = 0;
                NPC.active = false;
                
                if (Arena != null)
                {
                    Arena.Remove();
                }
                
                if (worldAegisIndex != -1 && Main.npc[worldAegisIndex]. active)
                {
                    Main.npc[worldAegisIndex]. active = false;
                }
                
                DespawnMirages();
                
                return;
            }

            EnforceArenaConfinement(target);

            if (Arena != null && Arena.IsOutsideArena(NPC.Center))
            {
                NPC. dontTakeDamage = true;
                NPC.Center = Arena.ClampToArena(NPC.Center);
            }
            else if (CurrentPhase != Phase.Phase1_Immune)
            {
                NPC. dontTakeDamage = false;
            }

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
            float circleSpeed = 0.02f;
            orbitAngle += circleSpeed;

            float orbitRadius = (arenaRadius + 15) * 16f;
            NPC.Center = arenaCenter + new Vector2(
                (float)Math.Cos(orbitAngle) * orbitRadius,
                (float)Math.Sin(orbitAngle) * orbitRadius
            );

            NPC.velocity = Vector2.Zero;
            NPC.rotation = (arenaCenter - NPC.Center).ToRotation() + MathHelper.PiOver2;

            if (worldAegisIndex != -1 && Main.npc[worldAegisIndex].active)
            {
                NPC worldAegis = Main.npc[worldAegisIndex];
                if (worldAegis.life <= worldAegis.lifeMax * 0.5f)
                {
                    TransitionToPhase2();
                }

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
            Vector2 direction = (target.Center - NPC.Center).SafeNormalize(Vector2.UnitX);

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, direction * 5f,
                    ModContent.ProjectileType<HallowedSpear>(),
                    120, 0f);
            }
        }

        private void FireHallowedSpearDouble()
        {
            Player target = Main.player[NPC.target];
            float baseAngle = (target.Center - NPC.Center).ToRotation();

            for (int i = 0; i < 2; i++)
            {
                float spread = MathHelper. Lerp(-0.22f, 0.22f, i / 1f);
                float angle = baseAngle + spread;

                Vector2 velocity = new Vector2(
                    (float)Math.Cos(angle),
                    (float)Math.Sin(angle)
                ) * 5f;

                if (Main. netMode != NetmodeID. MultiplayerClient)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                        ModContent.ProjectileType<HallowedSpear>(),
                        120, 0f);
                }
            }
        }

        // ==================== TRANSITION:  PHASE 1 → PHASE 2 ====================
        private void TransitionToPhase2()
        {
            CurrentPhase = Phase.Phase2_FirstAttacks;
            NPC. dontTakeDamage = false;
            NPC.rotation = 0f;
            AttackTimer = 0;
            currentAttack = -1;

            if (worldAegisIndex != -1 && Main.npc[worldAegisIndex]. active)
            {
                var aegis = Main.npc[worldAegisIndex]. ModNPC as WorldAegis;
                aegis?. TransitionToPhase2AndFlyAway();
            }

            NPC.Center = arenaCenter;
            NPC.velocity = Vector2.Zero;

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
            NPC.damage = 0;
            NPC.rotation = 0f;

            Player target = Main.player[NPC.target];

            if (currentAttack == -1)
            {
                currentAttack = Main.rand.Next(3);
                AttackTimer = 0;
                SubTimer = 0;
            }

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

            if (NPC.life <= NPC.lifeMax * 0.6f)
            {
                TransitionToPhase3();
            }
        }

        private void Phase2_AttackA()
        {
            Player target = Main.player[NPC.target];

            if (SubTimer == 0 || SubTimer == 30 || SubTimer == 60)
            {
                for (int i = 0; i < 3; i++)
                {
                    float baseAngle = (target.Center - NPC.Center).ToRotation();
                    float spread = MathHelper.Lerp(-0.35f, 0.35f, i / 2f);
                    float angle = baseAngle + spread;

                    Vector2 velocity = new Vector2(
                        (float)Math.Cos(angle),
                        (float)Math.Sin(angle)
                    ) * 5f;

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(NPC. GetSource_FromAI(), NPC.Center, velocity,
                            ModContent.ProjectileType<HallowedSpear>(),
                            120, 0f);
                    }
                }
            }

            SubTimer++;

            Vector2 direction = (target.Center - NPC.Center).SafeNormalize(Vector2.Zero);
            NPC.velocity = direction * 3f;

            if (SubTimer >= 90)
            {
                currentAttack = -1;
                SubTimer = 0;
            }
        }

        private void Phase2_AttackB()
        {
            Player target = Main.player[NPC.target];

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
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                            ModContent.ProjectileType<HallowedSpear>(),
                            120, 0f);
                    }
                }
            }

            if (SubTimer > 30 && SubTimer <= 150)
            {
                if (SubTimer % 45 == 0)
                {
                    Vector2 velocity = (target.Center - NPC.Center).SafeNormalize(Vector2.UnitX) * 8f;

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                            ModContent.ProjectileType<ForestLeaf>(),
                            100, 0f);
                    }
                }
            }

            SubTimer++;
            NPC.velocity *= 0.95f;

            if (SubTimer >= 180)
            {
                currentAttack = -1;
                SubTimer = 0;
            }
        }

        private void Phase2_AttackC()
        {
            Player target = Main. player[NPC.target];

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
                            120, 0f);
                    }
                }
            }

            SubTimer++;

            Vector2 direction = (target.Center - NPC.Center).SafeNormalize(Vector2.Zero);
            NPC.velocity = direction * 4f;

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
            NPC. rotation = 0f;
            AttackTimer = 0;
            currentAttack = -1;
            phase3PositionIndex = 0;

            if (worldAegisIndex != -1 && Main.npc[worldAegisIndex].active)
            {
                var aegis = Main.npc[worldAegisIndex].ModNPC as WorldAegis;
                aegis?.ReturnToArenaForPhase3(arenaCenter);
            }

            MoveToPhase3Position();

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
            NPC.damage = 0;
            NPC.rotation = 0f;

            float aegonHPPercent = NPC. life / (float)NPC.lifeMax;
            
            if (aegonHPPercent <= 0.30f)
            {
                TransitionToPhase4();
                return;
            }

            if (worldAegisIndex == -1 || !Main.npc[worldAegisIndex]. active)
            {
                TransitionToPhase4();
                return;
            }

            NPC worldAegis = Main.npc[worldAegisIndex];
            if (worldAegis.life <= 1)
            {
                var aegis = worldAegis. ModNPC as WorldAegis;
                aegis?.TransitionToPhase4AndScatterFragments();
                
                TransitionToPhase4();
                return;
            }

            if (currentAttack == -1)
            {
                if (AttackTimer >= 120)
                {
                    currentAttack = Main.rand.Next(3);
                    AttackTimer = 0;
                    SubTimer = 0;
                    MoveToPhase3Position();
                }
            }
            else
            {
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
            }
        }

        private void MoveToPhase3Position()
        {
            Vector2[] positions = new Vector2[]
            {
                arenaCenter + new Vector2(0, -arenaRadius * 16f * 0.6f),
                arenaCenter + new Vector2(arenaRadius * 16f * 0.6f, 0),
                arenaCenter + new Vector2(0, arenaRadius * 16f * 0.6f),
                arenaCenter + new Vector2(-arenaRadius * 16f * 0.6f, 0)
            };

            phase3PositionIndex = (phase3PositionIndex + 1) % positions.Length;
            Vector2 targetPos = positions[phase3PositionIndex];

            for (int i = 0; i < 30; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(8f, 8f);
                int dust = Dust.NewDust(NPC.Center, NPC.width, NPC.height, 
                    DustID.GoldFlame, velocity.X, velocity.Y, 100, Color.Gold, 2f);
                Main.dust[dust].noGravity = true;
            }

            NPC. Center = targetPos;
            NPC.velocity = Vector2.Zero;

            for (int i = 0; i < 30; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(8f, 8f);
                int dust = Dust.NewDust(NPC.Center, NPC.width, NPC.height, 
                    DustID.GoldFlame, velocity. X, velocity.Y, 100, Color.Gold, 2f);
                Main.dust[dust].noGravity = true;
            }
        }

        private void Phase3_AttackA()
        {
            if (SubTimer == 0)
            {
                for (int i = 0; i < 40; i++)
                {
                    int dust = Dust.NewDust(NPC.Center, NPC.width, NPC.height, 
                        DustID.JungleGrass, 0f, 0f, 100, Color.Green, 2.5f);
                    Main. dust[dust].noGravity = true;
                }
            }

            if (SubTimer % 60 == 0 && SubTimer <= 210)
            {
                Player target = Main.player[NPC.target];

                for (int i = 0; i < 3; i++)
                {
                    float baseAngle = (target.Center - NPC.Center).ToRotation();
                    float spread = MathHelper.Lerp(-0.087f, 0.087f, i / 2f);
                    float angle = baseAngle + spread;

                    Vector2 velocity = new Vector2(
                        (float)Math.Cos(angle),
                        (float)Math.Sin(angle)
                    ) * 6f;

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(NPC. GetSource_FromAI(), NPC.Center, velocity,
                            ModContent.ProjectileType<UnderworldFireball>(),
                            110, 0f);
                    }
                }

                if (worldAegisIndex != -1 && Main.npc[worldAegisIndex].active)
                {
                    var aegis = Main.npc[worldAegisIndex].ModNPC as WorldAegis;
                    aegis?.FireWorldAegisFireball();
                }
            }

            SubTimer++;

            if (SubTimer >= 240)
            {
                currentAttack = -1;
                SubTimer = 0;
            }
        }

        private void Phase3_AttackB()
        {
            Player target = Main.player[NPC.target];

            if (SubTimer == 0)
            {
                for (int i = 0; i < 40; i++)
                {
                    Color telegraphColor = Main.rand.NextBool() ? Color.Orange : Color. Cyan;
                    int dustType = Main.rand.NextBool() ? DustID. Torch : DustID.Ice;
                    int dust = Dust.NewDust(NPC.Center, NPC.width, NPC.height, 
                        dustType, 0f, 0f, 100, telegraphColor, 2.5f);
                    Main. dust[dust].noGravity = true;
                }
            }

            if (SubTimer % 45 == 0 && SubTimer < 120)
            {
                float angle = SubTimer * 0.3f;

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
                            120, 0f);
                    }
                }
            }

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
                            95, 0f);
                    }
                }
            }

            SubTimer++;

            if (SubTimer >= 180)
            {
                currentAttack = -1;
                SubTimer = 0;
            }
        }

        private void Phase3_AttackC()
        {
            Player target = Main. player[NPC.target];

            if (SubTimer == 0)
            {
                for (int i = 0; i < 40; i++)
                {
                    int dust = Dust.NewDust(NPC.Center, NPC.width, NPC.height, 
                        DustID. Water, 0f, 0f, 100, Color.Blue, 2.5f);
                    Main.dust[dust].noGravity = true;
                }

                if (worldAegisIndex != -1 && Main.npc[worldAegisIndex].active)
                {
                    var aegis = Main.npc[worldAegisIndex].ModNPC as WorldAegis;
                    bool clockwise = Main.rand.NextBool();
                    aegis?.StartWaterSpiral(clockwise);
                }
            }

            if (SubTimer % 30 == 0)
            {
                Vector2 spawnPos = target.Center + new Vector2(Main.rand.NextFloat(-400f, 400f), -600f);

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), spawnPos, new Vector2(0, 8f),
                        ModContent.ProjectileType<SkySpark>(),
                        100, 0f);
                }
            }

            if (SubTimer % 60 == 0)
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
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                            ModContent.ProjectileType<SnowSpike>(),
                            110, 0f);
                    }
                }
            }

            if (worldAegisIndex != -1 && Main.npc[worldAegisIndex].active)
            {
                var aegis = Main.npc[worldAegisIndex].ModNPC as WorldAegis;
                aegis?. FireWaterSpiral();
            }

            SubTimer++;

            if (SubTimer >= 300)
            {
                currentAttack = -1;
                SubTimer = 0;
            }
        }

        // ==================== TRANSITION:  PHASE 3 → PHASE 4 ====================
        private void TransitionToPhase4()
        {
            CurrentPhase = Phase.Phase4_ChunkCircle;
            NPC.rotation = 0f;
            AttackTimer = 0;
            currentAttack = -1;
            SubTimer = 0;
            chunksFired = 0;

            if (worldAegisIndex != -1 && Main.npc[worldAegisIndex]. active)
            {
                var aegis = Main.npc[worldAegisIndex].ModNPC as WorldAegis;
                aegis?.TransitionToPhase4AndScatterFragments();
            }

            NPC.velocity = Vector2.Zero;
            SpawnOrbitingChunks();

            for (int i = 0; i < 60; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(8f, 8f);
                int dust = Dust.NewDust(NPC.Center, NPC.width, NPC.height, DustID.Stone, velocity.X, velocity.Y, 100, Color.Red, 2.5f);
                Main.dust[dust].noGravity = true;
            }
        }
                // ==================== PHASE 4: MIRAGE & ORBITING CHUNKS ====================

// ==================== PHASE 4: MIRAGE & ORBITING CHUNKS ====================

private bool didFirstPhase4AttackE = false; // Guarantee mirage

private void Phase4_MirageAndChunks()
{
    NPC.noTileCollide = true; // Travel through walls in this phase!
    NPC.damage = 0;
    NPC.rotation = 0f;

    float hpPercent = NPC.life / (float)NPC.lifeMax;
    
    if (hpPercent <= 0.15f)
    {
        NPC.noTileCollide = false; // Restore collision on phase end
        TransitionToPhase5();
        return;
    }

    // ===== KEEP AEGON 40 BLOCKS TO THE RIGHT OF PLAYER =====
    Player target = Main.player[NPC.target];
    Vector2 desiredPosition = target.Center + new Vector2(40 * 16f, 0); // 40 blocks right
    Vector2 direction = desiredPosition - NPC.Center;
    float distance = direction.Length();
    
    if (distance > 10f)
    {
        direction.Normalize();
        NPC.velocity = direction * Math.Min(distance * 0.1f, 8f); // Smooth follow
    }
    else
    {
        NPC.velocity *= 0.9f;
    }

    UpdateOrbitingChunks();

    // Fire chunk every 30 frames (0.5 seconds)
    if (AttackTimer % 60 == 0 && chunksFired < 24)
    {
        FireOrbitingChunk(target.Center);
    }

    // Attack selection: Always start with Attack E
    if (currentAttack == -1)
    {
        if (!didFirstPhase4AttackE)
        {
            currentAttack = 4; // Attack E (charge/mirage)
            didFirstPhase4AttackE = true;
        }
        else if (chunksFired >= 24)
        {
            currentAttack = 4; // Force Attack E for safety if chunks fired
        }
        else
        {
            currentAttack = Main.rand.Next(4); // Random 0-3 (A, B, C, D)
        }
        AttackTimer = 0;
        SubTimer = 0;
    }

    switch (currentAttack)
    {
        case 0: Phase4_AttackA(); break;
        case 1: Phase4_AttackB(); break;
        case 2: Phase4_AttackC(); break;
        case 3: Phase4_AttackD(); break;
        case 4: Phase4_AttackE(); break;
    }
}

private void SpawnOrbitingChunks()
{
    orbitingChunks.Clear();
    orbitAngle = 0f;

    for (int i = 0; i < 24; i++)
    {
        float angle = (i / 24f) * MathHelper.TwoPi;
        Vector2 offset = new Vector2(
            (float)Math.Cos(angle),
            (float)Math.Sin(angle)
        ) * 150f;

        if (Main.netMode != NetmodeID.MultiplayerClient)
        {
            int projIndex = Projectile.NewProjectile(
                NPC.GetSource_FromAI(),
                NPC.Center + offset,
                Vector2.Zero,
                ModContent.ProjectileType<AegisChunk>(),
                130,
                0f
            );

            if (projIndex >= 0 && projIndex < Main.maxProjectiles)
            {
                orbitingChunks.Add(projIndex);
            }
        }
    }

    chunksFired = 0;
}

private void UpdateOrbitingChunks()
{
    orbitAngle += 0.05f;

    for (int i = orbitingChunks.Count - 1; i >= 0; i--)
    {
        int projIndex = orbitingChunks[i];
        if (projIndex >= 0 && projIndex < Main.maxProjectiles && Main.projectile[projIndex].active)
        {
            Projectile chunk = Main.projectile[projIndex];
            if (chunk.velocity == Vector2.Zero)
            {
                float angle = orbitAngle + (i / (float)orbitingChunks.Count) * MathHelper.TwoPi;
                Vector2 offset = new Vector2(
                    (float)Math.Cos(angle),
                    (float)Math.Sin(angle)
                ) * 150f;

                chunk.Center = NPC.Center + offset;
                chunk.rotation = angle;
            }
        }
        else
        {
            orbitingChunks.RemoveAt(i);
        }
    }
}

private void FireOrbitingChunk(Vector2 targetPos)
{
    if (orbitingChunks.Count == 0) return;

    int projIndex = orbitingChunks[0];
    orbitingChunks.RemoveAt(0);

    if (projIndex >= 0 && projIndex < Main.maxProjectiles && Main.projectile[projIndex].active)
    {
        Projectile chunk = Main.projectile[projIndex];
        Vector2 velocity = (targetPos - chunk.Center).SafeNormalize(Vector2.UnitX) * 12f;
        chunk.velocity = velocity;
        chunk.ai[0] = 1f;
    }

    chunksFired++;
}

// ==================== PHASE 4 ATTACKS ====================

private void Phase4_AttackA()
{
    Player target = Main.player[NPC.target];

    if (SubTimer % 60 == 0 && SubTimer < 300)
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
                int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                    ModContent.ProjectileType<AegisFragment>(),
                    125, 0f);
                Main.projectile[proj].ai[0] = 1f;
            }
        }

        FireMirageAttackA(target.Center);
    }

    SubTimer++;
    if (SubTimer >= 300)
    {
        currentAttack = -1;
        SubTimer = 0;
    }
}

private void Phase4_AttackB()
{
    Player target = Main.player[NPC.target];

    if (SubTimer == 0 || SubTimer == 90)
    {
        for (int i = 0; i < 9; i++)
        {
            float baseAngle = (target.Center - NPC.Center).ToRotation();
            float spread = MathHelper.Lerp(-0.5f, 0.5f, i / 8f);
            float angle = baseAngle + spread;
            float speed = 5f + Main.rand.NextFloat(-1f, 2f);

            Vector2 velocity = new Vector2(
                (float)Math.Cos(angle),
                (float)Math.Sin(angle)
            ) * speed;

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                    ModContent.ProjectileType<AegisShard>(),
                    120, 0f);
            }
        }

        FireMirageAttackB(target.Center);
    }

    SubTimer++;
    if (SubTimer >= 120)
    {
        currentAttack = -1;
        SubTimer = 0;
    }
}

private void Phase4_AttackC()
{
    Player target = Main.player[NPC.target];

    // Shards rain
    if (SubTimer % 20 == 0 && SubTimer < 120)
    {
        Vector2 spawnPos = target.Center + new Vector2(Main.rand.NextFloat(-400f, 400f), -600f);

        if (Main.netMode != NetmodeID.MultiplayerClient)
        {
            Projectile.NewProjectile(NPC.GetSource_FromAI(), spawnPos, new Vector2(0, 10f),
                ModContent.ProjectileType<AegisShard>(),
                120, 0f);
        }
    }

    // Fragments every 0.5 seconds
    if (SubTimer % 60 == 0 && SubTimer < 120)
    {
        Vector2 velocity = (target.Center - NPC.Center).SafeNormalize(Vector2.UnitX) * 7f;
        if (Main.netMode != NetmodeID.MultiplayerClient)
        {
            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                ModContent.ProjectileType<AegisFragment>(),
                125, 0f);
        }
        FireMirageAttackC_Fragment(target.Center);
    }

    SubTimer++;
    if (SubTimer >= 120)
    {
        currentAttack = -1;
        SubTimer = 0;
    }
}

private void Phase4_AttackD()
{
    Player target = Main.player[NPC.target];
    float duration = 300f;
    float progress = SubTimer / duration;

    float bottomY = arenaCenter.Y + (arenaRadius - 10) * 16f;
    float topY = arenaCenter.Y - (arenaRadius - 10) * 16f;
    float currentY = MathHelper.Lerp(bottomY, topY, progress);

    float targetX = target.Center.X + (40 * 16f);
    NPC.Center = new Vector2(targetX, currentY);
    NPC.velocity = Vector2.Zero;

    if (SubTimer % 60 == 0)
    {
        Vector2 velocity = new Vector2(-8f, 0);

        if (Main.netMode != NetmodeID.MultiplayerClient)
        {
            int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                ModContent.ProjectileType<AegisShard>(),
                120, 0f);
            Main.projectile[proj].ai[0] = 1f;
        }

        FireMirageAttackD(progress, true);
    }

    SubTimer++;
    if (SubTimer >= duration)
    {
        RecallAegisShards();
        currentAttack = -1;
        SubTimer = 0;
    }
}

private void RecallAegisShards()
{
    for (int i = 0; i < Main.maxProjectiles; i++)
    {
        Projectile proj = Main.projectile[i];
        if (proj.active && proj.type == ModContent.ProjectileType<AegisShard>() && proj.ai[0] == 1f)
        {
            Vector2 velocity = (NPC.Center - proj.Center).SafeNormalize(Vector2.UnitX) * 10f;
            proj.velocity = velocity;
            proj.ai[0] = 2f;
        }
    }
}

private void Phase4_AttackE()
{
    Player target = Main.player[NPC.target];

    if (SubTimer == 0)
    {
        NPC.damage = GetContactDamage(150);
        for (int i = 0; i < 30; i++)
        {
            int dust = Dust.NewDust(NPC.Center, NPC.width, NPC.height, DustID.GreenTorch, 0f, 0f, 100, Color.Lime, 2f);
            Main.dust[dust].noGravity = true;
        }
    }

    // Charge left
    if (SubTimer < 60)
    {
        NPC.velocity = new Vector2(-20f, 0);
    }
    // Stop and create mirage
    else if (SubTimer == 60)
    {
        NPC.velocity = Vector2.Zero;
        NPC.damage = 0;

        if (GetActiveMirageCount() == 0)
        {
            Vector2 miragePos = target.Center + new Vector2(-40 * 16f, 0);
            CreateMirage(miragePos);
        }
    }
    else if (SubTimer < 120)
    {
        NPC.damage = GetContactDamage(150);
        NPC.velocity = new Vector2(20f, 0);
    }
    // Stop and reset
    else if (SubTimer == 120)
    {
        NPC.velocity = Vector2.Zero;
        NPC.damage = 0;

        SpawnOrbitingChunks();
        currentAttack = -1;
        SubTimer = 0;
    }

    SubTimer++;
}

        // ==================== MIRAGE ATTACK HELPERS ====================

        private void FireMirageAttackA(Vector2 playerPos)
        {
            for (int i = 0; i < mirageIndices.Count; i++)
            {
                int index = mirageIndices[i];
                if (index != -1 && Main.npc[index].active)
                {
                    var mirage = Main.npc[index].ModNPC as AegonMirage;
                    mirage?.MirrorAttackA_AegisFragmentCircle(playerPos, true);
                }
            }
        }

        private void FireMirageAttackB(Vector2 playerPos)
        {
            for (int i = 0; i < mirageIndices. Count; i++)
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
            for (int i = 0; i < mirageIndices.Count; i++)
            {
                int index = mirageIndices[i];
                if (index != -1 && Main.npc[index].active)
                {
                    var mirage = Main.npc[index].ModNPC as AegonMirage;
                    mirage?.FireAegisFragment(playerPos, true);
                }
            }
        }

        private void FireMirageAttackD(float progress, bool shootingUp)
        {
            for (int i = 0; i < mirageIndices.Count; i++)
            {
                int index = mirageIndices[i];
                if (index != -1 && Main.npc[index].active)
                {
                    var mirage = Main.npc[index].ModNPC as AegonMirage;
                    mirage?.MirrorAttackD_RapidShards(progress, shootingUp, true);
                }
            }
        }

        private int GetActiveMirageCount()
        {
            int count = 0;
            for (int i = 0; i < mirageIndices.Count; i++)
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
            for (int i = 0; i < mirageIndices.Count; i++)
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
                            NetMessage.SendData(MessageID. SyncNPC, -1, -1, null, npcIndex);
                        }
                    }
                    break;
                }
            }
        }
        // ==================== TRANSITION:   PHASE 4 → PHASE 5 ====================

private void TransitionToPhase5()
{
    CurrentPhase = Phase.Phase5_Patience;
    NPC.life = (int)(NPC.lifeMax * 0.01f);
    NPC.dontTakeDamage = true;
    NPC.rotation = 0f;
    NPC.velocity = Vector2.Zero;
    AttackTimer = 0;
    patienceTimer = 0;
    patienceActive = true;

    // ===== AEGON GOES TO BOTTOM-LEFT CORNER =====
    Vector2 aegonCorner = arenaCenter + new Vector2(-(arenaRadius - 5) * 16f, (arenaRadius - 5) * 16f);
    NPC.Center = aegonCorner;

    // ===== WORLD AEGIS TO CENTER =====
    if (worldAegisIndex != -1 && Main.npc[worldAegisIndex].active)
    {
        NPC worldAegis = Main.npc[worldAegisIndex];
        worldAegis.Center = arenaCenter;
        worldAegis.velocity = Vector2.Zero;
        worldAegis.active = true;

        var aegis = worldAegis.ModNPC as WorldAegis;
        aegis?.ReturnForPhase5(arenaCenter);
    }

    while (GetActiveMirageCount() < 3)
    {
        CreateMirageInCorner();
    }

    PositionMiragesInCorners();

    for (int i = 0; i < 80; i++)
    {
        Vector2 velocity = Main.rand.NextVector2Circular(10f, 10f);
        int dust = Dust.NewDust(arenaCenter, 100, 100, DustID.Stone, velocity.X, velocity.Y, 100, Color.DarkRed, 3f);
        Main.dust[dust].noGravity = true;
    }

    if (Main.netMode != NetmodeID.Server)
    {
        Main.NewText("You have strength but do you have patience?", Color.Red);
    }
}

private void CreateMirageInCorner()
{
    // Will be repositioned soon after
    Vector2 position = arenaCenter;
    CreateMirage(position);
}

private void PositionMiragesInCorners()
{
    Vector2[] cornerPositions = new Vector2[]
    {
        arenaCenter + new Vector2(-(arenaRadius - 5) * 16f, -(arenaRadius - 5) * 16f), // Top-left
        arenaCenter + new Vector2((arenaRadius - 5) * 16f, -(arenaRadius - 5) * 16f),  // Top-right
        arenaCenter + new Vector2((arenaRadius - 5) * 16f, (arenaRadius - 5) * 16f)    // Bottom-right
    };

    int mirageIndex = 0;
    for (int i = 0; i < mirageIndices.Count; i++)
    {
        if (mirageIndices[i] != -1 && mirageIndices[i] < Main.maxNPCs && Main.npc[mirageIndices[i]].active && mirageIndex < cornerPositions.Length)
        {
            NPC mirage = Main.npc[mirageIndices[i]];
            mirage.Center = cornerPositions[mirageIndex];
            mirage.velocity = Vector2.Zero;

            var mirageNPC = mirage.ModNPC as AegonMirage;
            mirageNPC?.SetPosition(cornerPositions[mirageIndex]);
            
            mirageIndex++;
        }
    }
}
        private void Phase5_PatienceMechanic()
        {
            NPC.damage = 0;
            NPC.rotation = 0f;
            NPC.dontTakeDamage = true;

            // ===== AEGON IN BOTTOM-LEFT CORNER (NOT CENTER) =====
            Vector2 aegonCorner = arenaCenter + new Vector2(-(arenaRadius - 5) * 16f, (arenaRadius - 5) * 16f);
            NPC.Center = aegonCorner;
            NPC.velocity = Vector2.Zero;

            Player target = Main.player[NPC.target];

            DisablePlayerHealing(target);

            patienceTimer++;

            if (patienceTimer % 60 == 0)
            {
                int secondsLeft = (1800 - (int)patienceTimer) / 60;
                if (Main.netMode != NetmodeID.Server)
                {
                    Main.NewText($"Patience: {secondsLeft} seconds remaining", Color.Yellow);
                }
            }

            // Stage 1: Sky Sparks (0-6 seconds)
            if (patienceTimer >= 0 && patienceTimer < 360)
            {
                if (patienceTimer % 30 == 0)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        float xPos = arenaCenter.X + Main.rand.NextFloat(-arenaRadius * 16f, arenaRadius * 16f);
                        float yPos = arenaCenter.Y - (arenaRadius + 20) * 16f;
                        Vector2 spawnPos = new Vector2(xPos, yPos);
                        Vector2 velocity = new Vector2(0, 4f);

                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), spawnPos, velocity,
                                ModContent.ProjectileType<SkySpark>(),
                                100, 0f);
                        }
                    }
                }
            }

            // Stage 2: Jungle Needles (6-12 seconds)
            if (patienceTimer >= 360 && patienceTimer < 720)
            {
                if (patienceTimer % 60 == 0)
                {
                    FireJungleNeedleSpread();
                    FireMirageJungleNeedles();
                }
            }

            // Stage 3: Crimruption Bolts (12-18 seconds)
            if (patienceTimer >= 720 && patienceTimer < 1080)
            {
                if (patienceTimer % 60 == 0)
                {
                    FireCrimruptionBolts();
                    FireMirageCrimruptionBolts();
                }
            }

            // Stage 4: Ocean Spheres (18-24 seconds)
            if (patienceTimer >= 1080 && patienceTimer < 1440)
            {
                if (patienceTimer % 60 == 0)
                {
                    FireOceanSphere();
                    FireMirageOceanSpheres();
                }
            }

            // Stage 5: Hallowed Spears (24-30 seconds)
            if (patienceTimer >= 1440 && patienceTimer < 1800)
            {
                if (patienceTimer % 120 == 0)
                {
                    FireHallowedSpearsAtPlayer();
                    FireMirageHallowedSpears();
                }
            }

            // After 30 seconds
            if (patienceTimer >= 1800)
            {
                patienceActive = false;
                EnablePlayerHealing(target);

                if (worldAegisIndex != -1 && Main.npc[worldAegisIndex].active)
                {
                    var aegis = Main.npc[worldAegisIndex].ModNPC as WorldAegis;
                    aegis?.MakeVulnerable();
                }

                if (Main.netMode != NetmodeID.Server)
                {
                    Main.NewText("The World Aegis is now vulnerable! Destroy it to end the fight!", Color.Gold);
                }

                patienceTimer = 1800;
            }
        }

private void FireJungleNeedleSpread()
{
    int spreadCount = 7;
    float baseAngle = (arenaCenter - NPC.Center).ToRotation();
    
    for (int i = 0; i < spreadCount; i++)
    {
        float offsetAngle = MathHelper.Lerp(-MathHelper.PiOver4, MathHelper.PiOver4, i / (float)(spreadCount - 1));
        float angle = baseAngle + offsetAngle;

        Vector2 velocity = new Vector2(
            (float)Math.Cos(angle),
            (float)Math.Sin(angle)
        ) * 3f;

        if (Main.netMode != NetmodeID.MultiplayerClient)
        {
            Projectile.NewProjectile(NPC. GetSource_FromAI(), NPC.Center, velocity,
                ModContent.ProjectileType<JungleNeedle>(),
                95, 0f);
        }
    }
}

private void FireMirageJungleNeedles()
{
    for (int i = 0; i < mirageIndices. Count; i++)
    {
        int index = mirageIndices[i];
        if (index != -1 && Main.npc[index].active)
        {
            var mirage = Main.npc[index].ModNPC as AegonMirage;
            mirage?.FireJungleNeedleSpread(arenaCenter, true);
        }
    }
}

private void FireCrimruptionBolts()
{
    int spreadCount = 3;
    float baseAngle = (arenaCenter - NPC.Center).ToRotation();

    for (int i = 0; i < spreadCount; i++)
    {
        float offsetAngle = MathHelper.Lerp(-0.2f, 0.2f, i / (float)(spreadCount - 1));
        float angle = baseAngle + offsetAngle;

        Vector2 velocity = new Vector2(
            (float)Math.Cos(angle),
            (float)Math.Sin(angle)
        ) * 5f;

        if (Main. netMode != NetmodeID. MultiplayerClient)
        {
            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                ModContent. ProjectileType<CrimruptionBolt>(),
                120, 0f);
        }
    }
}

private void FireMirageCrimruptionBolts()
{
    for (int i = 0; i < mirageIndices.Count; i++)
    {
        int index = mirageIndices[i];
        if (index != -1 && Main.npc[index].active)
        {
            var mirage = Main.npc[index].ModNPC as AegonMirage;
            mirage?.FireCrimruptionBolts(arenaCenter, true);
        }
    }
}

private void FireOceanSphere()
{
    float angle = (arenaCenter - NPC.Center).ToRotation();

    Vector2 velocity = new Vector2(
        (float)Math.Cos(angle),
        (float)Math.Sin(angle)
    ) * 6f;

    if (Main.netMode != NetmodeID.MultiplayerClient)
    {
        Projectile. NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
            ModContent.ProjectileType<OceanSphere>(),
            120, 0f);
    }
}

private void FireMirageOceanSpheres()
{
    for (int i = 0; i < mirageIndices.Count; i++)
    {
        int index = mirageIndices[i];
        if (index != -1 && Main.npc[index].active)
        {
            var mirage = Main.npc[index].ModNPC as AegonMirage;
            mirage?. FireOceanSphere(arenaCenter, true);
        }
    }
}

private void FireHallowedSpearsAtPlayer()
{
    Player target = Main.player[NPC.target];
    int spreadCount = 3;
    float baseAngle = (target.Center - NPC.Center).ToRotation();

    for (int i = 0; i < spreadCount; i++)
    {
        float offsetAngle = MathHelper. Lerp(-0.35f, 0.35f, i / (float)(spreadCount - 1));
        float angle = baseAngle + offsetAngle;

        Vector2 velocity = new Vector2(
            (float)Math.Cos(angle),
            (float)Math.Sin(angle)
        ) * 4f;

        if (Main.netMode != NetmodeID.MultiplayerClient)
        {
            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                ModContent.ProjectileType<HallowedSpear>(),
                120, 0f);
        }
    }
}

private void FireMirageHallowedSpears()
{
    Player target = Main.player[NPC.target];

    for (int i = 0; i < mirageIndices.Count; i++)
    {
        int index = mirageIndices[i];
        if (index != -1 && Main.npc[index].active)
        {
            var mirage = Main.npc[index]. ModNPC as AegonMirage;
            mirage?. FireHallowedSpears(target. Center, true);
        }
    }
}

private void DisablePlayerHealing(Player player)
{
    player.lifeRegen = -999;
    player.lifeRegenTime = 0;
    player.potionDelay = int.MaxValue;
}

private void EnablePlayerHealing(Player player)
{
    player.potionDelay = 0;
}

        // ==================== FIGHT END ====================
        
        public void EndFight()
        {
            NPC.velocity = Vector2.Zero;
            NPC.  dontTakeDamage = true;

            for (int i = 0; i < 150; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(15f, 15f);
                int dust = Dust.NewDust(arenaCenter, 100, 100, DustID. Stone, velocity.X, velocity.Y, 100, Color.White, 3f);
                Main.dust[dust].noGravity = true;
            }

            NPC.active = false;

            Arena?. Remove();

            DespawnMirages();

            if (Main.netMode != NetmodeID.Server)
            {
                Main.NewText("Not bad, kid.", 50, 125, 255);
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
                return baseDamage * 3;
            if (Main.expertMode)
                return baseDamage * 2;
            return baseDamage;
        }

        private void DespawnMirages()
        {
            for (int i = 0; i < mirageIndices.Count; i++)
            {
                int index = mirageIndices[i];
                if (index != -1 && index < Main.maxNPCs && Main.npc[index].active)
                {
                    Main.npc[index].active = false;
                }
            }
            mirageIndices.  Clear();
        }

        private void EnforceArenaConfinement(Player player)
        {
            if (Arena == null || ! Arena.IsActive) return;

            if (Arena.IsOutsideArena(player.Center))
            {
                player.Center = Arena.ClampToArena(player.Center);
                player.velocity = Vector2.Zero;

                if (Main.rand.NextBool(20))
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
            if (CurrentPhase == Phase.Phase3_WithShield)
            {
                modifiers.FinalDamage *= 0.5f;
            }
        }

        public override void ModifyHitByItem(Player player, Item item, ref NPC.HitModifiers modifiers)
        {
            ApplySoftcap(ref modifiers);
        }

        public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            ApplySoftcap(ref modifiers);
        }

        private void ApplySoftcap(ref NPC.HitModifiers modifiers)
        {
            int estimatedDamage = (int)modifiers.FinalDamage. Base;
            int maxDamageBeforeCap = (int)(NPC.lifeMax * 0.12f);

            if (estimatedDamage > maxDamageBeforeCap)
            {
                int excessDamage = estimatedDamage - maxDamageBeforeCap;
                int reducedExcess = (int)(excessDamage * 0.05f);
                int finalDamage = maxDamageBeforeCap + reducedExcess;

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

            Arena?.Remove();

            if (worldAegisIndex != -1 && Main.npc[worldAegisIndex].active)
            {
                Main.npc[worldAegisIndex].active = false;
            }

            DespawnMirages();
        }

        public override bool CheckActive()
        {
            return false;
        }

        // ==================== LOOT ====================

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.FragmentSolar, 1, 20, 40));
        }
    }
}