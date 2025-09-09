using System;
using Terraria;
using Terraria.ModLoader;
using MightofUniverses.Common;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Common.Players
{
    /// <summary>
    /// Values for a reaper empowerment state. All values are optional with sensible defaults.
    /// </summary>
    public struct ReaperEmpowermentValues
    {
        public float ReaperDamage;
        public float AttackSpeed;
        public float CritChance;
        public float Endurance; // damage reduction percentage (0.1f = 10% DR)
        public int Defense;
        public int LifeRegen;
        public float DamageTakenMultiplier; // +% damage taken while empowered (0.1f = +10% damage taken)
        public int BonusMaxSouls; // temporary +max soul energy
        public int SoulDrainPerSecond; // drains souls while empowered; ends early if empty
        public float LifestealPercent; // heals % of damage when empowered (0.05f = 5%)
        public int ArmorPenetrationReaper; // flat armor pen for Reaper damage while empowered
        public bool IgnoreDefense; // when true, Reaper damage effectively ignores defense

        public static ReaperEmpowermentValues Default => new ReaperEmpowermentValues();
    }

    /// <summary>
    /// ModPlayer that manages the centralized reaper empowerment system.
    /// Stores empowerment values and applies dynamic effects.
    /// </summary>
    public class ReaperEmpowermentState : ModPlayer
    {
        private ReaperEmpowermentValues currentEmpowerment;
        private int soulDrainTimer = 0; // Timer for soul drain (counts up to 60)
        private bool hasEmpowerment = false;

        public bool IsEmpowered => hasEmpowerment && Player.HasBuff<SoulEmpowerment>();

        public ReaperEmpowermentValues CurrentEmpowerment => currentEmpowerment;

        public override void ResetEffects()
        {
            // Check if empowerment buff is active
            if (!Player.HasBuff<SoulEmpowerment>())
            {
                hasEmpowerment = false;
                currentEmpowerment = ReaperEmpowermentValues.Default;
                soulDrainTimer = 0;
            }
        }

        public override void PostUpdateMiscEffects()
        {
            if (IsEmpowered)
            {
                // Handle soul drain
                if (currentEmpowerment.SoulDrainPerSecond > 0)
                {
                    soulDrainTimer++;
                    if (soulDrainTimer >= 60) // Every second
                    {
                        soulDrainTimer = 0;
                        var reaperPlayer = Player.GetModPlayer<ReaperPlayer>();
                        if (!reaperPlayer.ConsumeSoulEnergy(currentEmpowerment.SoulDrainPerSecond))
                        {
                            // Not enough soul energy, end empowerment early
                            Player.ClearBuff(ModContent.BuffType<SoulEmpowerment>());
                        }
                    }
                }

                // Apply stat effects
                ApplyEmpowermentEffects();
            }
        }

        private void ApplyEmpowermentEffects()
        {
            // Bonus max souls
            if (currentEmpowerment.BonusMaxSouls != 0)
            {
                var reaperPlayer = Player.GetModPlayer<ReaperPlayer>();
                reaperPlayer.maxSoulEnergy += currentEmpowerment.BonusMaxSouls;
            }

            // Defense
            if (currentEmpowerment.Defense != 0)
            {
                Player.statDefense += currentEmpowerment.Defense;
            }

            // Life regen
            if (currentEmpowerment.LifeRegen != 0)
            {
                Player.lifeRegen += currentEmpowerment.LifeRegen;
            }

            // Reaper damage
            if (currentEmpowerment.ReaperDamage != 0)
            {
                Player.GetDamage<ReaperDamageClass>() += currentEmpowerment.ReaperDamage;
            }

            // Attack speed
            if (currentEmpowerment.AttackSpeed != 0)
            {
                Player.GetAttackSpeed<ReaperDamageClass>() += currentEmpowerment.AttackSpeed;
            }

            // Crit chance
            if (currentEmpowerment.CritChance != 0)
            {
                Player.GetCritChance<ReaperDamageClass>() += currentEmpowerment.CritChance;
            }

            // Endurance (damage reduction)
            if (currentEmpowerment.Endurance != 0)
            {
                Player.endurance += currentEmpowerment.Endurance;
            }
        }

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (IsEmpowered && item.DamageType == ModContent.GetInstance<ReaperDamageClass>())
            {
                ApplyLifesteal(damageDone);
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (IsEmpowered && proj.DamageType == ModContent.GetInstance<ReaperDamageClass>())
            {
                ApplyLifesteal(damageDone);
            }
        }

        private void ApplyLifesteal(int damageDone)
        {
            if (currentEmpowerment.LifestealPercent > 0 && damageDone > 0)
            {
                int healAmount = (int)(damageDone * currentEmpowerment.LifestealPercent);
                if (healAmount < 1) healAmount = 1;

                Player.HealEffect(healAmount);
                Player.statLife += healAmount;
                if (Player.statLife > Player.statLifeMax2)
                    Player.statLife = Player.statLifeMax2;
            }
        }

        public override void ModifyHitNPC(Item item, NPC target, ref NPC.HitModifier hitModifier)
        {
            if (IsEmpowered && item.DamageType == ModContent.GetInstance<ReaperDamageClass>())
            {
                ApplyArmorPenetration(ref hitModifier);
            }
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifier hitModifier)
        {
            if (IsEmpowered && proj.DamageType == ModContent.GetInstance<ReaperDamageClass>())
            {
                ApplyArmorPenetration(ref hitModifier);
            }
        }

        private void ApplyArmorPenetration(ref NPC.HitModifier hitModifier)
        {
            if (currentEmpowerment.IgnoreDefense)
            {
                hitModifier.ArmorPenetration += 9999; // Effectively ignore defense
            }
            else if (currentEmpowerment.ArmorPenetrationReaper > 0)
            {
                hitModifier.ArmorPenetration += currentEmpowerment.ArmorPenetrationReaper;
            }
        }

        public override void ModifyHurt(ref Player.HurtModifier hurtModifier)
        {
            if (IsEmpowered && currentEmpowerment.DamageTakenMultiplier != 0)
            {
                hurtModifier.FinalDamage *= (1f + currentEmpowerment.DamageTakenMultiplier);
            }
        }

        /// <summary>
        /// Activates empowerment with the specified values and duration.
        /// </summary>
        public void ActivateEmpowerment(ReaperEmpowermentValues values, int durationTicks)
        {
            currentEmpowerment = values;
            hasEmpowerment = true;
            soulDrainTimer = 0;
            Player.AddBuff(ModContent.BuffType<SoulEmpowerment>(), durationTicks);
        }

        /// <summary>
        /// Clears current empowerment.
        /// </summary>
        public void ClearEmpowerment()
        {
            hasEmpowerment = false;
            currentEmpowerment = ReaperEmpowermentValues.Default;
            soulDrainTimer = 0;
            Player.ClearBuff(ModContent.BuffType<SoulEmpowerment>());
        }
    }
}