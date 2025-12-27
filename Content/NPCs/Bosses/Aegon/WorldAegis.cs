// Content/NPCs/Bosses/Aegon/WorldAegis. cs

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria. ID;
using Terraria. ModLoader;
using ReLogic.Content;
using MightofUniverses.Content.Items.Projectiles. EnemyProjectiles;

namespace MightofUniverses. Content.NPCs.Bosses.Aegon
{
    public class WorldAegis : ModNPC
    {
        private enum State
        {
            Phase1_Full = 0,
            Phase2_FlewAway = 1,
            Phase3_Damaged = 2,
            Phase4_ScatterFragments = 3,
            Phase5_HeavilyDamaged = 4
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
        private Vector2 arenaCenter = Vector2.Zero;
        private bool arenaCenterSet = false;

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
    NPC.defense = 120;
    NPC.lifeMax = 100000;
    NPC.HitSound = SoundID.NPCHit4;
    NPC.DeathSound = null;
    NPC.knockBackResist = 0f;
    NPC. noGravity = true;
    NPC.noTileCollide = true;
    NPC.value = 0;
    NPC.boss = false;
    NPC.aiStyle = -1;
    NPC.netAlways = true;
    NPC.dontTakeDamage = false;

    if (Main.expertMode)
    {
        NPC.lifeMax = 150000;
        NPC.defense = 150;
    }

    if (Main.masterMode)
    {
        NPC. lifeMax = 200000;
        NPC.defense = 180;
    }
}

        public override void OnSpawn(IEntitySource source)
        {
            arenaCenter = NPC.Center;
            arenaCenterSet = true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if ((CurrentState == State.Phase2_FlewAway || CurrentState == State.Phase4_ScatterFragments))
            {
                if (Vector2.Distance(NPC.Center, Main.LocalPlayer.Center) > 2000f)
                    return false;
            }

            Texture2D texture;
            
            if (CurrentState == State. Phase1_Full)
            {
                texture = textureNormal. Value;
            }
            else if (CurrentState == State.Phase2_FlewAway || CurrentState == State.Phase3_Damaged)
            {
                texture = textureDamaged.Value;
            }
            else
            {
                texture = textureRuined.Value;
            }

            Vector2 drawPos = NPC.Center - screenPos;
            Vector2 origin = texture.Size() / 2f;

            spriteBatch.Draw(texture, drawPos, null, drawColor, NPC.rotation, origin, 1.0f, SpriteEffects.None, 0f);

            return false;
        }

        public override void AI()
        {
            if (aegonIndex == -1 || ! Main.npc[aegonIndex]. active || Main.npc[aegonIndex].type != ModContent.NPCType<Aegon>())
            {
                FindAegon();
                
                if (aegonIndex == -1)
                {
                    NPC.active = false;
                    return;
                }
            }

            float bobSpeed = 0.03f;
            float bobHeight = 8f;
            float bobOffset = (float)Math.Sin(AttackTimer * bobSpeed) * bobHeight;

            if (CurrentState != State.Phase2_FlewAway && CurrentState != State.Phase4_ScatterFragments)
            {
                if (arenaCenterSet)
                {
                    NPC.Center = new Vector2(arenaCenter.X, arenaCenter.Y + bobOffset);
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
                case State.Phase5_HeavilyDamaged:  
                    Phase5Attacks();
                    break;
            }

            AttackTimer++;
        }

        public override void ModifyIncomingHit(ref NPC.HitModifiers modifiers)
        {
            if (CurrentState == State.Phase3_Damaged && NPC.life <= 100)
            {
                if (NPC.life - modifiers.FinalDamage. Base <= 0)
                {
                    NPC.life = 1;
                    modifiers.FinalDamage.Base = 0;

                    TransitionToPhase4AndScatterFragments();
                }
            }
        }

        private void Phase1Attacks()
        {
            Player target = Main.player[NPC.target];

            if (AttackTimer % 60 == 0)
            {
                int projectileCount = NPC.life / (float)NPC.lifeMax > 0.8f ? 6 : 8;

                for (int i = 0; i < projectileCount; i++)
                {
                    float angle = (i / (float)projectileCount) * MathHelper.TwoPi;

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

            if (NPC.life / (float)NPC.lifeMax <= 0.9f && AttackTimer % 120 == 0)
            {
                // Calculate direction TO player (no fallback needed)
                Vector2 direction = target.Center - NPC.Center;

                // Normalize and apply speed
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

        public void TransitionToPhase2AndFlyAway()
        {
            CurrentState = State.Phase2_FlewAway;
            NPC. dontTakeDamage = true;
            NPC. dontCountMe = true;
            AttackTimer = 0;
            NPC.velocity = new Vector2(0, -10f);
            
            for (int i = 0; i < 30; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(4f, 4f);
                int dust = Dust.NewDust(NPC.Center, NPC.width, NPC.height, DustID.Stone, velocity.X, velocity.Y, 100, Color.Orange, 1.5f);
                Main.dust[dust].noGravity = true;
            }
        }

        private void Phase2Inactive()
        {
            if (AttackTimer < 120)
            {
                NPC.velocity. Y = -5f;
            }
            else
            {
                NPC.velocity = Vector2.Zero;
            }
        }

        public void ReturnToArenaForPhase3(Vector2 center)
        {
            CurrentState = State.Phase3_Damaged;
            NPC.dontTakeDamage = false;
            NPC.dontCountMe = false;
            NPC.Center = center;
            NPC. velocity = Vector2.Zero;
            AttackTimer = 0;
            arenaCenter = center;
            arenaCenterSet = true;
            
            for (int i = 0; i < 40; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(5f, 5f);
                int dust = Dust.NewDust(NPC.Center, NPC.width, NPC.height, DustID.Stone, velocity.X, velocity.Y, 100, Color.Orange, 1.8f);
                Main.dust[dust].noGravity = true;
            }
        }

        private void Phase3Attacks()
        {
            if (NPC.life <= 1 && CurrentState != State.Phase4_ScatterFragments)
            {
                TransitionToPhase4AndScatterFragments();
                return;
            }

            if (AttackTimer % 120 == 0)
            {
                for (int i = 0; i < 12; i++)
                {
                    float angle = (i / 12f) * MathHelper.TwoPi;
                    Vector2 velocity = new Vector2(
                        (float)Math.Cos(angle),
                        (float)Math.Sin(angle)
                    ) * 4f;

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                            ModContent. ProjectileType<WorldAegisBolt>(),
                            95, 0f);
                    }
                }
            }
        }

        public void FireWorldAegisFireball()
        {
            Player target = Main.player[NPC.target];
            Vector2 velocity = (target.Center - NPC.Center).SafeNormalize(Vector2.UnitX) * 7f;

            if (Main. netMode != NetmodeID. MultiplayerClient)
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
                        Projectile.NewProjectile(NPC. GetSource_FromAI(), NPC.Center, velocity,
                            ModContent.ProjectileType<WorldAegisWater>(),
                            95, 0f);
                    }
                }
            }
        }

