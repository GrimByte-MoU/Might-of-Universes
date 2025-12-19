using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Accessories
{
    // Remove the AutoloadEquip attribute to prevent wing sprites from appearing
    public class StellarTadpole : ModItem
    {
        public override void SetStaticDefaults()
        {
            ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(450, 9f, 2.5f);
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ItemRarityID.Purple;
            Item.accessory = true;
            
            Item.wingSlot = 2;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // Get the mod player to handle flight mechanics
            var modPlayer = player.GetModPlayer<TadpolePlayer>();
            modPlayer.hasStellarTadpole = true;

            // Apply stat bonuses
            player.statLifeMax2 += 40;
            player.moveSpeed += 0.35f;
            player.GetDamage(DamageClass.Generic) += 0.12f;
            player.statDefense += 7;
            
            // Apply slow fall effect
            player.slowFall = true;
        }

        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
            ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            ascentWhenFalling = 0.85f;     // Falling glide speed
            ascentWhenRising = 0.25f;      // Rising speed
            maxCanAscendMultiplier = 1.2f;
            maxAscentMultiplier = 2.5f;
            constantAscend = 0.25f;
        }

        public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
        {
            speed = 9f;
            acceleration = 2.5f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<PsychicTadpole>(), 1)
                .AddIngredient(ItemID.LunarBar, 5)
                .AddTile(TileID.LunarCraftingStation)
                .SortBefore(Main.recipe.First(recipe => recipe.createItem.wingSlot != -1)) // Places this recipe before any wing
                .Register();
        }
    }
}

