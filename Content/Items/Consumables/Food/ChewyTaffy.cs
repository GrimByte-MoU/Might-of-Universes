using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace MightofUniverses.Content.Items.Consumables.Food
{
        public class ChewyTaffy : ModItem
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
            Item.healLife = 10;
        }

        public override bool? UseItem(Player player)
        {
            player.AddBuff(BuffID.Regeneration, 60 * 60);
            player.AddBuff(BuffID.ManaRegeneration, 60 * 60);
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            return player.statLife < player.statLifeMax2;
        }
    }
}