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
        private Vector2 waveOrigin = Vector2.Zero;
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

            if (waveActive)
            {
                UpdateWave();
            }
        }

        private void StartWinterPulse()
        {
            waveActive = true;
            currentWaveRadius = 0f;
            waveOrigin = Player.Center;
            hitNPCs.Clear();

            for (int i = 0; i < 20; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(6f, 6f);
                Dust dust = Dust.NewDustPerfect(waveOrigin, DustID.IceTorch, velocity, 100, new Color(100, 200, 255), 1.5f);
                dust.noGravity = true;
            }

            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item28, waveOrigin);
        }

        private void UpdateWave()
        {
            currentWaveRadius += WaveSpeed;

            if (currentWaveRadius >= MaxWaveRadius)
            {
                waveActive = false;
                return;
            }

            int dustCount = (int)(currentWaveRadius / 50);
            if (dustCount < 50) dustCount = 50;
            
            for (int i = 0; i < dustCount; i++)
            {
                float angle = i / (float)dustCount * MathHelper.TwoPi;
                Vector2 position = waveOrigin + new Vector2(
                    (float)System.Math.Cos(angle) * currentWaveRadius,
                    (float)System.Math.Sin(angle) * currentWaveRadius
                );

                Color dustColor = new Color(100, 200, 255);
                Dust dust = Dust.NewDustPerfect(position, DustID.IceTorch, Vector2.Zero, 100, dustColor, 1.3f);
                dust.noGravity = true;
                dust.velocity = Vector2.Zero;
            }

            if (Main.rand.NextBool(1))
            {
                float angle = Main.rand.NextFloat(0, MathHelper.TwoPi);
                Vector2 position = waveOrigin + new Vector2(
                    (float)System.Math.Cos(angle) * currentWaveRadius,
                    (float)System.Math.Sin(angle) * currentWaveRadius
                );

                Dust dust = Dust.NewDustPerfect(position, DustID.Snow, Vector2.Zero, 100, new Color(200, 230, 255), 1.0f);
                dust.noGravity = true;
            }

            int baseDamage = 40;
            float damageMultiplier = 1f + Player.GetModPlayer<PacifistPlayer>().pacifistDamageMultiplier;
            int finalDamage = (int)(baseDamage * damageMultiplier);
            float waveThickness = WaveSpeed * 2;
            
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && !npc.friendly && npc.lifeMax > 5 && !npc.dontTakeDamage && !hitNPCs.Contains(npc.whoAmI))
                {
                    float distance = Vector2.Distance(waveOrigin, npc.Center);

                    if (distance <= currentWaveRadius && distance >= (currentWaveRadius - waveThickness))
                    {
                        hitNPCs.Add(npc.whoAmI);
                        Player.ApplyDamageToNPC(npc, finalDamage, 0f, 0, false);

                        if (!npc.boss)
                        {
                            npc.AddBuff(ModContent.BuffType<Paralyze>(), 180);
                        }

                        for (int j = 0; j < 8; j++)
                        {
                            Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.IceTorch, 0f, 0f, 100, new Color(100, 200, 255), 1.4f);
                            dust.noGravity = true;
                            dust.velocity = Main.rand.NextVector2Circular(3f, 3f);
                        }
                    }
                }
            }
            Lighting.AddLight(waveOrigin + new Vector2(currentWaveRadius, 0), 0.3f, 0.6f, 1f); // Use waveOrigin
        }
    }
}