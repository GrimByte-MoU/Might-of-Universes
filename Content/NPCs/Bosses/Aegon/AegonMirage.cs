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
    public class AegonMirage :  ModNPC
    {
        private ref float AttackTimer => ref NPC.ai[0];
        private ref float AttackState => ref NPC.ai[1];
        private ref float SubTimer => ref NPC.ai[2];

        private int aegonIndex = -1;
        private Vector2 targetPosition;

        public override void SetStaticDefaults()
        {
            NPCID.Sets.MPAllowedEnemies[Type] = true;
            NPCID.Sets.ProjectileNPC[Type] = false;
        }

        public override void SetDefaults()
        {
            NPC.width = 120;
            NPC.height = 120;
            NPC. damage = 0;
            NPC.defense = 9999;
            NPC.lifeMax = 1;
            NPC.life = 1;
            NPC.HitSound = null;
            NPC.DeathSound = null;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.value = 0;
            NPC.boss = false;
            NPC.aiStyle = -1;
            NPC.netAlways = true;
            NPC.dontTakeDamage = true;
            NPC. dontCountMe = true;
            NPC. immortal = true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>("MightofUniverses/Content/NPCs/Bosses/Aegon/Aegon").Value;

            Vector2 drawPos = NPC.Center - screenPos;
            Vector2 origin = texture.Size() / 2f;

            Color mirageColor = drawColor * 0.6f;

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

            for (int i = 0; i < 4; i++)
            {
                Vector2 offset = new Vector2(
                    (float)Math.Cos(MathHelper.TwoPi * i / 4f),
                    (float)Math.Sin(MathHelper. TwoPi * i / 4f)
                ) * 2f;

                spriteBatch.Draw(
                    texture,
                    drawPos + offset,
                    null,
                    Color. Cyan * 0.3f,
                    NPC.rotation,
                    origin,
                    NPC.scale,
                    SpriteEffects.None,
                    0f
                );
            }

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

            NPC aegon = Main.npc[aegonIndex];
            Player target = Main.player[aegon. target];

            if (Main.rand.NextBool(5))
            {
                int dust = Dust.NewDust(NPC.Center, NPC.width, NPC.height, DustID.Electric, 0f, 0f, 100, Color. Cyan, 0.8f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.3f;
            }

            AttackTimer++;
        }

        // ==================== POSITION MANAGEMENT ====================

        public void SetPosition(Vector2 position)
        {
            NPC.Center = position;
            targetPosition = position;
            NPC.velocity = Vector2.Zero;
        }

        public void SetTargetPosition(Vector2 position)
        {
            targetPosition = position;
        }

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

        public void MirrorAttackA_AegisFragmentCircle(Vector2 playerPosition, bool flipped)
        {
            int fragmentCount = 5;

            for (int i = 0; i < fragmentCount; i++)
            {
                float angle = (i / (float)fragmentCount) * MathHelper.TwoPi;

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
                        ModContent.ProjectileType<AegisFragment>(),
                        125, 0f);
                }
            }
        }

        public void MirrorAttackB_AegisShardSpread(Vector2 playerPosition, bool flipped)
        {
            int spreadCount = 9;
            float baseAngle = (playerPosition - NPC.Center).ToRotation();

            for (int i = 0; i < spreadCount; i++)
            {
                float offsetAngle = MathHelper.Lerp(-0.5f, 0.5f, i / (float)(spreadCount - 1));
                float angle = baseAngle + offsetAngle;

                if (flipped)
                {
                    angle = MathHelper. Pi - angle;
                }

                float speed = 4f + Main.rand.NextFloat(-1f, 2f);

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
                    125, 0f);
            }
        }

        public void MirrorAttackD_RapidShards(float verticalProgress, bool shootingUp, bool flipped)
        {
            float angle = flipped ? MathHelper.Pi :  0f;

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

        // ==================== PHASE 5 PATIENCE ATTACKS ====================

        public void FireJungleNeedleSpread(Vector2 arenaCenter, bool flipped)
        {
            int spreadCount = 7;
            float baseAngle = (arenaCenter - NPC.Center).ToRotation();

            for (int i = 0; i < spreadCount; i++)
            {
                float offsetAngle = MathHelper. Lerp(-MathHelper.PiOver4, MathHelper.PiOver4, i / (float)(spreadCount - 1));
                float angle = baseAngle + offsetAngle;

                Vector2 velocity = new Vector2(
                    (float)Math.Cos(angle),
                    (float)Math.Sin(angle)
                ) * 3f;

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                        ModContent.ProjectileType<JungleNeedle>(),
                        95, 0f);
                }
            }
        }

        public void FireCrimruptionBolts(Vector2 arenaCenter, bool flipped)
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

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                        ModContent.ProjectileType<CrimruptionBolt>(),
                        120, 0f);
                }
            }
        }

        public void FireOceanSphere(Vector2 arenaCenter, bool flipped)
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
                    120, 0f);
            }
        }

        public void FireHallowedSpears(Vector2 playerPosition, bool flipped)
        {
            int spreadCount = 3;
            float baseAngle = (playerPosition - NPC.Center).ToRotation();

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
                    Projectile. NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                        ModContent.ProjectileType<HallowedSpear>(),
                        120, 0f);
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

        public override bool?  CanBeHitByItem(Player player, Item item)
        {
            return false;
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            return false;
        }

        public override bool CheckActive()
        {
            return false;
        }

        public override void OnKill()
        {
            for (int i = 0; i < 20; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(4f, 4f);
                int dust = Dust.NewDust(NPC.Center, NPC.width, NPC.height, DustID.Electric, velocity.X, velocity.Y, 100, Color.Cyan, 1.2f);
                Main.dust[dust].noGravity = true;
            }
        }
    }
}