using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;
using Terraria.Audio;
using MightofUniverses.Content.Items.Armors;

namespace MightofUniverses.Common.Players
{
    public class MorticianSetPlayer : ModPlayer
    {
        private const int SetBonusMaxSouls = 40;

        private bool wearingMorticianSet;

        public override void ResetEffects()
        {
            wearingMorticianSet = false;
        }

        public override void UpdateEquips()
        {
            if (IsWearingFullMorticianSet())
            {
                wearingMorticianSet = true;
                var reaper = Player.GetModPlayer<ReaperPlayer>();
                reaper.hasReaperArmor = true;

                var acc = Player.GetModPlayer<ReaperAccessoryPlayer>();
                acc.flatMaxSoulsBonus += SetBonusMaxSouls;
            }
        }

        public override void PostUpdateEquips()
        {
            if (wearingMorticianSet)
            {
                Player.setBonus = $"+{SetBonusMaxSouls} max souls\nWhen you consume souls, gain the Mortician's Grace bufffor 5 seconds";
            }
        }

        public override void PostUpdate()
        {
            if (!wearingMorticianSet)
                return;

            var reaper = Player.GetModPlayer<ReaperPlayer>();
            if (reaper != null && reaper.justConsumedSouls)
            {
                Player.AddBuff(ModContent.BuffType<MorticianGraceBuff>(), 300);
                for (int i = 0; i < 8; i++)
                {
                    var pos = Player.Center + Main.rand.NextVector2Circular(12f, 12f);
                    int d = Dust.NewDust(pos, 1, 1, DustID.BlueCrystalShard, 0f, 0f, 150, default, 0.9f);
                    Main.dust[d].noGravity = true;
                    Main.dust[d].velocity *= 0.2f;
                }
                SoundEngine.PlaySound(SoundID.Item4, Player.position);
            }
        }

        private bool IsWearingFullMorticianSet()
        {
            return Player.armor[0].type == ModContent.ItemType<MorticianHat>()
                && Player.armor[1].type == ModContent.ItemType<MorticianChestplate>()
                && Player.armor[2].type == ModContent.ItemType<MorticianGreaves>();
        }
    }
}