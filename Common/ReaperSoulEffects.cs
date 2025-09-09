using System;
using Terraria;
using Microsoft.Xna.Framework;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Common
{
    /// <summary>
    /// Static helper methods for Reaper soul effects and empowerment system.
    /// </summary>
    public static class ReaperSoulEffects
    {
        /// <summary>
        /// Attempts to release souls and activate empowerment with specified values and consume effect.
        /// Handles the soul release key check, energy consumption, empowerment activation, and optional consume effect.
        /// </summary>
        /// <param name="player">The player attempting to release souls</param>
        /// <param name="cost">Soul energy cost</param>
        /// <param name="durationTicks">Duration of empowerment in ticks</param>
        /// <param name="empowermentValues">The empowerment values to apply</param>
        /// <param name="onConsume">Optional action to execute when souls are consumed (for special effects like projectiles)</param>
        /// <param name="releaseMessage">Optional custom message to display when souls are released</param>
        /// <returns>True if souls were successfully released and empowerment activated</returns>
        public static bool TryReleaseSoulsWithEmpowerment(
            Player player, 
            float cost, 
            int durationTicks, 
            ReaperEmpowermentValues empowermentValues,
            Action<Player> onConsume = null,
            string releaseMessage = null)
        {
            if (ReaperPlayer.SoulReleaseKey.JustPressed)
            {
                var reaperPlayer = player.GetModPlayer<ReaperPlayer>();
                if (reaperPlayer.ConsumeSoulEnergy(cost))
                {
                    // Activate empowerment
                    var empowermentState = player.GetModPlayer<ReaperEmpowermentState>();
                    empowermentState.ActivateEmpowerment(empowermentValues, durationTicks);

                    // Execute consume effect if provided
                    onConsume?.Invoke(player);

                    // Display message
                    string message = releaseMessage ?? $"{(int)cost} souls released!";
                    Main.NewText(message, Color.Green);
                    
                    return true;
                }
                else
                {
                    Main.NewText("Not enough soul energy to activate!", Color.Red);
                }
            }
            return false;
        }

        /// <summary>
        /// Creates empowerment values with only life regeneration.
        /// </summary>
        public static ReaperEmpowermentValues CreateLifeRegenEmpowerment(int lifeRegen)
        {
            return new ReaperEmpowermentValues
            {
                LifeRegen = lifeRegen
            };
        }

        /// <summary>
        /// Creates empowerment values with life regen and reaper damage boost.
        /// </summary>
        public static ReaperEmpowermentValues CreateLifeRegenAndDamageEmpowerment(int lifeRegen, float reaperDamage)
        {
            return new ReaperEmpowermentValues
            {
                LifeRegen = lifeRegen,
                ReaperDamage = reaperDamage
            };
        }

        /// <summary>
        /// Creates empowerment values with defense and reaper damage boost.
        /// </summary>
        public static ReaperEmpowermentValues CreateDefenseAndDamageEmpowerment(int defense, float reaperDamage)
        {
            return new ReaperEmpowermentValues
            {
                Defense = defense,
                ReaperDamage = reaperDamage
            };
        }

        /// <summary>
        /// Creates empowerment values for instant healing (no ongoing empowerment).
        /// Used for weapons that provide immediate heal + temporary buffs.
        /// </summary>
        public static ReaperEmpowermentValues CreateHealingAndBuffEmpowerment(int defense, float reaperDamage)
        {
            return new ReaperEmpowermentValues
            {
                Defense = defense,
                ReaperDamage = reaperDamage
            };
        }

        /// <summary>
        /// Performs instant healing on the player.
        /// Used alongside empowerment for weapons that provide immediate healing.
        /// </summary>
        public static void ApplyInstantHealing(Player player, int healAmount)
        {
            player.statLife += healAmount;
            player.HealEffect(healAmount);
            if (player.statLife > player.statLifeMax2)
                player.statLife = player.statLifeMax2;
        }
    }
}