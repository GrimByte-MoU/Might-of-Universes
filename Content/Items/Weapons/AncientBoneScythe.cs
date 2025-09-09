using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Content.Items.Buffs;
using System;
using MightofUniverses.Common;
using MightofUniverses.Common.Players;
using MightofUniverses.Common.Abstractions;
using MightofUniverses.Common.Util;

namespace MightofUniverses.Content.Items.Weapons
{
    public class AncientBoneScythe : ModItem, IHasSoulCost
    {
        public float BaseSoulCost => 100f;

        private const float PulseRadiusPx = 50f * 16f; // 30 tiles
        private const int TarOnHitDuration = 120;       // 2 seconds
        private const int PulseTarDuration = 300;       // 5 seconds
        private const int PrimalSavageryDuration = 300; // 5 seconds

        public override void SetDefaults()
        {
            Item.width = 66;
            Item.height = 66;

            Item.damage = 90;
            Item.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Item.knockBack = 6f;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item1;

            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(gold: 10);

            Item.noMelee = false;
            Item.scale = 1.45f;
        }

        public override void HoldItem(Player player)
        {
            var reaper = player.GetModPlayer<ReaperPlayer>();

            if (ReaperPlayer.SoulReleaseKey != null && ReaperPlayer.SoulReleaseKey.JustPressed)
            {
                int effectiveCost = SoulCostHelper.ComputeEffectiveSoulCostInt(player, BaseSoulCost);
                if (reaper.ConsumeSoulEnergy(effectiveCost))
                {
                    // Visual + sound feedback
                    SoundEngine.PlaySound(SoundID.Item62 with { Pitch = -0.15f }, player.Center);
                    DoGrayPulse(player);
                    for (int i = 0; i < Main.maxNPCs; i++)
                    {
                        NPC n = Main.npc[i];
                        if (!n.active || n.friendly || n.dontTakeDamage || n.life <= 0)
                            continue;

                        if (Vector2.Distance(n.Center, player.Center) <= PulseRadiusPx)
                        {
                            n.AddBuff(ModContent.BuffType<Tarred>(), PulseTarDuration);
                        }
                    }
                    player.AddBuff(ModContent.BuffType<PrimalSavageryBuff>(), PrimalSavageryDuration);
                }
            }
        }

        public override void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (player.HasBuff(ModContent.BuffType<PrimalSavageryBuff>()))
            {
                modifiers.ArmorPenetration += 50;
                modifiers.SourceDamage *= 1.30f;
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            // Gather souls and apply Tarred on hit
            var reaper = player.GetModPlayer<ReaperPlayer>();
            reaper.AddSoulEnergy(10f, target.Center);
            target.AddBuff(ModContent.BuffType<Tarred>(), TarOnHitDuration);
        }

        private void DoGrayPulse(Player player)
        {
            Vector2 center = player.Center;

            for (int i = 0; i < 48; i++)
            {
                float angle = MathHelper.TwoPi * i / 48f;
                Vector2 dir = angle.ToRotationVector2();
                Vector2 spawnPos = center + dir * 8f;

                int d = Dust.NewDust(spawnPos, 0, 0, DustID.Smoke, 0f, 0f, 140, new Color(180, 180, 180), 1.2f);
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity = dir * 6f;
            }
            for (int r = 800; r <= (int)PulseRadiusPx; r += 64)
            {
                for (int i = 0; i < 18; i++)
                {
                    float angle = MathHelper.TwoPi * i / 18f + Main.rand.NextFloat(0.1f);
                    Vector2 pos = center + angle.ToRotationVector2() * r;
                    int d2 = Dust.NewDust(pos - new Vector2(3), 6, 6, DustID.Smoke, 0f, 0f, 180, new Color(120, 120, 120), 1.0f);
                    Main.dust[d2].noGravity = true;
                    Main.dust[d2].velocity *= 0.2f;
                }
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<WardensHook>(), 1)
                .AddIngredient(ModContent.ItemType<PrehistoricAmber>(), 5)
                .AddIngredient(ModContent.ItemType<AncientBone>(), 10)
                .AddIngredient(ModContent.ItemType<TarChunk>(), 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}