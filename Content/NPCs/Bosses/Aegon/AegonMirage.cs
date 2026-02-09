using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Projectiles.EnemyProjectiles;

namespace MightofUniverses.Content.NPCs.Bosses.Aegon
{
    public class AegonMirage : ModNPC
    {
        private int aegonIndex = -1;

        public override void SetStaticDefaults()
        {
            NPCID.Sets.MPAllowedEnemies[Type] = true;
            NPCID.Sets.ProjectileNPC[Type] = false;
        }

        public override void SetDefaults()
        {
            NPC.width = 120;
            NPC.height = 120;
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
                FindAegon();

            if (aegonIndex == -1)
            {
                NPC.active = false;
                return;
            }

            NPC aegon = Main.npc[aegonIndex];
            Player target = Main.player[aegon.target];

            NPC.Center = new Vector2(
                target.Center.X - (aegon.Center.X - target.Center.X),
                aegon.Center.Y
            );

            if (Main.rand.NextBool(5))
            {
                int dust = Dust.NewDust(NPC.Center, NPC.width, NPC.height, DustID.Electric, 0f, 0f, 100, Color.Cyan, 0.8f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.3f;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>("MightofUniverses/Content/NPCs/Bosses/Aegon/Aegon").Value;
            Vector2 drawPos = NPC.Center - screenPos;
            Vector2 origin = texture.Size() / 2f;
            Color mirageColor = drawColor * 0.6f;
            spriteBatch.Draw(texture, drawPos, null, mirageColor, NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0f);

            for (int i = 0; i < 4; i++)
            {
                Vector2 offset = new Vector2(
                    (float)Math.Cos(MathHelper.TwoPi * i / 4f),
                    (float)Math.Sin(MathHelper.TwoPi * i / 4f)
                ) * 2f;

                spriteBatch.Draw(texture, drawPos + offset, null, Color.Cyan * 0.3f, NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0f);
            }
            return false;
        }

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

        public override bool? CanBeHitByItem(Player player, Item item) => false;
        public override bool? CanBeHitByProjectile(Projectile projectile) => false;
        public override bool CheckActive() => false;

        public override void OnKill()
        {
            for (int i = 0; i < 20; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(4f, 4f);
                int dust = Dust.NewDust(NPC.Center, NPC.width, NPC.height, DustID.Electric, velocity.X, velocity.Y, 100, Color.Cyan, 1.2f);
                Main.dust[dust].noGravity = true;
            }
        }

        // === PHASE 4 ATTACK HELPERS ===

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

                if (Main.netMode != NetmodeID.MultiplayerClient)
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
                    angle = MathHelper.Pi - angle;
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

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                    ModContent.ProjectileType<AegisFragment>(),
                    125, 0f);
            }
        }

        public void MirrorAttackD_RapidShards(float verticalProgress, bool shootingUp, bool flipped)
        {
            float angle = flipped ? MathHelper.Pi : 0f;

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
}