        public void TransitionToPhase4AndScatterFragments()
        {
            if (CurrentState == State.Phase4_ScatterFragments) return;

            CurrentState = State.Phase4_ScatterFragments;
            NPC.life = 1;
            NPC.dontTakeDamage = true;
            NPC.dontCountMe = true;
            AttackTimer = 0;
            NPC.velocity = new Vector2(0, -8f);
            
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
                int dust = Dust.NewDust(NPC.Center, NPC.width, NPC.height, DustID.Stone, velocity. X, velocity.Y, 100, Color.Red, 2f);
                Main.dust[dust].noGravity = true;
            }
        }

        private void Phase4FlyAway()
        {
            if (AttackTimer < 120)
            {
                NPC.velocity. Y = -6f;
            }
            else
            {
                NPC. velocity = Vector2.Zero;
            }
        }

        public void ReturnForPhase5(Vector2 center)
{
    CurrentState = State.Phase5_HeavilyDamaged;
    NPC. dontCountMe = false;
    NPC.Center = center; // ← Force to exact center
    NPC.velocity = Vector2.Zero;
    NPC.life = 1;
    NPC. dontTakeDamage = true; // ← Immune until patience ends
    NPC.active = true; // ← Make sure it's active
    AttackTimer = 0;
    arenaCenter = center;
    arenaCenterSet = true;
    
    for (int i = 0; i < 60; i++)
    {
        Vector2 velocity = Main.rand.NextVector2Circular(7f, 7f);
        int dust = Dust.NewDust(NPC.Center, NPC.width, NPC.height, DustID.Stone, velocity.X, velocity.Y, 100, Color.DarkRed, 2.5f);
        Main.dust[dust].noGravity = true;
    }
}

        private void Phase5Attacks()
        {
            if (AttackTimer % 120 == 0)
            {
                for (int i = 0; i < 12; i++)
                {
                    float angle = (i / 12f) * MathHelper.TwoPi;
                    Vector2 velocity = new Vector2(
                        (float)Math.Cos(angle),
                        (float)Math.Sin(angle)
                    ) * 2f;

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(NPC. GetSource_FromAI(), NPC.Center, velocity,
                            ModContent.ProjectileType<WorldAegisLeaf>(),
                            100, 0f);
                    }
                }
            }
        }

        public void MakeVulnerable()
        {
            NPC. dontTakeDamage = false;
        }

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

        public override void OnKill()
        {
            if (CurrentState == State.Phase5_HeavilyDamaged && ! NPC.dontTakeDamage)
            {
                if (aegonIndex != -1 && Main.npc[aegonIndex].active)
                {
                    var aegon = Main.npc[aegonIndex]. ModNPC as Aegon;
                    aegon?. EndFight();
                }
            }
        }

        public override bool CheckActive()
        {
            return false;
        }
    }
}