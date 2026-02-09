using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Projectiles.EnemyProjectiles;
using System;

namespace MightofUniverses.Content.NPCs.Bosses.Aegon
{
    public class AegonPatienceMirage : ModNPC
    {
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
            NPC.velocity = Vector2.Zero;

            if (Main.rand.NextBool(5))
            {
                int dust = Dust.NewDust(NPC.Center, NPC.width, NPC.height, DustID.Electric, 0f, 0f, 100, Color.Cyan, 0.8f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.3f;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(
                "MightofUniverses/Content/NPCs/Bosses/Aegon/Aegon").Value;
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

                spriteBatch.Draw(texture, drawPos + offset, null, Color.Cyan * 0.3f,
                    NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0f);
            }
            return false;
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

        // === PHASE 5 ATTACK HELPERS ===

        public void FireJungleNeedleSpread(Vector2 arenaCenter)
        {
            int spreadCount = 4;
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
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity,
                        ModContent.ProjectileType<JungleNeedle>(),
                        95, 0f);
                }
            }
        }

        public void FireCrimruptionBolts(Vector2 arenaCenter)
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

        public void FireOceanSphere(Vector2 arenaCenter)
        {
            float angle = (arenaCenter - NPC.Center).ToRotation();

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

        public void FireHallowedSpears(Vector2 playerCenter)
        {
            int spreadCount = 3;
            float baseAngle = (playerCenter - NPC.Center).ToRotation();

            for (int i = 0; i < spreadCount; i++)
            {
                float offsetAngle = MathHelper.Lerp(-0.35f, 0.35f, i / (float)(spreadCount - 1));
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
    }
}