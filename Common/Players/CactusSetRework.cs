using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common;               // ReaperDamageClass
using MightofUniverses.Common.Players;       // ReaperPlayer

namespace MightofUniverses.Common.Players
{
    public class CactusSetRework : ModPlayer
    {
        private const int SoulOnHitCooldownTicks = 30; // 0.5s @ 60 FPS
        private const int SoulOnHitAmount = 5;
        private const int SetBonusMaxSouls = 20;

        private bool wearingCactusSet;
        private int soulOnHitCooldown;

        public override void ResetEffects()
        {
            wearingCactusSet = false;
        }

        public override void UpdateEquips()
        {
            if (IsWearingFullCactusSet())
            {
                wearingCactusSet = true;

                // Mark as “Reaper armor” so the SoulBar UI draws.
                var reaper = Player.GetModPlayer<ReaperPlayer>();
                reaper.hasReaperArmor = true;

                // Add to max souls via a bonus accumulator if your ReaperPlayer supports it.
                // Prefer a bonus field that gets summed into max each tick.
                // Replace this line with your exact field name.
                reaper.maxSoulEnergy += 20f;
            }
        }

        // Set the set-bonus string LAST so vanilla doesn’t overwrite it.
        public override void PostUpdateEquips()
        {
            if (wearingCactusSet)
            {
                Player.setBonus =
                    "Attackers take damage\n" +
                    "+20 max souls\n" +
                    "When struck, gain 5 souls (0.5s cooldown)";
            }
        }

        public override void PostUpdate()
        {
            if (soulOnHitCooldown > 0)
                soulOnHitCooldown--;
        }

        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            if (!wearingCactusSet || !Player.active || Player.dead)
                return;

            TryGrantSoul(npc?.Center ?? Player.Center);
        }

        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
        {
            if (!wearingCactusSet || !Player.active || Player.dead)
                return;

            TryGrantSoul(proj?.Center ?? Player.Center);
        }

        private void TryGrantSoul(Vector2 worldPos)
        {
            if (soulOnHitCooldown > 0)
                return;

            var reaper = Player.GetModPlayer<ReaperPlayer>();

            // If AddSoulEnergy already syncs in MP, this is fine.
            // Otherwise, gate to server/owner before mutating state.
            reaper.AddSoulEnergy(SoulOnHitAmount, worldPos);

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
            // Armor slots: 0=head, 1=body, 2=legs (functional slots, not vanity)
            return Player.armor[0].type == ItemID.CactusHelmet
                && Player.armor[1].type == ItemID.CactusBreastplate
                && Player.armor[2].type == ItemID.CactusLeggings;
        }
    }
}