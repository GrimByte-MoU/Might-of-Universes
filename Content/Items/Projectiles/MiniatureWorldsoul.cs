using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Common.Players; // ADD THIS LINE!!!
using System;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class MiniatureWorldsoul : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.BabyEater;

        private enum AIState
        {
            Dormant,
            Ramming,
            Shooting
        }

        private AIState CurrentState
        {
            get => (AIState)Projectile.ai[0];
            set => Projectile.ai[0] = (float)value;
        }

        private ref float StateTimer => ref Projectile.ai[1];
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
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 18000;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.minionSlots = 0f;
            Projectile.netImportant = true;
            
            // Use ID Static immunity
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 20;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (!player.active || player. dead)
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

            // Find target
            targetNPC = FindClosestEnemy(600f);

            if (targetNPC == null)
            {
                CurrentState = AIState.Dormant;
                StateTimer = 0;
                DormantBehavior(player);
            }
            else
            {
                // State switching logic
                StateTimer++;

                if (StateTimer >= 300) // Switch states every 5 seconds
                {
                    StateTimer = 0;
                    shootTimer = 0;
                    
                    if (CurrentState == AIState.Ramming)
                        CurrentState = AIState.Shooting;
                    else
                        CurrentState = AIState.Ramming;
                }

                // Execute current state
                if (CurrentState == AIState.Ramming)
                {
                    RammingBehavior();
                }
                else if (CurrentState == AIState.Shooting)
                {
                    ShootingBehavior();
                }
            }

            // Visual effects
            Projectile.rotation += 0.15f;

            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.TerraBlade, 0, 0, 100, Color. Cyan, 1.5f);
                dust. noGravity = true;
                dust.velocity *= 0.3f;
            }

            // Different color based on state
            Color lightColor = CurrentState == AIState.Ramming ? Color.Red : 
                              CurrentState == AIState.Shooting ? Color.Yellow : 
                              Color.Cyan;
            
            Lighting.AddLight(Projectile.Center, lightColor.R / 255f * 0.5f, lightColor.G / 255f * 0.8f, lightColor.B / 255f * 0.8f);
        }

        private void DormantBehavior(Player player)
        {
            SwayOffset += 0.08f;
            float swayX = (float)Math.Sin(SwayOffset) * 20f;
            float swayY = (float)Math.Sin(SwayOffset * 0.7f) * 10f;

            // Position behind and above player
            Vector2 idlePosition = player.Center + new Vector2((-70 * player.direction) + swayX, -40 + swayY);
            Vector2 direction = (idlePosition - Projectile. Center);
            float distance = direction.Length();

            if (distance > 30f)
            {
                direction. Normalize();
                Projectile.velocity = direction * Math.Min(distance * 0.2f, 12f);
            }
            else
            {
                Projectile.velocity *= 0.90f;
            }
        }

        private void RammingBehavior()
        {
            if (targetNPC == null || ! targetNPC.active)
                return;

            SwayOffset += 0.12f;
            
            // Create figure-8 swiping pattern
            float swayX = (float)Math.Sin(SwayOffset) * 50f;
            float swayY = (float)Math.Sin(SwayOffset * 2f) * 40f;

            Vector2 targetPos = targetNPC.Center + new Vector2(swayX, swayY);
            Vector2 direction = (targetPos - Projectile.Center).SafeNormalize(Vector2.UnitX);
            
            // More aggressive movement
            float speed = 20f;
            Projectile.velocity = Vector2.Lerp(Projectile.velocity, direction * speed, 0.12f);
        }

        private void ShootingBehavior()
        {
            if (targetNPC == null || !targetNPC.active)
                return;

            SwayOffset += 0.08f;
            
            // Circle around target
            float circleX = (float)Math.Sin(SwayOffset) * 80f;
            float circleY = (float)Math.Cos(SwayOffset * 0.6f) * 60f;

            Vector2 hoverPosition = targetNPC.Center + new Vector2(circleX, -140 + circleY);
            Vector2 direction = (hoverPosition - Projectile.Center);
            float distance = direction.Length();

            if (distance > 40f)
            {
                direction.Normalize();
                Projectile.velocity = direction * Math. Min(distance * 0.15f, 14f);
            }
            else
            {
                Projectile.velocity *= 0.85f;
            }

            // Fire bolts
            shootTimer++;
            if (shootTimer >= 15) // Fire every 0.25 seconds (4 times per second)
            {
                shootTimer = 0;
                FireWorldsoulBolt();
            }
        }

        private void FireWorldsoulBolt()
        {
            if (Main.myPlayer != Projectile.owner || targetNPC == null)
                return;

            Vector2 direction = (targetNPC. Center - Projectile.Center).SafeNormalize(Vector2.UnitX);
            Vector2 velocity = direction * 16f;

            Projectile. NewProjectile(
                Projectile.GetSource_FromThis(),
                Projectile.Center,
                velocity,
                ModContent.ProjectileType<MiniWorldsoulBolt>(),
                Projectile.damage,
                2f,
                Projectile. owner
            );

            // Visual/audio feedback
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item8, Projectile.Center);
        }

        private NPC FindClosestEnemy(float maxDistance)
        {
            NPC closestNPC = null;
            float closestDist = maxDistance;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                
                if (! npc.active || npc. friendly || npc. dontTakeDamage)
                    continue;
                
                if (npc.type == NPCID. TargetDummy)
                    continue;

                float distance = Vector2.Distance(Projectile. Center, npc.Center);
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
            // Color based on state
            if (CurrentState == AIState. Ramming)
                return Color. Lerp(Color. Cyan, Color.Red, 0.5f);
            else if (CurrentState == AIState. Shooting)
                return Color. Lerp(Color. Cyan, Color.Yellow, 0.5f);
            
            return Color.Cyan;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target. AddBuff(ModContent.BuffType<TerrasRend>(), 180);

            for (int i = 0; i < 15; i++)
            {
                Dust dust = Dust.NewDustDirect(target.position, target.width, target.height, DustID.TerraBlade, 0, 0, 100, Color. Cyan, 2.0f);
                dust. noGravity = true;
                dust.velocity = new Vector2(Main.rand.NextFloat(-4f, 4f), Main.rand.NextFloat(-4f, 4f));
            }
        }
    }
}