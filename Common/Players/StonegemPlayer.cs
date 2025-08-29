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
                
                // Grant enhanced jump abilities
                Player.jumpSpeedBoost += 0.3f;
                Player.jumpBoost = true;
            }
        }
    }
}