using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Projectiles.EnemyProjectiles;

namespace MightofUniverses.Content.NPCs.Bosses.Aegon
{
    public abstract class AegonSigilBase : ModNPC
    {
        protected enum SigilState
        {
            Orbiting = 0,
            Telegraphing = 1,
            Attacking = 2,
            Cooldown = 3
        }

        protected SigilState CurrentState
        {
            get => (SigilState)NPC.ai[0];
            set => NPC.ai[0] = (float)value;
        }

        protected ref float StateTimer => ref NPC.ai[1];
        protected ref float OrbitAngle => ref NPC.ai[2];
        protected ref float FlashTimer => ref NPC.ai[3];

        protected int aegonIndex = -1;
        protected Player targetPlayer = null;
        protected float orbitRadius = 640f;
        protected int telegraphDuration = 180;
        protected int attackDuration = 120;
        protected int cooldownDuration = 60;
        protected bool isPatienceMode = false;

        protected abstract Color SigilColor { get; }
        protected abstract int SigilOrder { get; } 
        protected abstract string SigilTexturePath { get; }
        protected abstract int ProjectileType { get; }

        public override void SetStaticDefaults()
        {
            NPCID.Sets.MPAllowedEnemies[Type] = true;
            NPCID.Sets.ProjectileNPC[Type] = false;
        }

        public override void SetDefaults()
        {
            NPC.width = 80;
            NPC.height = 80;
            NPC.damage = 0;
            NPC.defense = 9999;
            NPC.lifeMax = 1;
            NPC.life = 1;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.value = 0;
            NPC.boss = false;
            NPC.aiStyle = -1;
            NPC.netAlways = true;
            NPC.dontTakeDamage = true;
            NPC.dontCountMe = true;
            NPC.immortal = true;
            NPC.HitSound = null;
            NPC.DeathSound = null;
        }

        public override void AI()
        {
            if (aegonIndex == -1 || !Main.npc[aegonIndex].active || Main.npc[aegonIndex].type != ModContent.NPCType<Aegon>())
            {
                FindAegon();
                if (aegonIndex == -1)
                {
                    NPC.active = false;
                    return;
                }
            }

            NPC aegon = Main.npc[aegonIndex];
            targetPlayer = Main.player[aegon.target];
            var aegonNPC = aegon.ModNPC as Aegon;
            int currentPhase = aegonNPC != null ? aegonNPC.GetCurrentPhase() : 0;
            isPatienceMode = aegonNPC != null && aegonNPC.IsPatienceActive();

            if (currentPhase >= 5)
            {
                OrbitPlayerClockwiseAndAttack();
            }
            else
            {
                OrbitAegonFrontToBack();
            }

            StateTimer++;
        }

        private void OrbitAegonFrontToBack()
        {
            NPC aegon = Main.npc[aegonIndex];
            OrbitAngle += 0.04f;
            
            float offset = SigilOrder / 5f * MathHelper.TwoPi;
            float angle = OrbitAngle + offset;

            float horizontalRadius = 120f;
            float verticalRadius = 180f;

            Vector2 orbitPos = aegon.Center + new Vector2(
                (float)Math.Cos(angle) * horizontalRadius,
                (float)Math.Sin(angle) * verticalRadius
            );

            NPC.Center = orbitPos;
            NPC.rotation = (aegon.Center - NPC.Center).ToRotation() + MathHelper.PiOver2;

            float normalizedAngle = (angle + offset) % MathHelper.TwoPi;
            if (normalizedAngle > MathHelper.Pi)
            {
                NPC.hide = true;
            }
            else
            {
                NPC.hide = false;
            }

            if (Main.rand.NextBool(10))
            {
                Dust dust = Dust.NewDustPerfect(NPC.Center, DustID.Electric, Vector2.Zero, 100, SigilColor, 1.0f);
                dust.noGravity = true;
            }
        }

        private void OrbitPlayerClockwiseAndAttack()
        {
            OrbitAngle += 0.03f;
            
            float offset = (SigilOrder / 5f) * MathHelper.TwoPi;
            float angle = OrbitAngle + offset;

            Vector2 orbitPos = targetPlayer.Center + new Vector2(
                (float)Math.Cos(angle) * orbitRadius,
                (float)Math.Sin(angle) * orbitRadius
            );

            NPC.Center = orbitPos;
            NPC.rotation = (targetPlayer.Center - NPC.Center).ToRotation() + MathHelper.PiOver2;
            NPC.hide = false;

            // State machine
            switch (CurrentState)
            {
                case SigilState.Orbiting:
                    break;

                case SigilState.Telegraphing:
                    TelegraphAttack();
                    if (StateTimer >= telegraphDuration)
                    {
                        CurrentState = SigilState.Attacking;
                        StateTimer = 0;
                    }
                    break;

                case SigilState.Attacking:
                    PerformAttack();
                    if (StateTimer >= attackDuration)
                    {
                        CurrentState = SigilState.Cooldown;
                        StateTimer = 0;
                    }
                    break;

                case SigilState.Cooldown:
                    if (StateTimer >= cooldownDuration)
                    {
                        CurrentState = SigilState.Orbiting;
                        StateTimer = 0;
                    }
                    break;
            }
        }

