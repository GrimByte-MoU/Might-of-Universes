using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using MightofUniverses.Content.Rarities;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Consumables
{
    public class PearlOfTheAbyss : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item2;
            Item.rare = ModContent.RarityType<EldritchRarity>();
            Item.consumable = true;
            Item.maxStack = 1;
        }

        public override bool? UseItem(Player player)
        {
            var modPlayer = player.GetModPlayer<PearlOfTheAbyssPlayer>();
            if (!modPlayer.usedPearlOfTheAbyss)
            {
                modPlayer.usedPearlOfTheAbyss = true;
                CombatText.NewText(player.Hitbox, CombatText.HealLife, 25);
                return true;
            }
            return false;
        }
    }
}