using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MightofUniverses.Content.NPCs.Bosses.Aegon
{
    public abstract class AegonSigilBase : ModNPC
    {
        private enum State
        {
            Idle = 0,
            Telegraph = 1,
            Attack = 2
        }

        protected int aegonIndex = -1;
        protected float orbitAngle = 0f;
        private State currentState = State.Idle;
        private int stateTimer = 0;

        protected abstract Color SigilColor { get; }
        protected abstract int SigilOrder { get; }
        protected abstract string SigilTexturePath { get; }
        protected abstract int ProjectileType { get; }
        protected virtual int TelegraphDustType => DustID.Electric;

        public override void SetStaticDefaults()
        {
            NPCID.Sets.MPAllowedEnemies[Type] = true;
        }

        public override void SetDefaults()
        {
            NPC.width = 80;
            NPC.height = 80;
            NPC.damage = 0;
            NPC.defense = 9999;
            NPC.lifeMax = 1;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.aiStyle = -1;
            NPC.dontTakeDamage = true;
            NPC.immortal = true;
        }

        public override void AI()
        {
            if (aegonIndex == -1 || !Main.npc[aegonIndex].active)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    if (Main.npc[i].active && Main.npc[i].type == ModContent.NPCType<Aegon>())
                    {
                        aegonIndex = i;
                        break;
                    }
                }
                
                if (aegonIndex == -1)
                {
                    NPC.active = false;
                    return;
                }
            }

            NPC aegon = Main.npc[aegonIndex];
            var aegonNPC = aegon.ModNPC as Aegon;
            int phase = aegonNPC?.GetCurrentPhase() ?? 0;
            bool patience = aegonNPC?.IsPatienceActive() ?? false;

            orbitAngle += 0.03f;
            float offset = SigilOrder / 5f * MathHelper.TwoPi;
            float angle = orbitAngle + offset;

            if (phase >= 4)
            {
                Player target = Main.player[aegon.target];
                NPC.Center = target.Center + new Vector2(
                    (float)Math.Cos(angle) * 480f,
                    (float)Math.Sin(angle) * 480f
                );

                switch (currentState)
                {
                    case State.Idle:
                        break;

                    case State.Telegraph:
                        if (stateTimer % 30 == 0)
                        {
                            for (int i = 0; i < 20; i++)
                            {
                                Vector2 dustVel = Main.rand.NextVector2Circular(4f, 4f);
                                Dust dust = Dust.NewDustDirect(
                                    NPC.Center - new Vector2(20, 20),
                                    40, 40,
                                    TelegraphDustType,
                                    dustVel.X, dustVel.Y,
                                    30,
                                    SigilColor,
                                    1.5f
                                );
                                dust.noGravity = true;
                            }
                        }

                        stateTimer++;
                        if (stateTimer >= 60)
                        {
                            currentState = State.Attack;
                            stateTimer = 0;
                        }
                        break;

                    case State.Attack:
                        if (patience)
                        {
                            for (int i = 0; i < 8; i++)
                            {
                                float burstAngle = i / 8f * MathHelper.TwoPi;
                                Vector2 vel = new Vector2(
                                    (float)Math.Cos(burstAngle),
                                    (float)Math.Sin(burstAngle)
                                ) * 5f;
                                
                                Projectile.NewProjectile(
                                    NPC.GetSource_FromAI(),
                                    NPC.Center,
                                    vel,
                                    ProjectileType,
                                    30,
                                    0f
                                );
                            }
                        }
                        else
                        {
                            Vector2 dir = target.Center - NPC.Center;
                            if (dir.Length() > 0)
                            {
                                dir.Normalize();
                                Projectile.NewProjectile(
                                    NPC.GetSource_FromAI(),
                                    NPC.Center,
                                    dir * 6f,
                                    ProjectileType,
                                    30,
                                    0f
                                );
                            }
                        }

                        currentState = State.Idle;
                        stateTimer = 0;
                        break;
                }
            }
            else
            {
                NPC.Center = aegon.Center + new Vector2(
                    (float)Math.Cos(angle) * 120f,
                    (float)Math.Sin(angle) * 180f
                );
            }

            NPC.rotation = 0f;
            NPC.velocity = Vector2.Zero;
        }

        public void StartTelegraph()
        {
            if (currentState == State.Idle)
            {
                currentState = State.Telegraph;
                stateTimer = 0;
            }
        }

        public bool IsReadyToAttack()
        {
            return currentState == State.Idle;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(SigilTexturePath).Value;
            Vector2 drawPos = NPC.Center - screenPos;
            Vector2 origin = texture.Size() / 2f;

            Color color = drawColor;
            float scale = 1.0f;

            if (currentState == State.Telegraph)
            {
                float pulse = 1f + (float)Math.Sin(stateTimer * 0.2f) * 0.3f;
                scale *= pulse;
                color = Color.Lerp(drawColor, SigilColor, (float)Math.Sin(stateTimer * 0.2f) * 0.5f + 0.5f);
            }

            spriteBatch.Draw(texture, drawPos, null, color, NPC.rotation, origin, scale, SpriteEffects.None, 0f);
            
            spriteBatch.Draw(texture, drawPos, null, SigilColor * 0.4f, NPC.rotation, origin, scale * 1.1f, SpriteEffects.None, 0f);

            return false;
        }

        public override bool? CanBeHitByItem(Player player, Item item) => false;
        public override bool? CanBeHitByProjectile(Projectile projectile) => false;
        public override bool CheckActive() => false;
    }
}