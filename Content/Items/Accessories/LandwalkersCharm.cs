using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Accessories
{
    public class LandwalkersCharm : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.value = Item.sellPrice(gold: 4);
            Item.rare = ItemRarityID.LightRed;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statDefense += 6;
            player.lifeRegen += 2;
            player.statLifeMax2 += 30;
            player.statManaMax2 += 30;
            
            // All damage types +5%
            player.GetDamage(DamageClass.Generic) += 0.08f;
            player.GetModPlayer<PacifistPlayer>().pacifistDamageMultiplier += 0.30f;
            
            player.endurance += 0.08f; // Damage reduction
            player.moveSpeed += 0.20f;
            player.maxMinions += 1;
            
            // Movement improvements
            player.jumpSpeedBoost += 0.25f;
            player.jumpBoost = true;
            player.extraFall += 25;
        }

        public override void AddRecipes()
        {
           // Combination 1: Palladium + Mythril + Titanium
CreateRecipe()
    .AddIngredient(ModContent.ItemType<LandwalkerBauble>())
    .AddIngredient(ItemID.Pearlwood, 10)
    .AddIngredient(ItemID.TitaniumBar, 10)
    .AddIngredient(ItemID.MythrilBar, 10)
    .AddIngredient(ItemID.PalladiumBar, 10)
    .AddIngredient(ModContent.ItemType<SyntheticumBar>())
    .AddIngredient(ModContent.ItemType<UnderworldMass>())
    .AddTile(TileID.DemonAltar)
    .Register();

// Combination 2: Palladium + Mythril + Adamantite
CreateRecipe()
    .AddIngredient(ModContent.ItemType<LandwalkerBauble>())
    .AddIngredient(ItemID.Pearlwood, 10)
    .AddIngredient(ItemID.AdamantiteBar, 10)
    .AddIngredient(ItemID.MythrilBar, 10)
    .AddIngredient(ItemID.PalladiumBar, 10)
    .AddIngredient(ModContent.ItemType<SyntheticumBar>())
    .AddIngredient(ModContent.ItemType<UnderworldMass>())
    .AddTile(TileID.DemonAltar)
    .Register();

// Combination 3: Palladium + Orichalcum + Titanium
CreateRecipe()
    .AddIngredient(ModContent.ItemType<LandwalkerBauble>())
    .AddIngredient(ItemID.Pearlwood, 10)
    .AddIngredient(ItemID.TitaniumBar, 10)
    .AddIngredient(ItemID.OrichalcumBar, 10)
    .AddIngredient(ItemID.PalladiumBar, 10)
    .AddIngredient(ModContent.ItemType<SyntheticumBar>())
    .AddIngredient(ModContent.ItemType<UnderworldMass>())
    .AddTile(TileID.DemonAltar)
    .Register();

// Combination 4: Palladium + Orichalcum + Adamantite
CreateRecipe()
    .AddIngredient(ModContent.ItemType<LandwalkerBauble>())
    .AddIngredient(ItemID.Pearlwood, 10)
    .AddIngredient(ItemID.AdamantiteBar, 10)
    .AddIngredient(ItemID.OrichalcumBar, 10)
    .AddIngredient(ItemID.PalladiumBar, 10)
    .AddIngredient(ModContent.ItemType<SyntheticumBar>())
    .AddIngredient(ModContent.ItemType<UnderworldMass>())
    .AddTile(TileID.DemonAltar)
    .Register();

// Combination 5: Cobalt + Mythril + Titanium
CreateRecipe()
    .AddIngredient(ModContent.ItemType<LandwalkerBauble>())
    .AddIngredient(ItemID.Pearlwood, 10)
    .AddIngredient(ItemID.TitaniumBar, 10)
    .AddIngredient(ItemID.MythrilBar, 10)
    .AddIngredient(ItemID.CobaltBar, 10)
    .AddIngredient(ModContent.ItemType<SyntheticumBar>())
    .AddIngredient(ModContent.ItemType<UnderworldMass>())
    .AddTile(TileID.DemonAltar)
    .Register();

// Combination 6: Cobalt + Mythril + Adamantite
CreateRecipe()
    .AddIngredient(ModContent.ItemType<LandwalkerBauble>())
    .AddIngredient(ItemID.Pearlwood, 10)
    .AddIngredient(ItemID.AdamantiteBar, 10)
    .AddIngredient(ItemID.MythrilBar, 10)
    .AddIngredient(ItemID.CobaltBar, 10)
    .AddIngredient(ModContent.ItemType<SyntheticumBar>())
    .AddIngredient(ModContent.ItemType<UnderworldMass>())
    .AddTile(TileID.DemonAltar)
    .Register();

// Combination 7: Cobalt + Orichalcum + Titanium
CreateRecipe()
    .AddIngredient(ModContent.ItemType<LandwalkerBauble>())
    .AddIngredient(ItemID.Pearlwood, 10)
    .AddIngredient(ItemID.TitaniumBar, 10)
    .AddIngredient(ItemID.OrichalcumBar, 10)
    .AddIngredient(ItemID.CobaltBar, 10)
    .AddIngredient(ModContent.ItemType<SyntheticumBar>())
    .AddIngredient(ModContent.ItemType<UnderworldMass>())
    .AddTile(TileID.DemonAltar)
    .Register();

// Combination 8: Cobalt + Orichalcum + Adamantite
CreateRecipe()
    .AddIngredient(ModContent.ItemType<LandwalkerBauble>())
    .AddIngredient(ItemID.Pearlwood, 10)
    .AddIngredient(ItemID.AdamantiteBar, 10)
    .AddIngredient(ItemID.OrichalcumBar, 10)
    .AddIngredient(ItemID.CobaltBar, 10)
    .AddIngredient(ModContent.ItemType<SyntheticumBar>())
    .AddIngredient(ModContent.ItemType<UnderworldMass>())
    .AddTile(TileID.DemonAltar)
    .Register();
        }
    }
}