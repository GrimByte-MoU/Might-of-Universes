using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Common;
using System.Collections.Generic;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Common.Players
{
    public class SongOfWinterPlayer : ModPlayer
    {
        public bool hasSongOfWinter = false;
        private int pulseTimer = 0;
        private const int PulseInterval = 180;
        private bool waveActive = false;
        private float currentWaveRadius = 0f;
        private Vector2 waveOrigin = Vector2.Zero; // Store where the wave started
        private const float MaxWaveRadius = 400;
        private const float WaveSpeed = 480 / 60f;
        private HashSet<int> hitNPCs = new HashSet<int>();

        public override void ResetEffects()
        {
            hasSongOfWinter = false;
        }

        public override void PostUpdate()
        {
            if (!hasSongOfWinter)
            {
                waveActive = false;
                return;
            }

            pulseTimer++;

            if (pulseTimer >= PulseInterval && !waveActive)
            {
                pulseTimer = 0;
                StartWinterPulse();
            }

            // Update expanding wave
            if (waveActive)
            {
                UpdateWave();
            }
        }

        private void StartWinterPulse()
        {
            waveActive = true;
            currentWaveRadius = 0f;
            waveOrigin = Player.Center; // Lock the origin position HERE
            hitNPCs.Clear();

            // Center burst visual
            for (int i = 0; i < 20; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(6f, 6f);
                Dust dust = Dust.NewDustPerfect(waveOrigin, DustID.IceTorch, velocity, 100, new Color(100, 200, 255), 1.5f);
                dust.noGravity = true;
            }

            // Sound effect
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item28, waveOrigin);
        }

        private void UpdateWave()
        {
            // Expand the wave
            currentWaveRadius += WaveSpeed;

            // Check if wave reached max radius
            if (currentWaveRadius >= MaxWaveRadius)
            {
                waveActive = false;
                return;
            }

            // Visual expanding ring
            int dustCount = (int)(currentWaveRadius / 50); // More dust as radius grows
            if (dustCount < 50) dustCount = 50;
            
            for (int i = 0; i < dustCount; i++)
            {
                float angle = (i / (float)dustCount) * MathHelper.TwoPi;
                Vector2 position = waveOrigin + new Vector2( // Use waveOrigin instead of Player.Center
                    (float)System.Math.Cos(angle) * currentWaveRadius,
                    (float)System.Math.Sin(angle) * currentWaveRadius
                );

                Color dustColor = new Color(100, 200, 255); // Icy blue
                Dust dust = Dust.NewDustPerfect(position, DustID.IceTorch, Vector2.Zero, 100, dustColor, 1.3f);
                dust.noGravity = true;
                dust.velocity = Vector2.Zero;
            }

            // Add some trailing particles
            if (Main.rand.NextBool(1))
            {
                float angle = Main.rand.NextFloat(0, MathHelper.TwoPi);
                Vector2 position = waveOrigin + new Vector2( // Use waveOrigin
                    (float)System.Math.Cos(angle) * currentWaveRadius,
                    (float)System.Math.Sin(angle) * currentWaveRadius
                );

                Dust dust = Dust.NewDustPerfect(position, DustID.Snow, Vector2.Zero, 100, new Color(200, 230, 255), 1.0f);
                dust.noGravity = true;
            }

            // Calculate pacifist damage with modifiers
            int baseDamage = 40;
            float damageMultiplier = 1f + Player.GetModPlayer<PacifistPlayer>().pacifistDamageMultiplier;
            int finalDamage = (int)(baseDamage * damageMultiplier);

            // Check for enemies at the wave edge (with some thickness)
            float waveThickness = WaveSpeed * 2; // Check slightly ahead and behind the wave front
            
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && !npc.friendly && npc.lifeMax > 5 && !npc.dontTakeDamage && !hitNPCs.Contains(npc.whoAmI))
                {
                    float distance = Vector2.Distance(waveOrigin, npc.Center); // Use waveOrigin
                    
                    // Check if enemy is within the wave's current position
                    if (distance <= currentWaveRadius && distance >= (currentWaveRadius - waveThickness))
                    {
                        // Mark as hit so it doesn't get hit multiple times
                        hitNPCs.Add(npc.whoAmI);
                        
                        // Deal damage
                        Player.ApplyDamageToNPC(npc, finalDamage, 0f, 0, false);
                        
                        // Inflict Paralyze on non-boss enemies for 3 seconds
                        if (!npc.boss)
                        {
                            npc.AddBuff(ModContent.BuffType<Paralyze>(), 180);
                        }

                        // Hit effect on enemy
                        for (int j = 0; j < 8; j++)
                        {
                            Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.IceTorch, 0f, 0f, 100, new Color(100, 200, 255), 1.4f);
                            dust.noGravity = true;
                            dust.velocity = Main.rand.NextVector2Circular(3f, 3f);
                        }
                    }
                }
            }

            // Blue lighting at wave front
            Lighting.AddLight(waveOrigin + new Vector2(currentWaveRadius, 0), 0.3f, 0.6f, 1f); // Use waveOrigin
        }
    }
}