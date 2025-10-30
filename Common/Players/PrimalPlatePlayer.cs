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
        public float[] spikeRespawnTimers = new float[8];
        public float globalSpikeRotation = 0f;

        public override void ResetEffects()
        {
            hasPrimalPlateSet = false;
        }

        public override void PostUpdateEquips()
        {
            if (!hasPrimalPlateSet) return;
            Player.GetDamage(DamageClass.Generic) *= 0.70f;
            Player.GetModPlayer<PacifistPlayer>().pacifistDamageMultiplier += 1.00f;
            globalSpikeRotation += 0.05f;
            MaintainSpikes();
        }

        public override void PostUpdate()
        {
            if (!hasPrimalPlateSet) return;
            for (int i = 0; i < 8; i++)
            {
                if (spikeRespawnTimers[i] > 0)
                {
                    spikeRespawnTimers[i]--;
                }
            }
            if (ModKeybindManager.ArmorAbility != null && 
                ModKeybindManager.ArmorAbility.JustPressed)
            {
                LaunchAllSpikes();
            }

            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, DustID.Bone, 0f, 0f, 100, default, 0.8f);
                dust.noGravity = true;
                dust.fadeIn = 0.2f;
            }

            if (Main.rand.NextBool(4))
            {
                int dustType = Main.rand.NextBool() ? DustID.AmberBolt : DustID.Shadowflame;
                Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, dustType, 0f, 0f, 100, default, 0.8f);
                dust.noGravity = true;
                dust.fadeIn = 0.2f;
            }

            if (Main.rand.NextBool(5))
            {
                Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, DustID.Torch, Player.velocity.X * 0.3f, Player.velocity.Y * 0.3f, 100, Color.Orange, 0.6f);
                dust.noGravity = true;
                dust.fadeIn = 0.1f;
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
                            125,
                            5f,
                            Player.whoAmI,
                            ai0: i,
                            ai1: 0f,
                            ai2: 0f
                        );
                    }
                }
            }
        }

        private void LaunchAllSpikes()
        {
            int launchedCount = 0;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && 
                    proj.type == ModContent.ProjectileType<PrimalSpike>() && 
                    proj.owner == Player.whoAmI &&
                    proj.ai[1] == 0f)
                {
                    proj.ai[1] = 1f;
                    int spikeIndex = (int)proj.ai[0];
                    if (spikeIndex >= 0 && spikeIndex < 8)
                    {
                        spikeRespawnTimers[spikeIndex] = 300;
                    }
                    
                    launchedCount++;
                }
            }
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
                
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item71, Player.Center);
            }
        }
    }
}