        private void TelegraphAttack()
        {
            FlashTimer++;
            if (FlashTimer % 60 == 0)
            {
                for (int i = 0; i < 20; i++)
                {
                    Vector2 velocity = Main.rand.NextVector2Circular(4f, 4f);
                    Dust dust = Dust.NewDustDirect(
                        NPC.Center - new Vector2(20, 20),
                        40, 40,
                        DustID.Electric,
                        velocity.X, velocity.Y,
                        100,
                        SigilColor,
                        1.5f
                    );
                    dust.noGravity = true;
                }
            }
        }

        protected virtual void PerformAttack()
        {
            if (isPatienceMode)
            {
                if (StateTimer % 120 == 0)
                {
                    FireRadialBurst();
                }
            }
            else
            {
                if (StateTimer == 0)
                {
                    FireSingleBolt();
                }
            }
        }

        protected void FireSingleBolt()
        {
            if (targetPlayer == null)
                return;

            Vector2 direction = (targetPlayer.Center - NPC.Center).SafeNormalize(Vector2.UnitX);
            Vector2 velocity = direction * 6f;

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Projectile.NewProjectile(
                    NPC.GetSource_FromAI(),
                    NPC.Center,
                    velocity,
                    ProjectileType,
                    100,
                    0f
                );
            }
        }

        protected void FireRadialBurst()
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
                    Projectile.NewProjectile(
                        NPC.GetSource_FromAI(),
                        NPC.Center,
                        velocity,
                        ProjectileType,
                        100,
                        0f
                    );
                }
            }
        }
        public void StartTelegraph()
        {
            if (CurrentState == SigilState.Orbiting)
            {
                CurrentState = SigilState.Telegraphing;
                StateTimer = 0;
                FlashTimer = 0;
            }
        }

        public bool IsReadyToAttack() => CurrentState == SigilState.Orbiting;

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

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(SigilTexturePath).Value;
            Vector2 drawPos = NPC.Center - screenPos;
            Vector2 origin = texture.Size() / 2f;

            float scale = 1.0f;
            Color color = drawColor;

            if (CurrentState == SigilState.Telegraphing)
            {
                float pulse = 1f + (float)Math.Sin(FlashTimer * 0.2f) * 0.3f;
                scale *= pulse;
                color = Color.Lerp(drawColor, SigilColor, (float)Math.Sin(FlashTimer * 0.2f) * 0.5f + 0.5f);
            }

            var aegonNPC = aegonIndex >= 0 ? Main.npc[aegonIndex].ModNPC as Aegon : null;
            bool isPhase5or6 = aegonNPC != null && aegonNPC.GetCurrentPhase() >= 5;

            if (!isPhase5or6)
            {
                float normalizedAngle = (OrbitAngle + SigilOrder / 5f * MathHelper.TwoPi) % MathHelper.TwoPi;
                float depthFade = 1f;

                if (normalizedAngle > MathHelper.Pi)
                {
                    float fadeAmount = (normalizedAngle - MathHelper.Pi) / MathHelper.Pi;
                    depthFade = 0.4f + (fadeAmount * 0.3f);
                }

                color *= depthFade;
            }

            spriteBatch.Draw(texture, drawPos, null, color, NPC.rotation, origin, scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(texture, drawPos, null, SigilColor * 0.4f, NPC.rotation, origin, scale * 1.1f, SpriteEffects.None, 0f);

            return false;
        }

        public override void DrawBehind(int index)
        {
            if (NPC.hide)
            {
                Main.instance.DrawCacheNPCsBehindNonSolidTiles.Add(index);
            }
        }

        public override bool? CanBeHitByItem(Player player, Item item) => false;
        public override bool? CanBeHitByProjectile(Projectile projectile) => false;
        public override bool CheckActive() => false;

        public override void OnKill()
        {
            for (int i = 0; i < 30; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(6f, 6f);
                Dust dust = Dust.NewDustDirect(
                    NPC.Center - new Vector2(20, 20),
                    40, 40,
                    DustID.Electric,
                    velocity.X, velocity.Y,
                    100,
                    SigilColor,
                    2f
                );
                dust.noGravity = true;
            }
        }
    }
}