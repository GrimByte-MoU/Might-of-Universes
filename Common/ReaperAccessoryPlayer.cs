using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MightofUniverses.Common.Players
{
    /// <summary>
    /// Aggregates ALL accessory-driven or passive modifiers that influence:
    ///  - Soul cost calculation
    ///  - Refund mechanics
    ///  - Crit-based soul returns
    ///  - Death-prevention (Ferryman chain)
    ///  - Future upgrade scaling hooks
    /// 
    /// IMPORTANT LIFECYCLE:
    ///   ResetEffects()  -> Accessories re-apply modifiers in UpdateAccessory().
    ///   RecordSoulSpend -> Called ONLY by ReaperPlayer.ConsumeSoulEnergy() after final net spend.
    ///   TryActivateFerrymanFatalSave -> Called from FreeDodge (preferred) before damage is finalized.
    /// 
    /// UPGRADE PATTERNS:
    ///   - Cost reduction:    SoulCostMultiplier *= 0.90f;
    ///   - Flat reduction:    SoulCostFlatReduction += 2;
    ///   - Guaranteed refund: GuaranteedSoulRefund = Math.Max(GuaranteedSoulRefund, 1);
    ///   - Refund chance:     RefundChance += 0.10f; RefundFraction = Math.Max(RefundFraction, 0.50f);
    ///   - Heal scaling:      FerrymanHealScale = Math.Max(FerrymanHealScale, 0.60f);
    ///   - Cooldown scaling:  FerrymanCooldownMultiplier *= 0.80f; FerrymanCooldownFlatReduction += 300;
    /// </summary>
    public class ReaperAccessoryPlayer : ModPlayer
    {
        // =========================
        // Soul Cost Modification
        // =========================
        public float SoulCostMultiplier;
        public int   SoulCostFlatReduction;

        // Safety clamp (can be tuned): prevents >90% total reduction through stacking.
        public const float MinEffectiveCostMultiplier = 0.10f;

        // =========================
        // Refund Mechanics
        // =========================
        public float RefundChance;      // 0.10f = 10% chance
        public float RefundFraction;    // 0.50f = refund 50% (rounded down)
        public int   GuaranteedSoulRefund; // Flat souls always refunded (applied before fraction logic)
        public bool  RefundAtLeastOne;     // If true, ensures net spend ≥ 1 (default behavior anyway)

        // =========================
        // Spend Tracking
        // =========================
        public int LastSoulSpendAmount;      // Final net souls consumed (after all refunds)
        public int LargestSoulSpendWindow;   // Largest spend in rolling window
        public int LargestSoulSpendTimer;    // Ticks until window resets
        public int LargestWindowDuration = 60 * 10; // 10 seconds default

        // If true, Ferryman will use LargestSoulSpendWindow instead of LastSoulSpendAmount for heal baseline.
        public bool UseLargestSpendWindowForFerryman;

        // =========================
        // Ferryman’s Token (cheat-death)
        // =========================
        public bool HasFerrymansToken;
        public int  FerrymanCooldownTicks;
        public const int FerrymanBaseCooldown = 60 * 60; // 60s
        public float FerrymanCooldownMultiplier;         // Multiplicative
        public int   FerrymanCooldownFlatReduction;      // Flat ticks removed
        public float FerrymanHealScale;                  // Scales chosen spend (default 0.5)
        public int   FerrymanMaxHeal;                    // 0 => uncapped

        // Debuff clearing policy
        public bool ClearOnlyDebuffs = true;

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

        // =========================
        // ResetEffects - stateless aggregation root
        // =========================
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
            FerrymanHealScale          = 0.5f;
            FerrymanMaxHeal            = 0;

            HasUndertakersBrooch       = false;
            HasGravediggersRing        = false;
            HasCharmOfDepths           = false;
            HasMonkeysPaw              = false;
            HasSoulSiphoningArtifact   = false;

            UseLargestSpendWindowForFerryman = false;
            ClearOnlyDebuffs = true;
        }

        // =========================
        // PostUpdate - timers
        // =========================
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

        // =========================
        // Spend Recording (called AFTER refunds finalize)
        // =========================
        public void RecordSoulSpend(int finalSpent)
        {
            LastSoulSpendAmount = finalSpent;

            // Rolling window
            if (finalSpent > LargestSoulSpendWindow)
            {
                LargestSoulSpendWindow = finalSpent;
                LargestSoulSpendTimer  = LargestWindowDuration;
            }
        }

        // =========================
        // Reaper Crit Tracking
        // =========================
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

        // =========================
        // Ferryman Core
        // =========================
        private int ComputeFerrymanCooldown()
        {
            int baseTicks = (int)(FerrymanBaseCooldown * FerrymanCooldownMultiplier) - FerrymanCooldownFlatReduction;
            return Math.Max(60, baseTicks); // Minimum 1 second
        }

        private int GetSpendForHeal()
        {
            if (UseLargestSpendWindowForFerryman && LargestSoulSpendWindow > 0)
                return LargestSoulSpendWindow;
            return LastSoulSpendAmount;
        }

        private int ComputeFerrymanHeal()
        {
            int spend = GetSpendForHeal();
            if (spend <= 0)
                return 1;

            double raw = spend * FerrymanHealScale;
            int heal = (int)Math.Ceiling(raw);
            if (heal <= 0)
                heal = 1;
            if (FerrymanMaxHeal > 0)
                heal = Math.Min(heal, FerrymanMaxHeal);
            return heal;
        }

        public bool TryActivateFerrymanFatalSave(int incomingDamage)
        {
            if (!HasFerrymansToken || FerrymanCooldownTicks > 0)
                return false;

            if (incomingDamage < Player.statLife)
                return false; // Not lethal

            int heal = ComputeFerrymanHeal();

            Player.statLife = Math.Min(Player.statLifeMax2, Player.statLife + heal);
            Player.HealEffect(heal, broadcast: true);

            // Clear debuffs OR everything, based on policy
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

            Player.immune = true;
            Player.immuneTime = Math.Max(Player.immuneTime, 60);

            // Visual feedback
            for (int i = 0; i < 40; i++)
            {
                var d = Dust.NewDustDirect(Player.position, Player.width, Player.height, 15, Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-3f, -0.5f));
                d.scale = 1.2f + Main.rand.NextFloat(0.3f);
                d.noGravity = true;
            }

            FerrymanCooldownTicks = ComputeFerrymanCooldown();
            return true;
        }

        // Preferred modern hook
        public override bool FreeDodge(Player.HurtInfo info)
        {
            if (TryActivateFerrymanFatalSave(info.Damage))
                return true;
            return false;
        }

        // Optional fallback (leave commented unless you want redundancy)
        // public override bool PreHurt(
        //     bool pvp, ref int damage, ref int hitDirection, ref bool crit,
        //     ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        // {
        //     if (TryActivateFerrymanFatalSave(damage))
        //     {
        //         damage = 0;
        //         return true;
        //     }
        //     return true;
        // }

        public override void OnRespawn()
        {
            // Optional: reset crit chain or keep it. We keep it.
            // ReaperCritCounter = 0;
        }

        // =========================
        // UI / External Helpers
        // =========================
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

        // =========================
        // (Optional) Networking Skeleton
        // Uncomment & implement if you need multiplayer syncing of cooldown / window.
        // =========================
        /*
        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket p = Mod.GetPacket();
            p.Write((byte)MessageType.SyncReaperAccessoryPlayer);
            p.Write((byte)Player.whoAmI);
            p.Write(FerrymanCooldownTicks);
            p.Write(LastSoulSpendAmount);
            p.Write(LargestSoulSpendWindow);
            p.Send(toWho, fromWho);
        }

        public void ReceivePacket(BinaryReader r)
        {
            FerrymanCooldownTicks = r.ReadInt32();
            LastSoulSpendAmount   = r.ReadInt32();
            LargestSoulSpendWindow = r.ReadInt32();
        }
        */
    }
}