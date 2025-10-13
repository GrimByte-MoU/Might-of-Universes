using Terraria;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Common.Input;
using Microsoft.Xna.Framework;

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
            
            // Spawn/maintain the 4 orbiting suns
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
                }
            }

            // Handle cooldown
            if (abilityCooldown > 0)
            {
                abilityCooldown--;
            }

            // Check for armor ability key press - use ModKeybindManager
            if (ModKeybindManager.ArmorAbility.JustPressed && abilityCooldown <= 0 && !abilityActive)
            {
                ActivateAbility();
            }
        }

        private void ActivateAbility()
        {
            abilityActive = true;
            abilityDuration = 5 * 60; // 5 seconds
            
            // Visual/audio feedback
            for (int i = 0; i < 20; i++)
            {
                Color dustColor = Main.rand.Next(3) switch
                {
                    0 => new Color(255, 100, 255), // Hot pink
                    1 => new Color(100, 255, 255), // Cyan
                    _ => new Color(200, 100, 255)  // Purple
                };
                
                Vector2 velocity = Main.rand.NextVector2Circular(8f, 8f);
                Dust dust = Dust.NewDustPerfect(Player.Center, DustID.RainbowMk2, velocity, 100, dustColor, 1.5f);
                dust.noGravity = true;
            }
            
            Terraria.Audio.SoundEngine.PlaySound(Terraria.ID.SoundID.Item4, Player.Center);
        }

        private void MaintainSuns()
        {
            int desiredSuns = 4;
            int activeSuns = 0;
            
            // Count existing suns
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.type == ModContent.ProjectileType<VaporwaveSun>() && proj.owner == Player.whoAmI)
                {
                    activeSuns++;
                }
            }

            // Spawn missing suns
            if (Main.myPlayer == Player.whoAmI)
            {
                while (activeSuns < desiredSuns)
                {
                    Projectile.NewProjectile(
                        Player.GetSource_FromThis(),
                        Player.Center,
                        Vector2.Zero,
                        ModContent.ProjectileType<VaporwaveSun>(),
                        45,
                        1f,
                        Player.whoAmI,
                        ai0: activeSuns, // Sun index (0-3)
                        ai1: 0f          // Current rotation angle
                    );
                    activeSuns++;
                }
            }
        }
    }
}