using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Content.Items.Buffs;
using Microsoft.Xna.Framework;
using MightofUniverses.Common;
using System.Collections.Generic;

namespace MightofUniverses.Content.Items.Accessories
{
    public class BlizzardPulser : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.sellPrice(gold: 15);
            Item.rare = ModContent.RarityType<TerraiumRarity>();
            Item.accessory = true;
            Item.DamageType = ModContent.GetInstance<PacifistDamageClass>();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<BlizzardPulserPlayer>().hasBlizzardPulser = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<SongOfWinter>()
                .AddIngredient(ModContent.ItemType<TerraiumBar>(), 7)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }

    public class BlizzardPulserPlayer : ModPlayer
    {
        public bool hasBlizzardPulser = false;
        private int pulseTimer = 0;
        private const int PulseInterval = 120;
        private bool waveActive = false;
        private float currentWaveRadius = 0f;
        private Vector2 waveOrigin = Vector2.Zero;
        private const float MaxWaveRadius = 600f;
        private const float WaveSpeed = 600f / 60f;
        private HashSet<int> hitNPCs = new HashSet<int>();

        public override void ResetEffects()
        {
            hasBlizzardPulser = false;
        }

        public override void PostUpdate()
        {
            if (!hasBlizzardPulser)
            {
                waveActive = false;
                return;
            }

            pulseTimer++;

            if (pulseTimer >= PulseInterval && !waveActive)
            {
                pulseTimer = 0;
                StartBlizzardPulse();
            }

            if (waveActive)
            {
                UpdateWave();
            }
        }

        private void StartBlizzardPulse()
        {
            waveActive = true;
            currentWaveRadius = 0f;
            waveOrigin = Player.Center;
            hitNPCs.Clear();

            for (int i = 0; i < 30; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(8f, 8f);
                Dust dust = Dust.NewDustPerfect(waveOrigin, DustID.IceTorch, velocity, 100, new Color(150, 220, 255), 2f);
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

            int dustCount = (int)(currentWaveRadius / 40);
            if (dustCount < 60) dustCount = 60;
            
            for (int i = 0; i < dustCount; i++)
            {
                float angle = i / (float)dustCount * MathHelper.TwoPi;
                Vector2 position = waveOrigin + new Vector2(
                    (float)System.Math.Cos(angle) * currentWaveRadius,
                    (float)System.Math.Sin(angle) * currentWaveRadius
                );

                Color dustColor = new Color(150, 220, 255);
                Dust dust = Dust.NewDustPerfect(position, DustID.IceTorch, Vector2.Zero, 100, dustColor, 1.5f);
                dust.noGravity = true;
                dust.velocity = Vector2.Zero;
            }

            if (Main.rand.NextBool(2))
            {
                float angle = Main.rand.NextFloat(0, MathHelper.TwoPi);
                Vector2 position = waveOrigin + new Vector2(
                    (float)System.Math.Cos(angle) * currentWaveRadius,
                    (float)System.Math.Sin(angle) * currentWaveRadius
                );

                Dust dust = Dust.NewDustPerfect(position, DustID.Snow, Vector2.Zero, 100, new Color(220, 240, 255), 1.2f);
                dust.noGravity = true;
            }

            int baseDamage = 125;
            float damageMultiplier = Player.GetDamage<PacifistDamageClass>().Additive + Player.GetDamage<PacifistDamageClass>().Multiplicative - 1f;
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

                        npc.StrikeNPC(new NPC.HitInfo
                        {
                            Damage = finalDamage,
                            Knockback = 2f,
                            HitDirection = 0
                        });

                        npc.AddBuff(ModContent.BuffType<SheerCold>(), 180);
                        
                        if (!npc.boss)
                        {
                            npc.AddBuff(ModContent.BuffType<Paralyze>(), 120);
                        }

                        for (int j = 0; j < 12; j++)
                        {
                            Dust dust = Dust.NewDustDirect(
                                npc.position,
                                npc.width,
                                npc.height,
                                DustID.IceTorch,
                                0f,
                                0f,
                                100,
                                new Color(150, 220, 255),
                                1.8f
                            );
                            dust.noGravity = true;
                            dust.velocity = Main.rand.NextVector2Circular(4f, 4f);
                        }
                    }
                }
            }

            Lighting.AddLight(waveOrigin + new Vector2(currentWaveRadius, 0), 0.4f, 0.7f, 1.2f);
        }
    }
}