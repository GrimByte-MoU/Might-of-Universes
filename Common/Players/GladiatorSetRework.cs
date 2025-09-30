using System.Reflection;
using Terraria.Audio;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Common.Players
{
    public class GladiatorSetRework : ModPlayer
    {
        private bool wearingGladiatorSet;
        private int phalanxCooldown;
        private int previousReaperSoulValue = -1;
        private bool attemptedReflectionLookup;

        public override void ResetEffects()
        {
            wearingGladiatorSet = false;
        }

        public override void UpdateEquips()
        {
            if (IsWearingFullGladiatorSet())
            {
                wearingGladiatorSet = true;
                var reaper = Player.GetModPlayer<ReaperPlayer>();
                reaper.hasReaperArmor = true;
                reaper.maxSoulEnergy += 60;
                Player.noKnockback = true;
            }
        }

        public override void PostUpdateEquips()
        {
            if (wearingGladiatorSet)
            {
                Player.setBonus = "+60 max souls\nImmunity to knockback\nWhen consuming souls, gain the Phalanx buff for 10 seconds (refreshes duration, 2s internal cooldown)";
            }
        }

        public override void PostUpdate()
        {
            if (phalanxCooldown > 0)
                phalanxCooldown--;

            if (!wearingGladiatorSet)
            {
                previousReaperSoulValue = -1;
                return;
            }

            var reaper = Player.GetModPlayer<ReaperPlayer>();
            int current = TryGetReaperSoulValue(reaper);
            if (current >= 0)
            {
                if (previousReaperSoulValue >= 0 && current < previousReaperSoulValue)
                {
                    TryTriggerPhalanx();
                }
                previousReaperSoulValue = current;
            }
            else
            {
                if (!attemptedReflectionLookup)
                {
                    attemptedReflectionLookup = true;
                }
            }
        }

        public void OnSoulConsumed()
        {
            if (!wearingGladiatorSet) return;
            TryTriggerPhalanx();
        }

        private void TryTriggerPhalanx()
        {
            if (phalanxCooldown > 0) return;
            phalanxCooldown = 120;
            Player.AddBuff(ModContent.BuffType<PhalanxBuff>(), 600);
            for (int i = 0; i < 12; i++)
            {
                var pos = Player.Center + Main.rand.NextVector2Circular(16f, 16f);
                int d = Dust.NewDust(pos, 1, 1, DustID.GoldCoin, 0f, 0f, 150, default, 1f);
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity *= 0.3f;
            }
            SoundEngine.PlaySound(SoundID.Item4, Player.position);
        }

        private bool IsWearingFullGladiatorSet()
        {
            return Player.armor[0].type == ItemID.GladiatorHelmet
                && Player.armor[1].type == ItemID.GladiatorBreastplate
                && Player.armor[2].type == ItemID.GladiatorLeggings;
        }

        private int TryGetReaperSoulValue(ReaperPlayer reaper)
        {
            var t = reaper.GetType();
            string[] candidates = new[]
            {
                "soulEnergy",
                "soulEnergyCurrent",
                "currentSoulEnergy",
                "currentSoul",
                "souls",
                "soul",
                "soulCount",
                "SoulEnergy",
                "Soul"
            };

            foreach (var name in candidates)
            {
                var f = t.GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (f != null)
                {
                    var val = f.GetValue(reaper);
                    if (val is int iv) return iv;
                    if (val is float fv) return (int)fv;
                    if (val is double dv) return (int)dv;
                }
                var p = t.GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (p != null)
                {
                    var val = p.GetValue(reaper);
                    if (val is int iv) return iv;
                    if (val is float fv) return (int)fv;
                    if (val is double dv) return (int)dv;
                }
            }

            return -1;
        }
    }
}