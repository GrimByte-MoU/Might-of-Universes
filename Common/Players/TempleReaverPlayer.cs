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

            if (!FullSetEquipped) return;

            Lighting.AddLight(Player.Center, 0.8f, 0.6f, 0.3f);

            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, DustID.Lihzahrd, 0f, 0f, 100, default, 0.8f);
                dust.noGravity = true;
                dust.fadeIn = 0.2f;
            }

            if (Main.rand.NextBool(4))
            {
                Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, DustID.Torch, 0f, 0f, 100, default, 0.8f);
                dust.noGravity = true;
                dust.fadeIn = 0.2f;
            }
        }

        public override void PostUpdateEquips()
        {
            if (!FullSetEquipped)
                return;
            Player.endurance += 0.10f;
            var acc = Player.GetModPlayer<ReaperAccessoryPlayer>();
            if (acc.EmpowerDurationMultiplier < 2f)
                acc.EmpowerDurationMultiplier = 2f;
            if (noSoulGainTimer > 0)
            {
                var reaper = Player.GetModPlayer<ReaperPlayer>();
                reaper.soulGatherMultiplier = 0f;
            }
        }
        public override void OnHurt(Player.HurtInfo info)
        {
            if (!FullSetEquipped)
                return;
            if (Player.HasBuff(ModContent.BuffType<SoulEmpowerment>()))
            {
                Player.ClearBuff(ModContent.BuffType<SoulEmpowerment>());
                noSoulGainTimer = 60 * 5;
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