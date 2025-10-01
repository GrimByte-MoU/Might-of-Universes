using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Common.Players
{
    public class YinYangSetPlayer : ModPlayer
    {
        private const int MaxSoulBonus = 125;
        private bool wearing;

        public override void ResetEffects()
        {
            wearing = false;
        }

        public override void UpdateEquips()
        {
            if (Player.armor[0].type == ModContent.ItemType<Content.Items.Armors.YinYangHat>()
             && Player.armor[1].type == ModContent.ItemType<Content.Items.Armors.YinYangCloak>()
             && Player.armor[2].type == ModContent.ItemType<Content.Items.Armors.YinYangShoes>())
            {
                wearing = true;
                var reaper = Player.GetModPlayer<ReaperPlayer>();
                reaper.hasReaperArmor = true;
                reaper.maxSoulEnergy += MaxSoulBonus;
            }
        }

        public override void PostUpdateEquips()
        {
            if (wearing)
                Player.setBonus = "+125 max souls\nConsuming souls grants Yang (3s). Also grants Yin (3s) if below 50% life.";
        }

        public override void PostUpdate()
        {
            if (!wearing) return;

            var reaper = Player.GetModPlayer<ReaperPlayer>();
            if (reaper.justConsumedSouls)
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
    }
}