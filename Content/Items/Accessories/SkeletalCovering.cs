using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;
using MightofUniverses.Common;

namespace MightofUniverses.Content.Items.Accessories
{
    public class SkeletalCovering : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Green;
            Item.DamageType = ModContent.GetInstance<PacifistDamageClass>();
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statDefense += 5;
            player.GetModPlayer<SkeletalCoveringPlayer>().hasSkeletalCovering = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SkeletronMask)
                .AddIngredient(ItemID.BoneGlove)
                .AddIngredient(ItemID.SkeletronHand)
                .AddIngredient(ItemID.BookofSkulls)
                .AddIngredient(ItemID.WaterCandle)
                .AddIngredient(ItemID.SkeletronTrophy)
                .AddIngredient(ItemID.Bone, 50)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}
