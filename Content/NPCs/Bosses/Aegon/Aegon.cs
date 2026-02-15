// Content/NPCs/Bosses/Aegon/Aegon.cs

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Projectiles.EnemyProjectiles;

namespace MightofUniverses.Content.NPCs.Bosses.Aegon
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
            Phase4_SigilCombat = 3,
            Phase5_FreeMovement = 4,
            Phase6_Patience = 5
        }

        private Phase CurrentPhase
        {
            get => (Phase)NPC.ai[0];
            set => NPC.ai[0] = (float)value;
        }

        // ==================== AI TIMERS ====================
        private ref float AttackTimer => ref NPC.ai[1];
        private ref float AttackState => ref NPC.ai[2];
        private ref float SubTimer => ref NPC.ai[3];

        // ==================== ARENA ====================
        private Vector2 arenaCenter;
        private float arenaRadius;

        private const float ARENA_RADIUS_NORMAL = 75.5f;
        private const float ARENA_RADIUS_EXPERT = 62.5f;
        private const float ARENA_RADIUS_MASTER = 50.5f;

        // ==================== REFERENCES ====================
        private int worldAegisIndex = -1;
        private List<int> sigilIndices = new List<int> { -1, -1, -1, -1, -1 };
        private int currentSigilAttackIndex = 0;

        // ==================== PHASE 2/3 DATA ====================
        private int currentAttack = -1;
        private int phase3PositionIndex = 0;

        // ==================== PHASE 6 DATA ====================
        private float patienceTimer = 0f;
        private bool patienceActive = false;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 1;
            NPCID.Sets.MPAllowedEnemies[Type] = true;
            NPCID.Sets.BossBestiaryPriority.Add(Type);

            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                CustomTexturePath = "MightofUniverses/Content/NPCs/Bosses/Aegon/Aegon",
                PortraitScale = 0.6f,
                PortraitPositionYOverride = 0f,
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
        }

        public override void SetDefaults()
        {
            NPC.width = 120;
            NPC.height = 120;
            NPC.defense = 100;
            NPC.lifeMax = 235000;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.value = Item.buyPrice(gold: 50);
            NPC.SpawnWithHigherTime(30);
            NPC.boss = true;
            NPC.npcSlots = 15f;
            NPC.aiStyle = -1;
            NPC.netAlways = true;
            NPC.scale = 1.5f;

            if (Main.expertMode)
            {
                NPC.lifeMax = 350000;
                NPC.defense = 125;
            }

            if (Main.masterMode)
            {
                NPC.lifeMax = 470000;
                NPC.defense = 150;
            }

            Music = MusicID.Boss2;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                new FlavorTextBestiaryInfoElement(
                    "Aegon, the eternal guardian born from the world's core. " +
                    "His legendary World Aegis has shielded countless civilizations from annihilation. " +
                    "Until now, nothing has been able to even chip the aegis."
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
                Main.NewText("The World Aegis has been summoned!", 175, 75, 255);
            }

            CurrentPhase = Phase.Phase1_Immune;
            NPC.dontTakeDamage = true;

            // Create arena
            AegonArena.Current = new AegonArena(arenaCenter);
            AegonArena.Current.Create();

            SpawnWorldAegis();
            SpawnSigils();
        }

        public override void AI()
        {
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest();
            }

            Player target = Main.player[NPC.target];

            // ==================== DESPAWN WHEN PLAYER DIES ====================
            if (!target.active || target.dead)
            {
                NPC.life = 0;
                NPC.active = false;

                if (worldAegisIndex != -1 && Main.npc[worldAegisIndex].active)
                {
                    Main.npc[worldAegisIndex].active = false;
                }

                DespawnSigils();
                
                if (AegonArena.Current != null)
                {
                    AegonArena.Current.Remove();
                }

                return;
            }
            else if (CurrentPhase != Phase.Phase1_Immune)
            {
                NPC.dontTakeDamage = false;
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

                case Phase.Phase4_SigilCombat:
                    Phase4_SigilAttacks();
                    break;

                case Phase.Phase5_FreeMovement:
                    Phase5_FreeMovementAttacks();
                    break;

                case Phase.Phase6_Patience:
                    Phase6_PatienceMechanic();
                    break;
            }

            AttackTimer++;
        }

        // ==================== PHASE 1: IMMUNE & CIRCLING ====================
        private void Phase1_CircleArena()
        {
            float circleSpeed = 0.02f;
            float orbitAngle = AttackTimer * circleSpeed;

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

                if (aegisHPPercent <= 0.9f && AttackTimer % 120 == 0)
                {
                    FireHallowedSpearSingle();
                }

                if (aegisHPPercent <= 0.7f && AttackTimer % 120 == 0)
                {
                    FireHallowedSpearDouble();
                }
            }
        }

        private void FireHallowedSpearSingle()
        {
            Player target = Main.player[NPC.target];
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
                float spread = MathHelper.Lerp(-0.22f, 0.22f, i / 1f);
                float angle = baseAngle + spread;

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

        // ==================== TRANSITION: PHASE 1 → PHASE 2 ====================
        private void TransitionToPhase2()
        {
            CurrentPhase = Phase.Phase2_FirstAttacks;
            NPC.dontTakeDamage = false;
            NPC.rotation = 0f;
            AttackTimer = 0;
            currentAttack = -1;

            if (worldAegisIndex != -1 && Main.npc[worldAegisIndex].active)
            {
                var aegis = Main.npc[worldAegisIndex].ModNPC as WorldAegis;
                aegis?.TransitionToPhase2AndFlyAway();
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
                currentAttack = Main.rand.Next(4);
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
                case 3:
                    Phase2_AttackD();
                    break;
            }

            if (NPC.life <= NPC.lifeMax * 0.5f)
            {
                TransitionToPhase3();
            }
        }

        private void Phase2_AttackA()
        {
            Player target = Main.player[NPC.target];

            if (SubTimer == 0 || SubTimer == 30 || SubTimer == 60 || SubTimer == 90 || SubTimer == 120)
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
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                            ModContent.ProjectileType<HallowedSpear>(),
                            120, 0f);
                    }
                }
            }

            SubTimer++;

            Vector2 direction = (target.Center - NPC.Center).SafeNormalize(Vector2.Zero);
            NPC.velocity = direction * 3f;

            if (SubTimer >= 150)
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
                    float angle = i / 12f * MathHelper.TwoPi;

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

            if (SubTimer >= 60 && SubTimer <= 180)
            {
                if (SubTimer % 30 == 0)
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

            if (SubTimer >= 210)
            {
                currentAttack = -1;
                SubTimer = 0;
            }
        }

        private void Phase2_AttackC()
        {
            Player target = Main.player[NPC.target];

            if (SubTimer == 0 || SubTimer == 60 || SubTimer == 120)
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
                            ModContent.ProjectileType<OceanSphere>(),
                            120, 0f);
                    }
                }

                for (int i = 0; i < 16; i++)
                {
                    float angle = i / 16f * MathHelper.TwoPi;
                    Vector2 velocity = new Vector2(
                        (float)Math.Cos(angle),
                        (float)Math.Sin(angle)
                    ) * 5f;

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                            ModContent.ProjectileType<CrimruptionBolt>(),
                            120, 0f);
                    }
                }
            }

            SubTimer++;

            Vector2 direction = (target.Center - NPC.Center).SafeNormalize(Vector2.Zero);
            NPC.velocity = direction * 6f;

            if (SubTimer >= 150)
            {
                currentAttack = -1;
                SubTimer = 0;
            }
        }

        private void Phase2_AttackD()
        {
            Player target = Main.player[NPC.target];

            if (SubTimer < 300)
            {
                if (SubTimer % 12 == 0)
                {
                    float angle = SubTimer * 0.2f;
                    for (int i = 0; i < 4; i++)
                    {
                        float spiralAngle = angle + (i * MathHelper.PiOver2);
                        Vector2 velocity = new Vector2(
                            (float)Math.Cos(spiralAngle),
                            (float)Math.Sin(spiralAngle)
                        ) * 5f;

                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                                ModContent.ProjectileType<CrimruptionBolt>(),
                                120, 0f);
                        }
                    }
                }

                if (SubTimer % 60 == 0)
                {
                    for (int i = 0; i < 16; i++)
                    {
                        float angle = i / 16f * MathHelper.TwoPi;
                        Vector2 velocity = new Vector2(
                            (float)Math.Cos(angle),
                            (float)Math.Sin(angle)
                        ) * 5f;

                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                                ModContent.ProjectileType<ForestLeaf>(),
                                100, 0f);
                        }
                    }
                }
            }

            SubTimer++;
            NPC.velocity *= 0.98f;

            if (SubTimer >= 300)
            {
                currentAttack = -1;
                SubTimer = 0;
            }
        }
                // ==================== TRANSITION: PHASE 2 → PHASE 3 ====================
        private void TransitionToPhase3()
        {
            CurrentPhase = Phase.Phase3_WithShield;
            NPC.rotation = 0f;
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

            if (worldAegisIndex == -1 || !Main.npc[worldAegisIndex].active)
            {
                TransitionToPhase4();
                return;
            }

            var aegis = Main.npc[worldAegisIndex].ModNPC as WorldAegis;
            if (aegis != null && aegis.IsInScatterFragmentsPhase())
            {
                TransitionToPhase4();
                return;
            }

            if (currentAttack == -1)
            {
                if (AttackTimer >= 120)
                {
                    currentAttack = Main.rand.Next(4);
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
                    case 3:
                        Phase3_AttackD();
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

            NPC.Center = targetPos;
            NPC.velocity = Vector2.Zero;

            for (int i = 0; i < 30; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(8f, 8f);
                int dust = Dust.NewDust(NPC.Center, NPC.width, NPC.height,
                    DustID.GoldFlame, velocity.X, velocity.Y, 100, Color.Gold, 2f);
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
                    Main.dust[dust].noGravity = true;
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
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                            ModContent.ProjectileType<UnderworldFireball>(),
                            110, 0f);
                    }
                }

                if (worldAegisIndex != -1 && Main.npc[worldAegisIndex].active)
                {
                    var aegis = Main.npc[worldAegisIndex].ModNPC as WorldAegis;
                    aegis?.FireWorldAegisFireball();
                }

                if (worldAegisIndex != -1 && Main.npc[worldAegisIndex].active)
                {
                    NPC worldAegis = Main.npc[worldAegisIndex];
                    for (int i = 0; i < 8; i++)
                    {
                        float angle = i / 8f * MathHelper.TwoPi;
                        Vector2 velocity = new Vector2(
                            (float)Math.Cos(angle),
                            (float)Math.Sin(angle)
                        ) * 5f;

                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), worldAegis.Center, velocity,
                                ModContent.ProjectileType<AegisChunk>(),
                                130, 0f);
                        }
                    }
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
                    Color telegraphColor = Main.rand.NextBool() ? Color.Orange : Color.Cyan;
                    int dustType = Main.rand.NextBool() ? DustID.Torch : DustID.Ice;
                    int dust = Dust.NewDust(NPC.Center, NPC.width, NPC.height,
                        dustType, 0f, 0f, 100, telegraphColor, 2.5f);
                    Main.dust[dust].noGravity = true;
                }
            }

            if (SubTimer % 30 == 0 && SubTimer < 180)
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
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                            ModContent.ProjectileType<CrimruptionBolt>(),
                            120, 0f);
                    }
                }
            }

            if (SubTimer == 150)
            {
                for (int i = 0; i < 16; i++)
                {
                    float angle = i / 16f * MathHelper.TwoPi;

                    Vector2 velocity = new Vector2(
                        (float)Math.Cos(angle),
                        (float)Math.Sin(angle)
                    ) * 4f;

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                            ModContent.ProjectileType<AegisFragment>(),
                            125, 0f);
                    }
                }

                if (worldAegisIndex != -1 && Main.npc[worldAegisIndex].active)
                {
                    NPC worldAegis = Main.npc[worldAegisIndex];
                    for (int i = 0; i < 32; i++)
                    {
                        float angle = i / 32f * MathHelper.TwoPi;
                        Vector2 velocity = new Vector2(
                            (float)Math.Cos(angle),
                            (float)Math.Sin(angle)
                        ) * 5f;

                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), worldAegis.Center, velocity,
                                ModContent.ProjectileType<AegisShard>(),
                                120, 0f);
                        }
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
            Player target = Main.player[NPC.target];

            if (SubTimer == 0)
            {
                for (int i = 0; i < 40; i++)
                {
                    int dust = Dust.NewDust(NPC.Center, NPC.width, NPC.height,
                        DustID.Water, 0f, 0f, 100, Color.Blue, 2.5f);
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
                aegis?.FireWaterSpiral();
            }

            SubTimer++;

            if (SubTimer >= 300)
            {
                currentAttack = -1;
                SubTimer = 0;
            }
        }

        private void Phase3_AttackD()
        {
            Player target = Main.player[NPC.target];

            if (SubTimer == 0)
            {
                for (int i = 0; i < 12; i++)
                {
                    float angle = i / 12f * MathHelper.TwoPi;
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

                if (worldAegisIndex != -1 && Main.npc[worldAegisIndex].active)
                {
                    NPC worldAegis = Main.npc[worldAegisIndex];
                    for (int i = 0; i < 32; i++)
                    {
                        float angle = i / 32f * MathHelper.TwoPi;
                        Vector2 velocity = new Vector2(
                            (float)Math.Cos(angle),
                            (float)Math.Sin(angle)
                        ) * 5f;

                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), worldAegis.Center, velocity,
                                ModContent.ProjectileType<AegisShard>(),
                                120, 0f);
                        }
                    }
                }
            }

            SubTimer++;

            if (SubTimer >= 90)
            {
                currentAttack = -1;
                SubTimer = 0;
            }
        }

        // ==================== TRANSITION: PHASE 3 → PHASE 4 ====================
        private void TransitionToPhase4()
        {
            CurrentPhase = Phase.Phase4_SigilCombat;
            NPC.rotation = 0f;
            AttackTimer = 0;
            currentAttack = -1;
            SubTimer = 0;

            if (worldAegisIndex != -1 && Main.npc[worldAegisIndex].active)
            {
                var aegis = Main.npc[worldAegisIndex].ModNPC as WorldAegis;
                aegis?.TransitionToPhase4AndScatterFragments();
            }

            NPC.velocity = Vector2.Zero;

            for (int i = 0; i < 60; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(8f, 8f);
                int dust = Dust.NewDust(NPC.Center, NPC.width, NPC.height, DustID.Stone, velocity.X, velocity.Y, 100, Color.Red, 2.5f);
                Main.dust[dust].noGravity = true;
            }

            if (Main.netMode != NetmodeID.Server)
            {
                Main.NewText("Aegon summons the Sigils of the World!", Color.Cyan);
            }
        }

        // ==================== PHASE 4: SIGIL COMBAT ====================
        private void Phase4_SigilAttacks()
        {
            NPC.damage = 0;
            NPC.rotation = 0f;

            float hpPercent = NPC.life / (float)NPC.lifeMax;

            if (hpPercent <= 0.25f)
            {
                TransitionToPhase5();
                return;
            }

            Player target = Main.player[NPC.target];
            Vector2 desiredPosition = target.Center + new Vector2(40 * 16f, 0);
            Vector2 direction = desiredPosition - NPC.Center;
            float distance = direction.Length();

            if (distance > 10f)
            {
                direction.Normalize();
                NPC.velocity = direction * Math.Min(distance * 0.1f, 8f);
            }
            else
            {
                NPC.velocity *= 0.9f;
            }

            if (AttackTimer % 300 == 0)
            {
                TriggerNextSigilAttack();
            }

            if (currentAttack == -1)
            {
                currentAttack = Main.rand.Next(5);
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

        private void Phase4_AttackA()
        {
            if (SubTimer % 75 == 0 && SubTimer < 300)
            {
                for (int i = 0; i < 5; i++)
                {
                    float angle = i / 5f * MathHelper.TwoPi;
                    Vector2 velocity = new Vector2(
                        (float)Math.Cos(angle),
                        (float)Math.Sin(angle)
                    ) * 6f;

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                            ModContent.ProjectileType<AegisFragment>(),
                            125, 0f);
                        if (proj >= 0 && proj < Main.maxProjectiles)
                        {
                            Main.projectile[proj].ai[0] = 1f;
                        }
                    }
                }
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

            if (SubTimer % 12 == 0 && SubTimer < 300)
            {
                Vector2 spawnPos = target.Center + new Vector2(Main.rand.NextFloat(-400f, 400f), -600f);

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), spawnPos, new Vector2(0, 10f),
                        ModContent.ProjectileType<AegisShard>(),
                        120, 0f);
                }
            }

            if (SubTimer % 120 == 0 && SubTimer < 300)
            {
                Vector2 velocity = (target.Center - NPC.Center).SafeNormalize(Vector2.UnitX) * 7f;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                        ModContent.ProjectileType<AegisFragment>(),
                        125, 0f);
                }
            }

            SubTimer++;
            if (SubTimer >= 300)
            {
                currentAttack = -1;
                SubTimer = 0;
            }
        }

        private void Phase4_AttackD()
        {
            if (SubTimer == 0)
            {
                for (int i = 0; i < 24; i++)
                {
                    float angleSpread = MathHelper.Lerp(-0.3f, 0.3f, i / 23f);
                    Vector2 velocity = new Vector2(-8f, 0).RotatedBy(angleSpread);

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                            ModContent.ProjectileType<AegisShard>(),
                            120, 0f);
                        if (proj >= 0 && proj < Main.maxProjectiles)
                        {
                            Main.projectile[proj].ai[0] = 1f;
                        }
                    }
                }
            }

            if (SubTimer == 180)
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

            SubTimer++;
            if (SubTimer >= 240)
            {
                currentAttack = -1;
                SubTimer = 0;
            }
        }

        private void Phase4_AttackE()
        {
            Player target = Main.player[NPC.target];

            if (SubTimer % 20 == 0 && SubTimer < 180)
            {
                Vector2 spawnPos = target.Center + new Vector2(Main.rand.NextFloat(-400f, 400f), -600f);

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), spawnPos, new Vector2(0, 8f),
                        ModContent.ProjectileType<SkySpark>(),
                        100, 0f);
                }
            }

            if (SubTimer >= 60 && SubTimer < 180)
            {
                if ((SubTimer - 60) % 10 == 0)
                {
                    int index = (int)((SubTimer - 60) / 10);
                    float angle = index / 12f * MathHelper.TwoPi;

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

            SubTimer++;
            if (SubTimer >= 240)
            {
                currentAttack = -1;
                SubTimer = 0;
            }
        }
                // ==================== TRANSITION: PHASE 4 → PHASE 5 ====================
        private void TransitionToPhase5()
        {
            CurrentPhase = Phase.Phase5_FreeMovement;
            NPC.rotation = 0f;
            AttackTimer = 0;
            currentAttack = -1;
            SubTimer = 0;

            if (AegonArena.Current != null)
            {
                AegonArena.Current.Remove();
                if (Main.netMode != NetmodeID.Server)
                {
                    Main.NewText("The arena shatters! You are free to move!", Color.Gold);
                }
            }

            for (int i = 0; i < 100; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(10f, 10f);
                int dust = Dust.NewDust(arenaCenter, 100, 100, DustID.Stone, velocity.X, velocity.Y, 100, Color.Orange, 3f);
                Main.dust[dust].noGravity = true;
            }
        }

        // ==================== PHASE 5: FREE MOVEMENT ====================
        private void Phase5_FreeMovementAttacks()
        {
            NPC.damage = 0;
            NPC.rotation = 0f;

            float hpPercent = NPC.life / (float)NPC.lifeMax;

            if (hpPercent <= 0.01f)
            {
                TransitionToPhase6();
                return;
            }

            Player target = Main.player[NPC.target];
            Vector2 desiredPosition = target.Center + new Vector2(Main.rand.NextFloat(-40f, 40f) * 16f, -40 * 16f);
            Vector2 direction = desiredPosition - NPC.Center;
            float distance = direction.Length();

            if (distance > 10f)
            {
                direction.Normalize();
                NPC.velocity = direction * Math.Min(distance * 0.08f, 10f);
            }
            else
            {
                NPC.velocity *= 0.95f;
            }

            if (AttackTimer % 180 == 0)
            {
                TriggerNextSigilAttack();
            }

            if (currentAttack == -1)
            {
                currentAttack = Main.rand.Next(4);
                SubTimer = 0;
            }

            switch (currentAttack)
            {
                case 0: Phase5_AttackA(); break;
                case 1: Phase5_AttackB(); break;
                case 2: Phase5_AttackC(); break;
                case 3: Phase5_AttackD(); break;
            }
        }

        private void Phase5_AttackA()
        {
            Player target = Main.player[NPC.target];

            if (SubTimer == 0)
            {
                for (int i = 0; i < 12; i++)
                {
                    float angle = i / 12f * MathHelper.TwoPi;
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

            if (SubTimer % 30 == 0 && SubTimer < 180)
            {
                Vector2 velocity = (target.Center - NPC.Center).SafeNormalize(Vector2.UnitX) * 8f;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                        ModContent.ProjectileType<AegisShard>(),
                        120, 0f);
                }
            }

            SubTimer++;
            if (SubTimer >= 180)
            {
                currentAttack = -1;
                SubTimer = 0;
            }
        }

        private void Phase5_AttackB()
        {
            NPC.velocity *= 0.5f;

            if (SubTimer % 12 == 0 && SubTimer < 600)
            {
                float angle = SubTimer * 0.15f;

                for (int i = 0; i < 2; i++)
                {
                    float spiralAngle = angle + (i * MathHelper.Pi);
                    Vector2 velocity = new Vector2(
                        (float)Math.Cos(spiralAngle),
                        (float)Math.Sin(spiralAngle)
                    ) * 5f;

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                            ModContent.ProjectileType<SnowSpike>(),
                            110, 0f);
                    }
                }
            }

            if (SubTimer % 180 == 0 && SubTimer < 600)
            {
                for (int i = 0; i < 32; i++)
                {
                    float angle = i / 32f * MathHelper.TwoPi;
                    Vector2 velocity = new Vector2(
                        (float)Math.Cos(angle),
                        (float)Math.Sin(angle)
                    ) * 8f;

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                            ModContent.ProjectileType<AegisShard>(),
                            120, 0f);
                    }
                }
            }

            SubTimer++;
            if (SubTimer >= 600)
            {
                currentAttack = -1;
                SubTimer = 0;
            }
        }

        private void Phase5_AttackC()
        {
            Player target = Main.player[NPC.target];

            if (SubTimer % 15 == 0 && SubTimer < 180)
            {
                float baseAngle = (target.Center - NPC.Center).ToRotation();
                float spread = Main.rand.NextFloat(-0.13f, 0.13f);

                Vector2 velocity = new Vector2(
                    (float)Math.Cos(baseAngle + spread),
                    (float)Math.Sin(baseAngle + spread)
                ) * 10f;

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                        ModContent.ProjectileType<AegisChunk>(),
                        130, 0f);
                }
            }

            if (SubTimer == 120 || SubTimer == 150)
            {
                for (int i = 0; i < 3; i++)
                {
                    float baseAngle = (target.Center - NPC.Center).ToRotation();
                    float spread = MathHelper.Lerp(-0.2f, 0.2f, i / 2f);
                    float angle = baseAngle + spread;

                    Vector2 velocity = new Vector2(
                        (float)Math.Cos(angle),
                        (float)Math.Sin(angle)
                    ) * 7f;

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                            ModContent.ProjectileType<UnderworldFireball>(),
                            110, 0f);
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

        private void Phase5_AttackD()
        {
            Player target = Main.player[NPC.target];

            if (SubTimer % 20 == 0 && SubTimer < 300)
            {
                Vector2 velocity = (target.Center - NPC.Center).SafeNormalize(Vector2.UnitX) * 6f;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                        ModContent.ProjectileType<OceanSphere>(),
                        120, 0f);
                }
            }

            if (SubTimer % 9 == 0 && SubTimer < 300)
            {
                Vector2 spawnPos = target.Center + new Vector2(Main.rand.NextFloat(-500f, 500f), -700f);

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), spawnPos, new Vector2(0, 10f),
                        ModContent.ProjectileType<SkySpark>(),
                        100, 0f);
                }
            }

            SubTimer++;
            if (SubTimer >= 300)
            {
                currentAttack = -1;
                SubTimer = 0;
            }
        }

        // ==================== TRANSITION: PHASE 5 → PHASE 6 ====================
        private void TransitionToPhase6()
        {
            CurrentPhase = Phase.Phase6_Patience;
            NPC.life = (int)(NPC.lifeMax * 0.01f);
            NPC.dontTakeDamage = true;
            NPC.rotation = 0f;
            NPC.velocity = Vector2.Zero;
            AttackTimer = 0;
            patienceTimer = 0;
            patienceActive = true;

            Player target = Main.player[NPC.target];

            if (worldAegisIndex != -1 && Main.npc[worldAegisIndex].active)
            {
                NPC worldAegis = Main.npc[worldAegisIndex];
                worldAegis.Center = target.Center + new Vector2(50 * 16f, 0);
                worldAegis.velocity = Vector2.Zero;
                worldAegis.active = true;

                var aegis = worldAegis.ModNPC as WorldAegis;
                aegis?.ReturnForPhase5(target.Center);
            }

            foreach (int sigilIndex in sigilIndices)
            {
                if (sigilIndex >= 0 && sigilIndex < Main.maxNPCs && Main.npc[sigilIndex].active)
                {
                }
            }

            for (int i = 0; i < 100; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(12f, 12f);
                int dust = Dust.NewDust(target.Center, 100, 100, DustID.Stone, velocity.X, velocity.Y, 100, Color.DarkRed, 3f);
                Main.dust[dust].noGravity = true;
            }

            if (Main.netMode != NetmodeID.Server)
            {
                Main.NewText("You have strength, but do you have patience?", Color.Red);
                Main.NewText("30 seconds remaining...", Color.Yellow);
            }
        }

        // ==================== PHASE 6: PATIENCE ====================
        private void Phase6_PatienceMechanic()
        {
            NPC.damage = 0;
            NPC.rotation = 0f;
            NPC.dontTakeDamage = true;

            Player target = Main.player[NPC.target];

            float orbitRadius = 50f * 16f;
            float orbitSpeed = 0.02f;
            float angle = patienceTimer * orbitSpeed;

            NPC.Center = target.Center + new Vector2(
                (float)Math.Cos(angle) * orbitRadius,
                (float)Math.Sin(angle) * orbitRadius
            );

            NPC.rotation = 0f;

            if (worldAegisIndex != -1 && Main.npc[worldAegisIndex].active)
            {
                NPC worldAegis = Main.npc[worldAegisIndex];
                worldAegis.Center = target.Center + new Vector2(
                    (float)Math.Cos(-angle) * orbitRadius,
                    (float)Math.Sin(-angle) * orbitRadius
                );
                worldAegis.rotation = 0f;
            }

            DisablePlayerHealing(target);

            patienceTimer++;

            if (patienceTimer % 60 == 0)
            {
                int secondsLeft = (1800 - (int)patienceTimer) / 60;
                if (secondsLeft == 20 || secondsLeft == 10 || secondsLeft == 0)
                {
                    if (Main.netMode != NetmodeID.Server)
                    {
                        if (secondsLeft > 0)
                            Main.NewText($"{secondsLeft} seconds remaining", Color.Yellow);
                        else
                            Main.NewText("Patience complete!", Color.Gold);
                    }
                }
            }

            if (patienceTimer >= 0 && patienceTimer % 20 == 0)
            {
                Vector2 velocityAegon = (target.Center - NPC.Center).SafeNormalize(Vector2.UnitX) * 6f;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocityAegon,
                        ModContent.ProjectileType<AegisShard>(),
                        120, 0f);
                }

                if (worldAegisIndex != -1 && Main.npc[worldAegisIndex].active)
                {
                    NPC worldAegis = Main.npc[worldAegisIndex];
                    Vector2 velocityAegis = (target.Center - worldAegis.Center).SafeNormalize(Vector2.UnitX) * 6f;
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), worldAegis.Center, velocityAegis,
                            ModContent.ProjectileType<AegisShard>(),
                            120, 0f);
                    }
                }
            }

            if (patienceTimer >= 300)
            {
                if ((patienceTimer - 300) % 120 == 0)
                {
                    foreach (int sigilIndex in sigilIndices)
                    {
                        if (sigilIndex >= 0 && sigilIndex < Main.maxNPCs && Main.npc[sigilIndex].active)
                        {
                            var sigil = Main.npc[sigilIndex].ModNPC as AegonSigilBase;
                            sigil?.StartTelegraph();
                        }
                    }
                }
            }

            if (patienceTimer >= 600 && patienceTimer % 20 == 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    float xPos = target.Center.X + Main.rand.NextFloat(-600f, 600f);
                    float yPos = target.Center.Y - 800f;
                    Vector2 spawnPos = new Vector2(xPos, yPos);
                    Vector2 velocity = new Vector2(0, 8f);

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), spawnPos, velocity,
                            ModContent.ProjectileType<SkySpark>(),
                            100, 0f);
                    }
                }
            }

            if (patienceTimer >= 900 && patienceTimer % 10 == 0)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero,
                        ModContent.ProjectileType<AegisChunk>(),
                        130, 0f);
                }

                if (worldAegisIndex != -1 && Main.npc[worldAegisIndex].active)
                {
                    NPC worldAegis = Main.npc[worldAegisIndex];
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), worldAegis.Center, Vector2.Zero,
                            ModContent.ProjectileType<AegisChunk>(),
                            130, 0f);
                    }
                }
            }

            if (patienceTimer >= 1200 && (patienceTimer - 1200) % 120 == 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    float baseAngle = (target.Center - NPC.Center).ToRotation();
                    float spread = MathHelper.Lerp(-0.3f, 0.3f, i / 2f);
                    float angle2 = baseAngle + spread;

                    Vector2 velocity = new Vector2(
                        (float)Math.Cos(angle2),
                        (float)Math.Sin(angle2)
                    ) * 5f;

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                            ModContent.ProjectileType<HallowedSpear>(),
                            120, 0f);
                    }
                }

                if (worldAegisIndex != -1 && Main.npc[worldAegisIndex].active)
                {
                    NPC worldAegis = Main.npc[worldAegisIndex];
                    for (int i = 0; i < 3; i++)
                    {
                        float baseAngle = (target.Center - worldAegis.Center).ToRotation();
                        float spread = MathHelper.Lerp(-0.3f, 0.3f, i / 2f);
                        float angle2 = baseAngle + spread;

                        Vector2 velocity = new Vector2(
                            (float)Math.Cos(angle2),
                            (float)Math.Sin(angle2)
                        ) * 5f;

                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), worldAegis.Center, velocity,
                                ModContent.ProjectileType<HallowedSpear>(),
                                120, 0f);
                        }
                    }
                }
            }

            if (patienceTimer >= 1800)
            {
                patienceActive = false;
                EnablePlayerHealing(target);

                NPC.velocity = Vector2.Zero;

                if (worldAegisIndex != -1 && Main.npc[worldAegisIndex].active)
                {
                    NPC worldAegis = Main.npc[worldAegisIndex];
                    worldAegis.velocity = Vector2.Zero;
                    worldAegis.Center = target.Center;

                    var aegis = worldAegis.ModNPC as WorldAegis;
                    aegis?.MakeVulnerable();
                }

                DespawnSigils();

                if (Main.netMode != NetmodeID.Server)
                {
                    Main.NewText("The World Aegis is now vulnerable! Destroy it to end the fight!", Color.Gold);
                }

                patienceTimer = 1800;
            }

            if (patienceTimer >= 1800)
            {
                PostPatienceAttacks();
            }
        }

        private void PostPatienceAttacks()
        {
            Player target = Main.player[NPC.target];

            Vector2 direction = (target.Center - NPC.Center).SafeNormalize(Vector2.Zero);
            NPC.velocity = direction * 6f;

            if (AttackTimer % 180 == 0)
            {
                for (int i = 0; i < 6; i++)
                {
                    float angle = i / 6f * MathHelper.TwoPi;
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

        private void SpawnSigils()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                sigilIndices[0] = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<AegonHallowedSigil>());
                sigilIndices[1] = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<AegonHellSigil>());
                sigilIndices[2] = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<AegonEvilSigil>());
                sigilIndices[3] = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<AegonDesertSigil>());
                sigilIndices[4] = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<AegonJungleSigil>());

                if (Main.netMode == NetmodeID.Server)
                {
                    foreach (int index in sigilIndices)
                    {
                        NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, index);
                    }
                }
            }
        }

        private void TriggerNextSigilAttack()
        {
            int index = sigilIndices[currentSigilAttackIndex];
            if (index >= 0 && index < Main.maxNPCs && Main.npc[index].active)
            {
                var sigil = Main.npc[index].ModNPC as AegonSigilBase;
                if (sigil != null && sigil.IsReadyToAttack())
                {
                    sigil.StartTelegraph();
                }
            }

            currentSigilAttackIndex = (currentSigilAttackIndex + 1) % 5;
        }

        private void DespawnSigils()
        {
            foreach (int sigilIndex in sigilIndices)
            {
                if (sigilIndex >= 0 && sigilIndex < Main.maxNPCs && Main.npc[sigilIndex].active)
                {
                    Main.npc[sigilIndex].active = false;
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

        public int GetCurrentPhase()
        {
            return (int)CurrentPhase;
        }

        public bool IsPatienceActive()
        {
            return CurrentPhase == Phase.Phase6_Patience && patienceActive;
        }

        public void EndFight()
        {
            NPC.velocity = Vector2.Zero;
            NPC.dontTakeDamage = true;

            for (int i = 0; i < 150; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(15f, 15f);
                int dust = Dust.NewDust(arenaCenter, 100, 100, DustID.Stone, velocity.X, velocity.Y, 100, Color.White, 3f);
                Main.dust[dust].noGravity = true;
            }

            DespawnSigils();

            if (AegonArena.Current != null)
            {
                AegonArena.Current.Remove();
            }

            NPC.active = false;

            if (Main.netMode != NetmodeID.Server)
            {
                Main.NewText("Not bad, kid.", 50, 125, 255);
            }
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
            int estimatedDamage = (int)modifiers.FinalDamage.Base;
            int maxDamageBeforeCap = (int)(NPC.lifeMax * 0.12f);

            if (estimatedDamage > maxDamageBeforeCap)
            {
                int excessDamage = estimatedDamage - maxDamageBeforeCap;
                int reducedExcess = (int)(excessDamage * 0.05f);
                int finalDamage = maxDamageBeforeCap + reducedExcess;

                modifiers.FinalDamage.Flat = finalDamage;
            }
        }

        public override void OnKill()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                Main.NewText("Not bad, kid", 255, 0, 0);
            }
        }

        public override bool CheckActive() => false;
    }
}