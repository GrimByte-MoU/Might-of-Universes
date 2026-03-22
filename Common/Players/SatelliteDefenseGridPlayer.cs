using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.ID;
using System;
using Terraria.Audio;
using MightofUniverses.Common;

namespace MightofUniverses.Common.Players
{
    public class SatelliteDefenseGridPlayer : ModPlayer
    {
        public const int DashRight = 2;
        public const int DashLeft = 3;
        public const int DashCooldown = 50;
        public const int DashDuration = 30;
        public const float DashVelocity = 17f;
        public const int DashDamage = 150;

        public bool hasSatelliteGrid;
        public int DashDir = -1;
        public int DashDelay = 0;
        public int DashTimer = 0;
        public bool regenActive;
        public int regenTimer;
        public int healCooldown;
        public int emergencyHealCooldown;

        private const float GREEN_RADIUS = 30f * 16f;
        private const int GREEN_BASE_DAMAGE = 50;
        private const int GREEN_DAMAGE_PER_ENEMY = 5;
        private const float GREEN_DAMAGE_INTERVAL = 40f;
        private int greenDamageTimer;

        private const float YELLOW_RADIUS = 20f * 16f;
        private const int YELLOW_BASE_DAMAGE = 75;
        private const int YELLOW_DAMAGE_PER_ENEMY = 10;
        private const float YELLOW_DAMAGE_INTERVAL = 30f;
        private int yellowDamageTimer;

        private const float RED_RADIUS = 10f * 16f;
        private const int RED_BASE_DAMAGE = 100;
        private const int RED_DAMAGE_PER_ENEMY = 15;
        private const float RED_DAMAGE_INTERVAL = 20f;
        private int redDamageTimer;

        public override void ResetEffects()
        {
            hasSatelliteGrid = false;

            if (healCooldown > 0) healCooldown--;
            if (emergencyHealCooldown > 0) emergencyHealCooldown--;

            if (Player.controlRight && Player.releaseRight && Player.doubleTapCardinalTimer[DashRight] < 15)
            {
                DashDir = DashRight;
            }
            else if (Player.controlLeft && Player.releaseLeft && Player.doubleTapCardinalTimer[DashLeft] < 15)
            {
                DashDir = DashLeft;
            }
            else
            {
                DashDir = -1;
            }
        }

        public override void PreUpdateMovement()
        {
            if (CanUseDash() && DashDir != -1 && DashDelay == 0)
            {
                Vector2 newVelocity = Player.velocity;

                switch (DashDir)
                {
                    case DashLeft when Player.velocity.X > -DashVelocity:
                    case DashRight when Player.velocity.X < DashVelocity:
                        float dashDirection = DashDir == DashRight ? 1 : -1;
                        newVelocity.X = dashDirection * DashVelocity;
                        break;
                    default:
                        return;
                }

                DashDelay = DashCooldown;
                DashTimer = DashDuration;
                Player.velocity = newVelocity;

                SoundEngine.PlaySound(SoundID.Item74, Player.Center);
            }

            if (DashDelay > 0)
                DashDelay--;

            if (DashTimer > 0)
            {
                Player.eocDash = DashTimer;
                Player.armorEffectDrawShadowEOCShield = true;
                Player.immune = true;
                Player.immuneTime = 6;

                for (int i = 0; i < 3; i++)
                {
                    Dust dust = Dust.NewDustDirect(
                        Player.position,
                        Player.width,
                        Player.height,
                        DustID.GreenTorch,
                        0f,
                        0f,
                        100,
                        default,
                        1.8f
                    );
                    dust.noGravity = true;
                    dust.velocity *= 0.7f;

                    Dust dust2 = Dust.NewDustDirect(
                        Player.position,
                        Player.width,
                        Player.height,
                        DustID.GreenTorch,
                        0f,
                        0f,
                        100,
                        default,
                        1.2f
                    );
                    dust2.noGravity = true;
                    dust2.velocity *= 0.5f;
                }

                Rectangle hitbox = Player.Hitbox;
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.active && !npc.friendly && hitbox.Intersects(npc.Hitbox))
                    {
                        int baseDamage = DashDamage;
                        float damageMultiplier = Player.GetDamage<PacifistDamageClass>().Additive + Player.GetDamage<PacifistDamageClass>().Multiplicative - 1f;
                        int finalDamage = (int)(baseDamage * damageMultiplier);

                        npc.StrikeNPC(new NPC.HitInfo
                        {
                            Damage = finalDamage,
                            Knockback = 8f,
                            HitDirection = DashDir == DashRight ? 1 : -1
                        });

                        SoundEngine.PlaySound(SoundID.Item14, npc.Center);

                        for (int d = 0; d < 20; d++)
                        {
                            Dust dust = Dust.NewDustDirect(
                                npc.position,
                                npc.width,
                                npc.height,
                                DustID.GreenTorch,
                                0f,
                                0f,
                                100,
                                default,
                                2f
                            );
                            dust.noGravity = true;
                            dust.velocity *= 2f;
                        }
                    }
                }

