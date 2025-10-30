using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Content.Items.Armors;

namespace MightofUniverses.Common.Players
{
    public class MorticianSetPlayer : ModPlayer
    {
        private const int MaxSoulBonus = 40;
        private bool wearing;

        public override void ResetEffects()
        {
            wearing = false;
        }

        public override void UpdateEquips()
        {
            if (Player.armor[0].type == ModContent.ItemType<MorticianHat>()
             && Player.armor[1].type == ModContent.ItemType<MorticianChestplate>()
             && Player.armor[2].type == ModContent.ItemType<MorticianGreaves>())
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
                Player.setBonus = "+40 max souls\nConsuming souls grants Mortician's Grace (5s).";
        }

        public override void PostUpdate()
        {
            if (!wearing) return;
            var reaper = Player.GetModPlayer<ReaperPlayer>();
            if (reaper.justConsumedSouls)
            {
                Player.AddBuff(ModContent.BuffType<MorticianGraceBuff>(), 300);
                for (int i = 0; i < 8; i++)
                {
                    var pos = Player.Center + Main.rand.NextVector2Circular(12f, 12f);
                    int d = Dust.NewDust(pos, 1, 1, DustID.BlueCrystalShard, 0f, 0f, 150, default, 0.9f);
                    Main.dust[d].noGravity = true;
                    Main.dust[d].velocity *= 0.2f;
                }
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item4, Player.position);
            }

            Lighting.AddLight(Player.Center, 0f, 0.3f, 0.8f);

            if (Main.rand.NextBool(5))
            {
                Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, DustID.BlueCrystalShard, 0f, 0f, 100, default, 0.8f);
                dust.noGravity = true;
                dust.fadeIn = 0.2f;
            }

            if (Main.rand.NextBool(6))
            {
                Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, DustID.Smoke, 0f, 0f, 100, default, 0.6f);
                dust.noGravity = true;
                dust.fadeIn = 0.1f;
            }
        }
    }
}