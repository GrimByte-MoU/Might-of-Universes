using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Common; // ReaperPlayer, ReaperDamageClass

namespace MightofUniverses.Common.Players
{
    public class CactusSetRework : ModPlayer
    {
        // Configurable knobs
        private const float ReaperDamageBonus = 0.08f;   // +8% reaper damage
        private const int SoulOnHitCooldownTicks = 30;   // 0.5s @ 60 FPS
        private const int SoulOnHitAmount = 5;

        private bool wearingCactusSet;
        private int soulOnHitCooldown;

        public override void ResetEffects()
        {
            wearingCactusSet = false; // will be re-enabled below if full set is worn
        }

        public override void UpdateEquips()
        {
            // Detect full Cactus set: head/body/legs
            if (IsWearingFullCactusSet())
            {
                wearingCactusSet = true;
                Player.setBonus =
                    "Bristling with spikes: attackers take damage\n" +
                    "+8% reaper damage\n" +
                    "When struck, gain 5 souls. This has a 0.5 second cooldown.";

                // Apply Reaper-only bonuses
                Player.GetDamage(ModContent.GetInstance<ReaperDamageClass>()) += ReaperDamageBonus;
                // NOTE: We intentionally do NOT add extra thorns here to avoid stacking with vanilla Cactus thorns behavior.
            }
        }

        public override void PostUpdate()
        {
            if (soulOnHitCooldown > 0)
                soulOnHitCooldown--;
        }

        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            if (!wearingCactusSet)
                return;

            TryGrantSoul(npc?.Center ?? Player.Center);
        }

        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
        {
            if (!wearingCactusSet)
                return;

            TryGrantSoul(proj?.Center ?? Player.Center);
        }

        private void TryGrantSoul(Vector2 worldPos)
        {
            if (soulOnHitCooldown > 0)
                return;

            var reaper = Player.GetModPlayer<ReaperPlayer>();
            reaper.AddSoulEnergy(SoulOnHitAmount, worldPos);

            // Small visual
            for (int i = 0; i < 6; i++)
            {
                var d = Dust.NewDustPerfect(
                    worldPos + Main.rand.NextVector2Circular(10f, 10f),
                    DustID.Grass,
                    Main.rand.NextVector2Circular(1.2f, 1.2f),
                    Alpha: 150,
                    new Color(80, 255, 120),
                    Scale: 1.0f
                );
                d.noGravity = true;
            }

            soulOnHitCooldown = SoulOnHitCooldownTicks;
        }

        private bool IsWearingFullCactusSet()
        {
            // Armor slots: 0=head, 1=body, 2=legs
            return Player.armor[0].type == ItemID.CactusHelmet
                && Player.armor[1].type == ItemID.CactusBreastplate
                && Player.armor[2].type == ItemID.CactusLeggings;
        }
    }
}