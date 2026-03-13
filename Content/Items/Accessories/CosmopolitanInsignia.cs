using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Content.Rarities;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Accessories
{
    public class CosmopolitanInsignia : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.sellPrice(platinum: 3);
            Item.rare = ModContent.RarityType<TerraiumRarity>();
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statDefense += 10;
            player.lifeRegen += 6;
            player.statLifeMax2 += 50;
            player.statManaMax2 += 50;
            player.GetDamage(DamageClass.Generic) += 0.15f;
            player.endurance += 0.10f;
            player.moveSpeed += 0.35f;
            player.maxMinions += 1;

            if (player.wingsLogic > 0)
            {
                player.wingTimeMax = (int)(player.wingTimeMax * 1.35f);
            }

            player.GetModPlayer<PacifistPlayer>().pacifistDamageMultiplier += 0.50f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<WorldwalkersCharm>(), 1)
                .AddIngredient(ModContent.ItemType<TerraiumBar>(), 15)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}