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
            Player.GetDamage(DamageClass.Generic) *= 0.70f;
            Player.GetModPlayer<PacifistPlayer>().pacifistDamageMultiplier += 0.60f;
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
            if (abilityActive)
            {
                abilityDuration--;
                if (abilityDuration <= 0)
                {
                    abilityActive = false;
                    abilityCooldown = 1200;
                    DespawnExtraSuns();
                }
            }
            
            if (abilityCooldown > 0)
            {
                abilityCooldown--;
            }
            if (ModKeybindManager.ArmorAbility != null && 
                ModKeybindManager.ArmorAbility.JustPressed && 
                abilityCooldown <= 0 && 
                !abilityActive)
            {
                ActivateAbility();
            }

            Lighting.AddLight(Player.Center, 1f, 0.7f, 0.3f);

            if (Main.rand.NextBool(3))
            {
                Color[] colors = { Color.Purple, Color.Blue, Color.Magenta, Color.Pink };
                Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, DustID.RainbowMk2, 0f, 0f, 100, Main.rand.Next(colors), 0.8f);
                dust.noGravity = true;
                dust.fadeIn = 0.2f;
            }
        }

        private void ActivateAbility()
        {
            abilityActive = true;
            abilityDuration = 300;
            if (Main.myPlayer == Player.whoAmI)
            {
                for (int i = 0; i < 4; i++)
                {
                    float sunPosition = i + 0.5f;
                    
                    Projectile.NewProjectile(
                        Player.GetSource_FromThis(),
                        Player.Center,
                        Vector2.Zero,
                        ModContent.ProjectileType<VaporwaveSun>(),
                        90,
                        1f,
                        Player.whoAmI,
                        ai0: sunPosition,
                        ai1: 0f,  
                        ai2: 1f 
                    );
                }
            }
            for (int i = 0; i < 30; i++)
            {
                Color dustColor = Main.rand.Next(3) switch
                {
                    0 => new Color(255, 100, 255),
                    1 => new Color(100, 255, 255),
                    _ => new Color(200, 100, 255)
                };
                
                Vector2 velocity = Main.rand.NextVector2Circular(12f, 12f);
                Dust dust = Dust.NewDustPerfect(Player.Center, DustID.RainbowMk2, velocity, 100, dustColor, 1.8f);
                dust.noGravity = true;
            }
            
            Terraria.Audio.SoundEngine.PlaySound(Terraria.ID.SoundID.Item4, Player.Center);
        }

        private void DespawnExtraSuns()
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && 
                    proj.type == ModContent.ProjectileType<VaporwaveSun>() && 
                    proj.owner == Player.whoAmI &&
                    proj.ai[2] == 1f)
                {
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
            bool[] sunExists = new bool[4];
            
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && 
                    proj.type == ModContent.ProjectileType<VaporwaveSun>() && 
                    proj.owner == Player.whoAmI &&
                    proj.ai[2] == 0f)
                {
                    int index = (int)proj.ai[0];
                    if (index >= 0 && index < 4)
                    {
                        sunExists[index] = true;
                    }
                }
            }
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
                            ai0: i,
                            ai1: 0f,
                            ai2: 0f
                        );
                    }
                }
            }
        }
    }
}