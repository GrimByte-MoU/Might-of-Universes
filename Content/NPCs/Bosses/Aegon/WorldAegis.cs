using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using ReLogic.Content;
using MightofUniverses.Content.Items.Projectiles.EnemyProjectiles;
using MightofUniverses.Content.Items.Weapons;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Content.Items.Accessories;
using MightofUniverses.Content.Items.BossBags;
using MightofUniverses.Content.NPCs.Bosses.Aegon;

namespace MightofUniverses.Content.NPCs.Bosses.Aegon
{
    public class WorldAegis : ModNPC
    {
        private enum State
        {
            Phase1_Full = 0,
            Phase2_FlewAway = 1,
            Phase3_Damaged = 2,
            Phase4_ScatterFragments = 3,
            Phase5_Orbiting = 4,
            Phase6_PostPatience = 5
        }

        private State CurrentState
        {
            get => (State)NPC.ai[0];
            set => NPC.ai[0] = (float)value;
        }

        private ref float AttackTimer => ref NPC.ai[1];
        private ref float RotationAngle => ref NPC.ai[2];
        private ref float SpiralDirection => ref NPC.ai[3];

        private int aegonIndex = -1;
        private Vector2 floatingPosition = Vector2.Zero;
        private bool floatingPositionSet = false;
        private bool hasCleanedUp = false;
        private float orbitAngle = 0f;

        private static Asset<Texture2D> textureNormal;
        private static Asset<Texture2D> textureDamaged;
        private static Asset<Texture2D> textureRuined;

        public override void SetStaticDefaults()
        {
            NPCID.Sets.MPAllowedEnemies[Type] = true;
            if (Main.netMode != NetmodeID.Server)
            {
                textureNormal = ModContent.Request<Texture2D>("MightofUniverses/Content/NPCs/Bosses/Aegon/WorldAegis");
                textureDamaged = ModContent.Request<Texture2D>("MightofUniverses/Content/NPCs/Bosses/Aegon/WorldAegis_Damaged");
                textureRuined = ModContent.Request<Texture2D>("MightofUniverses/Content/NPCs/Bosses/Aegon/WorldAegis_Ruined");
            }
        }

        public override void SetDefaults()
        {
            NPC.width = 160;
            NPC.height = 160;
            NPC.damage = 0;
            NPC.defense = 100;
            NPC.lifeMax = 100000;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = null;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.value = 0;
            NPC.boss = false;
            NPC.aiStyle = -1;
            NPC.netAlways = true;
            NPC.dontTakeDamage = false;

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
            floatingPosition = NPC.Center;
            floatingPositionSet = true;
            hasCleanedUp = false;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (CurrentState == State.Phase2_FlewAway || CurrentState == State.Phase4_ScatterFragments)
            {
                if (Vector2.Distance(NPC.Center, Main.LocalPlayer.Center) > 2000f)
                    return false;
            }

            Texture2D texture;
            if (CurrentState == State.Phase1_Full)
                texture = textureNormal.Value;
            else if (CurrentState == State.Phase2_FlewAway || CurrentState == State.Phase3_Damaged)
                texture = textureDamaged.Value;
            else
                texture = textureRuined.Value;

            Vector2 drawPos = NPC.Center - screenPos;
            Vector2 origin = texture.Size() / 2f;
            spriteBatch.Draw(texture, drawPos, null, drawColor, NPC.rotation, origin, 1.0f, SpriteEffects.None, 0f);
            return false;
        }

        public override void AI()
        {
            if (aegonIndex == -1 || !Main.npc[aegonIndex].active || Main.npc[aegonIndex].type != ModContent.NPCType<Aegon>())
            {
                FindAegon();
                if (aegonIndex == -1)
                {
                    if (!hasCleanedUp)
                    {
                        CleanupEverything();
                        hasCleanedUp = true;
                    }
                    NPC.active = false;
                    return;
                }
            }

            if (!IsAnyPlayerAlive())
            {
                if (!hasCleanedUp)
                {
                    CleanupEverything();
                    hasCleanedUp = true;
                }
                NPC.active = false;
                return;
            }

            if (CurrentState != State.Phase2_FlewAway && 
                CurrentState != State.Phase4_ScatterFragments && 
                CurrentState != State.Phase5_Orbiting)
            {
                float bobSpeed = 0.03f;
                float bobHeight = 8f;
                float bobOffset = (float)Math.Sin(AttackTimer * bobSpeed) * bobHeight;
                
                if (floatingPositionSet)
                {
                    NPC.Center = new Vector2(floatingPosition.X, floatingPosition.Y + bobOffset);
                    NPC.velocity = Vector2.Zero;
                }
            }

            NPC.rotation = 0f;

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
                case State.Phase5_Orbiting:
                    Phase5OrbitPlayer();
                    break;
                case State.Phase6_PostPatience:
                    Phase6FloatInPlace();
                    break;
            }

