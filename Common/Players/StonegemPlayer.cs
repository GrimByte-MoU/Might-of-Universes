using Terraria;
using Terraria.ModLoader;

namespace MightofUniverses.Common.Players
{
    public class StonegemPlayer : ModPlayer
    {
        public bool hasStonegemSet = false;

        public override void ResetEffects()
        {
            hasStonegemSet = false;
        }

        public override void PostUpdateEquips()
        {
            if (Player.armor[0].type == ModContent.ItemType<Content.Items.Armors.StonegemMask>() &&
                Player.armor[1].type == ModContent.ItemType<Content.Items.Armors.StonegemChestplate>() &&
                Player.armor[2].type == ModContent.ItemType<Content.Items.Armors.StonegemKilt>())
            {
                hasStonegemSet = true;
                Player.GetModPlayer<PacifistPlayer>().pacifistDamageMultiplier += 0.25f;
                Player.jumpSpeedBoost += 0.3f;
                Player.jumpBoost = true;
            }
        }

        public override void PostUpdate()
        {
            if (!hasStonegemSet) return;

            Lighting.AddLight(Player.Center, 0.5f, 0.3f, 0.7f);

            if (Main.rand.NextBool(3))
            {
                int[] gemDusts = { DustID.GemRuby, DustID.GemSapphire, DustID.GemEmerald, DustID.GemDiamond };
                Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, Main.rand.Next(gemDusts), 0f, 0f, 100, default, 0.8f);
                dust.noGravity = true;
                dust.fadeIn = 0.2f;
            }

            if (Main.rand.NextBool(4))
            {
                Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, DustID.Dirt, 0f, 0f, 100, default, 0.8f);
                dust.noGravity = true;
                dust.fadeIn = 0.2f;
            }
        }
    }
}