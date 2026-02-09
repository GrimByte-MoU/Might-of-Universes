using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.ID;
using System;
using Terraria.Audio;

namespace MightofUniverses.Common.Players
{
    public class SatelliteDefenseGridPlayer : ModPlayer
    {
        public bool hasSatelliteGrid;
        public bool dashing;
        public int dashTimer;
        public int dashDirection;
        public int dashCooldown;
        public bool regenActive;
        public int regenTimer;
        public int healCooldown;
        public int emergencyHealCooldown;
        private const float GREEN_RADIUS = 30f * 16f;
        private const int GREEN_BASE_DAMAGE = 50;
        private const int GREEN_DAMAGE_PER_ENEMY = 10;
        private const float GREEN_DAMAGE_INTERVAL = 20f;
        private int greenDamageTimer;
        private const float YELLOW_RADIUS = 20f * 16f;
        private const int YELLOW_BASE_DAMAGE = 75;
        private const int YELLOW_DAMAGE_PER_ENEMY = 15;
        private const float YELLOW_DAMAGE_INTERVAL = 10f;
        private int yellowDamageTimer;
        private const float RED_RADIUS = 10f * 16f;
        private const int RED_BASE_DAMAGE = 100;
        private const int RED_DAMAGE_PER_ENEMY = 25;
        private const float RED_DAMAGE_INTERVAL = 5f;
        private int redDamageTimer;
        private bool rightKeyDown;
        private bool leftKeyDown;
        private int rightKeyPressTime;
        private int leftKeyPressTime;
        private int rightKeyReleaseTime;
        private int leftKeyReleaseTime;
        private int rightTapCount;
        private int leftTapCount;
        private const int TAP_WINDOW = 15;
        private const int DASH_DURATION = 20;
        private const int DASH_COOLDOWN = 200;
        private const float DASH_VELOCITY = 15f;
        private const int DASH_DAMAGE = 100;

        public override void ResetEffects()
        {
            hasSatelliteGrid = false;
            
            if (healCooldown > 0) healCooldown--;
            if (emergencyHealCooldown > 0) emergencyHealCooldown--;
        }

        private void StartDash(int direction)
        {
            dashing = true;
            dashTimer = DASH_DURATION;
            dashDirection = direction;
            dashCooldown = DASH_COOLDOWN;
            SoundEngine.PlaySound(SoundID.Item74, Player.Center);
        }

        public override void PreUpdate()
        {
            if (dashCooldown > 0)
            {
                dashCooldown--;
            }

            if (dashTimer > 0)
            {
                dashTimer--;
                if (dashTimer <= 0)
                {
                    dashing = false;
                }
            }

            if (hasSatelliteGrid)
            {
                HandleDoubleTapDetection();
            }
            
            if (dashing)
            {
                Player.velocity.X = DASH_VELOCITY * dashDirection;
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
                        npc.StrikeNPC(new NPC.HitInfo
                        {
                            Damage = DASH_DAMAGE,
                            Knockback = 8f,
                            HitDirection = dashDirection
                        });

                        SoundEngine.PlaySound(SoundID.Item14, npc.Center);
                        for (int d = 0; d < 10; d++)
                        {
                            Dust.NewDust(
                                npc.position,
                                npc.width,
                                npc.height,
                                DustID.GreenTorch,
                                0f,
                                0f,
                                100,
                                default,
                                1.5f
                            );
                        }
                    }
                }
            }
        }
        
        private void HandleDoubleTapDetection()
        {
            bool rightKeyPressed = Player.controlRight && !Player.controlLeft;
            if (rightKeyPressed && !rightKeyDown)
            {
                rightKeyDown = true;
                rightKeyPressTime = 0;

                if (rightKeyReleaseTime < TAP_WINDOW)
                {
                    rightTapCount++;
                    if (rightTapCount >= 2 && dashCooldown <= 0 && !dashing)
                    {
                        StartDash(1);
                        rightTapCount = 0;
                    }
                }
                else
                {
                    rightTapCount = 1;
                }
            }
            else if (!rightKeyPressed && rightKeyDown)
            {
                rightKeyDown = false;
                rightKeyReleaseTime = 0;
            }

            bool leftKeyPressed = Player.controlLeft && !Player.controlRight;
            if (leftKeyPressed && !leftKeyDown)
            {
                leftKeyDown = true;
                leftKeyPressTime = 0;

                if (leftKeyReleaseTime < TAP_WINDOW)
                {
                    leftTapCount++;
                    if (leftTapCount >= 2 && dashCooldown <= 0 && !dashing)
                    {
                        StartDash(-1);
                        leftTapCount = 0;
                    }
                }
                else
                {
                    leftTapCount = 1;
                }
            }
            else if (!leftKeyPressed && leftKeyDown)
            {
                leftKeyDown = false;
                leftKeyReleaseTime = 0;
            }

            if (rightKeyDown)
            {
                rightKeyPressTime++;
            }
            else
            {
                rightKeyReleaseTime++;
            }
            
            if (leftKeyDown)
            {
                leftKeyPressTime++;
            }
            else
            {
                leftKeyReleaseTime++;
            }

            if (rightKeyReleaseTime > TAP_WINDOW * 2)
            {
                rightTapCount = 0;
            }
            
            if (leftKeyReleaseTime > TAP_WINDOW * 2)
            {
                leftTapCount = 0;
            }
        }

        public override void PostUpdate()
        {
            if (!hasSatelliteGrid) return;
            
            HandleRing(GREEN_RADIUS, GREEN_BASE_DAMAGE, GREEN_DAMAGE_PER_ENEMY, ref greenDamageTimer, Color.Green, DustID.GreenTorch, GREEN_DAMAGE_INTERVAL);
            HandleRing(YELLOW_RADIUS, YELLOW_BASE_DAMAGE, YELLOW_DAMAGE_PER_ENEMY, ref yellowDamageTimer, Color.Yellow, DustID.YellowTorch, YELLOW_DAMAGE_INTERVAL);
            HandleRing(RED_RADIUS, RED_BASE_DAMAGE, RED_DAMAGE_PER_ENEMY, ref redDamageTimer, Color.Red, DustID.RedTorch, RED_DAMAGE_INTERVAL);
        }

        private void HandleRing(float radius, int baseDamage, int damagePerEnemy, ref int timer, Color color, int dustType, float damageInterval)
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

            timer++;
            if (timer >= damageInterval)
            {
                timer = 0;
                List<NPC> affectedNPCs = new List<NPC>();
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.active && !npc.friendly && Vector2.Distance(npc.Center, Player.Center) <= radius)
                    {
                        affectedNPCs.Add(npc);
                    }
                }

                if (affectedNPCs.Count > 0)
                {
                    int damagePerNPC = baseDamage + (affectedNPCs.Count * damagePerEnemy);
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
}


