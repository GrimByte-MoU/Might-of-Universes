using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Common.Input;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MightofUniverses.Common.Players
{
    public class PrimalPlatePlayer : ModPlayer
    {
        public bool hasPrimalPlateSet = false;
        public float[] spikeRespawnTimers = new float[8]; // Track respawn for each spike
        public float globalSpikeRotation = 0f; // Shared rotation for all spikes

        public override void ResetEffects()
        {
            hasPrimalPlateSet = false;
        }

        public override void PostUpdateEquips()
        {
            if (!hasPrimalPlateSet) return;

            // Apply set bonus stats
            Player.GetDamage(DamageClass.Generic) *= 0.70f; // -30% weapon damage (set bonus)
            Player.GetModPlayer<PacifistPlayer>().pacifistDamageMultiplier += 1.00f; // +100% pacifist damage (set bonus)
            
            // Update global rotation for all spikes
            globalSpikeRotation += 0.05f;
            
            // Maintain spikes
            MaintainSpikes();
        }

        public override void PostUpdate()
        {
            if (!hasPrimalPlateSet)
            {
                // Reset timers when not wearing set
                for (int i = 0; i < 8; i++)
                {
                    spikeRespawnTimers[i] = 0;
                }
                return;
            }

            // Update respawn timers
            for (int i = 0; i < 8; i++)
            {
                if (spikeRespawnTimers[i] > 0)
                {
                    spikeRespawnTimers[i]--;
                }
            }

            // Check for armor ability key press
            if (ModKeybindManager.ArmorAbility != null && 
                ModKeybindManager.ArmorAbility.JustPressed)
            {
                LaunchAllSpikes();
            }
        }

        private void MaintainSpikes()
        {
            // Check which spikes exist
            bool[] spikeExists = new bool[8];
            
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && 
                    proj.type == ModContent.ProjectileType<PrimalSpike>() && 
                    proj.owner == Player.whoAmI)
                {
                    int index = (int)proj.ai[0];
                    if (index >= 0 && index < 8)
                    {
                        spikeExists[index] = true;
                    }
                }
            }

            // Spawn missing spikes (if respawn timer is ready)
            if (Main.myPlayer == Player.whoAmI)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (!spikeExists[i] && spikeRespawnTimers[i] <= 0)
                    {
                        Projectile.NewProjectile(
                            Player.GetSource_FromThis(),
                            Player.Center,
                            Vector2.Zero,
                            ModContent.ProjectileType<PrimalSpike>(),
                            125, // Base damage
                            5f,  // Knockback
                            Player.whoAmI,
                            ai0: i,   // Spike index (0-7)
                            ai1: 0f,  // Launch flag (0 = orbit, 1 = launch)
                            ai2: 0f   // Unused now (using globalSpikeRotation instead)
                        );
                    }
                }
            }
        }

        private void LaunchAllSpikes()
        {
            int launchedCount = 0;

            // Find all active spikes and mark them for launch
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && 
                    proj.type == ModContent.ProjectileType<PrimalSpike>() && 
                    proj.owner == Player.whoAmI &&
                    proj.ai[1] == 0f) // Not already launched
                {
                    // Set launch flag
                    proj.ai[1] = 1f;
                    
                    // Set respawn timer for this spike (5 seconds)
                    int spikeIndex = (int)proj.ai[0];
                    if (spikeIndex >= 0 && spikeIndex < 8)
                    {
                        spikeRespawnTimers[spikeIndex] = 300; // 5 seconds (60 fps * 5)
                    }
                    
                    launchedCount++;
                }
            }

            // Visual/audio feedback if any spikes were launched
            if (launchedCount > 0)
            {
                for (int i = 0; i < 20; i++)
                {
                    Color dustColor = Main.rand.Next(2) switch
                    {
                        0 => new Color(139, 90, 43),
                        _ => new Color(218, 165, 32)
                    };
                    
                    Vector2 velocity = Main.rand.NextVector2Circular(8f, 8f);
                    Dust dust = Dust.NewDustPerfect(Player.Center, DustID.Bone, velocity, 100, dustColor, 1.5f);
                    dust.noGravity = true;
                }
                
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item71, Player.Center); // Primal/earth sound
            }
        }
    }
}