using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace MightofUniverses.Content.Items.Consumables.Food
{
    public class RainbowLollipop : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.useTime = 17;
            Item.useAnimation = 17;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.UseSound = SoundID.Item2;
            Item.consumable = true;
            Item.maxStack = 30;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(silver: 2);
            Item.healLife = 25;
            Item.buffType = ModContent.BuffType<Buffs.Hyper>();
            Item.buffTime = 60 * 60;
        }

        public override bool CanUseItem(Player player)
        {
            return player.statLife < player.statLifeMax2;
        }
    }
}