                DashTimer--;
            }
        }

        private bool CanUseDash()
        {
            return hasSatelliteGrid
                && Player.dashType == 0
                && !Player.setSolar
                && !Player.mount.Active;
        }

        public override void PostUpdate()
        {
            if (!hasSatelliteGrid) return;

            DrawRing(GREEN_RADIUS, Color.Green, DustID.GreenTorch);
            DrawRing(YELLOW_RADIUS, Color.Yellow, DustID.YellowTorch);
            DrawRing(RED_RADIUS, Color.Red, DustID.RedTorch);

            greenDamageTimer++;
            yellowDamageTimer++;
            redDamageTimer++;

            if (greenDamageTimer >= GREEN_DAMAGE_INTERVAL)
            {
                greenDamageTimer = 0;
                DamageEnemiesInRing(YELLOW_RADIUS, GREEN_RADIUS, GREEN_BASE_DAMAGE, GREEN_DAMAGE_PER_ENEMY, DustID.GreenTorch);
            }

            if (yellowDamageTimer >= YELLOW_DAMAGE_INTERVAL)
            {
                yellowDamageTimer = 0;
                DamageEnemiesInRing(RED_RADIUS, YELLOW_RADIUS, YELLOW_BASE_DAMAGE, YELLOW_DAMAGE_PER_ENEMY, DustID.YellowTorch);
            }

            if (redDamageTimer >= RED_DAMAGE_INTERVAL)
            {
                redDamageTimer = 0;
                DamageEnemiesInRing(0f, RED_RADIUS, RED_BASE_DAMAGE, RED_DAMAGE_PER_ENEMY, DustID.RedTorch);
            }
        }

        private void DrawRing(float radius, Color color, int dustType)
        {
            const int NUM_POINTS = 120;
            for (int i = 0; i < NUM_POINTS; i++)
            {
                float angle = (float)(i * (2 * Math.PI) / NUM_POINTS);
                Vector2 offset = new Vector2(
                    (float)Math.Cos(angle) * radius,
                    (float)Math.Sin(angle) * radius
                );
                Vector2 dustPos = Player.Center + offset;
                Dust dust = Dust.NewDustPerfect(
                    dustPos,
                    dustType,
                    Vector2.Zero,
                    0,
                    color,
                    1f
                );
                dust.noGravity = true;
                dust.noLight = false;
            }
        }

        private void DamageEnemiesInRing(float innerRadius, float outerRadius, int baseDamage, int damagePerEnemy, int dustType)
        {
            List<NPC> affectedNPCs = new List<NPC>();
            
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && !npc.friendly)
                {
                    float distance = Vector2.Distance(npc.Center, Player.Center);
                    if (distance > innerRadius && distance <= outerRadius)
                    {
                        affectedNPCs.Add(npc);
                    }
                }
            }

            if (affectedNPCs.Count > 0)
            {
                int baseDamageCalculated = baseDamage + (affectedNPCs.Count * damagePerEnemy);
                float damageMultiplier = Player.GetDamage<PacifistDamageClass>().Additive + Player.GetDamage<PacifistDamageClass>().Multiplicative - 1f;
                int damagePerNPC = (int)(baseDamageCalculated * damageMultiplier);

                foreach (NPC npc in affectedNPCs)
                {
                    npc.StrikeNPC(new NPC.HitInfo
                    {
                        Damage = damagePerNPC,
                        Knockback = 0f,
                        HitDirection = 0
                    });
                    
                    for (int d = 0; d < 3; d++)
                    {
                        Dust.NewDust(
                            npc.position,
                            npc.width,
                            npc.height,
                            dustType,
                            0f,
                            0f,
                            100,
                            default,
                            1f
                        );
                    }
                }
            }
        }
    }
}