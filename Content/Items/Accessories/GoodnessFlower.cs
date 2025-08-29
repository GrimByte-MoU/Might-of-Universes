using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Common;

namespace MightofUniverses.Content.Items.Accessories
{
    public class GoodnessFlower : ModItem
    {
   public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.sellPrice(gold: 2);
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var accessoryPlayer = player.GetModPlayer<AccessoryPlayer>();

            accessoryPlayer.accessoryModeActive = true;

            // Configure tier-specific bonuses
            accessoryPlayer.maxStacks = 6;
            accessoryPlayer.framesPerStack = 600;
            accessoryPlayer.idleDelay = 300;
            accessoryPlayer.defensePerStack = 1;
            accessoryPlayer.lifeRegenPerStack = 1;

            player.GetDamage(DamageClass.Generic) *= 0.85f;
            player.GetModPlayer<PacifistPlayer>().pacifistDamageMultiplier += 0.25f;

        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Sunflower, 3)
                .AddIngredient(ItemID.HoneyBlock, 50)
                .AddIngredient(ModContent.ItemType<GummyMembrane>(), 5)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
