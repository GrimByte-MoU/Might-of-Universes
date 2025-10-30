using MightofUniverses.Content.Items.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MightofUniverses.Common.Players
{
    public class MurkyIceSetPlayer : ModPlayer
    {
        private const int MaxSoulBonus = 60;

        private bool wearingMurkyIceSet;

        public override void ResetEffects()
        {
            wearingMurkyIceSet = false;
        }

        public override void UpdateEquips()
        {
            if (IsWearingFullMurkyIceSet())
            {
                wearingMurkyIceSet = true;
                var reaper = Player.GetModPlayer<ReaperPlayer>();
                reaper.hasReaperArmor = true;
                reaper.maxSoulEnergy += MaxSoulBonus;
                Player.GetModPlayer<ReaperAccessoryPlayer>().flatMaxSoulsBonus += MaxSoulBonus;
            }
        }

        public override void PostUpdateEquips()
        {
            if (wearingMurkyIceSet)
            {
                Player.setBonus = "+60 max souls\nWhen you consume souls, gain Chilling Presence (5s). Reaper attacks +15% damage vs Frostburn, Corrupted or Spineless enemies.";
            }
        }

        public override void PostUpdate()
        {
            if (!wearingMurkyIceSet) return;

            var reaper = Player.GetModPlayer<ReaperPlayer>();
            if (reaper.justConsumedSouls)
            {
                Player.AddBuff(ModContent.BuffType<ChillingPresenceBuff>(), 300);
                for (int i = 0; i < 8; i++)
                {
                    var pos = Player.Center + Main.rand.NextVector2Circular(12f, 12f);
                    int d = Dust.NewDust(pos, 1, 1, DustID.Ice, 0f, 0f, 150, default, 0.9f);
                    Main.dust[d].noGravity = true;
                    Main.dust[d].velocity *= 0.2f;
                }
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item21, Player.position);
            }

            Lighting.AddLight(Player.Center, 0f, 0.4f, 0.9f);

            if (Main.rand.NextBool(4))
            {
                Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, DustID.Ice, 0f, 0f, 100, default, 0.8f);
                dust.noGravity = true;
                dust.fadeIn = 0.2f;
            }

            if (Main.rand.NextBool(3))
            {
                Color color = Main.rand.NextBool() ? new Color(0.8f, 0.5f, 1f) : new Color(1f, 0.5f, 0.8f);
                Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, DustID.Snow, 0f, 0f, 100, color, 0.8f);
                dust.noGravity = true;
                dust.fadeIn = 0.2f;
            }
        }

        private bool IsWearingFullMurkyIceSet()
        {
            return Player.armor[0].type == ModContent.ItemType<Content.Items.Armors.MurkyIceHelmet>()
                && Player.armor[1].type == ModContent.ItemType<Content.Items.Armors.MurkyIceChestplate>()
                && Player.armor[2].type == ModContent.ItemType<Content.Items.Armors.MurkyIceBoots>();
        }

        public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (item.DamageType == ModContent.GetInstance<ReaperDamageClass>())
                ApplyBonus(target, ref modifiers);
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (proj.DamageType == ModContent.GetInstance<ReaperDamageClass>())
                ApplyBonus(target, ref modifiers);
        }

        private void ApplyBonus(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (target == null || !target.active) return;
            bool hasFrost = target.HasBuff(BuffID.Frostburn);
            bool hasCorrupted = target.HasBuff(ModContent.BuffType<Corrupted>());
            bool hasSpineless = target.HasBuff(ModContent.BuffType<Spineless>());
            if (hasFrost || hasCorrupted || hasSpineless)
                modifiers.SourceDamage *= 1.15f;
        }
    }
}