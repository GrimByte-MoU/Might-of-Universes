using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Common;

namespace MightofUniverses.Content.Items.Accessories
{
    public class PrismaticLotus : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ItemRarityID.Yellow;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var accessoryPlayer = player.GetModPlayer<AccessoryPlayer>();

            accessoryPlayer.accessoryModeActive = true;

            accessoryPlayer.maxStacks = 6;
            accessoryPlayer.framesPerStack = 600;
            accessoryPlayer.idleDelay = 300;
            accessoryPlayer.defensePerStack = 4;
            accessoryPlayer.lifeRegenPerStack = 4;

            player.GetDamage(DamageClass.Generic) *= 0.3f;
            player.GetModPlayer<PacifistPlayer>().pacifistDamageMultiplier += 0.9f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<StiffBrassFlower>(), 1)
                .AddIngredient(ModContent.ItemType<PrismaticCatalyst>(), 3)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
