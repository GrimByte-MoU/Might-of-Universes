using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Common;

namespace MightofUniverses.Content.Items.Accessories
{
    public class StiffBrassFlower : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.sellPrice(gold: 2);
            Item.rare = ItemRarityID.LightPurple;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var accessoryPlayer = player.GetModPlayer<AccessoryPlayer>();

            accessoryPlayer.accessoryModeActive = true;

            accessoryPlayer.maxStacks = 6;
            accessoryPlayer.framesPerStack = 600;
            accessoryPlayer.idleDelay = 300;
            accessoryPlayer.defensePerStack = 3;
            accessoryPlayer.lifeRegenPerStack = 2;

            player.GetDamage(DamageClass.Generic) *= 0.5f;
            player.GetModPlayer<PacifistPlayer>().pacifistDamageMultiplier += 0.5f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<GoodnessFlower>(), 1)
                .AddIngredient(ModContent.ItemType<BrassBar>(), 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}

