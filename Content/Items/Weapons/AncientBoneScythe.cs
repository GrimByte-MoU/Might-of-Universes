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
        public float BaseSoulCost => 125f;

        private const float PulseRadiusPx = 30f * 16f;
        private const int TarOnHitDuration = 120;
        private const int PulseTarDuration = 300;
        private const int PrimalSavageryDuration = 300;
        private const int PulseDamage = 750;

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
            Item.UseSound = SoundID.Item71;

            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(gold: 10);

            Item.noMelee = false;
            Item.scale = 1.3f;
        }

        public override void HoldItem(Player player)
        {
            var reaper = player.GetModPlayer<ReaperPlayer>();

            if (ReaperPlayer.SoulReleaseKey != null && ReaperPlayer.SoulReleaseKey.JustPressed)
            {
                int effectiveCost = SoulCostHelper.ComputeEffectiveSoulCostInt(player, BaseSoulCost);
                if (reaper.ConsumeSoulEnergy(effectiveCost))
                {
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
            var reaper = player.GetModPlayer<ReaperPlayer>();
            reaper.AddSoulEnergy(8f, target.Center);
            target.AddBuff(ModContent.BuffType<Tarred>(), TarOnHitDuration);
        }

        private void DoGrayPulse(Player player)
        {
            Vector2 center = player.Center;

            int travelTime = 30;
            float speedForTravel = PulseRadiusPx / Math.Max(1, travelTime);

            for (int i = 0; i < 48; i++)
            {
                float angle = MathHelper.TwoPi * i / 48f;
                Vector2 dir = angle.ToRotationVector2();
                Vector2 velocity = dir * (speedForTravel * 0.9f);
                var dust = Dust.NewDustPerfect(center, DustID.Smoke, velocity, 150, new Color(180, 180, 180), 1.2f);
                dust.noGravity = true;
                dust.fadeIn = 0.8f;
                dust.alpha = 255 - (travelTime + 6);
            }

            int ringCount = 4;
            int dirsPerRing = 36;
            for (int r = 0; r < ringCount; r++)
            {
                for (int i = 0; i < dirsPerRing; i++)
                {
                    float angle = MathHelper.TwoPi * i / dirsPerRing + Main.rand.NextFloat(0.02f);
                    Vector2 dir = angle.ToRotationVector2();
                    float speedMult = 1f + (r * 0.05f) + Main.rand.NextFloat(-0.08f, 0.08f);
                    Vector2 velocity = dir * (speedForTravel * speedMult);

                    var d = Dust.NewDustPerfect(center, DustID.Smoke, velocity, 180, new Color(120, 120, 120), 1.0f);
                    d.noGravity = true;
                    d.fadeIn = 0.9f;
                    d.alpha = 255 - (travelTime + 10);
                }
            }

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC n = Main.npc[i];
                    if (n == null || !n.active || n.friendly || n.dontTakeDamage || n.life <= 0)
                        continue;

                    float dist = Vector2.Distance(n.Center, center);
                    if (dist <= PulseRadiusPx)
                    {
                        int hitDir = Math.Sign(n.Center.X - center.X);
                        if (hitDir == 0) hitDir = 1;

                        n.StrikeNPC(new NPC.HitInfo()
                        {
                            Damage = PulseDamage,
                            Knockback = 0f,
                            HitDirection = hitDir
                        });

                        for (int k = 0; k < 8; k++)
                        {
                            Vector2 dustPos = n.Center + Main.rand.NextVector2Circular(12f, 12f);
                            int dd = Dust.NewDust(dustPos, 2, 2, DustID.Smoke, 0f, 0f, 150, Color.White, 1.1f);
                            Main.dust[dd].noGravity = true;
                        }
                    }
                }
            }

            SoundEngine.PlaySound(SoundID.Item14, player.position);
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