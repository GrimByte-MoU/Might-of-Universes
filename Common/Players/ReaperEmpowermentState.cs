using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Common; 

namespace MightofUniverses.Common.Players
{
    public class ReaperEmpowermentValues
    {
        public float ReaperDamage;
        public float AttackSpeed;
        public float CritChance;
        public float Endurance;
        public int   Defense;
        public int   LifeRegen;
        public float DamageTakenMultiplier;
        public int   BonusMaxSouls;
        public int   SoulDrainPerSecond;
        public int   LifestealPercent;
        public int   ArmorPenetration;
    }
    public class ReaperEmpowermentState : ModPlayer
    {
        public ReaperEmpowermentValues Values;

        public bool Empowered => Player.HasBuff(ModContent.BuffType<SoulEmpowerment>());

        public override void ResetEffects()
        {
            if (!Empowered)
            {
                Values = null;
            }
        }

        public override void PostUpdate()
        {
            if (!Empowered || Values == null)
                return;

            if (Values.SoulDrainPerSecond > 0 && Player.whoAmI == Main.myPlayer && Main.GameUpdateCount % 60 == 0)
            {
                var reaper = Player.GetModPlayer<ReaperPlayer>();
                float drain = Values.SoulDrainPerSecond;
                if (reaper.soulEnergy >= drain)
                    reaper.soulEnergy -= drain;
                else
                    Player.ClearBuff(ModContent.BuffType<SoulEmpowerment>());
            }

            if (Values.BonusMaxSouls != 0)
            {
                var reaper = Player.GetModPlayer<ReaperPlayer>();
                reaper.maxSoulEnergy += Values.BonusMaxSouls;
                if (reaper.soulEnergy > reaper.maxSoulEnergy)
                    reaper.soulEnergy = reaper.maxSoulEnergy;
            }
        }

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
                Player.lifeRegen += Values.LifeRegen;
        }

        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (Empowered && Values != null && Values.DamageTakenMultiplier > 0f)
            {
                modifiers.FinalDamage *= 1f + Values.DamageTakenMultiplier;
            }
        }

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

        public static void Apply(Player player, int durationTicks, System.Action<ReaperEmpowermentValues> configure)
        {
            var state = player.GetModPlayer<ReaperEmpowermentState>();
            var vals = new ReaperEmpowermentValues();
            configure?.Invoke(vals);
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