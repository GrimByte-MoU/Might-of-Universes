using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Content.Rarities;
using MightofUniverses.Common;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Accessories
{
    public class ElementalSoulDetainer : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.accessory = true;
            Item.rare = ModContent.RarityType<TerraiumRarity>();
            Item.value = Item.sellPrice(platinum: 1);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage<ReaperDamageClass>() += 0.18f;
            
            var acc = player.GetModPlayer<ReaperAccessoryPlayer>();
            acc.accElementalSoulDetainer = true;
            acc.ApplyMaxSoulFromHP(0.20f);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<PrismaticSoulPrison>(), 1)
                .AddIngredient(ModContent.ItemType<TerraiumBar>(), 5)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}