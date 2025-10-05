using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Content.Items.Buffs;
// If your ReaperDamageClass is in a different namespace, add the correct using
using MightofUniverses.Common; 

namespace MightofUniverses.Common.Players
{
    // Use a class so lambdas like `vals => { vals.LifestealPercent += 5; }` actually mutate it.
    public class ReaperEmpowermentValues
    {
        public float ReaperDamage;          // +% to Reaper damage (e.g., 0.12f = +12%)
        public float AttackSpeed;           // +% Reaper attack speed
        public float CritChance;            // +% Reaper crit chance
        public float Endurance;             // +% DR (0.10f = +10% DR)
        public int   Defense;               // flat defense
        public int   LifeRegen;             // life regen (vanilla units; +2 ~= +1 HP/sec)
        public float DamageTakenMultiplier; // +% damage taken while active (e.g., 0.15f = +15% more dmg)
        public int   BonusMaxSouls;         // temporary +max soul energy while the buff is active
        public int   SoulDrainPerSecond;    // drains this many souls per second while active
        public int   LifestealPercent;      // 5 = 5% of damage dealt returned as healing
        public int   ArmorPenetration;      // flat armor penetration for Reaper damage
    }

    // Holds and applies current empowerment values while the buff is active
    public class ReaperEmpowermentState : ModPlayer
    {
        public ReaperEmpowermentValues Values;

        public bool Empowered => Player.HasBuff(ModContent.BuffType<SoulEmpowerment>());

        public override void ResetEffects()
        {
            // When the buff is not present, clear values. When present, keep last configured values for this tick.
            if (!Empowered)
            {
                Values = null;
            }
        }

        public override void PostUpdate()
        {
            if (!Empowered || Values == null)
                return;

            // Dynamic soul drain
            if (Values.SoulDrainPerSecond > 0 && Player.whoAmI == Main.myPlayer && Main.GameUpdateCount % 60 == 0)
            {
                var reaper = Player.GetModPlayer<ReaperPlayer>();
                float drain = Values.SoulDrainPerSecond;
                if (reaper.soulEnergy >= drain)
                    reaper.soulEnergy -= drain;
                else
                    Player.ClearBuff(ModContent.BuffType<SoulEmpowerment>()); // end early if out of souls
            }

            // Temporary +max souls while empowered
            if (Values.BonusMaxSouls != 0)
            {
                var reaper = Player.GetModPlayer<ReaperPlayer>();
                reaper.maxSoulEnergy += Values.BonusMaxSouls;
                if (reaper.soulEnergy > reaper.maxSoulEnergy)
                    reaper.soulEnergy = reaper.maxSoulEnergy;
            }
        }

        // Apply stat bonuses after equipment is processed
        public override void PostUpdateEquips()
        {
            if (!Empowered || Values == null)
                return;

            var reaperClass = ModContent.GetInstance<ReaperDamageClass>();

            if (Values.ReaperDamage != 0f)
                Player.GetDamage(reaperClass) += Values.ReaperDamage;

            if (Values.AttackSpeed != 0f)
                Player.GetAttackSpeed(reaperClass) += Values.AttackSpeed;

            if (Values.CritChance != 0f)
                Player.GetCritChance(reaperClass) += Values.CritChance;

            if (Values.ArmorPenetration != 0)
                Player.GetArmorPenetration(reaperClass) += Values.ArmorPenetration;

            if (Values.Endurance != 0f)
                Player.endurance += Values.Endurance;

            if (Values.Defense != 0)
                Player.statDefense += Values.Defense;

            if (Values.LifeRegen != 0)
                Player.lifeRegen += Values.LifeRegen; // Note: +2 ~= +1 HP/sec
        }

        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (Empowered && Values != null && Values.DamageTakenMultiplier > 0f)
            {
                modifiers.FinalDamage *= 1f + Values.DamageTakenMultiplier;
            }
        }

        // Lifesteal on hit based on damage dealt
        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            TryLifesteal(damageDone);
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            TryLifesteal(damageDone);
        }

        private void TryLifesteal(int damageDone)
        {
            if (!Empowered || Values == null || Values.LifestealPercent <= 0 || damageDone <= 0)
                return;

            int heal = (int)(damageDone * (Values.LifestealPercent * 0.01f));
            if (heal > 0)
                Player.Heal(heal);
        }

        // Helper to start empowerment for durationTicks and set the values
        public static void Apply(Player player, int durationTicks, System.Action<ReaperEmpowermentValues> configure)
        {
            var state = player.GetModPlayer<ReaperEmpowermentState>();
            var vals = new ReaperEmpowermentValues();
            configure?.Invoke(vals);

            // NEW: apply gear-driven empowerment duration modifiers
            var acc = player.GetModPlayer<ReaperAccessoryPlayer>();
            if (acc.EmpowerDurationMultiplier > 1f)
                durationTicks = (int)(durationTicks * acc.EmpowerDurationMultiplier);
            if (acc.EmpowerExtraDurationTicks > 0)
                durationTicks += acc.EmpowerExtraDurationTicks;

            state.Values = vals;
            player.AddBuff(ModContent.BuffType<SoulEmpowerment>(), durationTicks);
        }
    }
}