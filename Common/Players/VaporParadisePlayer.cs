using Terraria;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Common.Input;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace MightofUniverses.Common.Players
{
    public class VaporParadisePlayer : ModPlayer
    {
        public bool hasVaporParadiseSet = false;
        public bool abilityActive = false;
        private int abilityDuration = 0;
        private int abilityCooldown = 0;

        public override void ResetEffects()
        {
            hasVaporParadiseSet = false;
        }

        public override void PostUpdateEquips()
        {
            if (!hasVaporParadiseSet) return;

            // Apply set bonus stats
            Player.GetDamage(DamageClass.Generic) *= 0.70f; // -30% weapon damage (set bonus)
            Player.GetModPlayer<PacifistPlayer>().pacifistDamageMultiplier += 0.60f; // +60% pacifist damage (set bonus)
            
            // Spawn/maintain the suns
            MaintainSuns();
        }

        public override void PostUpdate()
        {
            if (!hasVaporParadiseSet)
            {
                abilityActive = false;
                abilityDuration = 0;
                return;
            }

            // Handle ability duration
            if (abilityActive)
            {
                abilityDuration--;
                if (abilityDuration <= 0)
                {
                    abilityActive = false;
                    abilityCooldown = 20 * 60; // 20 seconds
                    
                    // Kill the extra 4 suns when ability ends
                    DespawnExtraSuns();
                }
            }

            // Handle cooldown
            if (abilityCooldown > 0)
            {
                abilityCooldown--;
            }

            // Check for armor ability key press
            if (ModKeybindManager.ArmorAbility != null && 
                ModKeybindManager.ArmorAbility.JustPressed && 
                abilityCooldown <= 0 && 
                !abilityActive)
            {
                ActivateAbility();
            }
        }

        private void ActivateAbility()
        {
            abilityActive = true;
            abilityDuration = 5 * 60; // 5 seconds
            
            // Spawn 4 extra suns immediately with offset indices
            if (Main.myPlayer == Player.whoAmI)
            {
                for (int i = 0; i < 4; i++)
                {
                    // Offset by 0.5 to interleave between base suns
                    // Base suns: 0, 1, 2, 3
                    // Extra suns: 0.5, 1.5, 2.5, 3.5 (stored as 0.5, 1.5, 2.5, 3.5)
                    float sunPosition = i + 0.5f;
                    
                    Projectile.NewProjectile(
                        Player.GetSource_FromThis(),
                        Player.Center,
                        Vector2.Zero,
                        ModContent.ProjectileType<VaporwaveSun>(),
                        90, // Boosted damage
                        1f,
                        Player.whoAmI,
                        ai0: sunPosition, // Interleaved position (0.5, 1.5, 2.5, 3.5)
                        ai1: 0f,          // Current rotation angle
                        ai2: 1f           // Mark as ability sun (will be despawned)
                    );
                }
            }
            
            // Visual/audio feedback
            for (int i = 0; i < 30; i++)
            {
                Color dustColor = Main.rand.Next(3) switch
                {
                    0 => new Color(255, 100, 255), // Hot pink
                    1 => new Color(100, 255, 255), // Cyan
                    _ => new Color(200, 100, 255)  // Purple
                };
                
                Vector2 velocity = Main.rand.NextVector2Circular(12f, 12f);
                Dust dust = Dust.NewDustPerfect(Player.Center, DustID.RainbowMk2, velocity, 100, dustColor, 1.8f);
                dust.noGravity = true;
            }
            
            Terraria.Audio.SoundEngine.PlaySound(Terraria.ID.SoundID.Item4, Player.Center);
        }

        private void DespawnExtraSuns()
        {
            // Kill all suns marked as ability suns (ai[2] == 1)
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && 
                    proj.type == ModContent.ProjectileType<VaporwaveSun>() && 
                    proj.owner == Player.whoAmI &&
                    proj.ai[2] == 1f) // Only kill ability suns
                {
                    // Visual despawn effect
                    for (int d = 0; d < 10; d++)
                    {
                        Color dustColor = Main.rand.Next(3) switch
                        {
                            0 => new Color(255, 100, 255),
                            1 => new Color(100, 255, 255),
                            _ => new Color(200, 100, 255)
                        };
                        
                        Dust dust = Dust.NewDustDirect(proj.position, proj.width, proj.height, DustID.RainbowMk2, 0f, 0f, 100, dustColor, 1.2f);
                        dust.noGravity = true;
                        dust.velocity = Main.rand.NextVector2Circular(4f, 4f);
                    }
                    
                    proj.Kill();
                }
            }
        }

        private void MaintainSuns()
        {
            // Check which base sun indices exist (0, 1, 2, 3)
            bool[] sunExists = new bool[4];
            
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && 
                    proj.type == ModContent.ProjectileType<VaporwaveSun>() && 
                    proj.owner == Player.whoAmI &&
                    proj.ai[2] == 0f) // Only count base suns
                {
                    int index = (int)proj.ai[0];
                    if (index >= 0 && index < 4)
                    {
                        sunExists[index] = true;
                    }
                }
            }

            // Spawn missing base suns
            if (Main.myPlayer == Player.whoAmI)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (!sunExists[i])
                    {
                        Projectile.NewProjectile(
                            Player.GetSource_FromThis(),
                            Player.Center,
                            Vector2.Zero,
                            ModContent.ProjectileType<VaporwaveSun>(),
                            45,
                            1f,
                            Player.whoAmI,
                            ai0: i,   // Fixed index (0, 1, 2, 3)
                            ai1: 0f,  // Current rotation angle
                            ai2: 0f   // Base sun (persistent)
                        );
                    }
                }
            }
        }
    }
}