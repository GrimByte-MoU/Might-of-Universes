using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameInput;

namespace MightofUniverses.Common.Players
{
    public class ReaperPlayer : ModPlayer
    {
        // Core soul economy
        public float soulEnergy;
        public float maxSoulEnergy = 100f;
        public float soulGatherMultiplier = 1f;

        // Class flags/stats
        public bool hasReaperArmor;
        public float reaperDamageMultiplier = 1f;
        public float reaperCritChance = 0f;

        // Misc
        public int deathMarks;
        public const int MAX_DEATH_MARKS = 5;
        public static ModKeybind SoulReleaseKey;
        public bool justConsumedSouls;

        // Timers
        public int TempleBuffTimer;

        // Constants
        private const float BaseMaxSoulEnergy = 100f;

        public override void Load()
        {
            SoulReleaseKey = KeybindLoader.RegisterKeybind(Mod, "Release Soul Energy", "R");
        }

        public override void Unload()
        {
            SoulReleaseKey = null;
        }

        public override void Initialize()
        {
            soulEnergy = 0f;
            soulGatherMultiplier = 1f;
            hasReaperArmor = false;
            reaperDamageMultiplier = 1f;
            reaperCritChance = 0f;
            justConsumedSouls = false;
            deathMarks = 0;
            TempleBuffTimer = 0;
            maxSoulEnergy = BaseMaxSoulEnergy;
        }

        public override void ResetEffects()
        {
            // Reset per-tick accumulators; accessories and buffs add to these each tick
            maxSoulEnergy = BaseMaxSoulEnergy;
            soulGatherMultiplier = 1f;
            hasReaperArmor = false;
            reaperDamageMultiplier = 1f;
            reaperCritChance = 0f;
            justConsumedSouls = false;

            if (TempleBuffTimer > 0)
                TempleBuffTimer--;
        }

        // Visual-friendly soul gain from a world position (creates a small dust trail)
        public void AddSoulEnergy(float amount, Vector2 sourcePosition)
        {
            if (amount <= 0f)
                return;

            float adjustedAmount = amount * soulGatherMultiplier;

            if (soulEnergy < maxSoulEnergy)
            {
                soulEnergy = MathHelper.Clamp(soulEnergy + adjustedAmount, 0f, maxSoulEnergy);

                // Create soul-gathering dust trail effect
                Vector2 dirToPlayer = Player.Center - sourcePosition;
                float distance = dirToPlayer.Length();
                if (distance > 0.01f)
                {
                    dirToPlayer.Normalize();
                    for (int i = 0; i < 10; i++)
                    {
                        Vector2 dustPosition = sourcePosition + dirToPlayer * distance * (i / 10f);
                        Dust dust = Dust.NewDustPerfect(
                            dustPosition,
                            DustID.WhiteTorch,
                            dirToPlayer * 5f,
                            0,
                            Color.White,
                            1f
                        );
                        dust.noGravity = true;
                        dust.fadeIn = 1.2f;
                    }
                }
            }
        }

        // Silent, raw gain (no visuals)
        public void AddSoulEnergy(float amount)
        {
            if (amount <= 0f)
                return;

            soulEnergy = MathHelper.Clamp(soulEnergy + amount * soulGatherMultiplier, 0f, maxSoulEnergy);
        }

        // Consume a specific amount of souls
        public bool ConsumeSoulEnergy(float amount)
        {
            if (amount <= 0f)
                return true;

            if (soulEnergy >= amount)
            {
                soulEnergy -= amount;
                justConsumedSouls = true;
                return true;
            }
            return false;
        }

        // Try to consume souls due to a player-triggered release (bound key), then run callback
        // Also integrates accessory refund hooks after a successful spend.
        public bool TryReleaseSouls(float cost, Action<Player> onSuccess, string releaseMessage = null)
        {
            if (SoulReleaseKey.JustPressed)
            {
                if (ConsumeSoulEnergy(cost))
                {
                    onSuccess?.Invoke(Player);

                    Console.WriteLine($"Souls consumed: {cost}");

                    Main.NewText(releaseMessage ?? $"{(int)cost} souls released!", Color.Green);
                    return true;
                }
                else
                {
                    Main.NewText("Not enough soul energy to activate!", Color.Red);
                }
            }
            return false;
        }

        // Utility: percentage of current souls
        public float SoulEnergyPercent => maxSoulEnergy > 0f ? soulEnergy / maxSoulEnergy : 0f;

        // Utility: hard set (clamped)
        public void SetSoulEnergy(float value)
        {
            soulEnergy = MathHelper.Clamp(value, 0f, maxSoulEnergy);
        }

        // Utility: empty all souls and return amount removed
        public float ConsumeAllSouls()
        {
            float consumed = soulEnergy;
            soulEnergy = 0f;
            justConsumedSouls = consumed > 0f;
            return consumed;
        }

        public void UpdateReaperDamageMultiplier(float amount)
        {
            reaperDamageMultiplier = MathHelper.Clamp(reaperDamageMultiplier + amount, 1f, 10f);
        }

        public void UpdateReaperCritChance(float amount)
        {
            reaperCritChance = MathHelper.Clamp(reaperCritChance + amount, 0f, 100f);
        }
    }
}