            AttackTimer++;
        }

        private bool IsAnyPlayerAlive()
        {
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                if (player != null && player.active && !player.dead)
                {
                    return true;
                }
            }
            return false;
        }

        private void CleanupEverything()
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc == null || !npc.active)
                    continue;

                if (npc.type == ModContent.NPCType<Aegon>())
                {
                    npc.active = false;
                    npc.life = 0;
                }
                else if (npc.type == ModContent.NPCType<AegonHallowedSigil>() ||
                         npc.type == ModContent.NPCType<AegonHellSigil>() ||
                         npc.type == ModContent.NPCType<AegonEvilSigil>() ||
                         npc.type == ModContent.NPCType<AegonDesertSigil>() ||
                         npc.type == ModContent.NPCType<AegonJungleSigil>())
                {
                    npc.active = false;
                    npc.life = 0;
                }
            }

            if (AegonArena.Current != null)
            {
                AegonArena.Current.Remove();
                AegonArena.Current = null;
            }
        }

        // ==================== PHASE 1: FULL SHIELD ====================
        private void Phase1Attacks()
        {
            Player target = Main.player[NPC.target];

            if (AttackTimer % 60 == 0)
            {
                int projectileCount = NPC.life / (float)NPC.lifeMax > 0.8f ? 8 : 12;
                for (int i = 0; i < projectileCount; i++)
                {
                    float angle = i / (float)projectileCount * MathHelper.TwoPi;
                    Vector2 spawnOffset = new Vector2(
                        (float)Math.Cos(angle),
                        (float)Math.Sin(angle)
                    ) * 64f;
                    Vector2 velocity = new Vector2(
                        (float)Math.Cos(angle),
                        (float)Math.Sin(angle)
                    ) * 4f;
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + spawnOffset, velocity,
                            ModContent.ProjectileType<WorldAegisBolt>(),
                            95, 0f);
                    }
                }
            }

            if (NPC.life / (float)NPC.lifeMax <= 0.9f && AttackTimer % 30 == 0)
            {
                Vector2 direction = target.Center - NPC.Center;
                if (direction != Vector2.Zero)
                {
                    direction.Normalize();
                    Vector2 velocity = direction * 6f;
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                            ModContent.ProjectileType<WorldAegisLeaf>(),
                            100, 0f);
                    }
                }
            }
        }

        // ==================== TRANSITION: PHASE 1 → PHASE 2 ====================
        public void TransitionToPhase2AndFlyAway()
        {
            CurrentState = State.Phase2_FlewAway;
            NPC.dontTakeDamage = true;
            NPC.dontCountMe = true;
            AttackTimer = 0;
            NPC.velocity = new Vector2(0, -25f);

            for (int i = 0; i < 30; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(4f, 4f);
                int dust = Dust.NewDust(NPC.Center, NPC.width, NPC.height, DustID.Stone, velocity.X, velocity.Y, 100, Color.Orange, 1.5f);
                Main.dust[dust].noGravity = true;
            }
        }

        // ==================== PHASE 2: FLEW AWAY ====================
        private void Phase2Inactive()
        {
            if (AttackTimer < 120)
            {
                NPC.velocity.Y = -10f;
            }
            else
            {
                NPC.velocity = Vector2.Zero;
            }
        }

        // ==================== TRANSITION: PHASE 2 → PHASE 3 ====================
        public void ReturnToArenaForPhase3(Vector2 center)
        {
            CurrentState = State.Phase3_Damaged;
            NPC.dontTakeDamage = false;
            NPC.dontCountMe = false;
            NPC.Center = center;
            NPC.velocity = Vector2.Zero;
            AttackTimer = 0;
            floatingPosition = center;
            floatingPositionSet = true;

            for (int i = 0; i < 40; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(5f, 5f);
                int dust = Dust.NewDust(NPC.Center, NPC.width, NPC.height, DustID.Stone, velocity.X, velocity.Y, 100, Color.Orange, 1.8f);
                Main.dust[dust].noGravity = true;
            }
        }

        // ==================== PHASE 3: DAMAGED SHIELD ====================
        private void Phase3Attacks()
        {
            int tenPercent = (int)Math.Ceiling(NPC.lifeMax * 0.10f);

            if (NPC.life <= tenPercent && CurrentState != State.Phase4_ScatterFragments)
            {
                NPC.life = tenPercent;
                TransitionToPhase4AndScatterFragments();
                return;
            }

            if (AttackTimer % 120 == 0)
            {
                for (int i = 0; i < 12; i++)
                {
                    float angle = i / 12f * MathHelper.TwoPi;
                    Vector2 velocity = new Vector2(
                        (float)Math.Cos(angle),
                        (float)Math.Sin(angle)
                    ) * 4f;
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                            ModContent.ProjectileType<AegisShard>(),
                            95, 0f);
                    }
                }
            }
        }

        public override void ModifyIncomingHit(ref NPC.HitModifiers modifiers)
        {
            if (CurrentState == State.Phase3_Damaged)
            {
                int tenPercent = (int)Math.Ceiling(NPC.lifeMax * 0.10f);
                if (NPC.life > tenPercent && NPC.life - modifiers.FinalDamage.Base <= tenPercent)
                {
                    modifiers.FinalDamage.Base = Math.Max(0, NPC.life - tenPercent);
                    NPC.life = tenPercent;
                    TransitionToPhase4AndScatterFragments();
                }
            }
        }

        // ==================== HELPER METHODS FOR PHASE 3 ====================
        public void FireWorldAegisFireball()
        {
            Player target = Main.player[NPC.target];
            Vector2 velocity = (target.Center - NPC.Center).SafeNormalize(Vector2.UnitX) * 7f;
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                    ModContent.ProjectileType<WorldAegisFireball>(),
                    95, 0f);
            }
        }

        public void StartWaterSpiral(bool clockwise)
        {
            SpiralDirection = clockwise ? 1f : -1f;
            RotationAngle = 0f;
        }

        public void FireWaterSpiral()
        {
            if (AttackTimer % 20 == 0)
            {
                RotationAngle += 0.2f * SpiralDirection;
                for (int i = 0; i < 2; i++)
                {
                    float angle = RotationAngle + (i * MathHelper.Pi);
                    Vector2 velocity = new Vector2(
                        (float)Math.Cos(angle),
                        (float)Math.Sin(angle)
                    ) * 5f;
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                            ModContent.ProjectileType<WorldAegisWater>(),
                            95, 0f);
                    }
                }
            }
        }

        // ==================== TRANSITION: PHASE 3 → PHASE 4 ====================
        public void TransitionToPhase4AndScatterFragments()
        {
            if (CurrentState == State.Phase4_ScatterFragments) return;
            
            CurrentState = State.Phase4_ScatterFragments;
            NPC.life = (int)(NPC.lifeMax * 0.05f);
            NPC.dontTakeDamage = true;
            NPC.dontCountMe = true;
            AttackTimer = 0;
            NPC.velocity = new Vector2(0, -25f);

            for (int i = 0; i < 20; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(8f, 8f);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                        ModContent.ProjectileType<AegisFragment>(),
                        125, 0f);
                }
            }

            for (int i = 0; i < 50; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(6f, 6f);
                int dust = Dust.NewDust(NPC.Center, NPC.width, NPC.height, DustID.Stone, velocity.X, velocity.Y, 100, Color.Red, 2f);
                Main.dust[dust].noGravity = true;
            }
        }

        // ==================== PHASE 4: FLY AWAY ====================
        private void Phase4FlyAway()
        {
            if (AttackTimer < 120)
            {
                NPC.velocity.Y = -10f;
            }
            else
            {
                NPC.velocity = Vector2.Zero;
            }
        }

        public bool IsInScatterFragmentsPhase() => CurrentState == State.Phase4_ScatterFragments;

        // ==================== TRANSITION: PHASE 4 → PHASE 5 (PATIENCE) ====================
        public void ReturnForPhase5(Vector2 playerCenter)
        {
            CurrentState = State.Phase5_Orbiting;
            NPC.dontCountMe = false;
            NPC.Center = playerCenter + new Vector2(50 * 16f, 0); // Start 50 blocks right
            NPC.velocity = Vector2.Zero;
            NPC.life = (int)(NPC.lifeMax * 0.05f);
            NPC.dontTakeDamage = true;
            NPC.active = true;
            AttackTimer = 0;
            orbitAngle = 0f;

            for (int i = 0; i < 60; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(7f, 7f);
                int dust = Dust.NewDust(NPC.Center, NPC.width, NPC.height, DustID.Stone, velocity.X, velocity.Y, 100, Color.DarkRed, 2.5f);
                Main.dust[dust].noGravity = true;
            }
        }

        // ==================== PHASE 5: ORBIT PLAYER (PATIENCE) ====================
        private void Phase5OrbitPlayer()
        {
            if (aegonIndex == -1 || !Main.npc[aegonIndex].active)
                return;

            Player target = Main.player[Main.npc[aegonIndex].target];

            float orbitRadius = 50f * 16f;
            float orbitSpeed = 0.02f;
            orbitAngle -= orbitSpeed;

            NPC.Center = target.Center + new Vector2(
                (float)Math.Cos(orbitAngle) * orbitRadius,
                (float)Math.Sin(orbitAngle) * orbitRadius
            );

            NPC.rotation = 0f;
            NPC.velocity = Vector2.Zero;
        }

        // ==================== TRANSITION: PHASE 5 → PHASE 6 (POST-PATIENCE) ====================
        public void MakeVulnerable()
        {
            CurrentState = State.Phase6_PostPatience;
            NPC.dontTakeDamage = false;
            NPC.velocity = Vector2.Zero;
            floatingPosition = NPC.Center;
            floatingPositionSet = true;

            for (int i = 0; i < 80; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(8f, 8f);
                int dust = Dust.NewDust(NPC.Center, NPC.width, NPC.height, DustID.Stone, velocity.X, velocity.Y, 100, Color.Gold, 2.5f);
                Main.dust[dust].noGravity = true;
            }
        }

        // ==================== PHASE 6: FLOAT IN PLACE (POST-PATIENCE) ====================
        private void Phase6FloatInPlace()
        {
            float bobSpeed = 0.03f;
            float bobHeight = 10f;
            float bobOffset = (float)Math.Sin(AttackTimer * bobSpeed) * bobHeight;

            if (floatingPositionSet)
            {
                NPC.Center = new Vector2(floatingPosition.X, floatingPosition.Y + bobOffset);
                NPC.velocity = Vector2.Zero;
                NPC.rotation = 0f;
            }
        }

        // ==================== HELPER METHODS ====================
        private void FindAegon()
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active && Main.npc[i].type == ModContent.NPCType<Aegon>())
                {
                    aegonIndex = i;
                    return;
                }
            }
        }

        // ==================== ON KILL (DROPS LOOT) ====================
        public override void OnKill()
        {
            if (!hasCleanedUp)
            {
                CleanupEverything();
                hasCleanedUp = true;
            }

            if (aegonIndex != -1 && Main.npc[aegonIndex].active)
            {
                var aegon = Main.npc[aegonIndex].ModNPC as Aegon;
                aegon?.EndFight();
            }

            if (Main.netMode != NetmodeID.Server)
            {
                Main.NewText("Aegon has been defeated!", 50, 125, 255);
            }

            if (!Main.expertMode && !Main.masterMode)
            {
                int[] weapons = new int[]
                {
                    ModContent.ItemType<MouldoftheMother>(),
                    ModContent.ItemType<TomeoftheWorldsSoul>(),
                    ModContent.ItemType<GaiasLongbow>(),
                    ModContent.ItemType<WorldwalkerSword>(),
                    ModContent.ItemType<BiomeCleanser>(),
                };

                List<int> weaponList = new List<int>(weapons);
                List<int> toGive = new List<int>();
                for (int i = 0; i < 2 && weaponList.Count > 0; i++)
                {
                    int pick = Main.rand.Next(weaponList.Count);
                    toGive.Add(weaponList[pick]);
                    weaponList.RemoveAt(pick);
                }

                foreach (Player p in Main.player)
                {
                    if (p.active && !p.dead)
                    {
                        foreach (int w in toGive)
                            p.QuickSpawnItem(NPC.GetSource_Loot(), w);
                        p.QuickSpawnItem(NPC.GetSource_Loot(), ModContent.ItemType<AegisRemains>(), Main.rand.Next(200, 281));
                    }
                }
            }
        }

        public override bool CheckActive() => false;

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            int bagType = ModContent.ItemType<WorldAegisBag>();
            npcLoot.Add(ItemDropRule.BossBag(bagType));
        }
    }
}