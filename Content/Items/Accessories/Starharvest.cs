using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Content.Items.Accessories;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Accessories
{
    public class Starharvest : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ItemRarityID.Purple;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.maxMinions += 3;
            player.GetDamage(DamageClass.Melee) *= 0.5f;
            player.GetDamage(DamageClass.Ranged) *= 0.5f;
            player.GetDamage(DamageClass.Magic) *= 0.5f;
            player.GetDamage(DamageClass.Summon) *= 1.25f;
            
            // Add the lifesteal effect flag to the player
            player.GetModPlayer<StarharvestPlayer>().hasStarharvest = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.FragmentStardust, 10)
                .AddIngredient(ModContent.ItemType<PrismaticDelight>(), 1)
                .AddIngredient(ItemID.LunarBar, 10)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}
