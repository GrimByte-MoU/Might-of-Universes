using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Common.Players;
using MightofUniverses.Common;

namespace MightofUniverses.Content.Items.Accessories
{
    public class LunaFlower : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.sellPrice(platinum: 1);
            Item.rare = ItemRarityID.Purple;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var accessoryPlayer = player.GetModPlayer<AccessoryPlayer>();

            accessoryPlayer.accessoryModeActive = true;

            accessoryPlayer.maxStacks = 6;
            accessoryPlayer.framesPerStack = 600;
            accessoryPlayer.idleDelay = 300;
            accessoryPlayer.defensePerStack = 5;
            accessoryPlayer.lifeRegenPerStack = 6;

            player.GetDamage(DamageClass.Generic) *= 0.3f;
            player.GetModPlayer<PacifistPlayer>().pacifistDamageMultiplier += 1.15f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<PrismaticLotus>(), 1)
                .AddIngredient(ItemID.LunarBar, 5)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}