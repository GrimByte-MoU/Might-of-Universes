using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Common;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Common.Players
{
    public class TempleReaverPlayer : ModPlayer
    {
        public bool FullSetEquipped;
        public bool ChestSoulGenActive;

        // While this > 0, the player cannot gain souls
        private int noSoulGainTimer;

        public override void ResetEffects()
        {
            FullSetEquipped = false;
            ChestSoulGenActive = false;
        }

        public override void UpdateDead()
        {
            noSoulGainTimer = 0;
        }

        public override void PostUpdate()
        {
            if (ChestSoulGenActive)
            {
                // +3 souls/sec from plate
                var reaper = Player.GetModPlayer<ReaperPlayer>();
                reaper.AddSoulEnergy(3f / 60f);
            }

            if (noSoulGainTimer > 0)
                noSoulGainTimer--;
        }

        public override void PostUpdateEquips()
        {
            if (!FullSetEquipped)
                return;

            // +10% DR (set bonus)
            Player.endurance += 0.10f;

            // Double Soul Empowerment duration for soul-consume abilities
            var acc = Player.GetModPlayer<ReaperAccessoryPlayer>();
            // Ensure we at least double; if some other item sets a different multiplier, take the max
            if (acc.EmpowerDurationMultiplier < 2f)
                acc.EmpowerDurationMultiplier = 2f;

            // If under "no soul gain" penalty, block gains by nullifying gather multiplier
            if (noSoulGainTimer > 0)
            {
                var reaper = Player.GetModPlayer<ReaperPlayer>();
                reaper.soulGatherMultiplier = 0f;
            }
        }

        // When the player actually takes damage
        public override void OnHurt(Player.HurtInfo info)
        {
            if (!FullSetEquipped)
                return;

            // If currently Empowered, strip it and apply 5s "no soul gain"
            if (Player.HasBuff(ModContent.BuffType<SoulEmpowerment>()))
            {
                Player.ClearBuff(ModContent.BuffType<SoulEmpowerment>());
                noSoulGainTimer = 60 * 5;

                // Tiny visual for feedback
                for (int i = 0; i < 10; i++)
                {
                    int d = Dust.NewDust(Player.position, Player.width, Player.height, DustID.Smoke, 0f, -1.5f, 150);
                    Main.dust[d].scale = 1.1f;
                    Main.dust[d].noGravity = true;
                }
            }
        }
    }
}