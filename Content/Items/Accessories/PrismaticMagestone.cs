using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Accessories
{
    public class PrismaticMagestone : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ItemRarityID.Yellow;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            
            player.GetDamage(DamageClass.Magic) += 0.15f;
            player.GetKnockback(DamageClass.Magic) += 0.1f;
            player.GetCritChance(DamageClass.Magic) += 0.07f;
            player.manaCost -= 0.15f;
            player.GetModPlayer<PrismaticMagestonePlayer>().hasPrismaticMagestone = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SorcererEmblem)
                .AddIngredient(ModContent.ItemType<PrismaticCatalyst>(), 3)
                .AddIngredient(ModContent.ItemType<BrassBar>(), 8)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}