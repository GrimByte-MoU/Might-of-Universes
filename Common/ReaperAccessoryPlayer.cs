using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MightofUniverses.Common.Players
{
    /// <summary>
    /// Accessory-driven modifiers & Ferryman cheat-death.
    /// Now simplified: Ferryman consumes ALL current souls for the heal.
    /// </summary>
    public class ReaperAccessoryPlayer : ModPlayer
    {
        // =========================
        // Soul Cost Modification
        // =========================
        public float SoulCostMultiplier;
        public int   SoulCostFlatReduction;
        public const float MinEffectiveCostMultiplier = 0.10f;

        // =========================
        // Refund Mechanics
        // =========================
        public float RefundChance;
        public float RefundFraction;
        public int   GuaranteedSoulRefund;
        public bool  RefundAtLeastOne;

        // =========================
        // Spend Tracking (still kept in case other systems rely on it)
        // =========================
        public int LastSoulSpendAmount;
        public int LargestSoulSpendWindow;
        public int LargestSoulSpendTimer;
        public int LargestWindowDuration = 60 * 10;
        public bool UseLargestSpendWindowForFerryman; // No longer used for heal, retained for future

        // =========================
        // Ferryman’s Token (cheat-death)
        // =========================
        public bool HasFerrymansToken;
        public int  FerrymanCooldownTicks;
        public const int FerrymanBaseCooldown = 60 * 60; // 60 seconds
        public float FerrymanCooldownMultiplier;
        public int   FerrymanCooldownFlatReduction;

        // Heal scale: each soul consumed → HealScale HP (converted via Ceil)
        public float FerrymanHealScale;
        public int   FerrymanMaxHeal;      // 0 => uncapped

        // New simplified config: consume all & heal. Some optional tuning knobs:
        public int   FerrymanMinHealFloor = 1;   // Minimal heal after scaling (can raise if you wish)
        public bool  FerrymanSetLifeToBrink = true; // If true, set life to 1 first then add heal

        public bool  ClearOnlyDebuffs = true;

        // =========================
        // Crit / Misc Flags
        // =========================
        public bool HasUndertakersBrooch;
        public int  ReaperCritCounter;
        public int  CritsPerReward = 5;
        public int  CritRewardSouls = 5;

        public bool HasGravediggersRing;
        public bool HasCharmOfDepths;
        public bool HasMonkeysPaw;
        public bool HasSoulSiphoningArtifact;

        public override void ResetEffects()
        {
            SoulCostMultiplier         = 1f;
            SoulCostFlatReduction      = 0;

            RefundChance               = 0f;
            RefundFraction             = 0f;
            GuaranteedSoulRefund       = 0;
            RefundAtLeastOne           = true;

            HasFerrymansToken          = false;
            FerrymanCooldownMultiplier = 1f;
            FerrymanCooldownFlatReduction = 0;
            FerrymanHealScale          = 0.5f; // Example: every soul = 0.5 HP (tune in your accessory)
            FerrymanMaxHeal            = 0;

            HasUndertakersBrooch       = false;
            HasGravediggersRing        = false;
            HasCharmOfDepths           = false;
            HasMonkeysPaw              = false;
            HasSoulSiphoningArtifact   = false;

            UseLargestSpendWindowForFerryman = false;
            ClearOnlyDebuffs = true;
        }

        public override void PostUpdate()
        {
            if (FerrymanCooldownTicks > 0)
                FerrymanCooldownTicks--;

            if (LargestSoulSpendTimer > 0)
            {
                LargestSoulSpendTimer--;
                if (LargestSoulSpendTimer <= 0)
                {
                    LargestSoulSpendTimer = 0;
                    LargestSoulSpendWindow = 0;
                }
            }
        }

        public void RecordSoulSpend(int finalSpent)
        {
            LastSoulSpendAmount = finalSpent;
            if (finalSpent > LargestSoulSpendWindow)
            {
                LargestSoulSpendWindow = finalSpent;
                LargestSoulSpendTimer  = LargestWindowDuration;
            }
        }

        public void RegisterReaperCrit()
        {
            if (!HasUndertakersBrooch)
                return;

            ReaperCritCounter++;
            if (ReaperCritCounter >= CritsPerReward)
            {
                ReaperCritCounter = 0;
                var reaper = Player.GetModPlayer<ReaperPlayer>();
                reaper.AddSoulEnergy(CritRewardSouls, Player.Center);
            }
        }

        private int ComputeFerrymanCooldown()
        {
            int baseTicks = (int)(FerrymanBaseCooldown * FerrymanCooldownMultiplier) - FerrymanCooldownFlatReduction;
            return Math.Max(60, baseTicks);
        }

        // =========================
        // NEW CORE: Consume all souls
        // =========================
        private int ComputeHealFromConsumed(int soulsConsumed)
        {
            double raw = soulsConsumed * FerrymanHealScale;
            int heal = (int)Math.Ceiling(raw);
            if (heal < FerrymanMinHealFloor) heal = FerrymanMinHealFloor;
            if (FerrymanMaxHeal > 0) heal = Math.Min(heal, FerrymanMaxHeal);
            return heal;
        }

        public bool TryActivateFerrymanFatalSave(int incomingDamage)
        {
            if (!HasFerrymansToken || FerrymanCooldownTicks > 0)
                return false;

            if (incomingDamage < Player.statLife)
                return false; // Not lethal

            var reaper = Player.GetModPlayer<ReaperPlayer>();
            int soulsCurrent = (int)Math.Floor(reaper.soulEnergy);

            if (soulsCurrent <= 0)
                return false; // No souls to pay the ferryman → no save

            // Consume ALL current souls
            reaper.soulEnergy = 0f;

            int heal = ComputeHealFromConsumed(soulsCurrent);

            if (FerrymanSetLifeToBrink)
                Player.statLife = Math.Min(Player.statLifeMax2, 1 + heal);
            else
                Player.statLife = Math.Min(Player.statLifeMax2, Player.statLife + heal);

            Player.HealEffect(heal, broadcast: true);

            // Clear debuffs (or all buffs depending on policy)
            for (int i = Player.buffType.Length - 1; i >= 0; i--)
            {
                int b = Player.buffType[i];
                if (b <= 0) continue;

                if (ClearOnlyDebuffs)
                {
                    if (Main.debuff[b])
                        Player.DelBuff(i);
                }
                else
                {
                    Player.DelBuff(i);
                }
            }

            // Immunity frames
            Player.immune = true;
            Player.immuneTime = Math.Max(Player.immuneTime, 60);

            // Visual feedback
            for (int i = 0; i < 40; i++)
            {
                var d = Dust.NewDustDirect(Player.position, Player.width, Player.height, 15,
                    Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-3f, -0.5f));
                d.scale = 1.15f + Main.rand.NextFloat(0.25f);
                d.noGravity = true;
            }

            FerrymanCooldownTicks = ComputeFerrymanCooldown();

            CombatText.NewText(Player.getRect(), Color.Gold, $"Souls: {soulsCurrent}");

            return true;
        }

        public override bool FreeDodge(Player.HurtInfo info)
        {
            if (TryActivateFerrymanFatalSave(info.Damage))
                return true;
            return false;
        }

        public override void OnRespawn()
        {
            // Leave crit counters intact (design choice)
        }

        // UI helpers
        public float GetFerrymanCooldownFraction()
        {
            int denom = ComputeFerrymanCooldown();
            if (FerrymanCooldownTicks <= 0) return 0f;
            return Math.Clamp(FerrymanCooldownTicks / (float)denom, 0f, 1f);
        }

        public int GetFerrymanCooldownSeconds()
        {
            if (FerrymanCooldownTicks <= 0) return 0;
            return (int)Math.Ceiling(FerrymanCooldownTicks / 60f);
        }

        public string GetFerrymanStatusText()
        {
            if (!HasFerrymansToken)
                return "Ferryman inactive";
            return FerrymanCooldownTicks > 0
                ? $"Ferryman cooldown: {GetFerrymanCooldownSeconds()}s"
                : "Ferryman ready";
        }
    }
}