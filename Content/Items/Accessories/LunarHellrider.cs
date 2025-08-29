using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Common.Players;
using MightofUniverses.Common;

namespace MightofUniverses.Content.Items.Accessories
{

public class LunarHellrider : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 20;
        Item.height = 20;
        Item.value = Item.sellPrice(platinum: 1);
        Item.rare = ItemRarityID.Purple;
        Item.DamageType = ModContent.GetInstance<PacifistDamageClass>();
        Item.accessory = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetDamage(DamageClass.Generic) += 0.12f;
        player.endurance += 0.12f;
          player.lifeRegen += 5;
            player.statLifeMax2 += 40;
        player.GetModPlayer<LunarHellriderPlayer>().hasLunarHellrider = true;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ModContent.ItemType<PrismaticGolem>())
            .AddIngredient(ModContent.ItemType<PrismaticFamiliar>())
            .AddIngredient(ItemID.LunarBar, 5)
            .AddTile(TileID.LunarCraftingStation)
            .Register();
    }
}
}
