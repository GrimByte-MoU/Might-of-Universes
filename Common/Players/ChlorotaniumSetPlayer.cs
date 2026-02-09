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
        private const int MaxSoulBonus = 200;

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

                reaper.maxSoulEnergy += MaxSoulBonus;

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

        public override void PostUpdate()
        {
            if (!wearingFull) return;

            Lighting.AddLight(Player.Center, 0f, 0.8f, 0.5f);

            if (Main.rand.NextBool(3))
            {
                int dustType = Main.rand.NextBool() ? DustID.Titanium : DustID.Chlorophyte;
                Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, dustType, 0f, 0f, 100, default, 0.8f);
                dust.noGravity = true;
                dust.fadeIn = 0.2f;
            }

            if (Main.rand.NextBool(2))
            {
                Color color = Main.rand.NextBool() ? Color.Green : Color.Silver;
                Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, DustID.Torch, Player.velocity.X * 0.5f, Player.velocity.Y * 0.5f, 100, color, 0.5f);
                dust.noGravity = true;
                dust.fadeIn = 0.1f;
                dust.scale *= Main.rand.NextFloat(0.8f, 1.2f);
            }
        }
    }
}