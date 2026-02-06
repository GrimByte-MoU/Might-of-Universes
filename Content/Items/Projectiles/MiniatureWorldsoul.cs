using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Common.Players;
using System;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class MiniatureWorldsoul : ModProjectile
    {

        private ref float SwayOffset => ref Projectile.localAI[0];
        private int shootTimer = 0;
        private NPC targetNPC = null;

        public override void SetStaticDefaults()
        {
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.friendly = false;
            Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 18000;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.minionSlots = 0f;
            Projectile.netImportant = true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (!player.active || player.dead)
            {
                Projectile.Kill();
                return;
            }

            var armorPlayer = player.GetModPlayer<TerraiumArmorPlayer>();
            if (!armorPlayer.summonerSetBonus)
            {
                Projectile.Kill();
                return;
            }

            Projectile.timeLeft = 18000;

            targetNPC = FindClosestEnemy(800f);

            if (targetNPC == null)
            {
                DormantBehavior(player);
            }
            else
            {
                ShootingBehavior();
            }

            Projectile.rotation += 0.15f;

            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(
                    Projectile.position, 
                    Projectile.width, 
                    Projectile.height, 
                    DustID.TerraBlade, 
                    0, 0, 100, 
                    Color.Cyan, 
                    1.5f
                );
                dust.noGravity = true;
                dust.velocity *= 0.3f;
            }

            Color lightColor = targetNPC != null ? Color.Yellow : Color.Lime;
            Lighting.AddLight(Projectile.Center, lightColor.R / 255f * 0.5f, lightColor.G / 255f * 0.8f, lightColor.B / 255f * 0.8f);
        }

        private void DormantBehavior(Player player)
        {
            SwayOffset += 0.08f;
            float swayX = (float)Math.Sin(SwayOffset) * 20f;
            float swayY = (float)Math.Sin(SwayOffset * 0.7f) * 10f;

            Vector2 idlePosition = player.Center + new Vector2((-70 * player.direction) + swayX, -60 + swayY);
            Vector2 direction = idlePosition - Projectile.Center;
            float distance = direction.Length();

            if (distance > 30f)
            {
                direction.Normalize();
                Projectile.velocity = direction * Math.Min(distance * 0.2f, 12f);
            }
            else
            {
                Projectile.velocity *= 0.90f;
            }

            shootTimer = 0;
        }

        private void ShootingBehavior()
        {
            if (targetNPC == null || !targetNPC.active)
                return;

            SwayOffset += 0.08f;
            
            float circleRadius = 200f;
            float circleX = (float)Math.Cos(SwayOffset) * circleRadius;
            float circleY = (float)Math.Sin(SwayOffset) * (circleRadius * 0.6f);

            Vector2 hoverPosition = targetNPC.Center + new Vector2(circleX, -100 + circleY);
            Vector2 direction = hoverPosition - Projectile.Center;
            float distance = direction.Length();

            if (distance > 40f)
            {
                direction.Normalize();
                Projectile.velocity = direction * Math.Min(distance * 0.15f, 14f);
            }
            else
            {
                Projectile.velocity *= 0.85f;
            }

            shootTimer++;
            if (shootTimer >= 15)
            {
                shootTimer = 0;
                FireWorldsoulBolt();
            }
        }

        private void FireWorldsoulBolt()
        {
            if (Main.myPlayer != Projectile.owner || targetNPC == null)
                return;

            Vector2 targetVelocity = targetNPC.velocity * 10f;
            Vector2 predictedPosition = targetNPC.Center + targetVelocity;
            Vector2 direction = (predictedPosition - Projectile.Center).SafeNormalize(Vector2.UnitX);
            Vector2 velocity = direction * 18f;

            Projectile.NewProjectile(
                Projectile.GetSource_FromThis(),
                Projectile.Center,
                velocity,
                ModContent.ProjectileType<MiniWorldsoulBolt>(),
                Projectile.damage,
                2f,
                Projectile.owner
            );

            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item8, Projectile.Center);

            for (int i = 0; i < 5; i++)
            {
                Dust dust = Dust.NewDustDirect(
                    Projectile.Center,
                    0, 0,
                    DustID.TerraBlade,
                    direction.X * 3f,
                    direction.Y * 3f,
                    100,
                    Color.Yellow,
                    1.8f
                );
                dust.noGravity = true;
            }
        }

        private NPC FindClosestEnemy(float maxDistance)
        {
            NPC closestNPC = null;
            float closestDist = maxDistance;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                
                if (!npc.active || npc.friendly || npc.dontTakeDamage)
                    continue;
                
                if (npc.type == NPCID.TargetDummy)
                    continue;

                float distance = Vector2.Distance(Projectile.Center, npc.Center);
                if (distance < closestDist)
                {
                    closestDist = distance;
                    closestNPC = npc;
                }
            }

            return closestNPC;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            if (targetNPC != null)
                return Color.Lerp(Color.Lime, Color.Yellow, 0.6f);
            
            return Color.Lime;
        }
    }
}