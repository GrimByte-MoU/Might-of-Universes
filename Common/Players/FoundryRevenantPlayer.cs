using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common; // ReaperDamageClass

namespace MightofUniverses.Common.Players
{
    public class FoundryRevenantPlayer : ModPlayer
    {
        public const int MaxStacks = 50;

        public bool FullSetEquipped;
        public int Stacks;

        public override void Initialize()
        {
            Stacks = 0;
        }

        public override void ResetEffects()
        {
            FullSetEquipped = false;
        }

        public override void UpdateDead()
        {
            Stacks = 0;
        }

        public override void PostUpdateEquips()
        {
            if (!FullSetEquipped)
            {
                // Lose stacks when the set is unequipped (per design)
                if (Stacks > 0) Stacks = 0;
                return;
            }

            var reaperClass = ModContent.GetInstance<ReaperDamageClass>();

            // Set bonus +10% Reaper damage
            Player.GetDamage(reaperClass) += 0.10f;

            // Stack-based effects
            int s = Math.Clamp(Stacks, 0, MaxStacks);

            // +1 armor penetration per stack (generic)
            Player.GetArmorPenetration(DamageClass.Generic) += s;

            // 5+ stacks: +1% Reaper damage per stack
            if (s >= 5)
                Player.GetDamage(reaperClass) += 0.01f * s;

            // 10+ stacks: +1% movement speed per stack
            if (s >= 10)
                Player.moveSpeed += 0.01f * s;

            // 50 stacks: +0.2% DR per stack
            if (s >= 50)
                Player.endurance += 0.002f * s;
        }

        // In 1.4, use FreeDodge to cancel an incoming hit.
        // Return true to dodge; false to take the hit normally.
        public override bool FreeDodge(Player.HurtInfo info)
        {
            if (!FullSetEquipped)
                return false;

            int s = Stacks;
            if (s >= 25)
            {
                float chance = s * 0.001f; // 0.1% per stack at 25+, up to 5% at 50
                if (Main.rand.NextFloat() < chance)
                {
                    // Visual/audio feedback
                    SoundEngine.PlaySound(SoundID.Item29 with { Volume = 0.8f, Pitch = 0.1f }, Player.Center);
                    for (int i = 0; i < 12; i++)
                    {
                        int d = Dust.NewDust(Player.position, Player.width, Player.height, DustID.Smoke, 0f, 0f, 120, default, 1.1f);
                        Main.dust[d].noGravity = true;
                    }

                    // Small immunity window for feel (optional)
                    Player.SetImmuneTimeForAllTypes(20);
                    return true; // dodge the hit
                }
            }

            return false; // no dodge, take the hit
        }

        // Call this when the player consumes souls (1 stack per event; persists until death or unequip)
        public void OnSoulsConsumed(int amount)
        {
            if (!FullSetEquipped || amount <= 0) return;
            if (Stacks < MaxStacks)
            {
                Stacks++;
                int d = Dust.NewDust(Player.Center, 0, 0, DustID.Torch, Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f), 160, new Color(255, 160, 80), 1.1f);
                Main.dust[d].noGravity = true;
            }
        }
    }
}