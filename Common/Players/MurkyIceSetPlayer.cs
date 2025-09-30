using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Common.Players
{
    public class MurkyIceSetPlayer : ModPlayer
    {
        private const int SetBonusMaxSouls = 60;

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

                var acc = Player.GetModPlayer<ReaperAccessoryPlayer>();
                acc.flatMaxSoulsBonus += SetBonusMaxSouls;
            }
        }

        public override void PostUpdateEquips()
        {
            if (wearingMurkyIceSet)
            {
                Player.setBonus = $"+{SetBonusMaxSouls} max souls\nWhen you consume souls, gain Chilling Presence for 5 seconds. Reaper attacks deal +15% damage to enemies with Frostburn, Corrupted or Spineless.";
            }
        }

        public override void PostUpdate()
        {
            if (!wearingMurkyIceSet)
                return;

            var reaper = Player.GetModPlayer<ReaperPlayer>();
            if (reaper != null && reaper.justConsumedSouls)
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
        }

        private bool IsWearingFullMurkyIceSet()
        {
            return Player.armor[0].type == ModContent.ItemType<Content.Items.Armors.MurkyIceHelmet>()
                && Player.armor[1].type == ModContent.ItemType<Content.Items.Armors.MurkyIceChestplate>()
                && Player.armor[2].type == ModContent.ItemType<Content.Items.Armors.MurkyIceBoots>();
        }

        // Apply 15% extra Reaper damage to targets with Frostburn, Corrupted, or Spineless
        public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (item.DamageType != ModContent.GetInstance<ReaperDamageClass>()) return;
            ApplyChillDamageBonus(target, ref modifiers);
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (proj.DamageType != ModContent.GetInstance<ReaperDamageClass>()) return;
            ApplyChillDamageBonus(target, ref modifiers);
        }

        private void ApplyChillDamageBonus(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (target == null || !target.active) return;

            bool hasFrost = target.HasBuff(BuffID.Frostburn);
            bool hasCorrupted = target.HasBuff(ModContent.BuffType<Corrupted>());
            bool hasSpineless = target.HasBuff(ModContent.BuffType<Spineless>());

            if (hasFrost || hasCorrupted || hasSpineless)
            {
                modifiers.SourceDamage *= 1.15f;
            }
        }
    }
}