using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Common.Input;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Common.Players
{
    public class MasterworkPlayer : ModPlayer
    {
        public bool hasMasterworkSet = false;
        private int abilityCooldown = 0;

        public override void ResetEffects()
        {
            hasMasterworkSet = false;
        }

        public override void PostUpdateEquips()
        {
            if (!hasMasterworkSet) return;
            Player.GetDamage(DamageClass.Generic) *= 0.70f;
            Player.GetModPlayer<PacifistPlayer>().pacifistDamageMultiplier += 0.80f;
        }

        public override void PostUpdate()
        {
            if (!hasMasterworkSet) return;

            // Handle cooldown
            if (abilityCooldown > 0)
            {
                abilityCooldown--;
            }

            // Check for armor ability key press
            if (ModKeybindManager.ArmorAbility != null && 
                ModKeybindManager.ArmorAbility.JustPressed && 
                abilityCooldown <= 0)
            {
                ActivateAbility();
            }
        }

        private void ActivateAbility()
        {
            abilityCooldown = 600;

            // Fire 15 Twirly Whirlies at random velocities
            if (Main.myPlayer == Player.whoAmI)
            {
                for (int i = 0; i < 15; i++)
                {
                    Vector2 randomVelocity = Main.rand.NextVector2Circular(8f, 8f);
                    
                    Projectile.NewProjectile(
                        Player.GetSource_FromThis(),
                        Player.Center,
                        randomVelocity,
                        ModContent.ProjectileType<TwirlyWhirly>(),
                        100, // Base damage
                        2f,  // Knockback
                        Player.whoAmI
                    );
                }
            }

            // Visual/audio feedback
            for (int i = 0; i < 30; i++)
            {
                Color dustColor = Main.rand.Next(3) switch
                {
                    0 => new Color(255, 200, 100), // Gold
                    1 => new Color(200, 200, 200), // Silver
                    _ => new Color(150, 100, 50)   // Bronze
                };

                Vector2 velocity = Main.rand.NextVector2Circular(10f, 10f);
                Dust dust = Dust.NewDustPerfect(Player.Center, DustID.Copper, velocity, 100, dustColor, 1.5f);
                dust.noGravity = true;
            }

            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item37, Player.Center); // Mechanical sound
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!hasMasterworkSet) return;
            if (proj.DamageType != ModContent.GetInstance<Common.PacifistDamageClass>()) return;

            target.AddBuff(ModContent.BuffType<Shred>(), 180);
        }
    }
}