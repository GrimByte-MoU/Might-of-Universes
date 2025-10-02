using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Content.Items.Armors;
using MightofUniverses.Common;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Weapons;

namespace MightofUniverses.Common.Players
{
    public class ChlorotaniumSetPlayer : ModPlayer
    {
        private bool wearingFull;
        public bool HasChloroChest;
        private const int   MaxSoulBonus        = 200;      // previously added

        public override void ResetEffects()
        {
            wearingFull = false;
            HasChloroChest = false;
        }

        public override void UpdateEquips()
        {
            if (IsFullSet())
            {
                wearingFull = true;
                var reaper = Player.GetModPlayer<ReaperPlayer>();
                reaper.hasReaperArmor = true;

                // Max souls
                reaper.maxSoulEnergy += MaxSoulBonus;

                // Set-wide bonuses
                var reaperClass = ModContent.GetInstance<ReaperDamageClass>();
                Player.GetDamage(reaperClass) += 0.1f;
                Player.endurance += 0.1f;
            }
        }

        public override void PostUpdateEquips()
        {
            if (wearingFull)
            {
                Player.setBonus =
                    $"+{MaxSoulBonus} max souls\n" +
                    "+10% Reaper damage and +10% damage reduction\n" +
                    "The Chlorotanium Scythe gains an additional +10% damage and crit chance as well as +15% swing speed and size.";
            }
        }

        // Apply scythe-only damage bonus (kept from previous behavior)
        public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers)
        {
            ApplyScytheDamage(item, ref modifiers);
            ApplyCursedInfernoPen(target, ref modifiers);
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            ApplyScytheDamage(Player.HeldItem, ref modifiers);
            ApplyCursedInfernoPen(target, ref modifiers);
        }

        private void ApplyScytheDamage(Item item, ref NPC.HitModifiers modifiers)
        {
            if (!wearingFull) return;
            if (item == null) return;
            if (item.type == ModContent.ItemType<ChlorotaniumScythe>())
            {
                modifiers.SourceDamage *= 1f + 0.10f;
                modifiers.CritDamage *= 1f + 0.10f;
            }
        }

        private void ApplyCursedInfernoPen(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (!HasChloroChest) return;
            if (target == null || !target.active) return;
            if (target.HasBuff(BuffID.CursedInferno))
            {
                modifiers.ArmorPenetration += 30;
            }
        }

        // Item-specific attack speed (swing speed)
        public override void ModifyItemScale(Item item, ref float scale)
        {
            if (wearingFull && item != null && item.type == ModContent.ItemType<ChlorotaniumScythe>())
            {
                scale *= 1.15f;
                scale *= 1.15f;
            }
        }

        private bool IsFullSet()
        {
            return Player.armor[0].type == ModContent.ItemType<ChlorotaniumMaskedHelmet>()
                && Player.armor[1].type == ModContent.ItemType<ChlorotaniumChestplate>()
                && Player.armor[2].type == ModContent.ItemType<ChlorotaniumGreaves>();
        }
    }
}