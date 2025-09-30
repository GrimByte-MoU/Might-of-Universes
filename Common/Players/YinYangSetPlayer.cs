using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Common.Players
{
    public class YinYangSetPlayer : ModPlayer
    {
        private const int SetBonusMaxSouls = 125;

        private bool wearingYinYangSet;

        public override void ResetEffects()
        {
            wearingYinYangSet = false;
        }

        public override void UpdateEquips()
        {
            if (IsWearingFullYinYangSet())
            {
                wearingYinYangSet = true;
                var reaper = Player.GetModPlayer<ReaperPlayer>();
                reaper.hasReaperArmor = true;

                var acc = Player.GetModPlayer<ReaperAccessoryPlayer>();
                acc.flatMaxSoulsBonus += SetBonusMaxSouls;
            }
        }

        public override void PostUpdateEquips()
        {
            if (wearingYinYangSet)
            {
                Player.setBonus = $"+{SetBonusMaxSouls} max souls\nWhen you consume souls you gain the Yang buff for 3 seconds. If below 50% health you also gain the Yin buff for 3 seconds.";
            }
        }

        public override void PostUpdate()
        {
            if (!wearingYinYangSet) return;

            var reaper = Player.GetModPlayer<ReaperPlayer>();
            if (reaper != null && reaper.justConsumedSouls)
            {
                if (Player.statLife < Player.statLifeMax2 * 0.5f)
                {
                    Player.AddBuff(ModContent.BuffType<YinBuff>(), 180);
                    Player.AddBuff(ModContent.BuffType<YangBuff>(), 180);
                }
                else
                {
                    Player.AddBuff(ModContent.BuffType<YangBuff>(), 180);
                }

                for (int i = 0; i < 10; i++)
                {
                    var pos = Player.Center + Main.rand.NextVector2Circular(12f, 12f);
                    int d = Dust.NewDust(pos, 1, 1, DustID.AncientLight, 0f, 0f, 150, default, 1f);
                    Main.dust[d].noGravity = true;
                }
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item4, Player.position);
            }
        }

        private bool IsWearingFullYinYangSet()
        {
            return Player.armor[0].type == ModContent.ItemType<Content.Items.Armors.YinYangHat>()
                && Player.armor[1].type == ModContent.ItemType<Content.Items.Armors.YinYangCloak>()
                && Player.armor[2].type == ModContent.ItemType<Content.Items.Armors.YinYangShoes>();
        }
    }
}